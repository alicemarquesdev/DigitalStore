using DigitalStore.Data;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        private readonly BancoContext _context;

        public CarrinhoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task AddAoCarrinhoAsync(int produtoId, int usuarioId)
        {
            var produtoExistente = BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

            if (produtoExistente == null)
            {
                var novoProduto = new CarrinhoModel
                {
                    ProdutoId = produtoId,
                    UsuarioId = usuarioId
                };

                _context.Carrinho.Add(novoProduto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId)
        {
            return await _context.Carrinho.Include(x => x.Produto).Where(x => x.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<CarrinhoModel> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId)
        {
            return await _context.Carrinho.FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
        }

        public async Task<bool> RemoverDoCarrinhoAsync(int produtoId, int usuarioId)
        {
            CarrinhoModel produto = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

            if (produto == null) return false;

            _context.Carrinho.Remove(produto);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}