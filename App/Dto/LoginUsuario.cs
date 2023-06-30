using System.ComponentModel.DataAnnotations;

namespace App.Dto;

public class LoginUsuario
{
    public LoginUsuario(string email, string senha)
    {
        Email = email;
        Senha = senha;
    }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [MaxLength(100, ErrorMessage = "Email deve conter no máximo 100 caracteres")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Senha { get; set; }
}
