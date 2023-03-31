using FluentValidation;
using MarketBackend;

namespace BackendHeyTaxi.Validators
{
    public class LocationValidate:AbstractValidator<locations>
    {
        public LocationValidate() {
            RuleFor(x => x.altitude)
                .NotEmpty()
                .WithMessage("altitude alanu boş olamaz.");
            RuleFor(x => x.longitude)
               .NotEmpty()
               .WithMessage("longitude alanu boş olamaz.");
            RuleFor(x => x.latitude)
             .NotEmpty()
             .WithMessage("latitude alanu boş olamaz.");
        }
    }
}
