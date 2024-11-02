using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Areas.Admin.Controllers
{
    public class AdminMainController : Controller
    {
        
        public IActionResult Index()
        {
            var user = User.Identity.Name;
            return View();
        }
    }
}
