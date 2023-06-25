using App.Dto;
using App.Models;
using App.Services.Results;

namespace App.Services.Interfaces;

public interface IFuncionarioService
{
    public Task<ServiceResult<Funcionario>> AutenticarFuncionarioAsync(
        LoginFuncionario funcionarioInput
    );
    public Task<ServiceResult<Funcionario>> CadastrarFuncionarioAsync(Funcionario funcionarioInput);
    public Task<ServiceResult<Funcionario>> AtualizarFuncionarioAsync(
        AtualizarFuncionario funcionarioInput
    );
    public Task<ServiceResult<Funcionario>> DesativarFuncionarioAsync(int id);

    public Task<ServiceResult<Funcionario>> ObterFuncionarioAsync(int id);

    public Task<ServiceResult<IEnumerable<Funcionario>>> ObterFuncionariosAsync();

    public Task<ServiceResult<IEnumerable<Funcionario>>> ObterFuncionariosDesativadosAsync();
}
