function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoFecha();
    SuscribirEventoCliente();
    //suscribirEventoChangeRadioTipoBusquedas();
}

function SuscribirEventoProyecto() {
    $("#inputProyecto").kendoComboBox({
        dataSource: {
            data: ["", "ETILENO XXI"]
        },
        //dataTextField: "Nombre",
        //dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                AjaxPruebas();
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoCliente() {
    $("#inputCliente").kendoComboBox({
        dataSource: {
            data: ["", "SYPRIS TECHNOLOGIES", "EDGEN MURRAY CORPORATION", "VIAR S.P.A."]
        },
        //dataTextField: "Nombre",
        //dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                AjaxPruebas();
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoCuadrante() {
    $("#inputCuadrante").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                AjaxPruebas();
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoZona() {
    $("#inputZona").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                AjaxPruebas();
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

//function suscribirEventoChangeRadioTipoBusquedas() {
//    $('input:radio[name=Tiempo]:nth(0)').change(function () {
//        $('#divPeriodo').hide();
//        $('#FechaInicialDiv').show();
//        $('#FechaFinalDiv').show();
//    });
//    $('input:radio[name=Tiempo]:nth(1)').change(function () {
//        $('#FechaInicialDiv').hide();
//        $('#FechaFinalDiv').hide();
//        $('#divPeriodo').show();
//    });
//}