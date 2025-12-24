using FluentValidation;
using ShopDemo.Application.DTOs.RoleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Validation.RoleValidation
{
    public class RoleCreateDtoValidator : AbstractValidator<RoleCreatedDto>
    {
        public RoleCreateDtoValidator() 
        { 
            RuleFor(x =>  x.Name)
                .NotEmpty().WithMessage("Name is required");
        }
    }
}
