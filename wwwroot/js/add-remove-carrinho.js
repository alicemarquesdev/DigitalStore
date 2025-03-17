/*
    SCRIPT PARA ADICIONAR E REMOVER PRODUTOS DO CARRINHO
    Este arquivo contém a lógica para interagir com os botões de adicionar ou remover produtos do carrinho de compras.
    
    Funcionalidades principais:
    - Quando um botão de carrinho é clicado, um produto é adicionado ou removido do carrinho.
    - Atualização visual do botão de carrinho para refletir o estado atual (produto adicionado ou removido).
    - Comunicação assíncrona com o backend para atualizar o carrinho no servidor.

    Dependências:
    - JavaScript assíncrono (fetch API).
    - Bootstrap (para ícones de carrinho e manipulação de estilos).
*/

// Aguarda o carregamento completo do DOM antes de executar o código
document.addEventListener("DOMContentLoaded", function () {

    // Seleciona todos os botões que possuem a classe "carrinho-btn"
    const botoesCarrinho = document.querySelectorAll(".carrinho-btn");

    // Adiciona um evento de clique para cada botão do carrinho
    botoesCarrinho.forEach((button) => {
        button.addEventListener("click", async function () {

            // Obtém o ID do produto a partir do atributo "data-produto-id" do botão clicado
            const produtoId = button.dataset.produtoId;

            try {
                // Faz uma requisição assíncrona para a rota do backend responsável por adicionar/remover o produto do carrinho
                const response = await fetch('/Carrinho/AddOuRemoverCarrinho', {
                    method: 'POST', // Define o método HTTP como POST
                    headers: { 'Content-Type': 'application/json' }, // Define o tipo de conteúdo como JSON
                    body: JSON.stringify({ produtoId: produtoId }) // Envia o ID do produto como um objeto JSON
                });

                // Converte a resposta do servidor para JSON
                const result = await response.json();

                // Verifica se a operação foi bem-sucedida
                if (result.success) {
                    // Obtém o ícone dentro do botão
                    const icon = button.querySelector("i");

                    // Se o produto foi adicionado ao carrinho
                    if (result.carrinhoAtivo) {
                        button.classList.remove("text-dark"); // Remove a classe de cor escura
                        button.classList.add("text-danger"); // Adiciona a classe de cor vermelha
                        icon.classList.remove("bi-cart-plus"); // Remove o ícone de adicionar ao carrinho
                        icon.classList.add("bi-cart-fill"); // Adiciona o ícone de carrinho cheio
                    }
                    // Se o produto foi removido do carrinho
                    else {
                        button.classList.remove("text-danger"); // Remove a classe de cor vermelha
                        button.classList.add("text-dark"); // Adiciona a classe de cor escura
                        icon.classList.remove("bi-cart-fill"); // Remove o ícone de carrinho cheio
                        icon.classList.add("bi-cart-plus"); // Adiciona o ícone de adicionar ao carrinho
                    }

                    // Força a re-renderização do botão removendo o foco dele
                    button.blur();
                }
                else {
                    // Exibe um alerta caso a operação não tenha sido concluída com sucesso
                    alert('Não foi possível realizar a ação.');
                }
            }
            catch (error) {
                // Captura e exibe erros no console caso ocorra algum problema na requisição
                console.error('Erro ao tentar concluir ação:', error);
            }
        });
    });
});
