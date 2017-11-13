function suscribirEventoProyecto() {
    var dataItem;
    $("#inputProyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        delay: 10,
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
        }
    });
}

function SuscribirEventos() {
    suscribirEventoProyecto();
    suscribirEventoGuardar();
    suscribirEventoZona();
    SuscribirEventoPatio();
    SubscribeMedicionesTempAmbiente();
    SubscribeMedicionesCampoX();
    SubscribeMedicionesHumedad();
    SubscribeMedicionesPuntoRocio();
    SubscribeCalendarFechaToma();
    suscribirEventoHoras();
    changeInputs();
    //suscribirEventoMinutos();
    //SubscribeNumerosDecimal();
    //SubscribeHora();

};
SuscribirEventos();

function EventoGuardar() {
    //AjaxGuardarCaptura(0);
}

function suscribirEventoGuardar() {
    $('#Guardar, #btnGuardar, #GuardarPie, #btnGuardarPie').click(function (e) {
        if ($('#Guardar').text() == "Guardar") {
            AjaxGuardarCaptura(1);
        }
        else if ($('#Guardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo, #btnGuardarYNuevoPie').click(function (e) {
        if ($('#Guardar').text() == "Guardar") {
            AjaxGuardarCaptura(2);
        }
    });
}

function suscribirEventoHoras() {
    var dataItem;
    $("#inputMedicionesHoraToma").kendoTimePicker();
}

function Limpiar() {

    $(':input', '#FieldSetView').not(':button, :submit, :reset, :hidden, :radio, :checkbox').val('');
    if ($("#Guardar").text() == "Editar" && $("#GuardarPie").text() == 'Editar') {
        $("#Guardar").text(_dictionary.textoGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#GuardarPie").text(_dictionary.textoGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}


//patios
function SuscribirEventoPatio() {
    cbxPatios = $('#inputPatio').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "PatioID ",
        suggest: true,
        filter: "contains",
        index: 0,
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            if (dataItem == undefined) {
                //displayMessage("errorNoExistePatio", '', '2');
            }

            //var patio = $(this).value();
            //alert(patio);

        },
        change: function (e) {
            if ($("#inputPatio").data("kendoComboBox").dataItem($("#inputPatio").data("kendoComboBox").select()) != undefined) {
                AjaxGetListaZona();
            }
            else {
                $("#inputPatio").data("kendoComboBox").value("");
            }
        }

    });
}

//Zonas
function suscribirEventoZona() {
    $("#inputZona").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ZonaID",
        suggest: true,
        filter: "contains",
        cascadeFrom: "cbxPatios",
        index: 3,
        change: function (e) {
            if ($("#inputZona").data("kendoComboBox").dataItem($("#inputZona").data("kendoComboBox").select()) != undefined) {
                AjaxCargarEquiposToma();
            }
            else {
                $("#inputZona").data("kendoComboBox").value("");
            }
        }
    });
}
//herramientas para mediciones 
function SubscribeMedicionesTempAmbiente() {
    $("#inputEquipoTomaTempAmb").kendoComboBox({
        dataTextField: "NombreEquipo",
        dataValueField: "EquipoTomaID",
        suggest: true,
        filter: "contains",
        index: 0
    });
}

function SubscribeMedicionesHumedad() {
    $("#inputEquipoTomaHumedad").kendoComboBox({
        dataTextField: "NombreEquipo",
        dataValueField: "EquipoTomaID",
        suggest: true,
        filter: "contains",
        index: 0
    });

}

function SubscribeMedicionesPuntoRocio() {
    $("#inputEquipoTomaPtoRocio").kendoComboBox({
        dataTextField: "NombreEquipo",
        dataValueField: "EquipoTomaID",
        suggest: true,
        filter: "contains",
        index: 0
    });

}

function SubscribeMedicionesCampoX() {
    $("#inputEquipoTomaCampoX").kendoComboBox({
        dataTextField: "NombreEquipo",
        dataValueField: "EquipoTomaID",
        suggest: true,
        filter: "contains",
        index: 0
    });

}



function SubscribeCalendarFechaToma() {
    $("#inputMedicionesfechaToma").kendoDatePicker({
        parseFormats: ["MMddyyyy"]
    });
}


//habilitar botones 
function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#botonGuardar').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        
        $('#botonGuardar3').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardarPie").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

        $("#inputMedicionesHoraToma").data("kendoTimePicker").enable(false);
        $("#inputMedicionesfechaToma").data("kendoDatePicker").enable(false);

        $('#inputMedicionesTempAmbiente').data("kendoNumericTextBox").enable(false);
        $('#inputMedicionesHumedad').data("kendoNumericTextBox").enable(false);
        $('#inputMedicionesPuntoRocio').data("kendoNumericTextBox").enable(false);
        $('#inputMedicionesCampoX').data("kendoNumericTextBox").enable(false);

        $("#inputPatio").data("kendoComboBox").enable(false);
        $("#inputEquipoTomaCampoX").data("kendoComboBox").enable(false);
        $("#inputEquipoTomaTempAmb").data("kendoComboBox").enable(false);
        $("#inputEquipoTomaHumedad").data("kendoComboBox").enable(false);
        $("#inputEquipoTomaPtoRocio").data("kendoComboBox").enable(false);
        $("#inputZona").data("kendoComboBox").enable(false);
        $('#FieldSetView').find('*').attr('disabled', true);

    }
    else {

        $('#botonGuardar').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar2").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);

        $('#botonGuardar3').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardarPie").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);

        
        $('#FieldSetView').find('*').attr('disabled', true);

        $('#inputMedicionesTempAmbiente').data("kendoNumericTextBox").enable(true);
        $('#inputMedicionesHumedad').data("kendoNumericTextBox").enable(true);
        $('#inputMedicionesPuntoRocio').data("kendoNumericTextBox").enable(true);
        $('#inputMedicionesCampoX').data("kendoNumericTextBox").enable(true);

        $("#inputMedicionesHoraToma").data("kendoTimePicker").enable(true);
        $("#inputMedicionesfechaToma").data("kendoDatePicker").enable(true);
        $("#inputPatio").data("kendoComboBox").enable(true);
        $("#inputEquipoTomaCampoX").data("kendoComboBox").enable(true);
        $("#inputEquipoTomaTempAmb").data("kendoComboBox").enable(true);
        $("#inputEquipoTomaHumedad").data("kendoComboBox").enable(true);
        $("#inputEquipoTomaPtoRocio").data("kendoComboBox").enable(true);
        $("#inputZona").data("kendoComboBox").enable(true);
        
    }
}

function DeshablilitarInputs() {
    $('#FieldSetView').find(':input').prop('disabled', true);
}


function HablilitarInputs() {
    $('#FieldSetView').find(':input').prop('disabled', false);
}

function changeInputs() {
    $('#inputMedicionesTempAmbiente').kendoNumericTextBox();
    $('#inputMedicionesHumedad').kendoNumericTextBox();
    $('#inputMedicionesPuntoRocio').kendoNumericTextBox();
    $('#inputMedicionesCampoX').kendoNumericTextBox();
}