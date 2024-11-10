using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Insira o seu login.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Insira a sua senha.")]
        public string Senha { get; set; } = string.Empty;
    }
}