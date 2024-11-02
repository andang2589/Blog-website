using BlogWebsite.Service.Permission;
using Microsoft.AspNetCore.Authorization;

namespace BlogWebsite.WebApi.Middlewares
{
    public class AuthorizationMiddleware
    {
        private  readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPermissionService permissionService)
        {
            var loginPath = "/api/user/authenticate";

            if(context.Request.Path.StartsWithSegments(loginPath) )
            {
                await _next(context);
                return;
            }

            // Lấy thông tin người dùng hiện tại từ HttpContext
            var user = context.User;

            var routeValues = context.Request.RouteValues;
            

            


            var endpoint = context.GetEndpoint();

            var isApiRequest = context.Request.Path.StartsWithSegments("/api");//?????????
            if(isApiRequest)
            {
                var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>();
                if (allowAnonymous != null)
                {
                    await _next(context);
                    return;
                    //?return;?
                }
                if (user?.Identity?.IsAuthenticated != true)
                {
                    //var returnUrl = context.Request.Path + context.Request.QueryString;
                    //context.Response.Redirect($"/Admin/AdminLogin");
                    //return;

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.Headers.Add("Location", "/Admin/AdminLogin");
                    await context.Response.WriteAsync("You must login to access this resource.");
                    return;
                }

                if (user?.Identity?.IsAuthenticated == true)
                {
                    // Lấy route hoặc action từ request
                    //var endpoint = context.GetEndpoint();
                    var routeValue = context.Request.RouteValues;


                    // Kiểm tra quyền của user cho route hiện tại
                    if (routeValue != null && routeValue.TryGetValue("controller", out var controller) && routeValue.TryGetValue("action", out var action))
                    {
                        var hasPermission = await permissionService.UserHasPermissionAsync(user, controller.ToString(), action.ToString());
                        if (!hasPermission)
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("You dont have permission to access this resource");
                            return;
                        }
                    }
                }
            }
            
            // Bỏ qua nếu có [AllowAnonymous]
            
            await _next(context);
        }
    }
}
