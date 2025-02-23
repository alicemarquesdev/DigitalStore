using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IItensDoPedidoRepositorio
    {
        Task<List<ItensDoPedidoModel>> BuscarTodosOsItensDoPedidoAsync(int pedidoId);

        Task<ItensDoPedidoModel> BuscarItemDoPedidoPorIdAsync(int itemId);

        Task AddItemAsync(ItensDoPedidoModel item);

        Task AtualizarItemAsync(ItensDoPedidoModel item);

        Task<bool> RemoverItemAsync(int id);
    }
}