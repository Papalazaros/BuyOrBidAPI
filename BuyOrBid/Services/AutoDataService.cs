using BuyOrBid.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BuyOrBid.Services
{
    public class VehicleApiResponse<T>
    {
        public int? Count { get; set; }

        public string? Message { get; set; }

        public IEnumerable<T> Results { get; set; } = Enumerable.Empty<T>();
    }

    public class MakeResult
    {
        [JsonPropertyName("Make_ID")]
        public int? MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string? MakeName { get; set; }
    }

    public class ModelResult
    {
        [JsonPropertyName("Make_ID")]
        public int? MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string? MakeName { get; set; }

        [JsonPropertyName("Model_ID")]
        public int? ModelId { get; set; }

        [JsonPropertyName("Model_Name")]
        public string? ModelName { get; set; }
    }

    public interface IAutoDataService
    {
        Task<IEnumerable<Make>> GetMakes();
        Task<IEnumerable<Model>> GetModels(Make make);
    }

    public class AutoDataService : IAutoDataService
    {
        private readonly HttpClient _httpClient;

        public AutoDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Make>> GetMakes()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=json");
            VehicleApiResponse<MakeResult>? makeResult = await httpResponseMessage.ConvertResponseToObject<VehicleApiResponse<MakeResult>>();
            return makeResult?.Results.Select(x => new Make
            {
                VpicId = x.MakeId,
                MakeName = x.MakeName?.ToUpper()
            }).Where(x => !string.IsNullOrEmpty(x.MakeName) && x.VpicId.HasValue).GroupBy(x => x.MakeName, x => x).Select(x => x.FirstOrDefault()) ?? Enumerable.Empty<Make>();
        }

        public async Task<IEnumerable<Model>> GetModels(Make make)
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMakeId/{make.VpicId}?format=json");
            VehicleApiResponse<ModelResult>? modelResult = await httpResponseMessage.ConvertResponseToObject<VehicleApiResponse<ModelResult>>();
            return modelResult?.Results.Select(x => new Model
            {
                Make = make,
                MakeId = make.MakeId,
                ModelName = x.ModelName?.ToUpper(),
                VpicId = x.ModelId
            }).Where(x => !string.IsNullOrEmpty(x.ModelName) && x.VpicId.HasValue).GroupBy(x => x.ModelName, x => x).Select(x => x.FirstOrDefault()) ?? Enumerable.Empty<Model>();
        }
    }
}
