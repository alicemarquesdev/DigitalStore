using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface ISiteRepositorio
    {
        Task<SiteModel> BuscarDadosDoSiteAsync();

        Task AtualizarSite(SiteModel site, IFormFile imagem1, IFormFile imagem2, IFormFile imagem3, IFormFile? imagem4);
    }
}