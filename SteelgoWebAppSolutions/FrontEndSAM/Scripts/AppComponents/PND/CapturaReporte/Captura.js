var modeloRenglon;
var modeloRenglonPlaca;
var editado = false;
var esNormal;




function changeLanguageCall() {
    CargarGrid();
    CargarGridPopUpDetallePorPlaca();
    CargarGridPopUpDetallePorPlacaPorDefectos();

    var paramReq = getParameterByName('requisicion');
    var requisicionID = getParameterByName('requisicion');
    if (requisicionID != null) {
        inicio();
        AjaxObtenerElementoRequisicion(requisicionID)
    } else {
        inicio();
    }
    document.title = _dictionary.menuServiciosTecnicosReporteRT[$("#language").data("kendoDropDownList").value()];
};

function inicio() {
    SuscribirEventos();
    AjaxCargarCamposPredeterminados();
}

function getParameterByName(name, url) {

    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function validarReglasDeLlenado() {
    //return true;
    //for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
    //    if ($("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas.length > 0) {
    //        for (var j = 0; j < $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas.length; j++) {

    //            $('tr[data-uid="' + $("#grid").data("kendoGrid").dataSource._data[i].uid + '"] ').css("background-color", "#ffffff");

    //        }
    //    }
    //    else {
    //        $('tr[data-uid="' + $("#grid").data("kendoGrid").dataSource._data[i].uid + '"] ').css("background-color", "#ffcccc");
    //    }
    //}
    return true;
}

function CargarGrid() {

    kendo.ui.Grid.fn.editCell = (function (editCell) {
        return function (cell) {
            cell = $(cell);

            var that = this,
                column = that.columns[that.cellIndex(cell)],
                model = that._modelForContainer(cell),
                event = {
                    container: cell,
                    model: model,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

            if (model && typeof this.options.beforeEdit === "function") {
                this.options.beforeEdit.call(this, event);
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);
    $("#grid").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        OrdenTrabajoID: { type: "number", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        SpoolID: { type: "number", editable: false },
                        JuntaSpoolID: { type: "number", editable: false },
                        Junta: { type: "string", editable: false },
                        ClasificacionPND: { type: "string", editable: false },
                        TipoPrueba: { type: "string", editable: false },
                        Observaciones: { type: "string", editable: false },
                        CodigoAsme: { type: "string", editable: false },
                        NumeroPlacas: { type: "number", editable: true },
                        Resultado: { type: "string", editable: true },
                        ResultadoConciliacion: { type: "string", editable: false, hidden: true },
                        RazonNoConciliacion: { type: "string", editable: false, hidden: true },
                        TemplateDetalleElemento: { type: "string", editable: false },
                        EsSector: { type: "boolean", editable: false },
                        NombreGrafica: { type: "string", editable: true }
                    }
                }
            },
            pageSize: 10,
        },
        edit: function (e) {
            editado = true;
            if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            };

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
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
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "NumeroControl", title: _dictionary.CapturaReporteGridColumnSpoolJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Junta", title: _dictionary.CapturaReporteGridColumnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "65px" },
            { field: "ClasificacionPND", title: _dictionary.CapturaReporteGridColumnClasificacionPND[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "73px" },
            { field: "TipoPrueba", title: _dictionary.CapturaReporteGridColumnTipoPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "92px" },
            { field: "Observaciones", title: _dictionary.CapturaReporteGridColumnObservaciones[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "CodigoAsme", title: _dictionary.CapturaReporteGridColumnCodigoAsme[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Equipo", title: "Equipo", filterable: getGridFilterableCellNumberMaftec(), width: "100px", editor: RenderEquipo, filterable: getGridFilterableCellMaftec() },
            { field: "Turno", title: "Turno", filterable: getGridFilterableCellNumberMaftec(), width: "85px", editor: RenderTurno, filterable: getGridFilterableCellMaftec() },
            {
                field: "EsSector", title: "Evaluación", width: "120px", filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:140px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }],
                },
                template:
                    "<input class='RadioSector' value='sector' style=' display:inline-block; width:33% !important;'  type='radio' name='tipoEvaluacion#= NumeroControl##=Junta#' #= EsSector ? checked='checked' : '' #>"
                        + "<label style='display:inline-block;   width:70% !important;'>Sector</label><br>"
                    + "<input class='RadioSector' value='cuadrante' style='display:inline-block; width:33% !important;' type='radio' name='tipoEvaluacion#= NumeroControl##=Junta#'  #= !EsSector ? checked='checked' : '' #> "
                        + "<label style='display:inline-block;  width:70% !important;'>Cte</label>"
            },
            { field: "NombreGrafica", title: _dictionary.CapturaReportePruebasHeaderNombreGrafica[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), filterable: getGridFilterableCellMaftec(), width: "100px", },
            { field: "NumeroPlacas", title: _dictionary.CapturaReporteGridColumnNumeroPlacas[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px", editor: RenderNumeroPlacas, attributes: { style: "text-align:right;" } },
            { field: "TemplateDetalleElemento", title: _dictionary.CapturaReporteGridColumnInformacionResultados[$("#language").data("kendoDropDownList").value()], filterable: false, width: "100px"/*, editor: RenderGridDetallePorPlaca*/, template: "<div class='EnlacePorPlaca' style='text-align:center;'><a href='\\#'  > <span>#=TemplateDetalleElemento#</span></a></div> " },
            { field: "Resultado", title: _dictionary.CapturaReportePruebasHeaderResultado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), filterable: getGridFilterableCellMaftec(), width: "120px", editor: comboBoxResultadoDetalleElemento },
            { field: "Indicacion", title: _dictionary.CapturaReporteGridColumnIndicacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "120px", editor: comboBoxIndicacion, filterable: getGridFilterableCellMaftec() },
            { field: "ResultadoConciliacion", title: _dictionary.CapturaReporteGridColumnRusult1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "RazonNoConciliacion", title: _dictionary.CapturaReporteGridColumnRusult2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
        ],
        dataBound: function (a) {
            $(".RadioSector").bind("change", function (e) {
                if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
                    var grid = $("#grid").data("kendoGrid");
                    dataItem = grid.dataItem($(e.target).closest("tr"));


                    if (dataItem != null) {

                        if (dataItem.NumeroPlacas != null) {

                            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                                iframe: true,
                                title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
                                visible: false, //the window will not appear before its .open method is called
                                width: "auto",
                                height: "auto",
                                modal: true,
                                animation: {
                                    close: false,
                                    open: false
                                }
                            }).data("kendoWindow");

                            ventanaConfirm.content(_dictionary.CapturaReporteModificarNoPlacas[$("#language").data("kendoDropDownList").value()] +
                                        "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

                            ventanaConfirm.open().center();

                            $("#yesButton").click(function () {

                                if (e.target.value == "sector" && e.target.checked == true) {

                                    dataItem.EsSector = true;
                                }
                                else if (e.target.value == "cuadrante" && e.target.checked == true) {

                                    dataItem.EsSector = false;
                                }
                                dataItem.NumeroPlacas = null;
                                $("#grid").data("kendoGrid").dataSource.sync();
                                ventanaConfirm.close();
                            });
                            $("#noButton").click(function () {

                                if (e.target.value == "sector" && e.target.checked == true) {
                                    e.target.value = "cuadrante"
                                    e.target.checked == false;
                                    dataItem.EsSector = false;
                                }
                                else if (e.target.value == "cuadrante" && e.target.checked == true) {
                                    e.target.value = "sector"
                                    e.target.checked == false;
                                    dataItem.EsSector = true;
                                }
                                $("#grid").data("kendoGrid").dataSource.sync();
                                ventanaConfirm.close();
                            });
                        }
                        else if (dataItem.NumeroPlacas == null) {
                            if (e.target.value == "sector" && e.target.checked == true) {

                                dataItem.EsSector = true;
                            }
                            else if (e.target.value == "cuadrante" && e.target.checked == true) {

                                dataItem.EsSector = false;
                            }
                            dataItem.NumeroPlacas = null;
                            $("#grid").data("kendoGrid").dataSource.sync();
                        }

                    }
                }
                else
                    $("#grid").data("kendoGrid").closeCell();

            });


            var grid = $("#grid").data("kendoGrid");
            var gridData = grid.dataSource.view();

            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].RowOk == false) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");

                }
                else if (gridData[i].RowOk) {
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
        editable: true,
        navigatable: true,
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },
    });
    CustomisaGrid($("#grid")); 5
};

function isEditable(fieldName, model) {

    if (fieldName === "Resultado") {
        if ($("#inputPrueba").data("kendoComboBox").text().indexOf("RT") === -1) {
            return true;
        }
    }

    if (fieldName === "Indicacion") {
        if (model.Defecto == _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
            return false;
        }
    }

    if (fieldName === "Equipo") {
        var respuesta = $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura;
        return respuesta;
    }
    if (fieldName === "Turno") {
        if (model.Equipo == "")
            if ($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura)
                return false;
            else
                return true;
        else
            return true;
    }
    return true; // default to editable
}

function CargarGridPopUpDetallePorPlaca() {

    kendo.ui.Grid.fn.editCell = (function (editCell) {
        return function (cell) {
            cell = $(cell);

            var that = this,
                column = that.columns[that.cellIndex(cell)],
                model = that._modelForContainer(cell),
                event = {
                    container: cell,
                    model: model,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

            if (model && typeof this.options.beforeEdit === "function") {
                this.options.beforeEdit.call(this, event);
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);
    $("#gridPopUp").kendoGrid({
        ////autoBind: true,
        dataSource: {
            data: [],//options.model.ListaDetallePorPlacas,
            schema: {
                model: {
                    fields: {
                        Ubicacion: { type: "string", editable: false },
                        Resultado: { type: "String", editable: true },
                        ResultadoConciliacion: { type: "string", editable: false, hidden: true },
                        RazonNoConciliacion: { type: "string", editable: false, hidden: true },
                        TemplateDetallePorPlaca: { type: "String", editable: false },
                    }
                }
            },
            pageSize: 10,
        },
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          { field: "Ubicacion", title: _dictionary.ValidacionResultadosCabeceraUbicacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "10px" },
          { field: "Resultado", title: _dictionary.CapturaReportePruebasHeaderResultado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: comboBoxResultadoDetallePlaca, width: "10px" },
          { field: "ResultadoConciliacion", title: _dictionary.CapturaReporteGridColumnRusult1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "10px" },
            { field: "RazonNoConciliacion", title: _dictionary.CapturaReporteGridColumnRusult2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "10px" },
          { field: "TemplateDetallePorPlaca", title: "Indicaciones", filterable: false, width: "10px", template: "<div class='EnlaceDefectoPorPlaca' style='text-align:center;'><a href='\\#'  > <span>#=TemplateDetallePorPlaca#</span></a></div> " }
        ],
        editable: true,

        navigatable: true,
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditablePlaca(fieldName, e.model)) {
                e.preventDefault();
            }
        },
    });
    CustomisaGrid($("#gridPopUp"));


};

function isEditablePlaca(fieldName, model) {
    if (fieldName === "Resultado") {
        var respuesta;
        var dataItem;
        var Resultados = obtenerResultadoDefecto(model.ListaDetalleDefectos);
        if (Resultados[0].Resultado == "" || Resultados[0].Resultado != _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
            respuesta = true;
        }
        else
            respuesta = false;

        return respuesta;
    }

    return true; // default to editable
}

function CargarGridPopUpDetallePorPlacaPorDefectos() {

    $("#gridPopUpDefectos").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Defecto: { type: "string", editable: true },
                        InicioMM: { type: "number", editable: true },
                        FinMM: { type: "number", editable: true },
                        Cuadrante: { type: "string", editable: true },
                        Resultado: { type: "string", editable: true },

                    }
                }
            }, filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                    { field: "Accion", operator: "eq", value: 0 },
                    { field: "Accion", operator: "eq", value: undefined }
                ]
            },
            pageSize: 10,
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
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
                { field: "Defecto", title: "Indicación", filterable: getGridFilterableCellMaftec(), editor: comboBoxDefectos },
                { field: "Resultado", title: "Resultado", filterable: getGridFilterableCellMaftec(), editor: comboBoxResultadoDetalleDefecto },
                { field: "InicioMM", title: _dictionary.CapturaReportePruebasHeaderInicio[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), editor: RenderInicioMM, attributes: { style: "text-align:right;" } },
                { field: "FinMM", title: _dictionary.CapturaReportePruebasHeaderFin[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), editor: RenderFinMM, attributes: { style: "text-align:right;" } },
                { field: "Cuadrante", title: "Cte", filterable: getGridFilterableCellMaftec(), editor: RenderCuadrantes },
                //{ field: "FinCuadrante", title: "Cte Fin", filterable: getGridFilterableCellMaftec(), editor: RenderCuadrantes },
                //{ field: "Soldador", title: "Soldador", filterable: getGridFilterableCellMaftec(),  },
                {
                    command: {
                        text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                        click: function (e) {
                            e.preventDefault();
                            //var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                            var dataItem = $("#gridPopUpDefectos").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

                            if ((dataItem.Accion == 2))
                                dataItem.Accion = 3;
                            else
                                $("#gridPopUpDefectos").data("kendoGrid").dataSource.remove(dataItem);

                            $("#gridPopUpDefectos").data("kendoGrid").dataSource.sync();
                        }
                    },
                    title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
                    width: "50px",
                    attributes: { style: "text-align:center;" }
                },
            {
                command: {
                    text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()],
                    click: function (e) {
                        e.preventDefault();
                        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {

                            var itemToClean = $("#gridPopUpDefectos").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

                            if (itemToClean.Accion == 2)
                                itemToClean.Accion = 4;


                            if (itemToClean.DefectoID != "") {
                                itemToClean.DefectoID = 0;
                            }

                            itemToClean.Defecto = "";
                            itemToClean.InicioMM = "";
                            itemToClean.FinMM = "";
                            itemToClean.Cuadrante = "";
                            itemToClean.RangoCuadranteID = 0;
                            itemToClean.Resultado = "";
                            itemToClean.ResultadoID = 0;

                            var dataSource = $("#gridPopUpDefectos").data("kendoGrid").dataSource;
                            dataSource.sync();
                            //alert(itemToClean);
                        }
                    }
                },
                title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()],
                width: "50px",
                attributes: { style: "text-align:center;" }
            }
        ],
        editable: "incell",
        toolbar: [{ name: "create" }]

    });
    CustomisaGrid($("#gridPopUpDefectos"));

};

function LlenarGridPopUpDetallePlaca(data) {
    modeloRenglon = data;

    $("#gridPopUp").data('kendoGrid').dataSource.data([]);
    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    var array = data.ListaDetallePorPlacas;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    VentanaModalDetallePlaca();
}

function LlenarGridPopUpDetalleDefectoPorPlaca(data) {
    modeloRenglonPlaca = data;
    $("#gridPopUpDefectos").data('kendoGrid').dataSource.data([]);

    if ($("#gridPopUpDefectos").data('kendoGrid').dataSource._filter == undefined) {
        //investigar porque al destruir una ventana se eliminan solo los filtros.
        $("#gridPopUpDefectos").data('kendoGrid').dataSource._filter = {
            logic: "or",
            filters: [
              { field: "Accion", operator: "eq", value: 1 },
              { field: "Accion", operator: "eq", value: 2 },
                { field: "Accion", operator: "eq", value: 0 },
                { field: "Accion", operator: "eq", value: undefined }
            ]
        };
    }

    var ds = $("#gridPopUpDefectos").data("kendoGrid").dataSource;
    var array = data.ListaDetalleDefectos;
    listaDefectosAuxiliar = data.ListaDefectos;
    for (var i = 0; i < array.length; i++) {
        ds.add(array[i]);
    }
    $("#gridPopUpDefectos").data('kendoGrid').dataSource.sync();
    VentanaModalDetalleDefectoPorPlaca();
}

function VentanaModalDetallePlaca() {

    var modalTitle = "";
    if (modeloRenglon.EsSector)
        modalTitle = "Sector";
    else
        modalTitle = "Cuadrante";

    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: "DetallePlaca",
        resizable: false,
        visible: true,
        width: "60%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        actions: [],
        //close: function onClose(e) {
        //    var gridDataSource = $("#gridPopUp").data("kendoGrid").dataSource;
        //    gridDataSource.filter([]);
        //}
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

function VentanaModalDetalleDefectoPorPlaca() {

    var modalTitle = "";
    modalTitle = "Detalle Indicaciones";

    if (modeloRenglon.EsSector) {
        $("#gridPopUpDefectos").data("kendoGrid").showColumn("InicioMM");
        $("#gridPopUpDefectos").data("kendoGrid").showColumn("FinMM");
        $("#gridPopUpDefectos").data("kendoGrid").hideColumn("Cuadrante");
        //$("#gridPopUpDefectos").data("kendoGrid").hideColumn("FinCuadrante");
    }
    else {
        $("#gridPopUpDefectos").data("kendoGrid").hideColumn("InicioMM");
        $("#gridPopUpDefectos").data("kendoGrid").hideColumn("FinMM");
        $("#gridPopUpDefectos").data("kendoGrid").showColumn("Cuadrante");
        //$("#gridPopUpDefectos").data("kendoGrid").showColumn("FinCuadrante");
    }


    var window = $("#windowGridDefectos");
    var win = window.kendoWindow({
        modal: true,
        title: "DetalleDefectos",
        resizable: false,
        visible: true,
        width: "60%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        content: "texto texto texto y mas texto",
        actions: [],
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

var listaDetallePorPlaca = [];
function actualizaGridGeneralPorPlaca() {
    //currentPlaca
    var itera = 0;
    var datosCompletos = true;
    listaDetallePorPlaca = [];

    var dataSourceDefectos = $("#gridPopUp").data("kendoGrid").dataSource;
    var filters = dataSourceDefectos.filter();
    var allData = dataSourceDefectos.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
        for (var j = 0; j < data.length; j++) {
            if ($("#grid").data("kendoGrid").dataSource._data[i].ElementoPorClasificacionPNDID == data[j].ElementoPorClasificacionPNDID) {
                listaDetallePorPlaca[itera] = {
                    CapturaResultadoID: "",
                    CapturaResultadoPlacaID: "",
                    Resultado: "",
                    ResultadoID: "",
                    Ubicacion: "",
                    ElementoPorClasificacionPNDID: "",
                    ListaDetalleDefectos: "",
                    TemplateDetallePorPlaca: "",
                    ListaResultados: "",
                    ListaDefectos: ""

                };
                listaDetallePorPlaca[itera].Accion = data[j].Accion == 0 ? 1 : data[j].Accion;
                listaDetallePorPlaca[itera].CapturaResultadoPlacaID = data[j].CapturaResultadoPlacaID;
                listaDetallePorPlaca[itera].CapturaResultadoID = data[j].CapturaResultadoID;
                listaDetallePorPlaca[itera].Resultado = data[j].Resultado;
                listaDetallePorPlaca[itera].ResultadoID = data[j].ResultadoID;
                listaDetallePorPlaca[itera].Ubicacion = data[j].Ubicacion;
                listaDetallePorPlaca[itera].ElementoPorClasificacionPNDID = modeloRenglon.ElementoPorClasificacionPNDID;
                listaDetallePorPlaca[itera].ListaDetalleDefectos = data[j].ListaDetalleDefectos;
                listaDetallePorPlaca[itera].ListaResultados = data[j].ListaResultados;
                listaDetallePorPlaca[itera].ListaDefectos = data[j].ListaDefectos;
                listaDetallePorPlaca[itera].TemplateDetallePorPlaca = data[j].TemplateDetallePorPlaca;



                itera++;

                if ((data[j].ResultadoID != null) && (data[j].ResultadoID != 0)) {
                    $('tr[data-uid="' + $("#gridPopUp").data("kendoGrid").dataSource._data[j].uid + '"] ').css("background-color", "#ffffff");
                }
                else {
                    $('tr[data-uid="' + $("#gridPopUp").data("kendoGrid").dataSource._data[j].uid + '"] ').css("background-color", "#ffcccc");
                    datosCompletos = false;
                }

                if (datosCompletos) {
                    var Resultados = obtenerResultadoDefecto(listaDetallePorPlaca);
                    $("#grid").data("kendoGrid").dataSource._data[i].ResultadoID = Resultados[0].ResultadoID;
                    $("#grid").data("kendoGrid").dataSource._data[i].Resultado = Resultados[0].Resultado;
                    $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas = listaDetallePorPlaca;
                    hayDatosCapturados = true;
                    $("#windowGrid").data("kendoWindow").close();
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
            }

        }

    }
    if (!datosCompletos) {
        displayNotify("CapturaReporteValidacionErroneaDefecto", "", '2');
    }
}

var validacionCorrecta = false;
var listaDetalleDefectos = [];
function actualizaGridGeneralPorDefectos() {
    //currentDefectosPorPlaca
    listaDetalleDefectos = [];

    //filtra solo los registros diferentes a status 3 que es eliminar.
    var data = $("#gridPopUpDefectos").data("kendoGrid").dataSource._data;
    //var filters = dataSourceDefectos.filter();
    //var allData = dataSourceDefectos.data();
    //var query = new kendo.data.Query(allData);
    //var data = query.filter(filters).data;

    var itera = 0;
    validacionCorrecta = true;
    //buscar la manera de pasar un valor de una ventana modal a otra para saber de que placa se esta agregando el defecto.
    for (var i = 0; i < $("#gridPopUp").data("kendoGrid").dataSource._data.length; i++) {
        for (var j = 0; j < data.length; j++) {
            if ($("#gridPopUp").data("kendoGrid").dataSource._data[i].ElementoPorClasificacionPNDID == modeloRenglonPlaca.ElementoPorClasificacionPNDID && $("#gridPopUp").data("kendoGrid").dataSource._data[i].Ubicacion == modeloRenglonPlaca.Ubicacion) {
                if (((data[j].DefectoID != null) && (data[j].DefectoID != 0) && ($.isNumeric(data[j].InicioMM)) && ($.isNumeric(data[j].FinMM))) || (data[j].Accion != 3 || data[j].Accion != 4)) {

                    listaDetalleDefectos[itera] = {
                        CapturaResultadoPlacaDefectoID: "",
                        CapturaResultadoPlacaID: "",
                        DefectoID: "",
                        Defecto: "",
                        InicioMM: "",
                        FinMM: "",
                        Accion: "",
                        Posicion: "",
                        RangoCuadrante: "",
                        RangoCuadranteID: "",
                        Resultado: "",
                        ResultadoID: "",
                        ElementoPorClasificacionPNDID: "",
                        Ubicacion: ""
                    };
                    listaDetalleDefectos[itera].Accion = data[j].Accion == undefined ? 1 : data[j].Accion;
                    listaDetalleDefectos[itera].CapturaResultadoPlacaDefectoID = data[j].CapturaResultadoPlacaDefectoID == undefined ? 0 : data[j].CapturaResultadoPlacaDefectoID;
                    listaDetalleDefectos[itera].CapturaResultadoPlacaID = $("#gridPopUp").data("kendoGrid").dataSource._data[i].CapturaResultadoPlacaID;
                    listaDetalleDefectos[itera].DefectoID = data[j].DefectoID;
                    listaDetalleDefectos[itera].Defecto = data[j].Defecto;
                    listaDetalleDefectos[itera].InicioMM = data[j].InicioMM;
                    listaDetalleDefectos[itera].FinMM = data[j].FinMM;
                    listaDetalleDefectos[itera].RangoCuadrante = data[j].RangoCuadrante;
                    listaDetalleDefectos[itera].RangoCuadranteID = data[j].RangoCuadranteID;
                    listaDetalleDefectos[itera].Resultado = data[j].Resultado;
                    listaDetalleDefectos[itera].ResultadoID = data[j].ResultadoID;
                    listaDetalleDefectos[itera].ElementoPorClasificacionPNDID = modeloRenglon.ElementoPorClasificacionPNDID;
                    listaDetalleDefectos[itera].Ubicacion = modeloRenglonPlaca.Ubicacion;

                    itera++;
                    if (data[j].Resultado != "" || data[j].Accion != 3 || data[j].Accion != 4) {
                        if (data[i].Resultado == _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
                            if ((data[j].InicioMM > 0 && data[j].FinMM > 0) || (data[j].Cuadrante != "")) {
                                if (data[j].InicioMM < data[j].FinMM || !modeloRenglon.EsSector)
                                    $('tr[data-uid="' + data[j].uid + '"] ').css("background-color", "#ffffff");
                                else {
                                    $('tr[data-uid="' + data[j].uid + '"] ').css("background-color", "#ffcccc");
                                    validacionCorrecta = false;
                                }
                            }
                            else {
                                $('tr[data-uid="' + data[j].uid + '"] ').css("background-color", "#ffcccc");
                                validacionCorrecta = false;
                            }
                        }
                    }
                    else {
                        $('tr[data-uid="' + data[j].uid + '"] ').css("background-color", "#ffcccc");
                        validacionCorrecta = false;
                    }
                }
                else {
                    $('tr[data-uid="' + data[i].uid + '"] ').css("background-color", "#ffcccc");
                }
            }
        }

        if ($("#gridPopUp").data("kendoGrid").dataSource._data[i].ElementoPorClasificacionPNDID == modeloRenglonPlaca.ElementoPorClasificacionPNDID && $("#gridPopUp").data("kendoGrid").dataSource._data[i].Ubicacion == modeloRenglonPlaca.Ubicacion) {
            if (validacionCorrecta) {
                var Resultados = obtenerResultadoDefecto(listaDetalleDefectos);
                $("#gridPopUp").data("kendoGrid").dataSource._data[i].ResultadoID = Resultados[0].ResultadoID;
                $("#gridPopUp").data("kendoGrid").dataSource._data[i].Resultado = Resultados[0].Resultado;
                $("#gridPopUp").data("kendoGrid").dataSource._data[i].ListaDetalleDefectos = listaDetalleDefectos;
                hayDatosCapturados = true;
                $("#windowGridDefectos").data("kendoWindow").close();
                $("#gridPopUp").data("kendoGrid").dataSource.sync();
            }
            else {
                displayNotify("CapturaReporteValidacionErroneaDefecto", "", '2');
            }
        }


    }


}

function obtenerResultadoDefecto(data) {
    var resultado = [];
    resultado[0] = { Resultado: "", ResultadoID: "" }
    for (var i = 0; i < data.length; i++) {
        if (data[i].Resultado == _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
            resultado[0].Resultado = data[i].Resultado;
            resultado[0].ResultadoID = data[i].ResultadoID;
        }
        else {
            resultado[0].Resultado = data[i].Resultado;
            resultado[0].ResultadoID = data[i].ResultadoID;
        }
    }
    return resultado;
}


function PlanchaEvaluacion(EsSector) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    if ($('input:radio[name=LLena]:checked').val() == "Todos") {
        if (HayPlacasGrid(data)) {

            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                modal: true,
                draggable: false,
                resizable: false,
                animation: {
                    close: false,
                    open: false
                },
                actions: []
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaReporteModificarNoPlacasEvaluacion[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {

                for (var i = 0; i < data.length; i++) {
                    if (EsSector == "Si") {
                        data[i].EsSector = true;
                        data[i].NumeroPlacas = null;
                        data[i].ModificadoPorUsuario = true;
                    } else if (EsSector == "No") {
                        data[i].EsSector = false;
                        data[i].NumeroPlacas = null;
                        data[i].ModificadoPorUsuario = true;
                    }
                }

                $("#grid").data("kendoGrid").refresh();
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });

        }
        else {

            for (var i = 0; i < data.length; i++) {
                if (EsSector == "Si") {
                    data[i].EsSector = true;
                    data[i].NumeroPlacas = null;
                    data[i].ModificadoPorUsuario = true;
                } else if (EsSector == "No") {
                    data[i].EsSector = false;
                    data[i].NumeroPlacas = null;
                    data[i].ModificadoPorUsuario = true;
                }
            }
        }
        $("#grid").data("kendoGrid").refresh();
    }

}

function PlanchaResultado(Resultado) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() == "Todos") {
            if (Resultado == "Si") {
                data[i].Resultado = true;
                data[i].ModificadoPorUsuario = true;
            } else if (Resultado == "No") {
                data[i].Resultado = false;
                data[i].ModificadoPorUsuario = true;
            }
        } else if ($('input:radio[name=LLena]:checked').val() == "Vacios") {
            if (!data[i].Resultado == "") {
                if (Resultado == "Si") {
                    data[i].Resultado = true;
                    data[i].ModificadoPorUsuario = true;
                } else if (Resultado == "No") {
                    data[i].Resultado = false;
                    data[i].ModificadoPorUsuario = true;

                }
            }
        }

        $("#grid").data("kendoGrid").refresh();
    }
}


function PlanchaEquipoTurno(Equipo, Turno) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    var Prueba = $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select())
    if (Prueba != undefined && Prueba.Nombre != "") {
        if (Prueba.RequiereEquipoCaptura) {

            for (var i = 0; i < data.length; i++) {
                if ($('input:radio[name=LLena]:checked').val() == "Todos") {
                    data[i].EquipoID = Equipo.EquipoID;
                    data[i].Equipo = Equipo.NombreEquipo;
                    data[i].ProveedorEquipoID = Equipo.ProveedorEquipoID;
                    if (Turno != undefined) {
                        data[i].TurnoID = Turno.TurnoID;
                        data[i].Turno = Turno.Turno;
                        data[i].CapacidadTurnoEquipoID = Turno.CapacidadTurnoEquipoID;
                        data[i].CapacidadTurnoProveedorID = Turno.CapacidadTurnoProveedorID;
                    }

                    data[i].ModificadoPorUsuario = true;

                }
                else if ($('input:radio[name=LLena]:checked').val() == "Vacios") {
                    if (data[i].Equipo == "" || data[i].Equipo === null || data[i].Equipo === undefined) {
                        data[i].EquipoID = Equipo.EquipoID;
                        data[i].Equipo = Equipo.NombreEquipo;
                        data[i].ProveedorEquipoID = Equipo.ProveedorEquipoID;
                        if (Turno != undefined) {
                            data[i].TurnoID = Turno.TurnoID;
                            data[i].Turno = Turno.Turno;
                            data[i].CapacidadTurnoEquipoID = Turno.CapacidadTurnoEquipoID;
                            data[i].CapacidadTurnoProveedorID = Turno.CapacidadTurnoProveedorID;
                        }
                        data[i].ModificadoPorUsuario = true;
                    }
                }
            }
        }
        else {
            for (var i = 0; i < data.length; i++) {
                if ($('input:radio[name=LLena]:checked').val() == "Todos") {
                    if (Turno != undefined) {
                        data[i].TurnoID = Turno.TurnoID;
                        data[i].Turno = Turno.Turno;
                        data[i].CapacidadTurnoEquipoID = Turno.CapacidadTurnoEquipoID;
                        data[i].CapacidadTurnoProveedorID = Turno.CapacidadTurnoProveedorID;
                    }
                    data[i].ModificadoPorUsuario = true;

                }
                else if ($('input:radio[name=LLena]:checked').val() == "Vacios") {
                    if (data[i].Turno == "" || data[i].Turno === null || data[i].Turno === undefined) {
                        if (Turno != undefined) {
                            data[i].TurnoID = Turno.TurnoID;
                            data[i].Turno = Turno.Turno;
                            data[i].CapacidadTurnoEquipoID = Turno.CapacidadTurnoEquipoID;
                            data[i].CapacidadTurnoProveedorID = Turno.CapacidadTurnoProveedorID;
                        }
                        data[i].ModificadoPorUsuario = true;
                    }
                }
            }
        }
        $("#grid").data("kendoGrid").refresh();
    }
}

function PlanchaResultadoPop(Resultado) {
    var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLenaPop]:checked').val() == "Todos") {
            if (Resultado == "Si") {
                data[i].Resultado = true;
                data[i].ModificadoPorUsuario = true;
            } else if (Resultado == "No") {
                data[i].Resultado = false;
                data[i].ModificadoPorUsuario = true;
            }
        } else if ($('input:radio[name=LLenaPop]:checked').val() == "Vacios") {
            if (!data[i].Resultado == "") {
                if (Resultado == "Si") {
                    data[i].Resultado = true;
                    data[i].ModificadoPorUsuario = true;
                } else if (Resultado == "No") {
                    data[i].Resultado = false;
                    data[i].ModificadoPorUsuario = true;

                }
            }
        }

        $("#gridPopUp").data("kendoGrid").refresh();
    }
}

function HayPlacasGrid(data) {

    for (var i = 0; i < data.length; i++) {
        if (data[i].NumeroPlacas > 0 && data[i].NumeroPlacas != null) {
            return true;
        }
    }
    return false;
}



function myfunction() {

}