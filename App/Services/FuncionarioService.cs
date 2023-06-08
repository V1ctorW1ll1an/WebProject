using App.Dto;
using App.Entities;
using App.Repositories.Interfaces;
using App.Services.Interfaces;

namespace App.Services;

public class FuncionarioService : IFuncionarioService
{
    private readonly IFuncionarioRepository _userRepository;

    public FuncionarioService(IFuncionarioRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Funcionario> AutenticarFuncionario(FuncionarioLoginInput funcionarioInput)
    {
        var funcionario = await _userRepository.BuscarFuncionarioPeloEmailESenha(
            funcionarioInput.Email,
            funcionarioInput.Senha
        );

        return funcionario;
    }
}
