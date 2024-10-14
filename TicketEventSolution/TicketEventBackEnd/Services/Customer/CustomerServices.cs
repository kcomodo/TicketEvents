using MySql.Data.MySqlClient;
using TicketEventBackEnd.Models.Customer;
using TicketEventBackEnd.Repositories;
using TicketEventBackEnd.Repositories.Customer;
namespace TicketEventBackEnd.Services.Customer
{
 
    public class CustomerServices : ICustomerServices
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }
        public async Task<bool> validateCustomerLogin(string email, string password)
        {
            CustomerModel customerInfo = await _customerRepository.getCustomerInfo(email);
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
