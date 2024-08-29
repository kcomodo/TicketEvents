using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text;
using AspBackEnd.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using AspBackEnd.Repositories.Customer;
using AspBackEnd.Models;
namespace AspBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        //dependency injection field
        //declares a private readonly field for storing an instance for the interface repository
        private readonly ICustomerRepository _customerRepository;
        public IActionResult Index()
        {
            return View();
        }
        //constructor injection
        public CustomerController(ICustomerRepository customerRepository)
        {
            //Allows the controller to recieve its dependency
            //just make sure to go to program.cs to inject the dependency
            /*
              services.AddControllersWithViews();
              // Register the repository with the dependency injection container
              services.AddScoped<ICustomerRepository, CustomerRepository>();
             */
            _customerRepository = customerRepository;
        }
        //CRUD method for getting a customer by email
        [HttpPost]
        public IActionResult AddCustomer(CustomerModel customer)
        {
            _customerRepository.addCustomer(customer);
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public IActionResult DeleteCustomer(string email)
        {
            _customerRepository.deleteCustomer(email);
            return RedirectToAction("Index");
        }
    }
}
