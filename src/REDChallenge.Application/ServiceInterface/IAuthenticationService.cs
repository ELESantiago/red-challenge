using REDChallenge.Application.Models;

namespace REDChallenge.Application.ServiceInterface
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginModel model);
        Task<LoginResponse> SignUp(SignUpModel model);
    }
}
