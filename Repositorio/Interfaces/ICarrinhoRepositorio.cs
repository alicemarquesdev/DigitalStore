using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface ICarrinhoRepositorio
    {
        Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId);

        Task<CarrinhoModel> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId);

        Task AddAoCarrinhoAsync(int produtoId, int usuarioId);

        Task<bool> RemoverDoCarrinhoAsync(int produtoId, int usuarioId);
    }
}