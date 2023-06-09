using App.Entities;

namespace App.Repositories.Interfaces;

public interface ICargoRepository
{
  Task<Cargo> BuscarCargoPeloId(int id);
}
