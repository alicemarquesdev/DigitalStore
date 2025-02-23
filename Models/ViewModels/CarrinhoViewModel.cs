namespace DigitalStore.Models.ViewModels
{
    // ViewModel para view Carrinho
    // O usuário pode adicionar e escolher endereços de entrega.
    public class CarrinhoViewModel
    {
        // Endereço principal selecionado para entrega (pode ser nulo se o usuário ainda não escolheu um)
        public EnderecoModel? Endereco { get; set; }

        // Lista de itens adicionados ao carrinho pelo usuário
        public List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();

        // Lista de endereços cadastrados pelo usuário para escolha na finalização da compra
        public List<EnderecoModel> Enderecos { get; set; } = new List<EnderecoModel>();
    }
}
