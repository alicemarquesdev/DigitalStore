using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "Digite o seu email.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;
    }
}