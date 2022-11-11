using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REDChallenge.Application.Models;
using REDChallenge.Application.ServiceInterface;

namespace REDChallenge.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<LoginResponse>> Login(LoginModel login)
        {
            return Ok(await _authService.Login(login));
        }

        [HttpPost("sign-up")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<LoginResponse>> CreateAccount(SignUpModel signUp)
        {
            return Ok(await _authService.SignUp(signUp));
        }
    }
}
