using DigitalStore.Enums;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DigitalStore.Filters
{
    // Filtro de ação personalizado utilizado para verificar se o usuário está logado e se tem o perfil de cliente.
    // Esse filtro é aplicado nas ações dos controladores onde é necessário garantir que o usuário tenha permissão para acessar.
    // Se o usuário não estiver logado ou não for cliente, ele será redirecionado para a página de inicial.
    // Caso contrário, a execução da ação do controlador continuará normalmente.
    public class PaginaCliente : ActionFilterAttribute
    {
        // Este método é executado antes da execução de qualquer ação do controlador.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessaoDoUsuario = context.HttpContext.Session.GetString("SessaoDoUsuarioLogado");

            // Verifica se o perfil do usuário é do tipo "Cliente".
            if (string.IsNullOrEmpty(sessaoDoUsuario))
            {
                // Se o usuário não for um cliente, redireciona para a página inicial.
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
            }
            else
            {
                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoDoUsuario);
                // Se o objeto usuário for nulo após a desserialização (isso pode acontecer se a sessão estiver corrompida ou inválida),
                // o usuário também é redirecionado para a página de login.
                if (usuario == null || usuario.Perfil != PerfilEnum.Cliente)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
                }
            }

            // Chama o método base para garantir que a lógica do filtro seja executada corretamente.
            base.OnActionExecuting(context);
        }
    }
}
