
namespace REDChallenge.Application.Models
{
    public class CreateOrderModel
    {
        public int OrderTypeId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
