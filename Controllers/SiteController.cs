using DigitalStore.Filters;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    // Controlador responsável pela personalização do site
    // - Personalizacao(): Exibe a tela de personalização do site.
    // - AtualizarSiteAsync(SiteModel site, IFormFile? banner): Atualiza os dados e banners do site.
    [PaginaAdmin]
    public class SiteController : Controller
    {
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly ICaminhoImagem _caminhoImagem;
        private readonly ILogger<SiteController> _logger; // Logger para logar erros e eventos.

        // Construtor para injeção de dependências
        public SiteController(ISiteRepositorio siteRepositorio,
                              ICaminhoImagem caminhoImagem,
                              ILogger<SiteController> logger)
        {
            _siteRepositorio = siteRepositorio ?? throw new ArgumentNullException(nameof(siteRepositorio));
            _caminhoImagem = caminhoImagem ?? throw new ArgumentNullException(nameof(caminhoImagem));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método para exibir os dados de personalização do site
        public async Task<IActionResult> Personalizacao()
        {
            try
            {
                // Busca os dados do site, se o site retornar null, repositorio irá lançar uma exceção
                var site = await _siteRepositorio.BuscarDadosDoSiteAsync();
            
                return View(site);
            }
            catch (Exception ex)
            {
                // Loga qualquer erro durante a execução e exibe mensagem de erro amigável
                _logger.LogError(ex, "Erro ao buscar dados do site.");
                TempData["Alerta"] = "Ocorreu um erro ao carregar os dados do site. Tente novamente mais tarde.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Método para atualizar as configurações do site e banners
        [HttpPost]
        public async Task<IActionResult> AtualizarSiteAsync(SiteModel site, IFormFile? banner)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var siteDb = await _siteRepositorio.BuscarDadosDoSiteAsync();

                    // Atualiza a imagem se for fornecida e apaga a antiga imagem do servidor
                    if (banner != null)
                    {
                        var caminhoImagem = await _caminhoImagem.GerarCaminhoArquivoAsync(banner);
                        if (caminhoImagem == null || caminhoImagem.Length == 0)
                        {
                            throw new ArgumentException("Erro ao gerar arquivo de imagem.");
                        }

                        // Remover a imagem antiga, se necessário
                       await _caminhoImagem.RemoverImagemAntiga(siteDb.Banner);

                        // Atualiza a imagem do produto com o novo caminho
                        siteDb.Banner = caminhoImagem;
                    }

                    // Atualiza os dados do site
                    siteDb.NomeSite = site.NomeSite;
                    siteDb.Frase = site.Frase;
                    siteDb.Banner = siteDb.Banner;

                    await _siteRepositorio.AtualizarSiteAsync(siteDb);
                    TempData["Alerta"] = "Dados do site atualizados com sucesso!";
                    return RedirectToAction("Personalizacao");
                }

                TempData["Alerta"] = "Verifique os dados inseridos, houve um erro ao tentar atualizar os dados.";

                return RedirectToAction("Personalizacao");
            }
                 catch (Exception ex)
            {
                // Loga qualquer outra exceção e exibe uma mensagem de erro genérica
                _logger.LogError(ex, "Erro inesperado ao atualizar os dados do site.");
                TempData["Alerta"] = "Ocorreu um erro ao atualizar os dados do site. Tente novamente mais tarde.";
                return RedirectToAction("Personalizacao");
            }
        }
    }
}
