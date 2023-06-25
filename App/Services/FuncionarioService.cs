using App.Dto;
using App.Services.Results;
using App.Services.Interfaces;
using App.Models;

namespace App.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly ICryptoService _cryptoService;
        private readonly DataBaseContext _dataBaseContext;

        public FuncionarioService(ICryptoService cryptoService, DataBaseContext dataBaseContext)
        {
            _cryptoService = cryptoService;
            _dataBaseContext = dataBaseContext;
        }

        public async Task<ServiceResult<Funcionario>> AtualizarFuncionarioAsync(
            Funcionario funcionarioInput,
            int id
        )
        {
            // checked if id is empty
            if (id == 0)
                return ServiceResult<Funcionario>.Failure(
                    "Por favor, informe o id do funcionário que deseja atualizar"
                );

            var funcionario = await _dataBaseContext.Funcionarios.FindAsync(id);

            if (funcionario is null || !funcionario.IsEnable)
                return ServiceResult<Funcionario>.Failure(
                    "O Funcionário informado não existe em nosso sistema"
                );

            var newHashedPassword = _cryptoService.HashPassword(funcionarioInput.Senha);

            funcionario.Nome = funcionarioInput.Nome;
            funcionario.Email = funcionarioInput.Email;
            funcionario.Cargo = funcionarioInput.Cargo;
            funcionario.Cpf = funcionarioInput.Cpf;
            funcionario.Senha = newHashedPassword.Value;

            _dataBaseContext.Funcionarios.Update(funcionario);
            await _dataBaseContext.SaveChangesAsync();

            return ServiceResult<Funcionario>.Success(
                new Funcionario()
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Email = funcionario.Email,
                    Cargo = funcionario.Cargo,
                    Cpf = funcionario.Cpf
                }
            );
        }

        public Task<ServiceResult<Funcionario>> AutenticarFuncionarioAsync(
            LoginFuncionario funcionarioInput
        )
        {
            var funcionario = _dataBaseContext.Funcionarios.FirstOrDefault(
                f => f.Email == funcionarioInput.Email && f.IsEnable
            );

            if (funcionario is null)
                return Task.FromResult(
                    ServiceResult<Funcionario>.Failure("Usuário ou senha inválidos")
                );

            var senhaValida = _cryptoService.VerifyPassword(
                funcionarioInput.Senha,
                funcionario.Senha
            );

            if (!senhaValida.IsSuccess)
                return Task.FromResult(
                    ServiceResult<Funcionario>.Failure("Usuário ou senha inválidos")
                );

            return Task.FromResult(
                ServiceResult<Funcionario>.Success(
                    new Funcionario()
                    {
                        Id = funcionario.Id,
                        Nome = funcionario.Nome,
                        Email = funcionario.Email,
                        Cargo = funcionario.Cargo,
                        Cpf = funcionario.Cpf
                    }
                )
            );
        }

        public async Task<ServiceResult<Funcionario>> CadastrarFuncionarioAsync(
            Funcionario funcionarioInput
        )
        {
            var funcionarioExistente = _dataBaseContext.Funcionarios.FirstOrDefault(
                f =>
                    f.Email == funcionarioInput.Email || f.Cpf == funcionarioInput.Cpf && f.IsEnable
            );

            if (funcionarioExistente is not null)
                return ServiceResult<Funcionario>.Failure(
                    "Este usuário já está cadastrado em nosso sistema"
                );

            var hashedPassword = _cryptoService.HashPassword(funcionarioInput.Senha);

            if (!hashedPassword.IsSuccess)
                return ServiceResult<Funcionario>.Failure(
                    "Não foi possível cadastrar o usuário devido a um erro no servidor, tente novamente mais tarde"
                );

            funcionarioInput.Senha = hashedPassword.Value;

            await _dataBaseContext.Funcionarios.AddAsync(funcionarioInput);

            await _dataBaseContext.SaveChangesAsync();

            return ServiceResult<Funcionario>.Success(
                new Funcionario()
                {
                    Id = funcionarioInput.Id,
                    Nome = funcionarioInput.Nome,
                    Email = funcionarioInput.Email,
                    Cargo = funcionarioInput.Cargo,
                    Cpf = funcionarioInput.Cpf
                }
            );
        }

        public async Task<ServiceResult<Funcionario>> DesativarFuncionarioAsync(int id)
        {
            var funcionario = await _dataBaseContext.Funcionarios.FindAsync(id);

            if (funcionario is null || !funcionario.IsEnable)
                return ServiceResult<Funcionario>.Failure(
                    "O Funcionário informado não existe em nosso sistema"
                );

            funcionario.IsEnable = false;

            _dataBaseContext.Funcionarios.Update(funcionario);
            await _dataBaseContext.SaveChangesAsync();

            return ServiceResult<Funcionario>.Success(
                new Funcionario()
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Email = funcionario.Email,
                    Cargo = funcionario.Cargo,
                    Cpf = funcionario.Cpf
                }
            );
        }

        public Task<ServiceResult<Funcionario>> ObterFuncionarioAsync(int id)
        {
            var funcionario = _dataBaseContext.Funcionarios.FirstOrDefault(
                f => f.Id == id && f.IsEnable
            );

            if (funcionario is null)
                return Task.FromResult(
                    ServiceResult<Funcionario>.Failure(
                        "O Funcionário informado não existe em nosso sistema"
                    )
                );

            return Task.FromResult(
                ServiceResult<Funcionario>.Success(
                    new Funcionario()
                    {
                        Id = funcionario.Id,
                        Nome = funcionario.Nome,
                        Email = funcionario.Email,
                        Cargo = funcionario.Cargo,
                        Cpf = funcionario.Cpf
                    }
                )
            );
        }

        public Task<ServiceResult<IEnumerable<Funcionario>>> ObterFuncionariosAsync()
        {
            var funcionarios = _dataBaseContext.Funcionarios.Where(f => f.IsEnable);

            if (!funcionarios.Any())
                return Task.FromResult(
                    ServiceResult<IEnumerable<Funcionario>>.Failure(
                        "Não existem funcionários cadastrados em nosso sistema"
                    )
                );

            return Task.FromResult(
                ServiceResult<IEnumerable<Funcionario>>.Success(
                    funcionarios.Select(
                        f =>
                            new Funcionario()
                            {
                                Id = f.Id,
                                Nome = f.Nome,
                                Email = f.Email,
                                Cargo = f.Cargo,
                                Cpf = f.Cpf
                            }
                    )
                )
            );
        }

        public Task<ServiceResult<IEnumerable<Funcionario>>> ObterFuncionariosDesativadosAsync()
        {
            var funcionarios = _dataBaseContext.Funcionarios.Where(f => !f.IsEnable);

            if (!funcionarios.Any())
                return Task.FromResult(
                    ServiceResult<IEnumerable<Funcionario>>.Failure(
                        "Não existem funcionários desativados em nosso sistema"
                    )
                );

            return Task.FromResult(
                ServiceResult<IEnumerable<Funcionario>>.Success(
                    funcionarios.Select(
                        f =>
                            new Funcionario()
                            {
                                Id = f.Id,
                                Nome = f.Nome,
                                Email = f.Email,
                                Cargo = f.Cargo,
                                Cpf = f.Cpf,
                                IsEnable = f.IsEnable
                            }
                    )
                )
            );
        }
    }
}
