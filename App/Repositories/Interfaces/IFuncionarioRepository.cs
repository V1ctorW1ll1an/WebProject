using App.Entities;

namespace App.Repositories.Interfaces;

public interface IFuncionarioRepository
{
    public Task<Funcionario> BuscarFuncionarioPeloEmailESenha(string email, string senha);
}
