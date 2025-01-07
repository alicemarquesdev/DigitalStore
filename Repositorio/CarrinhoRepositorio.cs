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
        public async Task AddAoCarrinhoAsync(int produtoId, int usuarioId)
        {
            // Verifica se o usuário e o produto existem
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);
            ProdutoModel produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(produtoId);

            if (produto == null || usuario == null)
            {
                throw new ArgumentException("Produto ou Usuario não encontrado.");
            }

            // Verifica se o produto já está no carrinho
            var produtoDuplicado = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);
            if (produtoDuplicado != null)
            {
                throw new ArgumentException("O Produto já está salvo no carrinho.");
            }

            // Cria um novo carrinho com o produto
            var novoProduto = new CarrinhoModel
            {
                ProdutoId = produtoId,
                UsuarioId = usuarioId
            };

            // Adiciona o produto ao carrinho no banco de dados
            _context.Carrinho.Add(novoProduto);
            await _context.SaveChangesAsync();
        }

        // Método que remove um produto do carrinho
        public async Task<bool> RemoverDoCarrinhoAsync(int produtoId, int usuarioId)
        {
            // Busca o produto no carrinho
            CarrinhoModel produto = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

            // Se o produto não existir, retorna false
            if (produto == null) return false;

            // Remove o produto do carrinho e salva as alterações
            _context.Carrinho.Remove(produto);
            await _context.SaveChangesAsync();

            return true;
        }

        // Método que retorna o total de produtos no carrinho de um usuário
        public async Task<int> TotalProdutosNoCarrinhoDoUsuarioAsync(int usuarioId)
        {
            var carrinho = await BuscarCarrinhoDoUsuarioAsync(usuarioId);

            // Retorna a quantidade de produtos no carrinho
            return carrinho.Count();
        }
    }
}