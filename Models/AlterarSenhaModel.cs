using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class AlterarSenhaModel
    {
        public int Id { get; set; }

        // Senha atual do usuário
        [Required(ErrorMessage = "Digite sua senha atual.")]
        public string SenhaAtual { get; set; } = string.Empty;

        // Nova senha do usuário
        [Required(ErrorMessage = "Digite sua nova senha.")]
        public string NovaSenha { get; set; } = string.Empty;

        // Confirmação da nova senha
        [Required(ErrorMessage = "Digite novamente sua nova senha.")]
        [Compare("NovaSenha", ErrorMessage = "A senha não confere com a nova senha")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}