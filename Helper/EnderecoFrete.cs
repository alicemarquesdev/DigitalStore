using DigitalStore.Helper.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace DigitalStore.Helper
{
    // Classe responsável por calcular o frete com base no endereço do usuário
    public class EnderecoFrete : IEnderecoFrete
    {
        // Chave do Google Geocoding API
        private readonly string _googleAPIKey;

        // Construtor que recebe a chave da API do Google.
        public EnderecoFrete(IOptions<GoogleAPISettings> googleAPI, ILogger<EnderecoFrete> logger)
        {
            _googleAPIKey = googleAPI.Value.ApiKey ?? throw new ArgumentNullException(nameof(googleAPI.Value.ApiKey), "API key do google não encontrada");
        }

        // Método para calcular o frete com base no estado do endereço fornecido
        // Através do endereço, buscamos o estado e, se encontrado, calculamos o frete
        public async Task<decimal> CalcularFretePorEnderecoAsync(string endereco)
        {
            try
            {
                // Primeiro, buscamos o estado a partir do endereço
                string estado = await BuscarEstadoDoEnderecoAsync(endereco);

                if (estado == "Desconhecido")
                {
                    throw new Exception("Estado não encontrado no endereço fornecido.");
                }

                // Definindo as regiões do Brasil
                var regioes = new Dictionary<string, string[]>
                {
                    { "Norte", new[] { "AC", "AM", "AP", "PA", "RO", "RR", "TO" } },
                    { "Nordeste", new[] { "AL", "BA", "CE", "MA", "PB", "PE", "PI", "RN", "SE", "BA" } },
                    { "Centro-Oeste", new[] { "GO", "MT", "MS", "DF" } },
                    { "Sudeste", new[] { "ES", "MG", "RJ", "SP" } },
                    { "Sul", new[] { "PR", "RS", "SC" } }
                };

                // Definindo os preços por região
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
                    if (regiao.Value.Contains(estado))
                    {
                        return precos[regiao.Key]; // Retorna o preço do frete da região
                    }
                }

                throw new ArgumentException($"Estado '{estado}' não encontrado nas regiões definidas.");
            }
            catch (Exception ex)
            {
                // Retorna uma exceção personalizada com detalhes do erro
                throw new Exception($"Erro ao calcular o frete para o endereço: {endereco}", ex);
            }
        }

        // Método para buscar o estado a partir do endereço fornecido
        private async Task<string> BuscarEstadoDoEnderecoAsync(string endereco)
        {
            // URL da API do Google Geocoding
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(endereco)}&key={_googleAPIKey}";

            using (var client = new HttpClient())
            {
                // Fazendo a requisição HTTP para a API do Google
                var response = await client.GetStringAsync(url);
                var jsonResponse = JObject.Parse(response);

                // LOG DA RESPOSTA
                Console.WriteLine("Resposta da API do Google Geocoding: " + jsonResponse.ToString());

                // Verificando se a resposta tem resultados
                if (jsonResponse["status"].ToString() == "OK")
                {
                    // Pega o primeiro resultado da geocodificação
                    var result = jsonResponse["results"][0];

                    // Percorrendo os componentes do endereço
                    foreach (var component in result["address_components"])
                    {
                        // Verificando se o componente é o estado
                        var types = component["types"];
                        if (types != null && types.Any(t => t.ToString() == "administrative_area_level_1"))
                        {
                            return component["short_name"].ToString(); // Retorna o código do estado (ex: "SP", "RJ")
                        }
                    }
                }

                // Caso não encontre o estado, retornamos "Desconhecido"
                return "Desconhecido";
            }
        }
    }
}
