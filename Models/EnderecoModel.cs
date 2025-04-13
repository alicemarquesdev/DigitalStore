using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Representa o endereço de um usuário.
    // Um endereço pode estar associado a um ou mais pedidos de um usuário.
    public class EnderecoModel
    {
        // ID único do endereço
        [Key]
        public int EnderecoId { get; set; }

        // Endereço completo (rua, número, etc.)
        [Required(ErrorMessage = "O campo endereço é obrigatório.")]
        [StringLength(150, ErrorMessage = "O endereço deve ter no máximo 150 caracteres.")]
        public required string EnderecoCompleto { get; set; }

        // ID do usuário ao qual o endereço pertence
        [Required]
        public int UsuarioId { get; set; }

        // Relação com a entidade Usuario
        public virtual UsuarioModel? Usuario { get; set; }
    }
}
