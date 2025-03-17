using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Modelo usado para atualização dos dados do usuário sem incluir a senha.
    public class UsuarioSemSenhaModel
    {
        // Identificador único do usuário
        public int Id { get; set; }

        // Nome do usuário 
        [Required(ErrorMessage = "Digite o seu nome.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ]+(?: [A-Za-zÀ-ÿ]+)*$", ErrorMessage = "O nome deve conter apenas letras e não pode ter espaços duplicados.")]
        [StringLength(30, ErrorMessage = "O nome deve ter no máximo 30 caracteres.")]
        public required string Nome { get; set; }

        // Email do usuário 
        [Required(ErrorMessage = "Digite o seu email.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(80, ErrorMessage = "O email deve ter no máximo 80 caracteres.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "O email não pode conter espaços, verifique o começo e o fim.")]
        public required string Email { get; set; }

        // Data de Nascimento do usuário
        [Required(ErrorMessage = "Informe a data de nascimento.")]
        [DataType(DataType.Date)]
        public required DateTime DataNascimento { get; set; }

        // Gênero do usuário
        [Required(ErrorMessage = "Informe o gênero.")]
        public required GeneroEnum Genero { get; set; }
    }
}
