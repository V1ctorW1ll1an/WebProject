namespace App.Entities;

public class FuncionarioEntity
{
    public FuncionarioEntity() { }

    public FuncionarioEntity(
        int? id,
        string nome,
        string email,
        int? cpf,
        string? senha,
        CargoEntity cargo
    )
    {
        Id = id;
        Nome = nome;
        Email = email;
        Cpf = cpf;
        Senha = senha;
        Cargo = cargo;
    }

    public int? Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int? Cpf { get; set; }
    public string? Senha { get; set; }
    public CargoEntity Cargo { get; set; } = null!;
}
