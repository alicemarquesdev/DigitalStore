using DigitalStore.Data;
using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public class ContaRepositorio : IContaRepositorio
    {
        private readonly BancoContext _bancoContext;

        public ContaRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }



        public ContaModel Adicionar(ContaModel conta)
        {
            _bancoContext.Conta.Add(conta);
            _bancoContext.SaveChanges();
            return conta;
        }
    }
}
