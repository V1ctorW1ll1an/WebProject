using App.Entities;

namespace App.Services.Interfaces;

public interface ICargoService
{
  public Task<Cargo> BuscarCargoPeloId(int id);
}