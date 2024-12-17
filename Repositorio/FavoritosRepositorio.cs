using DigitalStore.Data;
using DigitalStore.Helper;
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
        private readonly ISessao _sessao;

        public FavoritosRepositorio(BancoContext context,
            IProdutoRepositorio produtoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ISessao sessao)
        {
            _context = context;
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public async Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId)
        {
            return await _context.Favoritos
          .Include(f => f.Produto) // Inclui os dados do Produto
          .Include(f => f.Usuario) // Inclui os dados do Usuario
          .Where(f => f.UsuarioId == usuarioId) // Filtra pelo UsuarioId
          .ToListAsync();
        }

        public async Task<FavoritosModel> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId)
        {
            return await _context.Favoritos.FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
        }

        public async Task AddFavoritoAsync(int produtoId, int usuarioId)
        {
            ProdutoModel produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(produtoId);
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);

            if (produto == null || usuario == null)
            {
                throw new ArgumentException("Produto ou Usuario não encontrado.");
            }

            var favorito = new FavoritosModel
            {
                ProdutoId = produtoId,
                UsuarioId = usuarioId
            };

            await _context.Favoritos.AddAsync(favorito);
            await _context.SaveChangesAsync();
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