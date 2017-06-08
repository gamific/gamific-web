
function initQuiz() {
    var listTo;
    $.ajax({
        type: "POST",
        url: "/admin/quiz/complete/" + idPrincipal,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, item) {
            monta(data)
        },
        error: function (data) {
            debugger;
            console.log(data);
        }
    });
}


function monta(listAll) {
    var htmlTotal = '';
    $.each(listAll, function (index, item) {
        htmlTotal = htmlTotal + montaHtmlPergunta(item.QuestionEntity.Question, item.QuestionEntity.Required);
        htmlTotal = htmlTotal + "<ul class='ss-choices ss-choices-required' role='group'> ";
        $.each(item.AnswersEntity, function (indexS, itemS) {
            htmlTotal = htmlTotal + "<li class='ss-choice-item'><label> <span class='ss-choice-item-control goog-inline-block'>";
            htmlTotal = htmlTotal + montaHtmlResposta(itemS.Answer, item.QuizEntity.IsMultiple);
        })
        htmlTotal = htmlTotal + "</ul>";
    })
    debugger;
    $('#replaceQuiz').html(htmlTotal);
}


function montaHtmlPergunta(texto, obrigatoria) {

    var htmlPergunta = '';
    debugger;
    if (!obrigatoria) {
        htmlPergunta = "<label class='ss-q-item-label'> <div class='ss-q-title'>" + texto + "</div></label>";
    } else {
        htmlPergunta = "<label class='ss-q-item-label'> <div class='ss-q-title'>(*)" + texto + "</div></label>";
    }
    return htmlPergunta;
}

function montaHtmlResposta(texto, multipla) {
    var htmlResposta = '';
    debugger;
    if (multipla) {
        htmlResposta = htmlResposta + "<input type='checkbox' name='entry.1000005' value='" + texto + "' role='checkbox' class='ss-q-checkbox' aria-required='true'></span>" +
            "<span class='ss-choice-label'>" + texto + "</span></label></li>";
    } else {
        htmlResposta = htmlResposta + "<input type='radio' name='entry.1000005' value='" + texto + "' role='checkbox' class='ss-q-checkbox' aria-required='true'></span>" +
            "<span class='ss-choice-label'>" + texto + "</span></label></li>";
    }
    return htmlResposta;
}

initQuiz();
