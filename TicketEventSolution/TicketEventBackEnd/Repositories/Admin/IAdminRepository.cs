using TicketEventBackEnd.Models.Admin;
namespace TicketEventBackEnd.Repositories.Admin;

public interface IAdminRepository
{
    IEnumerable<AdminModel> getAllAdmin();
    AdminModel getAdminInfo(string email);
    void addAdmin();
    void deleteAdmin();
}
