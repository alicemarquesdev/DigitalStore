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
        private readonly ISiteRepositorio _siteRepositorio;

        public HomeController(
            IProdutoRepositorio produtoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ISessao sessao,
            ICarrinhoRepositorio carrinhoRepositorio,
            IFavoritosRepositorio favoritosRepositorio,
            ISiteRepositorio siteRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
            _siteRepositorio = siteRepositorio;
        }

        // Método para carregar a página inicial com todos os produtos
        public async Task<IActionResult> Index()
        {
            try
            {
                var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
                var sessao = _sessao.BuscarSessaoDoUsuario();
                ViewBag.Sessao = sessao;
                ViewBag.SiteDados = await _siteRepositorio.BuscarDadosDoSiteAsync();
                ViewBag.Categorias = await _produtoRepositorio.BuscarCategoriasAsync();
                ViewBag.UltimasNovidades = await _produtoRepositorio.BuscarUltimosProdutosAdicionados();

                if (sessao != null)
                {
                    ViewBag.UsuarioId = sessao.UsuarioId;
                    ViewBag.PerfilUsuario = sessao.Perfil == Enum.PerfilEnum.Cliente;
                    var favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(sessao.UsuarioId);
                    ViewBag.FavoritosDoUsuario = favoritos.Select(f => f.ProdutoId).ToList();
                    var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(sessao.UsuarioId);
                    ViewBag.CarrinhoDoUsuario = carrinho.Select(f => f.ProdutoId).ToList();
                }

                return View(produtos);
            }
            catch (Exception ex)
            {
                // Adiciona um erro genérico
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar os produtos.";
                return View();
            }
        }

        // Método para listar todos os produtos
        public async Task<IActionResult> TodosOsProdutos()
        {
            try
            {
                var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
                var sessao = _sessao.BuscarSessaoDoUsuario();
                ViewBag.Sessao = sessao;

                if (sessao != null)
                {
                    ViewBag.UsuarioId = sessao.UsuarioId;
                    ViewBag.Perfil = sessao.Perfil == Enum.PerfilEnum.Cliente;
                    var favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(sessao.UsuarioId);
                    ViewBag.FavoritosDoUsuario = favoritos.Select(f => f.ProdutoId).ToList();
                    var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(sessao.UsuarioId);
                    ViewBag.CarrinhoDoUsuario = carrinho.Select(f => f.ProdutoId).ToList();
                }

                return View(produtos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao listar todos os produtos.";
                return View();
            }
        }

        // Método para exibir produtos por categoria
        public async Task<IActionResult> Categoria(ProdutoModel categoriaProdutos)
        {
            try
            {
                var produtosPorCategoria = await _produtoRepositorio.BuscarProdutosPorCategoriaAsync(categoriaProdutos);
                ViewBag.CategoriaSelecionada = categoriaProdutos.Categoria;

                var sessaoDoUsuario = _sessao.BuscarSessaoDoUsuario();
                ViewBag.Sessao = sessaoDoUsuario;

                if (sessaoDoUsuario != null)
                {
                    ViewBag.UsuarioId = sessaoDoUsuario.UsuarioId;
                    ViewBag.PerfilUsuario = sessaoDoUsuario.Perfil == Enum.PerfilEnum.Cliente;
                    var favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(sessaoDoUsuario.UsuarioId);
                    ViewBag.FavoritosDoUsuario = favoritos.Select(f => f.ProdutoId).ToList();
                    var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(sessaoDoUsuario.UsuarioId);
                    ViewBag.CarrinhoDoUsuario = carrinho.Select(f => f.ProdutoId).ToList();
                }

                return View(produtosPorCategoria);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar os produtos da categoria.";
                return View();
            }
        }

        // Método para exibir últimas novidades
        public async Task<IActionResult> UltimasNovidades(ProdutoModel categoriaProdutos)
        {
            try
            {
                var produtos = await _produtoRepositorio.BuscarUltimosProdutosAdicionados();
                var sessao = _sessao.BuscarSessaoDoUsuario();
                ViewBag.Sessao = sessao;

                if (sessao != null)
                {
                    var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);
                    ViewBag.UsuarioId = usuario.UsuarioId;
                    ViewBag.Perfil = usuario.Perfil == Enum.PerfilEnum.Cliente;
                    var favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuario.UsuarioId);
                    ViewBag.FavoritosDoUsuario = favoritos.Select(f => f.ProdutoId).ToList();
                    var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuario.UsuarioId);
                    ViewBag.CarrinhoDoUsuario = carrinho.Select(f => f.ProdutoId).ToList();
                }

                return View(produtos);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar as últimas novidades.";
                return View();
            }
        }

        // Método para exibir detalhes de um produto específico
        public async Task<IActionResult> Produto(int id)
        {
            try
            {
                var produtoDb = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

                if (produtoDb == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado";
                }

                var sessao = _sessao.BuscarSessaoDoUsuario();
                ViewBag.Sessao = sessao;

                if (sessao != null)
                {
                    var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);
                    ViewBag.UsuarioId = usuario.UsuarioId;
                    ViewBag.Perfil = usuario.Perfil == Enum.PerfilEnum.Cliente;
                }

                ViewBag.ProdutosRelacionados = await _produtoRepositorio.BuscarUltimosProdutosAdicionados();
                return View(produtoDb);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar o produto.";
                return View();
            }
        }

        // Método para exibir página de suporte
        public IActionResult Suporte()
        {
            return View();
        }
    }
}