namespace DigitalStore.Models.ViewModels
{
    public class NavBarViewModel
    {
        public int UsuarioLogadoId { get; set; }

        public int CarrinhoQuantidadeDeProdutos { get; set; }

        public int FavoritosQuantidadeDeProdutos { get; set; }
        public string NomeSite { get; set; } = "Digital Store";

        public bool UsuarioLogado { get; set; }

        public bool PerfilUsuarioCliente { get; set; }

        public List<string> Categorias { get; set; } = new List<string>();
    }
}