using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.Areas.Admin.Helper;
using BlogWebsite.DTO.Category;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlogWebsite.Areas.Admin.Controllers
{
    public class AdminCategoryController : Controller
    {

        private readonly ICategoryApiClient _categoryApiClient;

        public AdminCategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var cates = await _categoryApiClient.GetListCategories();

            var authorizationCheck = AuthorizationHelper.HandleAuthorization(cates, this);
            if (authorizationCheck != null)
            {
                return authorizationCheck;
            }

            return View(cates.Data);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var result = await _categoryApiClient.CreateCategory(dto);

            var authorizationCheck = AuthorizationHelper.HandleAuthorization(result, this);
            if (authorizationCheck != null)
            {
                return authorizationCheck;
            }

            return View();
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            if(id==0)
            {
                return View();
            }
            await _categoryApiClient.DeleteCategory(id);
            return NoContent();
        }


    }
}
