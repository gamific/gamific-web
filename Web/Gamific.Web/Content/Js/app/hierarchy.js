
function searchHierarchy(episode) {
    $("#custom-colored").empty();
    $.ajax({
        url: "/public/hierarchy/searchHierarchy",
        async: false,
        type: "GET",
        data:
        {
            "episodeId": episode
        },
        success: function (data) {
            //var chartConfig = JSON.parse(data);

            new Treant(data);
        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
}

$(document).ready(function () {
    searchHierarchy($('#EpisodeId').val());
    $.ajax({
        url: "/public/hierarchy/buscarEpisodios",
        async: false,
        type: "GET",
        success: function (data) {
            $("#dropDownEpisodes").empty();
            var episodes = JSON.parse(data);

            for (var i = 0; i < episodes.length; i++) {
                $("#dropDownEpisodes").append($("<option value='" + episodes[i].id + "'>" + episodes[i].name + "</option>"));
            }

        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
});

$('#dropDownEpisodes').change(function () {
    searchHierarchy($('#dropDownEpisodes').val());
});
