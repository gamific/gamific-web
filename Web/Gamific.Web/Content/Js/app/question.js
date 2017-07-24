function loadDataTableQuestion() {
    tableQuestion = $('#questionDataTable').dataTable({
        "serverSide": true,
        "searching": true,
        "ajax": "/admin/question/search/",
        "processing": true,
        "ordering": true,
        "language": {
            "emptyTable": "Não foram encontrados resultados.",
            "paginate": {
                "previous": '<i class="fa fa-angle-left"></i>',
                "next": '<i class="fa fa-angle-right"></i>'
            }
        },
        "columnDefs": [
            {
                "width": "10%",
                "targets": 0,
                "orderable": true,
                "searchable": false,
            },
            {
                "width": "45%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "15%",
                "targets": 2,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var item = row[0].split(";");
                    var check = "Inativo";
                    if (data === "True") {
                        check = "Ativo";
                    }
                    return check;
                }
            },
            {
                "width": "30%",
                "targets": 3,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var id = row[0].split(";");
                    var links = "<a href='#' class='fa fa-plus' onclick='showDivQuestionAssociate(\"" + id + "\")' title='Associar Respostas.'></a> &nbsp; " +
                        "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/question/editar/" + id + "' title='Editar Perguntas.'> </a> &nbsp; " +
                        " <a class='fa fa-search-plus' onclick='showDivQuestionComplete(\"" + id + "\")'" + id + "' title='Visualizar Repostas Associadas'> </a>  &nbsp;" +
                        " <a class='fa fa-remove' href='#' onclick='removeQuestion(\"" + id + "\",\"" + id + "\")' title='Remover Perguntas.'> </a>";

                    return links;
                }
            }
        ]
    });


}

function removeQuestion(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo desativar o registro?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/question/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        toastr.success("Registro removido com sucess", 'Sucesso');

                        $('#questionDataTable').dataTable().fnDestroy();
                        loadDataTableQuestion();

                        dialog.close();
                    },
                    error: function () {
                        toastr.error("Houve um erro ao desativar o registro.", 'Erro');
                        dialog.close();
                    }
                });

            }
        }, {
            label: 'Não',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });

    dialog.getModalHeader().css("background-color", "#AA0000");
}

/*function onSucessSaveQuestionToast(data, status, xhr) {
    if (data && data.status === 'error') {
        toastr.error(data.message, 'Erro');
    } else if (data && data.status === 'warn') {
        toastr.warning(data.message, 'Aviso');
    }
    else {
        if (data && data.status === 'sucess') {
            toastr.success(data.message, 'Sucesso');
        }
        $('#questionDataTable').dataTable().fnDestroy();
        loadDataTableQuestion();
        $('.modal').modal('hide');
    }
}*/

/*function onFailureSaveQuestionToast() {
    toastr.error('Ocorreu um erro inesperado no sistema!', 'Erro');
}*/

function onSucessSaveQuestion() {
    verifyErrors();
}

function onFailureSaveQuestion() {

}

loadDataTableQuestion();
