using App.Dto;
using App.Entities;
using App.Repositories.Interfaces;
using App.Services.Exceptions;
using App.Services.Interfaces;

namespace App.Services;

public class FuncionarioService : IFuncionarioService
{
    private readonly IFuncionarioRepository _userRepository;
    private readonly ICargoService _cargoService;

    public FuncionarioService(IFuncionarioRepository userRepository, ICargoService cargoService)
    {
        _userRepository = userRepository;
        _cargoService = cargoService;
    }

    public async Task<Funcionario> AtualizarFuncionario(AtualizarFuncionario funcionarioInput)
    {
        var funcionario = await _userRepository.BuscarFuncionarioPeloId(funcionarioInput.Id);
        if (funcionario == null)
            throw new FuncionarioNaoEncontrado(
                "O Funcionário informado não existe em nosso sistema"
            );

        var cargo = await _cargoService.BuscarCargoPeloId(funcionarioInput.CargoId);
        if (cargo == null)
            throw new CargoNaoEncontrado("Não foi possível encontrar o cargo informado");

        funcionario.Cargo = cargo;
        funcionario.Nome = funcionarioInput.Nome;
        var funcionarioAtualizado = await _userRepository.AtualizarDadosDoFuncionario(funcionario);

        return new Funcionario(
            id: funcionarioAtualizado.Id,
            nome: funcionarioAtualizado.Nome,
            email: funcionarioAtualizado.Email,
            senha: null,
            cargo: funcionarioAtualizado.Cargo
        );
    }

    public async Task<Funcionario> AutenticarFuncionario(LoginFuncionario funcionarioInput)
    {
        var funcionario = await _userRepository.BuscarFuncionarioPeloEmailESenha(
            funcionarioInput.Email,
            funcionarioInput.Senha
        );

        if (funcionario == null)
            throw new FuncionarioNaoEncontrado(
                "Não foi possível encontrar um funcionário com o e-mail e senha informados"
            );

        return funcionario;
    }

    public async Task<Funcionario> CadastrarFuncionario(CadastrarFuncionario funcionarioInput)
    {
        var funcionarioExistente = await _userRepository.BuscarFuncionarioPeloEmail(
            funcionarioInput.Email
        );
        if (funcionarioExistente != null)
            throw new EmailJaCadastrado("Este e-mail já está cadastrado no sistema");

        var cargo = await _cargoService.BuscarCargoPeloId(funcionarioInput.CargoId);
        if (cargo == null)
            throw new CargoNaoEncontrado("Não foi possível encontrar o cargo informado");

        var novoFuncionario = new Funcionario(
            id: null,
            nome: funcionarioInput.Nome,
            email: funcionarioInput.Email,
            senha: funcionarioInput.Senha,
            cargo: cargo
        );

        // TODO: Implement password hashing in other PR
        var funcionarioCadastrado = await _userRepository.CadastrarFuncionario(novoFuncionario);
        // TODO: This is a repository responsibility
        if (funcionarioCadastrado == null)
            throw new ErroCadastrarFuncionarioDB(
                "Não foi possível cadastrar o funcionário no banco de dados"
            );

        return new Funcionario(
            id: funcionarioCadastrado.Id,
            nome: funcionarioCadastrado.Nome,
            email: funcionarioCadastrado.Email,
            senha: null,
            cargo: funcionarioCadastrado.Cargo
        );
    }
}
