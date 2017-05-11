function loadItemsDataTable() {
    loadData();
    table = $('#itemsDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/items/search/",
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
                "width": "90%",
                "targets": 0,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "10%",
                "targets": 1,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/items/editar/" + data + "' title='Edite este item'> </a>";
                    links += " <a class='fa fa-times'  href='#' onclick='removeClickItem(\"" + data + "\",\"" + name + "\")' title='Delete esse item.'> </a>";
                    
                    return links;
                }
            }
            

        ]
    });
}

function removeClickItem(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo Excluir esse item " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/items/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Item excluido com sucesso.", "success");

                        $('#itemsDataTable').dataTable().fnDestroy();
                        loadItemsDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao Excluir o item.", "danger");
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




function onSucessSaveItems() {
    verifyErrors();
}

function onFailureSaveItems() {

}

function loadData() {
}


loadItemsDataTable();