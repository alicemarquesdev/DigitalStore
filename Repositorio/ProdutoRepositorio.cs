using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // Classe responsável por manipular operações relacionadas aos produtos no banco de dados.
    // Métodos:
    // - BuscarCategoriasAsync() - Retorna uma lista com todas as categorias distintas de produtos.
    // - BuscarProdutosPorCategoriaAsync(string categoria) - Retorna uma lista de produtos filtrados por categoria.
    // - BuscarProdutosBarraDePesquisaAsync(string termo) - Retorna uma lista de produtos filtrados por um termo de pesquisa.
    // - BuscarProdutoPorIdAsync(int id) - Busca um produto específico pelo ID.
    // - BuscarTodosOsProdutosAsync() - Retorna uma lista com todos os produtos cadastrados.
    // - BuscarUltimosProdutosAdicionadosAsync() - Retorna uma lista com os últimos produtos adicionados.
    // - AddProdutoAsync(ProdutoModel produto) - Adiciona um novo produto ao banco de dados.
    // - AtualizarProdutoAsync(ProdutoModel produto) - Atualiza os dados de um produto existente.
    // - RemoverProdutoAsync(ProdutoModel produto) - Remove um produto do banco de dados.

    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<ProdutoRepositorio> _logger;

        // Construtor que recebe o contexto do banco de dados.
        public ProdutoRepositorio(BancoContext context, ILogger<ProdutoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Retorna uma lista com todas as categorias distintas de produtos.
        public async Task<List<string>> BuscarCategoriasAsync()
        {
            try
            {
                // Seleciona a categoria de cada produto e remove duplicatas.
                return await _context.Produtos
                    .Select(x => x.Categoria)
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar buscar as categorias.
                _logger.LogError(ex, "Erro ao buscar categorias de produtos.");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Retorna uma lista de produtos filtrados por categoria.
        public async Task<List<ProdutoModel>> BuscarProdutosPorCategoriaAsync(string categoria)
        {
            try
            {
                // Filtra os produtos pelo nome da categoria.
                return await _context.Produtos
                    .Where(c => c.Categoria == categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar buscar os produtos por categoria.
                _logger.LogError(ex, "Erro ao buscar produtos pela categoria");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Retorna uma lista de produtos filtrados por um termo de pesquisa (nome ou categoria).
        public async Task<List<ProdutoModel>> BuscarProdutosBarraDePesquisaAsync(string termo)
        {
            try
            {
                // Filtra produtos cujos nomes ou categorias contêm o termo informado.
                return await _context.Produtos
                    .Where(x => x.NomeProduto.Contains(termo) || x.Categoria.Contains(termo))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Erro ao buscar produtos pela barra de pesquisa");
                throw new Exception("Erro ao buscar pedido.");
            }
        }

        // Busca um produto específico pelo ID.
        public async Task<ProdutoModel?> BuscarProdutoPorIdAsync(int id)
        {
            try
            {
                // Retorna o primeiro produto que corresponde ao ID informado.
                return await _context.Produtos
                    .FirstOrDefaultAsync(x => x.ProdutoId == id);
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar buscar o produto pelo ID.
                _logger.LogError(ex, "Erro ao buscar produto");
                throw new Exception("Erro ao buscar o produto.");
            }
        }

        // Retorna uma lista com todos os produtos cadastrados.
        public async Task<List<ProdutoModel>> BuscarTodosOsProdutosAsync()
        {
            try
            {
                // Retorna todos os produtos cadastrados no banco.
                return await _context.Produtos.ToListAsync();
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar buscar todos os produtos.
                _logger.LogError(ex, "Erro ao buscar todos os produtos");
                throw new Exception("Erro ao buscar todos os produtos");
            }
        }

        // Retorna uma lista com os últimos produtos adicionados, ordenados por data de cadastro.
        public async Task<List<ProdutoModel>> BuscarUltimosProdutosAdicionadosAsync()
        {
            try
            {
                // Ordena os produtos da data mais recente para a mais antiga.
                return await _context.Produtos
                    .OrderByDescending(p => p.DataCadastro)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar buscar os últimos produtos adicionados.
                _logger.LogError(ex, "Erro ao buscar os últimos produtos adicionados");
                throw new Exception("Erro ao buscar os últimos produtos adicionados");
            }
        }

        // Adiciona um novo produto ao banco de dados e verifica se a inserção foi bem-sucedida.
        public async Task AddProdutoAsync(ProdutoModel produto)
        {
            try
            {
                produto.NomeProduto = produto.NomeProduto.Trim();
                produto.Descricao = produto.Descricao.Trim();
                produto.Categoria = produto.Categoria.Trim();

                // Adiciona o produto ao contexto.
                await _context.Produtos.AddAsync(produto);

                // Salva as alterações no banco e verifica se foi salvo.
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    // Lança uma exceção caso nenhum produto tenha sido adicionado.
                    throw new Exception($"Nenhum produto adicionado");
                }
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar adicionar o produto.
                _logger.LogError(ex, "Erro ao adicionar produto");
                throw new Exception("Erro ao adicionar o produto.");
            }
        }

        // Atualiza os dados de um produto existente, verificando se foi encontrado no banco.
        public async Task AtualizarProdutoAsync(ProdutoModel produto)
        {
            try
            {
                // Busca o produto pelo ID antes de atualizar.
                ProdutoModel? produtoDb = await BuscarProdutoPorIdAsync(produto.ProdutoId);

                if (produtoDb == null)
                    // Lança uma exceção caso o produto não seja encontrado.
                    throw new Exception("Produto não encontrado para atualização.");

                // Atualiza os dados do produto existente.
                produtoDb.NomeProduto = produto.NomeProduto.Trim();
                produtoDb.Descricao = produto.Descricao.Trim();
                produtoDb.Preco = produto.Preco;
                produtoDb.Categoria = produto.Categoria.Trim();
                produtoDb.QuantidadeEstoque = produto.QuantidadeEstoque;
                produtoDb.ImagemUrl = produto.ImagemUrl;

                // Marca o produto como atualizado no contexto.
                _context.Produtos.Update(produtoDb);

                // Salva as alterações e verifica se houve atualização.
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    // Lança uma exceção caso nenhum produto tenha sido atualizado.
                    throw new Exception("Nenhum produto atualizado");
                }
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar atualizar o produto.
                _logger.LogError(ex, "Erro ao atualizar produto");
                throw new Exception("Erro ao atualizar o produto.");
            }
        }

        // Remove um produto do banco de dados e retorna se a operação foi bem-sucedida.
        public async Task<bool> RemoverProdutoAsync(ProdutoModel produto)
        {
            try
            {
                // Remove o produto do contexto.
                _context.Produtos.Remove(produto);

                // Salva as alterações no banco e retorna se a remoção foi bem-sucedida.
                var resultado = await _context.SaveChangesAsync();

                return resultado > 0;
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar remover o produto.
                _logger.LogError(ex, "Erro ao remover produto");
                throw new Exception("Erro ao remover o produto.");
            }
        }
    }
}
