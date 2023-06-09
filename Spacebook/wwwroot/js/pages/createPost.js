﻿const profileIdSet = new Set();
let form = document.getElementById('createPostForm');
const url = new URL(form.action);

document.getElementById('createPostForm').addEventListener('submit', function (e) {
    e.preventDefault();

    // Get form values
    let Caption = document.getElementById('Caption').value;
    let ImageFile = document.getElementById('ImageFile').files[0];
    let VideoFile = document.getElementById('VideoFile').files[0];
    let AccessLevel = document.getElementById('AccessLevel').value;
    let SharedIDs = getSelectedProfileIds();

    // Perform form validation
    if (Caption.trim() === '') {
        showError("Caption is required");
        return false;
    }

    // Create FormData object and append form data
    var formData = new FormData();
    formData.append('Caption', Caption);
    formData.append('ImageFile', ImageFile);
    formData.append('VideoFile', VideoFile);
    formData.append('AccessLevel', AccessLevel);
    formData.append('SharedIDs', SharedIDs);

    // Send data to server using fetch
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(function (response) {
            if (response.ok) {
                alert('Post created successfully!');
                reset();
            } else {
                throw new Error('Error: ' + response.status);
                reset();
            }
        })
        .catch(function (error) {
            reset();
            alert('An error occurred: ' + error.message);
            console.log(error);
        });
});


function handleImageUpload() {
    //don't allow video and images together
    document.getElementById("videoBox").style.display = "none";
    document.getElementById('VideoFile').value = "";

    

    var image = document.getElementById("ImageFile").files[0];

    //validate correct filetype and size
    let size = image.size;
    let type = image.type;

    if (size / ((1024 * 1024)) > 20) {
        showError("File size too large to upload");
        return false;
    }
    else if (!type.includes("image"))
    {
        showError("Incorrect file type selected");
        return false;
    }
    
    document.getElementById("mediaColumn").style.display = "block";
    document.getElementById("imageBox").style.display = "block";
    var reader = new FileReader();

    reader.onload = function (e) {
        document.getElementById("displayImage").src = e.target.result;
    }

    reader.readAsDataURL(image);
}

function handleVideoUpload() {
    //don't allow video and images together
    document.getElementById("imageBox").style.display = "none";
    document.getElementById('ImageFile').value = "";

    var video = document.getElementById("VideoFile").files[0];

    //validate correct filetype and size
    let size = video.size;
    let type = video.type;

    if (size / ((1024 * 1024)) > 20) {
        showError("File size too large to upload");
        return false;
    }
    else if (!type.includes("video")) {
        showError("Incorrect file type selected");
        return false;
    }
    
    document.getElementById("mediaColumn").style.display = "block";
    document.getElementById("videoBox").style.display = "block";
    var reader = new FileReader();

    reader.onload = function (e) {
        document.getElementById("displayVideo").src = e.target.result;
    }

    reader.readAsDataURL(video);
}

function getProfiles() {
    fetch("ProfileApi/Get", {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => displayData(data))
        .catch(function (error) {
            reset();
            alert('cannot fetch all users');
            console.log(error);
        });
}

function displayData(data) {
    var selectBox = document.getElementById("shareBox");
    selectBox.innerHTML = "";
    data.forEach(item => {
        selectBox.options.add( new Option(item.Email, item.UserId) );
    });
}

function saveSelectedChoices() {
    var selectedEmails = document.getElementById("selected-emails");

    let option = document.getElementById("shareBox");
    profileIdSet.add(option.value);
    console.log(option.text);

        if (!profileIdSet.has(option.value))
        {
            var email = option.text;
            var span = document.createElement('span');
            span.textContent = email + "&nbsp";
            selectedEmails.appendChild(span);
        }
    

}

function getSelectedProfileIds()
{
    let IdString = Array.from(profileIdSet).join(',');
    return IdString;
}

function accessLevelChanged(value)
{
    var sharebox = document.getElementById("shareBox");
    
    if (value == "private")
    {
        getProfiles();
        document.getElementById("dx-sharebox").style.display = "block";
    }
    else
    {
        document.getElementById("dx-sharebox").style.display = "none";
    }
}

function reset()
{
    $('#uploadMediaModal').modal('hide');
    location.reload(true);
}

function showError(errorMessages)
{
    let errorLabel = document.getElementById("errorField");
    errorLabel.innerText = errorMessages;
    errorLabel.style.display = "block";
}

function clearErrorMessage()
{
    let errorLabel = document.getElementById("errorField");
    errorLabel.innerText = "error";
    errorLabel.style.display = "none";
}