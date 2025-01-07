namespace DigitalStore.Helper
{
    public class FreteServicos
    {
        private readonly HttpClient _httpClient;

        public FreteServicos(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> CalcularFreteComAPIAsync(string cep)
        {
            if (!await CepNordesteAsync(cep))
            {
                return 0;
            }
            return 20.00m;
        }

        public async Task<bool> CepNordesteAsync(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return false;

            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            string uf = data.uf;

            // Estados do Nordeste
            var estadosNordeste = new[] { "BA", "SE", "AL", "PE", "PB", "RN", "CE", "PI", "MA" };

            return estadosNordeste.Contains(uf);
        }
    }
}