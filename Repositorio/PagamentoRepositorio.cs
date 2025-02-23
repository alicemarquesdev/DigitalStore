using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;

namespace DigitalStore.Repositorio
{
    public class PagamentoRepositorio : IPagamentoRepositorio
    {
        private readonly BancoContext _context;

        public PagamentoRepositorio(BancoContext context) 
        {
            _context = context;
        }

        public async Task AddPagamento(PagamentoModel pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();
        }
    }
}
