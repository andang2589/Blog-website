using BlogWebsite.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.PermissionId);

            //builder.HasData(
            //    new Permission { PermissionId = 1, Name = "Register" },
            //    new Permission { PermissionId = 2, Name = "DeleteUser" }

            //    );
        }
    }
}
