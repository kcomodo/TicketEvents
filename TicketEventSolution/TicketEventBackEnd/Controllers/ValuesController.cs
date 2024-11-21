﻿using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TicketEventBackEnd.Services.Customer;
using TicketEventBackEnd.Models.Customer;
using TicketEventBackEnd.Repositories.Customer;
using TicketEventBackEnd.Models.Login;

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
using static System.Net.WebRequestMethods;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketEventBackEnd.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        private readonly string _issuer = "https://localhost:7240"; // Your issuer
        private readonly string _audience = "https://localhost:7240 , http://localhost:4200"; // Your audience

     
        //Use this for transit api
        //And for searching specific agency/operator names
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerServices _customerServices;
        public ValuesController(ICustomerRepository customerRepository, ICustomerServices customerServices)
        {
            _customerRepository = customerRepository;
            _customerServices = customerServices;

        }
        
        //use async for asynchronous programming so that the method doesn't block the main thread while waiting for the
        //database to repsond
        //async will mark this method as asynchronous 
        //will contain await expression 
        //async always returns a task, or task<T> T is the type of result
        //await will pause hte execution until the task has completed, the await will only execute after
        //the asynchronous operation finished
        //if we dont use async then the code would block the thread during the query which prevents all other requests to not be processed
        //because they have to wait for this method to finish.
        //typically you use async when retrieving large amount of data
        //dont need it for updating or deletion or validation.
        [HttpGet("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer()
        {
            IEnumerable<CustomerModel> customer = await _customerRepository.getAllCustomer();
            return Ok(customer);
        }
        // GET api/<ValuesController>/5
        [Authorize]
        [HttpGet("GetCustomerByEmail")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            var tokenEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tokenEmail != email)
            {
                return Forbid("The email in the token does not match the requested email.");
            }
            var customer = await _customerRepository.getCustomerInfo(email);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found." });
            }
            return Ok(customer);
        }
        // POST api/<ValuesController>
        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer([FromBody] CustomerModel customer)
        {
            _customerRepository.addCustomer(customer);
            return Ok(customer);
        }
        /*
        [Authorize]
        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromQuery] string targetemail, [FromBody] CustomerModel customer)
        {
            var tokenEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tokenEmail != targetemail)
            {
                return Forbid("The email in the token does not match the requested email.");
            }
            System.Diagnostics.Debug.WriteLine(customer.FirstName, customer.LastName, customer.Email, customer.Password, targetemail);
            // Update the customer in the repository using the information provided
            // Fetch and return the updated customer details
            var updatedCustomer = _customerRepository.getCustomerInfo(targetemail);
            if (updatedCustomer == null)
            {
                return NotFound("Customer not found after update.");
            }
            _customerRepository.updateCustomer(customer.FirstName, customer.LastName, customer.Email, customer.Password, targetemail);
            return Ok();
        }
        */
        [Authorize]
        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromQuery] string targetemail, [FromBody] CustomerModel customer)
        {
            var tokenEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tokenEmail != targetemail)
            {
                return Forbid("The email in the token does not match the requested email.");
            }

            try
            {
                // First check if the customer exists
                var existingCustomer = _customerRepository.getCustomerInfo(targetemail).Result;
                if (existingCustomer == null)
                {
                    return NotFound($"Customer with email {targetemail} not found.");
                }

                // Then perform the update
                _customerRepository.updateCustomer(
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.Password,
                    targetemail
                );

                // Return success status
                return Ok(new { message = "Customer updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating customer", error = ex.Message });
            }
        }
        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomer(string email)
        {
            _customerRepository.deleteCustomer(email);
            return Ok();
        }
        [HttpPost("ValidateLogin")]
        public async Task<IActionResult> ValidateLogin([FromBody] LoginForm loginRequest)
        {
            var email = loginRequest.Email;
            var password = loginRequest.Password;
            bool validate = await _customerServices.validateCustomerLogin(email, password);
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
                return Unauthorized(new { message = "Invalid email or password" });
            }
        }

        [Authorize]
        [HttpGet("GetCustomerEmail")]
        public IActionResult GetCustomerEmail()
        {
            /*
             STEPS:
             Token handler 
             Get key (secretkey)
             Validate the token
             Get email using FindFirst(ClaimTypes)
             */
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("YourSuperSecureKeyHereThatIsAtLeast32BytesLong");

            try
            {
                // Get the token from the Authorization header
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var checktoken = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : null;

                if (string.IsNullOrEmpty(checktoken))
                {
                    return BadRequest("Token is missing.");
                }

                // Validate the token
                var principal = tokenHandler.ValidateToken(checktoken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:7240", // or your _issuer if defined
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:7240",
                    ValidateLifetime = true, // Ensure the token is not expired
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Get the email claim
                var emailClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // or JwtRegisteredClaimNames.Sub
                if (emailClaim != null)
                {
                    return Ok(new { email = emailClaim });
                }

                return BadRequest("Email claim not found in token.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Token validation failed: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("UpdateFeedToken")]
        public async Task<IActionResult> UpdateFeedToken([FromQuery] string customer_email, [FromBody] CustomerModel customer)
        {
 
            var tokenEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tokenEmail == null || tokenEmail != customer_email)
            {
                return Forbid("You are not authorized to update this feed token.");
            }
            try
            {
                var existingCustomer = await _customerRepository.getCustomerInfo(customer_email);
                if (existingCustomer == null)
                {
                    return NotFound($"Customer with email {customer_email} not found.");
                }

                _customerRepository.updateFeedToken(customer_email, customer.tokenFeed);

                return Ok(new { message = "Token feed updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating token feed", error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetAgencyLocation")]
        public async Task<IActionResult> getLocationBasedOnAgency(string siteToken, string agencyName)
        {
            //You can find schedule based on agency
            //https://transit.land/api/v2/rest/agencies?agency_name=Clemson University&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
            return Ok();
        }
        
        [Authorize]
        [HttpGet("GetFeed")]
        public async Task<IActionResult> getFeedFromSite(string siteToken)
        {
            //https://transit.land/api/v2/rest/feeds?apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV //get feeds
            return Ok();
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
