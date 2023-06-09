using App.Dto;
using App.Services.Exceptions;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("/api/funcionario")]
    [Authorize(Roles = "Admin")]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly ILogger<FuncionarioController> _logger;

        public FuncionarioController(
            IFuncionarioService funcionarioService,
            ILogger<FuncionarioController> logger
        )
        {
            _funcionarioService = funcionarioService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> CadastrarFuncionarioAsync(
            [FromBody] CadastrarFuncionario funcionarioForm
        )
        {
            try
            {
                var funcionario = await _funcionarioService.CadastrarFuncionario(funcionarioForm);
                return Ok(new { funcionario, mensagem = "Funcionário cadastrado com sucesso" });
            }
            catch (EmailJaCadastrado e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (CargoNaoEncontrado e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (ErroCadastrarFuncionarioDB e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (Exception)
            {
                var errorMessage = "Erro ao cadastrar funcionário";
                _logger.LogError(errorMessage);
                return BadRequest(new { mensagem = errorMessage });
            }
        }
    }
}
