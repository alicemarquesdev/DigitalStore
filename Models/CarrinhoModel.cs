using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Modelo que representa o carrinho de compras.
    // Estabelece uma relação de muitos-para-muitos entre produtos e usuários.
    public class CarrinhoModel
    {
        // Identificador do produto no carrinho.
        public int ProdutoId { get; set; }

        // Navegação para a entidade Produto associada ao carrinho.
        public virtual ProdutoModel? Produto { get; set; }

        // Identificador do usuário que adicionou o produto ao carrinho.
        public int UsuarioId { get; set; }

        // Navegação para a entidade Usuário que possui o carrinho.
        public virtual UsuarioModel? Usuario { get; set; }

        // Quantidade do produto no carrinho. Inicialmente, o valor é 0.
        [Range(0, 100, ErrorMessage = "A quantidade deve ser entre 0 e 100.")]
        public int Quantidade { get; set; } = 1;
    }
}
