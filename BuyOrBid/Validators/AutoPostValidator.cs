using BuyOrBid.Models.Database;
using FluentValidation;

namespace BuyOrBid
{
    public class AutoPostValidator : AbstractValidator<AutoPost>
    {
        public AutoPostValidator()
        {
            RuleFor(x => x.AutoCondition).NotNull();
            RuleFor(x => x.AutoType).NotNull();
            RuleFor(x => x.Color).NotNull();
            RuleFor(x => x.Cylinders).InclusiveBetween(0, 12);
            RuleFor(x => x.Doors).InclusiveBetween(0, 8);
            RuleFor(x => x.DriveType).NotNull();
            RuleFor(x => x.FuelType).NotNull();
            RuleFor(x => x.Language).NotNull();
            RuleFor(x => x.MakeId).NotNull();
            RuleFor(x => x.Mileage).InclusiveBetween(0, 1000000);
            RuleFor(x => x.ModelId).NotNull();
            RuleFor(x => x.PostId).Null();
            RuleFor(x => x.SystemTitle).Null();
            RuleFor(x => x.TitleStatus).NotNull();
            RuleFor(x => x.TransmissionType).NotNull();
            RuleFor(x => x.Year).InclusiveBetween(1900, 3000);
            RuleFor(x => x.Language).NotNull();
            RuleFor(x => x.IsPublic).NotNull();
            RuleFor(x => x.UserTitle).NotNull();
        }
    }
}
