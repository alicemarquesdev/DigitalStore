namespace DigitalStore.Models
{
    public class FavoritosModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}