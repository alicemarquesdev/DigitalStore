using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DigitalStore.Repositorio
{
    public class EnderecoRepositorio : IEnderecoRepositorio
    {
        private readonly BancoContext _context;
        private readonly string googleMapsApiKey = "AIzaSyDG0kqBvyaIBJjspyDxSWGaEwbaokw4BNg";

        public EnderecoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public async Task<EnderecoModel> BuscarEnderecoPorIdAsync(int enderecoId)
        {
            try
            {
                var endereco = await _context.Endereco.FirstOrDefaultAsync(x => x.EnderecoId == enderecoId);
                if (endereco == null) throw new Exception("Endereço não encontrado.");

                return endereco;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EnderecoModel>> BuscarTodosOsEnderecosDoUsuarioAsync(int id)
        {
            var enderecos = await _context.Endereco.Where(x => x.UsuarioId == id).ToListAsync();

            if (enderecos == null) return new List<EnderecoModel>();

            return enderecos;
        }

        public async Task AddEnderecoAsync(EnderecoModel endereco)
        {
            _context.Endereco.Add(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverEnderecoAsync(int id)
        {
            var endereco = await BuscarEnderecoPorIdAsync(id);

            if (endereco == null) return false;

            _context.Endereco.Remove(endereco);
            await _context.SaveChangesAsync();
            return true;
        }

        // Método para extrair o estado do endereço (simulando uma geocodificação)
        public async Task<string> ObterEstadoDoEnderecoAsync(string endereco)
        {
            // URL da API do Google Geocoding
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(endereco)}&key={googleMapsApiKey}";

            using (var client = new HttpClient())
            {
                // Fazendo a requisição HTTP para a API do Google
                var response = await client.GetStringAsync(url);
                var jsonResponse = JObject.Parse(response);

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

        // Método para calcular o frete por região
        public decimal CalcularFretePorRegiao(string estado)
        {
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
                {
                    return precos[regiao.Key]; // Retorna o preço do frete da região
                }
            }

            // Caso o estado não esteja em nenhuma região, retornamos um preço padrão
            return 25.00m; // Preço para estados não definidos
        }
    }
}