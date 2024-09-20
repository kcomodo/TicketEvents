using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TicketEventBackEnd.Services.Customer;
using TicketEventBackEnd.Models.Customer;
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
    //https://transit.land/api/v2/rest/feeds?apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
    //Use this for transit api
    public class ValuesController : ControllerBase
    {
        private readonly string _issuer = "https://localhost:7240"; // Your issuer
        private readonly string _audience = "https://localhost:7240 , http://localhost:4200"; // Your audience

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
        [Authorize]
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
            //when validation is true, generate a token for authorization of other methods
            if (validate)
            {
                var token = generateJWTToken(email);
                //create an instance for the cookie by assigning it to an object
                //normally has configurable options such as expiration date, security setting and accessbility
                var cookieOptions = new CookieOptions
                {
                    //Since we're using JWT token and authentication we need http
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1),
                    //Samesite controls cross-sites requests
                    //Strict means the cookie is only sent with requests from the same site
                    //Lax allows some external sites
                    //None means the cookie is sent with all requests including cross-site requests
                    SameSite = SameSiteMode.Strict
                };
                //After giving the object some configs append the token onto a cookie which can now be accessed from the client side
                Response.Cookies.Append("auth_token",token,cookieOptions);
                return Ok(new { Token = token });

            }
            else
            {
                return Unauthorized();
            }
        }
        
        //use email as parameter to identify the user when the token is presented in the future requests
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
                Audience = "https://localhost:7240",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription); // Create token
            return tokenHandler.WriteToken(token); // Write token and return it
        }
        
    }
}
