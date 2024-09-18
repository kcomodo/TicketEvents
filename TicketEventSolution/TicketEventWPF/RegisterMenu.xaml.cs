using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicketEventBackEnd.Repositories.Admin;
using TicketEventBackEnd.Services.Admin;
using TicketEventBackEnd.Models.Admin;
namespace TicketEventWPF
{
    /// <summary>
    /// Interaction logic for RegisterMenu.xaml
    /// </summary>
    public partial class RegisterMenu : Window
    {
        public RegisterMenu()
        {
            InitializeComponent();
        }

        private void RegisterAdmin(object sender, RoutedEventArgs e)
        {
            string email = emailBox.Text;
            string password = passwordBox.Text;
            IAdminRepository adminRepository = new AdminRepository();
            IAdminServices adminServices = new AdminServices(adminRepository);
            adminServices.registerAdmin(email, password);
            this.Hide();
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
