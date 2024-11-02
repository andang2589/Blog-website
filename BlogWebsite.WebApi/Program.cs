using Microsoft.AspNetCore.Identity;
using BlogWebsite.Service.User;
using BlogWebsite.Data.Models;
using BlogWebsite.Data.DAL;
using Microsoft.EntityFrameworkCore;
using BlogWebsite.Utilities.Config;
using BlogWebsite.Service.Role;
using BlogWebsite.Service.Blog;
using BlogWebsite.Service.Common;
using BlogWebsite.Service.Comment;
using BlogWebsite.Service.Category;
using FluentValidation.AspNetCore;
using BlogWebsite.DTO.User;
using BlogWebsite.DTO.Validation;
using FluentValidation;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Category;
using BlogWebsite.Service.Permission;
using BlogWebsite.WebApi.Middlewares;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));


var connectionString = builder.Configuration.GetConnectionString("BlogWebsiteConnection");
builder.Services.AddDbContext<BlogWebsiteContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser,AppRole>()
    .AddEntityFrameworkStores<BlogWebsiteContext>()
    .AddDefaultTokenProviders();


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);


//Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        // Tùy ch?nh hành vi khi token không h?p l?
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (!context.Response.HasStarted)
                {
                    //context.HandleResponse(); // Ng?n không cho thêm b?t k? ph?n t? nào vào response tr??c khi tùy ch?nh.
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"message\":\"Unauthorized access\"}");
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                if (!context.Response.HasStarted)
                {
                    context.HandleResponse(); // Ng?n ch?n response m?c ??nh c?a JWT Middleware.
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"message\":\"You must login to access this resource.\"}");
                }
                return Task.CompletedTask;
            }
        };
    });




//Declare DI
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IBlogService, BlogService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICommonService<BlogPost>, CommonService<BlogPost>>();
builder.Services.AddTransient<ICommonService<Comment>, CommonService<Comment>>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<ICommonService<Category>, CommonService<Category>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<BlogPostDTO>, BlogPostDtoValidator>();
builder.Services.AddScoped<IValidator<CategoryDto>, CategoryDtoValidator>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<ICommonService<AppUserRoles>, CommonService<AppUserRoles>>();
builder.Services.AddTransient<ICommonService<RolePermission>, CommonService<RolePermission>>();
builder.Services.AddTransient<ICommonService<Permission>, CommonService<Permission>>();


//Seed Permission Data
builder.Services.AddScoped<PermissionSeeder>();

//Session
//builder.Services.AddSession(option =>
//    option.IdleTimeout = TimeSpan.FromMinutes(30));

// Add services to the container.

//builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()) ;

builder.Services.AddControllers().AddNewtonsoftJson(option=>
    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedSqlServerCache(options =>
{
    //options.ConnectionString = "Data Source=<DESKTOP-VHBEPKJ>;Initial Catalog=<BlogDB>;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True";
    options.ConnectionString = connectionString;

    options.SchemaName = "dbo";
    options.TableName = "Cache";
});




// Configure Serilog for logging
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .WriteTo.Console()
//    .WriteTo.File("logs/MyAppLog.txt")
//    .CreateLogger();

//// Set Serilog as the logging provider
//// This will also replace default logging provider with Serilog
//builder.Host.UseSerilog();



builder.Host.UseSerilog((context, services, configuration) =>
{
    // Reads configuration settings for Serilog from the appsettings.json file or any other configuration source
    // This enables setting options such as log levels, sinks, and output formats directly from configuration files.
    configuration.ReadFrom.Configuration(context.Configuration);

    // Allows Serilog to integrate with other services registered in the Dependency Injection (DI) container
    // This is useful if any of the logging sinks require dependencies that are registered in DI,
    // allowing them to access services such as database or HTTP context.
    configuration.ReadFrom.Services(services);
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
//Calling seed Permission function
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var permissionSeeder = services.GetRequiredService<PermissionSeeder>();
    await permissionSeeder.SeedPermissionAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthorizationMiddleware>();






//app.UseSession();

app.MapControllers();
app.UseStaticFiles();

app.Run();
