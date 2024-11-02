using BlogWebsite.DTO.Category;
using BlogWebsite.DTO.Common;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public interface ICategoryApiClient
    {
        Task<ClassResult<CategoryDto>> CreateCategory(CategoryDto dto);
        Task<ClassResult<List<CategoryDto>>> GetListCategories();
        Task<ClassResult<CategoryDto>> DeleteCategory(int id);
    }
}
