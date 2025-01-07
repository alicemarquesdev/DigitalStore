using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class ItensDoPedidoRepositorio : IItensDoPedidoRepositorio
    {
        private readonly BancoContext _context;

        public ItensDoPedidoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task<ItensDoPedidoModel> BuscarItemDoPedidoPorIdAsync(int pedidoId)
        {
            return await _context.ItensDoPedido.FirstOrDefaultAsync(x => x.PedidoId == pedidoId);
        }

        public async Task<List<ItensDoPedidoModel>> BuscarTodosOsItensDoPedidoAsync(int pedidoId)
        {
            return await _context.ItensDoPedido.Where(x => x.PedidoId == pedidoId).ToListAsync();
        }

        public async Task AddItemAsync(ItensDoPedidoModel item)
        {
            await _context.ItensDoPedido.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarItemAsync(ItensDoPedidoModel item)
        {
            var itemDb = await BuscarItemDoPedidoPorIdAsync(item.ItemId);

            itemDb.QuantidadeDeProdutos = item.QuantidadeDeProdutos;

            _context.Update(itemDb);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverItemAsync(int id)
        {
            var item = await BuscarItemDoPedidoPorIdAsync(id);

            if (item == null) return false;

            _context.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}