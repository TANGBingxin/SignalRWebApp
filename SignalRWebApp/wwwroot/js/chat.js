"use strict"
//connect service
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


//Disabled send button until connection is established
$("#sendMessage").hide();

//start connection
connection.start().then(function () {
    $("#sendMessage").show();
    console.log("cccc");
}).catch(function (err) {
    return console.error(err.toString());
});

//send message by invoking SendMessage of Chathub
$("#sendMessage").click(function () {
    var user = $("#userInput").val();
    var message = $("#messageInput").text();
    connection.invoke("SendMessage", user, message).catch(function (err){
        return console.error(err.toString());
    })
});

//receive message from Chathub
connection.on("ReceiveMessage", function (user, message, time) {
    $("#content").append(`<p>${user} ${time}</p><p> ${message} </p><br>`);
    $("#content").animate({ scrollTop: 100000 });
});


//search chat history
var pageIndex = 1;
$("#findMessage").click(function () {
    $("#historyMessage").fadeIn();
    $.post("/Home/GetMessage", { pageIndex: pageIndex, pageSize: 10 }, function (data) {
        $.each(data, function (i, e) {
            $("#historyMessage").append(`<p>${e.userName} ${e.createTime}</p><p> ${e.content} </p><br>`);
        });
    })
});