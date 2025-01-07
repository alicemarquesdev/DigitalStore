using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
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

        // Método para buscar usuário pelo email (não sensível a maiúsculas/minúsculas)
        public async Task<UsuarioModel> BuscarUsuarioExistenteAsync(string email)
        {
            return await _context.Usuarios
                                  .FirstOrDefaultAsync(x => x.Email == email);
        }

        // Método para buscar usuário por ID
        public async Task<UsuarioModel> BuscarUsuarioPorIdAsync(int id)
        {
            return await _context.Usuarios
                                 .FirstOrDefaultAsync(x => x.UsuarioId == id);
        }

        // Método para adicionar um novo usuário
        public async Task AddUsuarioAsync(UsuarioModel usuario)
        {
            usuario.SetSenhaHash();  // Definir a senha criptografada
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        // Método para atualizar os dados do usuário
        public async Task AtualizarUsuarioAsync(UsuarioModel usuario)
        {
            var usuarioDb = await BuscarUsuarioPorIdAsync(usuario.UsuarioId);

            if (usuarioDb == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado para atualização");
            }

            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Email = usuario.Email;

            _context.Usuarios.Update(usuarioDb);
            await _context.SaveChangesAsync();
        }

        // Método para alterar a senha de um usuário
        public async Task AlterarSenhaAsync(AlterarSenhaModel alterarSenhaModel)
        {
            var usuarioDB = await BuscarUsuarioPorIdAsync(alterarSenhaModel.Id);

            if (usuarioDB == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado para atualização de senha");
            }

            if (!usuarioDB.SenhaValida(alterarSenhaModel.SenhaAtual))
            {
                throw new ArgumentException("Senha atual não confere!");
            }

            if (usuarioDB.SenhaValida(alterarSenhaModel.NovaSenha))
            {
                throw new ArgumentException("Nova senha deve ser diferente da senha atual!");
            }

            usuarioDB.SetNovaSenha(alterarSenhaModel.NovaSenha);

            _context.Usuarios.Update(usuarioDB);
            await _context.SaveChangesAsync();
        }

        // Método para remover um usuário
        public async Task<bool> RemoverUsuarioAsync(int id)
        {
            var usuarioDb = await BuscarUsuarioPorIdAsync(id);

            if (usuarioDb == null) return false;

            _context.Usuarios.Remove(usuarioDb);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}