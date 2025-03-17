using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para view Endereco
    // O usuário pode adicionar e escolher endereços de entrega.
    public class EnderecoViewModel
    {
        // Chave do Google API, para esconder a chvae no link da view
        public string? GoogleApiKey { get; set; }

        // ID do usuário logado.
        public int UsuarioId { get; set; }

        // Para adicionar um endereco
        public EnderecoModel? Endereco { get; set; }

        // Lista de endereços cadastrados pelo usuário para escolha na finalização da compra
        public List<EnderecoModel> Enderecos { get; set; } = new List<EnderecoModel>();

    }
}
