using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class SiteRepositorio : ISiteRepositorio
    {
        private readonly BancoContext _context;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ICaminhoImagem _caminhoImagem;

        public SiteRepositorio(BancoContext context, IProdutoRepositorio produtoRepositorio, ICaminhoImagem caminhoImagem)
        {
            _context = context;
            _produtoRepositorio = produtoRepositorio;
            _caminhoImagem = caminhoImagem;
        }

        // Método para buscar os dados do site
        public async Task<SiteModel> BuscarDadosDoSiteAsync()
        {
            return await _context.Site.FirstOrDefaultAsync();
        }

        // Método para atualizar os dados do site
        public async Task AtualizarSite(SiteModel site, List<IFormFile?> banners)
        {
            var siteDb = await BuscarDadosDoSiteAsync();

            if (siteDb == null)
            {
                throw new Exception("Site não encontrado no banco de dados.");
            }

            List<string> caminhosBanners = new List<string>();

            if (banners != null)
            {
                // Atualiza as imagens se fornecidas
                foreach (var banner in banners)
                {
                    if (banner != null)
                    {
                        var caminhoBanner = await _caminhoImagem.GerarCaminhoArquivoAsync(banner);
                        caminhosBanners.Add(caminhoBanner);
                    }
                }
            }

            // Atualiza os campos do site
            siteDb.NomeSite = site.NomeSite;
            siteDb.Frase = site.Frase;

            // Atualiza no banco de dados
            _context.Site.Update(siteDb);
            await _context.SaveChangesAsync();
        }
    }
}