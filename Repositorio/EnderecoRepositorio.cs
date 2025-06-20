using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DigitalStore.Repositorio
{
    // A classe EnderecoRepositorio gerencia as operações relacionadas aos endereços de usuários, como adicionar, buscar e remover endereços no banco de dados.
    // - BuscarEnderecoPorIdAsync(int enderecoId)
    // - BuscarTodosOsEnderecosDoUsuarioAsync(int id)
    // - AddEnderecoAsync(EnderecoModel endereco)
    // - RemoverEnderecoAsync(int id)

    public class EnderecoRepositorio : IEnderecoRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<EnderecoRepositorio> _logger;

        // Construtor que injeta o contexto do banco de dados
        public EnderecoRepositorio(BancoContext context, ILogger<EnderecoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método para buscar um endereço pelo seu ID
        public async Task<EnderecoModel?> BuscarEnderecoPorIdAsync(int enderecoId)
        {
            try
            {
                // Tenta buscar o endereço pelo ID
                var endereco = await _context.Enderecos.FirstOrDefaultAsync(x => x.EnderecoId == enderecoId);

                // Se o endereço não for encontrado, lança uma exceção 
                if (endereco == null)
                {
                    throw new Exception("Endereço não encontrado.");
                }

                return endereco;
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção genérica com a mensagem de erro
                _logger.LogError(ex, "Erro ao buscar o endereço com ID {EnderecoId}", enderecoId);
                throw new Exception("Erro ao buscar o endereço.");
            }
        }

        // Método para buscar todos os endereços de um usuário específico
        public async Task<List<EnderecoModel>> BuscarTodosOsEnderecosDoUsuarioAsync(int id)
        {
            try
            {
                // Tenta buscar todos os endereços relacionados ao usuário
                return await _context.Enderecos.Where(x => x.UsuarioId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção genérica com a mensagem de erro
                _logger.LogError(ex, "Erro ao buscar os endereços do usuário com ID {UsuarioId}", id);
                throw new Exception("Erro ao buscar os endereços.");
            }
        }

        // Método para adicionar um novo endereço
        public async Task AddEnderecoAsync(EnderecoModel endereco)
        {
            try
            {
                // Adiciona o novo endereço no banco de dados
                _context.Enderecos.Add(endereco);

                // Salva as mudanças no banco de dados
                var result = await _context.SaveChangesAsync();

                // Verifica se a operação foi bem-sucedida, ou seja, se houve alguma alteração no banco
                if (result == 0)
                {
                    throw new Exception("Falha ao adicionar o endereço.");
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção genérica com a mensagem de erro
                _logger.LogError(ex, "Erro ao adicionar o endereço.");
                throw new Exception( "Erro ao adicionar o endereço.");
            }
        }

        // Método para remover um endereço pelo seu ID
        public async Task<bool> RemoverEnderecoAsync(int id)
        {
            try
            {
                // Busca o endereço que será removido
                var endereco = await BuscarEnderecoPorIdAsync(id);

                // Se o endereço não for encontrado, retorna false
                if (endereco == null)
                {
                    return false;
                }

                // Remove o endereço do banco de dados
                _context.Enderecos.Remove(endereco);

                // Salva as mudanças no banco de dados
                var result = await _context.SaveChangesAsync();

                // Retorna true se o endereço foi removido com sucesso, caso contrário retorna false
                return result > 0;
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção genérica com a mensagem de erro
                _logger.LogError(ex, "Erro ao remover o endereço com ID {EnderecoId}", id);
                throw new Exception("Erro ao remover o endereço.");
            }
        }
    }
}
