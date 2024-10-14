using TicketEventBackEnd.Models.Customer;

namespace TicketEventBackEnd.Repositories.Customer
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerModel>> getAllCustomer();
        CustomerModel getCustomerInfo(string email);
        void addCustomer(CustomerModel customer);
        void deleteCustomer(string email);
        void updateCustomer(string firstname, string lastname, string email, string password, string targetemail);
    }
}
