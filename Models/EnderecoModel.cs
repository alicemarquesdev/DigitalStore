using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStore.Models
{
    public class EnderecoModel
    {
        [Key]
        public int EnderecoId { get; set; }

        [Required(ErrorMessage = "O campo Rua é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Rua deve ter no máximo 200 caracteres.")]
        public string Rua { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Bairro deve ter no máximo 100 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Cidade deve ter no máximo 100 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo Estado deve ter no máximo 50 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo CEP deve ter no máximo 20 caracteres.")]
        public string CEP { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo UsuarioId é obrigatório.")]
        public int UsuarioId { get; set; }

        public virtual UsuarioModel? Usuario { get; set; }

        public virtual IList<PedidoModel>? Pedido { get; set; }
    }
}
