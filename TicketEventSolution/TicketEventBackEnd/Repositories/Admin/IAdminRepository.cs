using TicketEventBackEnd.Models.Admin;
namespace TicketEventBackEnd.Repositories.Admin;

public interface IAdminRepository
{
    IEnumerable<AdminModel> getAllAdmin();
    AdminModel getAdminInfo(string email);
    void addAdmin(string email, string password);
    void deleteAdmin(string email);
    void updateAdmin(string email, string password, string target);
}
