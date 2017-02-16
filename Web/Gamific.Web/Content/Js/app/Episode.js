function loadEpisodeDataTable() {
    table = $('#episodeDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/episode/search/",
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
                "width": "30%",
                "targets": 0,
                "orderable": true,
                "searchable": true
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
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/episode/editar/" + data + "' title='Editar'> </a> &nbsp; " +
                                     " <a class='fa fa-clone' onclick='showEntityModal(this); return false;'  href='/admin/episode/clonar/" + data + "' title='Clonar.'> </a>" + "&nbsp;&nbsp;";
                                     

                    if (row[2] == "Sim") {
                        links += " <a class='fa fa-remove'  href='#' onclick='removeClickEpisode(\"" + data + "\",\"" + name + "\")' title='Finalizar.'> </a>";
                    }

                    return links;
                }
            }
        ]
    });
}

function removeClickEpisode(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo inativar a campanha " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/episode/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Campanha inativada com sucesso.", "success");

                        $('#episodeDataTable').dataTable().fnDestroy();
                        loadEpisodeDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao inativar a campanha.", "danger");
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

function onSucessSaveEpisode() {
    verifyErrors();
}

function onFailureSaveEpisode() {

}

loadEpisodeDataTable();

