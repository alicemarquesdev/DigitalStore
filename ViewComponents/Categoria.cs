using DigitalStore.Helper;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Component
{
    public class Categoria : ViewComponent
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;

        public Categoria(IProdutoRepositorio produtoRepositorio, ISessao sessao)
        {
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao == null)
            {
                // Se a sessão for null, atribua valores padrão ou faça o tratamento necessário.
                ViewBag.Sessao = null;
                ViewBag.UsuarioPerfil = null; // ou defina um valor padrão, se houver
            }
            else
            {
                ViewBag.Sessao = sessao;
                ViewBag.UsuarioPerfil = sessao?.Perfil;
            }

            var categorias = await _produtoRepositorio.BuscarCategoriasAsync();
            return View(categorias);
        }
    }
}