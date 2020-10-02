using BuyOrBid.Models.Database.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models.Database
{
    public class PostActivity
    {
        [Key]
        public int PostActivityId { get; set; }

        [Required]
        public DateTimeOffset? ActivityDate { get; set; }

        [Required]
        public PostStatus? PostStatus { get; set; }
    }
}
