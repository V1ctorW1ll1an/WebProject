using App.Entities;
using App.Repositories.Exceptions;
using App.Repositories.Interfaces;

namespace App.Repositories;

public class FuncionarioRepository : IFuncionarioRepository
{
    private readonly ILogger<FuncionarioRepository> _logger;

    public FuncionarioRepository(ILogger<FuncionarioRepository> logger)
    {
        _logger = logger;
    }

    // TODO create a database and replace this list with a real database
    public static List<Cargo> cargos = new List<Cargo>
    {
        new Cargo(1, "Admin"),
        new Cargo(2, "Operator"),
        new Cargo(3, "CommonEmployee"),
    };
    public static List<Funcionario> Funcionarios = new List<Funcionario>
    {
        new Funcionario(1, "John Doe", "john@teste.com", "123", cargos[0]),
        new Funcionario(2, "teste", "teste@teste.com", "123", cargos[1]),
        new Funcionario(3, "teste2", "teste2@teste.com", "123", cargos[2]),
    };

    public Task<Funcionario> BuscarFuncionarioPeloEmail(string email)
    {
        return Task.FromResult<Funcionario>(Funcionarios.FirstOrDefault(x => x.Email == email));
    }

    public Task<Funcionario> BuscarFuncionarioPeloEmailESenha(string email, string senha)
    {
        return Task.FromResult<Funcionario>(
            Funcionarios.FirstOrDefault(x => x.Email == email && x.Senha == senha)
        );
    }

    public Task<Funcionario> CadastrarFuncionario(Funcionario funcionario)
    {
        // generate fake id
        funcionario.Id = Funcionarios.Count + 1;
        Funcionarios.Add(funcionario);
        return Task.FromResult(funcionario);
    }

    public Task<Funcionario> AtualizarDadosDoFuncionario(Funcionario funcionario)
    {
        try
        {
            var index = Funcionarios.FindIndex(x => x.Id == funcionario.Id);
            Funcionarios[index].Nome = funcionario.Nome;
            Funcionarios[index].Cargo = funcionario.Cargo;
            return Task.FromResult(funcionario);
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível atualizar o funcionário. \nError: ", e);
            throw new ErroAtualizarFuncionarioDB("Não foi possível atualizar o funcionário");
        }
    }

    public Task<Funcionario> BuscarFuncionarioPeloId(int id)
    {
        return Task.FromResult<Funcionario>(Funcionarios.FirstOrDefault(x => x.Id == id));
    }
}
