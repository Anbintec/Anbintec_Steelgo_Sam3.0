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
        if ($('#botonGuardar').text() == "Guardar") {
            AjaxGuardarCaptura(0);
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo, #btnGuardarYNuevoPie').click(function (e) {
        if ($('#botonGuardar').text() == "Guardar") {
            AjaxGuardarCaptura(1);
        }
        else if ($('#botonGuardar').text() == "Editar") {
            AjaxGuardarCaptura(1);
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

function opcionHabilitarView(valor, name) {
    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);

        $('#Guardar, #btnGuardar, #GuardarPie, #btnGuardarPie').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);

        $('#Guardar, #btnGuardar, #GuardarPie, #btnGuardarPie').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);

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
        $('#Guardar, #').text(_dictionary.textoEditar[$("#language").data("kendoDropDownList").value()]);
        $("#GuardarPie").text(_dictionary.textoEditar[$("#language").data("kendoDropDownList").value()]);
        
        $('#FieldSetView').find('*').attr('disabled', true);
    }
    else {
        $('#botonGuardar2').text(_dictionary.textoGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.textoGuardar[$("#language").data("kendoDropDownList").value()]);

        $('#FieldSetView').find('*').attr('disabled', false);

        //$("#inputPatio").data("kendoComboBox").enable(true);
        //$("#inputZona").data("kendoComboBox").enable(true);
        //$("#inputMedicionesfechaToma").data("kendoComboBox").enable(true);
        //$("#inputMedicionesHoraToma").data("kendoComboBox").enable(true);
        //$("#inputMedicionesTempAmbiente").data("kendoComboBox").enable(true);
        //$("#inputEquipoTomaTempAmb").data("kendoComboBox").enable(true);
        //$("#inputMedicionesHumedad").data("kendoComboBox").enable(true);
        //$("#inputEquipoTomaHumedad").data("kendoComboBox").enable(true);
        //$("#inputMedicionesPuntoRocio").data("kendoComboBox").enable(true);
        //$("#inputEquipoTomaPtoRocio").data("kendoComboBox").enable(true);
        //$("#inputMedicionesCampoX").data("kendoComboBox").enable(true);
        //$("#inputEquipoTomaCampoX").data("kendoComboBox").enable(true);
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