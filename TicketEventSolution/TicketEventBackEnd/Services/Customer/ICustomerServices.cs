namespace TicketEventBackEnd.Services.Customer
{
    public interface ICustomerServices
    {
       Task<bool> validateCustomerLogin(string email, string password);
   
    }
}
