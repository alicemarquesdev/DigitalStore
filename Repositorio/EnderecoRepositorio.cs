using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class EnderecoRepositorio : IEnderecoRepositorio
    {
        private readonly BancoContext _context;

        public EnderecoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task<EnderecoModel> BuscarEnderecoPorIdAsync(int enderecoId)
        {
            try
            {
                var endereco = await _context.Endereco.FirstOrDefaultAsync(x => x.EnderecoId == enderecoId);
                if (endereco == null) throw new Exception("Endereço não encontrado.");

                return endereco;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EnderecoModel>> BuscarTodosOsEnderecosDoUsuarioAsync(int id)
        {
            return await _context.Endereco.Where(x => x.UsuarioId == id).ToListAsync();
        }


        public async Task AddEnderecoAsync(EnderecoModel endereco)
        {
            await _context.Endereco.AddAsync(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarEnderecoAsync(EnderecoModel endereco)
        {
            var enderecoDb = await BuscarEnderecoPorIdAsync(endereco.EnderecoId);

            if (enderecoDb == null) throw new Exception("Endereço não encontrado.");

            enderecoDb.Rua = endereco.Rua;
            enderecoDb.Bairro = endereco.Bairro;
            enderecoDb.Cidade = endereco.Cidade;
            enderecoDb.Estado = endereco.Estado;
            enderecoDb.CEP = endereco.CEP;

            _context.Endereco.Update(enderecoDb);
            await _context.SaveChangesAsync();
        }

        

        public async Task<bool> RemoverEnderecoAsync(int id)
        {
            var endereco = await BuscarEnderecoPorIdAsync(id);

            if (endereco == null) return false;

            _context.Remove(endereco);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}