using App.Dto;
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
                var resultado = await _funcionarioService.CadastrarFuncionarioAsync(
                    funcionarioForm
                );

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var funcionario = resultado.Value;
                return Ok(
                    new
                    {
                        funcionario = new
                        {
                            funcionario.Id,
                            funcionario.Nome,
                            funcionario.Email,
                            funcionario.Cargo
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

        [HttpPut]
        public async Task<ActionResult<dynamic>> AtualizarFuncionarioAsync(
            [FromBody] AtualizarFuncionario funcionarioForm
        )
        {
            try
            {
                var resultado = await _funcionarioService.AtualizarFuncionarioAsync(
                    funcionarioForm
                );

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var funcionario = resultado.Value;
                return Ok(new { funcionario, mensagem = "Funcionário atualizado com sucesso" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar funcionário" });
            }
        }
    }
}
