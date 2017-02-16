function loadWorkerTypeDataTable() {
    $('#workerTypeDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/funcoes/search/" + $('#NumberOfWorkerTypes').val(),
        "scrollY": "300px",
        "processing": true,
        "ordering": true,
        "scrollCollapse": true,
        "deferRender": true,
        "lengthChange": false,
        "language": {
            "emptyTable": "N�o foram encontrados resultados.",
            "paginate": {
                "previous": '<i class="fa fa-angle-left"></i>',
                "next": '<i class="fa fa-angle-right"></i>'
            }
        },
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "columnDefs": [
                {
                    "width": "45%",
                    "targets": 0,
                    "orderable": true,
                    "searchable": true,
                },
                {
                    "width": "45%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true,
                },
                {
                    "width": "10%",
                    "targets": 2,
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row) {
                        var name = row[0].split(";")[0];
                        var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/funcoes/editar/" + data + "' title='Editar Fun��o.'> </a> &nbsp; <a class='fa fa-remove' href='#' onclick='removeClickWorkerType(\"" + data + "\",\"" + name + "\")' title='Remover Fun��o.'> </a>";

                        return links;
                    }
                }
        ]
    });
};

function removeClickWorkerType(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Aten��o!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover a fun��o " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/funcoes/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Fun��o removida com sucesso.", "success");

                        $('#workerTypeDataTable').dataTable().fnDestroy();
                        loadWorkerTypeDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover fun��o.", "danger");
                        dialog.close();
                    }
                });

            }
        }, {
            label: 'N�o',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });

    dialog.getModalHeader().css("background-color", "#AA0000");
}


loadWorkerTypeDataTable();

function onSuccessSaveWorkerType(data) {
    verifyErrors();
}

function onFailureSaveWorkerType(data) {

}