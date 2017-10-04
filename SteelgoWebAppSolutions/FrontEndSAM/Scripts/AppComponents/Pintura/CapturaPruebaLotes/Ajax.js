function AjaxCargarCamposPredeterminados() {
    loadingStart();
    var TipoMuestraPredeterminadoID = 4079;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (data == "sin captura") {
            $('input:radio[name=Muestra]:nth(0)').trigger("click");
        }
        else if (data == "Todos") {
            $('input:radio[name=Muestra]:nth(1)').trigger("click");
        }
        loadingStop();
    });

}

function AjaxCargarProyecto() {
    loadingStart();
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
        var proyectoId = 0;

        if (data.length > 0) {
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProyectoID != 0) {
                        proyectoId = data[i].ProyectoID;
                    }
                }
                $("#inputProyecto").data("kendoComboBox").value(proyectoId);
                $("#inputProyecto").data("kendoComboBox").trigger("change");
            }
            else
                loadingStop();

        }
        else
            loadingStop();
    });
}

function AjaxCargarProcesos() {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), proyectoID: $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID }).done(function (data) {
        $("#inputProceso").data("kendoComboBox").dataSource.data([]);
        var datoID = 0;

        if (data.length > 0) {
            $("#inputProceso").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProcesoPinturaID != 0) {
                        datoID = data[i].ProcesoPinturaID;
                    }
                }
                $("#inputProceso").data("kendoComboBox").value(datoID);
                $("#inputProceso").data("kendoComboBox").trigger("change");
            }
            else
                loadingStop();

        }
        else
            loadingStop();
    });
}

function ajaxObtenerSistemasPintura(procesoid, ProyectoiD) {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), ProcesoPinturaID: procesoid, ProyectoID: ProyectoiD }).done(function (data) {
        $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
        var datoID = 0;

        if (data.length > 0) {
            $("#inputSistemaPintura").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].SistemaPinturaID != 0) {
                        datoID = data[i].SistemaPinturaID;
                    }
                }
                $("#inputSistemaPintura").data("kendoComboBox").value(datoID);
                $("#inputSistemaPintura").data("kendoComboBox").trigger("change");
            }
            else
                loadingStop();

        }
        else
            loadingStop();
    });
}

function AjaxColores(sistemaPinturaProyectoID,cuadranteID) {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), sistemaPinturaProyectoID: sistemaPinturaProyectoID, lenguaje: $("#language").val()}).done(function (data) {
        if (Error(data)) {
            $("#inputColor").data("kendoComboBox").dataSource.data(data);
            $("#inputColor").data("kendoComboBox").value("");
            if (data.length == 2) {
                $("#inputColor").data("kendoComboBox").select(1);
                $("#inputColor").data("kendoComboBox").trigger("change");
            }
        }
        loadingStop();
    });
}

function ajaxPruebas(ProcesoPinturaID, SistemaPinturaProyectoID, lenguaje) {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), ProcesoPinturaID: ProcesoPinturaID, SistemaPinturaProyectoID: SistemaPinturaProyectoID, lenguaje: lenguaje }).done(function (data) {
        $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
        var datoID = 0;

        if (data.length > 0) {
            $("#inputPrueba").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProyectoID != 0) {
                        datoID = data[i].PruebaProcesoPinturaID;
                    }
                }
                $("#inputPrueba").data("kendoComboBox").value(datoID);

                //	if ($("#inputFechaLote").val() != "") {
                $("#inputPrueba").data("kendoComboBox").trigger("change");
                //}
                //else
                //	loadingStop();



            }
            else
                loadingStop();

        }
        else
            loadingStop();
    });
}

function ajaxObtenerFechas(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID, lenguaje, SistemaPinturaColorID) {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), ProcesoPinturaID: ProcesoPinturaID, SistemaPinturaProyectoID: SistemaPinturaProyectoID, PruebaProcesoPinturaID: PruebaProcesoPinturaID, sistemaPinturaColorID: SistemaPinturaColorID, lenguaje: lenguaje }).done(function (data) {

        var arrayDates = [];

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                arrayDates[i] = new Date(ObtenerDato(data[i].Fecha, 1), ObtenerDato(data[i].Fecha, 2), ObtenerDato(data[i].Fecha, 3));
            }

            $("#inputFechaLote").data("kendoDatePicker").setOptions({
                dates: arrayDates
            });

            if (arrayDates.length == 1) {

                $("#inputFechaLote").data("kendoDatePicker").value(kendo.toString(arrayDates[0], String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim());

            }
            $("#inputFechaLote").data("kendoDatePicker").trigger("change");
        }
        else
            loadingStop();
    });
}

function ajaxLlenarLote(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID, FechaLote, lenguaje, sistemaPinturaColorID) {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), ProcesoPinturaID: ProcesoPinturaID, SistemaPinturaProyectoID: SistemaPinturaProyectoID, PruebaProcesoPinturaID: PruebaProcesoPinturaID, FechaLote: FechaLote, lenguaje: lenguaje, sistemaPinturaColorID: sistemaPinturaColorID }).done(function (data) {
        $("#inputLote").data("kendoComboBox").dataSource.data([]);
        var datoID = 0;

        if (data.length > 0) {
            $("#inputLote").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProyectoID != 0) {
                        datoID = data[i].LoteID;
                    }
                }
                $("#inputLote").data("kendoComboBox").value(datoID);
            }
            else
                loadingStop();

            //$("#inputLote").data("kendoComboBox").trigger("change");
        }
        else
            loadingStop();
    });

}


function ajaxGuardar(data, guardarYNuevo) {
    //loadingStart();
    //displayNotify("", "se guardo correctamente la informacion", '0');
    //opcionHabilitarView(true, "FieldSetView");
    //loadingStop();

    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];


    var index = 0;
    for (var i = 0; i < data.length; i++) {
        $("#grid").data("kendoGrid").dataSource._data[i].RowOk = true;
        if ((data[i].ListaDetallePruebas == undefined || data[i].ListaDetallePruebas == null || data[i].ListaDetallePruebas == "") && data[i].PruebasEjecutadas == 0) {
            $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
        }

        for (var j = 0; j < (data[i].ListaDetallePruebas == null ? 0 : data[i].ListaDetallePruebas.length) ; j++) {
            ListaDetalles[index] = { Accion: "", SpoolID: "", ProyectoProcesoPruebaID: "", UnidadMedida: "", FechaPrueba: "", ResultadoEvaluacion: "", Estatus: 1, SistemaPinturaColorID: "",PruebaLoteID:"" };
            ListaDetalles[index].Accion = (data[i].ListaDetallePruebas[j].Accion == undefined || data[i].ListaDetallePruebas[j].Accion == 0 || data[i].ListaDetallePruebas[j].Accion == null) ? 1 : data[i].ListaDetallePruebas[j].Accion;
            ListaDetalles[index].SpoolID = data[i].SpoolID;
            ListaDetalles[index].ProyectoProcesoPruebaID = data[i].ProyectoProcesoPruebaID;
            ListaDetalles[index].UnidadMedida = data[i].ListaDetallePruebas[j].UnidadMedida;
            ListaDetalles[index].ResultadoEvaluacion = data[i].ListaDetallePruebas[j].ResultadoEvaluacion;
            ListaDetalles[index].FechaPrueba = data[i].ListaDetallePruebas[j].FechaPrueba == null ? "" : kendo.toString(data[i].ListaDetallePruebas[j].FechaPrueba, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim().split(" ")[0];
            ListaDetalles[index].SistemaPinturaColorID = $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID != 4 ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID;
            ListaDetalles[index].PruebaLoteID = (data[i].ListaDetallePruebas[j].Accion == undefined || data[i].ListaDetallePruebas[j].Accion == 0 || data[i].ListaDetallePruebas[j].Accion == null) ? $("#inputLote").data("kendoComboBox").dataItem($("#inputLote").data("kendoComboBox").select()).LoteID : data[i].ListaDetallePruebas[j].PruebaLoteID;
            index++;
        }
    }
    Captura[0].Detalles = ListaDetalles;

    if (ListaDetalles.length > 0) {
        if (!ExistRowErrors($("#grid").data("kendoGrid").dataSource._data)) {
            if (Captura[0].Detalles.length > 0) {
                AjaxEjecutarGuardado(Captura[0], guardarYNuevo);
            }
            else {
                loadingStop();
            }
        }
        else {
            loadingStop();
            $("#grid").data("kendoGrid").dataSource.sync();
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopUpError[$("#language").data("kendoDropDownList").value()],
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

            ventanaConfirm.content(_dictionary.MensajeConfirmacionGuardadoGeneral[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function () {
                loadingStart();

                ArregloGuardado = [];
                var indice = 0;
                for (var i = 0; i < Captura[0].Detalles.length; i++) {
                    if (Captura[0].Detalles[i].Estatus == 1) {
                        ArregloGuardado[indice] = ListaDetalles[i];
                        indice++;
                    }
                }

                Captura[0].Detalles = [];
                Captura[0].Detalles = ArregloGuardado;

                if (Captura[0].Detalles.length > 0) {

                    AjaxEjecutarGuardado(Captura[0], guardarYNuevo);
                }
                else {
                    loadingStop();
                    displayNotify("AdverteciaExcepcionGuardado", "", '1');
                }

                ventanaConfirm.close();
            });

            $("#noButton").click(function () {
                ventanaConfirm.close();
            });

        }
    }
    else {
        loadingStart();
        if (guardarYNuevo == 1) {
            if (editado)
                displayNotify("MensajeGuardadoExistoso", "", '0');

            opcionHabilitarView(false, "FieldSetView");
            Limpiar();
            editado = false;
        }
        else
        {
            displayNotify("MensajeGuardadoExistoso", "", '0');
            opcionHabilitarView(true, "FieldSetView");
            editado = false;
        }
        loadingStop();
    }
};

function ajaxBuscarPorLote() {

    if ($("#inputProyecto").data("kendoComboBox").select() > 0) {
        if ($("#inputProceso").data("kendoComboBox").select() > 0) {
            if ($("#inputSistemaPintura").data("kendoComboBox").select() > 0) {
                if ($("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID == 4 ? $("#inputColor").data("kendoComboBox").select() > 0 : true) {
                    if ($("#inputPrueba").data("kendoComboBox").select() > 0) {
                        if ($("#inputLote").data("kendoComboBox").dataItem($("#inputLote").data("kendoComboBox").select()) != undefined && $("#inputLote").data("kendoComboBox").dataItem($("#inputLote").data("kendoComboBox").select()).LoteID != 0) {
                            loadingStart();
                            $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), procesoPinturaID: $("#inputProceso").data("kendoComboBox").dataItem($("#inputProceso").data("kendoComboBox").select()).ProcesoPinturaID, sistemaPinturaProyectoID: $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select()).SistemaPinturaProyectoID, pruebaProcesoPinturaID: $("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).PruebaProcesoPinturaID, sistemaPinturaColorID: ($("#inputColor").data("kendoComboBox").select() <= 0) ? 0 : $("#inputColor").data("kendoComboBox").dataItem($("#inputColor").data("kendoComboBox").select()).SistemaPinturaColorID, loteID: $("#inputLote").data("kendoComboBox").dataItem($("#inputLote").data("kendoComboBox").select()).LoteID, lenguaje: $("#language").val(), todosSinCaptura: ($('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 0) }).done(function (array) {
                                $("#grid").data("kendoGrid").dataSource.data([]);
                                if (array.length > 0) {
                                    $("#labelPruebasRequeridas").text(array[0].PruebasRequeridas);
                                    var ds = $("#grid").data("kendoGrid").dataSource;
                                    for (var i = 0; i < array.length; i++) {
                                        if (array[i].FechaPrueba != null) {
                                            array[i].FechaPrueba = new Date(ObtenerDato(array[i].FechaPrueba, 1), ObtenerDato(array[i].FechaPrueba, 2), ObtenerDato(array[i].FechaPrueba, 3));//año, mes, dia
                                        }
                                        ds.insert(0, array[i]);
                                    }
                                    //$("#grid").data("kendoGrid").dataSource.data(array);
                                    $("#grid").data("kendoGrid").dataSource.sync();
                                    sumarPruebasTotales();
                                    editado = true;
                                }
                                loadingStop();
                            });
                        }
                        else
                            displayNotify("PinturaNolote", "", '1');
                    }
                    else
                        displayNotify("PinturaNoPrueba", "", '1');
                }
                else
                    displayNotify("CapturaAvanceCuadranteNoColor", "", '1');
            }
            else
                displayNotify("PinturaReqSistemaPintura", "", '1');
        }
        else
            displayNotify("SistemaPinturaMensajeReqProcesoPintura", "", '1');
    }
    else
        displayNotify("CapturaAvanceCuadranteNoProyecto", "", '1');



}

function AjaxObtenerPruebasSpoolID(SpoolID, ProyectoProcesoPruebaID, SistemaPinturaColorID, unidadMedida, gridRow,loteID) {
    loadingStart();
    $PruebasPorLote.PruebasPorLote.read({ token: Cookies.get("token"), spoolID: SpoolID, proyectoProcesoPruebaID: ProyectoProcesoPruebaID, SistemaPinturaColorID: SistemaPinturaColorID, lenguaje: $("#language").val(), loteID: loteID }).done(function (data) {
        if (Error(data)) {
            gridRow.ListaDetallePruebas = data;
            $("#gridPopUp").data("kendoGrid").dataSource.data(data);
            $("#gridPopUp").data("kendoGrid").dataSource.sync();
            verVentanaPruebasPorSpool($("#inputPrueba").data("kendoComboBox").dataItem($("#inputPrueba").data("kendoComboBox").select()).Prueba, unidadMedida);
        }
        loadingStop();
    });

}

function AjaxEjecutarGuardado(data, guardarYNuevo) {
    $PruebasPorLote.PruebasPorLote.create(data, { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            displayNotify("MensajeGuardadoExistoso", "", '0');
            if (guardarYNuevo == 1) {
                opcionHabilitarView(false, "FieldSetView");
                Limpiar();
                // AjaxCargarCamposPredeterminados();
                editado = false;
            }
            else {
                $("#grid").data("kendoGrid").dataSource.data([]);
                ajaxBuscarPorLote();
                opcionHabilitarView(true, "FieldSetView");
            }
            loadingStop();
        }
        else {
            displayNotify("MensajeGuardadoErroneo", "", '2');
            loadingStop();

        }
    });
}