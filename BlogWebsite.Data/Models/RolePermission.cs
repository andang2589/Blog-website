using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Models
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public AppRole Role { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

    }
}
