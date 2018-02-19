function SuscribirEventos() {
    SuscribirEventoProceso();
    SuscribirEventoPrueba();
    SuscribirEventoSpoolID();
    SuscribirEventoBuscar();
    SuscribirEventoGuardar();
    SuscribirEventoColor();
    suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda();
    SuscribirEventoInspector();
    SuscribirEventoAplicar();
}

function SuscribirEventoAplicar() {

    $('#btnAplicarCapturaRapida').click(function (e) {
        e.preventDefault();

        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
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
                if ($("#inputInspector").val() != "") PlanchaInspector();
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });
        }
        else {
            if ($("#inputInspector").val() != "") PlanchaInspector();
        }
    });
}


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

        ventanaConfirmEdicionSinTipoBusqueda.close();
        editado = false;

        $("#grid").data("kendoGrid").dataSource.data([]);
        switch (EjecutaChange) {
            case 1:
                $("#InputID").data("kendoComboBox").trigger("change");
                break;
            case 2:
                $("#inputProceso").data("kendoComboBox").trigger("change");
                break;
            case 3:
                $("#inputPrueba").data("kendoComboBox").trigger("change");
                break;
            case 4:
                $("#inputColor").data("kendoComboBox").trigger("change");
                break;
            default:
                AjaxObtenerPruebasSpoolID($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).SpoolID, $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).ProyectoProcesoPruebaID, $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID);
        }


    });
    $("#noButtonProySinTipoBusqueda").click(function (e) {
        $("#InputID").data("kendoComboBox").value(LineaCaptura.InputIDSeleccionado);
        $("#inputProceso").data("kendoComboBox").value(LineaCaptura.ProcesoIDSeleccionado);
        $("#inputPrueba").data("kendoComboBox").value(LineaCaptura.ProyectoProcesoPruebaID);
        $("#inputColor").data("kendoComboBox").value(LineaCaptura.ColorIDSeleccionado);
        //$("#chkCerrar")[0].checked = carroActualSeleccionado.CarroCerrado;
        ventanaConfirmEdicionSinTipoBusqueda.close();
    });
}

function SuscribirEventoProceso() {

    $('#inputProceso').kendoComboBox({
        dataTextField: "ProcesoPintura",
        dataValueField: "ProcesoPinturaID ",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            EjecutaChange = 2;
            if (!editado) {

                $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputPrueba").data("kendoComboBox").value("");
                $("#inputColor").data("kendoComboBox").dataSource.data([]);
                $("#inputColor").data("kendoComboBox").value("");
                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    LineaCaptura.ProcesoIDSeleccionado = dataItem.ProcesoPinturaID;

                    if (dataItem.ProcesoPinturaID == 4)
                        $('#divColor').show();
                    else
                        $('#divColor').hide();

                    if ($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID == 4)
                        AjaxColores(dataItem.SistemaPinturaProyectoID);

                    ajaxObtenerProcesosPorSpool(dataItem.SistemaPinturaProyectoProcesoID, 2)
                }
                else {
                    $("#inputPrueba").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }

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
            if (!editado) {
                EjecutaChange = 4;
                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    LineaCaptura.ColorIDSeleccionado = dataItem.ColorID;
                }
                else {
                    $("#inputColor").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });
    $('#inputColor').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            //if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()) != undefined) {
            //    BuscarDetalleCarro();
            //}
            //else {
            //    $("#inputCarro").data("kendoComboBox").value("");
            //}
        }
    });
}

function SuscribirEventoPrueba() {

    $('#inputPrueba').kendoComboBox({
        dataTextField: "Prueba",
        dataValueField: "ProyectoProcesoPruebaID ",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            EjecutaChange = 3;
            if (!editado) {
                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    LineaCaptura.ProyectoProcesoPruebaID = dataItem.ProyectoProcesoPruebaID;
                }
                else {
                    $("#inputPrueba").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });
}

function SuscribirEventoSpoolID() {
    $('#InputID').kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor ",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            EjecutaChange = 1;
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (!editado) {
                if (dataItem != undefined) {
                    LineaCaptura.InputIDSeleccionado = dataItem.Valor;
                    $('#InformacionSpoolDiv').hide();
                    $("#inputProceso").data("kendoComboBox").dataSource.data([]);
                    $("#inputProceso").data("kendoComboBox").value("");
                    $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                    $("#inputPrueba").data("kendoComboBox").value("");
                    $("#inputColor").data("kendoComboBox").dataSource.data([]);
                    $("#inputColor").data("kendoComboBox").value("");
                    ajaxObtenerProcesosPorSpool(dataItem.Valor, 1);//1 es para el catalogo de procesos.
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });

    $("#InputOrdenTrabajo").blur(function (e) {
        if ($("#InputOrdenTrabajo").val() != "") {
            //if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                //if (!editado) {
                $("#inputProceso").data("kendoComboBox").dataSource.data([]);
                $("#inputProceso").data("kendoComboBox").value("");
                $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputPrueba").data("kendoComboBox").value("");
                $("#inputColor").data("kendoComboBox").dataSource.data([]);
                $("#inputColor").data("kendoComboBox").value("");

                AjaxObtenerSpoolID();
                //}
                //else {
                //    ventanaConfirmEdicionSinTipoBusqueda.open().center();
                //}
            } catch (e) {
                displayNotify("", e.message, '2');
            }
            //} else {
            //    $("#InputOrdenTrabajo").val("");
            //    displayNotify("PinturaCargaMensajeOrdenTrabajo", "", '1');
            //}
        }
    });



    $('#InputID').blur(function (e) {
        var spoollIDValue = $("#InputID").val().trim();
        var listaSpoolID = $("#InputID").data("kendoComboBox").dataSource._data;

        var itemIDSeleccionado = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select());
        if (listaSpoolID.length > 0) {
            for (var i = 0; i < listaSpoolID.length; i++) {
                if (TryParseInt(spoollIDValue, 0) != 0 && (TryParseInt(spoollIDValue, 0) == TryParseInt(listaSpoolID[i].IDValido, 0))) {
                    $("#InputID").data("kendoComboBox").select(0);
                    $("#InputID").data("kendoComboBox").value(listaSpoolID[i].Valor);
                    $("#InputID").data("kendoComboBox").trigger("change");
                    break;
                }
            }
        }

    });
}

function SuscribirEventoBuscar() {
    $('#btnBuscar').click(function (e) {

        if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor != "" || $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor != 0) {
            if ($("#inputProceso").data("kendoComboBox").select() > 0) {
                if ($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID == 4 ? $("#inputColor").data("kendoComboBox").select() > 0 : true) {
                    if ($("#inputPrueba").data("kendoComboBox").select() > 0) {
                        AjaxObtenerPruebasSpoolID($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).SpoolID, $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).ProyectoProcesoPruebaID, $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID);
                        AjaxMostrarInformacionSpool($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).UnidadMedida, $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).UnidadMinima, $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).UnidadMaxima);
                    }
                    else
                        displayNotify("PinturaNoPrueba", "", '1');
                }
                else
                    displayNotify("CapturaAvanceCuadranteNoColor", "", '1');
            }
            else
                displayNotify("SistemaPinturaMensajeReqProcesoPintura", "", '1');
        }
        else
            displayNotify("Despacho0028", "", '1');


    });
}

function SuscribirEventoGuardar() {
    //convertirImagen();
    $('#btnGuardarYNuevo,#btnGuardarYNuevo1').click(function (e) {
        ajaxGuardar($("#grid").data("kendoGrid").dataSource._data, 1);
    });

    $('#btnGuardar,#Guardar,#btnGuardar1,#GuardarPie').click(function (e) {
        if ($('#Guardar').text() == "Guardar") {
            ajaxGuardar($("#grid").data("kendoGrid").dataSource._data, 0);
        }
        else if ($('#Guardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });


};

function opcionHabilitarView(valor, name) {
    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);


        $('#InputID').data("kendoComboBox").enable(false);
        $("#inputProceso").data("kendoComboBox").enable(false);
        $("#inputPrueba").data("kendoComboBox").enable(false);
        $("#inputColor").data("kendoComboBox").enable(false);

        $('#botonGuardar2').text("Editar");
        $('#botonGuardar').text("Editar");
        $('#botonGuardar3').text("Editar");
        $('#botonGuardar4').text("Editar");
        $('.k-grid-add').click(function () { return false; });

    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);


        $('#InputID').data("kendoComboBox").enable(true);
        $("#inputProceso").data("kendoComboBox").enable(true);
        $("#inputPrueba").data("kendoComboBox").enable(true);
        $("#inputColor").data("kendoComboBox").enable(false);

        $('#botonGuardar2').text("Guardar");
        $('#botonGuardar').text("Guardar");
        $('#botonGuardar3').text("Guardar");
        $('#botonGuardar4').text("Guardar");
        $('.k-grid-add').unbind('click');
    }

}