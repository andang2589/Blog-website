using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Models
{
    [Table("AspNetRoles")]
    public class AppRole:IdentityRole<Guid>
    {
        public string Description { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<AppUserRoles> UserRoles { get; set; }



    }
}
