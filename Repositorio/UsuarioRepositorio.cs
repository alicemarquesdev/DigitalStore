using DigitalStore.Data;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _context;

        public UsuarioRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task AddUsuarioAsync(UsuarioModel usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<UsuarioModel> BuscarUsuarioPorIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);
        }

        public async Task AtualizarUsuarioAsync(UsuarioSemSenhaModel usuario)
        {
            UsuarioModel usuarioDb = await BuscarUsuarioPorIdAsync(usuario.Id);

            if (usuarioDb == null) throw new Exception("Houve um erro na alteração");

            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Email = usuario.Email;
            usuarioDb.NomeSite = usuario.NomeSite;

            _context.Usuarios.Update(usuarioDb);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverUsuarioAsync(int id)
        {
            UsuarioModel usuarioDb = await BuscarUsuarioPorIdAsync(id);

            if (usuarioDb == null) return false;

            _context.Usuarios.Remove(usuarioDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UsuarioModel> BuscarUsuarioExistenteAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper());
        }
    }
}