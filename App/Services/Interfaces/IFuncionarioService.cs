using App.Dto;
using App.Entities;

namespace App.Services.Interfaces;

public interface IFuncionarioService
{
    public Task<Funcionario> AutenticarFuncionario(LoginFuncionario funcionarioInput);
    public Task<Funcionario> CadastrarFuncionario(CadastrarFuncionario funcionarioInput);
    public Task<Funcionario> AtualizarFuncionario(AtualizarFuncionario funcionarioInput);
}
