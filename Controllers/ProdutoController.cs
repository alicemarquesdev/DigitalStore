using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduto(ProdutoModel produto, IFormFile imagem)
        {
            // Verificando se o ModelState não é válido
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["MensagemErro"] = error.ErrorMessage;  // Mensagens para falhas na validação do modelo
                }
                return RedirectToAction("AddProduto", "PerfilAdmin");
            }

            // Verificando se a imagem foi carregada corretamente
            if (imagem == null || imagem.Length == 0)
            {
                TempData["MensagemErro"] = "A imagem não foi enviada corretamente!";
                return RedirectToAction("AddProduto", "PerfilAdmin");
            }

            try
            {
                await _produtoRepositorio.AddProdutoAsync(produto, imagem);
                TempData["MensagemSucesso"] = "Produto adicionado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao adicionar o produto: {ex.Message}";
            }

            return RedirectToAction("AddProduto", "PerfilAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(ProdutoModel produto, IFormFile? novaImagem)
        {
            if (produto.ImagemUrl == null)
            {
                TempData["MensagemErro"] = "ImagemUrl está nulo no modelo!";
            }

            if (ModelState.IsValid)
            {
                await _produtoRepositorio.AtualizarProdutoAsync(produto, novaImagem);
                TempData["MensagemSucesso"] = "Os dados do produto foram atualizados com sucesso!";
                return RedirectToAction("AtualizarProduto", "PerfilAdmin");
            }
            TempData["MensagemErro"] = "Não foi possível atualizar os dados do produto. Por favor, tente novamente!";
            return RedirectToAction("AtualizarProduto", "PerfilAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            bool sucesso = await _produtoRepositorio.RemoverProdutoAsync(id);

            if (!sucesso)
            {
                // Retorna uma mensagem de erro adequada
                TempData["Erro"] = "Houve um erro ao tentar remover o produto. Tente novamente.";
                return RedirectToAction("GerenciamentoProdutos", "PerfilAdmin");
            }

            // Mensagem de sucesso
            TempData["Sucesso"] = "Produto removido com sucesso.";
            return RedirectToAction("GerenciamentoProdutos", "PerfilAdmin");
        }
    }
}