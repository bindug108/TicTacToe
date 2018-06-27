//import { setInterval } from "timers";

//import { clearInterval } from "timers";
var interval;

//function CheckEmailConfirmationStatus(email) {
//    $.get("/CheckEmailConfirmationStatus?email=" + email,
//        function (data) {
//            if (data === "OK") {
//                if (interval !== null)
//                    clearInterval(interval);

//                alert("ok");
//            }
//        });
//}

//function EmailConfirmation(email) {
//    interval = setInterval(() => {
//        CheckEmailConfirmationStatus(email);
//    }, 5000);
//}

function EmailConfirmation(email) {
    if (window.WebSocket) {
        alert("Websockets are enabled");
        openSocket(email, "Email");
    }
    else {
        alert("Websockets are not enabled");
        interval = setInterval(() => {
            CheckEmailConfirmationStatus(email);
        }, 5000);
    }
}

function GameInvitationConfirmation(id) {
    if (window.WebSocket) {
        alert("Websockets are enabled");
        openSocket(id, "GameInvitation");
    }
    else {
        alert("Websockets are not enabled");
        interval = setInterval(() => {
            CheckGameInvitationConfirmationStatus(id)
        }, 5000);
    }
}