<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.7/signalr.min.js"></script>

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (message) {
    var notificationArea = document.getElementById("notificationArea");
    var notification = document.createElement("p");
    notification.textContent = message;
    notificationArea.appendChild(notification);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

