using BuyOrBid.Models.Database.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models.Database
{
    public class PostActivity
    {
        [Key]
        public int? PostActivityId { get; set; }

        public Post? Post { get; set; }

        [Required]
        public int? PostId { get; set; }

        [Required]
        public DateTimeOffset? ActivityDate { get; set; }

        [Required]
        public PostStatus? PostStatus { get; set; }
    }
}
