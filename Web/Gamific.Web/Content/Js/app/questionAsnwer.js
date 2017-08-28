function loadAnswersDataTable() {

    $('#respostasDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/questionAnswer/search/" + $('#NumberOfAnswers').val(),
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
                "targets": 1,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/questionAnswer/editar/" + data + "' title='Editar Resposta.'> </a> &nbsp; <a class='fa fa-remove' href='/admin/questionAnswer/remover/" + data + "' title='Remover Resposta.'> </a>";

                    return links;
                }
            }
        ]
    });
};

function onSuccessSaveAnswer(data) {
    verifyErrors();
}

function onFailureSaveAnswer(data) {

}

$(document).ready(function () {
    loadAnswersDataTable();
});