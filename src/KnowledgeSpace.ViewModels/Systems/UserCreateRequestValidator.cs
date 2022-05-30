using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.ViewModels.Systems
{
   public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
               .MinimumLength(6).WithMessage("Password has to atleast 6 characters")
               .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
               .WithMessage("Password is not match complexity rules.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").Matches(@"/^[^\s@]+@[^\s@]+\.[^\s@]+$/").WithMessage("Email format is not match");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required").MaximumLength(50).WithMessage("FirstName cannot over 50 chara");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").MaximumLength(50).WithMessage("Lastname cannot over 50 chara");
        }

      
    }
}
