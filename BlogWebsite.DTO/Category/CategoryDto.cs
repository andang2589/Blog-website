﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Category
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        
        public string CategoryName { get; set; }
    }
}
