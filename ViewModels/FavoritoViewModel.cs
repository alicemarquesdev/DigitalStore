using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para view Favoritos
    // O usuário consegue visualizar os produtos favoritos e adicionar ou remover do carrinho.
    public class FavoritoViewModel
    {
        public List<FavoritosModel> Favoritos { get; set; } = new List<FavoritosModel>();

        public List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();
    }
}