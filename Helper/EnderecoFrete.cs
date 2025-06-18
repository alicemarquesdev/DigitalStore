using DigitalStore.Helper.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace DigitalStore.Helper
{
    // Classe responsável por calcular o frete com base no endereço do usuário
    public class EnderecoFrete : IEnderecoFrete
    {

        // Método para calcular o frete com base no estado do endereço fornecido
        // Através do endereço, buscamos o estado e, se encontrado, calculamos o frete
        public async Task<decimal> CalcularFretePorEnderecoAsync(string endereco)
        {
            try
            {
                // Mapeando estados diretamente para suas siglas
                var estadosMapeados = new Dictionary<string, string>
        {
            { "Acre", "AC" },
            { "Alagoas", "AL" },
            { "Amazonas", "AM" },
            { "Bahia", "BA" },
            { "Ceará", "CE" },
            { "Espírito Santo", "ES" },
            { "Goiás", "GO" },
            { "Maranhão", "MA" },
            { "Mato Grosso", "MT" },
            { "Mato Grosso do Sul", "MS" },
            { "Minas Gerais", "MG" },
            { "Pará", "PA" },
            { "Paraíba", "PB" },
            { "Paraná", "PR" },
            { "Pernambuco", "PE" },
            { "Piauí", "PI" },
            { "Rio de Janeiro", "RJ" },
            { "Rio Grande do Norte", "RN" },
            { "Rio Grande do Sul", "RS" },
            { "Rondônia", "RO" },
            { "Roraima", "RR" },
            { "Santa Catarina", "SC" },
            { "São Paulo", "SP" },
            { "Sergipe", "SE" },
            { "Tocantins", "TO" }
        };

                // Primeiro tenta pelo nome completo
                var estado = estadosMapeados
                    .FirstOrDefault(kvp => endereco.Contains(kvp.Key)).Value;

                // Se não encontrar, tenta pela sigla
                if (string.IsNullOrEmpty(estado))
                {
                    estado = estadosMapeados
                        .FirstOrDefault(kvp => endereco.Contains(kvp.Value)).Value;
                }

                if (string.IsNullOrEmpty(estado))
                {
                    throw new Exception("Estado não encontrado no endereço fornecido.");
                }

                var regioes = new Dictionary<string, string[]>
        {
            { "Norte", new[] { "AC", "AM", "AP", "PA", "RO", "RR", "TO" } },
            { "Nordeste", new[] { "AL", "BA", "CE", "MA", "PB", "PE", "PI", "RN", "SE" } },
            { "Centro-Oeste", new[] { "GO", "MT", "MS", "DF" } },
            { "Sudeste", new[] { "ES", "MG", "RJ", "SP" } },
            { "Sul", new[] { "PR", "RS", "SC" } }
        };

                var precos = new Dictionary<string, decimal>
        {
            { "Norte", 25.00m },
            { "Nordeste", 0.00m },
            { "Centro-Oeste", 20.00m },
            { "Sudeste", 35.00m },
            { "Sul", 30.00m }
        };

                // Verificando em qual região o estado está e retornando o frete
                foreach (var regiao in regioes)
                {
                    if (regiao.Value.Contains(estado))
                    {
                        return precos[regiao.Key];
                    }
                }

                throw new ArgumentException($"Estado '{estado}' não encontrado nas regiões definidas.");
            }
            catch (Exception ex)
            {
                // Logando o erro
                Console.WriteLine($"Erro ao calcular o frete: {ex.Message}");
                throw new Exception($"Erro ao calcular o frete para o endereço: {endereco}", ex);
            }
        }

    }
}
