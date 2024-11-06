using System.Collections;

namespace DigitalStore.Models
{
    public class CarrinhoModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}