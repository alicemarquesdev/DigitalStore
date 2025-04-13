using DigitalStore.Enums;
using DigitalStore.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStore.Models
{
    // Modelo representando um usuário no sistema.
    // Existe a opção de administrador apenas para o projeto ser testado, essa opção não ficaria visível e teria dados padrões para admin.
    // E a opção cliente padrão.
    public class UsuarioModel
    {
        // ID único do usuário
        [Key]
        public int UsuarioId { get; set; }

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

        // Perfil do usuário
        public PerfilEnum Perfil { get; set; } = PerfilEnum.Cliente;

        // Senha do usuário 
        [Required(ErrorMessage = "Digite a nova senha.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[a-zA-Z\d\W_]{8,20}$", ErrorMessage = "A senha deve ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial. No minimo 8 e no maximo 20 caracteres.")]
        public required string Senha { get; set; }

        // Lista de carrinhos do usuário (relacionamento com CarrinhoModel)
        public virtual List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();

        // Lista de produtos favoritos do usuário (relacionamento com FavoritosModel)
        public virtual List<FavoritosModel> Favoritos { get; set; } = new List<FavoritosModel>();

        // Lista de pedidos realizados pelo usuário (relacionamento com PedidoModel)
        public virtual List<PedidoModel> Pedido { get; set; } = new List<PedidoModel>();

        // Lista de endereços associados ao usuário (relacionamento com EnderecoModel)
        public virtual List<EnderecoModel> Endereco { get; set; } = new List<EnderecoModel>();

        // Método para verificar se a senha fornecida é válida.
        // Criptografa a senha fornecida pelo usuário e compara com a senha armazenada no banco de dados, que está criptografada.
        public bool SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        // Método para configurar a senha com seu hash, criptografar senha
        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

        // Método para gerar uma nova senha aleatória
        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8); // Gera uma nova senha com 8 caracteres
            return novaSenha;
        }

        // Método para configurar uma nova senha com seu hash
        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.GerarHash();
        }
    }
}
