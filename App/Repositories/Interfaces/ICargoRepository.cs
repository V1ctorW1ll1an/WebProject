using App.Entities;
using App.Repositories.Results;

namespace App.Repositories.Interfaces;

public interface ICargoRepository
{
    Task<DbResult<CargoEntity>> BuscarCargoPeloIdAsync(int id);
}
