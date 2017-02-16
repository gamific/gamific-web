function loadWorkerTypeMetricsDataTable() {
    var table = $('#workerTypeMetricsDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/funcaoMetrica/search/" + $('#MetricId').val() + "/" + $('#NumberOfFunctions').val(),
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
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 1,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-remove' href='#' onclick='removeClickWorkerTypeMetric(\"" + data + "\",\"" + name + "\")' title='Remover associação.'> </a>";

                    return links;
                }
            }
        ],
    });
}

function loadWorkerTypesDataTable() {
    var table = $('#associateWorkerTypesDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/funcaoMetrica/searchToAssociate/" + $('#MetricId').val() + "/" + $('#NumberOfFunctionsToAssociate').val(),
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
                "width": "20%",
                "targets": 0,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var links = "<input type='checkbox' class='workerTypesCheck' value='" + data + "'>";

                    return links;
                }
            },
            {
                "width": "80%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            }
        ]
    });
}

function getWorkerTypesSelected() {
    var listWorkerTypes = $('.workerTypesCheck');

    var workerTypesToSend = "";

    var i;
    for(i = 0; i < listWorkerTypes.length; i++){
        if (listWorkerTypes[i].checked) {
            workerTypesToSend += listWorkerTypes[i].value + ",";
        }
    }

    $('#workerTypesId').val(workerTypesToSend);
}

function removeClickWorkerTypeMetric(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover a associação com a função " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/funcaoMetrica/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Associação removida com sucesso.", "success");

                        $('#workerTypeMetricsDataTable').dataTable().fnDestroy();
                        loadWorkerTypeMetricsDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao dessassociar função.", "danger");
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

$(document).ready(function () {
    loadWorkerTypeMetricsDataTable();
});

