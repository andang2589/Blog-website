using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.DTO.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;
using Newtonsoft.Json;
using System.Net.Http;
using BlogWebsite.Data.Models;

namespace BlogWebsite.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserApiClient _userApiClient;
        private readonly IHttpClientFactory _clientFactory;


        public AdminLoginController(IConfiguration configuration, IUserApiClient userApiClient, IHttpClientFactory clientFactory) 
        {
            _configuration = configuration;
            _userApiClient = userApiClient;
            _clientFactory = clientFactory;
        }
        



        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Login()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }


        //[HttpPost]
        //[AllowAnonymous]

        //public async Task<IActionResult> Login(LoginRequest request, string? ReturnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(request);
        //    }
        //    var token = await _userApiClient.Authenticate(request);
        //    var userPrincipal = this.ValidateToken(token);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
        //        IsPersistent = false,
        //    };

        //    HttpContext.Session.SetString("Token", token);

        //    await HttpContext.SignInAsync(
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                userPrincipal,
        //                authProperties);
        //    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        //    {
        //        return LocalRedirect(ReturnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "AdminUser");
        //    }

        //}

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.Remove("Token");

            Response.Cookies.Delete("jwt");

            return RedirectToAction("Login", "AdminLogin");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = (_configuration["Tokens:Issuer"]);
            
            validationParameters.ValidIssuer = (_configuration["Tokens:Issuer"]);
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }



        public IActionResult LoginTest()
        {
            return View();
        }




        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");

            var response = await client.PostAsJsonAsync("https://localhost:7297/api/User/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                // Lưu JWT vào cookies
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Secure = true,  // Nếu bạn đang sử dụng HTTPS
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("jwt", result.Token, cookieOptions);
                var user = result.User;
                var userInfoJson = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("UserInfo", userInfoJson);
                return RedirectToAction("Index", "AdminUser", new { Areas = "Admin" });
            }

            ModelState.AddModelError(string.Empty, "Login failed.");
            return View(model);
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
        public UserDTO User { get; set; }
    }

}

    

