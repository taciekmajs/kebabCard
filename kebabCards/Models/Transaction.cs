using System.ComponentModel.DataAnnotations;

namespace kebabCards.Models
{
    public enum TransactionType
    {
        BoughtForMoney, 
        BoughtForPoints
    }
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime TransactionTime { get; set; }
        [Required]
        public int CardId { get; set; }
        [Required]
        public float Points { get; set; }
        [Required]
        public string ProductNames { get; set; }
        [Required]
        public string ProductIds { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }


    }
}
