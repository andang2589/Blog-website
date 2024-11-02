using BlogWebsite.DTO.Blog;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Validation
{
    public class BlogPostDtoValidator : AbstractValidator<BlogPostDTO>
    {
        public BlogPostDtoValidator() 
        {
            RuleFor(x=>x.Title).NotEmpty().WithMessage("Title cannot be empty")
                .NotNull().WithMessage("Title cannot be null")
                .MinimumLength(2).WithMessage("Title must atleast 3 characters");
            //RuleFor(x=>x.CreateDate).NotEmpty().WithMessage("Title cannot be empty")
            //    .NotNull().WithMessage("Title cannot be null");

            //RuleFor(x => x.ThumbnailUrl).NotNull().WithMessage("Please select the thumbnail");

            RuleFor(x => x.Description).NotNull().WithMessage("Please check the Description")
                .MinimumLength(15).WithMessage("Description must longer than 20 character");
        }
    }
}
