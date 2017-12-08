var ventanaConfirmEdicionSinTipoBusqueda;
var ventanaConfirmEdicionSinTipoBusquedaProyecto;

function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoSpoolID();
    SuscribirEventoCarro();
    SuscribirEventoCambiarEscenario();
    suscribirEventoWindowsConfirmaCaptura();
    SuscribirEventoGuardarNuevoMedioTransporte();
    SuscribirEventoCerrarNuevoMedioTransporte();
    SuscribirEventoTipoSeleccion();
    SuscribirEventoAgregar();
    suscribirEventoCodigo();
    suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda();
    suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusquedaProyecto()
    suscribirEventoChangeMostrar();
    SuscribirEventoGuardarCaptura();
    SuscribirEventoZona();
    SuscribirEventoCuadrante();
    suscribirEventoDescargarCarro();
    SuscribirEventoEliminaRegistro();
}

function SuscribirEventoEliminaRegistro() {
    $(document).on('click', '.k-grid-Cancelar', function (e) {

        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            if (!$('#chkCerrar').is(':checked')) {
                var dataSource = $("#grid").data("kendoGrid").dataSource;
                var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

                if (dataItem.Accion == 1 || dataItem.Accion == undefined) {
                    dataSource.remove(dataItem);
                    if (dataSource._data.length == 0) // si borro y es el ultimo elemento entonces se dice que el usuario no ah editado nada.
                        editado = false;
                }
                ImprimirAreaTonelada();
            }
            else {
                displayNotify("PinturaCargaCarroCerrar", "", '1');
            }
        }

    });
}

function suscribirEventoDescargarCarro() {
    windowDownload = $("#windowDownload").kendoWindow({
        iframe: true,
        title: _dictionary.PinturaCargaTitulo[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: {
            close: false,
            open: false
        },
        actions: [
            "Close"
        ],
    }).data("kendoWindow");
    $("#btnDescargar").click(function (handler) {


        var Zona = $("#inputZonaPopup").data("kendoComboBox").dataItem($("#inputZonaPopup").data("kendoComboBox").select());
        var Cuadrante = $("#inputCuadrantePopup").data("kendoComboBox").dataItem($("#inputCuadrantePopup").data("kendoComboBox").select());

        if (Zona != undefined && Zona.ZonaID != 0) {
            if (Cuadrante != undefined && Cuadrante.CuadranteID != 0) {
                windowDownload.close();
                AjaxDescargarSpool(dataItem, Cuadrante);
            } else {
                displayNotify("EmbarqueCargaMsjErrorCuadrante", "", "1");
            }
        } else {
            displayNotify("EmbarqueCargaMsjErrorZona", "", "1");
        }

    });

    $("#btnCerrarPopup").click(function () {
        windowDownload.close();
    });
}

function SuscribirEventoCuadrante() {
    $('#inputCuadrantePopup').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "CuadranteID",
        suggest: true,
        filter: "contains",
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined && dataItem.Cuadrante != "") {

            }
            else {
                $("#inputCuadrantePopup").data("kendoComboBox").value("");
            }
        }
    });
}


function SuscribirEventoZona() {
    $('#inputZonaPopup').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ZonaID",
        suggest: true,
        filter: "contains",
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            $("#inputCuadrantePopup").data("kendoComboBox").dataSource.data([]);
            $("#inputCuadrantePopup").data("kendoComboBox").value("");

            if (dataItem != undefined) {
                if (dataItem.ZonaID != 0) {
                    AjaxCargarCuadrante(dataItem.ZonaID);
                }
                else
                    windowDownload.open().center();
            }
            else {
                $("#inputZonaPopup").data("kendoComboBox").value("");
            }
        }
    });
}

function suscribirEventoChangeMostrar() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        FiltroMostrar(0);
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        FiltroMostrar(1);
    });

}

function suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda() {
    ventanaConfirmEdicionSinTipoBusqueda = $("#ventanaConfirmCaptura").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicionSinTipoBusqueda.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProySinTipoBusqueda'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProySinTipoBusqueda'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProySinTipoBusqueda").click(function (e) {

        proyectoActualSeleccionado = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
        LimpiarInformacionAgregada();

        AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
        ventanaConfirmEdicionSinTipoBusqueda.close();
        editado = false;


    });
    $("#noButtonProySinTipoBusqueda").click(function (e) {
        $("#inputProyecto").data("kendoComboBox").value(proyectoActualSeleccionado.ProyectoID);
        $("#inputCarro").data("kendoComboBox").value(carroActualSeleccionado.MedioTransporteID);
        $("#chkCerrar")[0].checked = carroActualSeleccionado.CarroCerrado;
        ventanaConfirmEdicionSinTipoBusqueda.close();
    });
}

function suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusquedaProyecto() {
    ventanaConfirmEdicionSinTipoBusquedaProyecto = $("#ventanaConfirmCapturaProyecto").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicionSinTipoBusquedaProyecto.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProySinTipoBusquedaProyecto'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProySinTipoBusquedaProyecto'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProySinTipoBusquedaProyecto").click(function (e) {

        ventanaConfirmEdicionSinTipoBusquedaProyecto.close();
        editado = false;
        $("#inputProyecto").data("kendoComboBox").trigger("change");

    });
    $("#noButtonProySinTipoBusquedaProyecto").click(function (e) {
        $("#inputProyecto").data("kendoComboBox").value(proyectoActualSeleccionado.ProyectoID);
        $("#inputCarro").data("kendoComboBox").value(carroActualSeleccionado.MedioTransporteID)
        ventanaConfirmEdicionSinTipoBusquedaProyecto.close();
    });
}

function suscribirEventoCodigo() {
    $("#inputCodigo").keydown(function (e) {
        if (e.keyCode == 13) {
            InsertarNuevoElemento();
        }
    });
}
function SuscribirEventoAgregar() {
    $('#btnAgregar').click(function () {
        InsertarNuevoElemento();
    });
}

function SuscribirEventoTipoSeleccion() {
    $('input:radio[name=TipoSeleccion]:nth(0)').change(function () {
        $("#InputIDDiv").show();
        $("#divCodigo").hide();
        $("#inputCodigo").val('');

    });

    $('input:radio[name=TipoSeleccion]:nth(1)').change(function () {
        $("#InputIDDiv").hide();
        $("#divCodigo").show();
        $("#InputOrdenTrabajo").val('');
        $("#InputID").data("kendoComboBox").value("");
    });
}

function suscribirEventoMostrar() {
    $('#btnMostrar').click(function (e) {
        InformacionMostrada = true;
        if (!editado) {
            AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
        }
        else {
            ventanaConfirmEdicionSinTipoBusqueda.open().center();
        }
    });
}



function SuscribirEventoCerrarNuevoMedioTransporte() {
    $('#btnCerrarVentanaCrearMedioTransporte').click(function (e) {
        windowNewCarriage.close();
        if (carroActualSeleccionado != null) {
            $("#chkCerrar")[0].checked = carroActualSeleccionado.CarroCerrado;
            $("#inputCarro").data("kendoComboBox").value(carroActualSeleccionado.MedioTransporteID)
        }
    });
};

function SuscribirEventoGuardarNuevoMedioTransporte() {
    $('#btnGuardarCrearMedioTransporte').click(function (e) {
        AjaxGuardarNuevoCarro();
    });

    $('#InputNombre').keydown(function (e) {
        if ($('#InputNombre').val() != "") {
            if (e.keyCode == 13) {
                AjaxGuardarNuevoCarro();
            }
        }
    });
}


function SuscribirEventoSpoolID() {
    var dataItem;
    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        suggest: true,
        filter: "contains",
        index: 3,
        delay: 10,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                if (dataItem.Status != 1) {
                    displayNotify("", dataItem.Status, '1');
                    $("#InputID").data("kendoComboBox").value("");
                }
            }
            else {
                $("#InputID").data("kendoComboBox").value("");
            }
        }
    });

    $("#InputOrdenTrabajo").blur(function (e) {

        //if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
        try {
            $("#InputID").data("kendoComboBox").enable(false);
            AjaxObtenerSpoolID();
        } catch (e) {
            displayNotify("Mensajes_error", e.message, '2');

        }
        //} else {
        //    displayNotify("CapturaArmadoMensajeOrdenTrabajo", "", '1');
        //    //$("#InputOrdenTrabajo").focus();
        //}
    });


    $("#InputOrdenTrabajo").focus(function (e) {

        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").dataSource.data([]);
        $("#InputID").data("kendoComboBox").value("");

    });

    $('#InputID').closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 37) {
            $("#InputOrdenTrabajo").focus();

        }
        else if (e.keyCode == 40) {
            $("#InputID").data("kendoComboBox").select();
        }
        else if (e.keyCode == 13) {
            InsertarNuevoElemento();
        }
    });

    $('#InputID').blur(function (e) {
        var spoollIDValue = $("#InputID").val().trim();
        var listaSpoolID = $("#InputID").data("kendoComboBox").dataSource._data;
        var valorEncontrado = false;
        if (listaSpoolID.length > 0) {
            for (var i = 0; i < listaSpoolID.length; i++) {
                if (TryParseInt(spoollIDValue, 0) != 0 && (TryParseInt(spoollIDValue, 0) == TryParseInt(listaSpoolID[i].IDValido, 0))) {
                    valorEncontrado = true;
                    $("#InputID").data("kendoComboBox").select(0);
                    $("#InputID").data("kendoComboBox").value(listaSpoolID[i].Valor);
                    break;
                }
            }
        }
    });
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

            if (!editado) {
                //AjaxObtenerDetalleCargaCarro(dataItem.MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
                LimpiarCargaProyecto();
                if (dataItem != undefined) {

                    if (dataItem.ProyectoID != 0) {

                        AjaxCargarMedioTransporte(dataItem.ProyectoID, null);
                        proyectoActualSeleccionado = dataItem;
                    }
                } else {
                    $("#inputProyecto").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusquedaProyecto.open().center();
            }
        }
    });

    $('#inputProyecto').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            AjaxObtenerDetalleCargaCarro($("#inputCarro").data("kendoComboBox").select() == -1 ? 0 : $("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()).MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
        }
    });

}


function SuscribirEventoCarro() {
    $("#inputCarro").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "MedioTransporteID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {

            dataItemCarro = this.dataItem(e.sender.selectedIndex);

            if (dataItemCarro != undefined) {
                $("#chkCerrar")[0].checked = dataItemCarro.CarroCerrado;
                if (dataItemCarro.MedioTransporteID == -1) {
                    CargaPopupNuevoMedioTransporte();
                } else {
                    InformacionMostrada = true;
                    if (!editado) {

                        AjaxObtenerDetalleCargaCarro(dataItemCarro.MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
                    }
                    else {
                        ventanaConfirmEdicionSinTipoBusqueda.open().center();
                    }
                }
            } else {
                $("#inputCarro").data("kendoComboBox").value("");
            }
        }
    });

    $('#inputCarro').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            InformacionMostrada = true;
            if (!editado) {
                if (dataItemCarro != undefined) {
                    AjaxObtenerDetalleCargaCarro(dataItemCarro.MedioTransporteID, $('input:radio[name=TipoVista]:checked').val(), '');
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });



}

function eventoCambioTipoEscenario() {


    if ($('input:radio[name=TipoVista]:checked').val() == "Escritorio") {
        $("#AgregarSpoolManual").css('display', 'none');
        $("#grid").data("kendoGrid").showColumn("Prioridad");
        $("#grid").data("kendoGrid").showColumn("MedioTransporte");
        $("#grid").data("kendoGrid").showColumn("Seleccionado");
        $("#grid").data("kendoGrid").refresh();
        LimpiarCambioEscenario();
    }
    else if ($('input:radio[name=TipoVista]:checked').val() == "Patio") {
        $("#AgregarSpoolManual").css('display', 'block');
        $("#grid").data("kendoGrid").hideColumn("Prioridad");
        $("#grid").data("kendoGrid").hideColumn("MedioTransporte");
        $("#grid").data("kendoGrid").hideColumn("Seleccionado");
        $("#grid").data("kendoGrid").refresh();
        LimpiarCambioEscenario();
    }
}

function SuscribirEventoCambiarEscenario() {

    $('input:radio[name=TipoVista]').change(function () {
        if ($('input:radio[name=TipoVista]:checked').val() == "Escritorio") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoEscenario();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
        else if ($('input:radio[name=TipoVista]:checked').val() == "Patio") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoEscenario();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
    });
}

function suscribirEventoWindowsConfirmaCaptura() {
    ventanaConfirmEdicion = $("#ventanaCambioVista").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicion.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProy").click(function (e) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        eventoCambioTipoEscenario();
        LimpiarCambioEscenario();
        ventanaConfirmEdicion.close();
        editado = false;
    });
    $("#noButtonProy").click(function (e) {
        eventoRegresarTipoListado();
        ventanaConfirmEdicion.close();
    });
}

function eventoRegresarTipoListado() {

    if ($('input:radio[name=TipoVista]:checked').val() == "Escritorio") {
        $('input:radio[name=TipoVista]:nth(1)').attr('checked', true);
        $('input:radio[name=TipoVista]:nth(1)').trigger("click");
    }
    else if ($('input:radio[name=TipoVista]:checked').val() == "Patio") {
        $('input:radio[name=TipoVista]:nth(0)').attr('checked', true);
        $('input:radio[name=TipoVista]:nth(0)').trigger("click");
    }
}

function SuscribirEventoGuardarCaptura() {
    $('#btnGuardarYNuevo,#btnGuardarYNuevo1').click(function (e) {
        if ($('input:radio[name=TipoVista]:checked').val() == "Escritorio") {
            ajaxGuardarEscritorio($("#grid").data("kendoGrid").dataSource._data, true);
        }
        else if ($('input:radio[name=TipoVista]:checked').val() == "Patio") {
            ajaxGuardarPatio($("#grid").data("kendoGrid").dataSource._data, true);
        }
    });

    $('#btnGuardar,#Guardar, #btnGuardar1, #Guardar1 ').click(function (e) {
        if ($('input:radio[name=TipoVista]:checked').val() == "Escritorio") {
            if ($('#btnGuardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                ajaxGuardarEscritorio($("#grid").data("kendoGrid").dataSource._data, false);
            }
            else if ($('#btnGuardar').text() == "Editar") {
                opcionHabilitarView(false, "FieldSetView")
            }
        }
        else if ($('input:radio[name=TipoVista]:checked').val() == "Patio") {
            if ($('#btnGuardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                ajaxGuardarPatio($("#grid").data("kendoGrid").dataSource._data, false);
            }
            else if ($('#btnGuardar').text() == "Editar") {
                opcionHabilitarView(false, "FieldSetView")
            }
        }
    });
};


function opcionHabilitarView(valor, name) {


    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#styleEscritorio").attr("disabled", true);
        $("#stylePatio").attr("disabled", true);
        $("input[name='Muestra']").attr("disabled", true);
        $("input[name='TipoSeleccion']").attr("disabled", true);
        $("#inputCarro").data("kendoComboBox").enable(false);
        $("#chkCerrar").attr("disabled", true);
        $("#btnMostrar").attr("disabled", true);
        $("#InputOrdenTrabajo").attr("disabled", true);
        $("#InputID").data("kendoComboBox").enable(false);
        $("#inputCodigo").attr("disabled", true);
        $("#btnAgregar").attr("disabled", true);
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar3').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#styleEscritorio").attr("disabled", false);
        $("#stylePatio").attr("disabled", false);
        $("input[name='Muestra']").attr("disabled", false);
        $("input[name='TipoSeleccion']").attr("disabled", false);
        $("#inputCarro").data("kendoComboBox").enable(true);
        $("#chkCerrar").attr("disabled", false);
        $("#btnMostrar").attr("disabled", false);
        $("#InputOrdenTrabajo").attr("disabled", false);
        $("#InputID").data("kendoComboBox").enable(true);
        $("#inputCodigo").attr("disabled", false);
        $("#btnAgregar").attr("disabled", false);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar4").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar3').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}


