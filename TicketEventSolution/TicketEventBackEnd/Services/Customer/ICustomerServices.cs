namespace TicketEventBackEnd.Services.Customer
{
    public interface ICustomerServices
    {
       Task<bool> validateCustomerLogin(string email, string password);
       void saveRouteInfo(string customer_id, string routeId, double routeLat, double routeLng);
    }
}
