/*
    SCRIPT DE VALIDAÇÃO DE SENHAS
    Este script verifica se as senhas inseridas nos campos 'Senha' e 'ConfirmarSenha' são iguais antes de permitir o envio do formulário.

    Funcionalidades principais:
    - Exibe uma mensagem de erro caso o campo 'ConfirmarSenha' esteja vazio ou as senhas não coincidam.
    - Impede o envio do formulário se as senhas não coincidirem, exibindo a mensagem de erro adequada.
    - Se as senhas coincidirem, permite o envio do formulário e limpa qualquer mensagem de erro anterior.
*/
function compararSenhas() {
    // Obtém os valores dos campos de senha e confirmar senha
    var senha = document.getElementById('Senha').value;
    var confirmarSenha = document.getElementById('ConfirmarSenha').value;

    // Obtém o elemento onde será exibida a mensagem de erro
    var errorMessage = document.getElementById('confirmarSenhaError');

    // Verifica se o campo 'ConfirmarSenha' está vazio
    if (confirmarSenha === "") {
        errorMessage.innerHTML = "Por favor, confirme sua senha."; // Exibe mensagem de erro
        return false; // Impede o envio do formulário
    }

    // Verifica se as senhas não coincidem
    if (senha !== confirmarSenha) {
        errorMessage.innerHTML = "As senhas não coincidem."; // Exibe mensagem de erro
        return false; // Impede o envio do formulário
    }

    // Se as senhas coincidirem, limpa qualquer mensagem de erro
    errorMessage.innerHTML = "";
    return true; // Permite o envio do formulário
}

// Adiciona um evento de submit ao formulário com o id 'formCriarConta'
document.getElementById('formCriarConta').addEventListener('submit', function (event) {
    // Se a função compararSenhas retornar 'false' (senhas não coincidem), impede o envio do formulário
    if (!compararSenhas()) {
        event.preventDefault(); // Impede o envio do formulário
    }
});
