/*
    SCRIPT PARA PRÉ-VISUALIZAÇÃO DE IMAGEM
    Este script permite que o usuário visualize a imagem selecionada antes de enviá-la. Ao escolher um arquivo de imagem,
    o script exibe a imagem em um elemento de pré-visualização na página.

    Funcionalidades principais:
    - Escuta o evento 'change' no campo de input de arquivo de imagem.
    - Quando um arquivo é selecionado, lê o conteúdo do arquivo usando FileReader.
    - Exibe a imagem selecionada no elemento de pré-visualização (imgPreview).

*/

document.getElementById('ImagemFile').addEventListener('change', function (event) {
    // Obtém o elemento de pré-visualização da imagem (onde a imagem será exibida)
    const imgPreview = document.getElementById('imgPreview');

    // Obtém os arquivos selecionados pelo usuário
    const files = event.target.files;

    // Verifica se há pelo menos um arquivo selecionado
    if (files.length > 0) {
        // Cria um objeto FileReader para ler o arquivo selecionado
        const reader = new FileReader();

        // Define o que deve acontecer quando a leitura do arquivo for concluída
        reader.onload = function (e) {
            // Define a fonte da imagem de pré-visualização com o conteúdo do arquivo
            imgPreview.src = e.target.result;
        };

        // Inicia a leitura do arquivo selecionado, convertendo-o em um URL de dados (base64)
        reader.readAsDataURL(files[0]);
    }
});
