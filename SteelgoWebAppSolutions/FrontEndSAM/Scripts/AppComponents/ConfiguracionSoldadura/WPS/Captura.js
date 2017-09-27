var ItemSeleccionado;
var resultadoJson;
var win;
var esNormal;

function changeLanguageCall() {
    document.title = _dictionary.WPSBreadcrumb[$("#language").data("kendoDropDownList").value()];
    CargarGrid();
    ObtenerJSONParaGrid();
};

function CargarGrid() {

    $("#grid").kendoGrid({
        edit: function (e) {


            if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()] && e.model.RegistrosWPS == 0) {

            } else {
                this.closeCell();
                if ($('#Guardar').text() != _dictionary.textoEditar[$("#language").data("kendoDropDownList").value()])
                    displayNotify("", _dictionary.lblWPSElementoWPS[$("#language").data("kendoDropDownList").value()] + e.model.WPSNombre + _dictionary.lblWPSYaAsignado[$("#language").data("kendoDropDownList").value()], 1);
            }
            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }

        },

        dataBound: function () {
            var myElem = document.getElementById('trParentHeader');
            if (myElem == null) {
                $("#grid").find("th.k-header").parent().before("<tr id='trParentHeader'> " +
                    "<th scope='col' colspan='1' class='k-header'></th>  <th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnPQR[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span>" + _dictionary.columnGrupoP[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnEspesormm[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnEspesorMax[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnEspesorRaiz[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnEspesorRelleno[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnPWHT[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + _dictionary.columnPreheat[$("#language").data("kendoDropDownList").value()] + "</span></th>" +
                    "<th width='auto'  colspan='2' class='k-header' style='text-align: center;'><span id=''>" + "CVN" + "</span></th>" +
                    
                    "</tr>");
            }
            var grid = $("#grid").data("kendoGrid");
            var gridData = grid.dataSource.view();

            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].RowOk == false) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");

                }
                else if (gridData[i].RowOk) {
                    if (i % 2 == 0)
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");

                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                }

            }

            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        WPSNombre: { type: "string", editable: true },
                        NombrePQRRaiz: { type: "string", editable: true },
                        NombrePQRRelleno: { type: "string", editable: true },
                        GrupoPRaiz: { type: "string", editable: false },
                        GrupoPRelleno: { type: "string", editable: false },
                        PWHTRelleno: { type: "boolean", editable: false },
                        PWHTRaiz: { type: "boolean", editable: false },
                        PREHEATRaiz: { type: "boolean", editable: false },
                        PREHEATRelleno: { type: "boolean", editable: false },
                        CVNRaiz: { type: "boolean", editable: false },
                        CVNRelleno: { type: "boolean", editable: false },
                        EspesorMaximo: { type: "number", editable: false },
                        EspesorMinimo: { type: "number", editable: false },
                        RaizEspesorRaiz: { type: "number", editable: false },
                        RaizEspesorRelleno: { type: "number", editable: false },
                        RellenoEspesorRaiz: { type: "number", editable: false },
                        RellenoEspesorRelleno: { type: "number", editable: false },
                        EspesorMaximoRaiz: { type: "number", editable: false },
                        EspesorMaximoRelleno: { type: "number", editable: false },
                        
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 4 }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        selectable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true
        },
        filterable: getGridFilterableMaftec(),

        columns: [
                    { field: "WPSNombre", title: _dictionary.columnNombre[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "110px" },
                    { field: "NombrePQRRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "110px", editor: RenderComboBoxPQRRaiz },
                    { field: "NombrePQRRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "110px", editor: RenderComboBoxPQRRelleno },
                    { field: "GrupoPRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
                    { field: "GrupoPRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
                    { field: "EspesorMinimo", title: _dictionary.columnMin[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "70px", attributes: { style: "text-align:right;" } },
                    { field: "EspesorMaximo", title: _dictionary.columnMax[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "70px", attributes: { style: "text-align:right;" } },
                    { field: "EspesorMaximoRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
                    { field: "EspesorMaximoRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px"},
                    { field: "RaizEspesorRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "65px", attributes: { style: "text-align:right;" } },
                    { field: "RaizEspesorRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "65px", attributes: { style: "text-align:right;" } },
                    { field: "RellenoEspesorRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "65px", attributes: { style: "text-align:right;" } },
                    { field: "RellenoEspesorRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "65px", attributes: { style: "text-align:right;" } },
                    {
                        field: "PWHTRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: {
                            multi: true,
                            messages: {
                                isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                style: "max-width:120px;"
                            },
                            dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                        }, width: "73px", template: "<input  readonly disabled type='checkbox' data-bind='checked: PWHTRaiz' #= PWHTRaiz ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
                    },
                    {
                        field: "PWHTRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: {
                            multi: true,
                            messages: {
                                isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                style: "max-width:120px;"
                            },
                            dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                        }, width: "80px", template: "<input readonly disabled type='checkbox' data-bind='checked: PWHTRelleno' #= PWHTRelleno ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
                    },
                    {
                        field: "PREHEATRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: {
                            multi: true,
                            messages: {
                                isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                style: "max-width:120px;"
                            },
                            dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                        }, width: "73px", template: "<input readonly disabled type='checkbox' data-bind='checked: PREHEATRaiz' #= PREHEATRaiz ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
                    },
                    {
                        field: "PREHEATRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: {
                            multi: true,
                            messages: {
                                isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                style: "max-width:120px;"
                            },
                            dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                        }, width: "80px", template: "<input readonly disabled type='checkbox' data-bind='checked: PREHEATRelleno' #= PREHEATRelleno ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
                    },
                    {
                        field: "CVNRaiz", title: _dictionary.columnRaiz[$("#language").data("kendoDropDownList").value()], filterable: {
                            multi: true,
                            messages: {
                                isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                style: "max-width:120px;"
                            },
                            dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                        }, width: "73px", template: "<input readonly disabled type='checkbox' data-bind='checked: CVNRaiz' #= CVNRaiz ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
                    },
                    {
                        field: "CVNRelleno", title: _dictionary.columnRelleno[$("#language").data("kendoDropDownList").value()], filterable: {
                            multi: true,
                            messages: {
                                isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                style: "max-width:120px;"
                            },
                            dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                        }, width: "80px", template: "<input readonly disabled type='checkbox' data-bind='checked: CVNRelleno' #= CVNRelleno ? checked='checked' : '' #/>", attributes: { style: "text-align:center;" }
                    },
                    { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px" },
                    //{ command: { text: _dictionary.botonDetalle[$("#language").data("kendoDropDownList").value()], click: EditaWPS }, title: _dictionary.tituloEditar[$("#language").data("kendoDropDownList").value()], width: "50px" }
        ],
    });
    CustomisaGrid($("#grid"));
};

function eliminarCaptura(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

        var filterValue = $(e.currentTarget).val();
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));


        if (dataItem.RegistrosWPS == 0) {

            var filterValue = $(e.currentTarget).val();
            var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));


            windowTemplate = kendo.template($("#windowTemplate").html());

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                modal: true,
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.WPSMensajeEliminar[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                var dataSource = $("#grid").data("kendoGrid").dataSource;
                dataItem.Accion = 3;
                dataItem.EditadoUsuario = true;
                dataSource.sync();
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });

            
        }
        else
            displayNotify("", _dictionary.lblWPSElementoWPS[$("#language").data("kendoDropDownList").value()] + dataItem.WPSNombre + _dictionary.lblWPSYaAsignado[$("#language").data("kendoDropDownList").value()], 1);    }
    else {
    }

}


function VentanaModal() {
    var modalTitle = "";
    modalTitle = _dictionary.WPSNombre[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowWPS");

    if (win == undefined) {

        win = window.kendoWindow({
            actions: "",
            modal: true,
            title: modalTitle,
            resizable: false,
            visible: false,
            width: "50%",
            minWidth: 660,
            position: {
                top: "10%",
                left: "20%"
            }
        }).data("kendoWindow");

    }

    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};


function NombreRepetido(listaDetalles) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var allData = dataSource.data();

    for (var i = 0; i < listaDetalles.length; i++) {
        for (var j = 0; j < listaDetalles.length; j++) {
            if (listaDetalles[i].WPSNombre.toLowerCase() == listaDetalles[j].WPSNombre.toLowerCase() && i != j) {
                listaDetalles[j].Estatus = -4;
                //$('tr[data-uid="' + allData[j].uid + '"] ').css("background-color", "#ffcccc");
                $("#grid").data("kendoGrid").dataSource._data[j].RowOk = false;
            }
        }
    }

    return ValidaNombreRepetido(listaDetalles);
}



function ValidaNombreRepetido(rows) {
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -4) {
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
            return true;
        }
    }
    return false;
}


function ObtenerEspesorCorrecto(EspesorTotalT, PWHT, CVN, ProcesoSoldadura, quitarCVN) {
    var espesores = [];
    espesores[0] = { EspesorMaximo: "", EspesorMinimo: "" };


    if (CVN && quitarCVN == 0) {
        if (PWHT == 0) {
            // Caso CVN sin PWHT
            // Si esta el valor entre 6 mm y 15.9999999 mm, se deja el espesor minimo es el valor de T,
            // y para 16 y superiores, es 16 mm 
            // si es menor de 6 mm el espesor minimo es: 1/2 T y los maximos se calculan con la tablota :v 
            if (EspesorTotalT < 6) {
                espesores[0].EspesorMinimo = (parseFloat(EspesorTotalT) / 2);
            }
            else if (EspesorTotalT >= 6 && EspesorTotalT < 16) {
                espesores[0].EspesorMinimo = parseFloat(EspesorTotalT);
            }
            else {
                espesores[0].EspesorMinimo = 16.0;
            }

            if (EspesorTotalT < 1.5) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
            }
            else if (EspesorTotalT >= 1.5 && EspesorTotalT < 10) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
            }
            else if (EspesorTotalT >= 10 && EspesorTotalT < 19) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
            }
            else if (EspesorTotalT >= 19 && EspesorTotalT < 38) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
            }
            else if (EspesorTotalT >= 38 && EspesorTotalT < 150) {
                espesores[0].EspesorMaximo = 200.0000;
            }
            else if (EspesorTotalT >= 150) {
                espesores[0].EspesorMaximo = (1.33 * parseFloat(EspesorTotalT));
            }

        }
        else {

            if ((ProcesoSoldadura.toLowerCase().indexOf("stt") !== -1 || ProcesoSoldadura.toLowerCase().indexOf("rmd") !== -1
        || ProcesoSoldadura.toLowerCase().indexOf("cmt") !== -1) && EspesorTotalT < 13) { // CVN para la comparacion del corto circuito, deben tener el proceso un sufijo STT,RMD o CMT 
                espesores[0].EspesorMaximo = (1.1 * parseFloat(EspesorTotalT));

                if (EspesorTotalT < 1.5) { // me voy al valor de la tabla para minimo 
                    espesores[0].EspesorMinimo = parseFloat(EspesorTotalT);
                }
                else if (EspesorTotalT >= 1.5 && EspesorTotalT < 10) {
                    espesores[0].EspesorMinimo = 1.5000;
                }
                else if (EspesorTotalT >= 10 && EspesorTotalT < 19) {
                    espesores[0].EspesorMinimo = 5.0000;
                }
                else if (EspesorTotalT >= 19 && EspesorTotalT < 38) {
                    espesores[0].EspesorMinimo = 5.0000;
                }
                else if (EspesorTotalT >= 38 && EspesorTotalT < 150) {
                    espesores[0].EspesorMinimo = 5.0000;
                }
                else if (EspesorTotalT >= 150) {
                    espesores[0].EspesorMinimo = 5.0000;
                }


            }
            else {
                if (EspesorTotalT < 1.5) {
                    espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                    espesores[0].EspesorMinimo = parseFloat(EspesorTotalT);
                }
                else if (EspesorTotalT >= 1.5 && EspesorTotalT < 10) {
                    espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                    espesores[0].EspesorMinimo = 1.5000;
                }
                else if (EspesorTotalT >= 10 && EspesorTotalT < 19) {
                    espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                    espesores[0].EspesorMinimo = 5.0000;
                }
                else if (EspesorTotalT >= 19 && EspesorTotalT < 38) {
                    espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                    espesores[0].EspesorMinimo = 5.0000;
                }
                else if (EspesorTotalT >= 38 && EspesorTotalT < 150) {
                    espesores[0].EspesorMaximo = 200.0000;
                    espesores[0].EspesorMinimo = 5.0000;
                }
                else if (EspesorTotalT >= 150) {
                    espesores[0].EspesorMaximo = (1.33 * parseFloat(EspesorTotalT));
                    espesores[0].EspesorMinimo = 5.0000;
                }
            }
        }
    }
    else {
        if ((ProcesoSoldadura.toLowerCase().indexOf("stt") !== -1 || ProcesoSoldadura.toLowerCase().indexOf("rmd") !== -1
        || ProcesoSoldadura.toLowerCase().indexOf("cmt") !== -1) && EspesorTotalT < 13) { // Para la comparacion del corto circuito, deben tener el proceso un sufijo STT,RMD o CMT 
            espesores[0].EspesorMaximo = (1.1 * parseFloat(EspesorTotalT));
            espesores[0].EspesorMinimo = (1.1 * parseFloat(EspesorTotalT)); // me voy al valor de la tabla para minimo 
        }
        else {
            if (EspesorTotalT < 1.5) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                espesores[0].EspesorMinimo = parseFloat(EspesorTotalT);
            }
            else if (EspesorTotalT >= 1.5 && EspesorTotalT < 10) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                espesores[0].EspesorMinimo = 1.5000;
            }
            else if (EspesorTotalT >= 10 && EspesorTotalT < 19) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                espesores[0].EspesorMinimo = 5.0000;
            }
            else if (EspesorTotalT >= 19 && EspesorTotalT < 38) {
                espesores[0].EspesorMaximo = (2 * parseFloat(EspesorTotalT));
                espesores[0].EspesorMinimo = 5.0000;
            }
            else if (EspesorTotalT >= 38 && EspesorTotalT < 150) {
                espesores[0].EspesorMaximo = 200.0000;
                espesores[0].EspesorMinimo = 5.0000;
            }
            else if (EspesorTotalT >= 150) {
                espesores[0].EspesorMaximo = (1.33 * parseFloat(EspesorTotalT));
                espesores[0].EspesorMinimo = 5.0000;
            }
        }
    }
    return espesores;
}

function EsCorrectoGruposMaterialBase(rows) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var allData = dataSource.data();


    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -1)
            // $('tr[data-uid="' + allData[i].uid + '"] ').css("background-color", "#ffcccc");
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
    }

    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -1)
            return true;
    }
    return false;
}

function EsCorrectoPWHTRELLENO(rows) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var allData = dataSource.data();


    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -2)
            //$('tr[data-uid="' + allData[i].uid + '"] ').css("background-color", "#ffcccc");
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
    }

    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -2)
            return true;
    }
    return false;
}

function EsCorrectoPreHitRelleno(rows) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var allData = dataSource.data();

    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -3)
            //$('tr[data-uid="' + allData[i].uid + '"] ').css("background-color", "#ffcccc");
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
    }

    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Estatus == -3)
            return true;
    }
    return false;
}





function obtenerGruposPLiberar(gp1, gp2, gp3, gp4) {
    var arreglo = [];
    arreglo[0] = gp1;
    arreglo[1] = gp2;
    arreglo[2] = gp3;
    arreglo[3] = gp4;


    var arregloSinRepetir = arreglo.filter(onlyUnique);
    if (arregloSinRepetir.length == 1) {
        arregloSinRepetir[1] = arregloSinRepetir[0];
    }

    return arregloSinRepetir;
}

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

function obtenerGruposP(wpsID, grupoP1, grupoP2, accion) {
    var gruposCompletos = [];

    if (grupoP1 == 'P15E' && grupoP2 == 'P15E') {
        gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P15E' };
        gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P5B' };
    }
    else if (grupoP1 == 'P15E' || grupoP2 == 'P15E') {
        if (grupoP1 == 'P15E') {
            gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P15E' };
            gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P5B' };
            gruposCompletos[2] = { Accion: accion, WPSID: wpsID, GrupoP: grupoP2 };
        }
        else if (grupoP2 == 'P15E') {
            gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P15E' };
            gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P5B' };
            gruposCompletos[2] = { Accion: accion, WPSID: wpsID, GrupoP: grupoP1 };
        }

    }
    else if (grupoP1 == 'P3' && grupoP2 == 'P3') {
        gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P1' };
        gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P3' };
    }
    else if (grupoP1 == 'P4' && grupoP2 == 'P4') {
        gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P1' };
        gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P3' };
        gruposCompletos[2] = { Accion: accion, WPSID: wpsID, GrupoP: 'P4' };
    }
    else if ((grupoP1 == 'P5A' && (grupoP2 == 'P1' || grupoP2 == 'P3' || grupoP2 == 'P4')) || (grupoP2 == 'P5A' && (grupoP1 == 'P1' || grupoP1 == 'P3' || grupoP1 == 'P4'))) {
        gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P1' };
        gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P3' };
        gruposCompletos[2] = { Accion: accion, WPSID: wpsID, GrupoP: 'P4' };

    }

    else if ((grupoP1 == 'P4' && (grupoP2 == 'P1' || grupoP2 == 'P3')) || (grupoP2 == 'P4' && (grupoP1 == 'P1' || grupoP1 == 'P3'))) {
        gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: 'P1' };
        gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: 'P3' };
    }
    else {
        gruposCompletos[0] = { Accion: accion, WPSID: wpsID, GrupoP: grupoP1 };
        gruposCompletos[1] = { Accion: accion, WPSID: wpsID, GrupoP: grupoP2 };
    }

    return gruposCompletos;

}