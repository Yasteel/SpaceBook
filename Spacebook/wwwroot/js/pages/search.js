
$(document).ready(function () {
    $('#searchTerm').on('input', function () {
        var searchTerm = $(this).val();


        $.ajax({
            url: '/api/Search',
            data: { 'searchTerm': searchTerm },
            success: function (results) {
                // Empty the results list
                $('#results').empty();

                // Add each result to the list
                results.forEach(function (search) {
                    $('#results').append(
                        '<li class="list-group-item">' +
                        '<h5>' + search.Type + '</h5>' +
                        `<a href="https://localhost:7232/${search.Type.charAt(0) == "@" ? "Profile/ViewProfile?email=" : "Posts/ViewPost?PostId="}${search.Text}"> View Post </a>` +
                        '</li>'
                    );
                });
            }
        });
    });
});
