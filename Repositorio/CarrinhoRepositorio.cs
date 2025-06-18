using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // A classe CarrinhoRepositorio é responsável por gerenciar as operações relacionadas ao carrinho de compras de um usuário,
    // - BuscarCarrinhoDoUsuarioAsync(int usuarioId)
    // - BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId)
    // - AddOuRemoverCarrinhoAsync(int produtoId, int usuarioId)
    // - AtualizarQuantidadeAsync(int produtoId, int usuarioId, int quantidade)

    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<CarrinhoRepositorio> _logger;

        // Injeção de dependência para o contexto do banco de dados e os repositórios de Produto e Usuário
        public CarrinhoRepositorio(BancoContext context, ILogger<CarrinhoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método que retorna o carrinho de um usuário específico
        public async Task<List<CarrinhoModel>> BuscarCarrinhoDoUsuarioAsync(int usuarioId)
        {
            try
            {
                // Busca todos os itens no carrinho de um usuário específico, inclui detalhes de usuário e produto
                return await _context.Carrinho
                    .Include(x => x.Usuario)
                    .Include(x => x.Produto)
                    .Where(x => x.UsuarioId == usuarioId)
                    .ToListAsync();              
            }
            catch (Exception ex)
            {
                // Captura exceções, caso ocorra algum erro na busca
                _logger.LogError(ex, "Erro ao buscar o carrinho do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao buscar o carrinho.");
            }
        }

        // Método que verifica se um produto já existe no carrinho
        public async Task<CarrinhoModel?> BuscarProdutoExistenteNoCarrinhoAsync(int produtoId, int usuarioId)
        {
            try
            {
                // Tenta buscar o produto no carrinho do usuário
                return await _context.Carrinho
                    .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UsuarioId == usuarioId);

            }
            catch (Exception ex)
            {
                // Captura exceções, caso ocorra algum erro na busca
                _logger.LogError(ex, "Erro ao verificar produto no carrinho do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao verificar produto no carrinho.");
            }
        }

        // Método que adiciona ou remove um produto do carrinho
        public async Task AddOuRemoverCarrinhoAsync(int produtoId, int usuarioId)
        {
            try
            {
                // Verifica se o produto já existe no carrinho do usuário
                var produtoNoCarrinho = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

                // Se o produto já está no carrinho, remove-o
                if (produtoNoCarrinho != null)
                {
                    _context.Carrinho.Remove(produtoNoCarrinho); // Produto foi removido do carrinho
                }
                else
                {
                    // Se o produto não está no carrinho, cria um novo registro para adicionar o produto
                    var novoProduto = new CarrinhoModel
                    {
                        ProdutoId = produtoId,
                        UsuarioId = usuarioId
                    };

                    _context.Carrinho.Add(novoProduto); // Produto foi adicionado ao carrinho
                }

                // Salva as alterações no banco de dados
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    // Caso não tenha ocorrido alteração alguma, lança exceção informando
                    throw new InvalidOperationException("Nenhuma alteração foi realizada no carrinho.");
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer erro e lança uma exceção com uma mensagem clara
                _logger.LogError(ex, "Erro ao adicionar ou remover produto no carrinho do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao adicionar ou remover produto no carrinho.");
            }
        }


        // Método que atualiza a quantidade de um produto no carrinho
        public async Task AtualizarQuantidadeAsync(int produtoId, int usuarioId, int quantidade)
        {
            try
            {
                // Verifica se o produto está no carrinho
                var produtoNoCarrinho = await BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

                // Se o produto não for encontrado, lança uma exceção
                if (produtoNoCarrinho == null)
                {
                    throw new Exception("Produto não encontrado no carrinho");
                }

                // Atualiza a quantidade do produto no carrinho
                produtoNoCarrinho.Quantidade = quantidade;
                _context.Carrinho.Update(produtoNoCarrinho);

                // Salva as alterações no banco de dados
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    // Caso não tenha ocorrido alteração alguma, lança exceção informando
                    throw new InvalidOperationException("Nenhuma alteração foi realizada no carrinho.");
                }
            }
            catch (Exception ex)
            {
                // Captura exceções, caso ocorra algum erro na atualização
                _logger.LogError(ex, "Erro ao atualizar a quantidade do produto no carrinho do usuário com ID {UsuarioId}", usuarioId);
                throw new Exception("Erro ao atualizar a quantidade do produto no carrinho.");
            }
        }
    }
}
