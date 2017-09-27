function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoPeriodo();
    SuscribirEventoFecha();
    SuscribirEventoBuscar();
}

function SuscribirEventoProyecto() {
    $("#InputProyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {

            }
            else {
                $("#InputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoFecha() {
    FechaInicio = $("#InputFechaInicio").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            if (!ValidarFecha(e.sender._value)) {
                $("#InputFechaInicio").data("kendoDatePicker").value("");
            }
            
        }
    });

    FechaInicio.on("keydown", function (e) {
        if (e.keyCode == 13) {
            if (!ValidarFecha($("#InputFechaInicio").data("kendoDatePicker").value())) {
                $("#InputFechaInicio").data("kendoDatePicker").value("");
            }
        }

        if (e.keyCode == 9) {
            if (!ValidarFecha($("#InputFechaInicio").data("kendoDatePicker").value())) {
                $("#InputFechaInicio").data("kendoDatePicker").value("");
            }
        }
    });

    $("#InputFechaInicio").blur(function (e) {
        if (!ValidarFecha($("#InputFechaInicio").data("kendoDatePicker").value())) {
            $("#InputFechaInicio").data("kendoDatePicker").value("");
        }
    });

    FechaFin = $("#InputFechaFin").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            if (!ValidarFecha(e.sender._value)) {
                $("#InputFechaFin").data("kendoDatePicker").value("");
            }
        }
    });

    FechaFin.on("keydown", function (e) {
        if (e.keyCode == 13) {
            if (!ValidarFecha($("#InputFechaFin").data("kendoDatePicker").value())) {
                $("#InputFechaFin").data("kendoDatePicker").value("");
            }
        }

        if (e.keyCode == 9) {
            if (!ValidarFecha($("#InputFechaFin").data("kendoDatePicker").value())) {
                $("#InputFechaFin").data("kendoDatePicker").value("");
            }
        }
    });

    $("#InputFechaFin").blur(function (e) {
        if (!ValidarFecha($("#InputFechaFin").data("kendoDatePicker").value())) {
            $("#InputFechaFin").data("kendoDatePicker").value("");
        }
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
            $("#InputFechaInicio").data("kendoDatePicker").value("");
            $("#InputFechaFin").data("kendoDatePicker").value("");
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

function SuscribirEventoBuscar() {
    $("#btnMostrar").click(function (e) {
        var Proyecto = $("#InputProyecto").data("kendoComboBox").dataItem($("#InputProyecto").data("kendoComboBox").select());
        var fechaInicial = $('#InputFechaInicio').val();
        var fechaFinal = $('#InputFechaFin').val();

        if (Proyecto != undefined && Proyecto.ProyectoID != 0) {
            if (fechaInicial != "") {
                if (fechaFinal != "") {
                    if (!ValidaRangoFecha()) {
                        AjaxObtenerDetalleListadoEmbarque(Proyecto.ProyectoID, fechaInicial, fechaFinal);
                    } else
                        displayNotify("MensajeSeleccionaFechaFinMenorFechaInicio", "", '1');
                } else
                    displayNotify("MensajeSeleccionaFechaFin", "", '1');
                
            }else
                displayNotify("MensajeSeleccionaFechaInicio", "", '1');
        }else
            displayNotify("MensajeSeleccionaProyecto", "", '1');
    });
}

function ValidaRangoFecha() {
    var startDate = $('#InputFechaInicio').val();
    var endDate = $('#InputFechaFin').val();
    var x = startDate.match(/\d+/g);
    var y = endDate.match(/\d+/g);
    var date1 = new Date(x[2], x[1] - 1, x[0]);
    var date2 = new Date(y[2], y[1] - 1, y[0]);

    if (date1 > date2) {
        return true;
    }

    return false;
}