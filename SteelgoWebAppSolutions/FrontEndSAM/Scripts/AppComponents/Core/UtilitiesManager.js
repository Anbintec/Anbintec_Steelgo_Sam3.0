/********************************************/
/********************************************/
/*********                          *********/
/*********    Utilities Manager     *********/
/*********                          *********/
/********************************************/
/********************************************/

/****************************/
/*    Global Variables      */
/****************************/

//Add all global variables for your partial view here
var $loadingCounter = 0;

/****************************/
/*    Document Ready        */
/****************************/
$(document).bind("ajaxStart", function () {
    loadingStart();
}).bind("ajaxStop", function () {
    loadingStop();
}).bind("ajaxError", function () {
    loadingStop();
});


//Method to be called on the document ready and contains all the pertinent code for a partial view
function utilitiesManagerToBeExecutedOnDocumentReady() {
    //CODE
}

/****************************/
/*    Global Functions      */
/****************************/

//Function to obtain an url parameter if is not present then return the notFindValue parameter
function getUrlParameter(sParam, notFindValue) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
    return notFindValue;
}

//function for open links from lists
function openFromList(event,url) {
    (event.ctrlKey || event.button === 1) ? window.open(url, "_blank") : window.location.href = url;
}

//Function to validate the existence of an element
function validateElementExistence(element) {
    if (typeof (element) !== 'undefined'
            && element != null) {
        return true;
    }
    return false;
}

function onRefresh(e) {
    kendo.ui.progress(e.sender.element, false);
};
function onOpen(e) {
    kendo.ui.progress(e.sender.element, true);
};

//Function to activate waiting screen
function loadingStart() {
    $(document).on('keypress keydown keyup', function (e) {
        if (e.which === 8 || e.which === 38 || e.which === 40) {
            e.stopImmediatePropagation();
            return false;
        };
        return false;
    });
    $("body").attr("style", "pointer-events:none;");
    $body.addClass("loading");
}

//Function to deactivate waiting screen
function loadingStop() {
    $("body").removeAttr("style", "pointer-events:none;");
    $body.removeClass("loading");
    $(document).off('keypress keydown keyup');   
}

//Function to encrypt base64 a string
function encrypt(toEncodData) {
    var encodedData = window.btoa(toEncodData);
    if (encodedData[encodedData.length - 1] == '=') {
        encodedData = encodedData.substr(0, encodedData.length - 1);
    }
    return encodedData;
}

//Function to decrypt base64 a string
function decrypt(encodedData) {
    if (encodedData[encodedData.length - 1] != '=') {
        encodedData = encodedData + '=';
    }
    return window.atob(encodedData);
}

//Function to show or hide the side menu
function toggleSideMenu() {
    if (Cookies.get("navegacion") != null && Cookies.get("navegacion") != "1" && Cookies.get("navegacion") != "2") {
        $("ul").removeClass("open");
        $("i.arrow").removeClass("down");
        $("#sidebar").toggleClass("minified");
        $(".content-container").toggleClass("expanded");
    }
}

//Function to redirect properly with the specified language
function redirectToLanguage(event,link) {
    event.preventDefault();
    if (!0 >= link.href.indexOf("?leng=") && link.href.indexOf("#") == -1) {
        if (!0 >= link.href.indexOf("&leng=")) {
            window.location.href = link.href + "?leng=" + $("#language").data("kendoDropDownList").value();
        } else {
            window.location.href = link.href + $("#language").data("kendoDropDownList").value();
        }
    } else {
        window.location.href = link.href;
    }
}

//Function to remove any grid
function removeGrid(grid) {
    var tmp = [];
    try {
        tmp = grid.data("kendoGrid").dataSource.data() || grid.data("kendoGrid").options.dataSource.data()
    } catch (e) { }
    var contenedor = grid.parent();
    grid.remove();
    contenedor.append("<div id='" + grid.attr("id") + "' class='" + grid.attr("class") + "'></div>");
    return tmp;
}

function messageindexKendoAutocomplete(obj, current) {
    if (obj.value()) {
        if (!current) {
            var elemento = obj.list.attr("id");
            var index = elemento.indexOf("-");
            var valor = elemento.substring(0, index);

            displayMessage("notificationslabel0083", $("#" + valor).closest("div").find("label").text(), '1');
        };
    }
};

function messageindexKendoCombobox(obj) {
    obj.select(function (dataItem) {
        if (dataItem.value === obj.value()) {
            obj.trigger("select");
            return dataItem.value === obj.value();
        }
    });
    if (obj.selectedIndex == -1) {
        if (obj.value()) {
            var elemento = obj.list.attr("id");
            var index = elemento.indexOf("-");
            var valor = elemento.substring(0, index);

            displayMessage("notificationslabel0083", $("#" + valor).closest("div").find("label").text(), '1');
        }
    };
};

function modificarResultTextoKendoCombobox(id, result, lng) {
    if (result.length > 0) {
        if (result[0].Placas == "Agregar nuevo" || result[0].Placas == "Add new") {
            switch (id) {
                case "Tracto":
                    if (lng == "es-MX") {
                        result[0].Placas = "Agregar nuevo";
                    } else {
                        result[0].Placas = "Add new";
                    }
                    break;
                default:
                    if (lng == "es-MX") {
                        result[0].Nombre = "Agregar nuevo";
                    } else {
                        result[0].Nombre = "Add new";
                    }
                    break;
            }
        }
    };
    return result;
};
function modificartextoKendoCombobox(controls, lng) {
    $(controls).each(function (i, obj) {
        switch ($(obj).attr("id")) {
            case "Tracto":
                if ($(obj).data("kendoComboBox").dataSource.data().length) {
                    if ($(obj).data("kendoComboBox").dataSource.data()[0].Placas == "Agregar nuevo"
                        || $(obj).data("kendoComboBox").dataSource.data()[0].Placas == "Add new")
                    {
                        if (lng == "es-MX") {
                            $(obj).data("kendoComboBox").dataSource.data()[0].Placas = "Agregar nuevo";
                        } else {
                            $(obj).data("kendoComboBox").dataSource.data()[0].Placas = "Add new";
                        }
                    }
                    $(obj).data("kendoComboBox").refresh();
                }
                break;
            case "PlanaID":
                if ($(obj).data("kendoMultiSelect").dataSource.data().length) {
                    if ($(obj).data("kendoMultiSelect").dataSource.data()[0].Nombre == "Agregar nuevo"
                        || $(obj).data("kendoMultiSelect").dataSource.data()[0].Nombre == "Add new")
                    {
                        if (lng == "es-MX") {
                            $(obj).data("kendoMultiSelect").dataSource.data()[0].Nombre = "Agregar nuevo";
                        } else {
                            $(obj).data("kendoMultiSelect").dataSource.data()[0].Nombre = "Add new";
                        }
                    }
                    $(obj).data("kendoMultiSelect").refresh();
                }
                break;
            default:
                if ($(obj).data("kendoComboBox").dataSource.data().length) {
                    if ($(obj).data("kendoComboBox").dataSource.data()[0].Nombre == "Agregar nuevo"
                            || $(obj).data("kendoComboBox").dataSource.data()[0].Nombre == "Add new") {
                        if (lng == "es-MX") {
                            $(obj).data("kendoComboBox").dataSource.data()[0].Nombre = "Agregar nuevo";
                        } else {
                            $(obj).data("kendoComboBox").dataSource.data()[0].Nombre = "Add new";
                        }
                        $(obj).data("kendoComboBox").refresh();
                    }
                };
                break;

        };

    });
};

function esNumero(n) {
    var patron = /^\d*$/;
    return patron.test(n);
}

function validaFloat(n) {
    var patron = /^([0-9])*[.]?[0-9]*$/;
    return patron.test(n);
};
//Ajusta el tamaño de los filtros rápidos a las columnas del grid
//Funcion: resizeFilters
//Parametros: N/A
//Return:     N/A
/*function resizeFilters() {   
    $("#grid .k-grid-header tr th:visible").each(function (i, obj) {
        var wd = 0;
        wd = $(this).outerWidth(true);
        //console.log("th: "+i+" width: "+$(this).outerWidth(true));
        $("#filterContainer > div").find("input").each(function (index, event) {
            if (i == index) {
                $(this).outerWidth(wd + 1.4);
                //console.log("qf: " + index + " width: " + $(this).outerWidth(true));
            }
        });
    });
    $(".quickFilters:first-of-type").outerWidth($(".quickFilters:first-of-type").outerWidth() + 0.6);

    $(window).resize(function () {
        $("#grid .k-grid-header tr th").each(function (i, obj) {
            var wd = 0
            wd = $(this).outerWidth(true);
            //console.log("th: "+i+" width: "+$(this).outerWidth(true));
            $("#filterContainer > div").find("input").each(function (index, event) {
                if (i == index) {
                    $(this).outerWidth(wd + 1.4);
                    //console.log("qf: " + index + " width: " + $(this).outerWidth(true));
                }
            });
        });
        $(".quickFilters:first-of-type").outerWidth($(".quickFilters:first-of-type").outerWidth() + 0.6);
    });
}*/

//function niceDeleteTemplate() {
//    return { command: [
//            {
//                name: _dictionary["KendoDeleteTxt0003"][$("#language").data("kendoDropDownList").value()],
//                click: function(e){
//                    var window = $("#window").kendoWindow({
//                        title: _dictionary["KendoDeleteTxt0003"][$("#language").data("kendoDropDownList").value()],
//                        visible: false, //the window will not appear before its .open method is called
//                        width: "400px",
//                        height: "200px",
//                    }).data("kendoWindow");
//                    var tr = $(e.target).closest("tr");
//                    var data = this.dataItem(tr);
//                    window.content('<div id="windowTemplate"><p>' + _dictionary.KendoDeleteTxt0001[$("#language").data("kendoDropDownList").value()] + '</p><button class="k-button" id="yesButton">' + _dictionary.KendoDeleteTxt0002[$("#language").data("kendoDropDownList").value()] + '</button> <button class="k-button" id="noButton">No</button></div>');
//                    window.open().center();  

//                    $("#KendoDeleteTxt0002").click(function(){
//                        grid.dataSource.remove(data)
//                        grid.dataSource.sync()
//                        window.close();
//                    })
//                    $("#noButton").click(function(){
//                        window.close();
//                    })
//                }                              
//            }
//    ]}
//}

//Adds validation to Date
Date.prototype.isValid = function () {
    return this.getTime() === this.getTime();
};

//Add the incell config for inline filter
function addTo(c,f) {
    if (c !== undefined) {//Verify the set of columns in case it is sent null
        if (f === null) {//ask if there is a set of model fields
            c.forEach(function (n) {
                Object.keys(n).indexOf("hidden") === -1 && Object.keys(n).indexOf("command") === -1 && Object.keys(n).indexOf("field") > -1 ? n["filterable"] = { cell: { showOperators: false, operator: "contains", delay: 2, dataTextField: n.field } } : 0;
            })
        } else {
            c.forEach(function (n) {//In case there is fields definition
                if (Object.keys(n).indexOf("hidden") === -1 && Object.keys(n).indexOf("command") === -1 && Object.keys(n).indexOf("field") > -1) {
                    if (f[n.field]["type"] === "number") {//Asks for the type of model field of the grid is equal to number
                        n["filterable"] = { cell: { showOperators: false, operator: "eq", delay: 2, dataTextField: n.field, ui: function (element) { element.kendoNumericTextBox({ format: "n4", decimals: 4 }); } } }
                    } else {
                        n["filterable"] = { cell: { showOperators: false, operator: "contains", delay: 2, dataTextField: n.field } }
                    }
                }
            })
        }
        return c;
    } else {
        return [];
    }
}

function quickHeadFilter(g) {
    var id = "#" + g.wrapper[0].id;
    var gr = '$("' + id + '").data("kendoGrid")';
    if ($(id + " .filter-row").length === 0) {
        g.thead.append(function () {
            var init = "<tr class='filter-row' tabindex='0'>"
            $(id+" thead th:visible").each(function () {
                if (this.hasAttribute("data-field") && $(this).attr("data-field") !== "") {
                    if (modelType(g, $(this).attr("data-field")) === "number") {
                        init += "<th tabindex='0'><input class='k-textbox' data-filter='" + $(this).attr("data-field") + "' type='number' onkeyup='quickFilter(" + gr + ",this,event)'/></th>";
                    } else {
                        init += "<th tabindex='0'><input class='k-textbox' data-filter='" + $(this).attr("data-field") + "' type='text' onkeyup='quickFilter(" + gr + ",this,event)'/></th>";
                    }
                } else {
                    init += "<th></th>";
                }
            })
            return init += "</tr>"
        })
    }
}

function modelType(g,fname) {
    try {
        return g.options.dataSource.schema.model.fields[$.trim(fname)].type;
    } catch (e) {
        return g.options.dataSource.options.schema.model.fields[$.trim(fname)].type;
    }
}

function quickFilter(g, i) {
    try {
        var f = g.dataSource.filter();
        var fname = $(i).attr("data-filter");
        if ($(i).val().length === 0 && f.filters.length > 0) {
            f.filters.forEach(function (n, p, t) {
                if (n.field === fname) {
                    t.splice(p, 1);
                    g.dataSource.filter(t);
                    f = t;
                }
            })
        }
        var tmp = { field: fname, value: $.trim($(i).val()) }
        modelType(g, fname) === "number" ? tmp.operator = function (item, value) { var u = false; (item !== null && item !== undefined) && item.toString().indexOf(value) !== -1 ? u = true : u = false; return u; } : tmp.operator = "contains";
        if (f !== undefined && f !== null) {
            if (modelType(g, fname) === "number" && event.target.value==="") {
                throw -1;
            }
            var found = false;
            f.filters.forEach(function (n) {
                if (n.field === fname) {
                    n.value = $(i).val();
                    found = true;
                }
            })
            found === false ? f.filters.push(tmp) : 0;
            g.dataSource.filter(f.filters)
        } else {
            g.dataSource.filter(tmp)
        }
    } catch (e) {
        console.log(e)
    }
}

function stringifyDate(g) {
    g.dataSource.data().forEach(function (n) {
        for (f in n) {
            var tmp = null;
            typeof n[f] === "object" ? tmp = new Date(n[f]) : tmp = new Date("Hello");
            if (tmp.isValid()) {
                var compD = "";
                var compM = "";
                10 > tmp.getDate() ? compD = "0" : null;
                10 > (tmp.getMonth() + 1) ? compM = "0" : null;
                n[f] = compD + tmp.getDate() + "/" + compM + (tmp.getMonth() + 1) + "/" + tmp.getFullYear();
            }
        }
    })
}

//Function that counts the number of non hidden columns and delete the left th
function checkTH(g) {
    var contador = 0;
    g.columns.forEach(function (c) {
        Object.keys(c).indexOf("hidden") === -1 ? contador++ : 0;
    })
    g.options.detailInit !== null && g.options.detailInit !== undefined ? contador++ : 0;
    while (contador < $("#" + g.wrapper[0].id + " tr:first th").length) {
        $("#" + g.wrapper[0].id + " tr:first th:last").remove();
        $("#" + g.wrapper[0].id + " .k-filter-row th:last").remove();
    }
}

//Detects IE
//Returns a Number in case of IE
//Returns a false in case of any other browser
//Works with IE6-IE11 && Edge
function detectIE() {
    var ua = window.navigator.userAgent;

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    var trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
        // IE 12 (aka Edge) => return version number
        return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
    }

    var msSafari = ua.indexOf('Safari');
    if (msSafari > 0) {
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msSafari)), 10);
    }
    // other browser
    return false;
}

function detectisExplorer() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return true;
    };
    return false;
};

function detectSafari() {
    var ua = window.navigator.userAgent;
    var msSafari = ua.indexOf('Safari');
    if (msSafari > 0) {
        return true;
    }
    return false;
};

function isInPopUp() {
    return window.location !== window.parent.location;
}

function Error(data) {
    if (data) {
        if (data.ReturnCode) {
            if (data.ReturnCode != 200) {
                if (data.ReturnCode == 401) {
                    removeUserSession();
                    return false;
                } else {
                    displayMessage("notificationslabel0008", data.ReturnMessage, '2');
                    return false;
                }
            } else {
                return true;
            }
        } else {
            return true;
        }
    } else {
        return false;
    }
};

function checkIfOne(w) {
    if (w !== undefined && w.dataSource !== undefined && w.dataSource.data().length!== 0) {
        if (w.dataSource.data().length === 1) {
            w.select(0);
            w.dataItem()[w.options.dataValueField] === "0" || w.dataItem()[w.options.dataValueField] === 0 ? w.value("") : w.trigger("change"); //Verifies if the first option is "Add new"
        }else w.select(-1);
    }
}

function notDotName(e) {
    var tmp = true;
    e.files.forEach(function (n) {
        if (n.name.split(".").length > 2) {
            displayMessage("notificationslabel0097", "", "2");
            tmp = false;
            e.preventDefault();
        }
    })
    return tmp;
}

function kendoDateValidation(start,end) {
    var tmp = false;
    var startDate = start.value().getTime();
    var endDate=null;
    if (typeof end === 'undefined') {
        endDate = new Date();
        endDate.setHours(0, 0, 0, 0)
    } else endDate=end.value().getTime();
    if (startDate > endDate && end !== 'undefined') {
        displayMessage("notificationslabel0098", "", "1");
        tmp = true;
    } else if (startDate > endDate && end === 'undefined') {
        displayMessage("notificationslabel0099", "", "1");
        tmp = true;
    }
    !tmp ? start.value(new Date(endDate)) : null;
    return tmp;
}

function getBySplit(str, sep, pos) {
    return str.split(sep)[pos];
}

function isKendoWidget(j) {
    return !(typeof kendo.widgetInstance(j) === "undefined")
}