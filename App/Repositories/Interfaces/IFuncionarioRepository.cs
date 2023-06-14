using App.Entities;
using App.Repositories.Results;

namespace App.Repositories.Interfaces;

public interface IFuncionarioRepository
{
    public Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloEmailAsync(string email);
    public Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloEmailESenhaAsync(
        string email,
        string senha
    );
    public Task<DbResult<FuncionarioEntity>> CadastrarFuncionarioAsync(
        FuncionarioEntity funcionario
    );
    public Task<DbResult<FuncionarioEntity>> AtualizarDadosDoFuncionarioAsync(
        FuncionarioEntity funcionario
    );
    public Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloIdAsync(int id);
}
