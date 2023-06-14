using App.Entities;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Services.Results;

namespace App.Services;

public class CargoService : ICargoService
{
    public readonly ICargoRepository _cargoRepository;

    public CargoService(ICargoRepository cargoRepository)
    {
        _cargoRepository = cargoRepository;
    }

    public async Task<ServiceResult<CargoEntity>> BuscarCargoPeloIdAsync(int id)
    {
        var cargo = await _cargoRepository.BuscarCargoPeloIdAsync(id);

        if (!cargo.IsSuccess)
            return ServiceResult<CargoEntity>.Failure("Cargo não encontrado");

        return ServiceResult<CargoEntity>.Success(cargo.Value);
    }
}
