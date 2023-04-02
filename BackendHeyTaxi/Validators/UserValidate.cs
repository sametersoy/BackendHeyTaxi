using FluentValidation;
using MarketBackend;

namespace BackendHeyTaxi.Validators
{
    public class UserValidate:AbstractValidator<users>
    {
        public UserValidate() {
            RuleFor(x => x.email)
                .NotEmpty()
                .WithMessage("email adresi formatı yanlış.")
                .EmailAddress()
                .WithMessage("mail adresi olmalı");

            RuleFor(x => x.password)
               .NotEmpty()
               .WithMessage("password alanu boş olamaz.")
               .MinimumLength(6)
               .WithMessage("Parola alanı en az 6 karakter olmalı");
               
        }
    }
}
