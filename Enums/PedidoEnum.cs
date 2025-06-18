namespace DigitalStore.Enums
{
    // O enum representa os possíveis estados de um pedido na aplicação.
    // Cada valor no enum está associado a um número inteiro, que facilita a representação dos estados.
    public enum PedidoEnum
    {
        // O estado 'Pendente' representa um pedido que ainda não foi processado.
        Pendente = 0,

        // O estado 'Em Preparação' representa um pedido que está sendo preparado, mas ainda não foi enviado.
        EmPreparacao = 1,

        // O estado 'Enviado' representa um pedido que foi enviado para o cliente.
        Enviado = 2,

        // O estado 'Cancelado' representa um pedido que foi cancelado.
        Cancelado = 3,

        // O estado 'Concluído' representa um pedido que foi finalizado com sucesso, ou seja, foi entregue ao cliente ou completado de alguma forma.
        Concluido = 4
    }
}
