using App.Dto;
using App.Entities;

namespace App.Services.Interfaces;

public interface IFuncionarioService
{
    public Task<Funcionario> AutenticarFuncionario(FuncionarioLoginInput funcionarioInput);
}
