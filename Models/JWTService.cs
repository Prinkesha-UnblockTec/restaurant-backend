using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace restaurant.Models
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;

        public string SecretKey { get; set; }
        public int TokenDuration { get; set; }

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
            this.SecretKey = _configuration.GetSection("jwtConfig").GetSection("Key").Value;
            this.TokenDuration = Int32.Parse(_configuration.GetSection("jwtConfig").GetSection("Duration").Value);
        }


        public string GenerateToken(TokenGenerationRequest user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var payload = new[]
            {
                new Claim("UserName",user.UserName),
                new Claim("Password",user.Password),
            };
            var jwttoken = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: payload,
            expires: DateTime.Now.AddMinutes(TokenDuration),
            signingCredentials: signature
            );


            return new JwtSecurityTokenHandler().WriteToken(jwttoken);

        }
    }
}
