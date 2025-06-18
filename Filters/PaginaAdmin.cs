using DigitalStore.Enums;
using DigitalStore.Models;
using DigitalStore.Helper.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DigitalStore.Filters
{
    // Filtro de ação personalizado utilizado para verificar se o usuário está logado e se tem o perfil de administrador.
    // Esse filtro é aplicado nas ações dos controladores onde é necessário garantir que o usuário tenha permissão para acessar.
    // Se o usuário não estiver logado ou não for administrador, ele será redirecionado para a página de inicial.
    // Caso contrário, a execução da ação do controlador continuará normalmente.
    public class PaginaAdmin : ActionFilterAttribute
    {
        // Este método é executado antes de a ação do controlador ser executada
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Recupera o valor da sessão do usuário logado. A sessão contém informações sobre o estado de login.
            string sessaoDoUsuario = context.HttpContext.Session.GetString("SessaoDoUsuarioLogado");


            // Se o usuário não estiver logado ou não for um administrador
            if (string.IsNullOrEmpty(sessaoDoUsuario))
            {
                // Redireciona o usuário para a página de login ou para a página inicial
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
            }
            else
            {
                // Caso a sessão exista, ela é desserializada para um objeto do tipo UsuarioModel.
                // Isso transforma a string armazenada na sessão de volta para um objeto.
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoDoUsuario);

                // Se o objeto usuário for nulo após a desserialização (isso pode acontecer se a sessão estiver corrompida ou inválida),
                // o usuário também é redirecionado para a página de login.
                if (usuario == null || usuario.Perfil != PerfilEnum.Admin)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
                }
            }

            // Chama a base para garantir que a lógica do filtro seja executada corretamente
            base.OnActionExecuting(context);
        }
    }
}
