using MVC_DataAccess.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace MVC_DataAccess.Repositories.Customer
{
    //Connect to the database and perform CRUD operations
    //Create a constructor for that connection
    public class CustomerRepository
    {
        //create an object to represent the connection to mySql
        private readonly MySqlConnection _connection;
        public CustomerRepository()
        {
            //check port on xampp
            //server is the root address so 127, etc.
            //userid is root
            string connectionString = "server=127.0.0.1;database=ticketevent;userid=root;port=3307";
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
        public CustomerModel getCustomerInfo(string email)
        {
            //create query to do stuff
            string query = "SELECT * FROM customer WHERE customer_email = @Email";
            //insert query into the commands as well as adding the connection to it
            MySqlCommand command = new MySqlCommand(query, _connection);
            //Bind the parameter value to the query value
            command.Parameters.AddWithValue("@Email", email);
            //Use a reader now to grab the data
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                //create a new object to store the data
                CustomerModel customer = new CustomerModel();
                {
                    customer.CustomerId = reader.GetInt32("customer_id");
                    customer.FirstName = reader.GetString("customer_firstname");
                    customer.LastName = reader.GetString("customer_lastname");
                    customer.Email = reader.GetString("customer_email");
                    customer.Password = reader.GetString("customer_password");
                };
                return customer;
            }
            return null;
        }
        public void AddCustomer(CustomerModel customer)
        {
            Regex verifyEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            string query = "INSERT INTO customer (customer_firstname, customer_lastname, customer_email, customer_password) VALUES (@FirstName, @LastName, @Email, @Password)";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.Parameters.AddWithValue("@Password", customer.Password);
            command.ExecuteNonQuery();
        }
    }
}
