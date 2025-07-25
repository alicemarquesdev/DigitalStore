/*
    SCRIPT DE VALIDAÇÃO E ATUALIZAÇÃO DO BOTÃO 'PROSSEGUIR'
    Este script contém a lógica para validar a seleção de um endereço de envio e a verificação do carrinho de compras antes de prosseguir.

    Funcionalidades principais:
    - Impede que o usuário avance se nenhum endereço de envio for selecionado, exibindo um alerta.
    - Impede que o usuário avance se o carrinho estiver vazio, exibindo um alerta.
    - Atualiza o atributo 'asp-route-id' do botão 'btnProsseguir' com o ID do endereço selecionado, garantindo que a seleção seja refletida na URL para o backend.

    Dependências:
    - O botão 'btnProsseguir' deve estar presente na página e o atributo 'asp-route-id' deve ser configurado para passar o ID do endereço.
    - A variável 'carrinhoVazio' deve ser passada pela View para indicar o status do carrinho.
*/

document.getElementById('btnProsseguir').addEventListener('click', function (e) {
    // Seleciona o endereço de envio marcado pelo usuário
    var enderecoSelecionado = document.querySelector('input[name="EnderecoId"]:checked');

    // Se nenhum endereço foi selecionado, impede a navegação e exibe uma mensagem de alerta
    if (!enderecoSelecionado) {
        e.preventDefault();  // Impede a navegação do botão
        alert("Por favor, selecione um endereço de envio antes de prosseguir.");
        return;  // Sai da função, não permitindo a navegação
    }

    // Verifica se o carrinho está vazio usando a variável 'carrinhoVazio' passada pela View
    if (carrinhoVazio === true) {
        e.preventDefault();  // Impede a navegação do botão
        alert("Seu carrinho está vazio. Adicione produtos antes de prosseguir.");
        return;  // Sai da função, não permitindo a navegação
    }

    var enderecoId = enderecoSelecionado.value;

    // Redireciona manualmente
    window.location.href = `/Pagamento/Pagamento?id=${enderecoId}`;
});
