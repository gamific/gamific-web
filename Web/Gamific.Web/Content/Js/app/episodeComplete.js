
function loadDataTableEpisodeComplete() {
    tableAnswer = $('#episodeCompleteDataTable').dataTable({
        "serverSide": true,
        "searching": true,
        "ajax": "/admin/episode/searchAssociate/" + idPrincipal,
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
                "searchable": true,
                "visible": true,
            },
            {
                "width": "40%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "15%",
                "targets": 1,
                "orderable": false,
                "searchable": false,
            },
            {
                "width": "15%",
                "targets": 2,
                "orderable": false,
                "searchable": false,
            },
            {
                "width": "30%",
                "targets": 4,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var id = row[0].split(";");
                    debugger;
                    var links = " <a class='fa fa-remove' href='#' onclick='removeAssociatedEpisode(" + id + ")' title='Remover Associação.'> </a>";

                    return links;
                }
            }
        ]
    });


}
function removeAssociatedEpisode(data) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo desativar o registro?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/episode/associate/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        toastr.success("Registro removido com sucess", 'Sucesso');

                        $('#episodeCompleteDataTable').dataTable().fnDestroy();
                        loadDataTableEpisodeComplete();

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


loadDataTableEpisodeComplete();

