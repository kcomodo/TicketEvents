namespace TicketEventBackEnd.Models.Customer
{
    public class CustomerModel
    {
        public int customer_id { get; set; }
        public string customer_firstname { get; set; }
        public string customer_lastname { get; set; }
        public string customer_email { get; set; }
        public string customer_password { get; set; }
        public string feed_token { get; set; }
    }
}
