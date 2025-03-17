/*
    SCRIPT PARA GERENCIAR O MODAL DE ATUALIZAÇÃO DE PRODUTOS
    Este script lida com a abertura de modais para exibir e editar as informações de um produto. Ele usa atributos personalizados dos botões para 
    preencher os campos do modal com os dados do produto, como nome, descrição, preço, quantidade em estoque, entre outros.

    Funcionalidades principais:
    - Seleciona os botões que abrem modais e contém dados de produtos.
    - Quando o botão é clicado, os dados do produto são extraídos dos atributos do botão.
    - Preenche o modal com os dados do produto, como ID, nome, descrição, categoria, preço, quantidade e imagem.
    - Se necessário, preenche um campo oculto para realizar a remoção do produto.

*/

document.addEventListener('DOMContentLoaded', function () {
    // Selecione todos os botões que têm o atributo data-bs-toggle="modal" e data-produto-id
    var buttons = document.querySelectorAll('[data-bs-toggle="modal"][data-produto-id]');

    // Para cada botão encontrado, adicione um listener de evento para o clique
    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            // Pega os valores dos atributos personalizados do botão (informações do produto)
            var produtoId = this.getAttribute('data-produto-id'); // ID do produto
            var nomeProduto = this.getAttribute('data-nomeproduto'); // Nome do produto
            var descricao = this.getAttribute('data-descricao'); // Descrição do produto
            var categoria = this.getAttribute('data-categoria'); // Categoria do produto
            var quantidade = this.getAttribute('data-quantidade'); // Quantidade em estoque
            var preco = this.getAttribute('data-preco'); // Preço do produto
            var imagem = this.getAttribute('data-imagem'); // URL da imagem do produto

            // Preencher os campos do modal com as informações do produto
            document.getElementById('ProdutoId').value = produtoId; // ID do produto
            document.getElementById('NomeProduto').value = nomeProduto; // Nome do produto
            document.getElementById('Descricao').value = descricao; // Descrição do produto
            document.getElementById('Categoria').value = categoria; // Categoria do produto
            document.getElementById('QuantidadeEstoque').value = quantidade; // Quantidade em estoque
            document.getElementById('Preco').value = preco; // Preço do produto

            // Exibir a imagem do produto no modal
            var imgPreview = document.getElementById('imgPreview'); // Seleciona o elemento de imagem
            if (imgPreview) {
                imgPreview.src = imagem; // Define o src da imagem com o valor obtido do atributo data-imagem
            }

            // Atribui o ID do produto ao campo hidden do modal de remoção, caso exista
            if (document.getElementById("produtoId")) {
                document.getElementById("produtoId").value = produtoId; // Preenche o campo hidden com o ID
            }
        });
    });
});
