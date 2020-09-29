using BuyOrBid.Models.Database.Enums;
using System.Collections.Generic;

namespace BuyOrBid.Models
{
    public static class AutoMapping
    {
        public static readonly Dictionary<string, FuelType> fuelTypeMap = new Dictionary<string, FuelType>
        {
            { "Diesel", FuelType.Diesel },
            { "Electric", FuelType.Electric },
            { "Gasoline", FuelType.Gasoline },
            { "Ethanol (E85)", FuelType.Gasoline },
            { "Flexible Fuel Vehicle (FFV)", FuelType.Gasoline },
            { "Natural Gas", FuelType.NaturalGas },
            { "Compressed Natural Gas (CNG)", FuelType.NaturalGas },
            { "Liquefied Natural Gas (LNG)", FuelType.NaturalGas },
            { "Liquefied Petroleum Gas (propane or LPG)", FuelType.NaturalGas },
            { "Compressed Hydrogen / Hydrogen", FuelType.Hydrogen }
        };

        public static readonly Dictionary<string, TransmissionType> transmissionStyleMap = new Dictionary<string, TransmissionType>
        {
            { "Automatic", TransmissionType.Automatic },
            { "Manual/Standard", TransmissionType.Manual },
            { "Electronic Continuously Variable (e-CVT)", TransmissionType.CVT },
            { "Continuously Variable Transmission (CVT)", TransmissionType.CVT },
            { "Automated Manual Transmission (AMT)", TransmissionType.AMT },
            { "Dual-clutch Transmission (DCT)", TransmissionType.DCT },
        };

        public static readonly Dictionary<string, DriveType> driveTypeMap = new Dictionary<string, DriveType>
        {
            { "FWD/Front Wheel Drive", DriveType.Front },
            { "4WD/4-Wheel Drive/4x4", DriveType.Four },
            { "AWD/All Wheel Drive", DriveType.All },
            { "RWD/ Rear Wheel Drive", DriveType.Rear },
            { "2WD/4WD", DriveType.Four }
        };

        public static readonly Dictionary<string, AutoType> bodyClassToAutoTypeMap = new Dictionary<string, AutoType>
        {
            {"Bus", AutoType.Bus },
            {"Bus - School Bus", AutoType.Bus },
            {"Cargo Van", AutoType.Van },
            {"Convertible/Cabriolet", AutoType.Convertible },
            {"Coupe", AutoType.Coupe },
            {"Crossover Utility Vehicle (CUV)", AutoType.CUV },
            {"Hatchback/Liftback/Notchback", AutoType.Hatchback },
            {"Minivan", AutoType.Van },
            {"Motorcycle - Competition", AutoType.Motorcycle },
            {"Motorcycle - Cross Country", AutoType.Motorcycle },
            {"Motorcycle - Cruiser", AutoType.Motorcycle },
            {"Motorcycle - Custom", AutoType.Motorcycle },
            {"Motorcycle - Dual Sport / Adventure / Supermoto / On/Off-road", AutoType.Motorcycle },
            {"Motorcycle - Enclosed Three Wheeled / Enclosed Autocycle", AutoType.Motorcycle },
            {"Motorcycle - Moped", AutoType.Motorcycle },
            {"Motorcycle - Scooter", AutoType.Motorcycle },
            {"Motorcycle - Side Car", AutoType.Motorcycle },
            {"Motorcycle - Small / Minibike", AutoType.Motorcycle },
            {"Motorcycle - Sport", AutoType.Motorcycle },
            {"Motorcycle - Standard", AutoType.Motorcycle },
            {"Motorcycle - Street", AutoType.Motorcycle },
            {"Motorcycle - Touring / Sport Touring", AutoType.Motorcycle },
            {"Motorcycle - Trike", AutoType.Motorcycle },
            {"Motorcycle - Underbone", AutoType.Motorcycle },
            {"Motorcycle - Unenclosed Three Wheeled / Open Autocycle", AutoType.Motorcycle },
            {"Motorcycle - Unknown Body Class", AutoType.Motorcycle },
            {"Pickup", AutoType.Truck },
            {"Roadster", AutoType.Roadster },
            {"Sedan/Saloon", AutoType.Sedan },
            {"Sport Utility Truck (SUT)", AutoType.Truck },
            {"Sport Utility Vehicle (SUV)/Multi-Purpose Vehicle (MPV)", AutoType.SUV },
            {"Step Van / Walk-in Van", AutoType.Van },
            {"Truck", AutoType.Truck },
            {"Van", AutoType.Van },
            {"Wagon", AutoType.Wagon },
        };

        public static readonly Dictionary<string, AutoType> autoTypeMap = new Dictionary<string, AutoType>
        {
            { "Motorcycle", AutoType.Motorcycle },
            { "Multipurpose Passenger Vehicle (MPV)", AutoType.SUV },
            { "Bus", AutoType.Bus },
            { "Truck ", AutoType.Truck }
        };
    }
}
