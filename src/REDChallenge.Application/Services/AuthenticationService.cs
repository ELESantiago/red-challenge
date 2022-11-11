using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using REDChallenge.Application.Models;
using REDChallenge.Application.ServiceInterface;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Exceptions;
using REDChallenge.Domain.Repository;

namespace REDChallenge.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        public AuthenticationService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }
        public async Task<LoginResponse> Login(LoginModel model)
        {
            var user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null ||
                user.Password != HashPassword(model.Password, SaltFromString(user.Salt))
            )
                throw new InvalidLoginAttemptException($"Invalid credentials for {model.Username}");

            return new LoginResponse
            {
                Id = user.Id,
                Username = user.Name,
                Token = GenerateJwt(user)
            };
        }

        public async Task<LoginResponse> SignUp(SignUpModel model)
        {
            byte[] salt = GenerateSalt();
            var password = HashPassword(model.Password, salt);
            var alreadyExistentUser = await _userRepository.GetUserByUsername(model.Username);
            if (alreadyExistentUser != null)
                throw new Exception("User already exists");
            var user = await _userRepository.CreateAsync(new User
            {
                Name = model.Username,
                Password = password,
                Salt = SaltAsString(salt)
            });
            return new LoginResponse
            {
                Id = user.Id,
                Username = user.Name,
                Token = GenerateJwt(user)
            };
        }

        #region Utils
        private string GenerateJwt(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim("sub", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = "my-issuer",
                Audience = "Test",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string SaltAsString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }

        private byte[] SaltFromString(string salt)
        {
            return Convert.FromBase64String(salt);
        }
        #endregion

    }
}
