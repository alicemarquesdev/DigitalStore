using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class SiteRepositorio : ISiteRepositorio
    {
        private readonly BancoContext _context;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public SiteRepositorio(BancoContext context, IProdutoRepositorio produtoRepositorio)
        {
            _context = context;
            _produtoRepositorio = produtoRepositorio;
        }

        // Método para buscar os dados do site
        public async Task<SiteModel> BuscarDadosDoSiteAsync()
        {
            return await _context.Site.FirstOrDefaultAsync();
        }

        // Método para atualizar os dados do site
        public async Task AtualizarSite(SiteModel site, IFormFile? imagem1, IFormFile? imagem2, IFormFile? imagem3, IFormFile? imagem4)
        {
            var siteDb = await BuscarDadosDoSiteAsync();

            if (siteDb == null)
            {
                throw new Exception("Site não encontrado no banco de dados.");
            }

            // Atualiza os campos do site
            siteDb.NomeSite = site.NomeSite;
            siteDb.Frase = site.Frase;

            // Atualiza as imagens se fornecidas
            if (imagem1 != null)
            {
                siteDb.Banner1Url = await _produtoRepositorio.GerarCaminhoArquivoAsync(imagem1);
            }
            if (imagem2 != null)
            {
                siteDb.Banner2Url = await _produtoRepositorio.GerarCaminhoArquivoAsync(imagem2);
            }
            if (imagem3 != null)
            {
                siteDb.Banner3Url = await _produtoRepositorio.GerarCaminhoArquivoAsync(imagem3);
            }
            if (imagem4 != null)
            {
                siteDb.Banner4Url = await _produtoRepositorio.GerarCaminhoArquivoAsync(imagem4);
            }

            // Atualiza no banco de dados
            _context.Site.Update(siteDb);
            await _context.SaveChangesAsync();
        }
    }
}