using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Representa o modelo de login para autenticação de usuários.
    public class LoginModel
    {
        // Endereço de email do usuário, necessário para autenticação.
        [Required(ErrorMessage = "Insira o seu login.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public required string Email { get; set; }

        // Senha do usuário, necessária para autenticação.
        [Required(ErrorMessage = "Insira a sua senha.")]
        public required string Senha { get; set; }
    }
}
