using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // Responsável por gerenciar as operações relacionadas a pagamentos no banco de dados.
    // - BuscarPagamentoPorIdAsync(int id)
    // - AddPagamentoAsync(PagamentoModel pagamento)
    // - AtualizarPagamentoAsync(PagamentoModel pagamento)
    public class PagamentoRepositorio : IPagamentoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<PagamentoRepositorio> _logger;

        // Construtor que recebe o contexto do banco de dados (BancoContext).
        public PagamentoRepositorio(BancoContext context, ILogger<PagamentoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método que busca um pagamento pelo seu ID.
        public async Task<PagamentoModel?> BuscarPagamentoPorIdAsync(int id)
        {
            try
            {
                // Retorna o primeiro pagamento que corresponde ao ID fornecido ou null caso não exista.
                return await _context.Pagamentos.FirstOrDefaultAsync(x => x.PagamentoId == id);
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção que ocorrer e lança uma nova exceção com a mensagem detalhada do erro.
                _logger.LogError(ex, "Erro ao buscar pagamento com ID {PagamentoId}", id);
                throw new Exception("Erro ao buscar pagamento.");
            }
        }

        // Método que adiciona um pagamento ao banco de dados.
        public async Task AddPagamentoAsync(PagamentoModel pagamento)
        {
            try
            {
                // Adiciona o novo pagamento à tabela de pagamentos no contexto.
                _context.Pagamentos.Add(pagamento);

                // Salva as mudanças no banco de dados de forma assíncrona.
                var result = await _context.SaveChangesAsync();

                // Se o resultado da operação for 0, significa que não houve alteração no banco de dados.
                if (result == 0)
                {
                    // Lança uma exceção caso a operação não tenha sido bem-sucedida.
                    throw new Exception("Falha ao adicionar pagamento no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma nova exceção com a mensagem detalhada.
                _logger.LogError(ex, "Erro ao adicionar pagamento.");
                throw new Exception("Erro ao adicionar pagamento.");
            }
        }

        // Método que atualiza um pagamento existente no banco de dados.
        public async Task AtualizarPagamentoAsync(PagamentoModel pagamento)
        {
            try
            {
                // Remove o pagamento atual do contexto (presumivelmente ele será atualizado em seguida).
                _context.Pagamentos.Remove(pagamento);

                // Salva as mudanças no banco de dados.
                var result = await _context.SaveChangesAsync();

                // Se o resultado for 0 ou menor, significa que a operação não foi bem-sucedida.
                if (result <= 0)
                {
                    // Lança uma exceção caso não tenha ocorrido nenhuma atualização.
                    throw new Exception("Falha ao atualizar pagamento no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma nova exceção com a mensagem detalhada do erro.
                _logger.LogError(ex, "Erro ao atualizar pagamento.");
                throw new Exception("Erro ao atualizar pagamento.");
            }
        }
    }
}
