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
        public async Task<bool> validateCustomerLogin(string customer_email, string customer_password)
        {
            CustomerModel customerInfo = await _customerRepository.getCustomerInfo(customer_email);
            if (customerInfo == null)
            {
                return false;
            }
            if (customerInfo.customer_email == customer_email && customerInfo.customer_password == customer_password)
            {
                return true;
            }
            return false;
        }
  
    }
}
