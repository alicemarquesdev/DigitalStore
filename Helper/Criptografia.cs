using System.Security.Cryptography;
using System.Text;

namespace DigitalStore.Helper
{
    public static class Criptografia
    {
        public static string GerarHash(this string valor)
        {
            // Criação de uma instância do algoritmo SHA1
            using (var hash = SHA1.Create())
            {
                // Codificação da string de entrada para um array de bytes
                var encoding = new ASCIIEncoding();
                var bytes = encoding.GetBytes(valor);

                // Computa o hash
                var hashBytes = hash.ComputeHash(bytes);

                // Converte os bytes do hash para uma string hexadecimal
                var strHexa = new StringBuilder();
                foreach (var byteValue in hashBytes)
                {
                    strHexa.Append(byteValue.ToString("x2"));
                }

                // Retorna o hash gerado em formato hexadecimal
                return strHexa.ToString();
            }
        }
    }
}