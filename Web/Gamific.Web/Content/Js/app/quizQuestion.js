function loadQuestionsDataTable() {

    $('#perguntasDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/quizQuestion/search/" + $('#NumberOfQuestions').val(),
        "scrollY": "300px",
        "processing": true,
        "ordering": true,
        "scrollCollapse": true,
        "deferRender": true,
        "lengthChange": false,
        "language": {
            "emptyTable": "Não foram encontrados resultados.",
            "paginate": {
                "previous": '<i class="fa fa-angle-left"></i>',
                "next": '<i class="fa fa-angle-right"></i>'
            }
        },
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [
            {
                "width": "30%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "30%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "30%",
                "targets": 2,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 3,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var links = "<a class='fa fa-plus' href='/admin/quizQuestion/cadastrarResposta/" + data + "' title='Adicionar Resposta.'> </a> &nbsp; <a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/quizQuestion/editar/" + data + "' title='Editar Pergunta.'> </a> &nbsp; <a class='fa fa-remove' href='/admin/quizQuestion/remover/" + data + "' title='Remover Pergunta.'> </a>";

                    return links;
                }
            }
        ]
    });
};

function onSuccessSaveQuestion(data) {
    verifyErrors();
}

function onFailureSaveQuestion(data) {

}

$(document).ready(function () {
    loadQuestionsDataTable();
});