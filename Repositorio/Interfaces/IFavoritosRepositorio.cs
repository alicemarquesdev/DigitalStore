using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que declara os métodos para manipulação dos dados dos favoritos no banco de dados.
    public interface IFavoritosRepositorio
    {
        // Retorna a lista de produtos favoritos de um usuário.
        Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId);

        // Verifica se um produto já existe na lista de favoritos do usuário e retorna o produto.
        Task<FavoritosModel?> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId);

        // Adiciona ou remove um produto da lista de favoritos, dependendo do estado atual.
        Task AddOuRemoverFavoritoAsync(int produtoId, int usuarioId);
    }
}
