using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Component
{
    public class Categoria : ViewComponent
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public Categoria(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categorias = await _produtoRepositorio.BuscarCategoriasAsync();
            return View(categorias);
        }
    }
}