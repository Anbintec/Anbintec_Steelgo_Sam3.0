function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoPeriodo();
    SuscribirEventoFecha();
    SuscribirEventoButtonMenu();
    SuscribirEventoTabMenu();
}


function SuscribirEventoProyecto() {
    $('#InputProyecto').kendoComboBox({
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
            if(!ValidarFecha($("#InputFechaInicio").data("kendoDatePicker").value())){
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

function SuscribirEventoButtonMenu() {
    $(".tbmenuEmb").click(function () {
        var activar = "#"+$(this).attr("data-bind");
        $("#tabButtonMenuEmbarque").hide();
        $("#tabGrid").show();
        $(activar).trigger("click");
    });
}

function SuscribirEventoTabMenu() {
    $("#divTabEmbarque .tabListEmb").click(function (e) {
        var idButton = $(this).attr("id");
        $(".btn-tabList").removeClass("active");

        $("#grid").data("kendoGrid").dataSource.data([]);
        $("#gridCarga").data("kendoGrid").dataSource.data([]);

        switch (idButton) {
            case "btnPorCargar":
                $("#" + idButton).addClass("active");
                $("#grid").hide();
                $("#gridCarga").show();
                break;
            case "btnPorEmbarcar":
                $("#" + idButton).addClass("active");
                $("#gridCarga").hide();
                $("#grid").data("kendoGrid").hideColumn("Embarque");
                $("#grid").show();
                break;
            case "btnPorEnviar":
                $("#" + idButton).addClass("active");
                $("#gridCarga").hide();
                $("#grid").data("kendoGrid").showColumn("Embarque");
                $("#grid").show();
                break;
            case "btnPorCargar":
                $("#" + idButton).addClass("active");
                $("#gridCarga").hide();
                $("#grid").data("kendoGrid").showColumn("Embarque");
                $("#grid").show();
                break;
            case "btnPorValidar":
                $("#" + idButton).addClass("active");
                $("#gridCarga").hide();
                $("#grid").data("kendoGrid").showColumn("Embarque");
                $("#grid").show();
                break;
            default:
                $("#btnPorCargar").addClass("active");
                $("#gridCarga").hide();
                $("#grid").data("kendoGrid").hideColumn("Plana");
                $("#grid").data("kendoGrid").showColumn("Embarque");
                $("#grid").show();
                break;
        }
    });
}

function AccionesListado(idButton) {
    switch (idButton) {
        case 1:
            loadingStart();
            setTimeout(function () {
                $("#grid").css("display", "none");
                $("#gridCarga").css("display", "block");
                loadingStop();
            }, 500);

            break;
        case 2:
            loadingStart();
            setTimeout(function () {
                $("#grid th[data-field=Plana]").html("Plana");
                $("#grid").data("kendoGrid").hideColumn("NombrePlana");
                $("#grid").css("display", "block");
                $("#gridCarga").css("display", "none");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.add({ Plana: "PL-345", Elementos: "2", M2: "2.73", KG: "135", URL: "/Embarque/EmbarqueCarro?EmbarqueID=2" });

                loadingStop();
            }, 500);



            break;
        case 3:
            loadingStart();
            setTimeout(function () {
                $("#grid th[data-field=Plana]").html("Embarque");
                $("#grid").data("kendoGrid").showColumn("NombrePlana");
                $("#grid").css("display", "block");
                $("#gridCarga").css("display", "none");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.add({ Plana: "Emb-3", NombrePlana: "PLN12", Elementos: "4", M2: "7.27", KG: "535", URL: "/Embarque/ListadoEmbarque?EmbarqueID=3" });
                loadingStop();
            }, 500);

            break;
        case 4:
            loadingStart();
            setTimeout(function () {
                $("#grid th[data-field=Plana]").html("Embarque");
                $("#grid").data("kendoGrid").showColumn("NombrePlana");
                $("#grid").css("display", "block");
                $("#gridCarga").css("display", "none");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.add({ Plana: "Emb-4", NombrePlana: "PLX-02", Elementos: "2", M2: "12.23", KG: "435", URL: "/Embarque/RevisionEmbarque?EmbarqueID=4" });
                loadingStop();
            }, 500);
            break;

    }
}

function changeLabelNumElementos() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        $('#PorCarcar').text('2');
        $('#PorEmbarcar').text('2');
        $('#PorEnviar').text('1');
        $('#PorValidar').text('1');

        $('.elementoPorCargar').text('2');
        $('.elementoPorEmbarcar').text('2');
        $('.elementoPorEnviar').text('1');
        $('.elementoPorValidar').text('1');
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        $('#PorCarcar').text('6.89');
        $('#PorEmbarcar').text('2.73');
        $('#PorEnviar').text('7.27');
        $('#PorValidar').text('12.23');

        $('.elementoPorCargar').text('6.89');
        $('.elementoPorEmbarcar').text('2.73');
        $('.elementoPorEnviar').text('7.27');
        $('.elementoPorValidar').text('12.23');
    });
    $('input:radio[name=Muestra]:nth(2)').change(function () {
        $('#PorCarcar').text('0.5147');
        $('#PorEmbarcar').text('0.135');
        $('#PorEnviar').text('0.535');
        $('#PorValidar').text('0.435');

        $('.elementoPorCargar').text('0.5147');
        $('.elementoPorEmbarcar').text('0.135');
        $('.elementoPorEnviar').text('0.535');
        $('.elementoPorValidar').text('0.435');
    });
}