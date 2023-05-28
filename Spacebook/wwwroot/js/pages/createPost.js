﻿const profileIdSet = new Set();
let form = document.getElementById('createPostForm');

document.getElementById('createPostForm').addEventListener('submit', function (e) {
    e.preventDefault();

    // Get form values
    let Caption = document.getElementById('Caption').value;
    let MediaFile = document.getElementById('MediaFile').files[0];
    let AccessLevel = document.getElementById('AccessLevel').value;
    let SharedIDs = getSelectedProfileIds();

    const url = new URL(form.action);

    // Perform form validation
    if (Caption.trim() === '') {
        alert('Caption is required.');
        return;
    }

    // Create FormData object and append form data
    var formData = new FormData();
    formData.append('Caption', Caption);
    formData.append('MediaFile', MediaFile);
    formData.append('AccessLevel', AccessLevel);
    formData.append('SharedIDs', SharedIDs);

    // Send data to server using fetch
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(function (response) {
            if (response.ok) {
                // Clear form fields
                document.getElementById('Caption').value = '';
                document.getElementById('MediaFile').value = '';

                alert('Post created successfully!');
            } else {
                throw new Error('Error: ' + response.status);
            }
        })
        .catch(function (error) {
            alert('An error occurred: ' + error.message);
        });
});


function handleImageUpload() {
    var image = document.getElementById("MediaFile").files[0];
    var reader = new FileReader();

    reader.onload = function (e) {
        document.getElementById("display-image").src = e.target.result;
    }

    reader.readAsDataURL(image);
}

function getSelectedProfileIds() {
    let items = $("#ShareTagBox").dxTagBox("option", "selectedItems");

    items.forEach(item => {
        let profileID = item.UserId;
        profileIdSet.add(profileID);
    });

    let IdString = Array.from(profileIdSet).join(',');
    $('#SharedIDs').val(IdString);
    console.log(profileIdSet);

    return IdString;
}

function accessLevelChanged(value)
{
    console.log(value);
    if (value == "private")
    {
        document.getElementById("dx-sharebox").style.display = "block";
    }
    else
    {
        document.getElementById("dx-sharebox").style.display = "none";
    }
}