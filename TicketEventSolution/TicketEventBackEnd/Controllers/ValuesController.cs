using Microsoft.AspNetCore.Mvc;
using MVC_DataAccess.Services.Customer;
using TicketEventBackEnd.Models;
using TicketEventBackEnd.Repositories;
using TicketEventBackEnd.Repositories.Customer;
/*
 Import Token from microsoft.IdentityModel
 Import the system Identity model
 Import Http from Asp.net
 Import Authentication for JWTBearer and Authorization from Asp.net
 */
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Security.Claims;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketEventBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string _issuer = "https://localhost:7059"; // Your issuer
        private readonly string _audience = "https://localhost:7059"; // Your audience

        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerServices _customerServices;
        public ValuesController(ICustomerRepository customerRepository, ICustomerServices customerServices)
        {
            _customerRepository = customerRepository;
            _customerServices = customerServices;

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
        [HttpGet("Validate Login")]
        public IActionResult ValidateLogin(string email, string password)
        {
            bool validate = _customerServices.validateCustomerLogin(email, password);
            return Ok(validate);
        }
        
        private string generateJWTToken(string email)
        {
            //creat a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //create an encrypted key
            var key = Encoding.UTF8.GetBytes("YourSuperSecureKeyHereThatIsAtLeast32BytesLong");

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = "https://localhost:7059 , http://localhost:4200/",  // Your audience
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
        
    }
}
