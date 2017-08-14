/********************************************/
/********************************************/
/*********                          *********/
/*********    Traslation Manager    *********/
/*********                          *********/
/********************************************/
/********************************************/

/****************************/
/*    Global Variables      */
/****************************/

//Add all global variables for your partial view here

/****************************/
/*    Document Ready        */
/****************************/

//Method to be called on the document ready and contains all the pertinent code for a partial view
function traslationManagerToBeExecutedOnDocumentReady() {
    //CODE
}

/****************************/
/*    Global Functions      */
/****************************/
    
//Function to change the labels to the corresponding language of the layout page
function changeLayoutLabels(language) {
    $("label").each(function (index) {
        if (validateElementExistence(_dictionary[$(this).attr("id")])
                && validateElementExistence(_dictionary[$(this).attr("id")][language])) {
            $("#" + $(this).attr("id")).text(_dictionary[$(this).attr("id")][language]);
        }
    });
}

//Function to change the spans to the corresponding language of the layout page
function changeLayoutSpans(language) {
    $("span").each(function (index) {
        if (validateElementExistence(_dictionary[$(this).attr("id")])
                && validateElementExistence(_dictionary[$(this).attr("id")][language])) {
            $("#" + $(this).attr("id")).text(_dictionary[$(this).attr("id")][language]);
        }
    });
}

//Function to change the inputs to the corresponding language of the layout page
function changeLayoutInputs(language) {
    $("input").each(function (index) {
        if (validateElementExistence(_dictionary[$(this).attr("id")])
                && validateElementExistence(_dictionary[$(this).attr("id")][language])) {
            $("#" + $(this).attr("id")).prop("value",_dictionary[$(this).attr("id")][language]);
            $("#layoutLabel0001").prop("value","").prop("placeholder",_dictionary[$(this).attr("id")][language]);
        }
    });
}

//Function to change language in the onChange trigger of the language dropdownlist
function changeLanguage() {
    var langValue = this.value();
    kendo.culture(this.value());
    var datePickerOptions = {
        format: kendo.culture().calendar.patterns.d
    };

    kendo.ui.progress($("#grid"), true);

    //if ($("#gridTubos"))
    //    kendo.ui.progress($("#gridTubos"), true);

    changeLayoutLabels(langValue);
    changeLayoutSpans(langValue);
    changeLayoutInputs(langValue);
    $.getScript("../Scripts/kendo/2014.2.716/messages/kendo.messages." + this.value() + ".js", function () {

        //if ($("#gridTubos"))
        //    kendo.ui.progress($("#gridTubos"), false);

        kendo.ui.progress($("#grid"), false);
        if (typeof changeLanguageCall == 'function') {
            changeLanguageCall();
        }
        if (typeof changeDatePickerDateFormat == 'function') {
            changeDatePickerDateFormat(datePickerOptions);
        }
    });
}

//Function to add elements to the dictionary from a string in this form "newItem°es°en|newItem2°es°en"
function addElementToDictionary(sParam) {
    var elementsToAdd = sParam.split("|");
    for (var i = 0; i < elementsToAdd.length; i++) {
        var partsOfElement = elementsToAdd[i].split("°");
        _dictionary[partsOfElement[0]] = {};
        _dictionary[partsOfElement[0]]["es-MX"] = partsOfElement[1];
        _dictionary[partsOfElement[0]]["en-US"] = partsOfElement[2];
    }
}

//Function to set the right localization ot KendoUpload
function getKendoUploadLocalization(val) {
    return {
        cancel: _dictionary.KendoUploadTxt0001[val],
        dropFilesHere: _dictionary.KendoUploadTxt0002[val],
        headerStatusUploaded: _dictionary.KendoUploadTxt0003[val],
        headerStatusUploading: _dictionary.KendoUploadTxt0004[val],
        remove: _dictionary.KendoUploadTxt0005[val],
        retry: _dictionary.KendoUploadTxt0006[val],
        select: _dictionary.KendoUploadTxt0007[val],
        statusFailed: _dictionary.KendoUploadTxt0008[val],
        statusUploaded: _dictionary.KendoUploadTxt0009[val],
        statusUploading: _dictionary.KendoUploadTxt0010[val]
    }
}

function getKendoGridFilterable(val) {
    return {
        extra: false,
        mode: "menu",
        operators: {
            string: {
                startswith: _dictionary.KendoGridFilterable0001[val],
                eq: _dictionary.KendoGridFilterable0002[val],
                neq: _dictionary.KendoGridFilterable0003[val],
            }
        },
        cell: {
            showOperators: false,
            operator: "contains"
        }
    }
}

//function getKendoGridFilterable(val) {
//    return {
//        extra: false,
//        operators: {
//            string: {
//                startswith: _dictionary.KendoGridFilterable0001[val],
//                eq: _dictionary.KendoGridFilterable0002[val],
//                neq: _dictionary.KendoGridFilterable0003[val],
//            }
//        }
//    }
//}

function getKendoGridFilterableComplementoRecepcion(val) {
    return {
        extra: false,
        mode: "menu",
        operators: {
            string: {
                startswith: _dictionary.KendoGridFilterable0001[val],
                eq: _dictionary.KendoGridFilterable0002[val],
                neq: _dictionary.KendoGridFilterable0003[val],
                contains: _dictionary.KendoGridFilterable0004[val],
            }
        },
        cell: {
            showOperators: false,
            operator: "contains"
        }
    }
}