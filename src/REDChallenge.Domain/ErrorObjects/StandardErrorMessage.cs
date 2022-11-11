
namespace REDChallenge.Domain.ErrorObjects
{
    public class StandardErrorMessage
    {
        public string Title { get; set; } = string.Empty;
        public IEnumerable<InvalidParameter> InvalidParams { get; set; } = new List<InvalidParameter>();
    }
}
