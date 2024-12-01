using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TicketEventBackEnd.Models.Admin;
namespace TicketEventBackEnd.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MySqlConnection _connection;
        public AdminRepository()
        {
            string connectionString = "server=127.0.0.1;database=ticketevent;userid=root;port=3306";
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }
        ~AdminRepository()
        {
            _connection.Close();
        }
        public IEnumerable<AdminModel> getAllAdmin()
        {

            List<AdminModel> adminInfo = new List<AdminModel>();
            
            string query = "SELECT * FROM admin";
            MySqlCommand _command = new MySqlCommand(query, _connection);
            MySqlDataReader reader = _command.ExecuteReader();
            if (reader.Read())
            {
                adminInfo.Add(new AdminModel
                {
                    admin_id = reader.GetInt32("admin_id"),
                    admin_email = reader.GetString("admin_email"),
                    admin_password = reader.GetString("admin_password")
                });
                reader.Close();
                return adminInfo;
            }
            return null;
        }
        public AdminModel getAdminInfo(string email)
        {
            string query = "SELECT * FROM admin where admin_email = @email";
            MySqlCommand _command = new MySqlCommand(query, _connection);
            _command.Parameters.AddWithValue("@email", email);
            MySqlDataReader reader = _command.ExecuteReader();
            if (reader.Read())
            {
                AdminModel admin = new AdminModel
                {
                    admin_id = reader.GetInt32("admin_id"),
                    admin_email = reader.GetString("admin_email"),
                    admin_password = reader.GetString("admin_password"),
                };
                reader.Close();
                return admin;
            }
            return null;
        }
        public void addAdmin(string email, string password)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (emailRegex.IsMatch(email))
            {
                string query = "INSERT INTO admin (admin_email,admin_password) VALUES (@email, @password)";
                MySqlCommand _command = new MySqlCommand(query, _connection);
                _command.Parameters.AddWithValue("@email", email);
                _command.Parameters.AddWithValue("@password", password);
                _command.ExecuteNonQuery();
            }
            else
            {
                throw new ArgumentException("Invalid email format");
            }
           

        }
        public void deleteAdmin(string email)
        {
            string query = "DELETE FROM admin where admin_email = @email";
            MySqlCommand _command = new MySqlCommand(query, _connection);
            _command.Parameters.AddWithValue("@email", email);
            _command.ExecuteNonQuery();
        }
        public void updateAdmin(string email, string password, string target)
        {
            string query = "UPDATE admin SET admin_email = @email, admin_password = @password WHERE admin_email = @target";
            MySqlCommand _command = new MySqlCommand(query, _connection);
            _command.Parameters.AddWithValue("@target", target);
            _command.ExecuteNonQuery();
        }
    }
}


