using DigitalStore.Helper;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Component
{
    public class NavBar : ViewComponent
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly ISiteRepositorio _site;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;

        public NavBar(IProdutoRepositorio produtoRepositorio, ISessao sessao, ISiteRepositorio site, ICarrinhoRepositorio carrinhorRepositorio, IFavoritosRepositorio favoritosRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
            _site = site;
            _carrinhoRepositorio = carrinhorRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao == null)
            {
                ViewBag.Sessao = null;
                ViewBag.UsuarioPerfil = null;
                ViewBag.NomeSite = null;
                ViewBag.UsuarioId = null;
            }
            else
            {
                ViewBag.Sessao = sessao;
                ViewBag.UsuarioPerfil = sessao.Perfil;
                ViewBag.UsuarioId = sessao.UsuarioId;
                int carrinhoProdutos = await _carrinhoRepositorio.TotalProdutosNoCarrinhoDoUsuarioAsync(sessao.UsuarioId);
                ViewBag.CarrinhoQuantidadeDeProdutos = carrinhoProdutos;
                int favoritosProdutos = await _favoritosRepositorio.TotalProdutosNosFavoritos(sessao.UsuarioId);
                ViewBag.FavoritosQuantidadeDeProdutos = favoritosProdutos;
            }

            var site = await _site.BuscarDadosDoSiteAsync();
            ViewBag.SiteDados = site.NomeSite;
            var categorias = await _produtoRepositorio.BuscarCategoriasAsync();

            return View(categorias);
        }
    }
}