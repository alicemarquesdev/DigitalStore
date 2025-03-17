namespace DigitalStore.ViewModels
{
    // ViewModel para a barra de navegação (NavBar) da aplicação
    public class NavBarViewModel
    {
        // ID do usuário logado
        // Usado para identificar o usuário atual na barra de navegação
        public int UsuarioLogadoId { get; set; }

        // Quantidade de produtos no carrinho do usuário, exibida no badge do ícone de carrinho.
        public int CarrinhoQuantidadeDeProdutos { get; set; }

        // Quantidade de produtos na lista de favoritos do usuário, exibida no badge do ícone de favoritos.
        public int FavoritosQuantidadeDeProdutos { get; set; }

        // Usado para mostrar o nome do site na barra de navegação
        public string NomeSite { get; set; } = "Digital Store";

        // Indica se o usuário está logado ou não
        // Controla a exibição de links e opções de navegação com base no status de login do usuário
        public bool UsuarioLogado { get; set; }

        // Indica se o perfil do usuário é de cliente
        // Usado para personalizar a interface e a navegação dependendo do perfil do usuário
        public bool PerfilUsuarioCliente { get; set; }

        // Lista de categorias disponíveis no site
        // Usada para exibir as categorias de produtos na barra de navegação
        public List<string> Categorias { get; set; } = new List<string>();
    }
}
