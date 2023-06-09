namespace App.Entities;

public class Funcionario
{
  public Funcionario(int? id, string nome, string email, string? senha, Cargo cargo)
  {
    Id = id;
    Nome = nome;
    Email = email;
    Senha = senha;
    Cargo = cargo;
  }

  public int? Id { get; set; }
  public string Nome { get; set; }
  public string Email { get; set; }
  public string? Senha { get; set; }
  public Cargo Cargo { get; set; }
}
