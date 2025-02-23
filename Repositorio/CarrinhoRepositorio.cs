using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        private readonly BancoContext _context;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CarrinhoRepositorio(BancoContext context,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio)
        {
            _context = context;
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Método que retorna o carrinho de um usuário específico
        public async Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId)
        {
            var carrinho = await _context.Carrinho
                .Include(x => x.Usuario)
                .Include(x => x.Produto)
                .Where(x => x.UsuarioId == usuarioId)
                .ToListAsync();

            if (!carrinho.Any())
            {
                Console.WriteLine("Nenhum item encontrado no carrinho para o usuário de ID " + usuarioId);
            }

            return carrinho;
        }

        // Método que verifica se um produto já existe no carrinho
        public async Task<CarrinhoModel> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId)
        {
            return await _context.Carrinho
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
        }

        // Método que adiciona um produto ao carrinho
        public async Task AddOuRemoverCarrinhoAsync(int produtoId, int usuarioId)
        {
            // Verifica se o produto já está no carrinho
            var produtoNoCarrinho = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

            if (produtoNoCarrinho != null)
            {
                // Remove do carrinho se já estiver lá
                _context.Carrinho.Remove(produtoNoCarrinho);
            }
            else
            {
                // Adiciona ao carrinho se não estiver
                var novoProduto = new CarrinhoModel
                {
                    ProdutoId = produtoId,
                    UsuarioId = usuarioId
                };

                _context.Carrinho.Add(novoProduto);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AtualizarQuantidadeAsync(int produtoId, int usuarioId, int quantidade)
        {
            var produtoNoCarrinho = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

            if (produtoNoCarrinho == null)
            {
                throw new Exception("Produto não encontrado no carrinho");
            }

            produtoNoCarrinho.Quantidade = quantidade;
            _context.Carrinho.Update(produtoNoCarrinho);
            await _context.SaveChangesAsync();
        }
    }
}