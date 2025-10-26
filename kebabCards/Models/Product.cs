using System.ComponentModel.DataAnnotations;

namespace kebabCards.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float PointsEarned { get; set; }
        [Required]
        public float PointsCost { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
