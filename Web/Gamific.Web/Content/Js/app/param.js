function loadParamDataTable() {
    table = $('#paramDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/param/search/",
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
        "fnServerParams": function (aoData) { },
        "columnDefs": [
            {
                "width": "20%",
                "targets": 0,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "20%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "50%",
                "targets": 2,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 3,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/param/editar/" + data + "' title='Editar Parametro.'>" + "&nbsp;&nbsp;" +
                                     " <a class='fa fa-remove'  href='#' onclick='removeClickParam(\"" + data + "\",\"" + name + "\")' title='Excluir Parametro.'> </a>";

                    return links;
                }
            }
        ]
    });
}

function onSucessSaveParam() {
    verifyErrors();
}

function removeClickParam(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover o parametro " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/param/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Parametro removido com sucesso.", "success");

                        $('#paramDataTable').dataTable().fnDestroy();
                        loadParamDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover parametro.", "danger");
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

function onFailureSaveParam() {

}

$(document).ready(function () {
    loadParamDataTable();
});

