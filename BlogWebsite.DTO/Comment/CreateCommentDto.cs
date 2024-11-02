using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Comment
{
    public class CreateCommentDto
    {
        public int? ReplyToCommentId { get; init; }
        [Required, StringLength(1000,MinimumLength =10)]
        public string Content { get; init; }
        [EmailAddress,StringLength(100,MinimumLength = 6),Required]
        public string Email { get; init; }
        [Required,StringLength(60,MinimumLength =3),DisplayName("Full Name")]
        public string FullName { get; init; }

        public int BlogPostID { get; init; }
    }
}
