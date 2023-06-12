using System.Text;
using App.Services.Interfaces;
using App.Services.Results;
using Konscious.Security.Cryptography;

namespace App.Services;

public class Argon2Service : ICryptoService
{
    private readonly byte[] salt = Encoding.UTF8.GetBytes("salt"); // change this to a random salt, this is just an example
    private readonly byte[] userUuidBytes = Encoding.UTF8.GetBytes("userUuid"); // change this to the user's uuid, same as above

    public ServiceResult<string> HashPassword(string password)
    {
        try
        {
            var byteArray = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2i(byteArray);

            argon2.DegreeOfParallelism = 16;
            argon2.MemorySize = 8192;
            argon2.Iterations = 40;
            argon2.Salt = salt;
            argon2.AssociatedData = userUuidBytes;

            var hashBytes = argon2.GetBytes(32); // or 128 bytes
            var hash = Convert.ToBase64String(hashBytes);

            return ServiceResult<string>.Success(hash);
        }
        catch (Exception)
        {
            return ServiceResult<string>.Failure("Erro ao gerar hash da senha");
        }
    }

    public ServiceResult<bool> VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            var newHashedPassword = HashPassword(password);

            if (newHashedPassword.Value.SequenceEqual(hashedPassword))
                return ServiceResult<bool>.Success(true);

            return ServiceResult<bool>.Failure("Usuário ou senha inválidos");
        }
        catch (Exception)
        {
            return ServiceResult<bool>.Failure("Erro ao verificar senha");
        }
    }
}
