using TicketEventBackEnd.Models.Admin;
using TicketEventBackEnd.Repositories.Admin;
namespace TicketEventBackEnd.Services.Admin {
    public class AdminServices : IAdminServices
    {
        private readonly IAdminRepository _adminRepository;
        public AdminServices(IAdminRepository adminRepository) {
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));

        }

        public bool validateAdminLogin(string email, string password)
        {
            AdminModel adminModel = _adminRepository.getAdminInfo(email);
            if(adminModel == null)
            {
                return false;
            }
            if(email == adminModel.admin_email && password == adminModel.admin_password) 
            {
                return true;
            }
            return false;
        }
        public void registerAdmin(string email, string password) 
        {
            _adminRepository.addAdmin(email, password);
        }
        public void removeCustomer(string email)
        {


        }
        public void editCustomer(string customer_firstname, string customer_lastname, string customer_email, string customer_password)
        {

        }

    }
}