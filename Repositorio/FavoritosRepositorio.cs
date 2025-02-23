using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class FavoritosRepositorio : IFavoritosRepositorio
    {
        private readonly BancoContext _context;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public FavoritosRepositorio(BancoContext context,
                                    IProdutoRepositorio produtoRepositorio,
                                    IUsuarioRepositorio usuarioRepositorio)
        {
            _context = context;
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Método para buscar todos os favoritos de um usuário
        public async Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId)
        {
            return await _context.Favoritos
                .Include(f => f.Produto) // Inclui os dados do Produto
                .Include(f => f.Usuario) // Inclui os dados do Usuario
                .Where(f => f.UsuarioId == usuarioId) // Filtra pelo UsuarioId
                .ToListAsync();
        }

        // Método para verificar se um produto já existe nos favoritos de um usuário
        public async Task<FavoritosModel> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId)
        {
            return await _context.Favoritos
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
        }

        // Método para adicionar um produto aos favoritos
        public async Task AddOuRemoverFavoritoAsync(int produtoId, int usuarioId)
        {
            var produtoNoFavorito = await BuscarProdutoExistenteNoFavoritosAsync(produtoId, usuarioId);

            if (produtoNoFavorito == null)
            {
                var favorito = new FavoritosModel
                {
                    ProdutoId = produtoId,
                    UsuarioId = usuarioId
                };

                _context.Favoritos.Add(favorito);
            }
            else
            {
                _context.Favoritos.Remove(produtoNoFavorito);
            }

            await _context.SaveChangesAsync();
        }
    }
}