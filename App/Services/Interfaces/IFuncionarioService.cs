using App.Dto;
using App.Entities;
using App.Services.Results;

namespace App.Services.Interfaces;

public interface IFuncionarioService
{
    public Task<ServiceResult<FuncionarioEntity>> AutenticarFuncionarioAsync(
        LoginFuncionario funcionarioInput
    );
    public Task<ServiceResult<FuncionarioEntity>> CadastrarFuncionarioAsync(
        CadastrarFuncionario funcionarioInput
    );
    public Task<ServiceResult<FuncionarioEntity>> AtualizarFuncionarioAsync(
        AtualizarFuncionario funcionarioInput
    );
}
