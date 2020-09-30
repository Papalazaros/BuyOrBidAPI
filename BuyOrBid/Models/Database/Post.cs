using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyOrBid.Models.Database
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Price { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? SystemTitle { get; set; }

        [Required]
        public string? UserTitle { get; set; }

        public User? CreatedByUser { get; set; }

        [Required]
        public int? CreatedByUserId { get; set; }

        [Required]
        public DateTimeOffset? CreatedDate { get; set; }

        [Required]
        public DateTimeOffset? ModifiedDate { get; set; }

        [Required]
        public bool? IsPublic { get; set; }

        [Required]
        public string? Language { get; set; }
    }
}
