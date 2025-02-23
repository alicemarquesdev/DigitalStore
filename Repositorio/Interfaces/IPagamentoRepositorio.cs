using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IPagamentoRepositorio
    {
        Task AddPagamento(PagamentoModel pagamento);
    }
}
