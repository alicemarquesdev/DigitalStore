using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly BancoContext _context;

        public PedidoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task<PedidoModel> BuscarPedidoPorIdAsync(int id)
        {
            var pedido = await _context.Pedidos.Include(e => e.Endereco).Include(p => p.Pagamento).FirstOrDefaultAsync(x => x.PedidoId == id);

            if (pedido == null) throw new Exception("Nenhum pedido encontrado");

            return pedido;
        }

        public async Task<List<PedidoModel>> BuscarTodosOsPedidosDoUsuarioAsync(int usuarioId)
        {
            var pedidos = await _context.Pedidos.Include(e => e.Endereco).Include(p => p.Pagamento).Where(x => x.UsuarioId == usuarioId).ToListAsync();

            if (pedidos == null) throw new Exception("Nenhum pedido encontrado");

            return pedidos;
        }

        public async Task AddPedidoAsync(PedidoModel pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }
    }
}