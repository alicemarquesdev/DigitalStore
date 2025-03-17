using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que declara os métodos para manipulação dos dados da tabela Carrinho no banco de dados.
    public interface ICarrinhoRepositorio
    {
        // Retorna a lista de produtos do carrinho de um usuário.
        Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId);

        // Verifica se um produto já existe no carrinho do usuário e retorna o produto.
        Task<CarrinhoModel?> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId);

        // Adiciona ou remove um produto do carrinho, dependendo do estado atual.
        Task AddOuRemoverCarrinhoAsync(int produtoId, int usuarioId);

        // Atualiza a quantidade de um produto no carrinho do usuário.
        Task AtualizarQuantidadeAsync(int produtoId, int usuarioId, int quantidade);
    }
}
