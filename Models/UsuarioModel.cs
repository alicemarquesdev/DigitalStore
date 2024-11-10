using DigitalStore.Enum;
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

        [Required(ErrorMessage = "Digite a sua senha.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite o nome do seu site.")]
        public string NomeSite { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o perfil.")]
        public PerfilEnum Perfil { get; set; } = PerfilEnum.Cliente;

        public virtual List<FavoritosModel> Favoritos { get; set; } = new List<FavoritosModel>();

        public virtual List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();

        public bool SenhaValida(string senha)
        {
            return Senha == senha;
        }
    }
}