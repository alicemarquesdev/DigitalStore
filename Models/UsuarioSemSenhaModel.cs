using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class UsuarioSemSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o seu nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o seu email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite o seu endereço.")]
        public string Endereço { get; set; }

    }
}
