var tiposCSV = ["application/csv", "application/excel", "application/lotus123", "application/msexcel", "application/vnd.lotus-1-2-3", "application/vnd.ms-excel", "application/vnd.ms-works", "application/vnd.msexcel", "application/wk1", "application/wks", "application/x-123", "application/x-dos_ms_excel", "application/x-excel", "application/x-lotus123", "application/x-ms-excel", "application/x-msexcel", "application/x-msworks", "application/x-wks", "application/x-xls", "application/xlc", "application/xls", "text/anytext", "text/comma-separated-values", "text/csv", "zz-application/zz-winassoc-wk1"];
var error = 0;
var arregloDatos;

function SuscribirEventos() {
    SuscribirEventoComboMoneda();
    suscribirEventoCarGaCSV();
    suscribirEventoDescarGaCSV();
    suscribirEventoGuardar();
    SuscribirEventoComboCliente();
}
function suscribirEventoGuardar() {
    $('#btnGuardarSup1, #btnGuardarInf1, #btnGuardarSup2, #btnGuardarInf2 ').click(function (e) {
        
        if ($("#inputCliente").data("kendoComboBox").text() != "" && $("#inputMonedaCobro").data("kendoComboBox").text() != "" && $("#inputMonedaPago").data("kendoComboBox").text() != ""
                && $('#inputOrdenCompra').val() != "") {
                var dataGrid = $("#grid").data("kendoGrid").dataSource._data;
                var cont = 0;
                if (dataGrid.length > 0) {
                    AjaxGuardarCaptura(dataGrid, 0);
                } else {
                    displayNotify("AdverteciaExcepcionGuardado", "", "1");
                }
            }
            else {
                displayNotify("MensajeErrorCreacionOCLlenarCampos", "", "1");
            }
        
    });

    //$('#btnGuardarYNuevoSup, #btnGuardarYNuevoInf').click(function (e) {
    //    if ($("#btnGuardarSup1").text() === _dictionary.lblGuardar[$("#language").val()]) {
    //        if ($("#inputCliente").data("kendoComboBox").text() != "" && $("#inputMonedaCobro").data("kendoComboBox").text() != ""
    //            && $('#inputOrdenCompra').val() != "") {
    //            var ds = $("#grid").data("kendoGrid").dataSource;
    //            if (ds._data.length > 0) {
    //                AjaxGuardarCaptura(ds._data, 1);
    //            }
    //            else {
    //                displayNotify("AdverteciaExcepcionGuardado", "", '1');
    //            }
    //        }
    //        else {
    //            displayNotify("MensajeErrorCreacionOCLlenarCampos", "", "1");
    //        }
    //    } else {
    //        opcionHabilitarView(false);
    //    }
    //});
}

function suscribirEventoDescarGaCSV() {
    $("#btnDescargaCsv, #btnDescargaCsv1").click(function (e) {
        window.location.href = "/FormatoOrdenesCompra.csv";
    });
}

function suscribirEventoCarGaCSV() {
    $('#btnCargaCsv, #btnCargaCsv1').click(function (e) {
        $("#files").val("");
        $("#files").click();
    });
    document.getElementById("files").addEventListener("change", function (evt) {
        if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
            displayMessage("", "ListadoCatalogos0007", '2');
        } else {
            var data = [];
            var file = evt.target.files[0];
            try {
                if (tiposCSV.indexOf(file.type.toLowerCase()) == -1) {
                    this.value = null;
                    displayMessage("ListadoCatalogos0008", "", '2'); //arhivo invalido
                } else {
                    var reader = new FileReader();
                    reader.readAsText(file);
                    reader.onload = function (event) {
                        var csvData = event.target.result;
                        var file = evt.target.files; // FileList object                            
                        var output = [];
                        var dataArray = CSVToArray(csvData); //se obtiene un arreglo de objetos con los datos
                        var filasOmitidas;

                        var ds = $("#grid").data("kendoGrid").dataSource;
                        if (dataArray.length > 0) {
                            ds.data(dataArray);
                            ds.sync();
                        }
                        else {
                            displayNotify("", "No hay datos correctos", "1");
                        }
                        
                    };
                    reader.onerror = function () {
                        DisplayNotify("", 'Unable to read ' + file.fileName, "1");
                    };
                }
            } catch (e) { }
        }
    });

}


function ObtenerNewData(data) {
    try {
        var tmpData = [];
        elementos = [];
        contador = {};
        var tmpNumControl = {};
        if (data.length > 0) {
            for (n in data) {
                tmpData[n] = {
                    JuntaSpoolid: 0,
                    FechaEntregaReporteArmado: "",
                    FechadeArmado: "",
                    ClaveTubero: "",
                    FechaEntregaReporteSoldadura: "",
                    FechaSoldadura: "",
                    ClaveSoldador1: "",
                    ClaveSoldador2: "",
                    Wps: "",
                    Proceso: "",
                    Material1: "",
                    Material2: "",
                    LiberacionArmado: "",
                    FechaLiberacionArmado: "",
                    LiberacionDimensional: "",
                    FechaLiberacionDimensional: "",
                    LiberacionVisual: "",
                    FechaLiberacionVisual: "",
                    Repetido: 0
                };
                //if (data[n].OKPND.toString().trim() === "") { data[n].OKPND = 0 };

                if (!isNaN(parseInt(data[n].JuntaSpoolid.trim()))) {

                    //tmpNumControl[n] = data[n].NumeroControl.toString().toUpperCase().trim();

                    tmpData[n].JuntaSpoolid = parseInt(data[n].JuntaSpoolid.trim());
                    tmpData[n].FechaEntregaReporteArmado = (new Date(ObtenerDato(data[n].FechaEntregaReporteArmado.trim(), 1), ObtenerDato(data[n].FechaEntregaReporteArmado.trim(), 2), ObtenerDato(data[n].FechaEntregaReporteArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].FechadeArmado = (new Date(ObtenerDato(data[n].FechadeArmado.trim(), 1), ObtenerDato(data[n].FechadeArmado.trim(), 2), ObtenerDato(data[n].FechadeArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].ClaveTubero = data[n].ClaveTubero.toString().trim();
                    tmpData[n].FechaEntregaReporteSoldadura = (new Date(ObtenerDato(data[n].FechadeArmado.trim(), 1), ObtenerDato(data[n].FechadeArmado.trim(), 2), ObtenerDato(data[n].FechadeArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].FechaSoldadura = (new Date(ObtenerDato(data[n].FechadeArmado.trim(), 1), ObtenerDato(data[n].FechadeArmado.trim(), 2), ObtenerDato(data[n].FechadeArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].ClaveSoldador1 = data[n].ClaveSoldador1.toString().trim();
                    tmpData[n].ClaveSoldador2 = data[n].ClaveSoldador2.toString().trim();
                    tmpData[n].Wps = data[n].Wps.toString().trim();
                    tmpData[n].Proceso = data[n].Proceso.toString().trim();
                    tmpData[n].Material1 = data[n].Material1.toString().trim();
                    tmpData[n].Material2 = data[n].Material2.toString().trim();
                    tmpData[n].LiberacionArmado = data[n].LiberacionArmado.toString().trim();
                    tmpData[n].FechaLiberacionArmado = (new Date(ObtenerDato(data[n].FechadeArmado.trim(), 1), ObtenerDato(data[n].FechadeArmado.trim(), 2), ObtenerDato(data[n].FechadeArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].LiberacionDimensional = data[n].LiberacionDimensional.toString().trim();
                    tmpData[n].FechaLiberacionDimensional = (new Date(ObtenerDato(data[n].FechadeArmado.trim(), 1), ObtenerDato(data[n].FechadeArmado.trim(), 2), ObtenerDato(data[n].FechadeArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].LiberacionVisual = data[n].LiberacionVisual.toString().trim();
                    tmpData[n].FechaLiberacionVisual = (new Date(ObtenerDato(data[n].FechadeArmado.trim(), 1), ObtenerDato(data[n].FechadeArmado.trim(), 2), ObtenerDato(data[n].FechadeArmado.trim(), 3))).toISOString().slice(0, 10).replace(/-/g, "");//año, mes, dia;
                    tmpData[n].Repetido = 0;

                } else {
                    displayNotify("ErrorColumnaTieneLetras", "", "2");
                    return;
                }

            }
        } else {
            //no hay datos
            tmpData = [];
        }
    } catch (e) {
        if (e !== -1) {
            error = 1;
            throw e;
        } else {
            //displayNotify("ListadoCatalogos0012", "", '2');
            displayNotify("ErrorColumnaTieneRegistroVacio", "", "2");
            error = 1;
        }
    }
    loadingStop();
    return tmpData;
}



function CSVToArray(strData, strDelimiter) {
    // Check to see if the delimiter is defined. If not,
    // then default to comma.
    strDelimiter = (strDelimiter || ",");

    // Create a regular expression to parse the CSV values.
    var objPattern = new RegExp(
        (
            // Delimiters.
            "(\\" + strDelimiter + "|\\r?\\n|\\r|^)" +

            // Quoted fields.
            "(?:\"([^\"]*(?:\"\"[^\"]*)*)\"|" +

            // Standard fields.
            "([^\"\\" + strDelimiter + "\\r\\n]*))"
        ),
        "gi"
        );
    // Create an array to hold our data. Give the array
    // a default empty first row.
    var arrData = [];
    var headers = [];
    var headersFound = false;
    var headerIndex = 0;

    // Create an array to hold our individual pattern
    // matching groups.
    var arrMatches = null;
    // Keep looping over the regular expression matches
    // until we can no longer find a match.
    while (arrMatches = objPattern.exec(strData)) {
        // Get the delimiter that was found.
        var strMatchedDelimiter = arrMatches[1];

        // Check to see if the given delimiter has a length
        // (is not the start of string) and if it matches
        // field delimiter. If id does not, then we know
        // that this delimiter is a row delimiter.
        if (strMatchedDelimiter.length && strMatchedDelimiter !== strDelimiter) {
            // Since we have reached a new row of data,
            // add an empty row to our data array.
            arrData.push({});
            headersFound = true;
            headerIndex = 0;
        }
        var strMatchedValue;
        // Now that we have our delimiter out of the way,
        // let's check to see which kind of value we
        // captured (quoted or unquoted).
        if (arrMatches[2]) {
            // We found a quoted value. When we capture
            // this value, unescape any double quotes.
            strMatchedValue = arrMatches[2].replace(new RegExp("\"\"", "g"), "\"");
        } else {
            // We found a non-quoted value.
            strMatchedValue = arrMatches[3];
        }
        // Now that we have our value string, let's add
        // it to the data array.
        if (!headersFound) {
            headers.push(strMatchedValue);
        } else {
            arrData[arrData.length - 1][headers[headerIndex]] = strMatchedValue;
            headerIndex++;
        }
    }
    // quito las filas vacias
    var tmpArray = [];
    var todoOk = true;
    for (var i = 0; i < arrData.length; i++) {
        arrData[i].RowOk = true;
        if (!(arrData[i].Consecutivo == "" && arrData[i].Descripcion == undefined )) {

            if (arrData[i].Consecutivo == "" || arrData[i].Descripcion == "" && arrData[i].MaterialNorma == ""
                || arrData[i].Diametro1 == "" || arrData[i].Cantidad == "" || arrData[i].PrecioCompra == "" || arrData[i].PrecioVenta == "") {
                arrData[i].RowOk = false;
                todoOk = false;
            }
            if (arrData[i].diametro2 != "")
                arrData[i].diametro2 = 0;
            arrData[i].Revision = arrData[i].Descripcion + arrData[i].MaterialNorma + arrData[i].Diametro1 +
                        arrData[i].Diametro2 + arrData[i].Shedule + arrData[i].Rating + arrData[i].PrepExt;
            tmpArray.push(arrData[i]);
        }

    }
    if (!todoOk)
        displayNotify("MensajeErrorRenglonesCSV", "", "1");

    return (tmpArray);
}


function SuscribirEventoComboMoneda() {
    $("#inputMonedaCobro").kendoComboBox({
        dataValueField: "MonedaID",
        dataTextField: "Moneda",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputMonedaCobro").data("kendoComboBox").value(0);
                $("#inputMonedaCobro").data("kendoComboBox").text("");
            }
        }
    });


    $("#inputMonedaPago").kendoComboBox({
        dataValueField: "MonedaID",
        dataTextField: "Moneda",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputMonedaPago").data("kendoComboBox").value(0);
                $("#inputMonedaPago").data("kendoComboBox").text("");
            }
        }
    });

}

function SuscribirEventoComboCliente() {
    $("#inputCliente").kendoComboBox({
        dataValueField: "ClienteID",
        dataTextField: "Cliente",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputCliente").data("kendoComboBox").value(0);
                $("#inputCliente").data("kendoComboBox").text("");
            }
        }
    });
}