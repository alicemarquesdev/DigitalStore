using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStore.Models
{
    public class CarrinhoModel
    {
        [Key]
        public int CarrinhoId { get; set; }

        [ForeignKey("Usuarios")]
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; } = new UsuarioModel();

        [ForeignKey("Produtos")]
        public int ProdutoId { get; set; }

        public ProdutoModel Produto { get; set; } = new ProdutoModel();
    }
}