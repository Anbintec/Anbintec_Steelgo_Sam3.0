var proyectoInicial;
var editado = false;

function SuscribirEventos() {
    SuscribirEventoComboProyecto();
    suscribirEventoDetallePartida();

    suscribirEventoCancelar();
    suscribirEventoGuardar();
}


function suscribirEventoDetallePartida() {

    $(document).on('click', '.EnlacePorPlaca', function (e) {
        e.preventDefault();
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            LlenarGridPopUp(dataItem);
        }
    });
}




function suscribirEventoCancelar() {
    $('#CancelarPlacas').click(function (e) {
        $("#windowGrid").data("kendoWindow").close();
    });

}

function suscribirEventoGuardar() {
    $('#GuardarPlacas').click(function (e) {
        var ds = $("#gridPopUp").data("kendoGrid").dataSource;
        if (placasCorrectas(ds)) {
            if (placasCapturadas(ds) == ds._data.length) {
                modeloRenglon.DefectosAsignados = true;
                $("#grid").data("kendoGrid").refresh();
            }
            else {
                modeloRenglon.DefectosAsignados = false;
                $("#grid").data("kendoGrid").refresh();
            }
            $("#windowGrid").data("kendoWindow").close();
        }
        else
            displayNotify("", "Favor de capturar las juntas para todos los soldadores ", '1');
    });


    //----------------------------------------GUARDAR CAPTURA TOTAL

    $('.accionGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;

        if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
            if (ds._data.length > 0 && editado) {
                AjaxGuardarCaptura(ds._data, 0);
            }
            else {
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
            }
        }
        else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
            opcionHabilitarView(false)
        }


    });

    $('.accionGuardarNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0 && editado) {
            AjaxGuardarCaptura(ds._data, 1);
        }
        else {
            displayNotify("AdverteciaExcepcionGuardado", "", '1');
        }

    });

}




function SuscribirEventoComboProyecto() {
    $('#inputProyecto').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined || dataItem.Nombre != "") {
                if (!editado) {
                    AjaxJuntas(dataItem.ProyectoID);
                    proyectoInicial = $("#inputProyecto").data("kendoComboBox").value();
                }
                else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "auto",
                        height: "auto",
                        modal: true,
                        animation: {
                            close: false,
                            open: false
                        },
                        actions: [],
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {
                        AjaxJuntas(dataItem.ProyectoID);

                        proyectoInicial = $("#inputProyecto").data("kendoComboBox").value();
                        ventanaConfirm.close();
                    });
                    $("#noButtonProy").click(function () {
                        $("#inputProyecto").data("kendoComboBox").value(proyectoInicial);
                        ventanaConfirm.close();
                    });
                }
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");

            }
        }
    });

};



function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#inputProyecto").data("kendoComboBox").enable(false);

        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputProyecto").data("kendoComboBox").enable(true);

        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}


function placasCorrectas(ds) {

    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i].Soldador != "")
            if (ds._data[i].JtaSeg1 == "" || ds._data[i].JtaSeg2 == "") {
                if (!ds._data[i].EsRepetido) {
                    return false
                }
            }

    }
    return true
}


function placasCapturadas(ds) {
    var contPlacas = 0;
    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i].Soldador != "")
            contPlacas++;
    }
    return contPlacas;
}