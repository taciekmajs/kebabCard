namespace kebabCards.Models
{
    public class TransationItemSeparated
    {
        public DateTime TransationTime { get; set; }
        public int CardId { get; set; }
        public string Product { get; set; }
        public TransactionType TransactionType { get; set; }
        public float Points { get; set; }
    }
}
