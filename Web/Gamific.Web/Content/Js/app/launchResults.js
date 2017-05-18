function loadLaunchResultsDataTable() {
    var logoPath = "";
    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    $('#launchResultsDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/lancarResultados/search/",
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
                "width": "40%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
                "render": function (data, type, row) {

                    var render = data.split(";")[0] + "<img src='" + logoPath + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";

                    return render;
                }
            },
            {
                "width": "30%",
                "targets": 1,
                "orderable": true,
                "serchable": true
            },
            {
                "width": "25%",
                "targets": 2,
                "orderable": true,
                "serchable": true,
            },
            {
                "width": "5%",
                "targets": 3,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var teste = $('#dropDownTeams').val();
                    var tesde2 = data;
                    var links = "<a class='fa fa-plus' href='/admin/lancarResultados/edit/" + $('#dropDownTeams').val() + "/" + data + "' onclick='showEntityModal(this);return false;'></a>";
                    return links;
                }
            }
        ],
    });
}

$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
});

$('#dropDownTeams').change(function () {
    $.ajax({
        url: "/admin/lancarResultados/atualizarEquipe/" + $("#dropDownTeams").val(),
        async: false,
        success: function () {
            $('#launchResultsDataTable').dataTable().fnDestroy();
            loadLaunchResultsDataTable();
        }
    });
});

function refreshDropDownEpisodes(currentId) {
    $.ajax({
        url: "/admin/lancarResultados/buscarEpisodios",
        async: false,
        type: "GET",
        success: function (data) {
            $("#dropDownEpisodes").empty();
            var episodes = JSON.parse(data);

            for (var i = 0; i < episodes.length; i++) {
                var selected = "";
                if (currentId == episodes[i].id) {
                    selected = "selected";
                }
                $("#dropDownEpisodes").append($("<option value='" + episodes[i].id + "'" + selected + ">" + episodes[i].name + "</option>"));
            }

            if (episodes.length >= 1) {
                refreshDropDownTeams($('#dropDownEpisodes').val());
            }
        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
}


function refreshDropDownTeams(episodeId, currentId) {
    $.ajax({
        url: "/admin/lancarResultados/buscarEquipes",
        async: false,
        type: "GET",
        data:
        {
            "episodeId": episodeId
        },
        success: function (data) {
            $("#dropDownTeams").empty();
            var teams = JSON.parse(data);

            for (var i = 0; i < teams.length; i++) {
                var selected = "";
                if (currentId == teams[i].id) {
                    selected = "selected";
                }
                $("#dropDownTeams").append($("<option value='" + teams[i].id + "'" + selected + ">" + teams[i].nick + "</option>"));
            }

            $.ajax({
                url: "/admin/lancarResultados/atualizarEquipe/" + $("#dropDownTeams").val(),
                async: false,
                success: function () {
                    $('#launchResultsDataTable').dataTable().fnDestroy();
                    loadLaunchResultsDataTable();
                }
            });
            
            if (teams.length < 1) {
                $("#dropDownTeams").append("<option value=''>Vazio</option>");
            }

            $('#archiveButton').attr('href', '/admin/lancarResultados/cadastrarResultadoArquivo/' + $("#dropDownTeams").val());
        },
        error: function () {
            $("#dropDownTeams").empty();
        }
    });
}

function submitNewResult() {
    var formData = new FormData($('#formNewResult')[0]);
    $.ajax({
        url: "/admin/lancarResultados/salvar/",
        type: "POST",
        data: formData,
        async: true,
        processData: false,
        contentType: false,
        cache: false,
        dataType: "json",
        success: function (data) {
            alertMessage("Resultado adicionado com sucesso.", "success");
        },
        error: function () {
            alertMessage("Não foi possivel adicionar esse resultado.", "danger");
        }
    });
}

$(document).ready(function () {
    refreshDropDownEpisodes("");
    $.ajax({
        url: "/admin/lancarResultados/atualizarEquipe/" + $("#dropDownTeams").val(),
        async: false,
        success: function () {
            $('#launchResultsDataTable').dataTable().fnDestroy();
            loadLaunchResultsDataTable();
        }
    });
});


function SubmitArchive() {
    var formData = new FormData($('#ArchiveForm')[0]);
    alertMessage("Resultados lançados, aguarde...", "success");
    $.ajax({
        url: "/admin/lancarResultados/salvarResultadoArquivo",
        type: "POST",
        data: formData,
        async: true,
        processData: false,
        contentType: false,
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data.Success == true) {
                alertMessage("Resultados adicionados com sucesso.", "success");
            }
            else {
                alertMessage(data.Exception, "danger");
            }
        },
        error: function () {
            alertMessage("Não foi possivel adicionar os resultados.", "danger");
        }
    });
};


function onSuccessSaveResultArchive(data) {
    verifyErrors();
}

function onFailureSaveResultArchive(data) {

}
