using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // Responsável pela manipulação dos itens do pedido no banco de dados.
    // - BuscarTodosOsItensDoPedidoAsync(int pedidoId)
    // - AddItemAsync(ItensDoPedidoModel item)
    public class ItensDoPedidoRepositorio : IItensDoPedidoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<ItensDoPedidoRepositorio> _logger;

        // Construtor que recebe o contexto do banco de dados (BancoContext).
        public ItensDoPedidoRepositorio(BancoContext context, ILogger<ItensDoPedidoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método que retorna todos os itens de um pedido específico, incluindo as informações do produto relacionado.
        public async Task<List<ItensDoPedidoModel>> BuscarTodosOsItensDoPedidoAsync(int pedidoId)
        {
            try
            {
                // Consulta os itens do pedido e inclui os dados do produto associado a cada item.
                return await _context.ItensDoPedido
                                           .Include(x => x.Produto)
                                           .Where(x => x.PedidoId == pedidoId)
                                           .ToListAsync();
            }
            catch (Exception ex)
            {
                // Captura e lança a exceção, com mensagem personalizada para facilitar o diagnóstico do erro.
                _logger.LogError(ex, "Erro ao buscar itens do pedido com ID {PedidoId}", pedidoId);
                throw new Exception("Erro ao buscar itens do pedido.");
            }
        }

        // Método que adiciona um item ao pedido e salva as alterações no banco de dados.
        public async Task AddItemAsync(ItensDoPedidoModel item)
        {
            try
            {
                // Adiciona o item ao contexto do pedido.
                _context.ItensDoPedido.Add(item);

                // Salva as mudanças no banco de dados e obtém o resultado da operação.
                var result = await _context.SaveChangesAsync();

                // Se a operação não afetar nenhuma linha (result == 0), lança uma exceção.
                if (result == 0)
                {
                    throw new Exception("Pedido não adicionado.");
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção personalizada com a mensagem original.
                _logger.LogError(ex, "Erro ao adicionar item ao pedido.");
                throw new Exception("Erro ao adicionar item ao pedido.");
            }
        }
    }
}
