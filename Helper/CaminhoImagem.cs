namespace DigitalStore.Helper
{
    public class CaminhoImagem : ICaminhoImagem
    {
        private readonly string _sistema;

        public CaminhoImagem(IWebHostEnvironment sistema)
        {
            _sistema = sistema.WebRootPath;
        }

        // Método para gerar caminho de arquivo da imagem do produto
        public async Task<string> GerarCaminhoArquivoAsync(IFormFile imagem)
        {
            // Gera um código único para o arquivo
            var codigoUnico = Guid.NewGuid().ToString();

            // Obtém a extensão original do arquivo (ex: ".jpg", ".png")
            var extensao = Path.GetExtension(imagem.FileName).ToLower();

            // Gera o nome final do arquivo (sem espaços e com a extensão original)
            var nomeCaminhoImagem = codigoUnico + extensao;

            // Caminho para salvar a imagem, dentro da pasta "wwwroot/images"
            var caminhoParaSalvarImagem = Path.Combine(_sistema, "image");

            // Verifica se a pasta existe e, se não, cria
            if (!Directory.Exists(caminhoParaSalvarImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagem);
            }

            // Salva o arquivo no caminho especificado
            var caminhoCompleto = Path.Combine(caminhoParaSalvarImagem, nomeCaminhoImagem);
            using (var stream = File.Create(caminhoCompleto))
            {
                await imagem.CopyToAsync(stream);
            }

            return Path.Combine("~/image", nomeCaminhoImagem).Replace("\\", "/");
        }
    }
}