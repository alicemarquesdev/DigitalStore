/*
    SCRIPT PARA ENVIAR NEWSLETTER
    Este script lida com o envio de e-mails para a newsletter. Ele realiza a validação do e-mail inserido e a autorização do usuário, 
    enviando a solicitação via AJAX para o servidor.

    Funcionalidades principais:
    - Valida se o campo de e-mail não está vazio.
    - Verifica se o usuário autorizou o envio de e-mails promocionais.
    - Envia a solicitação para o servidor via AJAX.
    - Exibe mensagens de erro ou sucesso dependendo da resposta do servidor.
 
*/

$(document).ready(function () {

    // Adiciona um evento de clique no botão com id 'enviarNewsletter'
    $("#enviarNewsletter").click(function (e) {
        e.preventDefault(); // Previne o comportamento padrão do botão (submissão do formulário)

        // Obtém o valor do input de e-mail e remove espaços em branco ao redor
        var email = $("#emailInput").val().trim();

        // Verifica se o campo de e-mail está vazio
        if (email === "") {
            // Exibe uma mensagem de erro caso o campo de e-mail esteja vazio
            $("#newsletterMessage").html('<small class="text-danger">Digite seu email e fique por dentro de todas as novidades.</small>');
            return; // Impede a execução do restante do código
        }

        // Verifica se o checkbox de autorização foi marcado
        if (!$("#authorizeCheck").prop("checked")) {
            // Exibe uma mensagem de erro caso o usuário não tenha autorizado o envio de e-mails
            $("#newsletterMessage").html('<small class="text-danger">Você precisa autorizar o envio de e-mails promocionais.</small>');
            return; // Impede a execução do restante do código
        }

        $("#enviarNewsletter").prop("disabled", true);
        $("#iconNewsletter").removeClass("bi-send").addClass("spinner-border spinner-border-sm");


        // Envia os dados via AJAX para a URL '/Home/EnviarNewsletter'
        $.ajax({
            url: "/Home/EnviarNewsletter", // URL para onde os dados serão enviados
            type: "POST", // Método da requisição (POST)
            data: { email: email }, // Dados a serem enviados (e-mail)
            dataType: "json", // Tipo de dado que a resposta deve retornar (JSON)

            // Função executada caso a requisição seja bem-sucedida
            success: function (response) {
                if (response.success) {
                    // Se a resposta for bem-sucedida, exibe a mensagem de sucesso
                    $("#newsletterMessage").html('<small class="text-dark">' + response.message + '</small>');
                } else {
                    // Caso contrário, exibe a mensagem de erro
                    $("#newsletterMessage").html('<small class="text-danger">' + response.message + '</small>');
                }

                $("#enviarNewsletter").prop("disabled", false);
                $("#iconNewsletter").removeClass("spinner-border spinner-border-sm").addClass("bi-send");

            },

            // Função executada caso ocorra algum erro na requisição
            error: function () {
                // Exibe uma mensagem de erro caso a requisição falhe
                $("#newsletterMessage").html('<small class="text-danger">Erro ao processar a requisição.</small>');
                $("#enviarNewsletter").prop("disabled", false);
                $("#iconNewsletter").removeClass("spinner-border spinner-border-sm").addClass("bi-send");

            }
        });
    });
});
