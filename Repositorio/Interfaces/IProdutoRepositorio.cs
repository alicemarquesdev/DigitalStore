using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que declara os métodos para manipulação dos dados de produtos no banco de dados.
    public interface IProdutoRepositorio
    {
        // Retorna uma lista de strings com todas as categorias de produtos disponíveis.
        Task<List<string>> BuscarCategoriasAsync();

        // Retorna uma lista de produtos filtrados por uma categoria específica.
        Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(string categoria);

        // Retorna um produto específico baseado no seu ID.
        Task<ProdutoModel?> BuscarProdutoPorIdAsync(int id);

        // Método para encontrar o produto que corresponde ao termo, usado na barra de pesquisa.
        Task<List<ProdutoModel>> BuscarProdutosBarraDePesquisaAsync(string termo);

        // Retorna uma lista de todos os produtos
        Task<List<ProdutoModel>> BuscarTodosOsProdutosAsync();

        // Retorna os últimos produtos adicionados ao sistema, para exibir os produtos novos.
        Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionadosAsync();

        // Adiciona um novo produto no banco de dados.
        Task AddProdutoAsync(ProdutoModel produto);

        // Atualiza as informações de um produto existente, podendo incluir uma nova imagem.
        Task AtualizarProdutoAsync(ProdutoModel produto);

        // Remove um produto do banco de dados com base no seu ID.
        Task<bool> RemoverProdutoAsync(ProdutoModel produto);
    }
}
