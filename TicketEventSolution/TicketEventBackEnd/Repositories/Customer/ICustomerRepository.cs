using TicketEventBackEnd.Models.Customer;
using TicketEventBackEnd.Models.Routes;

namespace TicketEventBackEnd.Repositories.Customer
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerModel>> getAllCustomer();
        Task<CustomerModel> getCustomerInfo(string customer_email);
        void addCustomer(CustomerModel customer);
        void deleteCustomer(string customer_email);
        void updateCustomer(string customer_firstname, string customer_lastname, string customer_email, string customer_password, string feed_token, string targetemail);
        void updateFeedToken(string customer_email, string feed_token);
        Task<CustomerModel> getFeedToken(string customer_email);
        void saveRouteInfo(string customer_id, string routeId, double latitude, double longitude);
        void deleteRouteInfo(string customer_id, string routeId);
        Task<List<routesModel>> getRouteInfo(string customer_id);
    }
}
