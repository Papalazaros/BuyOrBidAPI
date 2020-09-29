using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models.Database
{
    public class Model
    {
        [Key]
        public int? ModelId { get; set; }

        [Required]
        public string? ModelName { get; set; }

        [Required]
        public int? MakeId { get; set; }

        public Make? Make { get; set; }

        public int? VpicId { get; set; }

#nullable disable
        public virtual ICollection<AutoPost> AutoPosts { get; set; }
#nullable restore
    }
}
