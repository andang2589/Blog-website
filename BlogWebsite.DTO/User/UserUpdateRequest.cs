using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.DTO.User
{
    public class UserUpdateRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "User")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        

        
    }
}
