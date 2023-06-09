using App.Entities;
using App.Repositories.Interfaces;

namespace App.Repositories;

public class CargoRepository : ICargoRepository
{
    public static List<Cargo> Cargos = new List<Cargo>
    {
        new Cargo(1, "Admin"),
        new Cargo(2, "Funcionario"),
    };

    public Task<Cargo> BuscarCargoPeloId(int id)
    {
        var cargo = Cargos.FirstOrDefault(cargo => cargo.Id == id);
        return Task.FromResult<Cargo>(cargo);
    }
}
