
function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirFechaInicio();    
    SuscribirEventoTipoPrueba();
    SuscribirEventoProveedor();  
    suscribirEventoChangeRadio();
    suscribirEventoElementos();
    suscribirEventobuscar();
    SuscribirEventoCerrarPopUpJuntas();    
    SuscribirEventoPeriodo();    
}

function suscribirEventoElementos() {
    $(document).on('click', '.EnlaceDetalleElementos', function (e) {
        e.preventDefault();
        var grid = $("#grid").data("kendoGrid");
        dataItem = grid.dataItem($(e.target).closest("tr"));
        LlenarGridPopUp(dataItem);

    });
}

function SuscribirEventoCerrarPopUpJuntas() {
    $("#CerrarDetalleJunta").click(function (e) {
        e.preventDefault();
        $("#windowGrid").data("kendoWindow").close();
    });
}

function suscribirEventobuscar() {
    $("#botonBuscar").click(function (e) {
        e.preventDefault();
        $("#windowGrid").data("kendoWindow").close();
    });
}

function SuscribirEventoPeriodo() {
    $("#InputPeriodo").kendoComboBox({
        dataTextField: "Periodo",
        dataValueField: "PeriodoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            $("#inputFechaInicio").val("");
            $("#InputFechaFin").val("");
            if (dataItem != undefined) {
                if (dataItem.PeriodoID != 0) {
                    AjaxCargarRangoFechas(dataItem);
                }
            }
            else {
                $("#InputPeriodo").data("kendoComboBox").value("");
            }
        }
    });
}


function SuscribirFechaInicio() {        
    $("#inputFechaInicio").kendoDatePicker({        
        max: new Date(),                
    });    
    $("#inputFechaFin").kendoDatePicker({
        max: new Date(),
    });    
}

function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        $('.porElemento').css('display', 'inline-block');
        $('.porRequisicion').css('display', 'none');
    });
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        $('.porElemento').css('display', 'none');
        $('.porRequisicion').css('display', 'inline-block');
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
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem.ProyectoID != 0 && dataItem.ProyectoID != undefined) {
                AjaxPruebas();                
                AjaxCargarHeaderDashboard();
                OcultarCampos(false);
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
                OcultarCampos(true);
            }
        }
    });
}

function SuscribirEventoTipoPrueba() {
    $("#inputTipoPrueba").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TipoPruebaID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                AjaxObtenerProveedor();
                
            }
            else {
                $("#inputTipoPrueba").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoProveedor() {
    $("#inputProveedor").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProveedorID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {

            }
            else {
                $("#inputProveedor").data("kendoComboBox").value("");
            }
        }
    });
}


function ActivarRefrescarGrid(idBoton) {
    $("#contenidoDashboard").css('display', 'block');

    $("#tabEstatus").html("");
    tabActivo(idBoton);
    AjaxAccionesListado(idBoton);
}
