
namespace REDChallenge.Application.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedByUsername { get; set; } = string.Empty;
        public Guid CreatedById { get; set; }

    }
}
