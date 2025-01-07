using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class SiteController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISiteRepositorio _siteRepositorio;

        public SiteController(IProdutoRepositorio produtoRepositorio,
                                      IUsuarioRepositorio usuarioRepositorio,
                                      ISessao sessao,
                                      ISiteRepositorio siteRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _siteRepositorio = siteRepositorio;
        }

        public async Task<IActionResult> Personalizacao()
        {
            var site = await _siteRepositorio.BuscarDadosDoSiteAsync();
            return View(site);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarSite(SiteModel site, IFormFile? imagem1, IFormFile? imagem2, IFormFile? imagem3, IFormFile? imagem4)
        {
            if (site == null)
            {
                TempData["MensagemErro"] = "Dados inválidos.";
                return RedirectToAction("Personalizacao");
            }

            try
            {
                await _siteRepositorio.AtualizarSite(site, imagem1, imagem2, imagem3, imagem4);
                TempData["MensagemSucesso"] = "Dados atualizados com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar os dados: {ex.Message}";
            }

            return RedirectToAction("Personalizacao");
        }
    }
}