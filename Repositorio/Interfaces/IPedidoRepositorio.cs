using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que declara os métodos para manipulação dos dados de pedidos no banco de dados.
    public interface IPedidoRepositorio
    {
        // Retorna os detalhes de um pedido a partir do seu ID.
        Task<PedidoModel?> BuscarPedidoPorIdAsync(int id);

        // Retorna todos os pedidos de todos os usuário, incluindo pagamento, endereço e produtos do pedido.
        Task<List<PedidoModel>> BuscarTodosOsPedidosAsync();

        // Retorna a lista de todos os pedidos de um usuário,, incluindo pagamento, endereço e produtos do pedido.
        Task<List<PedidoModel>> BuscarTodosOsPedidosDoUsuarioAsync(int usuarioId);

        // Adiciona um novo pedido no banco de dados.
        Task AddPedidoAsync(PedidoModel pedido);

        // Atualiza as informações de um pedido existente no banco de dados.
        Task AtualizarPedidoAsync(PedidoModel pedido);
    }
}
