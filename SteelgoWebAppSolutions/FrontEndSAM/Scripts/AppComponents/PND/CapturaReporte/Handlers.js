var previousCurrentItem = 0;
var proyectoInicial = 0;
var pruebaOriginal = 0;
var requisicionOriginal = 0;
var proveedorInicial = 0;


function SuscribirEventos() {
    suscribirEventoGuardar();
    suscribirEventoPlanchar();
    suscribirEventoChangeRadioTipo();
    suscribirEventoProyecto();
    suscribirEventoProveedor();
    SuscribirEventoComboPrueba();
    suscribirEventoRequisicion();
    suscribirEventoFuente();
    suscribirEventTurno();
    suscribirEventoDetallePlaca();
    suscribirEventoDetalleDefectoPorPlaca();
}

function suscribirEventoDetallePlaca() {

    $(document).on('click', '.EnlacePorPlaca', function (e) {
        e.preventDefault();
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.NumeroPlacas  != null )
                if (dataItem.NumeroPlacas > 0)
                    LlenarGridPopUpDetallePlaca(dataItem);
        }
    });
}

function suscribirEventoDetalleDefectoPorPlaca() {

    $(document).on('click', '.EnlaceDefectoPorPlaca', function (e) {
        e.preventDefault();
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#gridPopUp").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            LlenarGridPopUpDetalleDefectoPorPlaca(dataItem);
        }
    });
}


function suscribirEventoGuardar() {

    $('#GuardarPlacas').click(function (e) {
        var ds = $("#gridPopUp").data("kendoGrid").dataSource;
        var window = $("#windowGrid");
        actualizaGridGeneralPorPlaca();
    });

    $('#GuardarDefectos').click(function (e) {
        var ds = $("#gridPopUpDefectos").data("kendoGrid").dataSource;
        var window = $("#windowGridDefectos");

        actualizaGridGeneralPorDefectos();
    });

    $('#CancelarPlacas').click(function (e) {
        $("#windowGrid").data("kendoWindow").close();
    });

    $('#CancelarDefectos').click(function (e) {
        $("#windowGridDefectos").data("kendoWindow").close();
    });


    //GuardarYNuevo
    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {

        if (infoGridTemp != null)
            if ($("#grid").data("kendoGrid").dataSource._data.length != infoGridTemp.length)
                $("#grid").data("kendoGrid").dataSource._data = infoGridTemp;

        if (validarReglasDeLlenado()) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            AjaxGuardarCaptura(ds._data, true);
        }
    });

    //Guardar
    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {

        if (infoGridTemp != null)
            if ($("#grid").data("kendoGrid").dataSource._data.length != infoGridTemp.length)
                $("#grid").data("kendoGrid").dataSource._data = infoGridTemp;

        if (validarReglasDeLlenado()) {
            if ($("#Guardar").text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                AjaxGuardarCaptura(ds._data, false);
            } else if ($("#Guardar").text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
                disableEnableView(false);
            }
        }
    });

    $('#btnAgregar').click(function (e) {
        
        if ($('input:radio[name=Muestra]:checked').val() == "SinCaptura") {
            filtraDatosCapturados($('input:radio[name=Muestra]:checked').val());
        }
        else if ($('input:radio[name=Muestra]:checked').val() == "Todos") {
            if ( infoGridTemp == null) {
                if (hayDatosCapturados) {

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
                    ventanaConfirm.content(_dictionary.CapturaReportePruebasMensajeEliminarDatosCapturados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");
                    ventanaConfirm.open().center();

                    $("#yesButton").click(function () {
                        ajaxResultadosDetalle($("#inputProyecto").data("kendoComboBox").value(), $("#inputProveedor").data("kendoComboBox").value(), $("#inputRequisicion").data("kendoComboBox").value());
                        infoGridTemp = null;
                        hayDatosCapturados = false;
                        ventanaConfirm.close();

                    });
                    $("#noButton").click(function () {
                        //$('#inputProyecto').data("kendoComboBox").value(previousCurrentItem);
                        ventanaConfirm.close();
                    });

                }
                else {
                    ajaxResultadosDetalle($("#inputProyecto").data("kendoComboBox").value(), $("#inputProveedor").data("kendoComboBox").value(), $("#inputRequisicion").data("kendoComboBox").value());
                    infoGridTemp = null;
                    hayDatosCapturados = false;
                }
            }
            else {//Filtra los capturados
                filtraDatosCapturados($('input:radio[name=Muestra]:checked').val());
            }
        }

    });
}

function suscribirEventoProyecto() {
    $('#inputProyecto').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (hayDatosCapturados) {
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
                    ventanaConfirm.content(_dictionary.CapturaReportePruebasMensajeEliminarDatosCapturados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");
                    ventanaConfirm.open().center();

                    $("#yesButton").click(function () {
                        $("#grid").data('kendoGrid').dataSource.data([]);
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        $("#inputFuente").data("kendoComboBox").value("");
                        $("#inputTurno").data("kendoComboBox").value("");
                        AjaxPruebas(dataItem.ProyectoID);
                        hayDatosCapturados = false;
                        ventanaConfirm.close();
                        proyectoInicial = dataItem.ProyectoID;
                    });
                    $("#noButton").click(function () {
                        $('#inputProyecto').data("kendoComboBox").value(proyectoInicial);
                        ventanaConfirm.close();
                    });

                }
                else {
                    $("#grid").data('kendoGrid').dataSource.data([]);
                    AjaxPruebas(dataItem.ProyectoID);
                    proyectoInicial = dataItem.ProyectoID;
                    $("#inputRequisicion").data("kendoComboBox").value("");
                    $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                    $("#inputFuente").data("kendoComboBox").value("");
                    $("#inputTurno").data("kendoComboBox").value("");
                }
                
            }
            else {
                $("#inputProyecto").data("kendoComboBox").text("");
                $("#inputProyecto").data("kendoComboBox").value("");
                proyectoInicial = 0;
                $("#inputPrueba").data("kendoComboBox").setDataSource();
                $("#inputPrueba").data("kendoComboBox").value("");
                $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputProveedor").data("kendoComboBox").setDataSource();
                $("#inputProveedor").data("kendoComboBox").value("");
                $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                $("#inputRequisicion").data("kendoComboBox").setDataSource();
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                $("#inputFuente").data("kendoComboBox").setDataSource();
                $("#inputFuente").data("kendoComboBox").value("");
                $("#inputFuente").data("kendoComboBox").dataSource.data([]);
                $("#inputTurno").data("kendoComboBox").setDataSource();
                $("#inputTurno").data("kendoComboBox").value("");
                $("#inputTurno").data("kendoComboBox").dataSource.data([]);
            }
        }
    });

    $("#inputProyecto").data("kendoComboBox").input.on("focus", function () {
        previousCurrentItem = this.value;
    });

}

function suscribirEventoProveedor() {
    $('#inputProveedor').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProveedorID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                
                if (hayDatosCapturados) {
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

                    ventanaConfirm.content(_dictionary.CapturaReportePruebasMensajeEliminarDatosCapturados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");
                    ventanaConfirm.open().center();

                    $("#yesButton").click(function () {
                        $("#grid").data('kendoGrid').dataSource.data([]);
                        
                        $("#inputFuente").data("kendoComboBox").value("");
                        $("#inputTurno").data("kendoComboBox").value("");

                        hayDatosCapturados = false;
                        proveedorInicial = dataItem.ProveedorID;
                        AjaxRequisicion($("#inputProyecto").data("kendoComboBox").value(), dataItem.ProveedorID);
                        ventanaConfirm.close();

                    });
                    $("#noButton").click(function () {
                        $('#inputProveedor').data("kendoComboBox").value(proveedorInicial);
                        ventanaConfirm.close();
                    });

                }
                else {
                    $("#grid").data('kendoGrid').dataSource.data([]);
                    proveedorInicial = dataItem.ProveedorID;
                    AjaxRequisicion($("#inputProyecto").data("kendoComboBox").value(), dataItem.ProveedorID);
                    $("#inputFuente").data("kendoComboBox").value("");
                    $("#inputTurno").data("kendoComboBox").value("");
                }
            }
            else {
                $("#inputProveedor").data("kendoComboBox").text("");
                $("#inputProveedor").data("kendoComboBox").value("");
                proveedorInicial = 0;
                $("#inputRequisicion").data("kendoComboBox").setDataSource();
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                $("#inputFuente").data("kendoComboBox").setDataSource();
                $("#inputFuente").data("kendoComboBox").value("");
                $("#inputFuente").data("kendoComboBox").dataSource.data([]);
                $("#inputTurno").data("kendoComboBox").setDataSource();
                $("#inputTurno").data("kendoComboBox").value("");
                $("#inputTurno").data("kendoComboBox").dataSource.data([]);
            }

        }
    });

    $("#inputProveedor").data("kendoComboBox").input.on("focus", function () {
        previousCurrentItem = this.value;
    });
}

function SuscribirEventoComboPrueba() {
    $('#inputPrueba').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TipoPruebaID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                
                if (hayDatosCapturados) {
                  
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
                    ventanaConfirm.content(_dictionary.CapturaReportePruebasMensajeEliminarDatosCapturados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");
                    ventanaConfirm.open().center();

                    $("#yesButton").click(function () {
                        $("#grid").data('kendoGrid').dataSource.data([]);

                        if (dataItem.Nombre.indexOf("RT") !== -1) {
                            $("#grid").data("kendoGrid").showColumn("EsSector");
                            $("#grid").data("kendoGrid").showColumn("NumeroPlacas");
                            $("#grid").data("kendoGrid").showColumn("TemplateDetalleElemento");
                            $("#grid").data("kendoGrid").hideColumn("ResultadoConciliacion");
                            $("#grid").data("kendoGrid").hideColumn("RazonNoConciliacion");
                            $("#grid").data("kendoGrid").hideColumn("Indicacion");
                            $("#divRes").css("display", "none");
                            $("#EvaluacionDiv").css("display", "block");
                        }
                        else {
                            $("#grid").data("kendoGrid").hideColumn("EsSector");
                            $("#grid").data("kendoGrid").hideColumn("NumeroPlacas");
                            $("#grid").data("kendoGrid").hideColumn("TemplateDetalleElemento");
                            $("#grid").data("kendoGrid").showColumn("ResultadoConciliacion");
                            $("#grid").data("kendoGrid").showColumn("RazonNoConciliacion");
                            $("#grid").data("kendoGrid").showColumn("Indicacion");
                            $("#divRes").css("display", "block");
                            $("#EvaluacionDiv").css("display", "none");
                            
                        }

                        $("#inputFuente").data("kendoComboBox").value("");
                        $("#inputTurno").data("kendoComboBox").value("");
                        AjaxProveedor($("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID, $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).PatioID)
                        pruebaOriginal = dataItem.TipoPruebaID;
                        hayDatosCapturados = false;
                        ventanaConfirm.close();

                    });
                    $("#noButton").click(function () {
                        $('#inputPrueba').data("kendoComboBox").value(pruebaOriginal);
                        ventanaConfirm.close();
                    });

                }
                else {
                    $("#grid").data('kendoGrid').dataSource.data([]);

                    if (dataItem.Nombre.indexOf("RT") !== -1) {
                        $("#grid").data("kendoGrid").showColumn("EsSector");
                        $("#grid").data("kendoGrid").showColumn("NumeroPlacas");
                        $("#grid").data("kendoGrid").showColumn("TemplateDetalleElemento");
                        $("#grid").data("kendoGrid").hideColumn("ResultadoConciliacion");
                        $("#grid").data("kendoGrid").hideColumn("RazonNoConciliacion");
                        $("#grid").data("kendoGrid").hideColumn("Indicacion");
                        $("#grid").data("kendoGrid").hideColumn("NombreGrafica");
                        $("#EvaluacionDiv").css("display", "block");
                        $("#divRes").css("display", "none");
                    }
                    else {
                        $("#grid").data("kendoGrid").hideColumn("EsSector");
                        $("#grid").data("kendoGrid").hideColumn("NumeroPlacas");
                        $("#grid").data("kendoGrid").hideColumn("TemplateDetalleElemento");
                        $("#grid").data("kendoGrid").showColumn("ResultadoConciliacion");
                        $("#grid").data("kendoGrid").showColumn("RazonNoConciliacion");
                        $("#grid").data("kendoGrid").showColumn("Indicacion");
                        $("#grid").data("kendoGrid").showColumn("NombreGrafica");
                        $("#EvaluacionDiv").css("display", "none");
                        $("#divRes").css("display", "block");
                    }


                    $("#inputFuente").data("kendoComboBox").value("");
                    $("#inputTurno").data("kendoComboBox").value("");
                    pruebaOriginal = dataItem.TipoPruebaID;
                    AjaxProveedor($("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID, $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).PatioID);

                }
            }
            else {
                $("#lblTurno").text("");
                $("#inputPrueba").data("kendoComboBox").text("");
                $("#inputPrueba").data("kendoComboBox").value("");
                pruebaOriginal = 0;
                $("#inputProveedor").data("kendoComboBox").setDataSource();
                $("#inputProveedor").data("kendoComboBox").value("");
                $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                $("#inputRequisicion").data("kendoComboBox").setDataSource();
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                $("#inputFuente").data("kendoComboBox").setDataSource();
                $("#inputFuente").data("kendoComboBox").value("");
                $("#inputFuente").data("kendoComboBox").dataSource.data([]);
                $("#inputTurno").data("kendoComboBox").setDataSource();
                $("#inputTurno").data("kendoComboBox").value("");
                $("#inputTurno").data("kendoComboBox").dataSource.data([]);
            }
            

        }
    });

    $("#inputPrueba").data("kendoComboBox").input.on("focus", function () {
        previousCurrentItem = this.value;
    });
};

function suscribirEventoChangeRadioTipo() {

    $('input:radio[name=Muestra]:nth(0)').change(function () {

    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {

    });
}

function suscribirEventoRequisicion() {
    $('#inputRequisicion').kendoComboBox({
        dataTextField: "NombreRequisicion",
        dataValueField: "RequisicionID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            $("#lblTurno").text("");
            dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                
                if (hayDatosCapturados) {

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
                    ventanaConfirm.content(_dictionary.CapturaReportePruebasMensajeEliminarDatosCapturados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");
                    ventanaConfirm.open().center();

                    $("#yesButton").click(function () {
                        $("#grid").data('kendoGrid').dataSource.data([]);
                        $("#lblTurno").text($("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select()).TurnoLaboral);
                        requisicionOriginal = dataItem.RequisicionID;
                        if ($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura) {
                            AjaxFuente();
                            $("#inputFuente").data("kendoComboBox").enable(true);
                        }
                        else {
                            AjaxTurno();
                            $("#inputFuente").data("kendoComboBox").enable(false);
                        }

                        hayDatosCapturados = false;
                        ventanaConfirm.close();

                    });
                    $("#noButton").click(function () {
                        $('#inputPrueba').data("kendoComboBox").value(requisicionOriginal);
                        ventanaConfirm.close();
                    });
                }
                else{
                    $("#grid").data('kendoGrid').dataSource.data([]);
                    $("#lblTurno").text("");
                    $("#lblTurno").text($("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select()).TurnoLaboral);
                    requisicionOriginal = dataItem.RequisicionID;
                    if ($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).RequiereEquipoCaptura) {
                        AjaxFuente();
                        $("#inputFuente").data("kendoComboBox").enable(true);
                    }
                    else {
                        AjaxTurno();
                        $("#inputFuente").data("kendoComboBox").enable(false);
                    }

                }
            }
                else {
                    $("#lblTurno").text("");
                    $("#inputRequisicion").data("kendoComboBox").text("");
                    $("#inputRequisicion").data("kendoComboBox").value("");
                    requisicionOriginal = 0;
                    $("#inputFuente").data("kendoComboBox").setDataSource();
                    $("#inputFuente").data("kendoComboBox").value("");
                    $("#inputFuente").data("kendoComboBox").dataSource.data([]);
                    $("#inputTurno").data("kendoComboBox").setDataSource();
                    $("#inputTurno").data("kendoComboBox").value("");
                    $("#inputTurno").data("kendoComboBox").dataSource.data([]);
                }
        }
    });

    $("#inputRequisicion").closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            $("#btnAgregar").trigger("click");
        }
    });


    $("#inputRequisicion").data("kendoComboBox").input.on("focus", function () {
        previousCurrentItem = this.value;
    });
}

function suscribirEventoFuente() {
    $('#inputFuente').kendoComboBox({
        dataTextField: "NombreEquipo",
        dataValueField: "EquipoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if(dataItem.NombreEquipo != "")
                    AjaxTurno();
                else {
                    $("#inputFuente").data("kendoComboBox").text("");
                    $("#inputTurno").data("kendoComboBox").setDataSource();
                    $("#inputTurno").data("kendoComboBox").value("");
                    $("#inputTurno").data("kendoComboBox").dataSource.data([]);
                }
            }
            else {
                
                $("#inputFuente").data("kendoComboBox").text("");
                $("#inputTurno").data("kendoComboBox").setDataSource();
                $("#inputTurno").data("kendoComboBox").value("");
                $("#inputTurno").data("kendoComboBox").dataSource.data([]);
                
            }
        }
    });
}

function suscribirEventTurno() {
    $('#inputTurno').kendoComboBox({
        dataTextField: "Turno",
        dataValueField: "TurnoLaboralID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputFuente").data("kendoComboBox").text("");
            }
           
        }
    });
}


function suscribirEventoPlanchar() {
    $("#btnPlanchar").click(function (e) {
        e.preventDefault();

        var Equipo = $("#inputFuente").data("kendoComboBox").dataItem($("#inputFuente").data("kendoComboBox").select());
        var Turno = $("#inputTurno").data("kendoComboBox").dataItem($("#inputTurno").data("kendoComboBox").select());

        var EsSector = $('input:radio[name=SelectSector]:checked').val();
        var Resultado = $('input:radio[name=SelectTodos]:checked').val();

        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
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

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                             "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                             + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                             + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    ventanaConfirm.close();
                    if (Equipo != undefined) {
                            PlanchaEquipoTurno(Equipo,Turno);
                    }
                    if (EsSector != "Ninguno") {
                        PlanchaEvaluacion(EsSector);
                    }
                    if (Resultado != "Ninguno") {
                        PlanchaResultado(Resultado);
                    }
                    
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            }
            else {
                if (Equipo != undefined) {
                        PlanchaEquipoTurno(Equipo,Turno);
                }
                if (EsSector != "Ninguno") {
                    PlanchaEvaluacion(EsSector);
                }
                if (Resultado != "Ninguno") {
                    PlanchaResultado(Resultado);
                }

            }
        }
    });

}



function suscribirEventoPlancharPopUp() {
    $("#btnPlancharPopUp").click(function (e) {
        e.preventDefault();

        var Resultado = $('input:radio[name=SelectTodosPop]:checked').val();

        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            if ($('input:radio[name=LLenaPop]:checked').val() === "Todos") {
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

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                             "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                             + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                             + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {

                    if (Resultado != "Ninguno") {
                        PlanchaResultadoPop(Resultado);
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            }
            else {

                if (Resultado != "Ninguno") {
                    PlanchaResultadoPop(Resultado);
                }

            }
        }
    });
}
