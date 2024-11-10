using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface ICarrinhoRepositorio
    {
        Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId);

        Task<CarrinhoModel> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId);

        Task AddAoCarrinhoAsync(int produtoId, int usuarioId);

        Task<bool> RemoverDoCarrinhoAsync(int produtoId, int usuarioId);
    }
}