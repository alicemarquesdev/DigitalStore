namespace DigitalStore.Models
{
    public class ErrorViewModel
    {
        // Identificador �nico da requisi��o, usado para rastrear o erro
        public string? RequestId { get; set; }

        // Mensagem detalhada sobre o erro que ocorreu
        public string? Message { get; set; }

        // C�digo de status HTTP associado ao erro
        public int StatusCode { get; set; }

        // Indica se o RequestId deve ser exibido
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

}
