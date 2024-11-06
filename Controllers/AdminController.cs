using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public AdminController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
            return View(produtos);
        }

        public IActionResult AddProduto()
        {
            return View();
        }

        public async Task<IActionResult> AtualizarProduto(int id)
        {
            var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduto(ProdutoModel produto)
        {
            if (ModelState.IsValid)
            {
                await _produtoRepositorio.AddProdutoAsync(produto);
                return RedirectToAction("Index", "Admin");
            }
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(ProdutoModel produto)
        {
            if (ModelState.IsValid)
            {
                await _produtoRepositorio.AtualizarProdutoAsync(produto);
                return RedirectToAction("Index", "Admin");
            }
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            bool sucesso = await _produtoRepositorio.RemoverProdutoAsync(id);

            if (!sucesso)
            {
                // Retorna uma mensagem de erro adequada
                TempData["Erro"] = "Houve um erro ao tentar remover o produto. Tente novamente.";
                return RedirectToAction("Index");
            }

            // Mensagem de sucesso
            TempData["Sucesso"] = "Produto removido com sucesso.";
            return RedirectToAction("Index");  // Redireciona para a página inicial após a remoção
        }
    }
}