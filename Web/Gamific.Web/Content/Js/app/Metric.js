function loadMetricDataTable(){
    table = $('#metricDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/metricas/search/",
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
                "width": "70%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
                "render": function (data, type, row) {
                    var icon = "fa " + data.split(";")[0];
                    var regex = new RegExp("_");
                    while (regex.test(icon)) {
                        icon = icon.replace("_", "-");
                    }
    
                    var metricName = data.split(";")[1];

                    var links = "<i style='padding-right:6px;' class='" + icon + "'> </i>" + " " + metricName;

                    return links;
                }
            },
            {
                "width": "20%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 2,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[1];
                    var links = "<a href='/admin/funcaoMetrica/associar/" + data + "' class='fa fa-plus' title='Associar tipo de jogador.'></a> &nbsp; " + 
                        "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/metricas/editar/" + data + "' title='Editar Metrica.'> </a> &nbsp; " +
                        " <a class='fa fa-remove' href='#' onclick='removeClickMetric(\"" + data + "\",\"" + name + "\")' title='Remover Metrica.'> </a>";

                    return links;
                }
            }
        ]
    });
}

function removeClickMetric(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover a metrica " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/metricas/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Metrica removida com sucesso.", "success");

                        $('#metricDataTable').dataTable().fnDestroy();
                        loadMetricDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover metrica.", "danger");
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

function onSucessSaveMetric(){
    verifyErrors();
}

function onFailureSaveMetric(){

}

$(document).ready(function () {
    loadMetricDataTable();
});

