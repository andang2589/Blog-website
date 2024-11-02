using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Data.Configurations;
using BlogWebsite.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlogWebsite.Data.DAL
{
    public class BlogWebsiteContext : IdentityDbContext<
        AppUser,
        AppRole,
        Guid,
        IdentityUserClaim<Guid>,
        AppUserRoles,
        IdentityUserLogin<Guid>,      
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public BlogWebsiteContext(DbContextOptions options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<AppUser>(b =>
            //{
            //    // Each User can have many UserClaims
            //    //b.HasMany(e => e.Claims)
            //    //    .WithOne(e => e.User)
            //    //    .HasForeignKey(uc => uc.UserId)
            //    //    .IsRequired();

            //    //// Each User can have many UserLogins
            //    //b.HasMany(e => e.Logins)
            //    //    .WithOne(e => e.User)
            //    //    .HasForeignKey(ul => ul.UserId)
            //    //    .IsRequired();

            //    //// Each User can have many UserTokens
            //    //b.HasMany(e => e.Tokens)
            //    //    .WithOne(e => e.User)
            //    //    .HasForeignKey(ut => ut.UserId)
            //    //    .IsRequired();

            //    // Each User can have many entries in the UserRole join table
            //    b.HasMany(e => e.UserRoles)
            //        .WithOne(e => e.User)
            //        .HasForeignKey(ur => ur.UserId)
            //        .IsRequired();
            //});

            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new RolePermissionConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new PermissionConfiguration());

            //builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            //builder.Entity<AppUserRoles>().ToTable("AppUserRoles").HasKey(x => new
            //{
            //    x.UserId,
            //    x.RoleId
            //});
            //builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x=>x.UserId);


            //builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            //builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x=>x.UserId);

            //builder.Entity<AppUserRoles>().HasKey(x => new
            //{
            //    x.UserId, x.RoleId
            //});

        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AppUserRoles> AppUserRoles { get; set; }
    }
}
