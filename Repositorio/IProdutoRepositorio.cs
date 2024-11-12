using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface IProdutoRepositorio
    {
        Task AddProdutoAsync(ProdutoModel produto);

        Task AtualizarProdutoAsync(ProdutoModel produto);

        Task<List<ProdutoModel>> BuscarCategoriasAsync();

        Task<List<ProdutoModel>> BuscarProdutoPorCategoriaAsync(ProdutoModel categoria);

        Task<ProdutoModel> BuscarProdutoPorIdAsync(int id);

        Task<List<ProdutoModel>> BuscarTodosProdutosAsync();

        Task<bool> RemoverProdutoAsync(int id);
    }
}