using kebabCards.Data;
using kebabCards.Models;

namespace kebabCards.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string AddCustomers(List<int> cardIds)
        {
            List<int> existingCustomers = new List<int>();
            foreach (int i in  cardIds)
            {
                Customer existing = _context.Customers.FirstOrDefault(c => c.CardId == i);
                if (existing != null) 
                {
                    existingCustomers.Add(i);
                    continue;
                }
                Customer customer = new Customer()
                {
                    CardId = i,
                    Points = 0,
                    isActive = false
                };
                _context.Customers.Add(customer);
            }
            _context.SaveChanges();
            string retIfCardsExisted = existingCustomers.Count == 0 ? "" : $" Te karty już istniały: {string.Join(",", existingCustomers)}.";
            return "Karty zostały dodane poprawnie."+retIfCardsExisted;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomer(int cardId)
        {
            return _context.Customers.First(u => u.CardId == cardId);
        }

        public string DeleteCustomers(List<int> cardIds)
        {
            List<int> notFound = new List<int>();
            List<int> alreadyActive = new List<int>();
            foreach (int i in cardIds)
            {
                Customer customer = _context.Customers.FirstOrDefault(c => c.CardId == i);
                if (customer == null)
                {
                    notFound.Add(i);
                    continue;
                }
                if (customer.isActive == true)
                {
                    alreadyActive.Add(i);
                    continue;
                }
                _context.Customers.Remove(customer);
            }
            _context.SaveChanges();
            string notFoundStr = notFound.Count > 0 ? $" Tych kart nie znaleziono: {string.Join(",", notFound)}." : "";
            string alreadyActiveStr = alreadyActive.Count > 0 ? $" Na te karty wykonywane były transakcje, nie można ich usunąć: {string.Join(",", alreadyActive)}." : "";
            return "Poprawnie usunięto karty lojalnościowe." + notFoundStr + alreadyActiveStr;
        }
    }
}
