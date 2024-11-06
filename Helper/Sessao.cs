using DigitalStore.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace DigitalStore.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public UsuarioModel BuscarSessaoDoUsuario()
        {
            string sessaoDoUsuario = _httpContext.HttpContext.Session.GetString("SessaoDoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoDoUsuario)) return null;

            return JsonConvert.DeserializeObject<UsuarioModel>("SessaoDoUsuarioLogado");
        }

        public void CriarSessaoDoUsuario(UsuarioModel usuario)
        {
            string valor = JsonConvert.SerializeObject(usuario);
            _httpContext.HttpContext.Session.SetString("SessaoDoUsuarioLogado", valor);

        }

        public void RemoverSessaoDoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("SessaoDoUsuarioLogado");
        }
    }
}
