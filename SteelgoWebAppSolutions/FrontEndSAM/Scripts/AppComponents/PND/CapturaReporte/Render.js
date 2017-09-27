var listaDefectosAuxiliar;
var infoGridTemp = null;
var numeroPlacasAnteriorElemento;
var dataItem;

function RenderNumeroPlacas(container, options) {

    var max = 0, min = 0;

    if (options.model.EsSector) {
        min = options.model.MinimoPlacasSector;
        max = options.model.MaximoPlacasSector;
    }
    else {
        min = options.model.MinimoPlacasCuadrante;
        max = options.model.MaximoPlacasCuadrante;
    }

    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

        numeroPlacasAnteriorElemento = { NumeroPlacas: options.model.NumeroPlacas, ElementoPorClasificacionPNDID: options.model.ElementoPorClasificacionPNDID };

        $('<input data-text-field="NumeroPlacas" id=' + options.model.uid + ' data-value-field="NumeroPlacas" data-bind="value:' + options.field + '" />')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "#",
            min: min,
            max: max,
            change: function (e) {
                var grid = $("#grid").data("kendoGrid");
                dataItem = grid.dataItem($(e.target).closest("tr"));
                hayDatosCapturados = true;

                var value = this.value();
                if (numeroPlacasAnteriorElemento.NumeroPlacas != null && numeroPlacasAnteriorElemento.NumeroPlacas != this.value() || options.model.TieneEntregaPlacas == 1) {
                    
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

                    var mensajeMostrar = options.model.TieneEntregaPlacas == 0 ? _dictionary.CapturaReporteModificarNoPlacas[$("#language").data("kendoDropDownList").value()] : _dictionary.CapturaReporteModificarNoPlacasConEntregaPlacas[$("#language").data("kendoDropDownList").value()];

                    ventanaConfirm.content(mensajeMostrar +
                                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

                    ventanaConfirm.open().center();

                    $("#yesButton").click(function () {
                        renderGenerarJsonNumeroPlacas(options.model.ElementoPorClasificacionPNDID, options.model.EsSector, options.model);
                        ventanaConfirm.close();
                    });
                    $("#noButton").click(function () {
                        for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
                            if (numeroPlacasAnteriorElemento.ElementoPorClasificacionPNDID == $("#grid").data("kendoGrid").dataSource._data[i].ElementoPorClasificacionPNDID) {
                                $("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas = numeroPlacasAnteriorElemento.NumeroPlacas;
                                $("#grid").data("kendoGrid").refresh();
                            }
                        }
                        ventanaConfirm.close();
                    });
                }
                else if (numeroPlacasAnteriorElemento.NumeroPlacas == null) {
                    renderGenerarJsonNumeroPlacas(options.model.ElementoPorClasificacionPNDID, options.model.EsSector, options.model);
                }
            }
        });
    };
}

function RenderEquipo(container, options) {
    $('<input required data-text-field="NombreEquipo" id=' + options.model.uid + ' data-value-field="NombreEquipo" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.listadoEquipo,
            dataTextField: "NombreEquipo",
            dataValueField: "EquipoID",
            template: "<i class=\"fa fa-#=data.NombreEquipo#\"></i> #=data.NombreEquipo#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.EquipoID = dataItem.EquipoID;
                    options.model.Equipo = dataItem.NombreEquipo;
                    options.model.ProveedorEquipoID = dataItem.ProveedorEquipoID;
                }
                else {
                    options.model.EquipoID = 0;
                    options.model.Equipo = '';
                    options.model.ProveedorEquipoID = 0;
                }

            }
        }
    );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}

function RenderTurno(container, options) {

    var listaTurnoFiltro;
    listaTurnoFiltro = [];
    if ($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura) {
        listaTurnoFiltro = obtenerTurnoLaboralEquipo(options.model, options.model.ProveedorEquipoID);
    }
    else {
        listaTurnoFiltro = obtenerTurnoLaboralProveedor(options.model, $("#inputProveedor").data("kendoComboBox").dataItem($("#inputProveedor").data("kendoComboBox").select()).TipoPruebaProveedorID);
    }

    $('<input required data-text-field="Turno" id=' + options.model.uid + ' data-value-field="Turno" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: listaTurnoFiltro,
            dataTextField: "Turno",
            dataValueField: "TurnoID",
            template: "<i class=\"fa fa-#=data.Turno#\"></i> #=data.Turno#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.TurnoID = dataItem.TurnoID;
                    options.model.CapacidadTurnoEquipoID = dataItem.CapacidadTurnoEquipoID;
                    options.model.CapacidadTurnoProveedorID = dataItem.CapacidadTurnoProveedorID;
                    options.model.Turno = dataItem.Turno;
                }
                else {
                    options.model.TurnoID = 0;
                    options.model.Turno = '';
                }

            }
        }
    );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}

function RenderInicioMM(container, options) {
    var dataItem;
    $('<input data-text-field="InicioMM" id=' + options.model.uid + ' data-value-field="InicioMM" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "{0: }",
        min: 0
    });

}

function RenderFinMM(container, options) {
    var dataItem;
    $('<input data-text-field="FinMM" id=' + options.model.uid + ' data-value-field="FinMM" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "{0: }",
        min: 0
    });

}

function renderGenerarJsonNumeroPlacas(ElementoPorClasificacionPNDID, EsSector, modeloRenglon) {

    var listaDetallePlacaAux = modeloRenglon.ListaDetallePorPlacas;
    modeloRenglon.ListaDetallePorPlacas = [];
    for (var j = 0; j < modeloRenglon.NumeroPlacas; j++) {
        if (j != (modeloRenglon.NumeroPlacas - 1))
            modeloRenglon.ListaDetallePorPlacas[j] = {
                Ubicacion: EsSector ? j + '-' + (j + 1) : String.fromCharCode(65 + j), ResultadoID: undefined, Resultado: '',
                ListaDetalleDefectos: [],
                Accion: 1,
                ListaResultados: modeloRenglon.ListaResultados,
                ListaDefectos: modeloRenglon.ListaDefectos,
                TemplateDetallePorPlaca: _dictionary.ServiciosTecnicosCapturaReporteTemplatePlacasDefecto[$("#language").data("kendoDropDownList").value()],
                ElementoPorClasificacionPNDID: modeloRenglon.ElementoPorClasificacionPNDID
            };
        else
            modeloRenglon.ListaDetallePorPlacas[j] = {
                Ubicacion: EsSector ? j + '-' + 0 : String.fromCharCode(65 + j), ResultadoID: undefined, Resultado: '',
                ListaDetalleDefectos: [], Accion: 1,
                ListaResultados: modeloRenglon.ListaResultados,
                ListaDefectos: modeloRenglon.ListaDefectos,
                TemplateDetallePorPlaca: _dictionary.ServiciosTecnicosCapturaReporteTemplatePlacasDefecto[$("#language").data("kendoDropDownList").value()],
                ElementoPorClasificacionPNDID: modeloRenglon.ElementoPorClasificacionPNDID
            };
    }
    for (var i = 0; i < listaDetallePlacaAux.length; i++) {
        for (var j = 0; j < listaDetallePlacaAux[i].ListaDetalleDefectos.length; j++) {
            if (listaDetallePlacaAux[i].ListaDetalleDefectos[j].Accion == 2)
                listaDetallePlacaAux[i].ListaDetalleDefectos[j].Accion = 3;
        }
        if (listaDetallePlacaAux[i].Accion == 2)
            listaDetallePlacaAux[i].Accion = 3;
    }

    var cont = 0;
    for (var y = 0; y < listaDetallePlacaAux.length; y++) {
        if (listaDetallePlacaAux[y].Accion == 3) {
            modeloRenglon.ListaDetallePorPlacas[modeloRenglon.ListaDetallePorPlacas.length + cont] = listaDetallePlacaAux[y];
            cont++;
        }
    }
    cont = 0;
}

function renderDataSourceDetalleDefectos(spoolJunta, junta, numeroControl) {

    for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
        if ((spoolJunta == $("#grid").data("kendoGrid").dataSource._data[i].SpoolID) && (junta == $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID) && (numeroControl == $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID)) {
            $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas = new Array($("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas);
            for (var j = 0; j < $("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas; j++) {
                if (j != ($("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas - 1))
                    $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas[j] = { ReporteRTResultadosID: 0, ReporteRTID: $("#grid").data("kendoGrid").dataSource._data[i].ReporteRTID, OrdenTrabajoID: $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID, SpoolID: $("#grid").data("kendoGrid").dataSource._data[i].SpoolID, JuntaSpoolID: $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID, NumeroControl: $("#grid").data("kendoGrid").dataSource._data[i].NumeroControl, Ubicacion: j + '-' + (j + 1), Resultado: '', ListaDetalleDefectos: [], Accion: 1, ListaResultados: $("#grid").data("kendoGrid").dataSource._data[i].ListaResultados, ListaDefectos: $("#grid").data("kendoGrid").dataSource._data[i].ListaDefectos };
                else
                    $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas[j] = { ReporteRTResultadosID: 0, ReporteRTID: $("#grid").data("kendoGrid").dataSource._data[i].ReporteRTID, OrdenTrabajoID: $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID, SpoolID: $("#grid").data("kendoGrid").dataSource._data[i].SpoolID, JuntaSpoolID: $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID, NumeroControl: $("#grid").data("kendoGrid").dataSource._data[i].NumeroControl, Ubicacion: j + '-' + 0, Resultado: '', ListaDetalleDefectos: [], Accion: 1, ListaResultados: $("#grid").data("kendoGrid").dataSource._data[i].ListaResultados, ListaDefectos: $("#grid").data("kendoGrid").dataSource._data[i].ListaDefectos };
            }
            break;
        }
    }
}

function VentanaModal(model) {
    CargarGridPopUp(model.ListaDetalleDefectos, model);
}

function comboBoxResultadoDetallePlaca(container, options) {

    $('<input required data-text-field="Resultado" id=' + options.model.uid + ' data-value-field="Resultado" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaResultados,
            dataTextField: "Resultado",
            dataValueField: "ResultadosID",
            template: "<i class=\"fa fa-#=data.Resultado#\"></i> #=data.Resultado#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    if (dataItem.Defecto == _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
                        options.model.ResultadoID = 0;
                        options.model.Resultado = '';
                        $("#gridPopUp").data("kendoGrid").dataSource.sync();
                    }
                    else {
                        options.model.ResultadoID = dataItem.ResultadosID;
                        options.model.Resultado = dataItem.Resultado;
                    }

                }
                else {
                    options.model.ResultadoID = 0;
                    options.model.Resultado = '';
                }
            }
        }
    );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });

}

function comboBoxResultadoDetalleDefecto(container, options) {
    var dataItem;

    $('<input required data-text-field="Resultado" id=' + options.model.uid + ' data-value-field="Resultado" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: modeloRenglon.ListaResultados,
            dataTextField: "Resultado",
            dataValueField: "ResultadosID",
            template: "<i class=\"fa fa-#=data.Resultado#\"></i> #=data.Resultado#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.ResultadoID = dataItem.ResultadosID;
                    options.model.Resultado = dataItem.Resultado;
                }
                else {
                    options.model.ResultadoID = 0;
                    options.model.Resultado = '';
                }

            }
        }
    );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}

function WindowModalGridDefectoDetalle(model) {
    var modalTitle = model.SpoolID + "_" + model.JuntaSpoolID + "_" + model.OrdenTrabajoID + "_" + model.Ubicacion;
    //modalTitle = _dictionary.ValidacionResultadosRequisicion[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "50%",
        minWidth: 30,
        position: {
            top: "1%",
            left: "1%"
        },
        actions: [
            "Pin",
            "Minimize",
            "Maximize",
            "Close"
        ],
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();
}

function comboBoxDefectos(container, options) {
    var dataItem;
    $('<input required data-text-field="Defecto" id=' + options.model.uid + ' data-value-field="Defecto" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataTextField: "Defecto",
            dataValueField: "DefectoID",
            dataSource: listaDefectosAuxiliar,
            template: "<i class=\"fa fa-#=data.Defecto#\"></i> #=data.Defecto#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.DefectoID != 0) {
                    if (dataItem.Defecto != "") {
                        options.model.DefectoID = dataItem.DefectoID;
                        options.model.Defecto = dataItem.Defecto;
                        options.model.ElementoPorClasificacionPNDID = modeloRenglonPlaca.ElementoPorClasificacionPNDID;
                        options.model.Ubicacion = modeloRenglonPlaca.Ubicacion;
                    }
                    else {
                        options.model.DefectoID = 0;
                        options.model.Defecto = "";
                        options.model.ElementoPorClasificacionPNDID = 0;
                        options.model.Ubicacion = "";
                    }
                }
                else {
                    options.model.DefectoID = 0;
                    options.model.Defecto = "";
                    options.model.ElementoPorClasificacionPNDID = 0;
                    options.model.Ubicacion = "";
                }
            }
        }
        );

    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}

var hayDatosCapturados = false;


function filtraDatosCapturados(tipoFiltro) {
    loadingStart();

    if (infoGridTemp == null)
        infoGridTemp = $("#grid").data("kendoGrid").dataSource._data;

    $("#grid").data('kendoGrid').dataSource.data([]);

    if (tipoFiltro == "SinCaptura") {
        for (var i = 0; i < infoGridTemp.length; i++) {
            if ((infoGridTemp[i].NumeroPlacas == 0) || (infoGridTemp[i].NumeroPlacas == null)) {
                $("#grid").data("kendoGrid").dataSource.add(infoGridTemp[i]);
            }
        }
    }
    else if (tipoFiltro == "Todos") {
        for (var i = 0; i < infoGridTemp.length; i++) {
            $("#grid").data("kendoGrid").dataSource.add(infoGridTemp[i]);
        }
    }

    loadingStop();
}

function disableEnableView(disable) {
    if (disable) {

        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputProveedor").data("kendoComboBox").enable(false);
        $("#inputRequisicion").data("kendoComboBox").enable(false);
        $("#inputFuente").data("kendoComboBox").enable(false);
        $("#inputTurno").data("kendoComboBox").enable(false);
        $("#inputPrueba").data("kendoComboBox").enable(false);

        $("input[name='Muestra']").attr("disabled", true);
        $("input[name='SelectSector']").attr("disabled", true);
        $("input[name='SelectTodos']").attr("disabled", true);
        $("input[name='LLena']").attr("disabled", true);

        $("#btnAgregar").attr("disabled", true);
        $("#btnPlanchar").attr("disabled", true);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);


    } else {
        //$(".addedSectionInLine").find('*').attr("disabled", false);

        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputProveedor").data("kendoComboBox").enable(true);
        $("#inputRequisicion").data("kendoComboBox").enable(true);
        $("#inputFuente").data("kendoComboBox").enable(true);
        $("#inputTurno").data("kendoComboBox").enable(true);
        $("#inputPrueba").data("kendoComboBox").enable(true);

        $("input[name='Muestra']").attr("disabled", false);
        $("input[name='SelectSector']").attr("disabled", false);
        $("input[name='SelectTodos']").attr("disabled", false);
        $("input[name='LLena']").attr("disabled", false);

        $("#btnAgregar").attr("disabled", false);
        $("#btnPlanchar").attr("disabled", false);


        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);

    }
}

function cleanView() {


    $("#inputProyecto").data("kendoComboBox").value("");
    $("#inputProveedor").data("kendoComboBox").value("");
    $("#inputRequisicion").data("kendoComboBox").value("");
    $("#inputFuente").data("kendoComboBox").value("");
    $("#inputTurno").data("kendoComboBox").value("");
    $("#inputPrueba").data("kendoComboBox").value("");

    $("#grid").data("kendoGrid").dataSource.data([]);
    disableEnableView(false);

}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}


function RenderCuadrantes(container, options) {
    $('<input required data-text-field="Cuadrante" id=' + options.model.uid + ' data-value-field="Cuadrante" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: modeloRenglon.listaRangoCuadrantes,
            dataTextField: "Cuadrante",
            dataValueField: "RangoCuadranteID",
            template: "<i class=\"fa fa-#=data.Cuadrante#\"></i> #=data.Cuadrante#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.RangoCuadranteID = dataItem.RangoCuadranteID;
                    options.model.Cuadrante = dataItem.Cuadrante;
                }
                else {
                    options.model.RangoCuadranteID = 0;
                    options.model.Cuadrante = '';
                }

            }
        }
    );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}



function obtenerTurnoLaboralProveedor(model, TipoPruebaProveedorID) {
    var lista = model.listadoTurno;
    var listaFinalTurnoLaboral = [];
    listaFinalTurnoLaboral[0] = { TurnoLaboralID: "", Turno: "", TipoPruebaProveedorID: "", CapacidadTurnoProveedorID: "", CapacidadTurnoEquipoID: "" };
    var cont = 1;
    listaFinalTurnoLaboral[0].TurnoLaboralID = 0;
    listaFinalTurnoLaboral[0].TipoPruebaProveedorID = 0;
    listaFinalTurnoLaboral[0].Turno = "";
    listaFinalTurnoLaboral[0].CapacidadTurnoProveedorID = 0;
    listaFinalTurnoLaboral[0].CapacidadTurnoEquipoID = 0;

    for (var i = 0; i < lista.length; i++) {
        if (lista[i].TipoPruebaProveedorID == TipoPruebaProveedorID && lista[i].CapacidadTurnoEquipoID == 0) {
            listaFinalTurnoLaboral[cont] = lista[i];
            cont++;
        }
    }

    return listaFinalTurnoLaboral;
}



function obtenerTurnoLaboralEquipo(model, ProveedorEquipoID) {
    var lista = model.listadoTurno;
    var listaFinalTurnoLaboral = [];
    listaFinalTurnoLaboral[0] = { TurnoLaboralID: "", Turno: "", TipoPruebaProveedorID: "", CapacidadTurnoProveedorID: "", CapacidadTurnoEquipoID: "" };
    var cont = 1;
    listaFinalTurnoLaboral[0].TurnoLaboralID = 0;
    listaFinalTurnoLaboral[0].TipoPruebaProveedorID = 0;
    listaFinalTurnoLaboral[0].Turno = "";
    listaFinalTurnoLaboral[0].CapacidadTurnoProveedorID = 0;
    listaFinalTurnoLaboral[0].CapacidadTurnoEquipoID = 0;

    for (var i = 0; i < lista.length; i++) {
        if (lista[i].ProveedorEquipoID == ProveedorEquipoID) {
            listaFinalTurnoLaboral[cont] = lista[i];
            cont++;
        }
    }

    return listaFinalTurnoLaboral;
}



function comboBoxIndicacion(container, options) {
    var dataItem;
    $('<input required data-text-field="Defecto" id=' + options.model.uid + ' data-value-field="Defecto" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataTextField: "Defecto",
            dataValueField: "DefectoID",
            dataSource: options.model.ListaDefectos,
            template: "<i class=\"fa fa-#=data.Defecto#\"></i> #=data.Defecto#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.DefectoID != 0) {
                    if (dataItem.Defecto != "") {
                        options.model.DefectoID = dataItem.DefectoID;
                        options.model.Indicacion = dataItem.Defecto;
                    }
                    else {
                        options.model.DefectoID = 0;
                        options.model.Indicacion = "";
                    }
                }
                else {
                    options.model.DefectoID = 0;
                    options.model.Indicacion = "";
                }
            }
        }
        );

    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}

function comboBoxResultadoDetalleElemento(container, options) {

    $('<input required data-text-field="Resultado" id=' + options.model.uid + ' data-value-field="Resultado" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaResultados,
            dataTextField: "Resultado",
            dataValueField: "ResultadosID",
            template: "<i class=\"fa fa-#=data.Resultado#\"></i> #=data.Resultado#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    if (dataItem.Defecto != _dictionary.ValidacionResultadosComboRechazado[$("#language").data("kendoDropDownList").value()]) {
                        options.model.DefectoID = 0;
                        options.model.Indicacion = '';
                        options.model.ResultadoID = dataItem.ResultadosID;
                        options.model.Resultado = dataItem.Resultado;
                    }
                    else {
                        options.model.ResultadoID = dataItem.ResultadosID;
                        options.model.Resultado = dataItem.Resultado;
                    }

                }
                else {
                    options.model.ResultadoID = 0;
                    options.model.Resultado = '';
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
    );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });

}