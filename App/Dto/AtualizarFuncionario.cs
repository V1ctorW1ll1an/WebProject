using System.ComponentModel.DataAnnotations;

namespace App.Dto;

public class AtualizarFuncionario
{
    public AtualizarFuncionario(int id, string nome, int cargoId)
    {
        Id = id;
        Nome = nome;
        CargoId = cargoId;
    }

    [Required(ErrorMessage = "O campo id é obrigatório")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100, ErrorMessage = "Nome deve conter no máximo 100 caracteres")]
    [MinLength(3, ErrorMessage = "Nome deve conter no mínimo 3 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Especifique o cargo do funcionário")]
    public int CargoId { get; set; }
}
