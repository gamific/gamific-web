
function changeChampion(playerId) {
   // $("#demo-" + playerId).val();
    //$("#name-" + playerId).val();

    /*$("#div-hallOfFameDestac").empty();
    $.ajax({
        url: "/public/hallDaFama/search/" + playerId,
        async: true,
        type: "GET",
        success: function (data) {
            var champion = JSON.parse(data);
            showChampion = "";
            showChampion += "<div class='row'>";
            showChampion += "<div class='pull-left col-md-5 col-sm-5'>";
            showChampion += "<img class='media-object img-circle' src='" + champion.GeneralWinners[0].logoPath + "'>";
            showChampion += "</div>";
            showChampion += "<div class='col-md-7 col-sm-7' style='font-size: 22px;'>";
            showChampion += "<p id='' class='media-heading'>" + champion.GeneralWinners[0].PlayerName + "</p>";
            showChampion += "<p id='' class='media-heading'>" + champion.GeneralWinners[0].EpisodeName + "</p>";
            showChampion += "</div>";
            showChampion += "</div>";
            showChampion += "<div class='row'>";
            showChampion += "<div class='col-md-12 col-sm-12'>";
            showChampion += "<div style='font-size:100px'>";
            showChampion += "<p>Score:" + champion.GeneralWinners[0].Score + "</p>";
            showChampion += "</div>";
            showChampion += "</div>";
            showChampion += "</div>";


            $('#div-hallOfFameDestac').append(showChampion);
            $("#demo").append("html");
                    }
    });*/
    $("#div-hallOfFameDestac").empty();
    showChampion = "";
    showChampion += "<div class='row'>";
    showChampion += "<div class='pull-left col-md-5 col-sm-5'>";
    showChampion += "<img class='media-object img-circle'style='width: 180px; height: 180px; margin: 70px;' src='" + $("#logoPath-" + playerId).val() + "'>";
    showChampion += "</div>";
    showChampion += "<div class='col-md-7 col-sm-7 text-center' style='font-size: 30px; padding-top: 14%; color: white;'>";
    showChampion += "<p id='' class='media-heading'><img style='width:35px; height: 35px;' src='../Content/Img/crownG.png' /> " + $("#playerName-" + playerId).val() + " <img style='width:35px; height: 35px;' src='../Content/Img/crownG.png' /></p>";
    showChampion += "<p id='' class='media-heading'>Campanha: " + $("#episodeName-" + playerId).val() + "</p>";
    showChampion += "<p>Score:" + $("#score-" + playerId).val() + "</p>";
    showChampion += "</div>";
    showChampion += "</div>";

    /*showChampion += "<div class='row'>";
    showChampion += "<div class='col-md-12 col-sm-12'>";
    showChampion += "<div class='text-center' style='font-size:100px; color: white;' >";
    showChampion += "<p>Score:" + $("#score-" + playerId).val() + "</p>";
    showChampion += "</div>";
    showChampion += "</div>";
    showChampion += "</div>";*/


    $('#div-hallOfFameDestac').append(showChampion);
}

$(document).ready(function () {
    changeChampion($("#firstPlayer").val());
});