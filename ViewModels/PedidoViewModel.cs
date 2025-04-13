using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para Pedido Controller
    public class PedidoViewModel
    {
        public int UsuarioId { get; set; }

        public List<PedidoModel> Pedidos { get; set; } = new List<PedidoModel>();
    }
}
