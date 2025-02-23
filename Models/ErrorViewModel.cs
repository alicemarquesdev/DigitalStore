namespace DigitalStore.Models
{
    public class ErrorViewModel
    {
        // Identificador único da requisição, usado para rastrear o erro
        public string? RequestId { get; set; }

        // Mensagem detalhada sobre o erro que ocorreu
        public string? Message { get; set; }

        // Código de status HTTP associado ao erro
        public int StatusCode { get; set; }

        // Indica se o RequestId deve ser exibido
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

}
