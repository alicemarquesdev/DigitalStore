namespace DigitalStore.Models
{
    // Representa um item de favorito adicionado por um usuário ao sistema.
    public class FavoritosModel
    {
        // ID do produto que foi adicionado aos favoritos.
        public int ProdutoId { get; set; }

        // Objeto que acessa os detalhes do produto favorito.
        public virtual ProdutoModel? Produto { get; set; }

        // ID do usuário que adicionou o produto aos favoritos.
        public int UsuarioId { get; set; }

        // Objeto que acessa os detalhes do usuário que adicionou o favorito.
        public virtual UsuarioModel? Usuario { get; set; }
    }
}
