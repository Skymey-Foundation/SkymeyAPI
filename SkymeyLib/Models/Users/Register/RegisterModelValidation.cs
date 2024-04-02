using FluentValidation;
using SharpCompress.Archives;
using SkymeyLib.Models.Users.Register;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Users.Register
{
    public class RegisterModelValidation : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidation()
        {
            
                RuleFor(p => p.Email).NotEmpty().WithMessage("You must enter a email address");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter valid an email");
            RuleFor(_ => _.Password).NotEmpty().WithMessage("Your password cannot be empty");
            RuleFor(_ => _.Password).MinimumLength(6).WithMessage("Your password length must be at least 6.");
            RuleFor(_ => _.Password).MaximumLength(16).WithMessage("Your password length must not exceed 16.");
            RuleFor(_ => _.Password).Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.");
            RuleFor(_ => _.Password).Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.");
            RuleFor(_ => _.Password).Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
            RuleFor(_ => _.Password).Matches(@"[\@\!\?\*\^\&\%\.]+").WithMessage("Your password must contain at least one (@!?&^% *.).");
        }
    }
}
