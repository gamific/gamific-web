
function loadDataTableAnswerComplete() {
    tableAnswer = $('#anwserDataTableComplete').dataTable({
        "serverSide": true,
        "searching": true,
        "ajax": "/admin/answer/searchAssociate/" + idPrincipal,
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
                "width": "20%",
                "targets": 2,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 3,
                "searchable": false,
                "render": function (data, type, row) {
                    var item = row[3].split(";");
                    var check = "Inativo";
                    if (item == "True") {
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
                    var id = row[4].split(";");
                    var links = " <a class='fa fa-remove' href='#' onclick='removeAssociatedAnswer(" + id + ")' title='Remover Associação.'> </a>";

                    return links;
                }
            }
        ]
    });


}
function removeAssociatedAnswer(data) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo desativar o registro?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/answer/associate/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        toastr.success("Registro removido com sucess", 'Sucesso');

                        $('#anwserDataTableComplete').dataTable().fnDestroy();
                        loadDataTableAnswerComplete();

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


loadDataTableAnswerComplete();
