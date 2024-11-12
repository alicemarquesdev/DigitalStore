using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly BancoContext _context;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AdminController(IProdutoRepositorio produtoRepositorio, IUsuarioRepositorio usuarioRepositorio, BancoContext context)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _context = context;
        }

        public IActionResult AddProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduto(IFormFile file, ProdutoModel produto)
        {
            if (file != null && file.Length > 0)
            {
                // Define o caminho onde a imagem será salva
                var pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image");
                Directory.CreateDirectory(pastaDestino); // Garante que a pasta existe
                var caminhoArquivo = Path.Combine(pastaDestino, file.FileName);

                // Salva a imagem no diretório wwwroot/images
                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Salva o caminho relativo no banco de dados
                produto.ImagemUrl = "/image/" + file.FileName;
                await _produtoRepositorio.AddProdutoAsync(produto);
            }

            return RedirectToAction("Index"); // Redireciona para uma ação, como a página inicial ou lista de produtos
        }

        public async Task<IActionResult> AtualizarProduto(int id)
        {
            var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);
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

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
            return View(produtos);
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