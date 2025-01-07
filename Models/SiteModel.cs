using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class SiteModel
    {
        [Key]
        public int Id { get; set; }

        // Nome do site com limite de 20 caracteres
        [Required]
        [StringLength(20, ErrorMessage = "O nome do site deve ter no máximo 20 caracteres.")]
        public string NomeSite { get; set; } = "DigitalStore";

        // URL do primeiro banner
        public string Banner1Url { get; set; } = "banner-1.jpg";

        // URL do segundo banner (pode ser vazio)
        public string Banner2Url { get; set; } = string.Empty;

        // URL do terceiro banner (pode ser vazio)
        public string Banner3Url { get; set; } = string.Empty;

        // URL do quarto banner (pode ser vazio)
        public string Banner4Url { get; set; } = string.Empty;

        // Frase do site com limite de 50 caracteres
        [Required]
        [StringLength(50, ErrorMessage = "A frase deve ter no máximo 50 caracteres")]
        public string Frase { get; set; } = "Tudo que você procura em um só lugar";
    }
}