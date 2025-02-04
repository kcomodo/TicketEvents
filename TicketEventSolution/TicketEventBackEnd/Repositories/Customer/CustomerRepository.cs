﻿using TicketEventBackEnd.Models.Customer;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Configuration;
using TicketEventBackEnd.Models.Routes;
namespace TicketEventBackEnd.Repositories.Customer
{
    //Connect to the database and perform CRUD operations
    //Create a constructor for that connection
    public class CustomerRepository: ICustomerRepository
    {
        //create an object to represent the connection to mySql
        private readonly MySqlConnection _connection;

        public CustomerRepository(IConfiguration configuration)
        {

            //check port on xampp
            //server is the root address so 127, etc.
            //userid is root
            string connectionString = configuration.GetConnectionString("TicketEventDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }

            _connection = new MySqlConnection(connectionString);

            //call the object and insert the connection url into MySqlConnection
            //then open the connection
            _connection.Open();
        }
        //Destructor to close the connection when not in use
        ~CustomerRepository()
        {
            _connection.Close();
        }
        //Create CRUD Methods
        public async Task<CustomerModel> getCustomerInfo(string email)
        {
            //create query to do stuff
            string query = "SELECT * FROM customer WHERE customer_email = @Email";
            //insert query into the commands as well as adding the connection to it
            MySqlCommand command = new MySqlCommand(query, _connection);
            //Bind the parameter value to the query value
            command.Parameters.AddWithValue("@Email", email);
            //Use a reader now to grab the data
            using (MySqlDataReader reader = await command.ExecuteReaderAsync() as MySqlDataReader)
            {
                if (await reader.ReadAsync())
                {
                    //create a new object to store the data
                    CustomerModel customer = new CustomerModel();
                    {
                        customer.customer_id = reader.GetInt32("customer_id");
                        customer.customer_firstname = reader.GetString("customer_firstname");
                        customer.customer_lastname = reader.GetString("customer_lastname");
                        customer.customer_email = reader.GetString("customer_email");
                        customer.customer_password = reader.GetString("customer_password");
                        customer.feed_token = reader.GetString("feed_token");
                    };
                    return customer;
                }
                return null;
            }
        }
        public async Task<IEnumerable<CustomerModel>> getAllCustomer()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            string query = "SELECT * FROM customer";
            MySqlCommand command = new MySqlCommand(query, _connection);

            using (MySqlDataReader reader = await command.ExecuteReaderAsync() as MySqlDataReader) 
            {
                while (await reader.ReadAsync())
                {
                    customers.Add(new CustomerModel
                    {
                        customer_id = reader.GetInt32("customer_id"),
                        customer_firstname = reader.GetString("customer_firstname"),
                        customer_lastname = reader.GetString("customer_lastname"),
                        customer_email = reader.GetString("customer_email"),
                        customer_password = reader.GetString("customer_password"),
                        feed_token = reader.GetString("feed_token")
                    });

                }
                return customers;
            }
        }

        public void addCustomer(CustomerModel customer)
        {
            Regex verifyEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            string query = "INSERT INTO customer (customer_firstname, customer_lastname, customer_email, customer_password, feed_token) VALUES (@FirstName, @LastName, @Email, @Password, @feed_token)";
            MySqlCommand command = new MySqlCommand(query, _connection);
            if(!verifyEmail.IsMatch(customer.customer_email))
            {
                Console.WriteLine("Invalid Email. Customer not added.");
                return; // Exit the method without executing the query
            }
            else
            {
            command.Parameters.AddWithValue("@FirstName", customer.customer_firstname);
            command.Parameters.AddWithValue("@LastName", customer.customer_lastname);
            command.Parameters.AddWithValue("@Email", customer.customer_email);
            command.Parameters.AddWithValue("@Password", customer.customer_password);
            command.Parameters.AddWithValue("@feed_token", customer.feed_token);
            }
    
            command.ExecuteNonQuery();
        }
        public void deleteCustomer(string customer_email)
        {
            Regex verifyEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!verifyEmail.IsMatch(customer_email))
            {
                Console.WriteLine("Email is invalid");
                return;
            }
            //   CustomerRepository customer = new CustomerRepository();
            //  Task<CustomerModel> customerInfo = customer.getCustomerInfo(customer_email);
            var customerInfo = getCustomerInfo(customer_email);
            if (customerInfo == null)
            {
                Console.WriteLine("Email does not exist");
                return;
            }
            else
            {
                string query = "DELETE FROM customer where customer_email = @Email";
                MySqlCommand command = new MySqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Email", customer_email);
                command.ExecuteNonQuery();
            }

        }
        public void updateCustomer(string customer_firstname, string customer_lastname, string customer_email, string customer_password, string feed_token, string target_email)
        {
            string query = "UPDATE customer SET customer_firstname = @first_name, customer_lastname = @last_name, " +
                           "customer_email = @email, customer_password = @password, feed_token = @feed_token " +
                           "WHERE customer_email = @target_email";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@target_email", target_email);
            command.Parameters.AddWithValue("@first_name", customer_firstname);
            command.Parameters.AddWithValue("@last_name", customer_lastname);
            command.Parameters.AddWithValue("@email", customer_email);
            command.Parameters.AddWithValue("@password", customer_password);
            command.Parameters.AddWithValue("@feed_token", feed_token); // Add feed_token as a parameter
            command.ExecuteNonQuery();
        }

        public async Task<CustomerModel> getFeedToken(string customer_email)
        {
            string query = "SELECT * FROM customer WHERE customer_email = @Email";
            //insert query into the commands as well as adding the connection to it
            MySqlCommand command = new MySqlCommand(query, _connection);
            //Bind the parameter value to the query value
            command.Parameters.AddWithValue("@Email", customer_email);
            //Use a reader now to grab the data
            using (MySqlDataReader reader = await command.ExecuteReaderAsync() as MySqlDataReader)
            {
                if (await reader.ReadAsync())
                {
                    //create a new object to store the data
                    CustomerModel customer = new CustomerModel();
                    {
                        customer.feed_token = reader.GetString("feed_token");
                    };
                    return customer;
                }
                return null;
            }
        }
        public void updateFeedToken(string target_email, string feed_token)
        {
            string query = "UPDATE customer SET feed_token = @feedToken WHERE customer_email = @target_email";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@target_email", target_email);
            command.Parameters.AddWithValue("@feedToken", feed_token);
            command.ExecuteNonQuery();
        }
        public void saveRouteInfo(string customer_id, string routes_id, double latitude, double longitude)
        {
            string query = "INSERT INTO savedroutes (customer_id, routes_id, latitude, longitude) VALUES (@customer_id, @routes_id, @latitude, @longitude)";
            MySqlCommand command = new MySqlCommand(query, _connection);

                command.Parameters.AddWithValue("@customer_id", customer_id);
                command.Parameters.AddWithValue("@routes_id", routes_id);
                command.Parameters.AddWithValue("@latitude", latitude);
                command.Parameters.AddWithValue("@longitude", longitude);

            command.ExecuteNonQuery();
        }
        public void deleteRouteInfo(string customer_id, string routes_id)
        {
            string query = "DELETE FROM savedroutes WHERE customer_id = @customer_id AND routes_id = @routes_id";
            MySqlCommand command = new MySqlCommand(query, _connection);

            command.Parameters.AddWithValue("@customer_id", customer_id);
            command.Parameters.AddWithValue("@routes_id", routes_id);
            command.ExecuteNonQuery();
        }
        public async Task<List<routesModel>> getRouteInfo(string customer_id)
        {
            string query = "SELECT * FROM savedRoutes WHERE customer_id = @customer_id";
            //insert query into the commands as well as adding the connection to it
            MySqlCommand command = new MySqlCommand(query, _connection);
            //Bind the parameter value to the query value
            command.Parameters.AddWithValue("@customer_id", customer_id);
            List<routesModel> routesList = new List<routesModel>();

            //Use a reader now to grab the data
            using (MySqlDataReader reader = await command.ExecuteReaderAsync() as MySqlDataReader)
            {
                while (await reader.ReadAsync())
                {
                    //create a new object to store the data
                    routesModel routes = new routesModel();
                    {
                    //    routes.customer_Id = reader.GetInt32("customer_id");
                        routes.routes_Id = reader.GetInt32("routes_id");
                        routes.latitude = reader.GetDouble("latitude");
                        routes.longitude = reader.GetDouble("longitude");

                    };
                    routesList.Add(routes);
                }
               
            }
            return routesList;
        }
    }
}
