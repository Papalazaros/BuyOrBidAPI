using System.ComponentModel.DataAnnotations;

namespace BuyOrBid.Models.Database
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
