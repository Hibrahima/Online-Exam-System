var culture = $("#routeDataId").val();
var answer;
var trueQuestion = 0;
var falseQuestion = 0;
var score = 0;
var time;
var questCount = 0;
var questionList;
var currentQuestionIndex = 0;
var quizTime = 0;
var devamButtonClickCount = 0;

function populateQuestNumberDropDown() {
    var option = '';

    $.getJSON("/" + culture + "/Exam/GetJsonData", {
        "categoryNames": $("#categoryNames").val(),
        "quizLevels": $("#quizLevels").val(),
        "langNames": $("#langNames").val()
    }, function (data) {
        for (var i = 0; i < data.length; i++) {
            option += '<option value="' + data[i] + '">' + data[i] + '</option>';
        }
        $("#quizQuestNumber").html(option);
    });
}


$(".select_change").click(function () {
    populateQuestNumberDropDown();
});

$("#quiz_start_button").click(function () {
    $(".hide_show_me").show();
    $("#quiz_start_button").prop('disabled', true);
    $(".select_change").prop('disabled', true);

    $.getJSON("/" + culture + "/Exam/SendData", {
        "quizQuestNumber": $("#quizQuestNumber").val(),
        "categoryNames": $("#categoryNames").val(),
        "quizLevels": $("#quizLevels").val(),
        "langNames": $("#langNames").val()
    }, function (data) {
        questionList = data;
        $("#write_quest_text").html(data[currentQuestionIndex].questionLabel);
        $("#write_quest_points").html("( " + data[currentQuestionIndex].points + " pts)")
        $("#choice1").get(0).nextSibling.data = data[currentQuestionIndex].choiceA;
        $("#choice1").val(data[currentQuestionIndex].choiceA);
        $("#choice2").get(0).nextSibling.data = data[currentQuestionIndex].choiceB;
        $("#choice2").val(data[currentQuestionIndex].choiceB);
        $("#choice3").get(0).nextSibling.data = data[currentQuestionIndex].choiceC;
        $("#choice3").val(data[currentQuestionIndex].choiceC);
        $("#choice4").get(0).nextSibling.data = data[currentQuestionIndex].choiceD;
        $("#choice4").val(data[currentQuestionIndex].choiceD);
        for (var i = 0; i < data.length; i++)
            quizTime += data[i].time;

        $("#exam_time").val(quizTime);
        time = $("#exam_time").val();
        var executeTimer = setInterval(myTimer, 1100);
        function myTimer() {
          
            if (time > 0) {
                time--;
                $("#exam_time").val(time);
            }
            if (time == 0) {
                 clearInterval(executeTimer);
                   done();
            }
            if (questionList.length == devamButtonClickCount) {
                clearInterval(executeTimer);
                done();
            }
        }

    });


});


function done() {
    alert("Quiz is finished");
    $.get("/" + culture + "/Exam/Terminate", 
        {
            "score": $("#score").val(),
            "questNumber": $("#quizQuestNumber").val(),
            "trueQuestionNumber": $("#true_quest").val(),
            "falseQuestionNumber": $("#false_quest").val()
        }, function (result) {
            window.location.href = "/"+culture+"/"+result;

    });
}
//Next Question button action go to controller /Exam/NestQuestion and get current question
$("#next_question_button").click(function () {
    devamButtonClickCount++;

    answer = $("input[name='optradio']:checked").val();
    $("#choice1").prop("checked", false);
    $("#choice2").prop("checked", false);
    $("#choice3").prop("checked", false);
    $("#choice4").prop("checked", false);
    
    if (answer === questionList[currentQuestionIndex].answer) {
        trueQuestion++; score += questionList[currentQuestionIndex].points;
        $("#true_quest").val(trueQuestion);
        $("#score").val(score);
    }
    else {
        falseQuestion++;
        $("#false_quest").val(falseQuestion);
        $("#score").val(score);
    }
  
    if (currentQuestionIndex < questionList.length - 1)
        currentQuestionIndex++;

    $("#write_quest_text").html(questionList[currentQuestionIndex].questionLabel);
    $("#write_quest_points").html("( " + questionList[currentQuestionIndex].points + " pts)")
    $("#choice1").get(0).nextSibling.data = questionList[currentQuestionIndex].choiceA;
    $("#choice1").val(questionList[currentQuestionIndex].choiceA);
    $("#choice2").get(0).nextSibling.data = questionList[currentQuestionIndex].choiceB;
    $("#choice2").val(questionList[currentQuestionIndex].choiceB);
    $("#choice3").get(0).nextSibling.data = questionList[currentQuestionIndex].choiceC;
    $("#choice3").val(questionList[currentQuestionIndex].choiceC);
    $("#choice4").get(0).nextSibling.data = questionList[currentQuestionIndex].choiceD;
    $("#choice4").val(questionList[currentQuestionIndex].choiceD);

  
    answer = $("input[name='optradio']:checked").val();
       
   
});

$(document).ready(function () {
    $(".hide_show_me").hide();
    $("#true_quest").val(trueQuestion);
    $("#score").val(score);
    $("#false_quest").val(falseQuestion);
 });