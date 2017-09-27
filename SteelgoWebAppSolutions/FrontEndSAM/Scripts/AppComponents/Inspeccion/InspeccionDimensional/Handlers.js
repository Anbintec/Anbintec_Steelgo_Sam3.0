function SuscribirEventos() {

    SuscribirEventoSpoolID();
    SuscribirEventoInspector();
    SuscribirEventoDefecto();
    SuscribirEventoResultadoDimensional();
    suscribirEventoAgregar();
    suscribirEventoGuardar();
    suscribirEventoCancelar();
    SuscribirEventoAgregarCapturaRapida();
    suscribirEventoChangeRadio();
    SuscribirEventoFecha();
};
var eventoAgreagarGridEscrito = true;


function SuscribirEventoFecha() {
    endRangeDate = $("#FechaInspeccion").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            ValidarFecha(e.sender._value)
        }
    });

    endRangeDate.on("keydown", function (e) {
        if (e.keyCode == 13) {
            //PlanchaFecha();
        }
        //return false;
        if (e.keyCode == 9) {
            ValidarFecha($("#FechaInspeccion").data("kendoDatePicker").value());
        }
    });

    $("#FechaInspeccion").blur(function (e) {
        ValidarFecha($("#FechaInspeccion").data("kendoDatePicker").value());
    });

}

function SuscribirEventoAgregarCapturaRapida() {

    $('#btnAplicarCapturaRapida').click(function (e) {
        e.preventDefault();
        if ($('input:radio[name=LLena]:checked').val() == undefined) {
            MensajesSteelGO('LLenadoMasivo', '');
        }
        else if ($('input:radio[name=ResultadoDimensional]:checked').val() == undefined) {
            MensajesSteelGO('ResultadoDimensional', '');
        }
        else if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
                visible: false, //the window will not appear before its .open method is called
                width: "auto",
                height: "auto",
                modal: true,
                actions: [],
                animation: {
                    close: false,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.CapturaMensajeArmadoPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                if ($("#inputDefecto").val() != "") PlanchaDefecto();
                if ($("#inputInspector").val() != "") PlanchaInspector();
                if ($('input:radio[name=ResultadoDimensional]:checked').val() != undefined) PlanchadoResultadoDimensional();
                if (endRangeDate.val() != "") PlanchaFecha();
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });
        }
        else {
            if ($("#inputDefecto").val() != "") PlanchaDefecto();
            if ($("#inputInspector").val() != "") PlanchaInspector();
            if ($('input:radio[name=ResultadoDimensional]:checked').val() != undefined) PlanchadoResultadoDimensional();
            if (endRangeDate.val() != "") PlanchaFecha();
        }
    });
}


function SuscribirEventoSpoolID() {

    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        suggest: true,
        filter: "endswith",
        index: 3,
        delay: 10,
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem != undefined && dataItem.IDValido != "") {
                eventoAgreagarGridEscrito = false;
                if (dataItem.Status != "1") {
                    e.preventDefault();
                    $("#InputID").val("");
                    console.log("borrar datos");
                }
                else {
                    $("#InputID").val(dataItem.IDValido);
                    Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                    $("#LabelProyecto").text(dataItem.Proyecto);
                }
            }
        }
        ,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
        }
    });

    $("#InputOrdenTrabajo").blur(function (e) {

        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                AjaxObtenerSpoolID();
            } catch (e) {
                displayNotify('Mensajes_error', e.message, '2');
            }
        } else {
            displayNotify("DimensionalVisualMensajeOrdenTrabajo", "", '1');
        }
    });

    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").value("");
        //$("#InputID").data("kendoComboBox").setDataSource();
    });

    $("#InputID").blur(function (e) {
        if (tieneClase(e.currentTarget)) {
            $("#InputID").data("kendoComboBox").select(0);
        }
    });

    $('#InputID').closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 37) {
            $("#InputOrdenTrabajo").focus();
        }
        else if (e.keyCode == 9) {
            if (tieneClase(e.currentTarget)) {
                $("#InputID").data("kendoComboBox").select(0);
            }
        }

        else if (e.keyCode == 40) {
            $("#InputID").data("kendoComboBox").select();
        }
        else if (e.keyCode == 13) {
            if ($("#InputID").data("kendoComboBox").select() != -1) {
                if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()) != undefined && $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).IDValido != "") {
                    if ($('input:radio[name=Muestra]:checked').val() != undefined) {
                        if ($("#InputID").val() != "" && $("#InputOrdenTrabajo").val()) {
                            //AjaxobtenerDetalleDimensional($("#InputID").val());
                            AjaxObtenerJSonGrid();
                            
                        }
                    }
                    else {
                        MensajesSteelGO('radioMostrar', '')

                    }
                }
                else
                    MensajesSteelGO('InputID-SelectInvalid', '')
            }
            else {
                var valorEncontrado = false;
                var spoollIDValue = $("#InputID").val().trim();
                var listaSpoolID = $("#InputID").data("kendoComboBox").dataSource._data;
                if (listaSpoolID.length > 0) {
                    for (var i = 0; i < listaSpoolID.length; i++) {
                        if (TryParseInt(spoollIDValue, 0) != 0 && (TryParseInt(spoollIDValue, 0) == TryParseInt(listaSpoolID[i].IDValido, 0))) {
                            valorEncontrado = true;
                            $("#InputID").data("kendoComboBox").select(0);
                            $("#InputID").data("kendoComboBox").value(listaSpoolID[i].Valor);
                            AjaxObtenerJSonGrid();
                            break;
                        }
                    }

                    if (!valorEncontrado)
                    {
                        MensajesSteelGO('InputID-SelectInvalid', '')
                    }
                }
            }
        }
    });
};
function SuscribirEventoInspector() {
    $('#inputInspector').kendoComboBox({
        dataTextField: "Codigo",
        dataValueField: "ObreroID",
        suggest: true,
        filter: "contains",
        index: 3,
        delay: 10,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputInspector").data("kendoComboBox").value("");
            }
        }
    });
    $('#inputInspector').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($("#inputInspector").data("kendoComboBox").dataItem($("#inputInspector").data("kendoComboBox").select()) != undefined) {
                //PlanchaInspector();
            }
            else {
                $("#inputInspector").data("kendoComboBox").value("");
            }
        }
    });


}


function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        // if ($("#InputID").val() != "" && $("#InputOrdenTrabajo").val()) {

        FiltroMostrar(0);
        //}
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        // if ($("#InputID").val() != "" && $("#InputOrdenTrabajo").val()) {

        FiltroMostrar(1);
        //  }
    });

}

function SuscribirEventoDefecto() {

    $('#inputDefecto').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "DefectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputDefecto").data("kendoComboBox").value("");
            }
        }
    });
    $('#inputDefecto').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($("#inputDefecto").data("kendoComboBox").dataItem($("#inputDefecto").data("kendoComboBox").select()) != undefined) {
                //PlanchaDefecto();
            }
            else {
                $("#inputDefecto").data("kendoComboBox").value("");
            }

        }
    });

}

function SuscribirEventoResultadoDimensional() {

    $('input:radio[name=ResultadoDimensional]:nth(0)').change(function () {
        $("#inputDefecto").data("kendoComboBox").enable(false);
        $("#inputDefecto").data("kendoComboBox").value("");
        //PlanchadoResultadoDimensional();
    });
    $('input:radio[name=ResultadoDimensional]:nth(1)').change(function () {
        $("#inputDefecto").data("kendoComboBox").enable(true);
        $("#inputDefecto").data("kendoComboBox").value("");
        //PlanchadoResultadoDimensional();
    });
    $('input:radio[name=ResultadoDimensional]:nth(2)').change(function () {
        $("#inputDefecto").data("kendoComboBox").enable(false);
        $("#inputDefecto").data("kendoComboBox").value("");
        //PlanchadoResultadoDimensional();
    });

}



function suscribirEventoAgregar() {
    $('#btnAgregar').click(function (e) {
        e.preventDefault();
        if ($("#InputOrdenTrabajo").val() != "") {
            if (!eventoAgreagarGridEscrito) {
                if ($("#InputID").data("kendoComboBox").select() != -1) {
                    if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()) != undefined && $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).IDValido != "") {
                        if ($('input:radio[name=Muestra]:checked').val() != undefined) {
                            //AjaxobtenerDetalleDimensional($("#InputID").val());
                            AjaxObtenerJSonGrid();
                            eventoAgreagarGridEscrito = true;
                        }
                        else {
                            MensajesSteelGO('radioMostrar', '')

                        }

                    }
                    else {
                        MensajesSteelGO('InputID-SelectInvalid', '')
                    }
                }
                else {
                    var spoollIDValue = $("#InputID").val().trim();
                    var listaSpoolID = $("#InputID").data("kendoComboBox").dataSource._data;
                    var valorEncontrado = false;
                    if (listaSpoolID.length > 0) {
                        for (var i = 0; i < listaSpoolID.length; i++) {
                            if (TryParseInt(spoollIDValue, 0) != 0 && (TryParseInt(spoollIDValue, 0) == TryParseInt(listaSpoolID[i].IDValido, 0))) {
                                valorEncontrado = true;
                                $("#InputID").data("kendoComboBox").select(0);
                                $("#InputID").data("kendoComboBox").value(listaSpoolID[i].Valor);
                                AjaxObtenerJSonGrid();
                                eventoAgreagarGridEscrito = true;
                                break;
                            }
                        }

                        if (!valorEncontrado) {
                            MensajesSteelGO('InputID-SelectInvalid', '')
                        }
                    }
                }
            }
            else {
                if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select(0)) != undefined && $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).IDValido != "") {
                    if ($('input:radio[name=Muestra]:checked').val() != undefined) {
                        //AjaxobtenerDetalleDimensional($("#InputID").val());
                        AjaxObtenerJSonGrid();
                    }
                    else {
                        MensajesSteelGO('radioMostrar', '')

                    }

                }
                else {
                    MensajesSteelGO('InputID-SelectInvalid', '')
                }
            }
        }
        else
            MensajesSteelGO('InputOrdenTrabajo');
    });
}


function suscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;

        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                AjaxGuardar(ds._data, 0);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false, "FieldSetView")
            }
        }

    });


    $('#btnGuardarYNuevo').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            AjaxGuardar(ds._data, 1);
        }

    });


    $('#btnGuardarYNuevo1').click(function (e) {

        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            AjaxGuardar(ds._data, 1);
        }
    });
};
function suscribirEventoCancelar() {
    $('#btnCancelar').click(function (e) {
        e.preventDefault();
        limpiar();
    });
}
function limpiar() {

    $("#InputOrdenTrabajo").prop("disabled", false);
    $("#InputOrdenTrabajo").val("");
    // $("#InputOrdenTrabajo").focus();

    $("#InputID").data("kendoComboBox").value("");
    $("#InputID").data("kendoComboBox").enable(true);

    $("#inputDefecto").data("kendoComboBox").enable(true);
    $("#inputDefecto").data("kendoComboBox").value("");

    // $("#Junta").data("kendoComboBox").value("");

    $("#inputInspector").data("kendoComboBox").value("");

    var radioButtons = document.getElementsByName('ResultadoDimensional');
    for (var x = 0; x < radioButtons.length; x++) {
        if (radioButtons[x].checked) {
            radioButtons[x].checked = false;

        }
    }
    $("#grid").data('kendoGrid').dataSource.data([]);

}
function deshabilitaSpool() {
    $("#InputOrdenTrabajo").prop("disabled", true);
    $("#InputID").data("kendoComboBox").enable(false);
}


function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $('#MuestraDiv').find('*').attr('disabled', true);
        $("#btnAgregar").prop('disabled', true);
        $('#DivResultadoDimensionalRadio').find('*').attr('disabled', true);
        $('#DivLlenadoMasivoRadio').find('*').attr('disabled', true);
        $("#InputID").data("kendoComboBox").enable(false);
        $("#InputOrdenTrabajo").prop('disabled', true);
        $('#InputOrdenTrabajo').css('opacity', '0.6');
        $("#inputDefecto").data("kendoComboBox").enable(false);
        $("#inputInspector").data("kendoComboBox").enable(false);
        $("#FechaInspeccion").data("kendoDatePicker").enable(false);

        $('#Guardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

        $("#grid").children().prop('readonly', true);
        $('#btnAplicarCapturaRapida').prop('disabled', true);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $('#MuestraDiv').find('*').attr('disabled', false);
        $("#btnAgregar").prop('disabled', false);
        $('#DivResultadoDimensionalRadio').find('*').attr('disabled', false);
        $('#DivLlenadoMasivoRadio').find('*').attr('disabled', false);
        $("#InputID").data("kendoComboBox").enable(true);
        $("#InputOrdenTrabajo").prop('disabled', false);
        $('#InputOrdenTrabajo').css('opacity', '1');
        if ($('input:radio[name=ResultadoDimensional]:checked').val() === "Rechazado") {
            $("#inputDefecto").data("kendoComboBox").enable(true);
        }
        $("#inputInspector").data("kendoComboBox").enable(true);
        $("#FechaInspeccion").data("kendoDatePicker").enable(true);
        $('#Guardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#grid").children().prop('readonly', false);
        $('#btnAplicarCapturaRapida').prop('disabled', false);
    }
}