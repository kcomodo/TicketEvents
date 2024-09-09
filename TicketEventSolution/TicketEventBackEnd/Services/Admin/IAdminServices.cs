namespace TicketEventBackEnd.Services.Admin;

public interface IAdminServices
{
    bool validateAdminLogin(string email, string password);
    void registerAdmin(string email, string password);
    void removeCustomer(string email);
    void editCustomer(string customer_firstname, string customer_lastname, string customer_email, string customer_password);
}
