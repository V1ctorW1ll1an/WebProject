using App.Entities;

namespace App.Services.Interfaces;

public interface ITokenService
{
    public string GenerateToken(FuncionarioEntity funcionario);
}
