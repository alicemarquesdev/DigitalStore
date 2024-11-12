using System.Globalization;

namespace DigitalStore.Helper
{
    public interface IEmail
    {
        bool Enviar(string email, string mensagem, string assunto);
    }
}