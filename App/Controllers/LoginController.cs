using Microsoft.AspNetCore.Mvc;
using App.Services.Interfaces;
using App.Dto;
using Microsoft.AspNetCore.Authorization;
using App.Services.Exceptions;

namespace App.Controllers;

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
        try
        {
            var funcionario = await _funcionarioService.AutenticarFuncionario(funcionarioForm);

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
        catch (FuncionarioNaoEncontrado e)
        {
            return BadRequest(new { mensagem = e.Message });
        }
        catch (System.Exception)
        {
            return Task.FromResult<ActionResult<dynamic>>(
                BadRequest(new { mensagem = "Erro ao autenticar funcionário" })
            );
        }
    }
}
