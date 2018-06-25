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

function EmailConfirmation(email) {
    interval = setInterval(() => {
        CheckEmailConfirmationStatus(email);
    }, 5000);
}