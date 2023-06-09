using App.Entities;
using App.Repositories.Interfaces;
using App.Services.Interfaces;

namespace App.Services;

public class CargoService : ICargoService
{

  public readonly ICargoRepository _cargoRepository;
  public CargoService(ICargoRepository cargoRepository)
  {
    _cargoRepository = cargoRepository;
  }
  public async Task<Cargo> BuscarCargoPeloId(int id)
  {
    var cargo = await _cargoRepository.BuscarCargoPeloId(id);
    return cargo;
  }
}