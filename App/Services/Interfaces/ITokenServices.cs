using App.Models;

namespace App.Services.Interfaces;

public interface ITokenService
{
    public string GenerateToken(Usuario usuario);
}
