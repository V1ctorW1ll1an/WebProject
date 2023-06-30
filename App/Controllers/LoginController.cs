using Microsoft.AspNetCore.Mvc;
using App.Services.Interfaces;
using App.Dto;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [ApiController]
    [Route("/api")]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> AuthenticateAsync(
            [FromBody] LoginUsuario usuarioForm
        )
        {
            try
            {
                var resultado = await _usuarioService.AutenticarUsuarioAsync(usuarioForm);

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var usuario = resultado.Value;

                var token = _tokenService.GenerateToken(usuario);

                var novoUsuario = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.NivelDeAcesso,
                };

                return new { usuario = novoUsuario, token };
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao autenticar usuario" });
            }
        }
    }
}
