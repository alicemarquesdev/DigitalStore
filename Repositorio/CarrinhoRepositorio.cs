using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ISessao _sessao;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CarrinhoRepositorio(BancoContext context,
                                    ISessao sessao,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio)
        {
            _context = context;
            _sessao = sessao;
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId)
        {
            var carrinho = await _context.Carrinho
                .Include(x => x.Usuario)
                .Include(x => x.Produto)
                .Where(x => x.UsuarioId == usuarioId)
                .ToListAsync();

            if (carrinho == null)
            {
                // Log para verificar se a consulta retornou null
                Console.WriteLine("Nenhum item encontrado no carrinho para o usuário de ID " + usuarioId);
            }

            return carrinho;
        }

        public async Task<CarrinhoModel> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId)
        {
            return await _context.Carrinho.FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);
        }

        public async Task AddAoCarrinhoAsync(int produtoId, int usuarioId)
        {
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);
            ProdutoModel produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(produtoId);

            if (produto == null || usuario == null)
            {
                throw new ArgumentException("Produto ou Usuario não encontrado.");
            }

            var produtoDuplicado = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

            if(produtoDuplicado != null)
            {
                throw new ArgumentException("O Produto já está salvo no carrinho.");
            }


            var novoProduto = new CarrinhoModel
            {
                ProdutoId = produtoId,
                UsuarioId = usuarioId
            };

            _context.Carrinho.Add(novoProduto);
            await _context.SaveChangesAsync();
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