using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que define os métodos para manipulação dos dados do site no banco de dados.
    // O site tem dados padrões definidos ao criar o banco de dados, como nome, banner e frase, não é necessário criar um novo registro.
    public interface ISiteRepositorio
    {
        // Retorna os dados gerais do site, como nome, imagem e frase de destaque.
        Task<SiteModel?> BuscarDadosDoSiteAsync();

        // Atualiza os dados do site
        Task AtualizarSiteAsync(SiteModel site);
    }
}
