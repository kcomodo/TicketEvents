using MVC_DataAccess.Models;
using MVC_DataAccess.Repositories.Customer;
// See https://aka.ms/new-console-template for more information
using System;

namespace ConsoleTestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //1 to add customer
            //2 to get customer info
            //3 to update customer info
            //4 to delete customer info
            Console.Write("Enter a number to test:");
            int choice = Convert.ToInt32(Console.ReadLine());
            //Create an object for the repository
            //Create an object for the model
            //Assign values to the model
            //Add the model to the repository method
            ICustomerRepository customerRepository = new CustomerRepository();
            CustomerModel customerModel = new CustomerModel();
            /*
            CustomerModel customerModel = new CustomerModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "JohnDoe@gmail.com",
                Password = "password1234"
            };
            */
            if (choice == 1)
            {
                Console.Write("Enter first name: ");
                customerModel.FirstName = Console.ReadLine();

                Console.Write("Enter last name: ");
                customerModel.LastName = Console.ReadLine();

                Console.Write("Enter Email: ");
                customerModel.Email = Console.ReadLine();

                Console.Write("Enter password: ");
                customerModel.Password = Console.ReadLine();
                customerRepository.addCustomer(customerModel);
                    
            }
            if (choice == 2)
            {
                CustomerModel customerInfo = customerRepository.getCustomerInfo("JohnDoe@gmail.com");
                //Concatenate the string when printing it out 
                Console.WriteLine($"Custoomer Id: {customerInfo.CustomerId}");
                Console.WriteLine($"Customer First Name: {customerInfo.FirstName}");
                Console.WriteLine($"Customer Last Name: {customerInfo.LastName}");
                Console.WriteLine($"Customer Email: {customerInfo.Email}");
                Console.WriteLine($"Customer Password: {customerInfo.Password}");
            }
            if (choice == 3)
            {
                customerRepository.deleteCustomer("testing@gmail.com");
            }
            if (choice == 4)
            {
                customerRepository.updateCustomer("UpdateJohn", "UpdateJoe", "UpdateJohnDoe@gmail.com", "Password54321", "JohnDoe@gmail.com");
            }
            if (choice == 5)
            {
               IEnumerable<CustomerModel> allCustomerInfo =  customerRepository.getAllCustomer();
               foreach(var customer in allCustomerInfo)
                {
                    Console.WriteLine(customer.CustomerId);
                    Console.WriteLine(customer.FirstName);
                    Console.WriteLine(customer.LastName);
                    Console.WriteLine(customer.Email);
                    Console.WriteLine(customer.Password);
                }
            }
            else
            {
                Console.Write("Invalid option");
            }

        }
    }
}