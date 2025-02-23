using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IPedidoRepositorio
    {
        Task<List<PedidoModel>> BuscarTodosOsPedidosDoUsuarioAsync(int usuarioId);

        Task<PedidoModel> BuscarPedidoPorIdAsync(int id);

        Task AddPedidoAsync(PedidoModel pedido);

        Task AtualizarPedidoAsync(PedidoModel pedido);
    }
}