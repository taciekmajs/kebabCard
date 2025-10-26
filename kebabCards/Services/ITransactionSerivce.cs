using kebabCards.Models;
using kebabCards.Models.Dtos;

namespace kebabCards.Services
{
    public interface ITransactionSerivce
    {
        string PerformTransaction(PerformTransactionDto transactionDto);
        string CancelTransaction(int  transactionId);
        List<Transaction> GetTodayTransactionsForUser(string userId);
        List<Transaction> GetTransatcionsByDate(DateTime startDate, DateTime endDate);
        List<TransationItemSeparated> GetTransationItemSeparatedByProduct(DateTime startDate, DateTime endDate);
    }
}
