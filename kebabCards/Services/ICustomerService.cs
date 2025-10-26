using kebabCards.Models;

namespace kebabCards.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomer(int cardId);
        string AddCustomers(List<int> cardIds);
        string DeleteCustomers(List<int> cardIds);

    }
}
