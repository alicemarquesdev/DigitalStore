using DigitalStore.Enums;
using DigitalStore.Models;
using DigitalStore.Helper.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Filters
{
    // Filtro de ação personalizado utilizado para verificar se o usuário está logado e se tem o perfil de administrador.
    // Esse filtro é aplicado nas ações dos controladores onde é necessário garantir que o usuário tenha permissão para acessar.
    // Se o usuário não estiver logado ou não for administrador, ele será redirecionado para a página de inicial.
    // Caso contrário, a execução da ação do controlador continuará normalmente.
    public class PaginaUsuarioLogado : ActionFilterAttribute
    {
        private readonly ISessao _sessao;

        // Construtor que recebe a interface ISessao por injeção de dependência, responsável por gerenciar a sessão do usuário
        public PaginaUsuarioLogado(ISessao sessao)
        {
            _sessao = sessao;
        }

        // Este método é executado antes de a ação do controlador ser executada
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Recupera a sessão do usuário logado utilizando a interface ISessao
            UsuarioModel usuario = _sessao.BuscarSessaoDoUsuario();

            // Se o usuário não estiver logado ou não for um administrador
            if (usuario == null || usuario.Perfil != PerfilEnum.Admin)
            {
                // Redireciona o usuário para a página de login ou para a página inicial
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
            }

            // Chama a base para garantir que a lógica do filtro seja executada corretamente
            base.OnActionExecuting(context);
        }
    }
}
