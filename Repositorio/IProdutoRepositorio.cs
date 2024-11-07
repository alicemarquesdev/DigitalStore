using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface IProdutoRepositorio
    {
        Task<List<ProdutoModel>> BuscarTodosProdutosAsync();

        Task<ProdutoModel> BuscarProdutoPorIdAsync(int id);

        Task<List<ProdutoModel>> BuscarProdutoPorCategoriaAsync(ProdutoModel categoria);

        Task<List<ProdutoModel>> BuscarCategoriasAsync();

        Task AddProdutoAsync(ProdutoModel produto);

        Task AtualizarProdutoAsync(ProdutoModel produto);

        Task<bool> RemoverProdutoAsync(int id);
    }
}