using BlogWebsite.DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Areas.Admin.Helper
{
    public static class AuthorizationHelper
    {
        public static IActionResult HandleAuthorization<T>(ClassResult<T> result, Controller controller) where T : class
        {
            if(!result.IsSuccess)
            {
                string returnUrl = controller.HttpContext.Request.Path + controller.HttpContext.Request.QueryString;

                if (result.Message == "Unauthorized")
                {
                    return controller.RedirectToAction("Login", "AdminLogin", new { returnUrl = returnUrl });
                }
                if (result.Message == "Forbidden")
                {
                    return controller.RedirectToAction("AccessDenied", "Home", new { area = "" });
                }
            }

            return null;
        }
    }
}
