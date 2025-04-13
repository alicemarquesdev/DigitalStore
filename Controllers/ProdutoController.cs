using DigitalStore.Filters;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using DigitalStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    // Controlador responsável pelo gerenciamento de produtos.
    // - GerenciamentoProdutos: Exibe todos os produtos, categorias e um formulário para adicionar novos produtos.
    // - AddProduto: Adiciona um novo produto ao estoque.
    // - AtualizarProduto: Atualiza os dados de um produto existente.
    // - RemoverProduto: Remove um produto do estoque.
    [PaginaAdmin]
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ICaminhoImagem _caminhoImagem;
        private readonly ILogger<ProdutoController> _logger;

        // Construtor para injeção de dependência
        public ProdutoController(IProdutoRepositorio produtoRepositorio,
                                 ICaminhoImagem caminhoImagem,
                                    ILogger<ProdutoController> logger)
        {
            _produtoRepositorio = produtoRepositorio ?? throw new ArgumentNullException(nameof(produtoRepositorio));
            _caminhoImagem = caminhoImagem ?? throw new ArgumentNullException(nameof(caminhoImagem));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método para exibir os produtos, categorias e formulário de gerenciamento
        public async Task<IActionResult> GerenciamentoProdutos()
        {
            try
            {
                // Criando uma nova instância de ProdutoModel com valores válidos para evitar erros de validação
                var produto = new ProdutoModel
                {
                    NomeProduto = string.Empty,    // Nome do produto vazio (pode ser alterado conforme necessário)
                    Descricao = string.Empty,      // Descrição vazia
                    Categoria = string.Empty,     // Categoria vazia
                    Preco = 0.0m,                  // Preço inicializado com zero
                    QuantidadeEstoque = 0,        // Estoque inicializado com zero
                    ImagemUrl = string.Empty,     // URL da imagem vazia (pode ser preenchido depois)
                };

                var viewModel = new GerenciamentoProdutoViewModel
                {
                    Produto = produto,
                    Categorias = await _produtoRepositorio.BuscarCategoriasAsync(),
                    Produtos = await _produtoRepositorio.BuscarTodosOsProdutosAsync(),
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar a página de gerenciamento de produtos.");
                TempData["Alerta"] = "Erro ao carregar a página de gerenciamento de produtos.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Método para adicionar um produto
        [HttpPost]
        public async Task<IActionResult> AddProduto(ProdutoModel produto, IFormFile imagem)
        {
            try
            {
                // Verifica se a imagem foi enviada corretamente
                if (imagem == null || imagem.Length == 0)
                {
                    TempData["Alerta"] = "A imagem não foi enviada!";
                    return RedirectToAction("GerenciamentoProdutos", produto);
                }
            
                // Processa a imagem e gera o caminho
                var caminhoImagem = await _caminhoImagem.GerarCaminhoArquivoAsync(imagem);
                if (caminhoImagem == null || caminhoImagem.Length == 0)
                {
                    throw new Exception("caminho da imagem não foi enviada corretamente!");
                }

                // Adiciona o caminho da imagem ao produto
                produto.ImagemUrl = caminhoImagem;


                if (ModelState.IsValid)
                {
                    await _produtoRepositorio.AddProdutoAsync(produto);
                    TempData["Alerta"] = "Produto adicionado com sucesso!";
                    return RedirectToAction("GerenciamentoProdutos", "Produto");
                }

                TempData["Alerta"] = "Verifique os dados do produto, houve um erro ao tentar adicionar.";
                return RedirectToAction("GerenciamentoProdutos", produto);
            }
            catch (Exception ex)
            {
                // Captura erros e exibe a mensagem
                _logger.LogError(ex, "Erro ao adicionar o produto");
                TempData["Alerta"] = "Erro ao adicionar o produto, tente novamente";
                return RedirectToAction("GerenciamentoProdutos", produto);
            }
        }


        // Método para atualizar um produto
        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(GerenciamentoProdutoViewModel viewModel, IFormFile? novaImagem)
        {
            try
            {
                // Verifica se o ModelState é válido
                if (ModelState.IsValid)
                {
                    // Busca o produto existente no banco de dados
                    var produtoDb = await _produtoRepositorio.BuscarProdutoPorIdAsync(viewModel.Produto.ProdutoId);

                    if (produtoDb == null)
                    {
                        // Caso o produto não seja encontrado, retorna erro
                        throw new ArgumentException("Produto não encontrado.");
                    }
                    // Atualiza a imagem se for fornecida e apaga a antiga imagem do servidor
                    if (novaImagem != null)
                    {
                        var caminhoImagem = await _caminhoImagem.GerarCaminhoArquivoAsync(novaImagem);
                        if (caminhoImagem == null || caminhoImagem.Length == 0)
                        {
                            throw new ArgumentException("Erro ao gerar arquivo de imagem.");
                        }

                        // Remover a imagem antiga, se necessário
                        await _caminhoImagem.RemoverImagemAntiga(produtoDb.ImagemUrl);

                        // Atualiza a imagem do produto com o novo caminho
                        produtoDb.ImagemUrl = caminhoImagem;
                    }

                    // Atualiza as propriedades do produto com as novas informações da viewModel
                    produtoDb.NomeProduto = viewModel.Produto.NomeProduto;
                    produtoDb.Descricao = viewModel.Produto.Descricao;
                    produtoDb.Categoria = viewModel.Produto.Categoria;
                    produtoDb.Preco = viewModel.Produto.Preco;
                    produtoDb.QuantidadeEstoque = viewModel.Produto.QuantidadeEstoque;

                    await _produtoRepositorio.AtualizarProdutoAsync(produtoDb);

                    TempData["Alerta"] = "Produto atualizado com sucesso!";

                    // Após a atualização, redireciona para a página de gerenciamento de produtos
                    return RedirectToAction("GerenciamentoProdutos");

                }
                // Se o ModelState não for válido, recarrega as categorias e produtos para mostrar erros
                var categorias = await _produtoRepositorio.BuscarCategoriasAsync();
                viewModel.Categorias = categorias;
                viewModel.Produtos = await _produtoRepositorio.BuscarTodosOsProdutosAsync();

                // Retorna a mesma view com as mensagens de erro
                return View("GerenciamentoProdutos", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o produto.");
                TempData["Alerta"] = "Erro ao atualizar o produto.";
                return View("GerenciamentoProdutos");
            }
        }

        // Método para remover um produto
        [HttpPost]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            try
            {
                // Verifica se o ID do produto é válido (maior que 0)
                if (id <= 0)
                {
                    throw new ArgumentException("O ID do produto não foi informado.");
                }

                // Busca o produto no repositório com o ID fornecido
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

                // Verifica se o produto foi encontrado
                if (produto == null)
                {
                    throw new ArgumentException("Produto não encontrado no banco de dados.");
                }

                // Remove a imagem associada ao produto do servidor, chamando o método RemoverImagemAntiga
                await _caminhoImagem.RemoverImagemAntiga(produto.ImagemUrl);

                // Remove o produto do banco de dados através do repositório
                bool sucesso = await _produtoRepositorio.RemoverProdutoAsync(produto);

                // Verifica se a remoção foi bem-sucedida
                if (sucesso)
                {
                    // Adiciona uma mensagem de sucesso para ser exibida na próxima requisição
                    TempData["Alerta"] = "Produto removido com sucesso.";
                }

                // Redireciona o usuário para a página de gerenciamento de produtos
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }
            catch (Exception ex)
            {
                // Loga o erro ocorrido durante a remoção do produto
                _logger.LogError(ex, "Erro ao remover produto.");

                // Adiciona uma mensagem de erro para ser exibida na próxima requisição
                TempData["Alerta"] = "Desculpe, houve um erro ao tentar remover o produto";

                // Redireciona o usuário de volta para a página de gerenciamento de produtos
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }
        }
    }
}
