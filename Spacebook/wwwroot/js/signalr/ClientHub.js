var connection = new signalR.HubConnectionBuilder().withUrl("/ConnectionHub").build();

$(document).ready(function () {

    connection.start()
        .then(() => {
            //document.getElementById("#sendButton").disabled = false;
        })
        .catch((err) => {
            return console.error(err.toString());
        });


    connection.on("ReceiveMessage", (conversationId, senderId, messageType, content) => {
        $("#messages-list").append(`<li class="from">${content}</li>`);


        $.ajax({
            url: "/MessageWebApi/NewMessage",
            type: "POST",
            data: {
                "conversationId": conversationId,
                "senderId": senderId,
                "messageType": messageType,
                "content": content
                
            },
            success: (response) => {
                console.log("Message Sent");
            },
            error: (xhr, textStatus, errorThrown) => {
                console.log(`Err: ${errorThrown}`);
            }
        });
    });

    $.ajax({
        url: "/MessageWebApi/GetContacts",
        type: "GET",
        success: (response) => {
            showContacts(JSON.parse(response));
        },
        error: (xhr, textStatus, err) => {
            console.log(`Err: ${errorThrown}`);
        }
    });


    $(document).on("click", "#sendButton", (e) => {
        var conversationId = parseInt($(".message-container").attr("data-conversation-id"));
        var senderUsername = $("input#username").val();
        var messageType = "";
        var content = $("#messageInput").val();

        if (content != null) {
            messageType = "Text";
        }

        console.log({
            "conversationId": conversationId,
            "senderUsername": senderUsername,
            "messageType": messageType,
            "content": content
        });

        $("#messages-list").append(`<li class="to">${content}</li>`);

        connection.invoke("SendMessage", conversationId, senderUsername, messageType, content)
        .catch((err) => {
            return console.error(err.toString());
        });


        $("#messageInput").val("");
        e.preventDefault();
    });

    $(document).on("click", "div.contact", (e) => {

        // contactUsername is the username/email of the contact you click on
        var contactUsername = $(e.target).closest(".contact").attr("data-username");

        console.log(contactUsername);

        $.ajax({
            url: "/MessageWebApi/GetConversationId",
            type: "GET",
            data: {
                "contactUsername": contactUsername
            },
            success: (response) => {

                $(".message-container").attr("data-conversation-id", response);
                $("#messages-list").html("");

                $.ajax({
                    url: "/MessageWebApi/GetMessages",
                    type: "GET",
                    data: {
                        "conversationId": response
                    },
                    success: (response) => {
                        showMessages(JSON.parse(response));
                    },
                    error: (xhr, textStatus, errorThrown) => {
                        console.log(`Err1: ${errorThrown}`);
                    }
                });

                showMessages(JSON.parse(response));
            },
            error: (xhr, textStatus, errorThrown) => {
                console.log(`Err2: ${errorThrown}`);
            }
        });

    });
});


function showContacts(contacts) {

    $(".contact-list").html('');
    let contactList = "";
    $.each(contacts, (i, v) => {
        contactList +=
            `
            <div class="contact" data-username="${v.Username}">
				<div class="details">
					<p class="name">${v.Name} ${v.Surname}</p>
					<p class="username">${v.Username.split("@")[0]}</p>
				</div>
			</div>
            `;
    });

    $(".contact-list").append(contactList);
}

function showMessages(messages) {
    $("#messages-list").html("");

    if (messages.length > 0) {
        var messageList = "";

        $.each(messages, (i, v) => {
            messageList += `<li class="${v.SenderId == currentUser ? "to" : "from"}">`;

            switch (v.MessageType) {
                case "Text":
                    messageList += v.Content;
                    break;

                case "Image":
                    messageList += `<img src="${v.Content}" alt="Image Not Found" />`;
                    break;

                case "Video":
                    messageList += `<video controls> <source src="${v.Content}"></source> </video>`;
                    break;

                case "Post":
                    // Need to find a way to display a Post
                    //messageList += `${v.Content}`;
                    break;
            }

            messageList += `</li>`
        });

        $("#messages-list").append(messageList);
    }
}

