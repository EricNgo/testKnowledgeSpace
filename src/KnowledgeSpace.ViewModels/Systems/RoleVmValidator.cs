﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeSpace.ViewModels.Systems
{
    public class RoleVmValidator : AbstractValidator<RoleVm>
    {
        public RoleVmValidator()
        {
            //validation for each field such as name, id...
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id value is required")
                .MaximumLength(50).WithMessage("Role id cannot over limit 50 characters");


            RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required");
        }
    }
}