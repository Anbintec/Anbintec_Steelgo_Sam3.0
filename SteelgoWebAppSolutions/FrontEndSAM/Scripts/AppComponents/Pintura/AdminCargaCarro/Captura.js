var editado = false;
var ventanaConfirmEdicion;
var windowNewCarriage;

var filaSeleccionada;
var CuadranteSpoolAnterior;
var proyectoActualSeleccionado;
var carroActualSeleccionado;

function PlanchaSeleccionado(opcionSeleccionada) {

    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            if (opcionSeleccionada == "Si") {
                data[i].Seleccionado = true;

            } else if (opcionSeleccionada == "No") {
                data[i].Seleccionado = false;
            }
        } else if ($('input:radio[name=LLena]:checked').val() === "Vacios") {
            if (!data[i].Seleccionado) {
                if (opcionSeleccionada == "Si") {
                    data[i].Seleccionado = true;
                }
            }
        }
    }
    $("#grid").data("kendoGrid").refresh();
}

function InsertarNuevoElemento() {
    if (!$('#chkCerrar').is(':checked')) {
        if ($('input:radio[name=TipoSeleccion]:checked').val() == "Spool" && ($("#InputID").val() == "" || $("#InputID").val() == "0")) {
            displayNotify("PinturaCargaSeleccionaSpool", "", '1');
        }
        else if ($('input:radio[name=TipoSeleccion]:checked').val() == "Spool" && $("#InputID").val() != "") {
            var valorBusqueda = $('input:radio[name=TipoSeleccion]:checked').val() == "Spool" ? $("#InputID").val() + '~' + "Spool" : $("#inputCodigo").val() + '~' + "Codigo";
            if (proyectoActualSeleccionado != undefined && proyectoActualSeleccionado != null && proyectoActualSeleccionado.ProyectoID != 0) {
                AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), valorBusqueda);
            }
            else
                displayNotify("SistemaPinturaMensajeErrorProyecto", "", '1');
        }
        else if ($('input:radio[name=TipoSeleccion]:checked').val() == "Codigo" && $("#inputCodigo").val() != "") {
            var valorBusqueda = $('input:radio[name=TipoSeleccion]:checked').val() == "Spool" ? $("#InputID").val() + '~' + "Spool" : $("#inputCodigo").val() + '~' + "Codigo";
            if (proyectoActualSeleccionado != undefined && proyectoActualSeleccionado != null && proyectoActualSeleccionado.ProyectoID != 0) {
                AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), valorBusqueda);
            }
            else
                displayNotify("SistemaPinturaMensajeErrorProyecto", "", '1');
        }
        else if ($('input:radio[name=TipoSeleccion]:checked').val() == "Codigo" && $("#inputCodigo").val() == "") {
            displayNotify("PinturaIngresaCodigoValido", "", '1');
        }
    }
    else {
        displayNotify("PinturaCargaCarroCerrar", "", '1');
    }
}

function Limpiar() {

    $("#labelM2").text("");
    $("#inputCarro").data("kendoComboBox").dataSource.data([]);
    $("#inputCarro").data("kendoComboBox").value("");
    $("#labelToneladas").text("");
    $("#grid").data('kendoGrid').dataSource.data([]);
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#inputCodigo").val("");
    $("#inputProyecto").data("kendoComboBox").value("");
    $("#chkCerrar")[0].checked = false;
    $("#grid").data('kendoGrid').dataSource.data([]);

    opcionHabilitarView(false, "FieldSetView");
}

function changeLanguageCall() {
    SuscribirEventos();
    CargaGrid();
    AjaxCargarCamposPredeterminados();
    AjaxCargarProyecto();
    document.title = _dictionary.PinturaHeaderCargaCarro[$("#language").data("kendoDropDownList").value()];
}

function ServicioPinturaCorrecto(ListaDetalles) {
    var sistemaID;
    for (var i = 0 ; i < ListaDetalles.length ; i++) {
        if (i == 0) {
            sistemaID = ListaDetalles[0].SistemaPinturaID;
        }
        if (sistemaID != ListaDetalles[i].SistemaPinturaID) {
            return false;
        }
    }
    return true;
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

function LimpiarCargaCarro() {
    $("#grid").data('kendoGrid').dataSource.data([]);

    $("#labelM2").text("");
    $("#labelToneladas").text("");

}

function LimpiarInformacionAgregada() {
    $("#grid").data('kendoGrid').dataSource.data([]);
    $("#labelM2").text("");
    $("#labelToneladas").text("");
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#inputCodigo").val("");
}

function LimpiarCargaProyecto() {
    $("#inputCarro").data("kendoComboBox").dataSource.data([]);
    $("#inputCarro").data("kendoComboBox").value("");
    $("#labelM2").text("");
    $("#labelToneladas").text("");
    $("#grid").data('kendoGrid').dataSource.data([]);
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#inputCodigo").val("");

}

function LimpiarCambioEscenario() {
    $("#inputProyecto").data("kendoComboBox").value("");

    $("#inputCarro").data("kendoComboBox").dataSource.data([]);
    $("#inputCarro").data("kendoComboBox").value("");

    $("#labelM2").text("");
    $("#labelToneladas").text("");
    $("#chkCerrar")[0].checked = false;
    $("#grid").data('kendoGrid').dataSource.data([]);

}

function CargaGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {
            if ($('#Guardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            }
            else {
                var inputName = e.container.find('input');
                inputName.select();
            }
        },
        autoBind: true,
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        MedioTransporteCargaDetalleID: { type: "number", editable: false },
                        OrdenTrabajoID: { type: "number", editable: false },
                        SpoolID: { type: "number", editable: false },
                        Prioridad: { type: "number", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        SistemaPinturaID: { type: "number", editable: false },
                        SistemaPintura: { type: "string", editable: false },
                        ColorPintura: { type: "string", editable: false },
                        CuadranteID: { type: "number", editable: false },
                        CuadranteAnteriorID: { type: "number", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Area: { type: "number", editable: false },
                        Peso: { type: "number", editable: false },
                        MedioTransporte: { type: "string", editable: false },
                        Seleccionado: { type: "boolean", editable: false },
                        EstatusCaptura: { type: "number", editable: false },
                        EstatusCarga: { type: "boolean", editable: false },
                        PEQS: { type: "number", editable: false },
                        PDIS: { type: "number", editable: false },
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: undefined }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            aggregate: [
                { field: "Area", aggregate: "sum" },
                { field: "Peso", aggregate: "sum" },
                { field: "PEQS", aggregate: "sum" },
                { field: "PDIS", aggregate: "sum" }

            ]
        },
        navigatable: true,
        editable: false,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        selectable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "Prioridad", title: _dictionary.columnPrioridad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" } },
            { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "SistemaPintura", title: _dictionary.columnSistemaPintura[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "ColorPintura", title: _dictionary.columnColor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), template: "<span>  #= ColorPintura==''||ColorPintura==null ? 'N/A' : ColorPintura #</span></div>", attributes: { style: "text-align:center;" } },
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },

            { field: "PEQS", title: _dictionary.AutorizacionPeticionHeaderPeqs[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" }, aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>" },
            { field: "PDIS", title: _dictionary.lblPdi[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" }, aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>" },

            { field: "Area", title: _dictionary.columnM2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" }, aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>" },
            { field: "Peso", title: _dictionary.columnPeso[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), attributes: { style: "text-align:right;" }, aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>" },
            { field: "MedioTransporte", title: _dictionary.columnMedioTransporte[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            {
                field: "Seleccionado", title: _dictionary.columnSeleccionado[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Seleccionado: true }, { Seleccionado: false }]
                }, template: '<input  #= Seleccionado ? "checked=checked" : "" # class="chkbx" #= Accion==2? "type=hidden":"type=checkbox" #  ></input>', attributes: { style: "text-align:center;" }
            },
            { command: { text: _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnDescargar[$("#language").data("kendoDropDownList").value()], attributes: { style: "text-align:center;" } }
        ],
        dataBound: function (e) {
            var ds = $("#grid").data("kendoGrid");
            var gridData = ds.dataSource.view();

            if (gridData.length > 0) {
                for (var i = 0; i < gridData.length; i++) {
                    var currentUid = gridData[i].uid;
                    var currenRow = ds.table.find("tr[data-uid='" + currentUid + "']");
                    var editButton = $(currenRow).find(".k-button");
                    if (gridData[i].Accion == 2) {
                        var classDescarga = $("#language").val() == "es-MX" ? "k-grid-Descarga" : "k-grid-Discharging";
                        editButton[0].outerHTML = '<a class="k-button k-button-icontext ' + classDescarga + '" href="#/"><span class=""></span>' +
                            _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()] + '</a>';

                    } else if ($('input:radio[name=TipoVista]:checked').val() == "Patio") {
                        editButton[0].outerHTML = '<a class="k-button k-button-icontext k-grid-Cancelar" href="#/"><span class=""></span>' +
                            _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()] + '</a>';
                    }
                    else {
                        editButton[0].outerHTML = '';
                    }
                }
            }

        }
    });

    CustomisaGrid($("#grid"));

    $("#grid .k-grid-content").on("change", "input.chkbx", function (e) {
        if ($('#Guardar').text() != "Editar") {
            if (!$('#chkCerrar').is(':checked')) {
                var grid = $("#grid").data("kendoGrid")
                dataItem = grid.dataItem($(e.target).closest("tr"));
                dataItem.Seleccionado = $(this)[0].checked
            }
            else {
                displayNotify("PinturaCargaCarroCerrar", "", '1');

                if ($(this)[0].checked) {
                    $(this)[0].checked = false;
                }
                else {
                    $(this)[0].checked = true;
                }
            }
        }
        else {
            if ($(this)[0].checked) {
                $(this)[0].checked = false;
            }
            else {
                $(this)[0].checked = true;
            }
        }
    });
}

function eliminarCaptura(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()] && !$("#chkCerrar")[0].disabled) {
        if (!$("#chkCerrar")[0].checked) {
            var filterValue = $(e.currentTarget).val();
            dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
            AjaxCargarZona($("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).PatioID, dataItem);
            CuadranteSpoolAnterior = dataItem.CuadranteAnteriorID;
        }
        else {
            displayNotify("PinturaCargaCarroCerrar", "", '1');
        }
    }
}

function CargaPopupNuevoMedioTransporte(e) {
    $("#InputNombre").val("");

    windowNewCarriage = $("#divNuevoMedioTransporte").kendoWindow({
        iframe: false,
        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "502px",
        height: "auto",
        modal: true,
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        actions: [],
        close: function () {
            $("#inputCarro").data("kendoComboBox").value("");
            $("#InputNombre").blur();
        }
    }).data("kendoWindow");

    $("#divNuevoMedioTransporte").data("kendoWindow").title(_dictionary.PinturaCargaNuevoCarro[$("#language").data("kendoDropDownList").value()]);
    $("#divNuevoMedioTransporte").data("kendoWindow").center().open();

    $("#InputNombre").focus();
}

function ImprimirAreaTonelada() {
    var ds = $("#grid").data("kendoGrid").dataSource;
    var array = ds._data;
    var totalAreaCargada = 0;
    var totalToneladasCargadas = 0;
    var curr_filters = ds.filter().filters;

    if (ds._data.length > 0) {

        for (var i = 0; i < array.length; i++) {
            if (curr_filters.length > 1) {
                if (array[i]["Accion"] == 1 || array[i]["Accion"] == 2) {
                    totalAreaCargada += parseFloat(array[i]["Area"], 10);
                    totalToneladasCargadas += parseFloat(array[i]["Peso"], 10);
                }
            } else {
                if (array[i]["Accion"] == 1) {
                    totalAreaCargada += parseFloat(array[i]["Area"], 10);
                    totalToneladasCargadas += parseFloat(array[i]["Peso"], 10);
                }
            }
        }

        totalToneladasCargadas = totalToneladasCargadas / 1000;

        $("#labelM2").css('text-align', 'left');
        $("#labelToneladas").css('text-align', 'left');

        $("#labelM2").text(totalAreaCargada != 0 ? totalAreaCargada.toFixed(2) : "");
        $("#labelToneladas").text(totalToneladasCargadas != 0 ? totalToneladasCargadas.toFixed(2) : "");
    }

    //return totalAreaCargada;
}

function validarInformacion(row) {
    //solo para patio.
    var ds = $("#grid").data("kendoGrid").dataSource;
    var existe = false;

    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i]["SpoolID"] == row.SpoolID && ds._data[i]["Accion"] != 3) {
            existe = true;
            break;
        }
    }
    return existe;
}