using DigitalStore.Models;
using Newtonsoft.Json;

namespace DigitalStore.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        // Método para buscar a sessão do usuário
        public UsuarioModel BuscarSessaoDoUsuario()
        {
            string sessaoDoUsuario = _httpContext.HttpContext.Session.GetString("SessaoDoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoDoUsuario))
                return null;

            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoDoUsuario);
        }

        // Método para criar uma nova sessão para o usuário
        public void CriarSessaoDoUsuario(UsuarioModel usuario)
        {
            string valor = JsonConvert.SerializeObject(usuario);
            _httpContext.HttpContext.Session.SetString("SessaoDoUsuarioLogado", valor);
        }

        // Método para remover a sessão do usuário
        public void RemoverSessaoDoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("SessaoDoUsuarioLogado");
        }
    }
}