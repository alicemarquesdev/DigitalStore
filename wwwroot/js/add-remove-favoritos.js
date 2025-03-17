/*
    SCRIPT PARA ADICIONAR E REMOVER PRODUTOS DOS FAVORITOS
    Este arquivo contém a lógica para gerenciar os produtos favoritos, permitindo adicionar ou remover itens da lista de favoritos ao clicar em um botão.

    Funcionalidades principais:
    - Quando o botão de favorito é clicado, o produto é adicionado ou removido da lista de favoritos.
    - A aparência do botão é atualizada para refletir o estado do favorito (ícone de coração preenchido ou vazio, mudança de cor).
    - Comunicação assíncrona com o backend para atualizar o estado do favorito no servidor.

    Dependências:
    - JavaScript assíncrono (fetch API).
    - Bootstrap (para ícones de coração e manipulação de estilos).
*/

document.addEventListener("DOMContentLoaded", function () {
    // Aguarda o carregamento completo do DOM antes de executar o código
    // Seleciona todos os botões de "curtir" ou "favoritar", que possuem a classe CSS 'favorito-btn'
    const botoesFavorito = document.querySelectorAll(".favorito-btn");

    // Itera sobre todos os botões de favorito encontrados na página
    botoesFavorito.forEach((button) => {
        // Adiciona um ouvinte de evento de clique para cada botão
        button.addEventListener("click", async function () {
            // Obtém o ID do produto associado ao botão através do atributo 'data-produto-id'
            const produtoId = button.dataset.produtoId;

            try {
                // Realiza uma requisição HTTP POST para o servidor para adicionar ou remover o produto dos favoritos
                const response = await fetch('/Favorito/AddOuRemoverFavorito', {
                    method: 'POST',  // Método da requisição, que é POST
                    headers: { 'Content-Type': 'application/json' },  // Define o tipo de conteúdo como JSON
                    body: JSON.stringify({ produtoId: produtoId })  // Envia o ID do produto como JSON no corpo da requisição
                });

                // Converte a resposta do servidor para um objeto JSON
                const result = await response.json();

                // Verifica se a resposta do servidor indicou sucesso
                if (result.success) {
                    // Seleciona o ícone dentro do botão (presumivelmente um ícone de coração)
                    const icon = button.querySelector("i");

                    // Se o produto foi favoritado (favoritoAtivo for true)
                    if (result.favoritoAtivo) {
                        // Modifica as classes do botão e do ícone para refletir o estado de favorito
                        button.classList.add("text-danger");  // Adiciona uma classe de estilo para cor vermelha (favorito)
                        button.classList.remove("text-dark");  // Remove a classe de cor escura (não favorito)
                        icon.classList.add("bi-heart-fill");  // Altera o ícone para um coração preenchido
                        icon.classList.remove("bi-heart");  // Remove o ícone de coração vazio
                    } else {
                        // Caso o produto tenha sido removido dos favoritos
                        button.classList.add("text-dark");  // Retorna a cor escura (não favorito)
                        button.classList.remove("text-danger");  // Remove a cor vermelha (favorito)
                        icon.classList.add("bi-heart");  // Altera o ícone para um coração vazio
                        icon.classList.remove("bi-heart-fill");  // Remove o ícone de coração preenchido
                    }

                    // Força a re-renderização do botão, removendo o foco dele
                    button.blur();  // Isso é feito para garantir que o botão perca o foco após o clique

                } else {
                    // Se o servidor retornar uma resposta indicando falha
                    alert('Não foi possível realizar a ação.');  // Exibe um alerta informando que a ação não pôde ser realizada
                }
            } catch (error) {
                // Se ocorrer um erro durante a requisição (exemplo: falha na rede), exibe o erro no console
                console.error('Erro ao tentar concluir ação:', error);
            }
        });
    });
});
