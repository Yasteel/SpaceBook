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
            // Response: Profile
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

        $(".contact").removeClass("active");
        $(e.target).closest(".contact").addClass("active");


        // contactUsername is the username/email of the contact you click on
        var contactUsername = $(e.target).closest(".contact").attr("data-username");

        $.ajax({
            url: "/MessageWebApi/GetConversationId",
            type: "GET",
            data: {
                "contactUsername": contactUsername
            },
            success: (conversationId) => {

                $(".message-container").attr("data-conversation-id", conversationId);
                $("#messages-list").html("");

                $.ajax({
                    url: "/MessageWebApi/GetMessages",
                    type: "GET",
                    data: {
                        "conversationId": conversationId
                    },
                    success: (response) => {
                        showMessages(JSON.parse(response));
                        console.log(response);
                    },
                    error: (xhr, textStatus, errorThrown) => {
                        console.log(`Err1: ${errorThrown}`);
                    }
                });
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
            <div class="contact" data-username="${v.Email}">
				<div class="details">
					<p class="name">${v.DisplayName}</p>
					<p class="username">${v.Email.split("@")[0]}</p>
				</div>
			</div>
            `;
    });

    $(".contact-list").append(contactList);
}

function showMessages(messageProperties) {
    $("#messages-list").html("");

    if (messageProperties.length > 0) {
        var messageList = "";

        $.each(messageProperties, (i, v) => {
            messageList += `<li class="${v.SenderId == currentUser ? "to" : "from"}">`;

            switch (v.Message.MessageType) {
                case "Text":
                    messageList += v.Message.Content;
                    break;

                case "Image":
                    messageList += `<img src="${v.Message.Content}" alt="Image Not Found" />`;
                    break;

                case "Video":
                    messageList += `<video controls> <source src="${v.Message.Content}"></source> </video>`;
                    break;

                case "Post":

                    messageList += 
                    `<div class="message-post">
                        <div class="post-head">
                            <div class="profile-picture"></div>
                            <div class="full-name"></div>
                        </div>
                        <div class="post-body">`;

                    switch (v.Post.Type) {
                        case "Text":
                            messageList +=
                            `<p>${v.Post.Caption}</p>
                        </div>
                    </div>`;
                            break;

                        case "Image":
                            messageList +=
                            `<img src="${v.Post.MediaUrl}" alt="Image Not Found" />
                            </div>
                        <div class="post-footer">
                            <div class="caption">
                                <p>${v.Post.Caption}</p>
                            </div>
                        </div>
                    </div>

                            `;
                            break;

                        case "Video":
                            messageList +=
                                `<video controls> 
                                    <source src="${v.Post.MediaUrl}"></source> 
                                </video>
                            </div>
                        <div class="post-footer">
                            <div class="caption">
                                <p>${v.Post.Caption}</p>
                            </div>
                        </div>
                    </div>

                            `;
                            break;
                    }
                    break;
            }

            messageList += `</li>`
        });

        $("#messages-list").append(messageList);
    }
}

