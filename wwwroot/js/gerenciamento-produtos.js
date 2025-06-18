document.addEventListener('DOMContentLoaded', function () {
    // Botões que abrem modal de edição
    var buttons = document.querySelectorAll('[data-bs-toggle="modal"][data-produto-id]');

    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            var produtoId = this.getAttribute('data-produto-id');
            var nomeProduto = this.getAttribute('data-nomeproduto');
            var descricao = this.getAttribute('data-descricao');
            var categoria = this.getAttribute('data-categoria');
            var quantidade = this.getAttribute('data-quantidade');
            var preco = this.getAttribute('data-preco')?.replace(',', '.');
            var imagem = this.getAttribute('data-imagem');

            document.getElementById('ProdutoId').value = produtoId;
            document.getElementById('NomeProduto').value = nomeProduto;
            document.getElementById('Descricao').value = descricao;
            document.getElementById('Categoria').value = categoria;
            document.getElementById('QuantidadeEstoque').value = quantidade;
            document.getElementById('Preco').value = preco;

        });
    });

    // Preview imagem - modal adicionar
    const inputAdd = document.getElementById("ImagemFileAdd");
    const previewAdd = document.getElementById("imgPreviewAdd");

    if (inputAdd && previewAdd) {
        inputAdd.addEventListener("change", function () {
            const file = this.files[0];
            if (file) {
                if (!file.type.match('image.*')) {
                    alert('Por favor, selecione um arquivo de imagem.');
                    return;
                }
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewAdd.src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Preview imagem - modal atualizar
    const inputUpdate = document.getElementById("ImagemFileUpdate");
    const previewUpdate = document.getElementById("imgPreviewUpdate");

    if (inputUpdate && previewUpdate) {
        inputUpdate.addEventListener("change", function () {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewUpdate.src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Botões de remoção
    const removeButtons = document.querySelectorAll('[data-bs-target="#removeProdutoModal"]');
    removeButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            const produtoId = btn.getAttribute('data-produto-id');
            const inputHidden = document.getElementById('produtoId');
            if (inputHidden) {
                inputHidden.value = produtoId;
            }
        });
    });
});
