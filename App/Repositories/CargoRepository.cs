using App.Entities;
using App.Repositories.Results;
using App.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace App.Repositories;

public class CargoRepository : ICargoRepository
{
    private readonly ILogger<CargoRepository> _logger;

    public CargoRepository(ILogger<CargoRepository> logger)
    {
        _logger = logger;
    }

    public async Task<DbResult<CargoEntity>> BuscarCargoPeloIdAsync(int id)
    {
        try
        {
            var connectionString = Settings.GetConnectionString();
            var query = "SELECT * FROM cargo WHERE id = @id";

            using var connection = new NpgsqlConnection(connectionString);
            var cargo = await connection.QueryFirstOrDefaultAsync<CargoEntity>(query, new { id });

            if (cargo is null)
                return DbResult<CargoEntity>.Failure();

            return DbResult<CargoEntity>.Success(cargo);
        }
        catch (NpgsqlException e)
        {
            _logger.LogError(e, "Erro ao buscar cargo pelo ID");
            throw;
        }
    }
}
