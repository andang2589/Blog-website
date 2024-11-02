using BlogWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Permission
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        //public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
