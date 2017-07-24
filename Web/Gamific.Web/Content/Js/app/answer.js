function loadDataTableAnswer() {
    tableAnswer = $('#anwserDataTable').dataTable({
        "serverSide": true,
        "searching": true,
        "ajax": "/admin/answer/search/",
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
                "width": "15%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "35%",
                "targets": 2,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 3,
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
                "targets": 4,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var id = row[0].split(";");
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/answer/editar/" + id + "' title='Editar Answer.'> </a> &nbsp; " +
                        " <a class='fa fa-remove' href='#' onclick='remove(\"" + id + "\",\"" + id + "\")' title='Remover Answer.'> </a>";

                    return links;
                }
            }
        ]
    });


}

function remove(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo desativar o registro?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/answer/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        toastr.success("Registro removido com sucess", 'Sucesso');

                        $('#anwserDataTable').dataTable().fnDestroy();
                        loadDataTableAnswer();

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

/*function onSucessSaveAnswerToast(data, status, xhr) {
    if (data && data.status === 'error') {
        toastr.error(data.message, 'Erro');
    } else if (data && data.status === 'warn') {
        toastr.warning(data.message, 'Aviso');
    }
    else {
        if (data && data.status === 'sucess') {
            toastr.success(data.message, 'Sucesso');
        }
        $('#anwserDataTable').dataTable().fnDestroy();
        loadDataTableAnswer();
        $('.modal').modal('hide');
    }
}*/

/*function onFailureSaveAnswerToast() {
    toastr.error('Ocorreu um erro inesperado no sistema!', 'Erro');
}*/

function onSucessSaveAnswer() {
    verifyErrors();
}
function onFailureSaveAnswer() {

}
loadDataTableAnswer();
