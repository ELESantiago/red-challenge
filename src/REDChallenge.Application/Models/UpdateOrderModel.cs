
namespace REDChallenge.Application.Models
{
    public class UpdateOrderModel
    {
        public Guid Id { get; set; }
        public int OrderTypeId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
