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
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.Property(x=>x.Description).HasMaxLength(200).IsRequired();
            builder.HasMany(ur => ur.UserRoles)
              .WithOne(u => u.Role) // Each user can have many roles
              .HasForeignKey(ur => ur.RoleId)
              .IsRequired();
        }
    }
}
