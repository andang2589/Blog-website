using BlogWebsite.DTO.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Validation
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator() 
        {
            RuleFor(x=>x.CategoryName).MinimumLength(4).WithMessage("Category Name must longer than 4");
        }
    }
}
