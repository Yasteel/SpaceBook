var connection = new signalR.HubConnectionBuilder().withUrl("/ConnectionHub").build();

connection.start()
.then(() => {
    //document.getElementById("#sendButton").disabled = false;
})
.catch((err) => {
    return console.error(err.toString());
});


connection.on("ReceiveMessage", (user, message) => {
    $("#messagesList").append(`<li>${user}: ${message}</li>`);


    $.ajax({
        url: "/MessageWebApi/NewMessage",
        type: "POST",
        data: {
            "conversationId": conversationId,
            "messageType": "Text",
            "content": message,
            "senderId": user
        },
        success: (response) => {
            console.log("it worked")
        },
        error: (xhr, textStatus, errorThrown) => {
            console.log(`Err: ${errorThrown}`);
        }
    });
    
});



$(document).on("click", "#sendButton", (e) => {
    //var user = $("#userInput").val();
    var message = $("#messageInput").val();
    //var toUser = $("#toUser").val();

    var user = "";
    var toUser = "";

    $("#messagesList").append(`<li>${user}: ${message}</li>`);

    connection.invoke("SendPrivateMessage", user, message, toUser)
    .catch((err) => {
        return console.error(err.toString());
    });


    $("#messageInput").val("");
    e.preventDefault();
});


