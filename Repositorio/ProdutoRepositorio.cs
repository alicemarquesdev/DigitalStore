using DigitalStore.Data;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _context;

        public ProdutoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task AddProdutoAsync(ProdutoModel produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarProdutoAsync(ProdutoModel produto)
        {
            ProdutoModel produtoDb = await BuscarProdutoPorIdAsync(produto.Id);
            if (produtoDb == null) throw new Exception("Houve um erro, ao tentar atualizar os dados do produto.");

            produtoDb.NomeProduto = produto.NomeProduto;
            produtoDb.Preco = produto.Preco;
            produtoDb.Categoria = produto.Categoria;
            produtoDb.ImagemUrl = produto.ImagemUrl;

            _context.Update(produtoDb);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdutoModel> BuscarProdutoPorIdAsync(int id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ProdutoModel>> BuscarTodosProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            ProdutoModel produto = await BuscarProdutoPorIdAsync(id);

            if (produto == null) return false;

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}