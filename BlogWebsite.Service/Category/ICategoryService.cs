using BlogWebsite.DTO.Category;
using BlogWebsite.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = BlogWebsite.Data.Models;

namespace BlogWebsite.Service.Category
{
    public interface ICategoryService
    {
        Task<ClassResult<Model.Category>> CreateCategory(CategoryDto dto);
        Task<ClassResult<List<CategoryDto>>> GetListCategories();

        Task<ClassResult<CategoryDto>> DeleteCategory(int id);
    }
}
