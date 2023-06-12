using App.Entities;
using App.Services.Results;

namespace App.Services.Interfaces;

public interface ICargoService
{
    public Task<ServiceResult<CargoEntity>> BuscarCargoPeloId(int id);
}
