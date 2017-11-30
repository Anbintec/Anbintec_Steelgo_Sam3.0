function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoFecha();
    SuscribirEventoCliente();
    SuscribirEventoPeriodo();
    suscribirEventoBuscar();
    changeRadioElementos();
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

            if (dataItem != undefined) {
                
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoCliente() {
    $("#inputCliente").kendoComboBox({
        dataTextField: "Cliente",
        dataValueField: "ClienteID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoFecha() {
    endRangeDate = $("#inputFechaInicial").kendoDatePicker({
        max: new Date()

    });
    endRangeDateV = $("#inputFechaFinal").kendoDatePicker({
        max: new Date()

    });
};

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
                    console.log(dataItem);
                }
            }
            else {
                $("#InputPeriodo").data("kendoComboBox").value("");
            }
        }
    });
}

function suscribirEventoBuscar() {
    $("#btnBuscar").click(function (e) {
        
        if ($("#inputProyecto").data("kendoComboBox").text() != "") {
            AjaxCargarHeaderDashboard();    
        }
        else {
            displayNotify("","Elije un proyecto, para consultar el dashboard",1);
        }
    });
}



function changeRadioElementos() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        mostrarFiltro();
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        mostrarFiltro();
    });
    $('input:radio[name=Muestra]:nth(2)').change(function () {
        mostrarFiltro();
    });
}