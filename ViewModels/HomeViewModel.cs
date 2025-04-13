using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para os métodos da HomeController
    public class HomeViewModel
    {
        // Indica se o usuário está logado
        // Controla o comportamento da interface (exibir ou esconder elementos com base no status de login)
        public bool UsuarioLogado { get; set; }

        // Indica se o perfil do usuário é cliente
        // Pode ser usado para personalizar a interface dependendo do tipo de usuário (cliente ou admin)
        public bool PerfilUsuarioCliente { get; set; }

        // Produto específico que pode ser destacado ou exibido na página inicial
        // Usado quando é necessário exibir um único produto com mais detalhes
        public ProdutoModel? Produto { get; set; }

        // Dados do site (como título, banner, frase.)
        // Usado para exibir informações relacionadas ao site na página inicial
        public SiteModel? SiteDados { get; set; }

        // Lista de produtos que o usuário adicionou ao carrinho
        public List<CarrinhoModel> CarrinhoDoUsuario { get; set; } = new List<CarrinhoModel>();

        // Lista de produtos favoritos do usuário
        public List<FavoritosModel> FavoritosDoUsuario { get; set; } = new List<FavoritosModel>();

        // Usada para apresentar uma coleção de produtos na página inicial
        public List<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();

        // Lista de "últimas novidades" que podem ser exibidas como produtos recentes ou lançamentos
        public List<ProdutoModel> UltimasNovidades { get; set; } = new List<ProdutoModel>();
    }
}
