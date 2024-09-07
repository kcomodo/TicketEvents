
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicketEventBackEnd.Repositories;
using TicketEventBackEnd.Repositories.Customer;
using TicketEventBackEnd.Services.Admin;
using TicketEventBackEnd.Repositories.Admin;
namespace TicketEventWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
            
        }


        private void adminLogin(object sender, RoutedEventArgs e)
        {
            string email = EmailInfo.Text;
            string password = PasswordInfo.Text;
            //create an instance of the repository to use the service
            IAdminRepository adminRepository = new AdminRepository();
            IAdminServices adminServices = new AdminServices(adminRepository);
            bool validation = adminServices.validateAdminLogin(email,password);
            testBlock.Text = validation.ToString();
            if (validation == true)
            {
                Home home = new Home();
                home.Show();
            }
            else
            {
                MessageBox.Show("Login failed. Invalid email or password.");
            }

        }
    }
}