﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Common
{
    public class ApiResult<T>
    {
        
        public bool IsSucceed { get; set; }

        public string Message { get; set; }
        public T ResultObj { get; set; }
    }
}
