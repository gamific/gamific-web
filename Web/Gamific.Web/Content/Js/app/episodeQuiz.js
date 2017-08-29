function loadQuizDataTable() {

    $('#quizDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/episodeQuiz/search/" + $('#NumberOfQuiz').val() + "?episodeId=" + $('#EpisodeId').val(),
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
                "width": "90%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 3,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var links = "<a class='fa fa-plus' href='/admin/episodeQuiz/cadastrarPergunta/" + data + "' title='Adicionar Pergunta.'> </a> &nbsp; <a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/episodeQuiz/editar/" + data + "' title='Editar Quiz.'> </a> &nbsp; <a class='fa fa-remove' href='/admin/episodeQuiz/remover/" + data + "' title='Remover Quiz.'> </a>";

                    return links;
                }
            }
        ]
    });
};

function onSuccessSaveQuiz(data) {
    verifyErrors();
}

function onFailureSaveQuiz(data) {

}

$(document).ready(function () {
    loadQuizDataTable();
});