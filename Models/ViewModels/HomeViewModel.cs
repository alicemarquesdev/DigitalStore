namespace DigitalStore.Models.ViewModels
{
    public class HomeViewModel
    {
        public ProdutoModel? Produto { get; set; }
        public List<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
        public SiteModel? SiteDados { get; set; }
        public List<string> Categorias { get; set; } = new List<string>();
        public List<ProdutoModel> UltimasNovidades { get; set; } = new List<ProdutoModel>();
        public bool UsuarioLogado { get; set; } = false;
        public bool PerfilUsuarioCliente { get; set; } = false;
        public List<FavoritosModel> FavoritosDoUsuario { get; set; } = new List<FavoritosModel>();
        public List<CarrinhoModel> CarrinhoDoUsuario { get; set; } = new List<CarrinhoModel>();
    }
}