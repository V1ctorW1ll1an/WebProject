using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using App.Entities;
using App.Services.Interfaces;

namespace App.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(FuncionarioEntity funcionario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, funcionario.Nome.ToString()), // User.Identity.Name
                    new Claim(ClaimTypes.Role, funcionario.Cargo.Nome.ToString()) // User.IsInRole("1")
                }
            ),
            Expires = DateTime.UtcNow.AddHours(40),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
