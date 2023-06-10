using App.Entities;
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

    // TODO: #8 Find from database
    public async Task<Cargo> BuscarCargoPeloId(int id)
    {
        try
        {
            var connectionString = Settings.GetConnectionString();
            var query = "SELECT * FROM cargo WHERE id = @id";

            using var connection = new NpgsqlConnection(connectionString);

            var cargo = await connection.QueryFirstOrDefaultAsync<Cargo>(query, new { id });

            return cargo;
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Erro ao buscar cargo pelo id");
            throw;
        }
    }
}
