function loadGoalDataTable() {
    var logoPath = "";
    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    $('#goalDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/metas/search/" + $('#dropDownTeams').val(),
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
                "searchable": true
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
                    var links = "<a class='fa fa-pencil' href='/admin/metas/editar/" + data + "/" + $('#dropDownTeams').val() + "/" + $('#dropDownEpisodes').val() + "' onclick='showEntityModal(this);return false;' title='Editar metas.'></a>";
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
            $('#goalDataTable').dataTable().fnDestroy();
            loadGoalDataTable();
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

            if (teams.length >= 1) {
                $.ajax({
                    url: "/admin/lancarResultados/atualizarEquipe/" + $("#dropDownTeams").val(),
                    async: false,
                    success: function () {
                        $('#goalDataTable').dataTable().fnDestroy();
                        loadGoalDataTable();
                    }
                });
            }
            else {
                $("#dropDownTeams").append("<option value='empty'>Vazio</option>");
            }
        },
        error: function () {
            $("#dropDownTeams").empty();
        }
    });
}


$(document).ready(function () {
    refreshDropDownEpisodes();
});

