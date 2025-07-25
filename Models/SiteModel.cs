using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Modelo que representa os dados do site que será exibido na Página Inicial do projeto.
    // O usuário administrador poderá alterar o nome do site, a imagem do banner e a frase de destaque.
    public class SiteModel
    {
        // ID único para identificar a configuração do site
        [Key]
        public int Id { get; set; }

        // Nome do site com limite de 20 caracteres
        [StringLength(20, ErrorMessage = "O nome do site deve ter no máximo 20 caracteres.")]
        [Required(ErrorMessage = "O campo não pode ser vazio")]
        public required string NomeSite { get; set; } = "DigitalStore";

        // Caminho para o banner do site, valor padrão para a imagem do banner
        public string? Banner { get; set; } = "~/image/banner.jpg";

        // Frase de destaque para o site, com limite de 40 caracteres
        [StringLength(40, ErrorMessage = "A frase deve ter no máximo 40 caracteres.")]
        [Required(ErrorMessage = "O campo não pode ser vazio")]
        public string Frase { get; set; }
    }
}
