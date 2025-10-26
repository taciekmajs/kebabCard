namespace kebabCards.Models.Dtos
{
    public class PerformTransactionDto
    {
        public string UserID { get; set; }
        public int CardID { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<int> productIds { get; set; }
    }
}
