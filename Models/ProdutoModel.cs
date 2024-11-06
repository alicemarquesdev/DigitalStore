using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }

        public string NomeProduto { get; set; } = string.Empty;

        public decimal Preco { get; set; } = 0.00m;

        public string Categoria { get; set; } = string.Empty;

        public string ImagemUrl { get; set; } = string.Empty;
    }
}