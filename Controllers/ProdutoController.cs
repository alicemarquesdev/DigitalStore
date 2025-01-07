using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        // Construtor para injeção de dependência
        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }
        public IActionResult GetPartialUpdateView(int id)
        {
            var produto = _produtoRepositorio.BuscarProdutoPorIdAsync(id); // Substitua pelo seu serviço
            return PartialView("_AtualizarProdutoPartial", produto);
        }

        public IActionResult AddProduto()
        {
            return View();
        }

        public async Task<IActionResult> AtualizarProduto(int id)
        {
            ProdutoModel produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);
            ViewBag.Categorias = await _produtoRepositorio.BuscarCategoriasAsync();
            return View(produto);
        }

        public async Task<IActionResult> GerenciamentoProdutos()
        {
            var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
            return View(produtos);
        }

        // Método para adicionar um produto
        [HttpPost]
        public async Task<IActionResult> AddProduto(ProdutoModel produto, IFormFile imagem)
        {
            // Verificando se o ModelState não é válido
            if (!ModelState.IsValid)
            {
                // Adiciona as mensagens de erro do ModelState
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["MensagemErro"] = error.ErrorMessage;
                }
                return RedirectToAction("AddProduto", "Produto");
            }

            // Verificando se a imagem foi carregada corretamente
            if (imagem == null || imagem.Length == 0)
            {
                TempData["MensagemErro"] = "A imagem não foi enviada corretamente!";
                return RedirectToAction("AddProduto", "Produto");
            }

            try
            {
                // Tenta adicionar o produto no repositório
                await _produtoRepositorio.AddProdutoAsync(produto, imagem);
                TempData["MensagemSucesso"] = "Produto adicionado com sucesso!";
            }
            catch (Exception ex)
            {
                // Captura erros e exibe a mensagem
                TempData["MensagemErro"] = $"Erro ao adicionar o produto: {ex.Message}";
            }

            return RedirectToAction("AddProduto", "Produto");
        }

        // Método para atualizar um produto
        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(ProdutoModel produto, IFormFile? novaImagem)
        {
            // Verifica se a ImagemUrl do produto está nula
            if (produto.ImagemUrl == null)
            {
                TempData["MensagemErro"] = "ImagemUrl está nulo no modelo!";
            }

            // Verifica se o ModelState é válido antes de atualizar
            if (ModelState.IsValid)
            {
                try
                {
                    await _produtoRepositorio.AtualizarProdutoAsync(produto, novaImagem);
                    TempData["MensagemSucesso"] = "Os dados do produto foram atualizados com sucesso!";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro ao atualizar os dados do produto: {ex.Message}";
                }
                return RedirectToAction("AtualizarProduto", "Produto");
            }

            TempData["MensagemErro"] = "Não foi possível atualizar os dados do produto. Por favor, tente novamente!";
            return RedirectToAction("AtualizarProduto", "Produto");
        }

        // Método para remover um produto
        [HttpPost]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            try
            {
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