using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Common
{
    public class ApiSucceedResult<T>:ApiResult<T>
    {

        public ApiSucceedResult()
        {
            IsSucceed = true;
            
        }
        public ApiSucceedResult(T resultObj)
        {
            IsSucceed = true;
            ResultObj = resultObj;
        }
    }
}
