using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Models
{
    [Table("AspNetUsers")]

    public class AppUser:IdentityUser<Guid>
    {
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public DateTime DoB { get; set; }
        public virtual ICollection<AppUserRoles> UserRoles { get; set; }
    }
}
