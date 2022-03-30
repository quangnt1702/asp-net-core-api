using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Second_Project.Helpers;
using Second_Project.Models;
using Second_Project.RequestModels;
using Second_Project.ViewModels;

namespace Second_Project.Services
{
    public interface IAuthenService
    {
        LoginUserViewModel Authenticate(LoginUserRequestModel loginUserRequestModel);
        string GenerateToken(User user);
        LoginUserViewModel EncodeToken(String token);
    }

    public class AuthenService : IAuthenService
    {
        private readonly ProductDBContext _productDbContext;
        private readonly AppSettings _appSettings;

        public AuthenService(ProductDBContext productDbContext, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _productDbContext = productDbContext;
            _appSettings = optionsMonitor.CurrentValue;
        }

        public LoginUserViewModel Authenticate(LoginUserRequestModel loginUserRequestModel)
        {
            var loginUser = _productDbContext.Users.SingleOrDefault(u =>
                u.UserName == loginUserRequestModel.UserName && u.Password == loginUserRequestModel.Password);
            if (loginUser == null)
            {
                throw new AuthenticationException("Invalid account or password");
            }
            var token = GenerateToken(loginUser);
            
            return new LoginUserViewModel()
            {
                UserName = loginUser.UserName,
                AccessToken = token
            };
        }

        public string GenerateToken(User user)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName", user.UserName),
                    new Claim(ClaimTypes.Role ,user.Role),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                    
                }),
                Expires = DateTime.Now.AddHours(1.0),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescription);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public LoginUserViewModel EncodeToken(String token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenRead = handler.ReadJwtToken(token);
            Claim claim = tokenRead.Claims.SingleOrDefault(c=>c.Type=="UserName");
            return new LoginUserViewModel()
            {
                UserName = claim.Value
            };
        }
    }
}