using App.Services.Results;

namespace App.Services.Interfaces;

public interface ICryptoService
{
    ServiceResult<string> HashPassword(string password);
    ServiceResult<bool> VerifyPassword(string password, string hashedPassword);
}
