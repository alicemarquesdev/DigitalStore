using DigitalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DigitalStore.Filters
{
    public class PaginaCliente : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Recupera a sessão do usuário logado
            string sessaoDoUsuario = context.HttpContext.Session.GetString("sessaoDoUsuarioLogado");

            // Se não houver sessão do usuário, redireciona para a página de login
            if (string.IsNullOrEmpty(sessaoDoUsuario))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "Login" } });
            }
            else
            {
                // Deserializa o usuário da sessão
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoDoUsuario);

                // Se o usuário não for encontrado, redireciona para a página de login
                if (usuario == null)
                {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { { "controller", "Home" }, { "action", "Login" } });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}