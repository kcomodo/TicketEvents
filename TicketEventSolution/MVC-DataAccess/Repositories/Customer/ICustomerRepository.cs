using MVC_DataAccess.Models;

namespace MVC_DataAccess.Repositories.Customer
{
    public interface ICustomerRepository
    {
        CustomerModel getCustomerInfo(string email);
        void addCustomer(CustomerModel customer);
        void deleteCustomer(string email);
        void updateCustomer(string firstname, string lastname, string email, string password, string targetemail);
    }
}
