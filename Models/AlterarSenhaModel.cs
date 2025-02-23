﻿using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Modelo para a alteração de senha do usuário.
    public class AlterarSenhaModel
    {
        // Identificador único do usuário.
        public int Id { get; set; }

        // Senha atual do usuário.
        [Required(ErrorMessage = "Digite sua senha atual.")]
        public required string SenhaAtual { get; set; }

        // Nova senha que o usuário deseja configurar.
        // A senha deve ter entre 8 e 20 caracteres, incluindo pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.
        [Required(ErrorMessage = "Digite a nova senha.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 20 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[a-zA-Z\d\W_]{8,20}$", ErrorMessage = "A senha deve ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.")]
        public required string NovaSenha { get; set; }

        // Confirmação da nova senha inserida pelo usuário.
        // A senha deve ser igual à NovaSenha.
        [Required(ErrorMessage = "Digite novamente a nova senha.")]
        [Compare("NovaSenha", ErrorMessage = "Não confere com a nova senha")]
        public required string ConfirmarNovaSenha { get; set; }
    }
}
