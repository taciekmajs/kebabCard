using kebabCards.Data;
using kebabCards.Models;
using kebabCards.Models.Dtos;

namespace kebabCards.Services
{
    public class TransactionService : ITransactionSerivce
    {
        private readonly ApplicationDbContext _context;
        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CancelTransaction(int transactionId)
        {
            Transaction transaction = _context.Transactions.Find(transactionId);
            if (transaction == null) 
            {
                return "Transakcja nie została znaleziona.";
            }
            Customer customer = _context.Customers.FirstOrDefault(c => c.CardId == transaction.CardId);
            if (customer.Points - transaction.Points < 0)
            {
                return "Anulowanie transakcji jest niemożliwe - karta miałaby ujemne punkty";
            }
            customer.Points -= transaction.Points;
            _context.Customers.Update(customer);
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return "Anulowanie transakcji powiodło się.";
        }

        public List<Transaction> GetTodayTransactionsForUser(string userId)
        {
            var transactions =  _context.Transactions.Where(t => t.UserId == userId && t.TransactionTime > DateTime.Now.AddDays(-1)).ToList();
            return transactions.OrderByDescending(t => t.TransactionTime).ToList();
        }

        public List<Transaction> GetTransatcionsByDate(DateTime startDate, DateTime endDate)
        {
            return _context.Transactions.Where(t => t.TransactionTime >= startDate && t.TransactionTime <= endDate).ToList();
        }

        public List<TransationItemSeparated> GetTransationItemSeparatedByProduct(DateTime startDate, DateTime endDate)
        {
            var transactions = _context.Transactions.Where(t => t.TransactionTime >= startDate && t.TransactionTime <= endDate).ToList();
            Dictionary<int,float> productEarned = new Dictionary<int, float>();
            Dictionary<int, float> productCost = new Dictionary<int, float>();
            Dictionary<int,string> productNames = new Dictionary<int, string>();
            List<Product> products = _context.Products.ToList();
            foreach (var product in products)
            {
                productEarned.Add(product.Id, product.PointsEarned);
                productCost.Add(product.Id, product.PointsCost);
                productNames.Add(product.Id, product.Name);
            }
            List<TransationItemSeparated> transationsItemSeparated = new List<TransationItemSeparated>();
            foreach (var transaction in transactions)
            {
                List<int> productIds  = transaction.ProductIds.Split(',').Select(int.Parse).ToList();
                foreach (var productId in productIds) 
                {
                    transationsItemSeparated.Add(new TransationItemSeparated()
                    {
                        TransationTime = transaction.TransactionTime,
                        TransactionType = transaction.TransactionType,
                        CardId = transaction.CardId,
                        Product = productNames[productId],
                        Points = transaction.TransactionType == TransactionType.BoughtForMoney ? productEarned[productId] : productCost[productId],
                    });
                }
            }
            return transationsItemSeparated;
        }

        public string PerformTransaction(PerformTransactionDto transactionDto)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == transactionDto.UserID);
            Customer customer = _context.Customers.FirstOrDefault(c => c.CardId == transactionDto.CardID);
            List<string> productNames = new List<string>();
            float points = 0;
            foreach (int productId in transactionDto.productIds)
            {
                Product product = _context.Products.FirstOrDefault(p => p.Id == productId);
                productNames.Add(product.Name);
                switch (transactionDto.TransactionType)
                {
                    case TransactionType.BoughtForMoney:
                        {
                            points += product.PointsEarned;
                            break;
                        }
                    case TransactionType.BoughtForPoints:
                        {
                            points -= product.PointsCost;
                            break;
                        }
                    default: break;
                }
            }
            if (customer.Points + points < 0) 
            {
                return "Transakcja jest niemożliwa";
            }
            customer.Points += points;
            customer.isActive = true;
            Transaction transaction = new Transaction()
            {
                UserId = user.Id,
                UserName = user.Name,
                TransactionTime = DateTime.Now,
                CardId = customer.CardId,
                Points = points,
                ProductNames = string.Join(",", productNames),
                ProductIds = string.Join(",", transactionDto.productIds),
                TransactionType = transactionDto.TransactionType,
            };
            _context.Customers.Update(customer);
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return "Transakcja przebiegła pomyślnie";
        }
    }
}
