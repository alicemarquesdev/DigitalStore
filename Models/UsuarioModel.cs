using DigitalStore.Enum;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o seu nome.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite o seu email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cria uma senha.")]
        public string Senha { get; set; } = string.Empty;

        public PerfilEnum Perfil { get; set; } = PerfilEnum.Usuario;

        public CarrinhoModel Carrinho { get; set; } = new CarrinhoModel();

        public FavoritosModel Favoritos { get; set; } = new FavoritosModel();

        public bool SenhaValida(string senha)
        {
            return Senha == senha;
        }
    }
}