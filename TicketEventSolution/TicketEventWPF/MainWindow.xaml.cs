using MVC_DataAccess.Repositories.Admin;
using MVC_DataAccess.Services.Admin;
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
        IAdminRepository adminRepository = new AdminRepository();
        IAdminServices adminServices = new AdminServices();
        public MainWindow(IAdminServices _adminServices, IAdminRepository _adminRepository)
        {
            adminRepository = _adminRepository;
            adminServices = _adminServices;
            InitializeComponent();
        }
        public void adminLogin()
        {
            string email = EmailInfo.Text;
            string password = PasswordInfo.Text;

        }
    }
}