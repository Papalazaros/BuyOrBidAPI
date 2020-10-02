using BuyOrBid.Extensions;
using BuyOrBid.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuyOrBid.Services
{
    public interface IVinDecodeService
    {
        Task<IEnumerable<VinDecodeResult>> DecodeVin(string vin, int? modelYear = null);

        bool IsValid(string vin);

        Task<Dictionary<string, IEnumerable<VinDecodeResult>>> GetAdditionalVinInformation(string vin, string? possibleVinConfigurations);
    }

    public class VinDecodeService : IVinDecodeService
    {
        private readonly HttpClient _httpClient;

        private static readonly Regex validationRegex = new Regex("[A-HJ-NPR-Z0-9]{17}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Dictionary<char, int> vinDecodeMap = new Dictionary<char, int>
        {
            {'A', 1 },
            {'B', 2 },
            {'C', 3 },
            {'D', 4 },
            {'E', 5 },
            {'F', 6 },
            {'G', 7 },
            {'H', 8 },
            {'J', 1 },
            {'K', 2 },
            {'L', 3 },
            {'M', 4 },
            {'N', 5 },
            {'P', 7 },
            {'R', 9 },
            {'S', 2 },
            {'T', 3 },
            {'U', 4 },
            {'V', 5 },
            {'W', 6 },
            {'X', 7 },
            {'Y', 8 },
            {'Z', 9 },
            {'a', 1 },
            {'b', 2 },
            {'c', 3 },
            {'d', 4 },
            {'e', 5 },
            {'f', 6 },
            {'g', 7 },
            {'h', 8 },
            {'j', 1 },
            {'k', 2 },
            {'l', 3 },
            {'m', 4 },
            {'n', 5 },
            {'p', 7 },
            {'r', 9 },
            {'s', 2 },
            {'t', 3 },
            {'u', 4 },
            {'v', 5 },
            {'w', 6 },
            {'x', 7 },
            {'y', 8 },
            {'z', 9 },
            {'1', 1 },
            {'2', 2 },
            {'3', 3 },
            {'4', 4 },
            {'5', 5 },
            {'6', 6 },
            {'7', 7 },
            {'8', 8 },
            {'9', 9 },
            {'0', 0 },
        };

        private static readonly int[] vinWeightMap = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };

        public VinDecodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, IEnumerable<VinDecodeResult>>> GetAdditionalVinInformation(string vin, string? possibleVinConfigurations)
        {
            string[] additionalVins = GenerateAdditionalVins(vin, possibleVinConfigurations).ToArray();

            if (additionalVins.Length == 0)
            {
                return new Dictionary<string, IEnumerable<VinDecodeResult>>();
            }

            Task<IEnumerable<VinDecodeResult>>[] additionalVinDecodeRequests = additionalVins.Select(x => DecodeVin(x)).ToArray();

            await Task.WhenAll(additionalVinDecodeRequests);

            Dictionary<string, IEnumerable<VinDecodeResult>> additionalVinInformation = new Dictionary<string, IEnumerable<VinDecodeResult>>(additionalVins.Length);

            int index = 0;

            foreach (string additionalVin in additionalVins)
            {
                additionalVinInformation.Add(additionalVin, additionalVinDecodeRequests[index].Result);
                index++;
            }

            return additionalVinInformation;
        }

        private IEnumerable<string> GenerateAdditionalVins(string vin, string? possibleVins)
        {
            if (string.IsNullOrEmpty(possibleVins))
            {
                return Enumerable.Empty<string>();
            }

            Regex regex = new Regex(@"\(([0-9]):([0-9A-Z]{1,})\)");

            List<(int position, string replacementCharacters)> vinConfigurationGroups = new List<(int position, string replacementCharacters)>();

            foreach (Match match in regex.Matches(possibleVins) ?? Enumerable.Empty<Match>())
            {
                vinConfigurationGroups.Add((int.Parse(match!.Groups[1].Value), match.Groups[2].Value));
            }

            List<string> availableVins = new List<string>()
            {
                vin
            };

            foreach ((int position, string replacementCharacters) in vinConfigurationGroups)
            {
                List<string> generatedVins = new List<string>(availableVins.Count * replacementCharacters.Length);

                foreach (string availableVin in availableVins)
                {
                    foreach (char possibleCharacter in replacementCharacters)
                    {
                        StringBuilder strBuilder = new StringBuilder(availableVin);
                        strBuilder[position - 1] = possibleCharacter;
                        string possibleVin = strBuilder.ToString();

                        if (!IsValid(possibleVin) || vin == possibleVin)
                        {
                            continue;
                        }

                        generatedVins.Add(possibleVin);
                    }
                }

                availableVins.AddRange(generatedVins);
            }

            availableVins.RemoveAt(0);

            return availableVins;
        }

        public bool IsValid(string vin)
        {
            if (string.IsNullOrEmpty(vin) || !validationRegex.IsMatch(vin))
            {
                return false;
            }

            int sum = 0;

            for (int i = 0; i < vin.Length; i++)
            {
                if (!vinDecodeMap.TryGetValue(vin[i], out int replacementValue))
                {
                    return false;
                }

                sum += vinWeightMap[i] * replacementValue;
            }

            char checkValue = vin[8];
            int intCheckValue = checkValue == 'X' ? 10 : checkValue - '0';

            return (sum % 11) == intCheckValue;
        }

        public async Task<IEnumerable<VinDecodeResult>> DecodeVin(string vin, int? modelYear = null)
        {
            string url = modelYear.HasValue
                ? $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json&modelyear={modelYear}"
                : $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json";

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
            VehicleApiResponse<VinDecodeResult>? vinDecodeResult = await httpResponseMessage.ConvertResponseToObject<VehicleApiResponse<VinDecodeResult>>();
            return vinDecodeResult?.Results ?? Enumerable.Empty<VinDecodeResult>();
        }
    }
}