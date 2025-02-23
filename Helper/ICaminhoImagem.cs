namespace DigitalStore.Helper
{
    public interface ICaminhoImagem
    {
        Task<string> GerarCaminhoArquivoAsync(IFormFile imagem);
    }
}