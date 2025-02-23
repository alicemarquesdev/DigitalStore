using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IEnderecoRepositorio
    {
        Task<EnderecoModel> BuscarEnderecoPorIdAsync(int enderecoId);

        Task<List<EnderecoModel>> BuscarTodosOsEnderecosDoUsuarioAsync(int id);

        Task AddEnderecoAsync(EnderecoModel endereco);

        Task<bool> RemoverEnderecoAsync(int id);

        Task<string> ObterEstadoDoEnderecoAsync(string endereco);

        public decimal CalcularFretePorRegiao(string estado);
    }
}