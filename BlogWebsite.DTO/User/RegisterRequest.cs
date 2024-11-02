using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.DTO.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;


namespace BlogWebsite.DTO.User
{
    public class RegisterRequest
    {
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        [Required]
        public DateTime DoB { get; set; }
        [Required]
        [EmailAddress]
        //[Remote(action:"IsEmailInUse", controller:"AdminUser", areaName:"Admin")]
        //[ValidEmailDomain(allowDomain:"gmail.com", ErrorMessage ="Email Domain must be gmail.com")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }


    }
}
