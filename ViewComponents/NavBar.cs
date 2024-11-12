using DigitalStore.Helper;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Component
{
    public class NavBar : ViewComponent
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;

        public NavBar(IProdutoRepositorio produtoRepositorio, ISessao sessao)
        {
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
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
                ViewBag.UsuarioPerfil = sessao?.Perfil;
                ViewBag.UsuarioId = sessao?.UsuarioId;
            }

            var categorias = await _produtoRepositorio.BuscarCategoriasAsync();
            return View(categorias);
        }
    }
}