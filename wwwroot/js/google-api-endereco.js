/*
    SCRIPT PARA AUTOCOMPLETE DE ENDEREÇO COM GOOGLE MAPS
    Este script lida com o preenchimento automático do campo de endereço utilizando a API do Google Maps. Ele usa o Autocomplete para sugerir 
    endereços com base na entrada do usuário e valida a seleção de um endereço de rua.

    Funcionalidades principais:
    - Inicializa o Autocomplete para o campo de endereço.
    - Restringe a busca para endereços no Brasil e tipos de endereço.
    - Preenche o campo de entrada com o endereço completo quando o usuário seleciona uma sugestão válida.
    - Limpa o campo de entrada caso o endereço selecionado não seja um endereço de rua válido.
    - Exibe um alerta caso o usuário não selecione um endereço válido.
    - Limpa o campo de entrada caso o usuário perca o foco e o endereço não seja válido.

*/


// Variável para armazenar a instância do Autocomplete
let autocomplete;

// Função de inicialização do Autocomplete do Google Maps
function initAutocomplete() {
    // Seleciona o campo de entrada do endereço
    let inputEndereco = document.getElementById("endereco");

    // Verifica se o campo de entrada do endereço foi encontrado
    if (!inputEndereco) {
        console.error("Campo de endereço não encontrado!"); // Exibe um erro no console caso o campo não exista
        return; // Impede a execução do código caso o campo não seja encontrado
    }

    // Cria a instância do Autocomplete do Google Maps com as configurações necessárias
    autocomplete = new google.maps.places.Autocomplete(inputEndereco, {
        types: ['address'], // Restringe os resultados à tipos de endereço
        componentRestrictions: { country: "BR" } // Limita a busca a endereços no Brasil
    });

    // Adiciona um ouvinte de evento para quando o usuário selecionar um local
    autocomplete.addListener("place_changed", function () {
        let place = autocomplete.getPlace(); // Obtém o local selecionado

        // Verifica se o local selecionado tem informações de geometria (detalhes do endereço)
        if (!place.geometry) {
            console.error("Nenhum detalhe do local foi encontrado."); // Exibe um erro caso o local não tenha detalhes
            return; // Impede o processamento adicional se o local não tiver detalhes
        }

        // Obtém o tipo do local selecionado
        const addressTypes = place.types;

        // Verifica se o tipo de endereço inclui "route" (indicando que é um endereço de rua)
        if (addressTypes.includes("route")) {
            inputEndereco.value = place.formatted_address; // Preenche o campo com o endereço completo formatado
        } else {
            inputEndereco.value = ''; // Limpa o campo caso o endereço não seja válido
            alert("Por favor, selecione um endereço válido de rua."); // Exibe um alerta informando ao usuário
        }
    });

    // Adiciona um evento 'blur' (perda de foco) no campo de entrada do endereço
    inputEndereco.addEventListener('blur', function () {
        const place = autocomplete.getPlace(); // Obtém o local selecionado após a perda de foco

        // Verifica se o local tem geometria e se o tipo do endereço não inclui "route"
        if (place.geometry && !place.types.includes("route")) {
            inputEndereco.value = ''; // Limpa o campo caso o endereço não seja um endereço de rua
        }
    });
}
