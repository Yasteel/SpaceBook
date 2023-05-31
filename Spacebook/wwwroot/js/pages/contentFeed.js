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

    $(document).on("click", ".fa.fa-thumbs-up", function (e) {
        var liked = $(e.target).attr("data-liked");
        var postId = $(e.target).closest("#contentCard").attr("data-postId");


        
        console.log(postId);

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

});

function showContentFeed(feed) {
    
    $("main.container").html('');

    var contentFeed = "";

    $.each(feed, (i, v) => {
        console.log(v);
        contentFeed +=
            `
    <div id="contentCard" class="card bg-light mb-3" data-postId="${v.Post.PostId}">

        <div class="card-header">
              
            <p>${v.Profile.Email}</p>
            <p>${v.Post.Timestamp}</p>
        </div>
        <div class="card-body">
            <div id="feedCaption">
                <textarea>${v.Post.Caption}</textarea>
            </div>
            <div id="feedMedia">`;

        if (v.Post.Type != null) {
            if (v.Post.Type.Contains("image")) {
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
                 <span><i class="fa-regular fa-paper-plane"></i></span>
            </div>
        </div>
    </div>`;

    });


    $("main.container").append(contentFeed);
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
