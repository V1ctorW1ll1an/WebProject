using System.ComponentModel.DataAnnotations;

namespace App.Dto;

public class CadastrarFuncionario
{
    private const string CpfRegex = @"^[0-9]{11}$";

    public CadastrarFuncionario(string nome, string email, int cpf, string senha, int cargoId)
    {
        Nome = nome;
        Email = email;
        Cpf = cpf;
        Senha = senha;
        CargoId = cargoId;
    }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100, ErrorMessage = "Nome deve conter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [MaxLength(100, ErrorMessage = "Email deve conter no máximo 100 caracteres")]
    public string Email { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    [RegularExpression(CpfRegex, ErrorMessage = "CPF inválido")]
    public int Cpf { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "Senha deve conter no mínimo 6 caracteres")]
    [MaxLength(100, ErrorMessage = "Senha deve conter no máximo 100 caracteres")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "Id do cargo é obrigatório")]
    public int CargoId { get; set; }
}
