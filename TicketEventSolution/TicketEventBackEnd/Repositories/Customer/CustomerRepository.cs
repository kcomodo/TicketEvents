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
            string connectionString = "server=127.0.0.1;database=ticketevent;userid=root;password=c9nbQ5yMX2E9WVW;port=3306";
                
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
            string query = "INSERT INTO customer (customer_firstname, customer_lastname, customer_email, customer_password) VALUES (@FirstName, @LastName, @Email, @Password)";
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
            CustomerRepository customer = new CustomerRepository();
            Task<CustomerModel> customerInfo = customer.getCustomerInfo(customer_email);
            if(customerInfo == null)
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
        public void updateCustomer(string customer_firstname, string customer_lastname, string customer_email, string customer_password, string target_email)
        {
            //UPDATE tablename SET value = @value, value = @value where customer_email = @email
            string query = "UPDATE customer SET customer_firstname = @first_name, customer_lastname = @last_name," +
                "customer_email = @email, customer_password = @password where customer_email = @target_email";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@target_email", target_email);
            command.Parameters.AddWithValue("@first_name", customer_firstname);
            command.Parameters.AddWithValue("@last_name", customer_lastname);
            command.Parameters.AddWithValue("@email", customer_email);
            command.Parameters.AddWithValue("@password", customer_password);
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
    }
}
