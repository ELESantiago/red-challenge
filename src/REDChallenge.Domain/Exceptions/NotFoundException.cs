
namespace REDChallenge.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("Element not found exception")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
