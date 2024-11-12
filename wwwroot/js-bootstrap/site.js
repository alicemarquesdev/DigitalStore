// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $('#table-produtos').DataTable({
        "paging": true,         // Ativa a paginação
        "ordering": true,       // Ativa a ordenação das colunas
        "searching": true,      // Ativa a barra de busca
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "Nada encontrado",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Nenhum registro disponível",
            "infoFiltered": "(filtrado de _MAX_ registros totais)",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            },
            "search": "Pesquisar:"
        }
    });
});

$('.close-alert').click(function () {
    $(".alert").hide('hide');