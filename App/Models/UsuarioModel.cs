using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class Usuario
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

    [Required(ErrorMessage = "Campo obrigatório")]
    [Range(
        1,
        3,
        ErrorMessage = "Você deve informar um cargo válido (Admin = 1, Tecnico = 2, Usuario = 3)"
    )]
    public NivelDeAcesso NivelDeAcesso { get; set; }
}
