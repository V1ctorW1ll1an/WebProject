using App.Models;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> AddAsync([FromBody] Usuario usuarioForm)
        {
            try
            {
                var resultado = await _usuarioService.CadastrarUsuarioAsync(usuarioForm);

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var usuario = resultado.Value;
                return Ok(
                    new
                    {
                        usuario = new
                        {
                            usuario.Id,
                            usuario.Nome,
                            usuario.Email,
                            usuario.NivelDeAcesso,
                            usuario.Cpf
                        },
                        mensagem = "Funcionário cadastrado com sucesso"
                    }
                );
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao cadastrar funcionário" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetAsync(int id)
        {
            try
            {
                var resultado = await _usuarioService.ObterUsuarioAsync(id);

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var usuario = resultado.Value;
                return Ok(
                    new
                    {
                        usuario = new
                        {
                            usuario.Id,
                            usuario.Nome,
                            usuario.Email,
                            usuario.NivelDeAcesso,
                            usuario.Cpf
                        }
                    }
                );
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao obter funcionário" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<dynamic>> GetAllAsync(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10
        )
        {
            try
            {
                var resultado = await _usuarioService.ObterUsuariosAsync(pagina, tamanhoPagina);

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var usuarios = resultado.Value;
                return Ok(
                    new
                    {
                        usuarios = usuarios.Select(
                            f =>
                                new
                                {
                                    f.Id,
                                    f.Nome,
                                    f.Email,
                                    f.NivelDeAcesso,
                                    f.Cpf
                                }
                        )
                    }
                );
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao obter funcionários" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteAsync(int id)
        {
            try
            {
                var resultado = await _usuarioService.DesativarUsuarioAsync(id);

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                return Ok(new { mensagem = "Funcionário desativado com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao desativar funcionário" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> AtualizarUsuarioAsync(
            [FromBody] Usuario usuarioForm,
            int id
        )
        {
            try
            {
                var resultado = await _usuarioService.AtualizarUsuarioAsync(usuarioForm, id);

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var usuario = new Usuario
                {
                    Id = resultado.Value.Id,
                    Nome = resultado.Value.Nome,
                    Email = resultado.Value.Email,
                    NivelDeAcesso = resultado.Value.NivelDeAcesso,
                    Cpf = resultado.Value.Cpf
                };
                return Ok(new { usuario, mensagem = "Funcionário atualizado com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar funcionário" });
            }
        }

        [HttpGet]
        [Route("desativados")]
        public async Task<ActionResult<dynamic>> GetAllDisabledAsync()
        {
            try
            {
                var resultado = await _usuarioService.ObterUsuariosDesativadosAsync();

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var usuarios = resultado.Value;
                return Ok(
                    new
                    {
                        usuarios = usuarios.Select(
                            f =>
                                new
                                {
                                    f.Id,
                                    f.Nome,
                                    f.Email,
                                    f.NivelDeAcesso,
                                    f.Cpf,
                                    f.IsEnable
                                }
                        )
                    }
                );
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao obter funcionários desativados" });
            }
        }
    }
}
