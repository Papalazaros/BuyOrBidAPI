using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyOrBid.Models.Database
{
    public class PostImage
    {
        [Key]
        public int PostImageId { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public string? ThumbnailUrl { get; set; }
    }
}
