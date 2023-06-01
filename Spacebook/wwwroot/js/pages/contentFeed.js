$(document).ready(() => {
    $.ajax({
        url: "/ContentFeedWebApi/Get",
        method: "GET",
        success: (response) => {
            showContentFeed(JSON.parse(response));
        },
        error: (xhr, textStatus, errorThrown) => {
            console.log(errorThrown);
        }
    });

    // Event Handlers

    $(document).on("click", ".fa.fa-thumbs-up", (e) => {
        var liked = $(e.target).attr("data-liked");
        var postId = $(e.target).closest("#contentCard").attr("data-postId");

        $.ajax({
            url: `/LikeWebApi/${liked == "true" ? "Unlike" : "Like"}`,
            method: "POST",
            data: {
                postId: parseInt(postId)
            },
            success: (response) => {
                updatePostLikeCount(postId);
                $(e.target).attr("data-liked", liked == "true" ? "false" : "true");
            },
            error: (xhr, textStatus, errorThrown) => {
                console.log(errorThrown);
            }
        });
    });

    $(document).on("click", ".far.fa-comment", (e) => {

        // close all other comment inputs
        $(".comment").html("");


        var commentElement = $(e.target).closest("#contentCard").find(".comment");
        var visible = $(commentElement).attr("data-visible");

        if (visible == "false") {

            $(commentElement).append(
                `
            <textarea id="userComment"></textarea>
            <button class="post-comment">Post Comment</button>
            `
            );
        }

        $(commentElement).attr("data-visible", visible == "true" ? "false" : "true");
    });

    $(document).on("click", "button.post-comment", (e) => {
        var comment = $("#userComment").val();
        var postId = $(e.target).closest("#contentCard").attr("data-postId");

        $.ajax({
            url: "/CommentWebApi/MakeComment",
            method: "POST",
            data: {
                originalPostId: parseInt(postId),
                comment: comment
            },
            success: (response) => {
                updateCommentCount(postId);
            },
            error: (xhr, textStatus, errorThrown) => {
                console.log(errorThrown);
            }
        });
    });

    $(document).on("click", ".show-comments", (e) => {
        var postId = $(e.target).closest("#contentCard").attr("data-postId");

        $.ajax({
            url: "/CommentWebApi/GetCommentsForPost",
            method: "GET",
            data: {
                originalPostId: parseInt(postId)
            },
            success: (response) => {
                showComments(postId, JSON.parse(response));
            },
            error: (xhr, textStatus, errorThrown) => {
                console.log(errorThrown);
            }
        });
    });

    // For Modal
    $(document).on("click", ".far.fa-paper-plane", (e) => {
        var postId = $(e.target).closest("#contentCard").attr("data-postId");
        showContacts(postId);
    });

    $(document).on("click", ".send-post-modal button.cancel", (e) => {
        $(".send-post-modal").attr("data-visible", "false");
        $(".send-post-modal .body").html("");
    });

    $(document).on("click", ".fa-solid.fa-x", (e) => {
        $(".send-post-modal button.cancel").click();
    });
    
    $(document).on("click", "button.share", (e) => {
        var username = $(e.target).attr("data-username");
        var postId = $(e.target).closest(".send-post-modal").attr("data-postId");

        $.ajax({
            url: "/MessageWebApi/SharePost",
            method: "POST",
            data: {
                username: username,
                postId: JSON.parse(postId)
            },
            success: (response) => {
                console.log("post Shared");
            },
            error: (xhr, textStatus, errorThrown) => {
                console.log(errorThrown);
            }
        });

    });

});

function showContentFeed(feed) {

    $(".main-container").html('');

    var contentFeed = "";

    $.each(feed, (i, v) => {
        console.log(v);
        contentFeed +=
            `
    <div id="contentCard" class="card" data-postId="${v.Post.PostId}">

        <div class="card-title">
            <p>
                <span>
                    <image class="profile-image" src="${v.Profile.ProfilePicture}"></image>
                </span>
                <span id="titleText">${v.Profile.Email}</span>
            </p>
            <p>${v.Post.Timestamp}</p>
        </div>

        <div id="contentBody" class="card-body">
            <div id="feedCaption">
                <textarea>${v.Post.Caption}</textarea>
            </div>
            <div id="feedMedia">`;

        if (v.Post.Type != null) {
            if (v.Post.Type.includes("image")) {
                contentFeed += `<image src="${v.Post.MediaUrl}"></image>`;
            }
            else {
                contentFeed += `<video src="${v.Post.MediaUrl}"></video>`;
            }
        }

        contentFeed += `                
            </div>
            <div class="contentCounters">
                <p>
                    <span id="likeCount">Likes &nbsp ${v.Post.LikeCount}</span>
                    <span id="commentCount">Comments &nbsp ${v.Post.CommentCount}</span>
                </p>
            </div>

            <div id="contentIcons">
                <span>
                    <i class="fa fa-thumbs-up" data-liked="${v.LikedPost}"></i>
                </span>
                <span>
                    <i class='far fa-comment'></i>
                 </span>
                 <span><i class="far fa-paper-plane"></i></span>
            </div>
            <div class="comment" data-visible="false">
                
            </div>`;

        if (v.Post.CommentCount > 0) {
            contentFeed +=
                `
                <div class="comments-container" data-close="true">
                    <span class="show-comments">Show Comments <i class="fa-solid fa-chevron-down"></i></span>
                    <div class="comments">
                    </div>
                </div>
                `;
        }


        contentFeed +=
        `
        </div>
    </div>`;

    });


    $(".main-container").append(contentFeed);
}

function updatePostLikeCount(postId) {
    $.ajax({
        url: "/LikeWebApi/GetLikeCount",
        method: "GET",
        data: {
            postId: parseInt(postId)
        },
        success: (response) => {
            $(`#contentCard[data-postId="${postId}"]`).find("span#likeCount").html(`Likes &nbsp ${response}`);
        },
        error: (xhr, textStatus, errorThrown) => {
            console.log(errorThrown);
        }
    });
}

function updateCommentCount(postId) {
    $.ajax({
        url: "/CommentWebApi/GetCommentCount",
        method: "GET",
        data: {
            postId: parseInt(postId)
        },
        success: (response) => {
            $(`#contentCard[data-postId="${postId}"]`).find("span#CommentCount").html(`Comments &nbsp ${response}`);
            $(".comment").html("");
        },
        error: (xhr, textStatus, errorThrown) => {
            console.log(errorThrown);
        }
    });
}

function showComments(postId, response) {
    $(`#contentCard[data-postId="${postId}"]`).find(".comments").html("");
    let comments = "";


    $.each(response, (i, v) => {
        comments +=
            `
                <div class="comment-box">
                    <p>${v.Profile.DisplayName}</p>
                    <p>${v.Post.Caption}</p>
                    <p>${v.Post.Timestamp}</p>
                </div>
            `;
    });

    $(`#contentCard[data-postId="${postId}"]`).find(".comments").append(comments);
}

function showContacts(postId) {
    $(".send-post-modal").attr("data-postId", postId);

    $.ajax({
        url: "/MessageWebApi/GetContacts",
        method: "GET",
        success: (response) => {
            var contacts = JSON.parse(response);
            $(".send-post-modal .body").html("");
            var contactList = "";

            $.each(contacts, (i, v) => {
                contactList +=
                    `
                    <div class="contact" >
				        <div class="information">
					        <p class="username">${v.Email}</p>
					        <p class="display-name">${v.DisplayName}</p>
				        </div>
				        <button class="share" data-username="${v.Email}">Share</button>
			        </div>
                    `;
            });

            $(".send-post-modal .body").append(contactList);
        },
        error: (xhr, textStatus, errorThrown) => {
            console.log(errorThrown);
        }
    });

    $(".send-post-modal").attr("data-visible", "true");
}

function test(searchTerm) {
    $.ajax({
        url: "/Search/SearchUsers",
        method: "GET",
        data: {
            searchTerm: searchTerm
        },
        success: (response) => {
            console.log(response);
        },
        error: (xhr, textStatus, errorThrown) => {
            console.log(errorThrown);
        }
    });
}