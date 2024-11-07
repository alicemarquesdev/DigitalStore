using System.Collections;

namespace DigitalStore.Models
{
    public class CarrinhoModel
    {
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}