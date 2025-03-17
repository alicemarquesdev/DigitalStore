namespace DigitalStore.Enums
{
    // O enum 'StatusPagamentoEnum' representa os possíveis estados do pagamento de um pedido na aplicação.
    public enum StatusPagamentoEnum
    {
        // O estado 'Pendente' representa um pagamento que ainda não foi processado ou confirmado.
        Pendente = 0,

        // O estado 'Pago' representa um pagamento que foi confirmado com sucesso.
        Pago = 1,

        // O estado 'Falhou' representa um pagamento que não foi processado corretamente devido a algum erro.
        Falhou = 2,

        // O estado 'Cancelado' representa um pagamento que foi cancelado pelo Administrador.
        Cancelado = 3
    }
}
