using System.ComponentModel.DataAnnotations;

namespace kebabCards.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CardId { get; set; }
        [Required]
        public float Points { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}
