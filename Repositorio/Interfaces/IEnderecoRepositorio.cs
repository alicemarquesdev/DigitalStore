using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Define os métodos para manipular os endereços no banco de dados
    public interface IEnderecoRepositorio
    {
        // Busca um endereço pelo ID
        Task<EnderecoModel?> BuscarEnderecoPorIdAsync(int enderecoId);

        // Busca todos os endereços de um usuário
        Task<List<EnderecoModel>> BuscarTodosOsEnderecosDoUsuarioAsync(int id);

        // Adiciona um novo endereço
        Task AddEnderecoAsync(EnderecoModel endereco);

        // Remove um endereço pelo ID
        Task<bool> RemoverEnderecoAsync(int id);
    }
}
