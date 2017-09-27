IniciarCapturaEmpaquetado();
var FechaPaquete;
var esNormal;

function IniciarCapturaEmpaquetado() {
    SuscribirEventos();
}
function changeLanguageCall() {
    CargarGrid();
    AjaxCargarCamposPredeterminados();
    AjaxCargarProyectos();
    document.title = _dictionary.EmbarqueEmpaquetadoTituloPagina[$("#language").data("kendoDropDownList").value()];
    FechaPaquete.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });

    opcionHabilitarView(false, "FieldSetView");
};


function CargarGrid() {
    $("#grid").kendoGrid({
        autoBind: true,
        edit: function (e) {
            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        Consecutivo: { type: "number", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Area: { type: "number", editable: false },
                        Peso: { type: "number", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            aggregate: [
                { field: "Area", aggregate: "sum" },
                { field: "Peso", aggregate: "sum" }
            ]
        },
        navigatable: true,
        filterable: {
            extra: false
        },
        editable: true,
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
            { field: "Consecutivo", title: _dictionary.columnConsecutivoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "150px", attributes: { style: "text-align:right;" } },
            { field: "NumeroControl", title: _dictionary.columnSpool[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Area", title: _dictionary.columnM2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n3') #</div>", attributes: { style: "text-align:right;" } },
            { field: "Peso", title: _dictionary.columnPeso[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n3') #</div>", attributes: { style: "text-align:right;" } },
            { command: { text: _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()], click: descargaSpool }, title: _dictionary.columnDescargar[$("#language").data("kendoDropDownList").value()], width: "70px", attributes: { style: "text-align:center;" } }
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

                        editButton[0].outerHTML = '<a class="k-button k-button-icontext '+classDescarga+'" href="#/"><span class=""></span>' +
                            _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()] + '</a>';
                    } else {
                        editButton[0].outerHTML = '<a class="k-button k-button-icontext k-grid-Cancelar" href="#/"><span class=""></span>' +
                            _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()] + '</a>';
                    }
                }
            }

            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }
        }
    });
    CustomisaGrid($("#grid"));
};

function descargaSpool(e) {
    e.preventDefault();
    
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

            $("#InputZonaDescarga").data("kendoComboBox").value(dataItem.ZonaAnteriorID == 0 && dataItem.CuadranteAnteriorSam3ID != 0 ? 1 : dataItem.ZonaAnteriorID);
            $("#InputZonaDescarga").data("kendoComboBox").trigger("change");
            CuadranteAnterior = dataItem.CuadranteAnteriorSam3ID;
            $("#InputUidRow").val(dataItem.uid);

            windowDownload.title(_dictionary.EmbarqueCargaTituloPopupCuadrante[$("#language").data("kendoDropDownList").value()]);
            windowDownload.open().center();
        }
}

function existenCambios() {
    var ds = $("#grid").data("kendoGrid").dataSource._data;
    if (ds.length > 0) {
        for (var i = 0; i < ds.length; i++) {
            if (ds[i].ModificadoPorUsuario)
                return true;
        }

    }
    return false;
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

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        $("#InputFechaPaquete").data("kendoDatePicker").value('');
    }
}

function ImprimirTotalToneladas(ds) {
    if (ds != null) {
        var totalToneladas = 0;
        for (var i = 0; i < ds.length; i++) {

            totalToneladas = totalToneladas + ds[i].Peso;
        }
        if ($("#language").val() === "es-MX")
            $("#TotalToneladas").css('width', '55%');
        else
            $("#TotalToneladas").css('width', '34%');

        totalToneladas = totalToneladas / 1000;

        $("#TotalToneladas").css('text-align', 'right');
        $("#TotalToneladas").text(totalToneladas != 0 ? kendo.toString(totalToneladas, 'n3') : "");
    } else {
        $("#TotalToneladas").text("");
    }
}

function ImprimirTotalPiezas(ds) {
    if (ds != null) {
        $("#TotalPiezas").css('text-align', 'center');
        $("#TotalPiezas").text(ds.length != 0 ? ds.length : "");
    } else {
        $("#TotalPiezas").text("");
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