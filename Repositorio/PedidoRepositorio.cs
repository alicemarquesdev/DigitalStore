using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // A classe PedidoRepositorio é responsável por manipular as operações relacionadas aos pedidos no banco de dados.
    // Isso inclui as operações:
    // - BuscarPedidoPorIdAsync(int id) - Para buscar um pedido específico pelo seu ID.
    // - BuscarTodosOsPedidosAsync() - Para buscar todos os pedidos do banco de dados.
    // - BuscarTodosOsPedidosDoUsuarioAsync(int usuarioId) - Para buscar todos os pedidos de um usuário específico.
    // - AddPedidoAsync(PedidoModel pedido) - Para adicionar um novo pedido.
    // - AtualizarPedidoAsync(PedidoModel pedido) - Para atualizar um pedido existente.
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<PedidoRepositorio> _logger;

        // Construtor que recebe o contexto do banco de dados (BancoContext).
        // O contexto é injetado para interagir com o banco de dados.
        public PedidoRepositorio(BancoContext context, ILogger<PedidoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método para buscar um pedido específico pelo seu ID, incluindo as informações de pagamento.
        public async Task<PedidoModel?> BuscarPedidoPorIdAsync(int id)
        {
            try
            {
                // Utiliza o método Include para carregar as informações de Pagamento relacionadas ao pedido.
                return await _context.Pedidos
                     .Include(p => p.Pagamento)  // Inclui os detalhes do pagamento do pedido.
                     .FirstOrDefaultAsync(x => x.PedidoId == id);  // Retorna o pedido com o ID fornecido ou null.
            }
            catch (Exception ex)
            {
                // Em caso de erro, lança uma nova exceção com uma mensagem detalhada.
                _logger.LogError(ex, "Erro ao buscar pedido com ID.");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Método para buscar todos os pedidos no banco de dados, incluindo informações de pagamento e itens do pedido.
        public async Task<List<PedidoModel>> BuscarTodosOsPedidosAsync()
        {
            try
            {
                // Retorna todos os pedidos, incluindo informações detalhadas de pagamento e itens do pedido.
                return await _context.Pedidos
                    .Include(p => p.Pagamento)  // Inclui informações do pagamento.
                    .Include(i => i.ItensDoPedido)  // Inclui os itens do pedido.
                    .ThenInclude(p => p.Produto)  // Inclui os detalhes dos produtos dos itens.
                    .OrderByDescending(x => x.DataDoPedido)  // Ordena os pedidos pela data em ordem decrescente.
                    .ToListAsync();  // Converte para lista e retorna os resultados.
            }
            catch (Exception ex)
            {
                // Em caso de erro, lança uma exceção com a mensagem detalhada.
                _logger.LogError(ex, "Erro ao buscar todos os pedidos.");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Método para buscar todos os pedidos de um usuário específico, incluindo as informações de pagamento, itens e produtos.
        public async Task<List<PedidoModel>> BuscarTodosOsPedidosDoUsuarioAsync(int usuarioId)
        {
            try
            {
                // Retorna todos os pedidos de um usuário, incluindo detalhes do pagamento, itens do pedido e produtos.
                return await _context.Pedidos
                    .Include(p => p.Pagamento)  // Inclui os detalhes de pagamento.
                    .Include(i => i.ItensDoPedido)  // Inclui os itens do pedido.
                    .ThenInclude(p => p.Produto)  // Inclui os produtos dos itens.
                    .Where(x => x.UsuarioId == usuarioId)  // Filtra os pedidos do usuário com o ID fornecido.
                    .OrderByDescending(x => x.DataDoPedido)  // Ordena os pedidos pela data em ordem decrescente.
                    .ToListAsync();  // Converte para lista e retorna.
            }
            catch (Exception ex)
            {
                // Caso ocorra um erro, lança uma exceção detalhada.
                _logger.LogError(ex, "Erro ao buscar pedidos do usuário com ID");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Método para adicionar um novo pedido ao banco de dados.
        public async Task AddPedidoAsync(PedidoModel pedido)
        {
            try
            {
                // Adiciona o pedido ao contexto de pedidos.
                await _context.Pedidos.AddAsync(pedido);

                // Tenta salvar as mudanças no banco de dados.
                var result = await _context.SaveChangesAsync();

                // Se não houver alterações no banco (result == 0), lança uma exceção.
                if (result == 0)
                {
                    throw new Exception("Falha ao adicionar o pedido no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Caso ocorra um erro, lança uma exceção detalhada.
                _logger.LogError(ex, "Erro ao adicionar pedido.");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Método para atualizar um pedido existente no banco de dados.
        public async Task AtualizarPedidoAsync(PedidoModel pedido)
        {
            try
            {
                // Atualiza o pedido no contexto de pedidos.
                _context.Pedidos.Update(pedido);

                // Tenta salvar as mudanças no banco de dados.
                var result = await _context.SaveChangesAsync();

                // Se não houver alterações no banco (result == 0), lança uma exceção.
                if (result == 0)
                {
                    throw new Exception("Falha ao atualizar o pedido no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Caso ocorra um erro, lança uma exceção detalhada.
                _logger.LogError(ex, "Erro ao atualizar pedido.");
                throw new Exception("Erro ao buscar pedido.");
            }
        }
    }
}
