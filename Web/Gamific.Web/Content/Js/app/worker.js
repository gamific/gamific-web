function loadWorkersDataTable() {
    $('#funcionariosDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/funcionarios/search/" + $('#NumberOfWorkers').val(),
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
                "width": "45%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
                "render": function (data, type, row) {

                    var render = data.split(";")[0] + "<img src='https://s3.amazonaws.com/gamific-prd/images/logos/empresas/logo-" + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";

                    return render;
                }
            },
            {
                "width": "25%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "25%",
                "targets": 2,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "5%",
                "targets": 3,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/funcionarios/editar/" + data + "' title='Editar Funcionário.'> </a> &nbsp; <a class='fa fa-remove' href='#' onclick='removeClickTeam(\"" + data + "\",\"" + name + "\")' title='Remover Funcionário.'> </a>";

                    return links;
                }
            }
        ]
    });
};

function removeClickTeam(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover o jogador " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/funcionarios/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Jogador removido com sucesso.", "success");

                        $('#funcionariosDataTable').dataTable().fnDestroy();
                        loadWorkersDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover jogador.", "danger");
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

function onSuccessSaveWorker(data) {

    verifyErrors();
}

function onFailureSaveWorker(data) {

}

function onSuccessSaveWorkerArchive(data) {

    verifyErrors();
}

function onFailureSaveWorkerArchive(data) {

}

function loadLogo(inputFile) {
    if (inputFile.files && inputFile.files[0]) {
        document.getElementById('img').src = '/api/media/0';
        document.getElementById('img').src = URL.createObjectURL(inputFile.files[0]);
        document.getElementById('img').style.display = 'block';
    }
}

$(document).ready(function () {
    loadWorkersDataTable();
})

/*{
    "width": "15%",
    "targets": 1,
    "orderable": true,
    "searchable": true,
    "render": function (data, type, row) {
        var render = "";

        if (data){
            render = data.substr(0, 3) + "." + data.substr(3, 3) + "." + data.substr(6, 3) + "-" + data.substr(9,2);
        }
        
        return render;
    }
},*/