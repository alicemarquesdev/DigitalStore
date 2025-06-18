using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que declara os métodos para manipulação dos dados de pagamento no banco de dados.
    public interface IPagamentoRepositorio
    {
        // Método para buscar pagamento por id.
        Task<PagamentoModel?> BuscarPagamentoPorIdAsync(int id);

        // Método para adicionar um pagamento no banco de dados.
        Task AddPagamentoAsync(PagamentoModel pagamento);

        // Método para atualizar pagamento.
        Task AtualizarPagamentoAsync(PagamentoModel pagamento);
    }
}
