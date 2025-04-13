/*
    SCRIPT PARA INTEGRAÇÃO DE PAGAMENTO COM STRIPE
    Este script lida com a integração do Stripe para realizar pagamentos via cartão de crédito. Ele cria os elementos de pagamento, 
    coleta as informações do cartão, gera um token de pagamento e envia os dados para o servidor.

    Funcionalidades principais:
    - Inicializa o Stripe com a chave pública passada da view.
    - Cria os campos de entrada do número do cartão, data de expiração e CVC com os estilos personalizados.
    - Realiza a captura do evento de envio do formulário de pagamento.
    - Cria um token do cartão de crédito utilizando o Stripe.
    - Exibe mensagens de erro caso o token não seja criado corretamente.
    - Envia os dados de pagamento (token, valor, endereço, etc.) para o backend via requisição AJAX.
    - Redireciona o usuário para a página de sucesso ou exibe um erro caso o pagamento falhe.
*/
if(typeof stripeKey === 'undefined' || !stripeKey) {
    alert("Chave Stripe não definida.");
    throw new Error("stripeKey não está definida.");
}

// Inicialização do Stripe com a chave pública
var stripe = Stripe(stripeKey);  // Usando a chave pública passada da View
var elements = stripe.elements(); // Criação dos elementos do Stripe

// Definindo o estilo dos campos do cartão
var style = {
    base: {
        color: '#32325d', // Cor base do texto
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif', // Família de fontes
        fontSize: '16px', // Tamanho da fonte
        '::placeholder': {
            color: '#aab7c4' // Cor do texto de placeholder
        }
    },
    invalid: {
        color: '#fa755a', // Cor do texto quando o valor é inválido
        iconColor: '#fa755a' // Cor do ícone quando o valor é inválido
    }
};

// Criando os componentes de entrada para o número do cartão, expiração e CVC
var cardNumber = elements.create('cardNumber', { style: style });
var cardExpiry = elements.create('cardExpiry', { style: style });
var cardCvc = elements.create('cardCvc', { style: style });

// Montando os campos do Stripe na página
cardNumber.mount('#card-number');
cardExpiry.mount('#card-expiry');
cardCvc.mount('#card-cvc');

// Captura o envio do formulário
var form = document.getElementById('payment-form');
form.addEventListener('submit', function (event) {
    event.preventDefault(); // Previne o comportamento padrão de envio do formulário

    // Criando o token do cartão com Stripe
    stripe.createToken(cardNumber).then(function (result) {
        console.log(result); // Verifica o resultado do token gerado

        if (result.error) {
            // Se houver erro ao criar o token, exibe a mensagem de erro
            var errorElement = document.getElementById('card-errors');
            errorElement.textContent = result.error.message; // Mostra o erro no campo específico
        } else {
            // Se o token foi criado com sucesso, prepara o modelo de pagamento
            var stripeModel = {
                UsuarioId: usuarioId, // ID do usuário (passado da view)
                EnderecoCompleto: endereco, // Endereço completo (passado da view)
                Token: result.token.id, // Token gerado pelo Stripe
                Valor: valor.toFixed(2), // Valor total da compra (formato 2 casas decimais)
                Moeda: "brl", // Moeda: Real Brasileiro
                Descricao: "Compra na DigitalStore" // Descrição da compra
            };

            console.log(stripeModel); // Verificando o objeto antes de enviar

            // Envia a requisição para o backend para processar o pagamento
            fetch('/Pagamento/ProcessarPagamento', {
                method: 'POST',
                body: JSON.stringify(stripeModel),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                return response.json();
            }).then(function (data) {
                if (data.success) {
                    // Redireciona para a página de sucesso no pagamento
                    window.location.href = '/Pagamento/PagamentoSucesso';
                } else {
                    alert("Erro no pagamento: " + data.message);
                }
            }).catch(function (error) {
                alert("Erro na requisição: " + error.message);
            });
        }
    });
});
