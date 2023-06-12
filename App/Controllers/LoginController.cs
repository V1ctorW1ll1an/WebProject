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
        private readonly IFuncionarioService _funcionarioService;

        public LoginController(IFuncionarioService funcionarioService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _funcionarioService = funcionarioService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> AuthenticateAsync(
            [FromBody] LoginFuncionario funcionarioForm
        )
        {
            var resultado = await _funcionarioService.AutenticarFuncionario(funcionarioForm);

            if (!resultado.IsSuccess)
                return BadRequest(new { mensagem = resultado.ErrorMessage });

            var funcionario = resultado.Value;

            var token = _tokenService.GenerateToken(funcionario);

            var novoFuncionario = new
            {
                funcionario.Id,
                funcionario.Nome,
                funcionario.Email,
                funcionario.Cargo,
            };

            return new { funcionario = novoFuncionario, token };
        }
    }
}
