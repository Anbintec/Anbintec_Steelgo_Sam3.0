var tiposCSV = ["application/csv", "application/excel", "application/lotus123", "application/msexcel", "application/vnd.lotus-1-2-3", "application/vnd.ms-excel", "application/vnd.ms-works", "application/vnd.msexcel", "application/wk1", "application/wks", "application/x-123", "application/x-dos_ms_excel", "application/x-excel", "application/x-lotus123", "application/x-ms-excel", "application/x-msexcel", "application/x-msworks", "application/x-wks", "application/x-xls", "application/xlc", "application/xls", "text/anytext", "text/comma-separated-values", "text/csv", "zz-application/zz-winassoc-wk1"];
var arregloDatos;
function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoCargarCSV();
    SuscribirEventoCancelar();
    SuscribirEventoPlancharCampos();
}
function SuscribirEventoCancelar() {
    $("#Cancelar").click(function (e) {
        window.location.reload(true);
    });
}

function SuscribirEventoProyecto() {    
    $("#inputProyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            $("input[type=checkbox]").prop("checked", false);
        }
    });
}

function SuscribirEventoCargarCSV() {
    $("#btnCargaCsv").click(function () {
        $("#files").val("");
        $("#files").click();
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
                            for (var i = 0, f; f = file[i]; i++) {
                                $("#lblArchivo").html("");
                                $("#lblArchivo").html(f.name);
                            }                                                        
                            var dataArray = CSVToArray(csvData); //se obtiene un arreglo de objetos con los datos
                            var filasOmitidas;
                            
                            if (dataArray.length > 0) {
                                arregloDatos = dataArray; //guardo en variable global
                            } else {
                                displayMessage("", "No hay Datos", "1");
                            }                            
                        };
                        reader.onerror = function () {
                            alert('Unable to read ' + file.fileName);
                        };
                    }
                } catch (e) { }
            }
        });
    });
}

//funcion creada por jssuttles GIT HUB: https://gist.github.com/jssuttles/8fb8b16a152558906469dfefbf88f658
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
    for (var i = 0; i < arrData.length; i++) {
        if (arrData[i].Nu != "" && (arrData[i].diametro1 != "" && arrData[i].diametro2 != "")) {
            tmpArray.push(arrData[i]);
        }
    }
    return (tmpArray);
}

function SuscribirEventoPlancharCampos() {
    $("#btnPlanchar").click(function () {
        var proyecto = $("#inputProyecto").data("kendoComboBox").value();        
        var labelArchivo = $("#lblArchivo").html();
        var aplicaColada = $("#checkColada").prop("checked");
        var aplicaMTR = $("#checkMTR").prop("checked");
        var aplicaInventario = $("#checkInventario").prop("checked");
        var aplicaEstFisico = $("#checkEstatusFisico").prop("checked");
        var aplicaEstDoc = $("#checkEstatusDoc").prop("checked");
        var aplicaFabricante = $("#checkFabricante").prop("checked");
        var aplicaNUC = $("#checkNumUnicoCliente").prop("checked");
        var aplicaRack = $("#checkRack").prop("checked");
        if (proyecto != 0 && proyecto != undefined && proyecto != "") {
            if (labelArchivo != "") {                
                if (arregloDatos.length > 0) {                    
                    if (!aplicaColada && !aplicaMTR && !aplicaInventario && !aplicaEstFisico && !aplicaEstDoc && !aplicaFabricante && !aplicaNUC && !aplicaRack) {
                        displayMessage("", "No hay seleccionado ningún campo para planchar","1");
                    } else {
                        var data = [];
                        var coma = ",";
                        for (n in arregloDatos) {
                            data[n] = {
                                Nu: "",
                                Diametro1: "",
                                Diametro2: "",
                                TotalRecibido: "",
                                FisicoCondicionado: "",
                                FisicoDanado: "",
                                FisicoAprobado: "",
                                TotalFisico: "",
                                InventarioCongelado: "",
                                DocumentalAprobado: "",
                                DocumentalRechazado: "",
                                NumeroColada: "",
                                NumeroMtr: "",
                                Fabricante: "",
                                NumeroUnicoCliente: "",
                                Rack: "",
                                AplicaColada: false,
                                AplicaMtr: false,
                                AplicaInventario: false,
                                AplicaEstatusFisico: false,
                                AplicaEstatusDocumental: false,
                                AplicaFabricante: false,
                                AplicaNumeroUnicoCliente: false,
                                AplicaRack: false
                            };
                            data[n].Nu = arregloDatos[n].Nu.toString().trim();
                            data[n].Diametro1 = arregloDatos[n].diametro1;
                            data[n].Diametro2 = arregloDatos[n].diametro2;                            
                            if (arregloDatos[n].TotalRecibido.toString().indexOf(coma) !== -1) {
                                data[n].TotalRecibido = arregloDatos[n].TotalRecibido = arregloDatos[n].TotalRecibido.toString().replace(coma, "");
                            } else {
                                data[n].TotalRecibido = arregloDatos[n].TotalRecibido;
                            }
                            if (arregloDatos[n].FisicoCondicionado.toString().indexOf(coma) !== -1) {
                                data[n].FisicoCondicionado = arregloDatos[n].FisicoCondicionado = arregloDatos[n].FisicoCondicionado.toString().replace(coma, "");
                            } else {
                                data[n].FisicoCondicionado = arregloDatos[n].FisicoCondicionado;
                            }                            

                            if (arregloDatos[n].FisicoDanado.toString().indexOf(coma) !== -1) {
                                data[n].FisicoDanado = arregloDatos[n].FisicoDanado = arregloDatos[n].FisicoDanado.toString().replace(coma, "");
                            } else {
                                data[n].FisicoDanado = arregloDatos[n].FisicoDanado;
                            }
                            
                            if (arregloDatos[n].FisicoAprobado.toString().indexOf(coma) !== -1) {
                                data[n].FisicoAprobado = arregloDatos[n].FisicoAprobado = arregloDatos[n].FisicoAprobado.toString().replace(coma, "");
                            } else {
                                data[n].FisicoAprobado = arregloDatos[n].FisicoAprobado;
                            }
                            if (arregloDatos[n].TotalFisico.toString().indexOf(coma) !== -1) {
                                data[n].TotalFisico = arregloDatos[n].TotalFisico = arregloDatos[n].TotalFisico.toString().replace(coma, "");
                            } else {
                                data[n].TotalFisico = arregloDatos[n].TotalFisico;
                            }
                            if (arregloDatos[n].InventarioCongelado.toString().indexOf(coma) !== -1) {
                                data[n].InventarioCongelado = arregloDatos[n].InventarioCongelado = arregloDatos[n].InventarioCongelado.toString().replace(coma, "");
                            } else {
                                data[n].InventarioCongelado = arregloDatos[n].InventarioCongelado;
                            }
                            if (arregloDatos[n].DocumentalAprobado.toString().indexOf(coma) !== -1) {
                                data[n].DocumentalAprobado = arregloDatos[n].DocumentalAprobado = arregloDatos[n].DocumentalAprobado.toString().replace(coma, "");
                            } else {
                                data[n].DocumentalAprobado = arregloDatos[n].DocumentalAprobado;
                            }
                            if (arregloDatos[n].DocumentalRechazado.toString().indexOf(coma) !== -1) {
                                data[n].DocumentalRechazado = arregloDatos[n].DocumentalRechazado = arregloDatos[n].DocumentalRechazado.toString().replace(coma, "");
                            } else {
                                data[n].DocumentalRechazado = arregloDatos[n].DocumentalRechazado;
                            }                            
                            data[n].NumeroColada = arregloDatos[n].numerocolada;
                            data[n].NumeroMtr = arregloDatos[n].numeromtr;
                            data[n].Fabricante = arregloDatos[n].fabricante;
                            data[n].NumeroUnicoCliente = arregloDatos[n].numerounicocliente;
                            data[n].Rack = arregloDatos[n].rack;
                            data[n].AplicaColada = aplicaColada;
                            data[n].AplicaMtr = aplicaMTR;
                            data[n].AplicaInventario = aplicaInventario;
                            data[n].AplicaEstatusFisico = aplicaEstFisico;
                            data[n].AplicaEstatusDocumental = aplicaEstDoc;
                            data[n].AplicaFabricante = aplicaFabricante;
                            data[n].AplicaNumeroUnicoCliente = aplicaNUC;
                            data[n].AplicaRack = aplicaRack;
                        }                                    
                        $(".alert-container").removeClass("active");                        
                        if (data.length > 0) {
                            AjaxPlancharCampos(data);
                        }
                    }                    
                }                
            } else {
                displayMessage("", "Por favor suba un archivo con extencion .csv", "1");
            }
        } else {
            displayMessage("","Por favor seleccione un proyecto","1");
        }        
    });
}