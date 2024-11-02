using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.Utilities.Config;
using BlogWebsite.Data.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using BlogWebsite.DTO.Validation;
using BlogWebsite.Areas.Admin.Helper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using BlogWebsite.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddHttpClient();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);


//Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
//{

//    option.LoginPath = "/Admin/AdminLogin/";
//    option.AccessDeniedPath = "/Admin/Forbidden";
//});


builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(fv=>
fv.RegisterValidatorsFromAssemblyContaining<CategoryDtoValidator>());
builder.Services.AddSession(option => 
    option.IdleTimeout = TimeSpan.FromMinutes(60));


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
builder.Services.AddTransient<IBlogApiClient, BlogApiClient>();
builder.Services.AddTransient<ICommentApiClient, CommentApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<HttpClientHelper>();
builder.Services.AddTransient<TokenValidationParameters>();

//Register DbContext
var connectionString = builder.Configuration.GetConnectionString("BlogWebsiteConnection");
builder.Services.AddDbContext<BlogWebsiteContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css", "/lib/bootstrap/dist/css/bootstrap.min.css", /*"/css/HomePage.css",*/ "/css/Admin/AdminUser/CreateView.css");
    pipeline.AddJavaScriptBundle("/js/bundle.js", "js/HomePage.js","js/Posts.js"/*,"/lib/jquery/dist/jquery.js"*/, "/js/Admin/AdminLogin/Create.js", "/js/Admin/AdminBlog/BlogIndex.js", "/js/Admin/AdminUser/UserIndex.js", "/js/Admin/AdminCategory/CategoryIndex.js", "/js/Admin/AdminRole/RoleIndex.js");
    pipeline.MinifyCssFiles();
    pipeline.MinifyJsFiles();
});

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = connectionString;
    options.SchemaName = "dbo";
    options.TableName = "Cache";
});


var app = builder.Build();

// Cấu hình Permission Helper với Memory Cache
//PermissionHelper.Configure(app.Services.GetRequiredService<IMemoryCache>());
var cache = app.Services.GetService<IDistributedCache>();
var httpContextAccessor = app.Services.GetService<IHttpContextAccessor>();

PermissionHelper.Configure(cache, httpContextAccessor);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseMiddleware<JwtCookieMiddleware>();


//app.UseAuthentication();
app.UseWebOptimizer();
app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

        endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=AdminLogin}/{action=Login}/{id?}");

        endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=AdminMain}/{action=Index}/{id?}");

        endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=AdminLogin}/{action=Index}/{id?}");

        endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=AdminUser}/{action=Create}/{id?}");

        endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=AdminUser}/{action=Edit}/{id?}");
    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.UseStaticFiles();

app.Run();
