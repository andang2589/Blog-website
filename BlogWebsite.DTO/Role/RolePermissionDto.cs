using BlogWebsite.DTO.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Role
{
    public class RolePermissionDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionDto> Permissions { get; set; }
        
    }
}
