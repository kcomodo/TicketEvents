using Microsoft.AspNetCore.Mvc;
using MVC_DataAccess.Services.Customer;
using TicketEventBackEnd.Models;
using TicketEventBackEnd.Repositories;
using TicketEventBackEnd.Repositories.Customer;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketEventBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public ValuesController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }
        [HttpGet("Get All Customer")]
        public IActionResult GetAllCustomer()
        {
            IEnumerable<CustomerModel> customer = _customerRepository.getAllCustomer();
            return Ok(customer);
        }
        // GET api/<ValuesController>/5
        [HttpGet("Get Customer By Email")]
        public IActionResult GetCustomerByEmail(string email)
        {
            CustomerModel customer = _customerRepository.getCustomerInfo(email);
            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }
            return Ok(customer);
        }
        // POST api/<ValuesController>
        [HttpPost("Add Customer")]
        public IActionResult AddCustomer(CustomerModel customer)
        {
            _customerRepository.addCustomer(customer);
            return Ok(customer);
        }
        [HttpDelete("Delete Customer")]
        public IActionResult DeleteCustomer(string email)
        {
            _customerRepository.deleteCustomer(email);
            return Ok();
        }

    }
}
