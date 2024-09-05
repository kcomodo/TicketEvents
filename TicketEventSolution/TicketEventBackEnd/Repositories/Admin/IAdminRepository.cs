using TicketEventBackEnd.Models.Admin;
namespace TicketEventBackEnd.Repositories.Admin;

public interface IAdminRepository
{
    IEnumerable<AdminModel> GetAdminModels();
    AdminModel getAdminInfo();
}
