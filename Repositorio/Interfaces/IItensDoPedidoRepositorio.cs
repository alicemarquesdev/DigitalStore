using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que declara os métodos para manipulação dos itens do pedido no banco de dados.
    public interface IItensDoPedidoRepositorio
    {
        // Retorna todos os itens de um pedido específico.
        Task<List<ItensDoPedidoModel>> BuscarTodosOsItensDoPedidoAsync(int pedidoId);

        // Adiciona um item no pedido.
        Task AddItemAsync(ItensDoPedidoModel item);
    }
}
