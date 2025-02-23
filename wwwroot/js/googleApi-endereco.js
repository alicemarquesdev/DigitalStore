function initAutocomplete() {
    // Autocomplete para o CEP (somente códigos postais)
    let inputCep = document.getElementById("cep");
    let autocompleteCep = new google.maps.places.Autocomplete(inputCep, {
        types: ["postal_code"], // Somente CEP
        componentRestrictions: { country: "BR" }
    });

    // Autocomplete para endereço completo (rua, bairro, cidade, estado)
    let inputEndereco = document.getElementById("endereco");
    let autocompleteEndereco = new google.maps.places.Autocomplete(inputEndereco, {
        types: ["geocode"], // Endereço completo
        componentRestrictions: { country: "BR" }
    });

    // Quando o usuário seleciona um endereço, o evento é acionado
    autocompleteEndereco.addListener('place_changed', function () {
        var place = autocompleteEndereco.getPlace();

        if (!place.geometry) {
            document.getElementById("endereco").placeholder = 'Adicione um endereço';
        } else {
            // Exibe as informações no console ou faz outro tratamento com o lugar
            console.log(place);
            // Exemplo de como capturar as informações do endereço:
            document.getElementById("endereco").value = place.formatted_address;
            // Aqui você pode preencher outros campos ou fazer algo mais
        }
    });

    autocompleteCep.addListener('place_changed', function () {
        var place = autocompleteCep.getPlace();

        if (!place.geometry) {
            document.getElementById("cep").placeholder = 'Adicione um CEP';
        } else {
            // Exemplo de como preencher o campo do CEP
            document.getElementById("cep").value = place.formatted_address;
        }
    });
}

// Inicia o autocomplete assim que o script do Google Maps carregar
window.addEventListener("load", initAutocomplete);