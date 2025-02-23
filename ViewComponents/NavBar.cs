using DigitalStore.Helper;
using DigitalStore.Models.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Component
{
    public class NavBar : ViewComponent
    {
        private readonly ISessao _sessao;
        private readonly ISiteRepositorio _site;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public NavBar(ISessao sessao, ISiteRepositorio site, ICarrinhoRepositorio carrinhorRepositorio, IFavoritosRepositorio favoritosRepositorio, IProdutoRepositorio produtoRepositorio)
        {
            _sessao = sessao;
            _site = site;
            _carrinhoRepositorio = carrinhorRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

            var site = await _site.BuscarDadosDoSiteAsync();

            var viewModel = new NavBarViewModel
            {
                UsuarioLogado = usuarioSessao != null,
                UsuarioLogadoId = usuarioSessao?.UsuarioId ?? 0,
                PerfilUsuarioCliente = usuarioSessao?.Perfil == Enum.PerfilEnum.Cliente,
                NomeSite = site.NomeSite,
                Categorias = await _produtoRepositorio.BuscarCategoriasAsync(),
                CarrinhoQuantidadeDeProdutos = usuarioSessao != null
                ? (await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId)).Count
                : 0,
                FavoritosQuantidadeDeProdutos = usuarioSessao != null
                ? (await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId)).Count
                : 0
            };

            return View(viewModel);
        }
    }
}