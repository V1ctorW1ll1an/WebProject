using Microsoft.AspNetCore.Mvc;
using App.Services.Interfaces;
using App.Dto;

namespace App.Controllers;

[ApiController]
[Route("/api")]
public class LoginController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IFuncionarioService _funcionarioService;

    // TODO #2 Inject FuncionarioRepository
    public LoginController(IFuncionarioService funcionarioService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _funcionarioService = funcionarioService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> AuthenticateAsync(
        [FromBody] FuncionarioLoginInput funcionarioForm
    )
    {
        var funcionario = await _funcionarioService.AutenticarFuncionario(funcionarioForm);

        if (funcionario == null)
            return Task.FromResult<ActionResult<dynamic>>(
                NotFound(new { mensagem = "Email ou password inválidos" })
            );

        var token = _tokenService.GenerateToken(funcionario);

        var novoFuncionario = new FuncionarioLoginOutput(
            funcionario.Id,
            funcionario.Nome,
            funcionario.Email,
            funcionario.Cargo,
            token
        );

        return new { funcionario = novoFuncionario };
    }
}
