using TicketEventBackEnd.Models.Customer;

namespace TicketEventBackEnd.Repositories.Customer
{
    public interface ICustomerRepository
    {
        CustomerModel getCustomerInfo(string email);
        IEnumerable<CustomerModel> getAllCustomer();
        void addCustomer(CustomerModel customer);
        void deleteCustomer(string email);
        void updateCustomer(string firstname, string lastname, string email, string password, string targetemail);
    }
}
