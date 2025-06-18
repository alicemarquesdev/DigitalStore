namespace DigitalStore.Helper.Interfaces
{
    // Interface que define métodos para calcular o frete, com base no endereço do usuário
    public interface IEnderecoFrete
    {
        // Método para calcular o valor do frete diretamente a partir do endereço
        // Recebe o endereço completo como parâmetro e retorna o valor do frete.
        Task<decimal> CalcularFretePorEnderecoAsync(string endereco);
    }
}
