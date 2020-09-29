using BuyOrBid.Models.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models
{
    public class AutoFilterRequest
    {
        public string? Vin { get; set; }
        public IEnumerable<int>? Makes { get; set; }
        public IEnumerable<int>? Models { get; set; }
        [Range(0, 100000)]
        public int? MileageFrom { get; set; }
        [Range(0, 100000)]
        public int? MileageTo { get; set; }
        public string? Series { get; set; }
        public IEnumerable<AutoType>? AutoTypes { get; set; }
        public IEnumerable<TransmissionType>? TransmissionTypes { get; set; }
        public IEnumerable<TitleStatus>? TitleStatuses { get; set; }
        public string? Color { get; set; }
        public IEnumerable<FuelType>? FuelTypes { get; set; }
        public IEnumerable<DriveType>? DriveTypes { get; set; }
        [Range(1900, 3000)]
        public int? YearFrom { get; set; }
        [Range(1900, 3000)]
        public int? YearTo { get; set; }
        public string? Trim { get; set; }
        [Range(0, 12)]
        public IEnumerable<int>? CylinderCount { get; set; }
        public IEnumerable<AutoCondition>? AutoConditions { get; set; }
        public DateTime? ModifiedDateFrom { get; set; }
        public DateTime? ModifiedDateTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public bool? HasImages { get; set; }
    }
}
