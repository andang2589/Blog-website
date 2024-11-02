using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Common
{
    public class ClassResult<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public static ClassResult<T> SuccessResult(T? data = null)
        {
            if(data == null)
            {
                return new ClassResult<T>
                {
                    IsSuccess = true
                };
            }
            return new ClassResult<T>
            {
                IsSuccess = true, 
                Data = data   
            };
            
        }

        //public static ClassResult<T> SuccessResult()
        //{
        //    return new ClassResult<T> { Success = true };
        //}

        public static ClassResult<T> FailureResult(params string[] errors)
        {
            return new ClassResult<T>
            {
                IsSuccess = false,
                Errors = errors.ToList()
            };

        }

        public static ClassResult<T> UnauthorizedResult()
        {
            return new ClassResult<T> { IsSuccess = false, Message = "Unauthorized" };
        }

        public static ClassResult<T> ForbiddenResult()
        {
            return new ClassResult<T> { IsSuccess = false, Message = "Forbidden" };
        }

        public static ClassResult<T> BadRequestResult()
        {
            return new ClassResult<T> { IsSuccess = false, Message = "BadRequest" };
        }

    }
}
