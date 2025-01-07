using DigitalStore.Enum;
using DigitalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DigitalStore.Filters
{
    public class PaginaUsuarioLogado : ActionFilterAttribute
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

                // Se o usuário não for encontrado ou não for administrador, redireciona para a página de login ou restrita
                if (usuario == null || usuario.Perfil != PerfilEnum.Admin)
                {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { { "controller", "Restrito" }, { "action", "Index" } });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}