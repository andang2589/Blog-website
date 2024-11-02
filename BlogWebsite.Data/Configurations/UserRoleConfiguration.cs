using BlogWebsite.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<AppUserRoles>
    {
        public void Configure(EntityTypeBuilder<AppUserRoles> builder)
        {
            builder.HasKey(ur => new {ur.UserId, ur.RoleId});
            builder.HasOne(ur => ur.User)
              .WithMany(u => u.UserRoles) // Each user can have many roles
              .HasForeignKey(ur => ur.UserId)
              .IsRequired();

            // Configure relationship between AppUserRoles and AppRole
            builder.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles) // Each role can be assigned to many users
                  .HasForeignKey(ur => ur.RoleId)
                  .IsRequired();

        }
    }
}
