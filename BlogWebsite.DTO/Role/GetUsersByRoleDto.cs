using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Role
{
    public class GetUsersByRoleDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
