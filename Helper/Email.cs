using System.Net;
using System.Net.Mail;

namespace DigitalStore.Helper
{
    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;

        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Enviar(string email, string mensagem, string assunto)
        {
            try
            {
                // Recupera as configurações do SMTP do arquivo de configuração
                string host = _configuration.GetValue<string>("SMTP:Host");
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string userName = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                // Criação da mensagem de e-mail
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(userName, nome);
                    mail.To.Add(email);
                    mail.Subject = assunto;
                    mail.Body = mensagem;
                    mail.IsBodyHtml = true; // Marca o corpo como HTML
                    mail.Priority = MailPriority.High; // Define a prioridade do e-mail

                    // Envia o e-mail utilizando o SmtpClient
                    using (var smtp = new SmtpClient(host, porta))
                    {
                        smtp.Credentials = new NetworkCredential(userName, senha);
                        smtp.EnableSsl = true; // Ativa o uso de SSL

                        smtp.Send(mail); // Envia a mensagem
                    }
                }

                return true; // E-mail enviado com sucesso
            }
            catch (Exception ex)
            {
                // Registra o erro em um log (pode ser implementado conforme necessário)
                // Exemplo: _logger.LogError($"Erro ao enviar e-mail: {ex.Message}");

                return false; // Retorna false em caso de erro
            }
        }
    }
}