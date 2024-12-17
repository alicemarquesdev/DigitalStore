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

        public async Task<List<ProdutoModel>> BuscarCategoriasAsync()
        {
            var categorias = await _context.Produtos.Select(x => x.Categoria).Distinct().ToListAsync();

            var categoriaList = categorias.Select(categoria => new ProdutoModel
            {
                Categoria = categoria
            }).ToList();

            return categoriaList;
        }

        public async Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(ProdutoModel categoria)
        {
            return await _context.Produtos.Where(c => c.Categoria == categoria.Categoria).ToListAsync();
        }

        public async Task<ProdutoModel> BuscarProdutoPorIdAsync(int id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(x => x.ProdutoId == id);
        }

        public async Task<List<ProdutoModel>> BuscarTodosProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<string> GerarCaminhoArquivoAsync(IFormFile imagem)
        {
            // Gera um código único para o arquivo
            var codigoUnico = Guid.NewGuid().ToString();

            // Obtém a extensão original do arquivo (ex: ".jpg", ".png")
            var extensao = Path.GetExtension(imagem.FileName).ToLower();

            // Gera o nome final do arquivo (sem espaços e com a extensão original)
            var nomeCaminhoImagem = codigoUnico + extensao;

            // Caminho para salvar a imagem, dentro da pasta "wwwroot/images"
            var caminhoParaSalvarImagem = Path.Combine(_sistema, "~/image");

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

                await _context.Produtos.AddAsync(produtoDb);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarProdutoAsync(ProdutoModel produto, IFormFile? novaImagem)
        {
            ProdutoModel produtoDb = await BuscarProdutoPorIdAsync(produto.ProdutoId);
            if (produtoDb == null) throw new Exception("Houve um erro, ao tentar atualizar os dados do produto.");

            produtoDb.NomeProduto = produto.NomeProduto;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Preco = produto.Preco;
            produtoDb.Categoria = produto.Categoria;
            produtoDb.QuantidadeEstoque = produto.QuantidadeEstoque;

            if (novaImagem != null)
            {
                // Opcional: Remover a imagem antiga
                if (!string.IsNullOrEmpty(produtoDb.ImagemUrl) && string.IsNullOrEmpty(produto.ImagemUrl))
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

            _context.Produtos.Update(produtoDb);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            ProdutoModel produto = await BuscarProdutoPorIdAsync(id);

            if (produto == null) return false;

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionados()
        {
            return await _context.Produtos.OrderByDescending(p => p.DataCadastro).ToListAsync();
        }
    }
}