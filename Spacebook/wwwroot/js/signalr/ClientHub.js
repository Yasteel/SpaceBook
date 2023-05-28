var connection = new signalR.HubConnectionBuilder().withUrl("/ConnectionHub").build();

$(document).ready(function () {

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
        var message = $("#messageInput").val();
        var user = $("#userId").val();
        var toUser = "";

        $("#messagesList").append(`<li>${user}: ${message}</li>`);

        //connection.invoke("SendPrivateMessage", user, message, toUser)
        //.catch((err) => {
        //    return console.error(err.toString());
        //});


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
                $(".messages").html("");

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
    console.log(messages);
}

