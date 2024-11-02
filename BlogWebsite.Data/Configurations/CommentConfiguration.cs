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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> comment) 
        {
            comment.HasKey(c => c.CommentID);
            comment.HasIndex(c => c.ParentID);

            comment.HasOne(c => c.Parent)
                   .WithMany(c => c.Children)
                   .HasForeignKey(c => c.ParentID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false); /*.WillCascadeOnDelete(false);*/

            comment.Property(c => c.FullName).HasMaxLength(60).IsRequired();
            comment.Property(c => c.Email).HasMaxLength(100).IsRequired();
            comment.Property(c => c.Content).HasMaxLength(1000).IsRequired();

        }
    }
}
