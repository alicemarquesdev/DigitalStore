using DigitalStore.Helper;
using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface responsável apenas pelas operações de alteração de senha do usuário.
    public interface IAlteracaoSenhaRepositorio
    {
        // Altera a senha do usuário com base no modelo fornecido.
        Task AlterarSenhaAsync(AlterarSenhaModel alterarSenhaModel);

        // Redefine a senha do usuário com base no ID do usuário e na nova senha fornecida.
        Task<bool> RedefinirSenhaAsync(int id, string novaSenha);
    }
}
