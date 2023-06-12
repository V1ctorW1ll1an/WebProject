using App.Dto;
using App.Entities;
using App.Services.Results;

namespace App.Services.Interfaces;

public interface IFuncionarioService
{
    public Task<ServiceResult<FuncionarioEntity>> AutenticarFuncionario(
        LoginFuncionario funcionarioInput
    );
    public Task<ServiceResult<FuncionarioEntity>> CadastrarFuncionario(
        CadastrarFuncionario funcionarioInput
    );
    public Task<ServiceResult<FuncionarioEntity>> AtualizarFuncionario(
        AtualizarFuncionario funcionarioInput
    );
}
