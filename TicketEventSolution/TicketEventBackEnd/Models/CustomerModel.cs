using System.ComponentModel.DataAnnotations;

namespace TicketEventBackEnd.Models.Customer
{
    public class CustomerModel
    {
        public int customer_id { get; set; }
        [Required]
        public string customer_firstname { get; set; }
        [Required]
        public string customer_lastname { get; set; }
        [Required]
        public string customer_email { get; set; }
        [Required]
        public string customer_password { get; set; }
        public string feed_token { get; set; }
    }
}
