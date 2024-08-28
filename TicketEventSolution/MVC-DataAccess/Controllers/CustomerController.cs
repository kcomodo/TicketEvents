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
            //In the beginning of the page called index, fill in the form with all customer info
            IEnumerable<CustomerModel> customers = _customerRepository.getAllCustomer();
            //View works like this: CustomerIndex.cshtml on the left side and object on the right side
            //So basically send the model to the view
            return View("CustomerIndex", customers);
    
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
        public IActionResult AddCustomerControllerMethod(CustomerModel customer)
        {
            _customerRepository.addCustomer(customer);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteCustomerControllerMethod(string email) {
            _customerRepository.deleteCustomer(email);
            return RedirectToAction("Index");
        }

    }
}
