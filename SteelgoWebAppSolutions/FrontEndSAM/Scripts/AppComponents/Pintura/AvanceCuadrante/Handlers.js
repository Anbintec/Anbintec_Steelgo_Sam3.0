function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoZona();
    SuscribirEventoCuadrante();
    SuscribirEventoSistemaPintura();
    SuscribirEventoOrdenTrabajo();
    SuscribirEventoSpoolID();
    SuscribirEventoMostrar();
    SuscribirEventoGuardar();
    SuscribirEventoColor();
    suscribirEventoSeleccionProcesoPintura();
    suscribirEventoWindowsConfirmaCapturaCambioProcesoPintura();
    suscribirEventoPintor();
    suscribirEventoFechaProceso();
    suscribirEventoWindowsConfirmaLineaCaptura();
    SuscribirEventoAgregar();
    SuscribirEventoPlanchar();
};


function SuscribirEventoPlanchar() {
    $("#ButtonPlanchar").click(function (e) {
        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
                windowTemplate = kendo.template($("#windowTemplate").html());

                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.PinturaCargaTitulo[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    animation: false,
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                             "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    plancharTodo();
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            }
            else {
                plancharTodo();
            }
        }
    });
}

function SuscribirEventoAgregar() {
    $("#btnAgregar").click(function () {
        if ($("#InputID").data("kendoComboBox").value() > 0) {
            AjaxAgregarSpool($("#InputID").data("kendoComboBox").value());
        }
        else {
            displayNotify("PinturaCargaSeleccionaSpool", "", '1');
        }
    });
}


function suscribirEventoWindowsConfirmaLineaCaptura() {
    ventanaConfirmEdicionCaptura = $("#ventanaConfirmCapturaLineaCaptura").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicionCaptura.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProySinTipoBusquedaProyecto'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProySinTipoBusquedaProyecto'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProySinTipoBusquedaProyecto").click(function (e) {
        ventanaConfirmEdicionCaptura.close();
        editado = false;
        LimpiarDespuesCambioCaptura();
        switch (elementoEjecutoChange) {
            case 1:
                $("#inputProyecto").data("kendoComboBox").trigger("change");
                break;
            case 2:
                $("#inputZona").data("kendoComboBox").trigger("change");
                break;
            case 3:
                $("#inputCuadrante").data("kendoComboBox").trigger("change");
                break;
            case 4:
                $("#inputSistemaPintura").data("kendoComboBox").trigger("change");
                break;
            case 5:
                $("#inputColor").data("kendoComboBox").trigger("change");
                break;
            case 6:
                BuscarDetalle();
                break;
        }
    });
    $("#noButtonProySinTipoBusquedaProyecto").click(function (e) {
        $("#inputProyecto").data("kendoComboBox").value(LineaCaptura.proyectoIDSeleccionado);
        $("#inputZona").data("kendoComboBox").value(LineaCaptura.zonaIDSeleccionado);
        $("#inputCuadrante").data("kendoComboBox").value(LineaCaptura.cuadranteIDSeleccionado);
        $("#inputSistemaPintura").data("kendoComboBox").value(LineaCaptura.sistemaPinturaIDSeleccionado);
        $("#inputColor").data("kendoComboBox").value(LineaCaptura.ColorIDSeleccionado);
        ventanaConfirmEdicionCaptura.close();
    });
}

function suscribirEventoFechaProceso() {
    endRangeDate=$("#inputFechaProceso").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            ValidarFechaPrimario(e.sender._value);
        }
    });

    endRangeDate.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]
    });
}

function suscribirEventoPintor() {

    $("#inputPintor").kendoMultiSelect({
        dataSource: '',
        dataTextField: "Codigo",
        dataValueField: "ObreroID",
        suggest: true,
        filter: "contains"
    }).data("kendoMultiSelect");
}

function suscribirEventoWindowsConfirmaCapturaCambioProcesoPintura() {
    ventanaConfirmEdicionCambioProcesoPintura = $("#ventanaConfirmCapturaProcesoPintura").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicionCambioProcesoPintura.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonCambioProcesoPintura'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonCambioProcesoPintura'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonCambioProcesoPintura").click(function (e) {
        LimpiarDespuesCambioProcesoPintura();
        ventanaConfirmEdicionCambioProcesoPintura.close();
        editado = false;

        CambiarProcesoPintura();
    });
    $("#noButtonCambioProcesoPintura").click(function (e) {
        eventoRegresarTipoListado();
        ventanaConfirmEdicionCambioProcesoPintura.close();
    });
}


function eventoRegresarTipoListado() {

    switch (procesoPinturaSeleccionadoAnterior) {
        case "1":
            $('input:radio[name=ProcesoPintura]:nth(0)').attr('checked', true);
            $('input:radio[name=ProcesoPintura]:nth(0)').trigger("click");
            break;
        case "2":
            $('input:radio[name=ProcesoPintura]:nth(1)').attr('checked', true);
            $('input:radio[name=ProcesoPintura]:nth(1)').trigger("click");
            break;
        case "3":
            $('input:radio[name=ProcesoPintura]:nth(2)').attr('checked', true);
            $('input:radio[name=ProcesoPintura]:nth(2)').trigger("click");
            break;
        case "4":
            $('input:radio[name=ProcesoPintura]:nth(3)').attr('checked', true);
            $('input:radio[name=ProcesoPintura]:nth(3)').trigger("click");
            break;
    }
}

function suscribirEventoSeleccionProcesoPintura() {
    $('input:radio[name=ProcesoPintura]').change(function () {
        CambiarProcesoPintura();
    });
}

function SuscribirEventoZona() {

    $("#inputZona").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ZonaID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            elementoEjecutoChange = 2;
            if (!editado) {
                if (dataItem != undefined) {
                    
                    LineaCaptura.zonaIDSeleccionado = dataItem.ZonaID;
                    $("#inputCuadrante").data("kendoComboBox").dataSource.data([]);
                    $("#inputCuadrante").data("kendoComboBox").value("");
                    $("#inputSistemaPintura").data("kendoComboBox").value("");
                    $("#inputColor").data("kendoComboBox").value("");
                    AjaxCuadrante(dataItem.ZonaID);
                }
            }
            else {
                ventanaConfirmEdicionCaptura.open().center();
            }
        }
    });
}

function SuscribirEventoCuadrante() {
    $('#inputCuadrante').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "CuadranteID ",
        suggest: true,
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            elementoEjecutoChange = 3;
            if (!editado) {
                if (dataItem != undefined && dataItem.CuadranteID != 0) {
                    LineaCaptura.cuadranteIDSeleccionado = dataItem.CuadranteID;
                    $("#inputSistemaPintura").data("kendoComboBox").value("");
                    $("#inputColor").data("kendoComboBox").value("");
                    AjaxSistemaPintura($("#inputZona").data("kendoComboBox").value(), dataItem.CuadranteID, $("#inputProyecto").data("kendoComboBox").value());
                }
            }
            else {
                ventanaConfirmEdicionCaptura.open().center();
            }
        }
    });
}

function SuscribirEventoSistemaPintura() {
    $('#inputSistemaPintura').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "SistemaPinturaID",
        suggest: true,
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            elementoEjecutoChange = 4;
            if (!editado) {
                if (dataItem != undefined) {
                    LineaCaptura.sistemaPinturaIDSeleccionado = dataItem.SistemaPinturaID;
                    $("#inputColor").data("kendoComboBox").value("");
                    AjaxColores($("#inputZona").data("kendoComboBox").value(), $("#inputCuadrante").data("kendoComboBox").value(), dataItem.SistemaPinturaProyectoID);
                }
            }
            else {
                ventanaConfirmEdicionCaptura.open().center();
            }
        }

    });
}

function SuscribirEventoOrdenTrabajo() {

    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").value("");
        $("#InputID").data("kendoComboBox").setDataSource();
    });

    $("#InputOrdenTrabajo").blur(function (e) {

        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                $("#InputID").data("kendoComboBox").enable(false);
                AjaxObtenerSpoolID();
            } catch (e) {
                displayNotify("Mensajes_error", e.message, '2');

            }
        } else {
            displayNotify("CapturaArmadoMensajeOrdenTrabajo", "", '1');

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
        delay: 10
        
    });

    $('#InputID').closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 37) {
            $("#InputOrdenTrabajo").focus();

        }
        else if (e.keyCode == 40) {
            $("#InputID").data("kendoComboBox").select();
        }
        else if (e.keyCode == 13) {
            if ($("#InputID").data("kendoComboBox").value() > 0) {
                AjaxAgregarSpool($("#InputID").data("kendoComboBox").value());
            }
            else {
                displayNotify("PinturaCargaSeleccionaSpool", "", '1');
            }
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

function SuscribirEventoMostrar() {
    $("#btnMostrar").click(function () {
        elementoEjecutoChange = 6;
        BuscarDetalle();
    });
}

function SuscribirEventoGuardar() {
    $("#btnGuardarYNuevo, #btnGuardarYNuevo2").click(function () {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxGuardarAvanceCarro(ds._data, true);
    });

    $("#Guardar, #btnGuardar, #GuardarPie, #btnGuardar4").click(function (e) {
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            AjaxGuardarAvanceCarro(ds._data, false);
        }
        else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "FieldSetView")
        }
    });

}

function SuscribirEventoColor() {
    $('#inputColor').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ColorID",
        suggest: true,
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            elementoEjecutoChange = 5;
            if (!editado) {
                
                if ($("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()) != undefined) {
                    LineaCaptura.ColorIDSeleccionado = dataItem.ColorID;
                }
                else {
                    $("#inputColor").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionCaptura.open().center();
            }
        }
    });
    $('#inputColor').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()) != undefined) {
                //PlanchaColor();
            }
            else {
                $("#inputColor").data("kendoComboBox").value("");
            }

        }
    });
}

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("input:radio[name=LLena]").prop('disabled', true);
        $("#InputOrdenTrabajo").attr('disabled', true);
        $("#InputID").data("kendoComboBox").enable(false);

        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputZona").data("kendoComboBox").enable(false);

        $("#inputCuadrante").data("kendoComboBox").enable(false);
        $("#inputColor").data("kendoComboBox").enable(false);
        $("#inputFechaProceso").data("kendoDatePicker").enable(false);
        $("#inputPintor").data("kendoMultiSelect").enable(false);
        $("#inputSistemaPintura").data("kendoComboBox").enable(false);
        
        $("#stylePPShotBlast").attr("disabled", true);
        $("#stylePPPrimario").attr("disabled", true);
        $("#stylePPIntermedio").attr("disabled", true);
        $("#stylePPAvanzado").attr("disabled", true);
        $("#btnMostrar").attr("disabled", true);
        BloquearElementosDinamicos(false);

        $("#btnAgregar").attr("disabled", true);
        $("#ButtonPlanchar").attr("disabled", true);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#GuardarPie").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputZona").data("kendoComboBox").enable(true);
        $("#InputOrdenTrabajo").attr('disabled', true);
        $("#InputID").data("kendoComboBox").enable(false);

        $("#stylePPShotBlast").attr("disabled", false);
        $("#stylePPPrimario").attr("disabled", false);
        $("#stylePPIntermedio").attr("disabled", false);
        $("#stylePPAvanzado").attr("disabled", false);
        $("#btnMostrar").attr("disabled", false);
        BloquearElementosDinamicos(true);

        $("input:radio[name=LLena]").prop('disabled', false);
        $("#inputCuadrante").data("kendoComboBox").enable(true);
        $("#inputColor").data("kendoComboBox").enable(true);
        $("#inputFechaProceso").data("kendoDatePicker").enable(true);
        $("#inputPintor").data("kendoMultiSelect").enable(true);
        $("#inputSistemaPintura").data("kendoComboBox").enable(true);
      
        $("#InputOrdenTrabajo").attr('disabled', false);
        $("#InputID").data("kendoComboBox").enable(true);

        $("#btnAgregar").attr("disabled", false);
        $("#ButtonPlanchar").attr("disabled", false);

        $("#Guardar").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#GuardarPie").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar4").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
    }
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
            elementoEjecutoChange = 1;
            if (!editado) {
                LimpiarDespuesCambioCaptura();
                if (dataItem != undefined) {
                    if (dataItem.ProyectoID != 0) {
                        LineaCaptura.proyectoIDSeleccionado = dataItem.ProyectoID;
                        AjaxZona();
                    }
                } else {
                    $("#inputProyecto").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionCaptura.open().center();
            }
        }
    });

  
}

