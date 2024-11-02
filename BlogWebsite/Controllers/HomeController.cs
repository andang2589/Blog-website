using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogApiClient _blogApiClient;

        public HomeController(ILogger<HomeController> logger, IBlogApiClient blogApiClient)
        {
            _logger = logger;
            _blogApiClient = blogApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _blogApiClient.GetPostsList();
            return View(list.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Temp() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}