using App.Models;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Tecnico")]
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
        public async Task<ActionResult<dynamic>> AddAsync([FromBody] Funcionario funcionarioForm)
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
                            funcionario.Cargo,
                            funcionario.Cpf
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
                var resultado = await _funcionarioService.ObterFuncionarioAsync(id);

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
                            funcionario.Cargo,
                            funcionario.Cpf
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
        public async Task<ActionResult<dynamic>> GetAllAsync()
        {
            try
            {
                var resultado = await _funcionarioService.ObterFuncionariosAsync();

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var funcionarios = resultado.Value;
                return Ok(
                    new
                    {
                        funcionarios = funcionarios.Select(
                            f =>
                                new
                                {
                                    f.Id,
                                    f.Nome,
                                    f.Email,
                                    f.Cargo,
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
                var resultado = await _funcionarioService.DesativarFuncionarioAsync(id);

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
        public async Task<ActionResult<dynamic>> AtualizarFuncionarioAsync(
            [FromBody] Funcionario funcionarioForm,
            int id
        )
        {
            try
            {
                var resultado = await _funcionarioService.AtualizarFuncionarioAsync(
                    funcionarioForm,
                    id
                );

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var funcionario = new Funcionario
                {
                    Id = resultado.Value.Id,
                    Nome = resultado.Value.Nome,
                    Email = resultado.Value.Email,
                    Cargo = resultado.Value.Cargo,
                    Cpf = resultado.Value.Cpf
                };
                return Ok(new { funcionario, mensagem = "Funcionário atualizado com sucesso" });
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
                var resultado = await _funcionarioService.ObterFuncionariosDesativadosAsync();

                if (!resultado.IsSuccess)
                    return BadRequest(new { mensagem = resultado.ErrorMessage });

                var funcionarios = resultado.Value;
                return Ok(
                    new
                    {
                        funcionarios = funcionarios.Select(
                            f =>
                                new
                                {
                                    f.Id,
                                    f.Nome,
                                    f.Email,
                                    f.Cargo,
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
