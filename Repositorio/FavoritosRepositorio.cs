using DigitalStore.Data;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class FavoritosRepositorio : IFavoritosRepositorio
    {
        private readonly BancoContext _context;

        public FavoritosRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task AddFavoritoAsync(int produtoId, int usuarioId)
        {
            var produtoExistente = BuscarProdutoExistenteNoFavoritosAsync(produtoId, usuarioId);

            if (produtoExistente == null)
            {
                var novoProduto = new FavoritosModel
                {
                    ProdutoId = produtoId,
                    UsuarioId = usuarioId
                };

                _context.Favoritos.Add(novoProduto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId)
        {
            return await _context.Favoritos.Include(x => x.Produto).Where(x => x.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<FavoritosModel> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId)
        {
            return await _context.Favoritos.FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
        }

        public async Task<bool> RemoverFavoritoAsync(int produtoId, int usuarioId)
        {
            FavoritosModel produto = await BuscarProdutoExistenteNoFavoritosAsync(produtoId, usuarioId);

            if (produto == null) return false;

            _context.Favoritos.Remove(produto);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}