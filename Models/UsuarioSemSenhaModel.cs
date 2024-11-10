using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class UsuarioSemSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o seu nome.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite o seu email.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite o nome do seu site.")]
        public string NomeSite { get; set; } = string.Empty;
    }
}