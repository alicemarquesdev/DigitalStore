document.addEventListener("DOMContentLoaded", function () {
    // Seleciona todos os botões de curtida
    const botoesFavorito = document.querySelectorAll(".favorito-btn");

    botoesFavorito.forEach((button) => {
        button.addEventListener("click", async function () {
            const produtoId = button.dataset.produtoId; // Obtém o postId do atributo data-post-id

            try {
                const response = await fetch('/Favorito/AddOuRemoverFavorito', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ produtoId: produtoId })
                });

                const result = await response.json();

                if (result.success) {
                    const icon = button.querySelector("i");

                    if (result.favoritoAtivo) {
                        button.classList.add("text-danger");
                        button.classList.remove("text-dark");
                        icon.classList.add("bi-heart-fill");
                        icon.classList.remove("bi-heart");
                    } else {
                        button.classList.add("text-dark");
                        button.classList.remove("text-danger");
                        icon.classList.add("bi-heart");
                        icon.classList.remove("bi-heart-fill");
                    }
                } else {
                    alert('Não foi possível realizar a ação.');
                }
            } catch (error) {
                console.error('Erro ao tentar concluir ação:', error);
            }
        });
    });
});