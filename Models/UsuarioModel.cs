using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Digite o seu nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o seu email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite o seu endereço.")]
        public string Endereço { get; set; }

        [Required(ErrorMessage = "Cria uma senha.")]
        public string Senha { get; set; }

        public CarrinhoModel Carrinho { get; set; }

        public FavoritosModel Favoritos { get; set; }   

        public bool SenhaValida(string senha)
        {
            return Senha == senha;
        }

    }



}
