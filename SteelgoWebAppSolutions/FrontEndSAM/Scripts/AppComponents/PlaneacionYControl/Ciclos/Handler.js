/// <reference path="C:\Users\Fernando\Source\Repos\SAM3.0\SteelgoWebAppSolutions\FrontEndSAM\Views/Armado/CapturaArmado.cshtml" />
/// <reference path="C:\Users\Fernando\Source\Repos\SAM3.0\SteelgoWebAppSolutions\FrontEndSAM\Views/PlaneacionYControl/AutorizacionPeticion.cshtml" />
/// <reference path="C:\Users\Fernando\Source\Repos\SAM3.0\SteelgoWebAppSolutions\FrontEndSAM\Views/PlaneacionYControl/Ciclos.cshtml" />
function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoSeleccionar();
    SubscribirEventoGuardar();
}

SuscribirEventos();
function SuscribirEventoProyecto() {
    $('#inputProyecto').kendoComboBox({
        dataTextField: "Proyecto",
        dataValueField: "ProyectoID ",

        suggest: true,
        filter: "contains",
        index: 3,
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());

            if (dataItem != undefined) {
                // $("#inputPatio").data("kendoComboBox").value("");
                //$("#inputPatio").data("kendoComboBox").dataSource.data(dataItem.ListaPatio);
            }
            else {
                displayMessage("errorNoExisteProyecto", '', '2');
            }
        },
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {

            }
            else {
                displayMessage("errorNoExisteProyecto", '', '2');
            }
        }

    });
}

function SuscribirEventoSeleccionar1() {
    $("input[name='Cerrar']").change(function () {
        var columnasVisiblesFamiliaAcero = [1, 9, 13];
        var columnasVisiblesFamiliaMateriales = [3, 5, 7, 11, 15, 17, 19];
        var columnasVisiblesPWHT = [2, 10, 14];
        var columnasVisiblesPWHT = [2, 10, 14];

        var checked = $("#chkFamiliaAcero").is(':checked');

        if ($(this).attr("id") == "chkFamiliaAcero") {
            OcultarTodasColumnas(columnasVisiblesFamiliaAcero);

            if ($("#chkFamiliaMateriales").is(':checked')) {
                MostrarColumnas(columnasVisiblesFamiliaMateriales);
            }

            if (!$(this).is(':checked')) {
                OcultarColumnas(columnasVisiblesFamiliaAcero);
                if (!$("#chkFamiliaMateriales").is(':checked')) {
                    $('#chkPWHT').attr('checked', false);
                }
            }

            if ($("#chkPWHT").is(':checked')) {
                // MostrarColumnas(columnasVisiblesPWHT);
                MostrarColumnasPWHT(columnasVisiblesFamiliaAcero);
            }

        }


        if ($(this).attr("id") == "chkFamiliaMateriales") {
            OcultarTodasColumnas(columnasVisiblesFamiliaMateriales);

            if ($("#chkFamiliaAcero").is(':checked')) {
                MostrarColumnas(columnasVisiblesFamiliaAcero);
            }

            if ($("#chkPWHT").is(':checked')) {
                MostrarColumnasPWHT(columnasVisiblesFamiliaMateriales);
                //MostrarColumnas(columnasVisiblesPWHT);
            }

            if (!$(this).is(':checked')) {
                OcultarColumnas(columnasVisiblesFamiliaMateriales);
            }
        }


        if ($(this).attr("id") == "chkPWHT") {
            if ($(this).is(':checked') && $("#chkFamiliaAcero").is(':checked') ||
                $(this).is(':checked') && $("#chkFamiliaMateriales").is(':checked')) {

                if ($(this).is(':checked') && $("#chkFamiliaAcero").is(':checked')) {
                    MostrarColumnasPWHT(columnasVisiblesFamiliaAcero);
                }

                if ($(this).is(':checked') && $("#chkFamiliaMateriales").is(':checked')) {
                    MostrarColumnasPWHT(columnasVisiblesFamiliaMateriales);
                }

                //  OcultarTodasColumnas(columnasVisiblesPWHT);

            }
            else if ($(this).is(':checked')) {
                displayMessage("Mensajes_error", "Seleccione una familia", '0');
            }

            if (!$(this).is(':checked')) {
                OcultarColumnas(columnasVisiblesPWHT);
            }
        }

    });
}

function SuscribirEventoSeleccionar() {
    $("input[name='Cerrar']").change(function () {
        var columnasVisiblesFamiliaAcero = [1, 9, 13];
        var columnasVisiblesFamiliaMateriales = [3, 5, 7, 11, 15, 17, 19];
        var columnasVisiblesPWHT = [2, 10, 14];

        var checked = $("#chkFamiliaAcero").is(':checked');

        OcultarTodasColumnas();

        if ($("#chkFamiliaAcero").is(':checked')) {
            MostrarColumnas(columnasVisiblesFamiliaAcero);

            if ($("#chkPWHT").is(':checked')) {
                MostrarColumnasPWHT(columnasVisiblesFamiliaAcero);
            }
        }

        if ($("#chkFamiliaMateriales").is(':checked')) {
            MostrarColumnas(columnasVisiblesFamiliaMateriales);

            if ($("#chkPWHT").is(':checked')) {
                MostrarColumnasPWHT(columnasVisiblesFamiliaMateriales);
            }
        }

        if ((!$("#chkFamiliaAcero").is(':checked') && !$("#chkFamiliaMateriales").is(':checked')) &&
                $("#chkPWHT").is(':checked')) {
            OcultarTodasColumnas();
        }
        $(".DivScrollSup").css("width", $('table[role="grid"]').width() - 17 + 120)
    });

}

function SubscribirEventoGuardar() {
    $("#Guardar").click(function (e) {

        //$.ajax({
        //    type: "GET",
        //    url: "/MedicionesClimatologicas/CondicionesClimatologicas",
        //    success: function (result) {
        //        var w = window.open();
        //        $(w.document.body).html(result);
        //    },
        //    error: function (request, error) {
        //        debugger;
        //        console.log(arguments);
        //        alert(error);
        //    },
        //});

        //if ($(this).text() == "Editar" || $(this).text() == "Edit") {
        //    $(this).text("Guardar") 
        //    $("#GuardarPie").text("Guardar");
        //}
        //else {
        //}

        var guarda = 1;

        GuardarCliclos(guarda);


    });
}