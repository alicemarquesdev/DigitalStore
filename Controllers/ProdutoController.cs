using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Models.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ICaminhoImagem _caminhoImagem;

        // Construtor para injeção de dependência
        public ProdutoController(IProdutoRepositorio produtoRepositorio, ICaminhoImagem caminhoImagem)
        {
            _produtoRepositorio = produtoRepositorio;
            _caminhoImagem = caminhoImagem;
        }

        public async Task<IActionResult> GerenciamentoProdutos()
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
                Produtos = await _produtoRepositorio.BuscarTodosProdutosAsync(),
            };

            return View(viewModel);
        }

        // Método para adicionar um produto
        [HttpPost]
        public async Task<IActionResult> AddProduto(ProdutoModel produto, IFormFile imagem)
        {
            // Verifica se a imagem foi enviada corretamente
            if (imagem == null || imagem.Length == 0)
            {
                TempData["MensagemErro"] = "A imagem não foi enviada corretamente!";
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }

            // Processa a imagem e gera o caminho
            var caminhoImagem = await _caminhoImagem.GerarCaminhoArquivoAsync(imagem);
            if (caminhoImagem == null || caminhoImagem.Length == 0)
            {
                TempData["MensagemErro"] = "caminho da imagem não foi enviada corretamente!";
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }

            Console.WriteLine("Caminho da imagem: " + produto.ImagemUrl);

            produto.ImagemUrl = caminhoImagem;

            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                // Se o modelo não for válido, adiciona as mensagens de erro e redireciona
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["MensagemErro"] = error.ErrorMessage;
                }
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }

            try
            {
                // Tenta adicionar o produto no repositório
                await _produtoRepositorio.AddProdutoAsync(produto);
                TempData["MensagemSucesso"] = "Produto adicionado com sucesso!";
            }
            catch (Exception ex)
            {
                // Captura erros e exibe a mensagem
                TempData["MensagemErro"] = $"Erro ao adicionar o produto: {ex.Message}";
            }

            return RedirectToAction("GerenciamentoProdutos", "Produto");
        }


        // Método para atualizar um produto
        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(GerenciamentoProdutoViewModel viewModel, IFormFile? novaImagem)
        {
            // Verifica se o ModelState é válido
            if (ModelState.IsValid)
            {
                // Busca o produto existente no banco de dados
                var produtoExistente = await _produtoRepositorio.BuscarProdutoPorIdAsync(viewModel.Produto.ProdutoId);

                if (produtoExistente == null)
                {
                    // Caso o produto não seja encontrado, retorna erro
                    return NotFound();
                }

                // Atualiza as propriedades do produto com as novas informações da viewModel
                produtoExistente.NomeProduto = viewModel.Produto.NomeProduto;
                produtoExistente.Descricao = viewModel.Produto.Descricao;
                produtoExistente.Categoria = viewModel.Produto.Categoria;
                produtoExistente.Preco = viewModel.Produto.Preco;
                produtoExistente.QuantidadeEstoque = viewModel.Produto.QuantidadeEstoque;

                // Tenta atualizar o produto no banco de dados
                try
                {
                    await _produtoRepositorio.AtualizarProdutoAsync(produtoExistente, novaImagem);
                    TempData["MensagemSucesso"] = "Os dados do produto foram atualizados com sucesso!";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro ao atualizar os dados do produto: {ex.Message}";
                }

                // Após a atualização, redireciona para a página de gerenciamento de produtos
                return RedirectToAction("GerenciamentoProdutos");
            }

            // Se o ModelState não for válido, recarrega as categorias e produtos para mostrar erros
            var categorias = await _produtoRepositorio.BuscarCategoriasAsync();
            viewModel.Categorias = categorias;
            viewModel.Produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();

            // Retorna a mesma view com as mensagens de erro
            return View("GerenciamentoProdutos", viewModel);
        }

        // Método para remover um produto
        [HttpPost]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            try
            {
                if (id == 0)
                {
                    TempData["Erro"] = "O ID do produto não foi informado.";
                    return RedirectToAction("GerenciamentoProdutos", "Produto");
                }

                bool sucesso = await _produtoRepositorio.RemoverProdutoAsync(id);

                if (!sucesso)
                {
                    TempData["Erro"] = "Houve um erro ao tentar remover o produto. Tente novamente.";
                    return RedirectToAction("GerenciamentoProdutos", "Produto");
                }

                TempData["Sucesso"] = "Produto removido com sucesso.";
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao remover produto: {ex.Message}";
                return RedirectToAction("GerenciamentoProdutos", "Produto");
            }
        }
    }
}