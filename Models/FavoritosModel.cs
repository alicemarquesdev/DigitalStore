namespace DigitalStore.Models
{
    public class FavoritosModel
    {
        public int ProdutoId { get; set; }
        public virtual ProdutoModel Produto { get; set; }

        public int UsuarioId { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
    }
}