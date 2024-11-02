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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.LastName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DoB).IsRequired();


            builder.HasMany(ur => ur.UserRoles)
             .WithOne(u => u.User) // Each user can have many roles
             .HasForeignKey(ur => ur.UserId)
             .IsRequired();
        }
    }
}
