namespace TicketEventBackEnd.Services.Admin;

public interface IAdminServices
{
    bool validateAdminLogin(string email, string password);
}
