using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface ICarrinhoRepositorio
    {
        Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId);

        Task<CarrinhoModel> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId);

        Task AddOuRemoverCarrinhoAsync(int produtoId, int usuarioId);
        Task AtualizarQuantidadeAsync(int produtoId, int usuarioId, int quantidade);
    }
}