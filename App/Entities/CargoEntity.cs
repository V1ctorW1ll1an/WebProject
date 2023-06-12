namespace App.Entities;

public class CargoEntity
{
    public CargoEntity() { }

    public CargoEntity(int? id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public int? Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
