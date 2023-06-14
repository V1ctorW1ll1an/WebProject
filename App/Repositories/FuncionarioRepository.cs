using App.Entities;
using App.Repositories.Interfaces;
using App.Repositories.Results;
using Dapper;
using Npgsql;

namespace App.Repositories;

public class FuncionarioRepository : IFuncionarioRepository
{
    private readonly ILogger<FuncionarioRepository> _logger;
    private readonly string _connectionString = Settings.GetConnectionString();

    public FuncionarioRepository(ILogger<FuncionarioRepository> logger)
    {
        _logger = logger;
    }

    public async Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloEmailAsync(string email)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql =
                @"SELECT f.*, c.* FROM Funcionario f INNER JOIN Cargo c ON f.idCargo = c.id WHERE f.email = @Email";

            var funcionarios = await connection.QueryAsync<
                FuncionarioEntity,
                CargoEntity,
                FuncionarioEntity
            >(
                sql,
                (funcionario, cargo) =>
                {
                    funcionario.Cargo = cargo;
                    return funcionario;
                },
                new { Email = email },
                splitOn: "idCargo"
            );

            var funcionario = funcionarios.FirstOrDefault();

            if (funcionario is null)
                return DbResult<FuncionarioEntity>.Failure();

            return DbResult<FuncionarioEntity>.Success(funcionario);
        }
        catch (NpgsqlException e)
        {
            _logger.LogError(e, "Erro ao buscar Funcionário pelo email");
            throw;
        }
    }

    public async Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloEmailESenhaAsync(
        string email,
        string senha
    )
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query =
                "SELECT f.*, c.* FROM Funcionario f INNER JOIN Cargo c ON f.idCargo = c.id WHERE f.email = @Email AND f.senha = @Senha";

            var funcionario = await connection.QueryAsync<
                FuncionarioEntity,
                CargoEntity,
                FuncionarioEntity
            >(
                query,
                (funcionario, cargo) =>
                {
                    funcionario.Cargo = cargo;
                    return funcionario;
                },
                new { Email = email, Senha = senha },
                splitOn: "id"
            );

            if (!funcionario.Any())
                return DbResult<FuncionarioEntity>.Failure();

            return DbResult<FuncionarioEntity>.Success(funcionario.First());
        }
        catch (NpgsqlException e)
        {
            _logger.LogError(e, "Erro ao buscar funcionário pelo email e senha");
            throw;
        }
    }

    public async Task<DbResult<FuncionarioEntity>> CadastrarFuncionarioAsync(
        FuncionarioEntity funcionario
    )
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query =
                "INSERT INTO Funcionario (nome, email, senha, idCargo) VALUES (@Nome, @Email, @Senha, @IdCargo) RETURNING *";

            var funcionarioCadastrado =
                await connection.QueryFirstOrDefaultAsync<FuncionarioEntity>(
                    query,
                    new
                    {
                        Nome = funcionario.Nome,
                        Email = funcionario.Email,
                        Senha = funcionario.Senha,
                        IdCargo = funcionario.Cargo.Id
                    }
                );

            if (funcionarioCadastrado is null)
                return DbResult<FuncionarioEntity>.Failure();

            var cargo = await connection.QueryFirstOrDefaultAsync<CargoEntity>(
                "SELECT * FROM Cargo WHERE id = @Id",
                new { Id = funcionario.Cargo.Id }
            );
            funcionarioCadastrado.Cargo = cargo;

            return DbResult<FuncionarioEntity>.Success(funcionarioCadastrado);
        }
        catch (NpgsqlException e)
        {
            _logger.LogError(e, "Erro ao cadastrar funcionário");
            throw;
        }
    }

    public async Task<DbResult<FuncionarioEntity>> AtualizarDadosDoFuncionarioAsync(
        FuncionarioEntity funcionario
    )
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query =
                "UPDATE Funcionario SET nome = @Nome, email = @Email, senha = @Senha, idCargo = @IdCargo WHERE id = @Id RETURNING *";

            var funcionarioAtualizado =
                await connection.QueryFirstOrDefaultAsync<FuncionarioEntity>(
                    query,
                    new
                    {
                        Nome = funcionario.Nome,
                        Email = funcionario.Email,
                        Senha = funcionario.Senha,
                        IdCargo = funcionario.Cargo.Id,
                        Id = funcionario.Id
                    }
                );

            if (funcionarioAtualizado is null)
                return DbResult<FuncionarioEntity>.Failure();

            return DbResult<FuncionarioEntity>.Success(funcionarioAtualizado);
        }
        catch (NpgsqlException e)
        {
            _logger.LogError(e, "Erro ao atualizar funcionário");
            throw;
        }
    }

    public async Task<DbResult<FuncionarioEntity>> BuscarFuncionarioPeloIdAsync(int id)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var query =
                "SELECT f.*, c.* FROM Funcionario f INNER JOIN Cargo c ON f.idCargo = c.id WHERE f.id = @Id";

            var funcionario = await connection.QueryAsync<
                FuncionarioEntity,
                CargoEntity,
                FuncionarioEntity
            >(
                query,
                (funcionario, cargo) =>
                {
                    funcionario.Cargo = cargo;
                    return funcionario;
                },
                new { Id = id }
            );

            if (!funcionario.Any())
                return DbResult<FuncionarioEntity>.Failure();

            return DbResult<FuncionarioEntity>.Success(funcionario.First());
        }
        catch (NpgsqlException e)
        {
            _logger.LogError(e, "Erro ao buscar funcionário pelo ID");
            throw;
        }
    }
}
