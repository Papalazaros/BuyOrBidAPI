using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BuyOrBid.Models.Database
{
    public class PostImage
    {
        [Key]
        public int PostImageId { get; set; }

        [Required]
        public int? PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public string? ThumbnailUrl { get; set; }
    }
}
