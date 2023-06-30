using App.Services.Results;
using App.Services.Interfaces;
using App.Models;
using App.Dto;

namespace App.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ICryptoService _cryptoService;
        private readonly DataBaseContext _dataBaseContext;

        public UsuarioService(ICryptoService cryptoService, DataBaseContext dataBaseContext)
        {
            _cryptoService = cryptoService;
            _dataBaseContext = dataBaseContext;
        }

        public async Task<ServiceResult<Usuario>> AtualizarUsuarioAsync(
            Usuario usuarioInput,
            int id
        )
        {
            // checked if id is empty
            if (id == 0)
                return ServiceResult<Usuario>.Failure(
                    "Por favor, informe o id do funcionário que deseja atualizar"
                );

            var usuario = await _dataBaseContext.Usuarios.FindAsync(id);

            if (usuario is null || !usuario.IsEnable)
                return ServiceResult<Usuario>.Failure(
                    "O Funcionário informado não existe em nosso sistema"
                );

            var newHashedPassword = _cryptoService.HashPassword(usuarioInput.Senha);

            usuario.Nome = usuarioInput.Nome;
            usuario.Email = usuarioInput.Email;
            usuario.NivelDeAcesso = usuarioInput.NivelDeAcesso;
            usuario.Cpf = usuarioInput.Cpf;
            usuario.Senha = newHashedPassword.Value;

            _dataBaseContext.Usuarios.Update(usuario);
            await _dataBaseContext.SaveChangesAsync();

            return ServiceResult<Usuario>.Success(
                new Usuario()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    NivelDeAcesso = usuario.NivelDeAcesso,
                    Cpf = usuario.Cpf
                }
            );
        }

        public Task<ServiceResult<Usuario>> AutenticarUsuarioAsync(LoginUsuario usuarioInput)
        {
            var usuario = _dataBaseContext.Usuarios.FirstOrDefault(
                f => f.Email == usuarioInput.Email && f.IsEnable
            );

            if (usuario is null)
                return Task.FromResult(
                    ServiceResult<Usuario>.Failure("Usuário ou senha inválidos")
                );

            var senhaValida = _cryptoService.VerifyPassword(usuarioInput.Senha, usuario.Senha);

            if (!senhaValida.IsSuccess)
                return Task.FromResult(
                    ServiceResult<Usuario>.Failure("Usuário ou senha inválidos")
                );

            return Task.FromResult(
                ServiceResult<Usuario>.Success(
                    new Usuario()
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        NivelDeAcesso = usuario.NivelDeAcesso,
                        Cpf = usuario.Cpf
                    }
                )
            );
        }

        public async Task<ServiceResult<Usuario>> CadastrarUsuarioAsync(Usuario usuarioInput)
        {
            var usuarioExistente = _dataBaseContext.Usuarios.FirstOrDefault(
                f => f.Email == usuarioInput.Email || f.Cpf == usuarioInput.Cpf && f.IsEnable
            );

            if (usuarioExistente is not null)
                return ServiceResult<Usuario>.Failure(
                    "Este usuário já está cadastrado em nosso sistema"
                );

            var hashedPassword = _cryptoService.HashPassword(usuarioInput.Senha);

            if (!hashedPassword.IsSuccess)
                return ServiceResult<Usuario>.Failure(
                    "Não foi possível cadastrar o usuário devido a um erro no servidor, tente novamente mais tarde"
                );

            usuarioInput.Senha = hashedPassword.Value;

            await _dataBaseContext.Usuarios.AddAsync(usuarioInput);

            await _dataBaseContext.SaveChangesAsync();

            return ServiceResult<Usuario>.Success(
                new Usuario()
                {
                    Id = usuarioInput.Id,
                    Nome = usuarioInput.Nome,
                    Email = usuarioInput.Email,
                    NivelDeAcesso = usuarioInput.NivelDeAcesso,
                    Cpf = usuarioInput.Cpf
                }
            );
        }

        public async Task<ServiceResult<Usuario>> DesativarUsuarioAsync(int id)
        {
            var usuario = await _dataBaseContext.Usuarios.FindAsync(id);

            if (usuario is null || !usuario.IsEnable)
                return ServiceResult<Usuario>.Failure(
                    "O Funcionário informado não existe em nosso sistema"
                );

            usuario.IsEnable = false;

            _dataBaseContext.Usuarios.Update(usuario);
            await _dataBaseContext.SaveChangesAsync();

            return ServiceResult<Usuario>.Success(
                new Usuario()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    NivelDeAcesso = usuario.NivelDeAcesso,
                    Cpf = usuario.Cpf
                }
            );
        }

        public Task<ServiceResult<Usuario>> ObterUsuarioAsync(int id)
        {
            var usuario = _dataBaseContext.Usuarios.FirstOrDefault(f => f.Id == id && f.IsEnable);

            if (usuario is null)
                return Task.FromResult(
                    ServiceResult<Usuario>.Failure(
                        "O Funcionário informado não existe em nosso sistema"
                    )
                );

            return Task.FromResult(
                ServiceResult<Usuario>.Success(
                    new Usuario()
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        NivelDeAcesso = usuario.NivelDeAcesso,
                        Cpf = usuario.Cpf
                    }
                )
            );
        }

        public Task<ServiceResult<IEnumerable<Usuario>>> ObterUsuariosAsync(
            int pagina,
            int tamanhoPagina
        )
        {
            var usuarios = _dataBaseContext.Usuarios
                .Where(f => f.IsEnable)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina);

            if (!usuarios.Any())
                return Task.FromResult(
                    ServiceResult<IEnumerable<Usuario>>.Failure(
                        "Não existem funcionários cadastrados em nosso sistema"
                    )
                );

            return Task.FromResult(
                ServiceResult<IEnumerable<Usuario>>.Success(
                    usuarios.Select(
                        f =>
                            new Usuario()
                            {
                                Id = f.Id,
                                Nome = f.Nome,
                                Email = f.Email,
                                NivelDeAcesso = f.NivelDeAcesso,
                                Cpf = f.Cpf
                            }
                    )
                )
            );
        }

        public Task<ServiceResult<IEnumerable<Usuario>>> ObterUsuariosDesativadosAsync()
        {
            var usuarios = _dataBaseContext.Usuarios.Where(f => !f.IsEnable);

            if (!usuarios.Any())
                return Task.FromResult(
                    ServiceResult<IEnumerable<Usuario>>.Failure(
                        "Não existem funcionários desativados em nosso sistema"
                    )
                );

            return Task.FromResult(
                ServiceResult<IEnumerable<Usuario>>.Success(
                    usuarios.Select(
                        f =>
                            new Usuario()
                            {
                                Id = f.Id,
                                Nome = f.Nome,
                                Email = f.Email,
                                NivelDeAcesso = f.NivelDeAcesso,
                                Cpf = f.Cpf,
                                IsEnable = f.IsEnable
                            }
                    )
                )
            );
        }
    }
}
