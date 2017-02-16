
function searchHierarchy(episode) {
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
