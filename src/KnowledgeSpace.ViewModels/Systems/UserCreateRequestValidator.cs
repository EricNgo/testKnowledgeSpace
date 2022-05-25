using FluentValidation;
using KnowledgeSpace.ViewModels.System;
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

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").Matches(@"/^[^\s@]+@[^\s@]+\.[^\s@]+$/").WithMessage("Email format is not match");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required").MaximumLength(50).WithMessage("FirstName cannot over 50 chara");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").MaximumLength(50).WithMessage("Lastname cannot over 50 chara");
        }

      
    }
}
