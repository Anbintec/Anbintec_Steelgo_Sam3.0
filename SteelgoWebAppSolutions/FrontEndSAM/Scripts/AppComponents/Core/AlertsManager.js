/********************************************/
/********************************************/
/*********                          *********/
/*********     Alerts Manager       *********/
/*********                          *********/
/********************************************/
/********************************************/

/****************************/
/*    Global Variables      */
/****************************/

//Add all global variables for your partial view here
var alertTimeOut = 60000;//15000; Temporary changed

/****************************/
/*    Document Ready        */
/****************************/

//Method to be called on the document ready and contains all the pertinent code for a partial view
function alertsManagertoBeExecutedOnDocumentReady() {
    if (Cookies.get("messageBody") != null && Cookies.get("messageComplement") != null && Cookies.get("messageType") != null) {
        //displayMessage(Cookies.get("messageBody"), Cookies.get("messageComplement"), Cookies.get("messageType"))
    }
}

/****************************/
/*    Global Functions      */
/****************************/

//Method to remove all display classes of the message area
function cleanDisplayMessage() {
    $(".alert-container").removeClass("active");
    $(".message").text("");
    $(".message").removeClass("success");
    $(".message").removeClass("warning");
    $(".message").removeClass("error");
    //Cookies.remove("messageBody", { path: '/' });
    //Cookies.remove("messageComplement", { path: '/' });
    //Cookies.remove("messageType", { path: '/' });
}

//Method to display a message on the proper area Types(o:success | 1:warning | 2:error)
function displayMessage(message, messageComplement, type) {

    cleanDisplayMessage();

    $(".alert-container").addClass("active");

    var messageText = "";
    if (message.length > 0) {
        messageText = _dictionary[message][$("#language").data("kendoDropDownList").value()];
    }

    messageText = messageText + messageComplement;

    $(".message").text(messageText);
    //console.log(messageText);
    //Cookies.set("messageBody", message, { path: '/' });
    //Cookies.set("messageComplement", messageComplement, { path: '/' });
    //Cookies.set("messageType", type, { path: '/' });

    switch (type) {
        case '0': $(".message").addClass("success"); break;
        case '1': $(".message").addClass("warning"); break;
        case '2': $(".message").addClass("error"); break;
    }

    setTimeout(function () {
        cleanDisplayMessage();
    }, alertTimeOut);
}

//Method to display a message when the user inserts an invalid username or password
function displayError(message, messageComplement, type) {
    cleanDisplayMessage();

    $(".NotAuthenticated").removeClass("hidden");
    $(".NotAuthenticated").addClass("active");
    $(".NotAuthenticated").addClass("message");

    var messageText = "";
    if (message.length > 0) {
        messageText = _dictionary[message][$("#language").data("kendoDropDownList").value()];
    }

    messageText = messageText + messageComplement;

    $(".message").text(messageText);
    $(".message").addClass("error");

    setTimeout(function () {
        cleanDisplayMessage();
    }, alertTimeOut);
}

function displayNotify(message, extraMessage, type) {
    var messageNotify = "";

    if (message.length > 0) {
        messageNotify = _dictionary[message][$("#language").data("kendoDropDownList").value()];
    }
    messageNotify = messageNotify + extraMessage;
    var options = {

        text: messageNotify,
        styling: 'brighttheme', // brighttheme, bootstrap3, jqueryui
        hide: true,
        buttons: {
            closer: false,
            sticker: false
        },
        animate: {
            animate: true,
            in_class: 'slideInDown',
            out_class: 'slideOutUp'
        },
        delay: 5000,
    };

    switch (type) {
        case '2':
            options.type = "error";
            options.styling = "bootstrap3";
            options.title = _dictionary.ErrorTitle[$("#language").data("kendoDropDownList").value()];
            break;
        case '1':
            options.title = _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()];
            break;
        case '0':
            options.type = "success";
            options.title = _dictionary.SuccessTitle[$("#language").data("kendoDropDownList").value()];
            break;

    }
    var notice = new PNotify(options);
    notice.get().click(function () {
        notice.remove();
    });

}