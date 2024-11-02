using BlogWebsite.DTO.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlogWebsite.DTO.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
