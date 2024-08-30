namespace MVC_DataAccess.Services.Customer
{
    public interface ICustomerServices
    {
        bool validateCustomerLogin(string email, string password);
    }
}
