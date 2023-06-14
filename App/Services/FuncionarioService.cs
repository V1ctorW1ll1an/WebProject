using App.Dto;
using App.Entities;
using App.Repositories.Interfaces;
using App.Services.Results;
using App.Services.Interfaces;

namespace App.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _FuncionarioRepository;
        private readonly ICargoService _cargoService;
        private readonly ICryptoService _cryptoService;

        public FuncionarioService(
            IFuncionarioRepository FuncionarioRepository,
            ICargoService cargoService,
            ICryptoService cryptoService
        )
        {
            _FuncionarioRepository = FuncionarioRepository;
            _cargoService = cargoService;
            _cryptoService = cryptoService;
        }

        public async Task<ServiceResult<FuncionarioEntity>> AtualizarFuncionarioAsync(
            AtualizarFuncionario funcionarioInput
        )
        {
            var funcionario = await _FuncionarioRepository.BuscarFuncionarioPeloIdAsync(
                funcionarioInput.Id
            );
            if (funcionario is null)
                return ServiceResult<FuncionarioEntity>.Failure(
                    "O Funcionário informado não existe em nosso sistema"
                );

            var cargo = await _cargoService.BuscarCargoPeloIdAsync(funcionarioInput.CargoId);
            if (cargo is null)
                return ServiceResult<FuncionarioEntity>.Failure(
                    "Não foi possível encontrar o cargo informado"
                );

            funcionario.Value.Cargo = cargo.Value;
            funcionario.Value.Nome = funcionarioInput.Nome;
            var funcionarioAtualizado =
                await _FuncionarioRepository.AtualizarDadosDoFuncionarioAsync(funcionario.Value);

            if (funcionarioAtualizado is null)
                return ServiceResult<FuncionarioEntity>.Failure(
                    "Não foi possível atualizar os dados do funcionário"
                );

            return ServiceResult<FuncionarioEntity>.Success(
                new FuncionarioEntity(
                    id: funcionario.Value.Id,
                    nome: funcionario.Value.Nome,
                    email: funcionario.Value.Email,
                    senha: null,
                    cargo: funcionario.Value.Cargo
                )
            );
        }

        public async Task<ServiceResult<FuncionarioEntity>> AutenticarFuncionarioAsync(
            LoginFuncionario funcionarioInput
        )
        {
            var funcionario = await _FuncionarioRepository.BuscarFuncionarioPeloEmailAsync(
                funcionarioInput.Email
            );

            if (funcionario is null || funcionario.Value.Senha is null)
                return ServiceResult<FuncionarioEntity>.Failure("Usuário ou senha inválidos");

            var senhaValida = _cryptoService.VerifyPassword(
                funcionarioInput.Senha,
                funcionario.Value.Senha
            );

            if (!senhaValida.IsSuccess)
                return ServiceResult<FuncionarioEntity>.Failure("Usuário ou senha inválidos");

            return ServiceResult<FuncionarioEntity>.Success(
                new FuncionarioEntity(
                    id: funcionario.Value.Id,
                    nome: funcionario.Value.Nome,
                    email: funcionario.Value.Email,
                    senha: null,
                    cargo: funcionario.Value.Cargo
                )
            );
        }

        public async Task<ServiceResult<FuncionarioEntity>> CadastrarFuncionarioAsync(
            CadastrarFuncionario funcionarioInput
        )
        {
            var funcionarioExistente = await _FuncionarioRepository.BuscarFuncionarioPeloEmailAsync(
                funcionarioInput.Email
            );
            if (funcionarioExistente.Value is not null)
                return ServiceResult<FuncionarioEntity>.Failure(
                    "Este e-mail já está cadastrado no sistema"
                );

            var cargo = await _cargoService.BuscarCargoPeloIdAsync(funcionarioInput.CargoId);
            if (cargo.Value is null)
                return ServiceResult<FuncionarioEntity>.Failure(
                    "Não foi possível encontrar o cargo informado"
                );

            var novoFuncionario = new FuncionarioEntity(
                id: null,
                nome: funcionarioInput.Nome,
                email: funcionarioInput.Email,
                senha: _cryptoService.HashPassword(funcionarioInput.Senha).Value,
                cargo: cargo.Value
            );

            var funcionarioCadastrado = await _FuncionarioRepository.CadastrarFuncionarioAsync(
                novoFuncionario
            );

            if (!funcionarioCadastrado.IsSuccess)
                return ServiceResult<FuncionarioEntity>.Failure(
                    "Não foi possível cadastrar o funcionário no banco de dados"
                );

            return ServiceResult<FuncionarioEntity>.Success(
                new FuncionarioEntity(
                    id: funcionarioCadastrado.Value.Id,
                    nome: funcionarioCadastrado.Value.Nome,
                    email: funcionarioCadastrado.Value.Email,
                    senha: null,
                    cargo: funcionarioCadastrado.Value.Cargo
                )
            );
        }
    }
}
