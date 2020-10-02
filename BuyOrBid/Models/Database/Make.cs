using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models.Database
{
    public class Make
    {
        [Key]
        public int MakeId { get; set; }

        [Required]
        public string? MakeName { get; set; }

        public int? VpicId { get; set; }

#nullable disable
        public virtual ICollection<AutoPost> AutoPosts { get; set; }
#nullable restore
    }
}
