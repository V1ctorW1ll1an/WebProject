using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class Funcionario
{
    public int Id { get; set; }

    [StringLength(
        250,
        MinimumLength = 3,
        ErrorMessage = "O nome deve ter entre 3 e 250 caracteres."
    )]
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório")]
    [StringLength(
        250,
        MinimumLength = 3,
        ErrorMessage = "O email deve ter entre 3 e 250 caracteres."
    )]
    [EmailAddress(ErrorMessage = "Email invalido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório")]
    [StringLength(
        250,
        MinimumLength = 6,
        ErrorMessage = "A senha deve ter entre 6 e 250 caracteres."
    )]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos")]
    public Int64 Cpf { get; set; }

    public Boolean IsEnable { get; set; } = true;

    public Cargo Cargo { get; set; } = Cargo.Funcionario;
}
