
function IniciarCapturaSC() {
    SuscribirEventos();
    
}
IniciarCapturaSC();
function AsignarEncabezados() {
    $("#inputFechaInicioCertificado").data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]
    });
    $("#inputFechaFinCertificado").data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]
    });

    AjaxNuevoSoldadorCertificacion();
}




function limpiarCaptura() {


    $("#inputSoldador").data("kendoComboBox").value("");


    $("#inputSoldador").data("kendoComboBox").value("");


    $("#inputNombreWPS").data("kendoComboBox").value("");

    $("#inputPasosSoldadura").data("kendoNumericTextBox").value('');

    $("#inputCedulaTuboPQR").data("kendoComboBox").value("");

    $("#inputFechaInicioCertificado").val('');
    $("#inputFechaFinCertificado").val('');
    $("#inputEspesorMinimo").data("kendoNumericTextBox").value('');
    $("#inputEspesorMaximo").data("kendoNumericTextBox").value('');
    $("#inputDiametroCalificado").data("kendoNumericTextBox").value('');
    $("#inputDiametroMinimo").data("kendoNumericTextBox").value('');

    $("#inputTipoPrueba").data("kendoComboBox").value("");

    $("#inputPosicionPQR").val('');

    $("#inputProcesoSol").data("kendoComboBox").value("");

    $("#inputFechaInicioCertificado").val('');
    $("#inputFechaFinCertificado").val('');
    $("#inputPosicionPQR").data("kendoNumericTextBox").value('');
}

function ValidarFecha(control, valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        control.value('');
        return false;
    }
    return true;
}

function startChange() {

    var startDate = $("#inputFechaInicioCertificado").data("kendoDatePicker").value(),
     endDate = $("#inputFechaFinCertificado").data("kendoDatePicker").value(),
    end = $("#inputFechaFinCertificado").data("kendoDatePicker"),
    start = $("#inputFechaInicioCertificado").data("kendoDatePicker");

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        end.min(startDate);
    }
    else if (endDate) {
        start.max(new Date(endDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}

function endChange() {

    var endDate = $("#inputFechaFinCertificado").data("kendoDatePicker").value(),
    startDate = $("#inputFechaInicioCertificado").data("kendoDatePicker").value(),
    end = $("#inputFechaFinCertificado").data("kendoDatePicker"),
    start = $("#inputFechaInicioCertificado").data("kendoDatePicker");

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        start.max(endDate);
    }
    else if (startDate) {
        end.min(new Date(startDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}



function changeLanguageCall() {
    AsignarEncabezados();
    limpiarCaptura();
    document.title = _dictionary.lblNuevoSoldadorCertificacionBreadCrum[$("#language").data("kendoDropDownList").value()]
}


function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false;
}

function HabilitarCapturaNuevoSoldadorCertificacioon(valor, name) {

    if (valor) {
        //$('#FieldSetView').find('*').attr('disabled', true);

        $("#inputSoldador").data("kendoComboBox").enable(false);
        $("#inputNombreWPS").data("kendoComboBox").enable(false);
        $("#inputTipoPrueba").data("kendoComboBox").enable(false);
        $("#inputProcesoSol").data("kendoComboBox").enable(false);
        $("#inputFechaInicioCertificado").data("kendoDatePicker").enable(false);
        $("#inputFechaFinCertificado").data("kendoDatePicker").enable(false);

        $("#inputPasosSoldadura").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
        $("#inputEspesorMinimo").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
        $("#inputPosicionPQR").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
        $("#inputDiametroCalificado").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
        $("#inputPasosSoldadura").data("kendoNumericTextBox").enable(false);
        $("#inputEspesorMinimo").data("kendoNumericTextBox").enable(false);
        $("#inputEspesorMaximo").data("kendoNumericTextBox").enable(false);
        $("#inputPosicionPQR").data("kendoNumericTextBox").enable(false);
        $("#inputDiametroCalificado").data("kendoNumericTextBox").enable(false);
        $("#inputDiametroMinimo").data("kendoNumericTextBox").enable(false);
        $("#inputCedulaTuboPQR").data("kendoComboBox").enable(false);

        $('#botonGuardar').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        //$('#FieldSetView').find('*').attr('disabled', false);
        $("#inputSoldador").data("kendoComboBox").enable(true);
        $("#inputNombreWPS").data("kendoComboBox").enable(true);
        $("#inputTipoPrueba").data("kendoComboBox").enable(true);
        $("#inputProcesoSol").data("kendoComboBox").enable(true);
        $("#inputCedulaTuboPQR").data("kendoComboBox").enable(true);

        $("#inputPasosSoldadura").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
        $("#inputEspesorMinimo").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
        $("#inputPosicionPQR").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
        $("#inputDiametroCalificado").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
        $("#inputPasosSoldadura").data("kendoNumericTextBox").enable(true);
        $("#inputEspesorMinimo").data("kendoNumericTextBox").enable(true);

        $("#inputPosicionPQR").data("kendoNumericTextBox").enable(true);
        $("#inputDiametroCalificado").data("kendoNumericTextBox").enable(true);
        $("#inputDiametroMinimo").data("kendoNumericTextBox").enable(true);
        $("#inputDiametroMinimo").data("kendoNumericTextBox").readonly();
        $("#inputEspesorMaximo").data("kendoNumericTextBox").enable(true);
        $("#inputEspesorMaximo").data("kendoNumericTextBox").readonly();

        $("#inputFechaInicioCertificado").data("kendoDatePicker").enable(true);
        $("#inputFechaFinCertificado").data("kendoDatePicker").enable(true);

        $('#botonGuardar').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function ValidarInformacionNuevoSoldadorCertificacion(tipo) {
    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    var nuevoSoldadorOk = true;
    var index = 0;

    ListaDetalles[index] = {
        SoldadorCertificacionID: "",
        Accion: "",
        ObreroID: "",
        WPSID: "",
        ProcesoSoldaduraID: "",
        TipoDePruebaID: "",
        Posicion: "",
        FechaInicioCertificado: "",
        FechaFinCertificado: "",
        CedulaTuboCalificadoID: "",
        DiametroCalificado: "",
        DiametroMinimo: "",
        EspesorMinimo: "",
        EspesorMaximo: "",
        PasosSoldadura: "",
        Estatus: 1

    };


    if (
        (parseFloat($("#inputSoldador").data("kendoComboBox").value()) <= 0) ||
        ($("#inputSoldador").val() == '') ||
        (parseFloat($("#inputCedulaTuboPQR").data("kendoComboBox").value()) <= 0) ||
        ($("#inputCedulaTuboPQR").val() == '') ||
        (parseFloat($("#inputNombreWPS").data("kendoComboBox").value()) <= 0) ||
        ($("#inputSoldador").val() == '') ||
        (parseFloat($("#inputPasosSoldadura").data("kendoNumericTextBox").value()) == 0.0) ||
        ($("#inputPasosSoldadura").data("kendoNumericTextBox").value() == null) ||
        ($("#inputPasosSoldadura").val() == '') ||
        ($("#inputFechaInicioCertificado").val() == '') ||
        ($("#inputFechaFinCertificado").val() == '') ||
        (parseFloat($("#inputEspesorMinimo").data("kendoNumericTextBox").value()) == 0.0) ||
        ($("#inputEspesorMinimo").val() == "") ||
        (parseFloat($("#inputEspesorMaximo").data("kendoNumericTextBox").value()) == 0.0) ||
        ($("#inputEspesorMaximo").val() == "") || $("#inputDiametroCalificado").val() == '' ||
        (parseFloat($("#inputDiametroCalificado").data("kendoNumericTextBox").value()) == 0.0) ||
        (parseFloat($("#inputTipoPrueba").data("kendoComboBox").value()) <= 0) ||
        ($("#inputTipoPrueba").val() == "") ||
        (parseFloat($("#inputPosicionPQR").data("kendoNumericTextBox").value()) == 0.0) ||
        ($("#inputPosicionPQR").data("kendoNumericTextBox").value() == null) ||
        (parseFloat($("#inputProcesoSol").data("kendoComboBox").value()) <= 0) ||
        ($("#inputProcesoSol").val() == "") ||
        (parseFloat($("#inputTipoPrueba").data("kendoComboBox").value()) <= 0) ||
        ($("#inputTipoPrueba").val() == '')
       ) {

        if ((parseFloat($("#inputSoldador").data("kendoComboBox").value()) <= 0) || ($("#inputSoldador").val() == ''))
            displayNotify("CapturaSoldadorCertificacionSoldadorVacio", "", '1');
        else if ((parseFloat($("#inputNombreWPS").data("kendoComboBox").value()) <= 0) || ($("#inputSoldador").val() == ''))
            displayNotify("CapturaSoldadorCertificacionNombreVacio", "", '1');
        else if ((parseFloat($("#inputProcesoSol").data("kendoComboBox").value()) <= 0) || ($("#inputProcesoSol").val() == ""))
            displayNotify("CapturaSoldadorCertificacionProcSolVacio", "", '1');
        else if (($("#inputPasosSoldadura").data("kendoNumericTextBox").value() == null) || ($("#inputPasosSoldadura").val() == ''))
            displayNotify("CapturaSoldadorCertificacionPasosVacio", "", '1');
        else if ((parseFloat($("#inputCedulaTuboPQR").data("kendoComboBox").value()) <= 0) || ($("#inputCedulaTuboPQR").val() == ''))
            displayNotify("CapturaSoldadorCertificacionCedulaVacio", "", '1');
        else if ((parseFloat($("#inputEspesorMinimo").data("kendoNumericTextBox").value()) == 0.0) || ($("#inputEspesorMinimo").val() == ""))
            displayNotify("CapturaSoldadorCertificacionEspesorVacio", "", '1');
        else if ((parseFloat($("#inputDiametroCalificado").data("kendoNumericTextBox").value()) == 0.0) || ($("#inputDiametroCalificado").val() == ""))
            displayNotify("CapturaSoldadorCertificacionDiametroVacio", "", '1');
        else if ((parseFloat($("#inputTipoPrueba").data("kendoComboBox").value()) <= 0) || ($("#inputTipoPrueba").val() == ""))
            displayNotify("CapturaSoldadorCertificacionTipoPruebaVacio", "", '1');
        else if ((parseFloat($("#inputPosicionPQR").data("kendoNumericTextBox").value()) == 0.0) || ($("#inputPosicionPQR").data("kendoNumericTextBox").value() == null))
            displayNotify("CapturaSoldadorCertificacionPosicionVacio", "", '1');
        else if (parseFloat($("#inputPasosSoldadura").data("kendoNumericTextBox").value()) <= 0)
            displayNotify("CapturaSoldadorCertificacionNoPasosMsg", "", '1');
        else if (parseFloat($("#inputEspesorMinimo").data("kendoNumericTextBox").value()) <= 0)
            displayNotify("CapturaSoldadorEspesorMsg", "", '1');
        else if (parseFloat($("#inputDiametroCalificado").data("kendoNumericTextBox").value()) <= 0)
            displayNotify("CapturaSoldadorCertificacionDiametroMsg", "", '1');
        else if (parseFloat($("#inputPosicionPQR").data("kendoNumericTextBox").value()) <= 0)
            displayNotify("CapturaSoldadorCertificacionPosicionMsg", "", '1');
        else {
            displayNotify("MensajeCamposIncorrector", "", '1');
            
        }
        nuevoSoldadorOk = false;
        ListaDetalles[index].Estatus = 0;
    }
    else if ((parseFloat($("#inputEspesorMinimo").data("kendoNumericTextBox").value()) > parseFloat($("#inputEspesorMaximo").data("kendoNumericTextBox").value()))) {
        displayNotify("SoldadorCertificacionMensajeErrorEspesor", "", '2');
        ListaDetalles[index].Estatus = 0;
    }


    Captura[0].Detalles = ListaDetalles;


    if (nuevoSoldadorOk) {

        // ListaDetalles[index].SoldadorCertificacionID =;
        ListaDetalles[index].Accion = 1;
        ListaDetalles[index].ObreroID = $("#inputSoldador").val();
        ListaDetalles[index].WPSID = $("#inputNombreWPS").val();
        ListaDetalles[index].ProcesoSoldaduraID = $("#inputProcesoSol").val();
        ListaDetalles[index].TipoDePruebaID = $("#inputTipoPrueba").val();
        ListaDetalles[index].Posicion = $("#inputPosicionPQR").val();
        ListaDetalles[index].FechaInicioCertificado = $("#inputFechaInicioCertificado").val().trim();
        ListaDetalles[index].FechaFinCertificado = $("#inputFechaFinCertificado").val().trim();
        ListaDetalles[index].CedulaTuboCalificadoID = $("#inputCedulaTuboPQR").val();
        ListaDetalles[index].DiametroCalificado = $("#inputDiametroCalificado").val();
        ListaDetalles[index].DiametroMinimo = $("#inputDiametroMinimo").val();
        ListaDetalles[index].EspesorMinimo = $("#inputEspesorMinimo").val();
        ListaDetalles[index].EspesorMaximo = $("#inputEspesorMaximo").val();
        ListaDetalles[index].PasosSoldadura = $("#inputPasosSoldadura").val();

        if (Captura[0].Detalles.length > 0) {
            AjaxValidarExisteSoldadorCertificacion(Captura[0], tipo)
        }
    }
}