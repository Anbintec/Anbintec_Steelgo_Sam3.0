var ventanaPopup;

var dataItemSeleccionadoPopup;
var EmbarquePlanaID = 0;
var esNormal;

function changeLanguageCall() {
    CargarGrid();
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#lblEstatus").text("");
    $("#lblEmbarqueCargaTotalPiezas").text("");
    $("#lblEmbarqueCargaToneladasCargadas").text("");
    $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value("");
    $("#inputProveedor").data("kendoComboBox").value("");

    AjaxCargarProyecto();
    AjaxCargarZona();
    AjaxCargarCamposPredeterminados();

    document.title = _dictionary.EmbarqueHeaderCargaPlana[$("#language").data("kendoDropDownList").value()];

    opcionHabilitarView(false, "FieldSetView");
};

if ($("#inputHiddenEmbarquePlanaID").val() != null && $("#inputHiddenEmbarquePlanaID").val() != undefined && $("#inputHiddenEmbarquePlanaID").val() != "0") {
    EmbarquePlanaID = $("#inputHiddenEmbarquePlanaID").val();
}
else {
    EmbarquePlanaID = 0;

}

IniciarCapturaEmbarqueCarga();

function IniciarCapturaEmbarqueCarga() {
    SuscribirEventos();
}


function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');

                inputName.select();
            });

            if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                this.closeCell();
            }

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Consecutivo: { type: "number", editable: false },
                        Spool: { type: "string", editable: false },
                        Paquete: { type: "string", editable: false },
                        Peso: { type: "number", editable: false },
                        Pdi: { type: "number", editable: false },
                        Peq: { type: "number", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                    { field: "Accion", operator: "eq", value: 0 },
                    { field: "Accion", operator: "eq", value: 2 },
                    { field: "Accion", operator: "eq", value: undefined }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            aggregate: [
                { field: "Peso", aggregate: "sum" },
                { field: "Pdi", aggregate: "sum" },
                { field: "Peq", aggregate: "sum" }
            ]
        },
        navigatable: true,
        filterable: getGridFilterableMaftec(),
        editable: false,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "Consecutivo", title: _dictionary.columnConsecutivoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "100px", attributes: { style: "text-align:right;" } },
            { field: "Spool", title: _dictionary.columnSpoolIDEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "150px" },
            { field: "Paquete", title: _dictionary.columnPaqueteEmbarque[$("#language").data("kendoDropDownList").value()], template: "<div class='descargarPaquete' style='text-align:center;'><a href='\\#'> <span>#=Paquete#</span></a></div>", filterable: getGridFilterableCellMaftec(), width: "150px" },
            { field: "Peso", title: _dictionary.columnPeso[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n')#</div>", attributes: { style: "text-align:right;" } },
            { field: "Pdi", title: "PDI", filterable: getGridFilterableCellNumberMaftec(), aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", attributes: { style: "text-align:right;" }, width: "150px" },
            { field: "Peq", title: "PEQ", filterable: getGridFilterableCellNumberMaftec(), aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", attributes: { style: "text-align:right;" }, width: "150px" },
            { command: { text: _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()], click: DescargarSpool }, title: _dictionary.columnDescargar[$("#language").data("kendoDropDownList").value()], width: "70px", attributes: { style: "text-align:center;" } },

        ],
        dataBound: function (e) {
            var grid = $("#grid").data("kendoGrid");
            var gridData = grid.dataSource.view();

            if (gridData.length > 0) {
                for (var i = 0; i < gridData.length; i++) {
                    var currentUid = gridData[i].uid;
                    var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    var editButton = $(currenRow).find(".k-button");
                    if (gridData[i].Accion == 2) {
                        var classDescarga = $("#language").val() == "es-MX" ? "k-grid-Descarga" : "k-grid-Discharging";
                        editButton[0].outerHTML = '<a class="k-button k-button-icontext ' + classDescarga + '" href="#/"><span class=""></span>' +
                            _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()] + '</a>';

                    } else {
                        if (gridData[i].PaqueteID == 0) {
                            editButton[0].outerHTML = '<a class="k-button k-button-icontext k-grid-Cancelar" href="#/"><span class=""></span>' +
                                _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()] + '</a>';
                        }
                        else {
                            editButton[0].outerHTML = '<a href="#/"><span class=""></span></a>';
                        }
                    }
                }
            }

            
            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].RowOk == false) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowErrorNU");
                }
                else if (gridData[i].RowOk) {
                    if (i % 2 == 0)
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");

                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowErrorNU");
                }
            }

            if (esNormal)
                $(".k-grid-content td").css("white-space", "normal");
            else
                $(".k-grid-content td").css("white-space", "nowrap");


        }
    });
    CustomisaGrid($("#grid"));

    $("#grid .k-grid-content").on("change", "input.chkbx", function (e) {
        if ($("#language").val() == "es-MX") {
            if ($('#Guardar').text() != "Editar" && $("#lblEstatus").text().toLowerCase() != "cerrada") {
                var grid = $("#grid").data("kendoGrid");
                dataItem = grid.dataItem($(e.target).closest("tr"));
                dataItem.set("Seleccionado", this.checked);
                if (this.checked) {
                    dataItem.Seleccionado = true;
                }
                else {
                    dataItem.Seleccionado = false;
                }

                grid.dataSource.sync();
            }
            else {
                var grid = $("#grid").data("kendoGrid");
                if (this.checked) {
                    e.target.checked = false;
                }
                else {
                    e.target.checked = true;
                }
                grid.dataSource.sync();
            }
        }
        else {
            if ($('#Guardar').text() != "Edit" && $("#lblEstatus").text().toLowerCase() != "closed") {
                var grid = $("#grid").data("kendoGrid");
                dataItem = grid.dataItem($(e.target).closest("tr"));
                dataItem.set("Seleccionado", this.checked);
                if (this.checked) {
                    dataItem.Seleccionado = true;
                }
                else {
                    dataItem.Seleccionado = false;
                }

                grid.dataSource.sync();
            }
            else {
                var grid = $("#grid").data("kendoGrid");
                if (this.checked) {
                    this.checked = false;
                }
                else {
                    this.checked = true;
                }
                grid.dataSource.sync();
            }
        }


    });
};

function DescargarSpool(e) {
    e.preventDefault();
    if (!$("#inputCerrar").is(":checked")) {
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
            $("#rowGridDescargaSpool").val(dataItem.uid);

            $("#inputZonaPopup").data("kendoComboBox").value(dataItem.ZonaAnteriorID == 0 && dataItem.CuadranteAnteriorID != 0 ? 1 : dataItem.ZonaAnteriorID);
            $("#inputZonaPopup").data("kendoComboBox").trigger("change");
            CuadranteSpoolAnterior = dataItem.CuadranteAnteriorID;

            ventanaPopup.title(_dictionary.EmbarqueCargaTituloPopupCuadrante[$("#language").data("kendoDropDownList").value()]);
            ventanaPopup.open().center();
        }
    } else {
        CuadranteSpoolAnterior = 0;
        displayNotify("EmbarqueCargaMsjDescargaPlanaCerrada", "", "1");
    }
}

function validarInformacion(row) {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var existe = false;

    for (var i = 0; i < ds._data.length; i++) {

        if (ds._data[i]["NumeroControl"] == row.NumeroControl) {
            existe = true;
            break;
        }


    }
    return existe;
}

function validarExistaSoloUnpaqueteSeleccionado() {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var PaqueteIDSeleccionado;
    var filaConPaqueteSinAsignar = false;
    var contador = 0;
    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i]["Seleccionado"]) {
            for (var j = 0; j < ds._data.length; j++) {
                if (ds._data[i]["EmbarquePaqueteID"] != 0 && i != j && ds._data[i]["EmbarquePaqueteID"] != ds._data[j]["EmbarquePaqueteID"] && ds._data[j]["EmbarquePaqueteID"] != 0)
                    contador++;
            }
        }
    }

    if (contador == 0) {//no hay mas de un paquete seleccionado
        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i]["Seleccionado"] && ds._data[i]["EmbarquePaqueteID"] == 0) {
                filaConPaqueteSinAsignar = true;
            }
            else if (ds._data[i]["Seleccionado"] && ds._data[i]["EmbarquePaqueteID"] != 0) {
                filaConPaqueteSinAsignar = false;
                break;
            }
        }

        return (true && !filaConPaqueteSinAsignar);
    }
    return false;
}



function CargaPopupNuevaPlana(e) {
    $("#inputNombreNuevaPlana").val("");

    windowNewPlate = $("#divNuevoPlana").kendoWindow({
        modal: true,
        visible: true,
        width: "500px",
        height: "auto",
        position: {
            top: "1%",
            left: "1%"
        },
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        actions: []
    }).data("kendoWindow");
    $("#divNuevoPlana").data("kendoWindow").title(_dictionary.EmbarqueCargaNuevaPlana[$("#language").data("kendoDropDownList").value()]);
    $("#divNuevoPlana").data("kendoWindow").center().open();
    $("#inputNombreNuevaPlana").focus();
}


function CargaPopupNuevoProveedor(e) {
    $("#inputNombreNuevoProveedor").val("");

    windowNewProvider = $("#divNuevoProveedor").kendoWindow({
        modal: true,
        resizable: false,
        visible: true,
        width: "500px",
        height: "auto",
        position: {
            top: "1%",
            left: "1%"
        },
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        actions: []
    }).data("kendoWindow");
    $("#divNuevoProveedor").data("kendoWindow").title(_dictionary.EmbarqueCargaNuevoProveedor[$("#language").data("kendoDropDownList").value()]);
    $("#divNuevoProveedor").data("kendoWindow").center().open();

    $("#inputNombreNuevoProveedor").focus();
}

function ImprimirTotalToneladas(ds) {
    if (ds.length > 0) {
        var totalToneladas = 0;
        for (var i = 0; i < ds.length; i++) {
            totalToneladas = totalToneladas + ds[i].Peso;
        }

        if ($("#language").val() === "es-MX")
            $("#lblEmbarqueCargaToneladasCargadas").css('width', '87%');
        else
            $("#lblEmbarqueCargaToneladasCargadas").css('width', '52%');

        totalToneladas = totalToneladas / 1000;
        $("#lblEmbarqueCargaToneladasCargadas").css('text-align', 'right');
        $("#lblEmbarqueCargaToneladasCargadas").text(totalToneladas != 0 ? kendo.toString(totalToneladas, 'n') : "");
    } else {
        $("#lblEmbarqueCargaToneladasCargadas").text("");
    }
}

function ImprimirTotalPiezas(ds) {
    if (ds.length > 0) {
        $("#lblEmbarqueCargaTotalPiezas").css('text-align', 'center');
        $("#lblEmbarqueCargaTotalPiezas").text(ds.length != 0 ? ds.length : "");
    } else {
        $("#lblEmbarqueCargaTotalPiezas").text("");
    }
}

function ObtieneConsecutivo() {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var cont = 1;
    if (ds._data.length > 0) {
        for (var i = 0; i < ds._data.length; i++) {
            ds._data[i].Consecutivo = cont;
            cont++;
        }
    }

    ds.sync();
}

function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function ExisteSpool(row) {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].SpoolID == row.SpoolID) {
            return true
        }
    }
    return false;

}

function ExistePaquete(PaqueteID) {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].PaqueteID == PaqueteID) {
            return true
        }
    }
    return false;
}