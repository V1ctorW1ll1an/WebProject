using App.Entities;
using App.Repositories.Results;

namespace App.Repositories.Interfaces;

public interface IFuncionarioRepository
{
    public Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloEmail(string email);
    public Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloEmailESenha(
        string email,
        string senha
    );
    public Task<DbResult<FuncionarioEntity>> CadastrarFuncionario(FuncionarioEntity funcionario);
    public Task<DbResult<FuncionarioEntity>> AtualizarDadosDoFuncionario(
        FuncionarioEntity funcionario
    );
    public Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloId(int id);
}
