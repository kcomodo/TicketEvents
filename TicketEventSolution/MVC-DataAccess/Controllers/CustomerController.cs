using Microsoft.AspNetCore.Mvc;
using MVC_DataAccess.Models;
using MVC_DataAccess.Repositories;
using MVC_DataAccess.Repositories.Customer;
using MVC_DataAccess.Services;
namespace MVC_DataAccess.Controllers
{
    public class CustomerController : Controller
    {
        //dependency injection field
        //declares a private readonly field for storing an instance for the interface repository
        private readonly ICustomerRepository _customerRepository;
        public IActionResult Index()
        {
           
            return View("CustomerIndex");
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
        public IActionResult AddCustomerController(CustomerModel customer)
        {
            _customerRepository.addCustomer(customer);
            return RedirectToAction("Index");
        }
    }
}
