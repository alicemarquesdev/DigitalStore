using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface ISiteRepositorio
    {
        Task<SiteModel> BuscarDadosDoSiteAsync();

        Task AtualizarSite(SiteModel site, List<IFormFile?> banners);
    }
}