using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using App.Services.Interfaces;
using App.Models;

namespace App.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nome.ToString()), // User.Identity.Name
                    new Claim(ClaimTypes.Role, usuario.NivelDeAcesso.ToString()) // User.IsInRole("1")
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
