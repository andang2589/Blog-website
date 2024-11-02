using BlogWebsite.DTO.Category;
using BlogWebsite.Service.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.CreateCategory(dto);
            return Ok(result);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            var result = await _categoryService.GetListCategories();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return Ok(result);
        }
    }
}
