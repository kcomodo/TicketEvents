namespace TicketEventBackEnd.Services.Customer
{
    public interface ICustomerServices
    {
        bool validateCustomerLogin(string email, string password);
    }
}
