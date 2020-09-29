using BuyOrBid.Models;
using BuyOrBid.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyOrBid.Services
{
    public interface IAutoPostService
    {
        Task<AutoPost> CreatePostFromVin(string vin, int? modelYear = null);
        Task<IEnumerable<AutoPost>> CreatePostsFromVin(string vin, int? modelYear = null);
        Task<IEnumerable<AutoPost>> Filter(AutoFilterRequest autoFilterRequest);
        Task<Model> GetModel(int modelId);
        Task<Make> GetMake(int makeId);
        Task<IEnumerable<Model>> GetModels();
        Task<IEnumerable<Make>> GetMakes();
    }

    public class AutoPostService : IAutoPostService
    {
        private readonly IVinDecodeService _vinDecodeService;
        private readonly MyDbContext _myDbContext;

        public AutoPostService(IVinDecodeService vinDecodeService, MyDbContext myDbContext)
        {
            _vinDecodeService = vinDecodeService;
            _myDbContext = myDbContext;
        }

        public async Task<AutoPost> CreatePostFromVin(string vin, int? modelYear = null)
        {
            IEnumerable<VinDecodeResult> vinDecodeResults = await _vinDecodeService.DecodeVin(vin, modelYear);
            return await ConvertToAutoPost(vin, vinDecodeResults);
        }

        public async Task<IEnumerable<AutoPost>> CreatePostsFromVin(string vin, int? modelYear = null)
        {
            IEnumerable<VinDecodeResult> vinDecodeResults = await _vinDecodeService.DecodeVin(vin, modelYear);

            Dictionary<int, VinDecodeResult> results = GetResultsDictionary(vinDecodeResults);

            AutoPost autoPost = await ConvertToAutoPost(vin, results);

            results.TryGetValue(143, out VinDecodeResult? errorCodeResult);
            results.TryGetValue(144, out VinDecodeResult? possibleVinConfigurationsResult);

            Dictionary<string, IEnumerable<VinDecodeResult>> additionalVinInformation = await _vinDecodeService.GetAdditionalVinInformation(vin, possibleVinConfigurationsResult?.Value);
            IEnumerable<Task<AutoPost>> additionalVinInformationTasks = additionalVinInformation.Select(x => ConvertToAutoPost(x.Key, x.Value));
            AutoPost[] otherPosts = await Task.WhenAll(additionalVinInformationTasks);

            IEnumerable<string> errors = Enumerable.Empty<string>();

            if (errorCodeResult != null && errorCodeResult.Value != "0" && results.TryGetValue(191, out VinDecodeResult? vinErrorTextResult))
            {
                errors = vinErrorTextResult.Value?.Split(';').Select(x => x.Trim()) ?? Enumerable.Empty<string>();
                if (results.TryGetValue(156, out VinDecodeResult? vinAdditionalErrorTextResult))
                {
                    errors = errors.Concat(vinAdditionalErrorTextResult.Value?.Split(';').Select(x => x.Trim()) ?? Enumerable.Empty<string>());
                }
            }

            return otherPosts;
        }

        public async Task<IEnumerable<AutoPost>> Filter(AutoFilterRequest autoFilterRequest)
        {
            IQueryable<AutoPost> filteredPosts = _myDbContext.AutoPosts.AsNoTracking().Where(x => x.IsPublic == true);

            if (!string.IsNullOrEmpty(autoFilterRequest.Vin))
            {
                filteredPosts = filteredPosts.Where(x => !string.IsNullOrEmpty(x.Vin) && x.Vin.Contains(autoFilterRequest.Vin.ToUpper()));
            }

            if (!string.IsNullOrEmpty(autoFilterRequest.Series))
            {
                filteredPosts = filteredPosts.Where(x => !string.IsNullOrEmpty(x.Series) && x.Series.Contains(autoFilterRequest.Series.ToUpper()));
            }

            if (!string.IsNullOrEmpty(autoFilterRequest.Trim))
            {
                filteredPosts = filteredPosts.Where(x => !string.IsNullOrEmpty(x.Trim) && x.Trim.Contains(autoFilterRequest.Trim.ToUpper()));
            }

            if (!string.IsNullOrEmpty(autoFilterRequest.Color))
            {
                filteredPosts = filteredPosts.Where(x => !string.IsNullOrEmpty(x.Color) && x.Color.Contains(autoFilterRequest.Color.ToUpper()));
            }

            if (autoFilterRequest.MileageFrom.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.Mileage.HasValue && x.Mileage >= autoFilterRequest.MileageFrom);
            }

            if (autoFilterRequest.MileageTo.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.Mileage.HasValue && x.Mileage <= autoFilterRequest.MileageTo);
            }

            if (autoFilterRequest.YearFrom.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.Year.HasValue && x.Year >= autoFilterRequest.YearFrom);
            }

            if (autoFilterRequest.YearTo.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.Year.HasValue && x.Year <= autoFilterRequest.YearTo);
            }

            if (autoFilterRequest.Makes != null && autoFilterRequest.Makes.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.MakeId.HasValue && autoFilterRequest.Makes.Contains(x.MakeId.Value));
            }

            if (autoFilterRequest.Models != null && autoFilterRequest.Models.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.ModelId.HasValue && autoFilterRequest.Models.Contains(x.ModelId.Value));
            }

            if (autoFilterRequest.CylinderCount != null && autoFilterRequest.CylinderCount.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.Cylinders.HasValue && autoFilterRequest.CylinderCount.Contains(x.Cylinders.Value));
            }

            if (autoFilterRequest.AutoTypes != null && autoFilterRequest.AutoTypes.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.AutoType.HasValue && autoFilterRequest.AutoTypes.Contains(x.AutoType.Value));
            }

            if (autoFilterRequest.TransmissionTypes != null && autoFilterRequest.TransmissionTypes.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.TransmissionType.HasValue && autoFilterRequest.TransmissionTypes.Contains(x.TransmissionType.Value));
            }

            if (autoFilterRequest.TitleStatuses != null && autoFilterRequest.TitleStatuses.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.TitleStatus.HasValue && autoFilterRequest.TitleStatuses.Contains(x.TitleStatus.Value));
            }

            if (autoFilterRequest.FuelTypes != null && autoFilterRequest.FuelTypes.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.FuelType.HasValue && autoFilterRequest.FuelTypes.Contains(x.FuelType.Value));
            }

            if (autoFilterRequest.AutoConditions != null && autoFilterRequest.AutoConditions.Any())
            {
                filteredPosts = filteredPosts.Where(x => x.AutoCondition.HasValue && autoFilterRequest.AutoConditions.Contains(x.AutoCondition.Value));
            }

            if (autoFilterRequest.ModifiedDateFrom.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.ModifiedDate.HasValue && x.ModifiedDate >= autoFilterRequest.ModifiedDateFrom);
            }

            if (autoFilterRequest.ModifiedDateTo.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.ModifiedDate.HasValue && x.ModifiedDate <= autoFilterRequest.ModifiedDateTo);
            }

            if (autoFilterRequest.CreatedDateFrom.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.CreatedDate.HasValue && x.CreatedDate >= autoFilterRequest.CreatedDateFrom);
            }

            if (autoFilterRequest.CreatedDateTo.HasValue)
            {
                filteredPosts = filteredPosts.Where(x => x.CreatedDate.HasValue && x.CreatedDate <= autoFilterRequest.CreatedDateTo);
            }

            return await filteredPosts.ToArrayAsync();
        }

        public async Task<Model> GetModel(int modelId)
        {
            return await _myDbContext.Models.FindAsync(modelId);
        }

        public async Task<Make> GetMake(int makeId)
        {
            return await _myDbContext.Makes.FindAsync(makeId);
        }

        public async Task<IEnumerable<Model>> GetModels()
        {
            return await _myDbContext.Models.ToArrayAsync();
        }

        public async Task<IEnumerable<Make>> GetMakes()
        {
            return await _myDbContext.Makes.ToArrayAsync();
        }

        public static string GenerateTitle(AutoPost autoPost)
        {
            return string.Join(' ', new List<string?>
            {
                autoPost.Year?.ToString(),
                autoPost.Make?.MakeName,
                autoPost.Model?.ModelName,
                autoPost.Series?.ToUpper(),
                autoPost.Trim?.ToUpper(),
                //autoPost.DisplacementInLiters > 0 && autoPost.AutoType != AutoType.Motorcycle ? $"{autoPost.DisplacementInLiters}L" : null,
                //autoPost.DisplacementInCc > 0 && autoPost.AutoType == AutoType.Motorcycle ? $"{autoPost.DisplacementInCc}CC" : null,
                //autoPost.Cylinders > 0 ? $"{autoPost.Cylinders}CYL" : null,
                //autoPost.Doors > 0 ? $"{autoPost.Doors}DR" : null
            }.Where(x => !string.IsNullOrEmpty(x)));
        }

        private async Task<AutoPost> ConvertToAutoPost(string vin, IEnumerable<VinDecodeResult> vinDecodeResults)
        {
            Dictionary<int, VinDecodeResult> results = GetResultsDictionary(vinDecodeResults);
            return await ConvertToAutoPost(vin, results);
        }

        private async Task<AutoPost> ConvertToAutoPost(string vin, Dictionary<int, VinDecodeResult> results)
        {
            results.TryGetValue(9, out VinDecodeResult? cylinderResult);
            results.TryGetValue(14, out VinDecodeResult? doorsResult);
            results.TryGetValue(24, out VinDecodeResult? primaryFuelTypeResult);
            results.TryGetValue(66, out VinDecodeResult? secondaryFuelTypeResult);
            results.TryGetValue(29, out VinDecodeResult? modelYearResult);
            results.TryGetValue(34, out VinDecodeResult? seriesResult);
            results.TryGetValue(38, out VinDecodeResult? trimResult);
            results.TryGetValue(18, out VinDecodeResult? engineModelResult);
            results.TryGetValue(15, out VinDecodeResult? driveTypeResult);
            results.TryGetValue(37, out VinDecodeResult? transmissionStyleResult);
            results.TryGetValue(5, out VinDecodeResult? bodyClassResult);
            results.TryGetValue(39, out VinDecodeResult? vehicleTypeResult);
            results.TryGetValue(13, out VinDecodeResult? displacementInLitersResult);
            results.TryGetValue(11, out VinDecodeResult? displacementInCcResult);
            results.TryGetValue(71, out VinDecodeResult? horsePowerResult);

            string? trim = trimResult?.Value?.ToUpper();
            string? series = seriesResult?.Value?.ToUpper();
            string? engineModel = engineModelResult?.Value;
            int.TryParse(cylinderResult?.Value, out int cylinders);
            int.TryParse(modelYearResult?.Value, out int modelYearFromVin);
            int.TryParse(doorsResult?.Value, out int doors);
            double.TryParse(displacementInLitersResult?.Value, out double displacementInLiters);
            double.TryParse(displacementInCcResult?.Value, out double displacementInCc);
            double.TryParse(horsePowerResult?.Value, out double horsepower);

            displacementInLiters = Math.Round(displacementInLiters, 1);
            displacementInCc = Math.Round(displacementInCc, 1);
            horsepower = Math.Round(horsepower, 1);

            Make? make = null;
            Model? model = null;

            if (results.TryGetValue(28, out VinDecodeResult? modelResult) && int.TryParse(modelResult.ValueId, out int modelId))
            {
                model = await _myDbContext.Models.Include(x => x.Make).FirstOrDefaultAsync(x => x.VpicId == modelId);
                make = model?.Make;
            }
            else if (results.TryGetValue(26, out VinDecodeResult? makeResult) && int.TryParse(makeResult.ValueId, out int makeId))
            {
                make = await _myDbContext.Makes.FirstOrDefaultAsync(x => x.VpicId == makeId);
            }

            AutoPost autoPost = new AutoPost
            {
                Doors = doors,
                DriveType = AutoMappingHelpers.GetDriveType(driveTypeResult?.Value),
                AutoType = AutoMappingHelpers.GetAutoType(vehicleTypeResult?.Value, bodyClassResult?.Value),
                Cylinders = cylinders,
                EngineModel = engineModel,
                FuelType = AutoMappingHelpers.GetFuelType(primaryFuelTypeResult?.Value, secondaryFuelTypeResult?.Value),
                Trim = trim,
                Make = make,
                MakeId = make?.MakeId,
                Model = model,
                ModelId = model?.ModelId,
                Series = series,
                Vin = vin.ToUpper(),
                Year = modelYearFromVin,
                TransmissionType = AutoMappingHelpers.GetTransmissionType(transmissionStyleResult?.Value),
                Horsepower = horsepower,
                DisplacementInLiters = displacementInLiters,
                DisplacementInCc = displacementInCc
            };

            autoPost.SystemTitle = GenerateTitle(autoPost);

            return autoPost;
        }

        private static Dictionary<int, VinDecodeResult> GetResultsDictionary(IEnumerable<VinDecodeResult> vinDecodeResults)
        {
            return vinDecodeResults
                .Where(x => !string.IsNullOrEmpty(x.Value) && x.VariableId.HasValue && x.Value != "Not Applicable")
                .GroupBy(x => x.VariableId!.Value)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());
        }
    }
}
