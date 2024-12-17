using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;

        public HomeController(IProdutoRepositorio produtoRepositorio,
                              IUsuarioRepositorio usuarioRepositorio,
                              ISessao sessao,
                              ICarrinhoRepositorio carrinhoRepositorio,
                              IFavoritosRepositorio favoritosRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
        }

        public async Task<IActionResult> Categoria(ProdutoModel categoriaProdutos)
        {
            List<ProdutoModel> produtosPorCategoria = await _produtoRepositorio.BuscarProdutosPorCategoriaAsync(categoriaProdutos);
            ViewBag.CategoriaSelecionada = categoriaProdutos.Categoria;

            // Retorna um UsuarioModel
            var sessaoDoUsuario = _sessao.BuscarSessaoDoUsuario();

            if (sessaoDoUsuario != null)
            {
                ViewBag.UsuarioId = sessaoDoUsuario.UsuarioId; 
                ViewBag.PerfilUsuario = sessaoDoUsuario.Perfil == Enum.PerfilEnum.Cliente;
                var favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(sessaoDoUsuario.UsuarioId);
                ViewBag.FavoritosDoUsuario = favoritos.Select(f => f.ProdutoId).ToList();
            }

            ViewBag.Sessao = sessaoDoUsuario;

            return View(produtosPorCategoria);
        }

        public async Task<IActionResult> Index()
        {
            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();

            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao != null)
            {
              
                ViewBag.UsuarioId = sessao.UsuarioId;
                ViewBag.PerfilUsuario = sessao.Perfil == Enum.PerfilEnum.Cliente;
               

            }

            ViewBag.Sessao = sessao;
            ViewBag.Categorias = await _produtoRepositorio.BuscarCategoriasAsync();
            ViewBag.UltimasNovidades = await _produtoRepositorio.BuscarUltimosProdutosAdicionados();

            return View(produtos);
        }

        public async Task<IActionResult> UltimasNovidades(ProdutoModel categoriaProdutos)
        {
            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarUltimosProdutosAdicionados();

            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao != null)
            {
                UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);

                ViewBag.UsuarioId = usuario.UsuarioId;
                ViewBag.Perfil = usuario.Perfil == Enum.PerfilEnum.Cliente;
            }

            ViewBag.Sessao = sessao;

            return View(produtos);
        }

        public async Task<IActionResult> TodosOsProdutos()
        {
            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();

            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao != null)
            {
                UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);

                ViewBag.UsuarioId = usuario.UsuarioId;
                ViewBag.Perfil = usuario.Perfil == Enum.PerfilEnum.Cliente;
            }

            ViewBag.Sessao = sessao;

            return View(produtos);
        }

        public async Task<IActionResult> Produto(int id)
        {
            ProdutoModel produtoDb = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

            if (produtoDb == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado";
            }

            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao != null)
            {
                UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);

                ViewBag.UsuarioId = usuario.UsuarioId;
                ViewBag.Perfil = usuario.Perfil == Enum.PerfilEnum.Cliente;
            }

            ViewBag.Sessao = sessao;

            ViewBag.ProdutosRelacionados = await _produtoRepositorio.BuscarUltimosProdutosAdicionados();

            return View(produtoDb);
        }

        public IActionResult Suporte()
        {
            return View();
        }
    }
}