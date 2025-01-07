using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _context;
        private readonly string _sistema;

        public ProdutoRepositorio(BancoContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _sistema = sistema.WebRootPath;
        }

        // Método para buscar categorias distintas de produtos
        public async Task<List<ProdutoModel>> BuscarCategoriasAsync()
        {
            var categorias = await _context.Produtos.Select(x => x.Categoria).Distinct().ToListAsync();

            var categoriaList = categorias.Select(categoria => new ProdutoModel
            {
                Categoria = categoria
            }).ToList();

            return categoriaList;
        }

        // Método para buscar produtos por categoria
        public async Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(ProdutoModel categoria)
        {
            return await _context.Produtos
                .Where(c => c.Categoria == categoria.Categoria)
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

        // Método para gerar caminho de arquivo da imagem do produto
        public async Task<string> GerarCaminhoArquivoAsync(IFormFile imagem)
        {
            // Gera um código único para o arquivo
            var codigoUnico = Guid.NewGuid().ToString();

            // Obtém a extensão original do arquivo (ex: ".jpg", ".png")
            var extensao = Path.GetExtension(imagem.FileName).ToLower();

            // Gera o nome final do arquivo (sem espaços e com a extensão original)
            var nomeCaminhoImagem = codigoUnico + extensao;

            // Caminho para salvar a imagem, dentro da pasta "wwwroot/images"
            var caminhoParaSalvarImagem = Path.Combine(_sistema, "image");

            // Verifica se a pasta existe e, se não, cria
            if (!Directory.Exists(caminhoParaSalvarImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagem);
            }

            // Salva o arquivo no caminho especificado
            var caminhoCompleto = Path.Combine(caminhoParaSalvarImagem, nomeCaminhoImagem);
            using (var stream = File.Create(caminhoCompleto))
            {
                await imagem.CopyToAsync(stream);
            }

            return Path.Combine("~/image", nomeCaminhoImagem).Replace("\\", "/");
        }

        // Método para adicionar um novo produto
        public async Task AddProdutoAsync(ProdutoModel produto, IFormFile imagem)
        {
            try
            {
                var caminhoImagem = await GerarCaminhoArquivoAsync(imagem);

                var produtoDb = new ProdutoModel
                {
                    NomeProduto = produto.NomeProduto,
                    Descricao = produto.Descricao,
                    Categoria = produto.Categoria,
                    Preco = produto.Preco,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    ImagemUrl = caminhoImagem
                };

                // Adiciona o produto ao banco de dados
                await _context.Produtos.AddAsync(produtoDb);
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
                var caminhoImagem = await GerarCaminhoArquivoAsync(novaImagem);
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

        // Método para buscar os últimos produtos adicionados
        public async Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionados()
        {
            return await _context.Produtos
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }
    }
}