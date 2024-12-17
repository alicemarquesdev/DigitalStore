using DigitalStore.Enum;
using DigitalStore.Helper;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class UsuarioModel
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Digite o seu nome.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite o seu email.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o perfil.")]
        public PerfilEnum Perfil { get; set; } = PerfilEnum.Cliente;

        [Required(ErrorMessage = "Digite a sua senha.")]
        public string Senha { get; set; } = string.Empty;

        public virtual List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();

        public virtual List<FavoritosModel> Favoritos { get; set; } = new List<FavoritosModel>();

        public bool SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
            return novaSenha;
        }

        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.GerarHash();
        }
    }
}