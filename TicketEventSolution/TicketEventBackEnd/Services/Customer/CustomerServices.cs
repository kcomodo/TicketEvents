using MySql.Data.MySqlClient;
using TicketEventBackEnd.Models;
using TicketEventBackEnd.Repositories;
using TicketEventBackEnd.Repositories.Customer;
namespace MVC_DataAccess.Services.Customer
{
 
    public class CustomerServices : ICustomerServices
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }
        public bool validateCustomerLogin(string email, string password)
        {
            CustomerModel customerInfo = _customerRepository.getCustomerInfo(email);
            if(customerInfo == null)
            {
                return false;
            }
            if(customerInfo.Email == email && customerInfo.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
