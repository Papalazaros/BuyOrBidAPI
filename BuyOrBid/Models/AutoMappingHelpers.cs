using BuyOrBid.Models.Database.Enums;

namespace BuyOrBid.Models
{
    public static class AutoMappingHelpers
    {
        public static FuelType? GetFuelType(string? primaryFuelType, string? secondaryFuelType)
        {
            if (!string.IsNullOrEmpty(primaryFuelType) && !string.IsNullOrEmpty(secondaryFuelType))
            {
                return FuelType.Hybrid;
            }

            if (!string.IsNullOrEmpty(primaryFuelType) && AutoMapping.fuelTypeMap.TryGetValue(primaryFuelType, out FuelType primaryFuelTypeEnum))
            {
                return primaryFuelTypeEnum;
            }
            else if (!string.IsNullOrEmpty(secondaryFuelType) && AutoMapping.fuelTypeMap.TryGetValue(secondaryFuelType, out FuelType secondaryFuelTypeEnum))
            {
                return secondaryFuelTypeEnum;
            }

            return null;
        }

        public static TransmissionType? GetTransmissionType(string? transmissionStyle)
        {
            if (!string.IsNullOrEmpty(transmissionStyle) && AutoMapping.transmissionStyleMap.TryGetValue(transmissionStyle, out TransmissionType transmissionType))
            {
                return transmissionType;
            }

            return null;
        }

        public static DriveType? GetDriveType(string? driveType)
        {
            if (!string.IsNullOrEmpty(driveType) && AutoMapping.driveTypeMap.TryGetValue(driveType, out DriveType driveTypeEnum))
            {
                return driveTypeEnum;
            }

            return null;
        }

        public static AutoType? GetAutoType(string? autoType, string? bodyClass)
        {
            if (!string.IsNullOrEmpty(bodyClass) && AutoMapping.bodyClassToAutoTypeMap.TryGetValue(bodyClass, out AutoType autoTypeFromBodyClass))
            {
                return autoTypeFromBodyClass;
            }

            if (!string.IsNullOrEmpty(autoType) && AutoMapping.autoTypeMap.TryGetValue(autoType, out AutoType autoTypeEnum))
            {
                return autoTypeEnum;
            }

            return null;
        }
    }
}
