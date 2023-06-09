using App.Entities;

namespace App.Repositories.Interfaces;

public interface IFuncionarioRepository
{
  public Task<Funcionario> BuscarFuncionarioPeloEmail(string email);
  public Task<Funcionario> BuscarFuncionarioPeloEmailESenha(string email, string senha);
  public Task<Funcionario> CadastrarFuncionario(Funcionario funcionario);
}
