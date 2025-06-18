using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para a view GerenciamentoProduto
    // Usuários com permissão de administrador podem adicionar, editar e excluir produtos
    // Ao atualizar um produto, o usuário tem acesso ao select com todas as categorias disponíveis
    public class GerenciamentoProdutoViewModel
    {
        // Usado quando é necessário exibir ou editar as informações de um único produto
        public ProdutoModel? Produto { get; set; }

        // Lista de categorias disponíveis para o produto
        // Essas categorias são usadas para preencher um campo de seleção ao adicionar ou editar um produto
        public List<string> Categorias { get; set; } = new List<string>();

        // Lista de todos os produtos existentes no sistema
        public List<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
    }
}
