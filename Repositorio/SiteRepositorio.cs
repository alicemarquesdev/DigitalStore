using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // Classe responsável por acessar e manipular os dados do site no banco de dados.
    // BuscarDadosDoSiteAsync() - Retorna os dados gerais do site, como nome, imagem e frase de destaque.
    // AtualizarSiteAsync(SiteModel site) - Atualiza os dados do site.
    public class SiteRepositorio : ISiteRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<SiteRepositorio> _logger;

        // Construtor que recebe o contexto do banco de dados.
        public SiteRepositorio(BancoContext context, ILogger<SiteRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Busca os dados do site no banco de dados.
        public async Task<SiteModel?> BuscarDadosDoSiteAsync()
        {
            try
            {
                var site = await _context.Site.FirstOrDefaultAsync();

                if (site == null)
                    throw new Exception("Nenhum dado do site encontrado no banco de dados.");

                return site;
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar buscar os dados do site.
                _logger.LogError(ex, "Erro ao buscar dados do site");
                throw new Exception("Erro ao buscar dados do site.");
            }
        }

        // Atualiza os dados do site no banco de dados.
        public async Task AtualizarSiteAsync(SiteModel site)
        {
            try
            {
                var siteDb = await BuscarDadosDoSiteAsync();

                // Atualiza apenas os campos necessários
                siteDb.NomeSite = site.NomeSite.Trim();
                siteDb.Banner = site.Banner;
                siteDb.Frase = site.Frase.Trim();

                // Salva as alterações e verifica se a atualização ocorreu
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    throw new Exception("Nenhuma alteração foi realizada ao atualizar os dados do site.");
            }
            catch (Exception ex)
            {
                // Lança uma exceção detalhando o erro ocorrido ao tentar atualizar os dados do site.
                _logger.LogError(ex, "Erro ao atualizar dados do site");
                throw new Exception("Erro ao atualizar os dados do site.");
            }
        }
    }
}
