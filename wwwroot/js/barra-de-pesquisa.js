/*
    SCRIPT DE BARRA DE PESQUISA COM SUGESTÕES EM TEMPO REAL
    Este arquivo contém a lógica de busca de sugestões enquanto o usuário digita na barra de pesquisa.

    Funcionalidades principais:
    - Ao digitar no campo de pesquisa, é feita uma requisição assíncrona para o servidor para buscar sugestões com base no termo inserido.
    - As sugestões são exibidas em um dropdown dinâmico abaixo do campo de pesquisa.
    - Se o termo for vazio, o dropdown é ocultado.
    - Caso o usuário clique fora do campo de pesquisa ou do dropdown, ele é fechado.

    Dependências:
    - `fetch` para realizar a requisição assíncrona ao servidor.
    - Backend configurado para retornar sugestões no endpoint '/Home/BarraDePesquisa' baseado no termo de pesquisa.
*/

document.addEventListener("DOMContentLoaded", function () {

    // Seleciona os elementos da página: o campo de pesquisa (input) e o dropdown onde as sugestões aparecerão
    const searchInput = document.getElementById("searchInput");
    const searchDropdown = document.getElementById("searchDropdown");

    // Adiciona um evento de "keyup" (quando o usuário digita no campo de pesquisa)
    searchInput.addEventListener("keyup", function () {

        // Obtém o valor digitado no campo de pesquisa e remove espaços em branco nas extremidades
        let searchTerm = searchInput.value.trim();

        // Só realiza a busca se o termo tiver pelo menos 1 caractere (ajuste conforme desejado)
        if (searchTerm.length > 0) {

            // Envia uma requisição para o servidor para obter sugestões com base no termo digitado
            fetch(`/Home/BarraDePesquisa?termo=${searchTerm}`)
                .then(response => response.json()) // Converte a resposta em formato JSON
                .then(data => {
                    // Limpa as sugestões anteriores (caso haja)
                    searchDropdown.innerHTML = "";

                    // Se houver resultados, cria os links no dropdown para cada sugestão
                    if (data.length > 0) {
                        data.forEach(item => {
                            // Cria um novo link de sugestão para cada item retornado
                            let option = document.createElement("a");
                            option.href = `/Home/Produto/${item.id}`; // Define o link para a página do produto (ajuste conforme sua rota)
                            option.classList.add("dropdown-item"); // Adiciona a classe para estilo
                            option.textContent = item.nome; // Define o nome do item como o texto do link
                            searchDropdown.appendChild(option); // Adiciona o novo item ao dropdown
                        });

                        // Exibe o dropdown se houver resultados
                        searchDropdown.style.display = "block";
                    } else {
                        // Caso não haja resultados, oculta o dropdown
                        searchDropdown.style.display = "none";
                    }
                })
                .catch(error => console.error("Erro ao buscar sugestões:", error)); // Exibe um erro no console se a requisição falhar
        } else {
            // Se o termo de pesquisa for vazio, oculta o dropdown
            searchDropdown.style.display = "none";
        }
    });

    // Adiciona um evento de clique no documento para fechar o dropdown se o usuário clicar fora
    document.addEventListener("click", function (event) {
        // Verifica se o clique foi fora do campo de pesquisa e do dropdown
        if (!searchInput.contains(event.target) && !searchDropdown.contains(event.target)) {
            // Se for, oculta o dropdown
            searchDropdown.style.display = "none";
        }
    });
});
