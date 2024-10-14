using TicketEventBackEnd.Models.Customer;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace TicketEventBackEnd.Repositories.Customer
{
    //Connect to the database and perform CRUD operations
    //Create a constructor for that connection
    public class CustomerRepository: ICustomerRepository
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
                        CustomerId = reader.GetInt32("customer_id"),
                        FirstName = reader.GetString("customer_firstname"),
                        LastName = reader.GetString("customer_lastname"),
                        Email = reader.GetString("customer_email"),
                        Password = reader.GetString("customer_password")
                    });

                }
                return customers;
            }
        }

        public void addCustomer(CustomerModel customer)
        {
            Regex verifyEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            string query = "INSERT INTO customer (customer_firstname, customer_lastname, customer_email, customer_password) VALUES (@FirstName, @LastName, @Email, @Password)";
            MySqlCommand command = new MySqlCommand(query, _connection);
            if(!verifyEmail.IsMatch(customer.Email))
            {
                Console.WriteLine("Invalid Email. Customer not added.");
                return; // Exit the method without executing the query
            }
            else
            {
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.Parameters.AddWithValue("@Password", customer.Password);
            }
    
            command.ExecuteNonQuery();
        }
        public void deleteCustomer(string email)
        {
            Regex verifyEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!verifyEmail.IsMatch(email))
            {
                Console.WriteLine("Email is invalid");
                return;
            }
            CustomerRepository customer = new CustomerRepository();
            CustomerModel customerInfo = customer.getCustomerInfo(email);
            if(customerInfo == null)
            {
                Console.WriteLine("Email does not exist");
                return;
            }
            else
            {
                string query = "DELETE FROM customer where customer_email = @Email";
                MySqlCommand command = new MySqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();
            }

        }
        public void updateCustomer(string firstname, string lastname, string email, string password, string target_email)
        {
            //UPDATE tablename SET value = @value, value = @value where customer_email = @email
            string query = "UPDATE customer SET customer_firstname = @first_name, customer_lastname = @last_name," +
                "customer_email = @email, customer_password = @password where customer_email = @target_email";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@target_email", target_email);
            command.Parameters.AddWithValue("@first_name", firstname);
            command.Parameters.AddWithValue("@last_name", lastname);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
        }
    }
}
