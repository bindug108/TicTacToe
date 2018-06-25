//import { clearInterval } from "timers";

function CheckEmailConfirmationStatus(email) {
    $.get("/CheckEmailConfirmationStatus?email=" + email,
        function (data) {
            if (data === "OK") {
                if (interval !== null)
                    clearInterval(interval);

                //alert("ok");
                window.location.href = "/GameInvitation?email=" + email;
            }
        });
}