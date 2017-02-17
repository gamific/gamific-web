function loadMessages(teamId) {
    $.ajax(
    {
        type: 'GET',
        url: '/public/chat/buscarMensagens/' + teamId,
        dataType: 'html',
        cache: false,
        async: true,
        success: function (data) {
            $('#messagesPartialView').empty();
            $('#messagesPartialView').html(data);
        }
    });
}

function SendMessage() {
    $.ajax(
    {
        type: 'POST',
        url: '/public/chat/enviarMensagem/',
        data:
        {
            "teamId": $('#teamId').val(),
            "message" : $('#message').val()
        },
        async: true,
        success: function (data) {
            var logoPath = "";
            $.ajax({
                url: window.location.origin + "/apiMedia/imagePath",
                async: false,
                type: "GET",
                success: function (data) {
                    logoPath = data;
                }
            });

            var messageInformation = JSON.parse(data);
            $("#chat-content").append("<ul class='chat-list'><li class='message receive'><div class='media'><div class='pull-left user-avatar'><img class='media-object img-circle' src='" + logoPath + messageInformation.SenderLogoId + "'></div><div class='media-body'><p class='media-heading'> <span>" + messageInformation.SenderName + "</span> <span class='time'>" + messageInformation.SendDateTime + "</span></p>" + messageInformation.Message + "</div></div></li></ul>");
            $('#message').val("");
        }
    });
}




