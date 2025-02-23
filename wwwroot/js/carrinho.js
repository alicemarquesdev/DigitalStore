document.addEventListener("DOMContentLoaded", function () {
    const botoesCarrinho = document.querySelectorAll(".carrinho-btn");

    botoesCarrinho.forEach((button) => {
        button.addEventListener("click", async function () {
            const produtoId = button.dataset.produtoId;

            try {
                const response = await fetch('/Carrinho/AddOuRemoverCarrinho', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ produtoId: produtoId })
                });

                const result = await response.json();

                if (result.success) {
                    const icon = button.querySelector("i");

                    if (result.carrinhoAtivo) {
                        button.classList.remove("text-dark");
                        button.classList.add("text-danger");
                        icon.classList.remove("bi-cart-plus");
                        icon.classList.add("bi-cart-fill");
                    } else {
                        button.classList.remove("text-danger");
                        button.classList.add("text-dark");
                        icon.classList.remove("bi-cart-fill");
                        icon.classList.add("bi-cart-plus");
                    }

                    // Força a re-renderização do botão
                    button.blur();
                } else {
                    alert('Não foi possível realizar a ação.');
                }
            } catch (error) {
                console.error('Erro ao tentar concluir ação:', error);
            }
        });
    });
});