namespace App.Entities;

public class Cargo
{
    public Cargo(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
}
