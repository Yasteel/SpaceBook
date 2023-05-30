function uploadImage(event) {
    event.preventDefault();
    console.log("We made it here")

    let ProfilePictureFile = document.getElementById('ProfilePictureFile').files[0];

    var formData = new FormData();
    formData.append('ProfilePictureFile', ProfilePictureFile);

    fetch("https://localhost:7232/api/ProfileWebApi/Upload", {
        method: 'POST',
        body: formData
    })
    .then(function (response) {
        if (response.ok) {
            alert('Profile picture changed successfully!');
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
}

function handleImageUpload() {
    var image = document.getElementById("ProfilePictureFile").files[0];

    let size = image.size;
    let type = image.type;

    if (size / ((1024 * 1024)) > 20) {
        showError("File size too large to upload");
        return false;
    }
    else if (!type.includes("image")) {
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

function reset() {
    $('#uploadMediaModal').modal('hide');
    location.reload(true);
}

function showError(errorMessages) {
    let errorLabel = document.getElementById("errorField");
    errorLabel.innerText = errorMessages;
    errorLabel.style.display = "block";
}

function clearErrorMessage() {
    let errorLabel = document.getElementById("errorField");
    errorLabel.innerText = "error";
    errorLabel.style.display = "none";
}