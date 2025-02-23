using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IProdutoRepositorio
    {
        Task<List<string>> BuscarCategoriasAsync();

        Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(string categoria);

        Task<ProdutoModel> BuscarProdutoPorIdAsync(int id);

        Task<List<ProdutoModel>> BuscarTodosProdutosAsync();

        Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionados();

        Task AddProdutoAsync(ProdutoModel produto);

        Task AtualizarProdutoAsync(ProdutoModel produto, IFormFile? novaImagem);

        Task<bool> RemoverProdutoAsync(int id);
    }
}