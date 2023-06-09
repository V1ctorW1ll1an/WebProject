using App.Entities;
using App.Repositories.Interfaces;

namespace App.Repositories;

public class FuncionarioRepository : IFuncionarioRepository
{
  // TODO create a database and replace this list with a real database
  public static List<Cargo> cargos = new List<Cargo>
    {
        new Cargo(1, "Admin"),
        new Cargo(2, "Operator"),
        new Cargo(3, "CommonEmployee"),
    };
  public static List<Funcionario> Funcionarios = new List<Funcionario>
    {
        new Funcionario(1, "John Doe", "john@teste.com", "123", cargos[0]),
        new Funcionario(2, "teste", "teste@teste.com", "123", cargos[1]),
        new Funcionario(3, "teste2", "teste2@teste.com", "123", cargos[2]),
    };

  public Task<Funcionario> BuscarFuncionarioPeloEmail(string email)
  {
    return Task.FromResult<Funcionario>(Funcionarios.FirstOrDefault(x => x.Email == email));
  }

  public Task<Funcionario> BuscarFuncionarioPeloEmailESenha(string email, string senha)
  {
    return Task.FromResult<Funcionario>(Funcionarios.FirstOrDefault(x => x.Email == email && x.Senha == senha));
  }

  public Task<Funcionario> CadastrarFuncionario(Funcionario funcionario)
  {
    Funcionarios.Add(funcionario);
    return Task.FromResult(funcionario ?? null);
  }
}
