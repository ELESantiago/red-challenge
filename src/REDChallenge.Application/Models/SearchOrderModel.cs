
namespace REDChallenge.Application.Models
{
    public class SearchOrderModel
    {
        public int? OrderType { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        
    }
}
