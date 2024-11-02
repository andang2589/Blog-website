using BlogWebsite.WebApi.Utilities;

namespace BlogWebsite.WebApi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log lỗi và xử lý ngoại lệ
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Cấu trúc lỗi trả về
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound, // Nếu lỗi liên quan đến dữ liệu không tìm thấy
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized, // Nếu lỗi liên quan đến phân quyền
                CustomException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError, // Lỗi mặc định (Internal Server Error)

            };

            var errorDetails = new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message // Trả về thông tin lỗi
            };

            return context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
