using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class LoginModel
    {
        [Required (ErrorMessage = "Insira o seu login.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Insira a sua senha.")]
        public string Senha { get; set; }
    }
}
