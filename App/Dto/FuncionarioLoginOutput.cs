using App.Entities;

namespace App.Dto;

public class FuncionarioLoginOutput
{
    public FuncionarioLoginOutput(int id, string nome, string email, Cargo cargo, string token)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Cargo = cargo;
        Token = token;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public Cargo Cargo { get; set; }

    public string Token { get; set; }
}
