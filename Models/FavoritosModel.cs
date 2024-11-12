using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStore.Models
{
    public class FavoritosModel
    {
        public int FavoritosId { get; set; }

        public ProdutoModel Produto { get; set; } = new ProdutoModel();

        [ForeignKey("Produtos")]
        public int ProdutoId { get; set; }

        public UsuarioModel Usuario { get; set; } = new UsuarioModel();

        [ForeignKey("Usuarios")]
        public int UsuarioId { get; set; }
    }
}