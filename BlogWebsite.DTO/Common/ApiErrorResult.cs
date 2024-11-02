using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; }
        public ApiErrorResult()
        {
            
        }
        public ApiErrorResult(string message) 
        {
            IsSucceed = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErrors )
        {
            IsSucceed = false;
            ValidationErrors = validationErrors;
        }
    }
}
