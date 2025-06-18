using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para view Carrinho
    // O usuário pode adicionar e escolher endereços de entrega.
    public class CarrinhoViewModel
    {
        // Chave do Google API, para esconder a chvae no link da view
        public string? GoogleApiKey { get; set; }

        // Endereço principal selecionado para entrega (pode ser nulo se o usuário ainda não escolheu um)
        public EnderecoModel? Endereco { get; set; }

        // Lista de itens adicionados ao carrinho pelo usuário
        public List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();

        // Lista de endereços cadastrados pelo usuário para escolha na finalização da compra
        public List<EnderecoModel> Enderecos { get; set; } = new List<EnderecoModel>();
    }
}
