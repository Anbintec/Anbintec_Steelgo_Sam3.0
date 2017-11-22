function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoProceso();
    SuscribirEventoSistemaPintura();
    SuscribirEventoPrueba();
    SuscribirEventoFechaLote();
    SuscribirEventoLote();
    suscribirEventoMostrar();
    SuscribirEventoGuardar();
    suscribirEventoEnlaceDetallePrueba();
    SuscribirEventCerrarWindow();
    SuscribirEventoColor();
    SuscribirEventoVentanaPopupPruebas();
    suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda();
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
        editado = false;
        ventanaConfirmEdicionSinTipoBusqueda.close();
        $("#grid").data("kendoGrid").dataSource.data([]);

        switch (EjecutaChange) {
            case 1:
                $("#inputProyecto").data("kendoComboBox").trigger("change");
                break;
            case 2:
                $("#inputProceso").data("kendoComboBox").trigger("change");
                break;
            case 3:
                $("#inputSistemaPintura").data("kendoComboBox").trigger("change");
                break;
            case 4:
                $("#inputColor").data("kendoComboBox").trigger("change");
                break;
            case 5:
                $("#inputPrueba").data("kendoComboBox").trigger("change");
                break;
            case 6:
                $("#inputFechaLote").data("kendoDatePicker").trigger("change");
                break;
            case 7:
                $("#inputLote").data("kendoComboBox").trigger("change");
                break;
            default:
                ajaxBuscarPorLote();
        }



    });
    $("#noButtonProySinTipoBusqueda").click(function (e) {
        $("#inputProyecto").data("kendoComboBox").value(LineaCaptura.ProyectoIDSeleccionado);
        $("#inputProceso").data("kendoComboBox").value(LineaCaptura.ProcesoIDSeleccionado);
        $("#inputSistemaPintura").data("kendoComboBox").value(LineaCaptura.SistemaPinturaIDSeleccionado);
        $("#inputColor").data("kendoComboBox").value(LineaCaptura.ColorIDSeleccionado);
        $("#inputPrueba").data("kendoComboBox").value(LineaCaptura.PruebaIDSeleccionado);
        $("#inputFechaLote").data("kendoDatePicker").value(LineaCaptura.FechaSeleccionada);
        $("#inputLote").data("kendoComboBox").value(LineaCaptura.LoteIDSeleccionada);

        //$("#inputProyecto").data("kendoComboBox").value(proyectoActualSeleccionado.ProyectoID);
        //$("#inputCarro").data("kendoComboBox").value(carroActualSeleccionado.MedioTransporteID);
        //$("#chkCerrar")[0].checked = carroActualSeleccionado.CarroCerrado;
        ventanaConfirmEdicionSinTipoBusqueda.close();
    });
}

function SuscribirEventoVentanaPopupPruebas() {

    windowPopupPruebasSpool = $("#windowGrid").kendoWindow({
        modal: true,
        iframe: true,
        resizable: false,
        visible: true,
        width: "60%",
        height: "auto",
        visible: false,
        animation: false,
        actions: [],
        position: {
            top: "10px",
            left: "10px"
        },
        close: function onClose(e) {
            //var gridDataSource = $("#gridPopUp").data("kendoGrid").dataSource;
            //  gridDataSource.filter([]);
        }
    }).data("kendoWindow");

}


function SuscribirEventCerrarWindow() {
    $("#GuardarDetallePopup").click(function (e) {
        //  e.preventDefault();
        var tieneErrores = false;
        var data = $("#gridPopUp").data("kendoGrid").dataSource._data;

        for (var i = 0; i < $("#gridPopUp").data("kendoGrid").dataSource._data.length; i++) {
            $("#gridPopUp").data("kendoGrid").dataSource._data[i].RowOk = true;
            if ((data[i].Accion == undefined || data[i].Accion == 1 || data[i].Accion == 2) && (data[i].Medida == "" || data[i].FechaPrueba == "" || data[i].Medida == undefined || data[i].FechaPrueba == undefined || data[i].Medida == null || data[i].UnidadMedida == 0 || data[i].FechaPrueba == null)) {
                $("#gridPopUp").data("kendoGrid").dataSource._data[i].RowOk = false;
                tieneErrores = true;
            }
        }

        if (!tieneErrores) {
            var numeroPruebas = sumarPruebasTotalesPorSpool();
            gridRow.PruebasEjecutadas = numeroPruebas;
            gridRow.ListaDetallePruebas = $("#gridPopUp").data("kendoGrid").dataSource._data;
            $("#windowGrid").data("kendoWindow").close();
            sumarPruebasTotales();
            $("#grid").data("kendoGrid").dataSource.sync();
        }
        else {
            displayNotify("MensajeCamposIncorrectorEnRojo", "", '2');
            $("#gridPopUp").data("kendoGrid").dataSource.sync();
        }



    });

    $("#CerrarDetallePopup").click(function (e) {
        // e.preventDefault();

        $("#windowGrid").data("kendoWindow").close();
    });
}

function suscribirEventoMostrar() {
    $('#btnBuscar').click(function (e) {
        ajaxBuscarPorLote();
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
            EjecutaChange = 1;
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (!editado) {
                $("#labelPruebasEjecutadas").text("");
                $("#labelPruebasRequeridas").text("");
                $("#inputProceso").data("kendoComboBox").dataSource.data([]);
                $("#inputProceso").data("kendoComboBox").value("");

                $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
                $("#inputSistemaPintura").data("kendoComboBox").value("");

                $("#inputColor").data("kendoComboBox").dataSource.data([]);
                $("#inputColor").data("kendoComboBox").value("");

                $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputPrueba").data("kendoComboBox").value("");

                $("#inputFechaLote").data("kendoDatePicker").value("");


                $("#inputLote").data("kendoComboBox").dataSource.data([]);
                $("#inputLote").data("kendoComboBox").value("");

                if (dataItem != undefined) {
                    LineaCaptura.ProyectoIDSeleccionado = dataItem.ProyectoID;

                    AjaxCargarProcesos();
                }
                else {
                    $("#inputProyecto").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
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
                var dataItem = this.dataItem(e.sender.selectedIndex);
                $("#labelPruebasEjecutadas").text("");
                $("#labelPruebasRequeridas").text("");
                $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
                $("#inputSistemaPintura").data("kendoComboBox").value("");

                $("#inputColor").data("kendoComboBox").dataSource.data([]);
                $("#inputColor").data("kendoComboBox").value("");

                $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputPrueba").data("kendoComboBox").value("");

                $("#inputFechaLote").data("kendoDatePicker").value("");


                $("#inputLote").data("kendoComboBox").dataSource.data([]);
                $("#inputLote").data("kendoComboBox").value("");

                if (dataItem != undefined) {
                    LineaCaptura.ProcesoIDSeleccionado = dataItem.ProcesoPinturaID;
                    if (dataItem.ProcesoPinturaID == 4)
                        $('#divColor').show();
                    else
                        $('#divColor').hide();
                    ajaxObtenerSistemasPintura(dataItem.ProcesoPinturaID, $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID);
                }
                else {
                    $("#inputProceso").data("kendoComboBox").value("");
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });
}

function SuscribirEventoSistemaPintura() {
    $("#inputSistemaPintura").kendoComboBox({
        dataTextField: "SistemaPintura",
        dataValueField: "SistemaPinturaID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            EjecutaChange = 3;
            if (!editado) {
                $("#labelPruebasEjecutadas").text("");
                $("#labelPruebasRequeridas").text("");
                $("#inputColor").data("kendoComboBox").dataSource.data([]);
                $("#inputColor").data("kendoComboBox").value("");

                $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputPrueba").data("kendoComboBox").value("");

                $("#inputFechaLote").data("kendoDatePicker").value("");


                $("#inputLote").data("kendoComboBox").dataSource.data([]);
                $("#inputLote").data("kendoComboBox").value("");

                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    LineaCaptura.SistemaPinturaIDSeleccionado = dataItem.SistemaPinturaID;

                    if ($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID == 4)
                        AjaxColores(dataItem.SistemaPinturaProyectoID, 0);

                    ajaxPruebas($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID, dataItem.SistemaPinturaProyectoID, $("#language").val());
                }
                else {
                    $("#inputSistemaPintura").data("kendoComboBox").value("");
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
            EjecutaChange = 4;
            if (!editado) {
                var dataItem = this.dataItem(e.sender.selectedIndex);
                $("#labelPruebasEjecutadas").text("");
                $("#labelPruebasRequeridas").text("");
                LineaCaptura.ColorIDSeleccionado = dataItem.ColorID;

                $("#inputFechaLote").data("kendoDatePicker").value("");


                $("#inputLote").data("kendoComboBox").dataSource.data([]);
                $("#inputLote").data("kendoComboBox").value("");

                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()) != undefined) {
                    LineaCaptura.PruebaIDSeleccionado = dataItem.PruebaIDSeleccionado;
                    ajaxObtenerFechas($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID, $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select()).SistemaPinturaProyectoID, $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).PruebaProcesoPinturaID, $("#language").val(), $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID)
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
            if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()) != undefined) {
                // BuscarDetalleCarro();
            }
            else {
                $("#inputCarro").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoPrueba() {

    $('#inputPrueba').kendoComboBox({
        dataTextField: "Prueba",
        dataValueField: "PruebaProcesoPinturaID ",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            EjecutaChange = 5;
            if (!editado) {
                $("#labelPruebasRequeridas").text("");
                $("#labelPruebasEjecutadas").text("");
                $("#inputFechaLote").data("kendoDatePicker").value("");


                $("#inputLote").data("kendoComboBox").dataSource.data([]);
                $("#inputLote").data("kendoComboBox").value("");

                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    if ($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID == 4 ? $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()) != undefined :true) {
                        LineaCaptura.PruebaIDSeleccionado = dataItem.PruebaIDSeleccionado;
                        ajaxObtenerFechas($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID, $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select()).SistemaPinturaProyectoID, dataItem.PruebaProcesoPinturaID, $("#language").val(), $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID)
                    }
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



function SuscribirEventoFechaLote() {
    //var disabledDays = [
    //	new Date(2000, 01, 01),
    //	new Date(2000, 01, 02)
    //];

    $("#inputFechaLote").kendoDatePicker({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()],
        //dates: disabledDays,
        value: new Date(),
        max: new Date(),
        change: function (e) {
            EjecutaChange = 6;
            if (!editado) {
                $("#labelPruebasEjecutadas").text("");
                $("#labelPruebasRequeridas").text("");
                $("#inputLote").data("kendoComboBox").dataSource.data([]);
                $("#inputLote").data("kendoComboBox").value("");

                var dataItem = e.sender._oldText;
                if (dataItem != "") {
                    LineaCaptura.FechaSeleccionada = e.sender._oldText;
                    var ProcesoPinturaID = $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID;
                    var SistemaPinturaProyectoID = $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select()).SistemaPinturaProyectoID;
                    var PruebaProcesoPinturaID = $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).PruebaProcesoPinturaID;
                    var fecha = e.sender._oldText;
                    ajaxLlenarLote(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID, fecha, $("#language").val(), $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID);
                }

            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        },
        month: {
            content: $("#cell-template").html()
        }

    }).data("kendoDatePicker");

}

function SuscribirEventoLote() {

    $('#inputLote').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "LoteID ",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            EjecutaChange = 7;
            if (!editado) {
                $("#labelPruebasEjecutadas").text("");
                $("#labelPruebasRequeridas").text("");
                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && $("#inputLote").val() != "") {
                    LineaCaptura.LoteIDSeleccionada = dataItem.LoteID;
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


function SuscribirEventoGuardar() {
    $('#btnGuardarYNuevo,#btnGuardarYNuevo1').click(function (e) {
        ajaxGuardar($("#grid").data("kendoGrid").dataSource._data, 1);

    });

    $('#Guardar,#btnGuardar1,#btnGuardar,#Guardar1').click(function (e) {
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
        $("#inputSistemaPintura").data("kendoComboBox").enable(false);

        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputProceso").data("kendoComboBox").enable(false);
        $("#inputColor").data("kendoComboBox").enable(false);
        $("#inputPrueba").data("kendoComboBox").enable(false);
        $("#inputFechaLote").data("kendoDatePicker").enable(false);

        $("#inputLote").data("kendoComboBox").enable(false);
        $('#botonGuardar2').text("Editar");
        $('#botonGuardar').text("Editar");
        $('#botonGuardar3').text("Editar");
        $('#botonGuardar4').text("Editar");
    }

    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputSistemaPintura").data("kendoComboBox").enable(true);

        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputProceso").data("kendoComboBox").enable(true);
        $("#inputColor").data("kendoComboBox").enable(true);
        $("#inputPrueba").data("kendoComboBox").enable(true);
        $("#inputFechaLote").data("kendoDatePicker").enable(true);
        $("#inputLote").data("kendoComboBox").enable(true);

        $('#botonGuardar2').text("Guardar");
        $('#botonGuardar').text("Guardar");
        $('#botonGuardar3').text("Guardar");
        $('#botonGuardar4').text("Guardar");
    }

}

function suscribirEventoEnlaceDetallePrueba() {
    $(document).on('click', '.EnlaceDetallePrueba', function (e) {
        e.preventDefault();
        if ($('#Guardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            gridRow = $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr"));

            if (gridRow.ListaDetallePruebas == null || gridRow.ListaDetallePruebas == undefined || gridRow.ListaDetallePruebas == "")
                AjaxObtenerPruebasSpoolID(gridRow.SpoolID, gridRow.ProyectoProcesoPruebaID, $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID, gridRow.Medida, gridRow, $("#inputLote").data("kendoComboBox").dataItem($("#inputLote").data("kendoComboBox").select()).LoteID);
            else {
                $("#gridPopUp").data("kendoGrid").dataSource.data(gridRow.ListaDetallePruebas);
                verVentanaPruebasPorSpool($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).Prueba, gridRow.Medida);
                $("#gridPopUp").data("kendoGrid").dataSource.sync();
            }
        }
    });
}