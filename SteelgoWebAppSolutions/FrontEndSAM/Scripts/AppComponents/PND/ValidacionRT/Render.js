var listaDefectosAuxiliar;
var infoGridTemp = null;
var llamadasATodos = 0;
var numeroPlacasAnteriorElemento;
var dataItem;

function RenderNumeroPlacas(container, options) {
    var max = 9999;
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
        if (!options.model.EsSector) {
            max = 3;
        }
        numeroPlacasAnteriorElemento = { NumeroPlacas: options.model.NumeroPlacas, OrdenTrabajoID: options.model.OrdenTrabajoID, SpoolID: options.model.SpoolID, JuntaSpoolID: options.model.JuntaSpoolID };
        $('<input data-text-field="NumeroPlacas" id=' + options.model.uid + ' data-value-field="NumeroPlacas" data-bind="value:' + options.field + '" />')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "#",
            min: 0,
            max: max,
            change: function (e) {
                var grid = $("#grid").data("kendoGrid");
                dataItem = grid.dataItem($(e.target).closest("tr"));
                hayDatosCapturados = true;

                var value = this.value();
                if (numeroPlacasAnteriorElemento.NumeroPlacas != null && numeroPlacasAnteriorElemento.NumeroPlacas != this.value()) {
                    //hayDatosCapturados = true;

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
                        renderGenerarJsonNumeroPlacas(options.model.SpoolID, options.model.JuntaSpoolID, options.model.OrdenTrabajoID, options.model.EsSector);

                        ventanaConfirm.close();
                    });
                    $("#noButton").click(function () {
                        //dataItem.NumeroPlacas = numeroPlacasAnteriorElemento;
                        for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
                            if ((numeroPlacasAnteriorElemento.SpoolID == $("#grid").data("kendoGrid").dataSource._data[i].SpoolID) && (numeroPlacasAnteriorElemento.JuntaSpoolID == $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID) && (numeroPlacasAnteriorElemento.OrdenTrabajoID == $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID)) {
                                $("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas = numeroPlacasAnteriorElemento.NumeroPlacas;
                                $("#grid").data("kendoGrid").refresh();
                                break;
                            }
                        }

                        ventanaConfirm.close();
                    });
                }
                else if (numeroPlacasAnteriorElemento.NumeroPlacas == null) {
                    renderGenerarJsonNumeroPlacas(options.model.SpoolID, options.model.JuntaSpoolID, options.model.OrdenTrabajoID, options.model.EsSector);
                }
            }
        });
    };
}

function RenderTamano(container, options) {
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

        var dataItem;
        $('<input data-text-field="Tamano" id=' + options.model.uid + ' data-value-field="Tamano" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "#.0000",
            min: 0,
            change: function (e) {
                hayDatosCapturados = true;
            }
        });
    };
}


function RenderEquipo(container, options) {
    $('<input required data-text-field="NombreEquipo" id=' + options.model.uid + ' data-value-field="NombreEquipo" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: listadoTurno,//[{ EquipoID: 0, Equipo: '' }, { EquipoID: 1, Equipo: 'Matutino' }, { EquipoID: 2, Equipo: 'Vespertino' }, { EquipoID: 3, Equipo: 'NocEquipo' }],
            dataTextField: "NombreEquipo",
            dataValueField: "EquipoID",
            template: "<i class=\"fa fa-#=data.NombreEquipo#\"></i> #=data.NombreEquipo#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.EquipoID = dataItem.EquipoID;
                    options.model.Equipo = dataItem.NombreEquipo;
                }
                else {
                    options.model.EquipoID = 0;
                    options.model.Equipo = '';
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
    $('<input required data-text-field="Turno" id=' + options.model.uid + ' data-value-field="Turno" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: [{ TurnoID: 0, Turno: '' }, { TurnoID: 1, Turno: 'Matutino' }, { TurnoID: 2, Turno: 'Vespertino' }, { TurnoID: 3, Turno: 'Nocturno' }],
            dataTextField: "Turno",
            dataValueField: "TurnoID",
            template: "<i class=\"fa fa-#=data.Turno#\"></i> #=data.Turno#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.TurnoID = dataItem.TurnoID;
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

function renderGenerarJsonNumeroPlacas(spoolJunta, junta, numeroControl, EsSector) {
    //alert(spoolJunta);
    //var infoResults = new Array($("#grid").data("kendoGrid").dataSource._data.NumeroPlacas);
    for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
        if ((spoolJunta == $("#grid").data("kendoGrid").dataSource._data[i].SpoolID) && (junta == $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID) && (numeroControl == $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID)) {
            $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas = new Array($("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas);
            for (var j = 0; j < $("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas; j++) {
                if (j != ($("#grid").data("kendoGrid").dataSource._data[i].NumeroPlacas - 1))
                    $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas[j] = {
                        OrdenTrabajoID: $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID,
                        SpoolID: $("#grid").data("kendoGrid").dataSource._data[i].SpoolID,
                        JuntaSpoolID: $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID,
                        Ubicacion: EsSector ? j + '-' + (j + 1) : String.fromCharCode(65 + j), ResultadoID: undefined, Resultado: '',
                        ListaDetalleDefectos: [],
                        Accion: $("#grid").data("kendoGrid").dataSource._data[i].Accion,
                        ListaResultados: $("#grid").data("kendoGrid").dataSource._data[i].ListaResultados,
                        ListaDefectos: $("#grid").data("kendoGrid").dataSource._data[i].ListaDefectos,
                        TemplateDetallePorPlaca: _dictionary.ServiciosTecnicosCapturaReporteTemplatePlacasDefecto[$("#language").data("kendoDropDownList").value()], Posicion: j
                    };
                else
                    $("#grid").data("kendoGrid").dataSource._data[i].ListaDetallePorPlacas[j] = {
                        OrdenTrabajoID: $("#grid").data("kendoGrid").dataSource._data[i].OrdenTrabajoID,
                        SpoolID: $("#grid").data("kendoGrid").dataSource._data[i].SpoolID,
                        JuntaSpoolID: $("#grid").data("kendoGrid").dataSource._data[i].JuntaSpoolID,
                        Ubicacion: EsSector ? j + '-' + 0 : String.fromCharCode(65 + j), ResultadoID: undefined, Resultado: '',
                        ListaDetalleDefectos: [], Accion: $("#grid").data("kendoGrid").dataSource._data[i].Accion,
                        ListaResultados: $("#grid").data("kendoGrid").dataSource._data[i].ListaResultados,
                        ListaDefectos: $("#grid").data("kendoGrid").dataSource._data[i].ListaDefectos, TemplateDetallePorPlaca: _dictionary.ServiciosTecnicosCapturaReporteTemplatePlacasDefecto[$("#language").data("kendoDropDownList").value()], Posicion: j
                    };
            }
            break;
        }
    }
}

function renderDataSourceDetalleDefectos(spoolJunta, junta, numeroControl) {
    //alert(spoolJunta);
    //var infoResults = new Array($("#grid").data("kendoGrid").dataSource._data.NumeroPlacas);
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
    //modeloRenglon = model;
    //$("#gridPopUp").data('kendoGrid').dataSource.data(model.ListaDetalleDefectos);
    CargarGridPopUp(model.ListaDetalleDefectos, model);
}

function comboBoxResultadoDetallePlaca(container, options) {
    var dataItem;

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
                    options.model.ResultadoID = dataItem.ResultadosID;
                    options.model.Resultado = dataItem.Resultado;
                }
                else {
                    options.model.ResultadoID = 0;
                    options.model.Resultado = '';
                }
                var grid = $("#gridPopUp").data("kendoGrid");                
                var item = grid.dataItem(grid.select());
                if (item != null) {
                    if (item.ResultadoConciliacion == "Aprobado") {
                        item.RazonNoConciliacion = "";
                        grid.dataSource.sync();
                    }
                }
                var gridPrincipal = $("#grid").data("kendoGrid");
                var item2 = gridPrincipal.dataItem(gridPrincipal.select());
                if (item2 != null) {
                    if (item2.ResultadoConciliacion == "Aprobado") {
                        item2.RazonNoConciliacion = "";
                        gridPrincipal.dataSource.sync();
                    }
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
            //template: "<i class=\"fa fa-#=data.Defecto#\"></i> #=data.Defecto#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.DefectoID != 0) {
                    options.model.DefectoID = dataItem.DefectoID;
                    options.model.Defecto = dataItem.Defecto;
                    //OrdenTrabajoID+SpoolID+JuntaSpoolID+Ubicacion+Posicion
                    var itemPlaca = $("#PlacaID").text().split("°")
                    options.model.OrdenTrabajoID = itemPlaca[0];
                    options.model.SpoolID = itemPlaca[1];
                    options.model.JuntaSpoolID = itemPlaca[2];
                    options.model.Ubicacion = itemPlaca[3];
                    options.model.Posicion = itemPlaca[4];
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
        llamadasATodos = 0;
    }
    else if (tipoFiltro == "Todos") {
        for (var i = 0; i < infoGridTemp.length; i++) {
            //if (infoGridTemp[i].NumeroPlacas != 0) {
            $("#grid").data("kendoGrid").dataSource.add(infoGridTemp[i]);
            //}
        }
    }

    loadingStop();
}

function disableEnableView(disable) {
    if (disable) {
        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputProveedor").data("kendoComboBox").enable(false);
        $("#inputRequisicion").data("kendoComboBox").enable(false);        
        $("#inputPrueba").data("kendoComboBox").enable(false);
        $("input[name='Muestra']").attr("disabled", true);
        $("#btnAgregar").attr("disabled", true);        
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    } else {
        $(".addedSectionInLine").find('*').attr("disabled", false);
        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputProveedor").data("kendoComboBox").enable(true);
        $("#inputRequisicion").data("kendoComboBox").enable(true);        
        $("#inputPrueba").data("kendoComboBox").enable(true);
        $("#btnAgregar").attr("disabled", false);        
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
            dataSource: [{ CuadranteID: 0, Cuadrante: '' }, { CuadranteID: 1, Cuadrante: 'A' }, { CuadranteID: 2, Cuadrante: 'B' }, { CuadranteID: 3, Cuadrante: 'C' }],
            dataTextField: "Cuadrante",
            dataValueField: "CuadranteID",
            template: "<i class=\"fa fa-#=data.Cuadrante#\"></i> #=data.Cuadrante#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.CuadranteID = dataItem.CuadranteID;
                    options.model.Cuadrante = dataItem.Cuadrante;
                }
                else {
                    options.model.CuadranteID = 0;
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