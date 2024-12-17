using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IProdutoRepositorio
    {
        Task<List<ProdutoModel>> BuscarCategoriasAsync();

        Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(ProdutoModel categoria);

        Task<ProdutoModel> BuscarProdutoPorIdAsync(int id);

        Task<List<ProdutoModel>> BuscarTodosProdutosAsync();

        Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionados();

        Task AddProdutoAsync(ProdutoModel produto, IFormFile imagem);

        Task AtualizarProdutoAsync(ProdutoModel produto, IFormFile? novaImagem);

        Task<bool> RemoverProdutoAsync(int id);

        Task<string> GerarCaminhoArquivoAsync(IFormFile imagem);
    }
}