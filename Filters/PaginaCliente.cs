using DigitalStore.Enums;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalStore.Filters
{
    // Filtro de ação personalizado utilizado para verificar se o usuário está logado e se tem o perfil de cliente.
    // Esse filtro é aplicado nas ações dos controladores onde é necessário garantir que o usuário tenha permissão para acessar.
    // Se o usuário não estiver logado ou não for cliente, ele será redirecionado para a página de inicial.
    // Caso contrário, a execução da ação do controlador continuará normalmente.
    public class PaginaCliente : ActionFilterAttribute
    {
        private readonly ISessao _sessao;

        // Construtor que recebe a interface ISessao por injeção de dependência, responsável por gerenciar a sessão do usuário
        public PaginaCliente(ISessao sessao)
        {
            _sessao = sessao;
        }

        // Este método é executado antes da execução de qualquer ação do controlador.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Recupera a sessão do usuário logado usando a interface ISessao.
            UsuarioModel usuario = _sessao.BuscarSessaoDoUsuario();

            // Verifica se o perfil do usuário é do tipo "Cliente".
            if (usuario == null || usuario.Perfil != PerfilEnum.Cliente)
            {
                // Se o usuário não for um cliente, redireciona para a página inicial.
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } });
            }

            // Chama o método base para garantir que a lógica do filtro seja executada corretamente.
            base.OnActionExecuting(context);
        }
    }
}
