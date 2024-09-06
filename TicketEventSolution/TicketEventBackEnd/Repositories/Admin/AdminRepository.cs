using MySql.Data.MySqlClient;
using TicketEventBackEnd.Models.Admin;
namespace TicketEventBackEnd.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MySqlConnection _connection;
        public AdminRepository()
        {
            string connectionString = "server=127.0.0.1;database=ticketevent;userid=root;port=3307";
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
        public void addAdmin()
        {

        }
        public void deleteAdmin()
        {

        }
    }
}


