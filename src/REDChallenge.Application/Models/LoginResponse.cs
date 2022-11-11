
namespace REDChallenge.Application.Models
{
    public class LoginResponse
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
