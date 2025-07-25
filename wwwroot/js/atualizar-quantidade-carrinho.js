/*
    SCRIPT PARA ATUALIZAR A QUANTIDADE DO PRODUTO NO CARRINHO
    Este arquivo contém a lógica para atualizar a quantidade de um produto no carrinho de compras.

    Funcionalidades principais:
    - Quando o botão de aumentar ou diminuir quantidade é clicado, a quantidade do produto no carrinho é atualizada.
    - A requisição é enviada para o servidor via AJAX para refletir as mudanças no carrinho.
    - A interface do usuário (UI) é atualizada para mostrar a nova quantidade após a resposta do servidor.

    Dependências:
    - jQuery para manipulação do DOM e requisições AJAX.
    - Backend configurado para manipular a requisição '/Carrinho/AtualizarQuantidade' e retornar a nova quantidade.

*/

function atualizarQuantidade(produtoId, acao) {
    // Criar o objeto de requisição com os dados necessários: ID do produto e a ação (aumentar ou diminuir)
    const requestData = {
        ProdutoId: produtoId,  // O ID do produto que será alterado
        Acao: acao             // A ação que será executada: 'aumentar' ou 'diminuir'
    };

    // Requisição AJAX para enviar os dados para o servidor
    $.ajax({
        url: '/Carrinho/AtualizarQuantidade',  // URL para onde a requisição será enviada
        type: 'POST',  // Método da requisição: POST
        contentType: 'application/json',  // Tipo de conteúdo enviado (JSON)
        data: JSON.stringify(requestData),  // Transforma o objeto requestData em uma string JSON para enviar
        success: function (response) {
            // Função de sucesso que é chamada se a requisição for bem-sucedida

            if (response.success) {  // Verifica se a resposta do servidor indicou sucesso
                // Localizar o input de quantidade correspondente ao produto clicado
                // Utiliza-se o atributo 'data-produto-id' para identificar o produto específico
                const inputQuantidade = $('input[name="Quantidade"][data-produto-id="' + produtoId + '"]');

                // Atualiza o valor do input com a nova quantidade retornada do servidor
                inputQuantidade.val(response.novaQuantidade);
            }
        },
        error: function () {
            // Se ocorrer um erro na requisição (exemplo: falha na comunicação), exibe uma mensagem de erro genérica
            alert('Erro ao atualizar a quantidade.');
        }
    });
}

// Exemplo de como chamar a função para aumentar a quantidade
// O seletor pega todos os botões com a classe '.btnAumentar'
$('.btnAumentar').click(function () {
    // Obtém o ID do produto a partir do atributo 'data-produto-id' do botão
    const produtoId = $(this).data('produto-id');

    // Chama a função 'atualizarQuantidade' passando o ID do produto e a ação 'aumentar'
    atualizarQuantidade(produtoId, 'aumentar');
});

// Exemplo de como chamar a função para diminuir a quantidade
// O seletor pega todos os botões com a classe '.btnDiminuir'
$('.btnDiminuir').click(function () {
    // Obtém o ID do produto a partir do atributo 'data-produto-id' do botão
    const produtoId = $(this).data('produto-id');

    // Chama a função 'atualizarQuantidade' passando o ID do produto e a ação 'diminuir'
    atualizarQuantidade(produtoId, 'diminuir');
});
