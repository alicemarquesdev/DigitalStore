using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class AlterarSenhaModel
    {
        [Required(ErrorMessage = "Digite novamente sua nova senha.")]
        [Compare("NovaSenha", ErrorMessage = "A senha não confere com a nova senha")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;

        public int Id { get; set; }

        [Required(ErrorMessage = "Digite sua nova senha.")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite sua senha atual.")]
        public string SenhaAtual { get; set; } = string.Empty;
    }
}