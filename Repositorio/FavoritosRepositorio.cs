using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // Repositório responsável por gerenciar as operações relacionadas aos favoritos do usuário.
    // - BuscarFavoritosDoUsuarioAsync(int usuarioId)
    // - BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId)
    // - AddOuRemoverFavoritoAsync(int produtoId, int usuarioId)
    public class FavoritosRepositorio : IFavoritosRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<FavoritosRepositorio> _logger;

        public FavoritosRepositorio(BancoContext context, ILogger<FavoritosRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método que retorna todos os produtos favoritos pelo usuário.
        public async Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId)
        {
            try
            {
                return await _context.Favoritos
                    .Include(f => f.Produto) // Inclui os dados do Produto
                    .Include(f => f.Usuario) // Inclui os dados do Usuario
                    .Where(f => f.UsuarioId == usuarioId) // Filtra pelo UsuarioId
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção com a mensagem de erro
                _logger.LogError(ex, "Erro ao buscar favoritos do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao buscar favoritos.");
            }
        }

        // Método que verifica se um produto já existe nos favoritos de um usuário.
        public async Task<FavoritosModel?> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId)
        {
            try
            {
                return await _context.Favoritos
                    .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção com a mensagem de erro
                _logger.LogError(ex, "Erro ao verificar produto nos favoritos do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao verificar produto nos favoritos.");
            }
        }

        // Método que adiciona ou remove um produto dos favoritos de um usuário.
        public async Task AddOuRemoverFavoritoAsync(int produtoId, int usuarioId)
        {
            try
            {
                // Verifica se o produto já está nos favoritos
                var produtoNoFavorito = await BuscarProdutoExistenteNoFavoritosAsync(produtoId, usuarioId);

                if (produtoNoFavorito == null)
                {
                    // Se o produto não estiver nos favoritos, adiciona-o
                    var favorito = new FavoritosModel
                    {
                        ProdutoId = produtoId,
                        UsuarioId = usuarioId
                    };

                    _context.Favoritos.Add(favorito);
                }
                else
                {
                    // Se o produto já estiver nos favoritos, remove-o
                    _context.Favoritos.Remove(produtoNoFavorito);
                }

                // Salva as mudanças no banco de dados
               var result = await _context.SaveChangesAsync();

                if( result == 0)
                {
                    throw new Exception("Nenhuma ação executada");
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção com a mensagem de erro
                _logger.LogError(ex, "Erro ao adicionar/remover produto nos favoritos do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao adicionar/remover produto nos favoritos.");
            }
        }
    }
}
