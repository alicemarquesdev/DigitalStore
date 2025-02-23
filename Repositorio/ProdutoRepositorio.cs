using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ICaminhoImagem _caminhoImagem;

        public ProdutoRepositorio(BancoContext context, ICaminhoImagem caminhoImagem)
        {
            _context = context;
            _caminhoImagem = caminhoImagem;
        }

        // Método para buscar categorias distintas de produtos
        public async Task<List<string>> BuscarCategoriasAsync()
        {
            var categorias = await _context.Produtos.Select(x => x.Categoria).Distinct().ToListAsync();

            return categorias;
        }

        // Método para buscar produtos por categoria
        public async Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(string categoria)
        {
            return await _context.Produtos
                .Where(c => c.Categoria == categoria)
                .ToListAsync();
        }

        // Método para buscar um produto por ID
        public async Task<ProdutoModel> BuscarProdutoPorIdAsync(int id)
        {
            return await _context.Produtos
                .FirstOrDefaultAsync(x => x.ProdutoId == id);
        }

        // Método para buscar todos os produtos
        public async Task<List<ProdutoModel>> BuscarTodosProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        // Método para buscar os últimos produtos adicionados
        public async Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionados()
        {
            return await _context.Produtos
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }

        // Método para adicionar um novo produto
        public async Task AddProdutoAsync(ProdutoModel produto)
        {
            try
            {
                              // Adiciona o produto ao banco de dados
                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar o produto: " + ex.Message);
            }
        }

        // Método para atualizar um produto
        public async Task AtualizarProdutoAsync(ProdutoModel produto, IFormFile? novaImagem)
        {
            ProdutoModel produtoDb = await BuscarProdutoPorIdAsync(produto.ProdutoId);

            if (produtoDb == null)
                throw new Exception("Produto não encontrado para atualização.");

            produtoDb.NomeProduto = produto.NomeProduto;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Preco = produto.Preco;
            produtoDb.Categoria = produto.Categoria;
            produtoDb.QuantidadeEstoque = produto.QuantidadeEstoque;

            if (novaImagem != null)
            {
                // Remover a imagem antiga, se necessário
                if (!string.IsNullOrEmpty(produtoDb.ImagemUrl))
                {
                    var caminhoAntigo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", produtoDb.ImagemUrl.TrimStart('~', '/').Replace("/", "\\"));
                    if (File.Exists(caminhoAntigo))
                    {
                        File.Delete(caminhoAntigo);
                    }
                }

                // Gerar o novo caminho da imagem
                var caminhoImagem = await _caminhoImagem.GerarCaminhoArquivoAsync(novaImagem);
                produtoDb.ImagemUrl = caminhoImagem;
            }

            // Atualiza o produto no banco de dados
            _context.Produtos.Update(produtoDb);
            await _context.SaveChangesAsync();
        }

        // Método para remover um produto
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