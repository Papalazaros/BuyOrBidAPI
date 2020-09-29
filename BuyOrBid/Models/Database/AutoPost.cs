using BuyOrBid.Models.Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models.Database
{
    public class AutoPost : Post
    {
        public string? Vin { get; set; }

        public Make? Make { get; set; }

        [Required]
        public int? MakeId { get; set; }

        public Model? Model { get; set; }

        [Required]
        public int? ModelId { get; set; }

        [Required]
        public int? Mileage { get; set; }

        public string? Series { get; set; }

        [Required]
        public AutoType? AutoType { get; set; }

        [Required]
        public TransmissionType? TransmissionType { get; set; }

        [Required]
        public TitleStatus? TitleStatus { get; set; }

        [Required]
        public string? Color { get; set; }

        [Required]
        public int? Doors { get; set; }

        public double? Horsepower { get; set; }

        public double? DisplacementInLiters { get; set; }

        public double? DisplacementInCc { get; set; }

        [Required]
        public FuelType? FuelType { get; set; }

        [Required]
        public DriveType? DriveType { get; set; }

        [Required]
        public int? Year { get; set; }

        public string? Trim { get; set; }

        public int? Cylinders { get; set; }

        public string? EngineModel { get; set; }

        [Required]
        public AutoCondition? AutoCondition { get; set; }
    }
}