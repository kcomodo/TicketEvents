using System.ComponentModel.DataAnnotations;

namespace TicketEventBackEnd.Models.Routes
{
    public class routesModel
    {
        public int savedRoutesId { get; set; }
        [Required]
        public int customer_Id { get; set; }
        [Required]
        public int routes_Id { get; set; }
        [Required]
        public double latitude { get; set; }
        [Required]
        public double longitude { get; set; }
    }
}
