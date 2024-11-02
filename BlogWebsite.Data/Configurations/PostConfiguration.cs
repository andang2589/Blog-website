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
    public class PostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            //builder.HasMany(e => e.Categories)
            //    .WithMany(e=>e.Posts)
            //    .UsingEntity<Dictionary<string, object>>(
            //"BlogPostCategory", // Tên bảng kết hợp
            //j => j
            //    .HasOne<Category>()
            //    .WithMany()
            //    .HasForeignKey("CategoryID"), // Tên cột khóa ngoại cho Category
            //j => j
            //    .HasOne<BlogPost>()
            //    .WithMany()
            //    .HasForeignKey("BlogPostID"), // Tên cột khóa ngoại cho BlogPost
            //j =>
            //{
            //    j.HasKey("CategoryID", "BlogPostID"); // Đặt khóa chính cho bảng kết hợp
            //    j.ToTable("BlogPostCategories"); // Tên bảng kết hợp
            //});

            builder.Property(x=>x.ThumbnailUrl).HasMaxLength(250);
            builder.Property(x=>x.Description).HasMaxLength(250);
            //builder.Property(x => x.CreateDate).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(250);
        }
    }
}
