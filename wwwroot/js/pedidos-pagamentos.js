/*
    SCRIPT PARA ABRIR MODAL DE STATUS DO PEDIDO
    Este script lida com a exibição dinâmica de modais de status de pedidos. Quando um botão de status é clicado, ele
    seleciona o modal correspondente ao ID do pedido e o exibe.

    Funcionalidades principais:
    - Seleciona todos os botões cujo ID começa com 'btnStatus'.
    - Ao clicar no botão, pega o ID do pedido a partir do atributo 'data-id'.
    - Constrói dinamicamente o ID do modal de status correspondente ao ID do pedido.
    - Exibe o modal utilizando o Bootstrap.

*/

document.addEventListener("DOMContentLoaded", function () {
    // Seleciona todos os botões cujo ID começa com 'btnStatus'
    const buttons = document.querySelectorAll('[id^="btnStatus"]');

    // Para cada botão encontrado, adicione um event listener para o evento de clique
    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            // Pega o valor do atributo 'data-id' do botão clicado, que contém o ID do pedido
            var pedidoId = this.getAttribute('data-id');

            // Construa o ID do modal de status usando o ID do pedido
            var modalId = '#modalStatus' + pedidoId; // Exemplo: '#modalStatus123'

            // Seleciona o modal correspondente usando o ID gerado
            var modal = new bootstrap.Modal(document.querySelector(modalId));

            // Mostra o modal usando o método show() do Bootstrap
            modal.show();
        });
    });
});
