using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set;}
    }
}
