using BuyOrBid.Models.Database.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BuyOrBid.Models.Database
{
    public class PostActivity
    {
        [Key]
        public int PostActivityId { get; set; }

        [Required]
        public int? PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }

        [Required]
        public DateTimeOffset? ActivityDate { get; set; }

        [Required]
        public PostStatus? PostStatus { get; set; }
    }
}
