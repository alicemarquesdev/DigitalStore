using DigitalStore.Data;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace DigitalStore.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
       private readonly BancoContext _bancoContext;

        public UsuarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }


        public UsuarioModel Criar(UsuarioModel usuario)
        {
            _bancoContext.Add(usuario);
            _bancoContext.SaveChanges();
            return usuario;
        }

        public UsuarioModel ListarPorId(int id)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);

        }

        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDb = ListarPorId(usuario.Id);

            if(usuarioDb == null) throw new Exception("Houve um erro na alteração");

            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Email = usuario.Email;
            usuarioDb.Endereço = usuario.Endereço;
            usuarioDb.Senha = usuario.Senha;

            _bancoContext.Update(usuarioDb);
            _bancoContext.SaveChanges();
            return usuarioDb;

        }

        public bool Apagar(int id)
        {
            UsuarioModel usuarioDb = ListarPorId(id);

            if(usuarioDb == null) return false;

            _bancoContext.Remove(usuarioDb);
            _bancoContext.SaveChanges();
            return true;
        }

        public UsuarioModel BuscarPorEmail(string email)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper());
        }
    }
}
