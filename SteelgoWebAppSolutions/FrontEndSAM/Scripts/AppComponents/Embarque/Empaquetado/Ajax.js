var cuadranteSave = 0;
var guardado = false;
var CuadranteAnterior = 0;
var cuadranteAnteriorDescarga = 0;
function AjaxCargarCamposPredeterminados() {
    var campoPredeterminado = 3067;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: campoPredeterminado }).done(function (data) {
        if (data == "Spool") {
            $('input:radio[name=TipoCarga]:nth(0)').trigger("click");
        }
        else if (data == "Codigo") {
            $('input:radio[name=TipoCarga]:nth(1)').trigger("click");
        }
        loadingStop();
    });
}

function AjaxCargarProyectos() {
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        $("#InputProyecto").data("kendoComboBox").dataSource.data([]);
        var proyectoId = 0;

        if (data.length > 0) {
            $("#InputProyecto").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProyectoID != 0) {
                        proyectoId = data[i].ProyectoID;
                    }
                }
            }
            $("#InputProyecto").data("kendoComboBox").value(proyectoId);
            $("#InputProyecto").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxObtenerSpoolID() {
    try {
        loadingStart();
        $CapturasRapidas.CapturasRapidas.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                $("#InputOrdenTrabajo").val(data.OrdenTrabajo);
                $("#InputID").data("kendoComboBox").dataSource.data(data.idStatus)
                Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
                loadingStop();
            }
        });
    } catch (e) {
        displayNotify("Mensajes_error", e.message, '2');
    }
}

function AjaxCargarPaquetes(proyectoID, paqueteID) {
    loadingStart();
    $Empaquetado.Empaquetado.read({ token: Cookies.get("token"), ProyectoID: proyectoID, lenguaje: $("#language").val() }).done(function (data) {
        $("#InputPaquete").data("kendoComboBox").dataSource.data([]);
        if (data.length > 0) {
            $("#InputPaquete").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3 && paqueteID == 0) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].PaqueteID != 0) {
                        paqueteID = data[i].PaqueteID;
                    }
                }
            }

            $("#InputPaquete").data("kendoComboBox").value(paqueteID);
            $("#InputPaquete").data("kendoComboBox").trigger("change");
        }

        if (guardado)
            AjaxCargarDetalleEmpaquetado(paqueteID, 1);
        loadingStop();
    });
}

function AjaxCargarZona(patioID) {
    loadingStart();
    $Zona.Zona.read({ token: Cookies.get("token"), PatioID: patioID }).done(function (data) {
        $("#InputZonaDescarga").data("kendoComboBox").dataSource.data([]);
        $("#inputZonaPaqueteDescarga").data("kendoComboBox").dataSource.data([]);

        if (data.length > 0) {
            var zonaID = 0;
            $("#InputZonaDescarga").data("kendoComboBox").dataSource.data(data);
            $("#inputZonaPaqueteDescarga").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ZonaID != 0) {
                        zonaID = data[i].ZonaID;
                    }
                }
            }

            $("#InputZonaDescarga").data("kendoComboBox").value(zonaID);
            $("#InputZonaDescarga").data("kendoComboBox").trigger("change");
            $("#inputZonaPaqueteDescarga").data("kendoComboBox").value(zonaID);
            $("#inputZonaPaqueteDescarga").data("kendoComboBox").trigger("change");
        }

        loadingStop();
    });
}

function AjaxCargarZonaGuardado(patioID, zonaID, cerrado) {
    loadingStart();
    $Empaquetado.Empaquetado.read({ token: Cookies.get("token"), PatioID: patioID, PaqueteCerrado: cerrado }).done(function (data) {
        $("#InputZonaPaquete").data("kendoComboBox").dataSource.data([]);
        if (data.length > 0) {
            $("#InputZonaPaquete").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3 && zonaID == 0) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ZonaID != 0) {
                        zonaID = data[i].ZonaID;
                    }
                }
            }

            $("#InputZonaPaquete").data("kendoComboBox").value(zonaID);
            $("#InputZonaPaquete").data("kendoComboBox").trigger("change");
        }

        loadingStop();
    });
}

function AjaxCargarCuadranteGuardado(zonaID) {
    var proyectoID = $("#InputProyecto").data("kendoComboBox").value();
    $Empaquetado.Empaquetado.read({ token: Cookies.get("token"), ProyectoID: proyectoID, ZonaID: zonaID }).done(function (data) {
        if (data.length > 0) {
            var cuadranteID = 0;
            $("#InputCuadrantePaquete").data("kendoComboBox").dataSource.data(data);
            $("#InputCuadrantePaquete").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3 && cuadranteSave == 0) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].CuadranteID != 0) {
                        cuadranteID = data[i].CuadranteID;
                    }
                }
            } else {
                cuadranteID = cuadranteSave;
            }
            $("#InputCuadrantePaquete").data("kendoComboBox").value(cuadranteID);
            $("#InputCuadrantePaquete").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxCargarCuadranteDescarga(zonaID) {
    $Cuadrante.Cuadrante.read({ token: Cookies.get("token"), ZonaID: zonaID }).done(function (data) {
        var CuadranteId = 0;
        if (data.length > 0) {
            $("#InputCuadranteDescarga").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3 && CuadranteAnterior == 0) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].CuadranteID != 0) {
                        CuadranteId = data[i].CuadranteID;
                    }
                }
            } else {
                CuadranteId = CuadranteAnterior;
            }

            $("#InputCuadranteDescarga").data("kendoComboBox").value(CuadranteId);
            $("#InputCuadranteDescarga").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxCargarDetalleEmpaquetado(paqueteID, todos) {
    loadingStart();
    $Empaquetado.Empaquetado.read({ token: Cookies.get("token"), PaqueteID: paqueteID, Todos: todos }).done(function (data) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        if (data != null) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            for (var i = 0; i < data.length; i++) {
                data[i].Consecutivo = $("#grid").data("kendoGrid").dataSource._data.length + 1;
                ds.add(data[i]);
            }
            ImprimirTotalToneladas(ds._data);
            ImprimirTotalPiezas(ds._data);
        }
        guardado = false;
        loadingStop();
    });
}

function AjaxObtenerDetalleSpool(tipoConsulta, spoolID, codigo) {
    loadingStart();
    var ProyectoID = parseInt($("#InputProyecto").data("kendoComboBox").value());
    $Empaquetado.Empaquetado.read({ token: Cookies.get("token"), TipoConsulta: tipoConsulta, OrdenTrabajoSpoolID: spoolID, Codigo: codigo }).done(function (data) {
        if (data != null) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            for (var i = 0; i < data.length; i++) {
                if (!validarInformacion(data[i])) {
                    if (data[i].ProyectoID === ProyectoID) {
                        if (data[i].CargaPlana === 0) {
                            if (data[i].Empaquetado === 0) {
                                ds.insert(0, data[i]);
                            } else {
                                displayNotify("", _dictionary.EmbarqueEmpaquetadoErrorSpoolPaquete[$("#language").data("kendoDropDownList").value()] +
                                    data[i].Paquete, '1');
                            }
                        } else {
                            if (data[i].Plana != "")
                                displayNotify("", _dictionary.EmbarqueEmpaquetadoErrorSpoolPlana[$("#language").data("kendoDropDownList").value()] +
                                        data[i].Plana, '1');
                            else
                                displayNotify('', _dictionary.EmbarqueCargaMsjErrorSpoolAgregarCarro[$("#language").data("kendoDropDownList").value()]
                                     + data[i].CarroCargado, '1');
                        }
                    } else {
                        displayNotify("EmbarqueEmpaquetadoErrorProyectoDiferente", "", '1');
                    }
                } else {
                    displayNotify("SpoolIDExistente", "", '1');
                }
            }
            if (data.length == 0 && tipoConsulta == 2) {
                displayNotify("CapturaSoldaduraNoExisteSpoolID", "", '2');
            }


        }

        $("#InputID").data("kendoComboBox").value("");
        $("#inputCodigo").val("");
        ObtieneConsecutivo();
        ImprimirTotalToneladas(ds._data);
        ImprimirTotalPiezas(ds._data);
        loadingStop();
    });
}

function AbrirPopUpGuardar(Paquete, TipoGuardado, PatioID) {
    var fechaPredeterminada;
    cuadranteSave = Paquete == undefined ? 0 : Paquete.CuadranteUbicacion;
    var cerrado = $("#InputCerrar").is(":checked") ? 1 : 0;
    var zonaID = Paquete.ZonaID == 0 && Paquete.CuadranteUbicacion != 0 ? 1 : Paquete.ZonaID;
    AjaxCargarZonaGuardado(PatioID, zonaID, cerrado);

    $("#InputTipoGuardado").val(TipoGuardado);
    if (Paquete.PaqueteID == 0) {
        $("#InputNombre").val("");
        $("#InputNombre").attr("disabled", false);
        var idFechaPaquete = 3068;
        $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: idFechaPaquete }).done(function (data) {
            fechaPredeterminada = kendo.toString(data, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
            $("#InputFechaPaquete").val(fechaPredeterminada);
        });
    } else {
        $("#InputNombre").val(Paquete.Nombre);
        $("#InputNombre").attr("disabled", true);
        $("#InputCuadrantePaquete").data("kendoComboBox").value("");
        $("#InputZonaPaquete").data("kendoComboBox").value(Paquete.ZonaID == 0 && Paquete.CuadranteUbicacion != 0 ? 1 : Paquete.ZonaID);
        $("#InputZonaPaquete").data("kendoComboBox").trigger("change");
        fechaPredeterminada = kendo.toString(Paquete.FechaCreacion, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
        $("#InputFechaPaquete").val(fechaPredeterminada);

    }
    windowSave.title(_dictionary.EmbarqueEmpaquetadoNuevoPaquete[$("#language").data("kendoDropDownList").value()]);
    windowSave.open().center();

}

function AjaxGuardarCaptura(ds, tipoGuardado, cerrarPaquete, Paquete, Proyecto) {
    windowSave.close();
    var Captura = {
        listaDetalle: ""
    };
    var Detalle = [];
    var cont = 0;
    if (ds.length > 0) {
        for (var i = 0; i < ds.length; i++) {
            Detalle[i] = {
                Accion: "", SpoolID: "", CuadranteActualSam2ID: "", CuadranteActualSam3ID: ""
            }
            Detalle[i].Accion = ds[i].Accion;
            Detalle[i].SpoolID = ds[i].SpoolID;
            Detalle[i].CuadranteActualSam2ID = ds[i].CuadranteSam2ID;
            Detalle[i].CuadranteActualSam3ID = ds[i].CuadranteSam3ID;
        }
    } else {
        Detalle[0] = {
            Accion: 0, SpoolID: 0, CuadranteActualSam2ID: 0, CuadranteActualSam3ID: 0
        }
    }
    Captura.listaDetalle = Detalle;


    //validacion de carga plana
    $Empaquetado.Empaquetado.create(Captura, { token: Cookies.get("token"), PaqueteID: Paquete.PaqueteID }).done(function (dataValidacion) {
        loadingStop();
        var esCorrectaValidacion = true;
        for (var j = 0; j < ds.length; j++) {
            for (var i = 0; i < dataValidacion.length; i++) {


                if (ds[j].SpoolID == dataValidacion[i].SpoolID && dataValidacion[i].EsCorrecto == 0) {
                    $("#grid").data("kendoGrid").dataSource._data[j].RowOk = false;

                    $("#grid").data("kendoGrid").dataSource.sync();
                    esCorrectaValidacion = false;
                }
            }
        }


        //guardado de carga plana
        if (esCorrectaValidacion) {


            $Empaquetado.Empaquetado.create(Captura, {
                token: Cookies.get("token"), lenguaje: $("#language").val(),
                PaqueteID: Paquete.PaqueteID, NombrePaquete: $("#InputNombre").val(), CuadranteID: $("#InputCuadrantePaquete").data("kendoComboBox").value(),
                Cerrado: cerrarPaquete, FechaPaquete: $("#InputFechaPaquete").val(), CuadrantePaqueteSam2ID: Paquete.CuadrantePaqueteSam2ID,
                CuadrantePaqueteSam3ID: Paquete.CuadrantePaqueteSam3ID
            }).done(function (data) {
                if (data.ReturnMessage.length == 2 && data.ReturnMessage[0] == "Ok") {
                    if (tipoGuardado != "1") {
                        Limpiar();
                    } else {
                        var paqueteID = parseInt(data.ReturnMessage[1]);
                        opcionHabilitarView(true, "FieldSetView");
                        guardado = true;
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        AjaxCargarPaquetes(Proyecto.ProyectoID, paqueteID);

                    }
                    displayNotify("MensajeGuardadoExistoso", "", '0');
                } else if (data.ReturnMessage.length == 1 && data.ReturnMessage[0] == "Paquete Existe") {
                    displayNotify("EmbarqueEmpaquetadoErrorPaqueteExiste", "", '2');
                } else {
                    displayNotify("MensajeGuardadoErroneo", "", '2');
                }
            });
        }
        else
            displayNotify("EmbarqueEmpaquetadoValidacionEmpaquetado", "", "1");

    });

}

function AjaxDescargarSpool(dataItem, Paquete) {
    loadingStart();
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var cuadranteID = $("#InputCuadranteDescarga").data("kendoComboBox").value();
    var elementos = 0;
    windowDownload.close();
    $Empaquetado.Empaquetado.read({
        token: Cookies.get("token"), EmpaquetadoID: dataItem.EmpaquetadoID, SpoolID: dataItem.SpoolID,
        CuadranteID: cuadranteID, CuadranteAnteriorID: dataItem.CuadranteSam3ID,
        PaqueteID: Paquete.PaqueteID
    }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                dataSource.remove(dataItem);
                dataSource.sync();
                elementos = parseInt(data.ReturnMessage[1]);
                if (elementos == 0 && Paquete.Cerrado) {
                    if (Paquete.CargaPlana == 1) {

                        cuadranteAnteriorDescarga = Paquete.CuadranteUbicacionAnt

                        $("#inputZonaPaqueteDescarga").data("kendoComboBox").value(Paquete.ZonaUbicacionAnt);
                        $("#inputZonaPaqueteDescarga").data("kendoComboBox").trigger("change");
                        windowPackageEmpty.title(_dictionary.EmbarqueEmpaquetadoAdvertenciaPaqueteVacio[$("#language").data("kendoDropDownList").value()]);
                        windowPackageEmpty.open().center();
                    } else
                        AjaxAbrirPaquete(Paquete);
                } else {
                    displayNotify("EmbarqueEmpaquetadoMsjDescargaSpoolExito", "", "0");
                }
            } else {
                displayNotify("EmbarqueEmpaquetadoMsjDescargaSpoolError", "", "2");
            }

            ObtieneConsecutivo();
            ImprimirTotalToneladas(dataSource._data);
            ImprimirTotalPiezas(dataSource._data);
        } else {
            displayNotify("EmbarqueEmpaquetadoMsjDescargaSpoolError", "", "2");
        }
        $("#InputZonaDescarga").data("kendoComboBox").value("");
        $("#InputCuadranteDescarga").data("kendoComboBox").value("");
        $("#InputCuadranteDescarga").data("kendoComboBox").dataSource.data([]);
    });
    loadingStop();
}

function AjaxEliminarPaquete(Paquete) {
    loadingStart();
    $Empaquetado.Empaquetado.read({
        token: Cookies.get("token"), PaqueteID: Paquete.PaqueteID, CuadrantePaqueteSam2ID: Paquete.CuadrantePaqueteSam2ID,
        CuadrantePaqueteSam3ID: Paquete.CuadrantePaqueteSam3ID
    }).done(function (data) {
        if (data.ReturnCode === 200) {
            AjaxCargarPaquetes($("#InputProyecto").data("kendoComboBox").value(), 0);
            $("#TotalPiezas").text("");
            $("#TotalToneladas").text("");
            displayNotify("EmbarqueEmpaquetadoMsjExitoEliminarPaquete", "", "0");
        } else if (data.ReturnCode === 201) {
            displayNotify("EmbarqueEmpaquetadoMsjErrorExistenSpoolsPaquete", "", "2");

        } else {
            displayNotify("EmbarqueEmpaquetadoMsjErrorEliminarPaquete", "", "2");
        }

        loadingStop();
    });
}

function AjaxCargarCuadrantePaquete(zonaID) {
    $Cuadrante.Cuadrante.read({ token: Cookies.get("token"), ZonaID: zonaID }).done(function (data) {
        if (data.length > 0) {
            var cuadranteID = 0;
            $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").dataSource.data(data);
            $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3 && cuadranteAnteriorDescarga == 0) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].CuadranteID != 0) {
                        cuadranteID = data[i].CuadranteID;
                    }
                }
            } else {
                cuadranteID = cuadranteAnteriorDescarga;
            }
            $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").value(cuadranteID);
            $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxDescargarPaquete(Paquete) {
    loadingStart();
    var cuadranteID = $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").value();
    var proyectoID = $("#InputProyecto").data("kendoComboBox").value();
    $CargaPlana.CargaPlana.read({
        token: Cookies.get("token"), PaqueteID: Paquete.PaqueteID, CuadranteID: cuadranteID
    }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
            AjaxCargarPaquetes(proyectoID, Paquete.PaqueteID);
            displayNotify("EmbarqueCargaMsjDescargaPaqueteExito", "", "0");
        } else {
            displayNotify("EmbarqueCargaMsjDescargaPaqueteError", "", "2");
        }
        loadingStop();
    });
}

function AjaxAbrirPaquete(Paquete) {
    loadingStart();
    var proyectoID = $("#InputProyecto").data("kendoComboBox").value();
    $Empaquetado.Empaquetado.read({
        token: Cookies.get("token"), PaqueteID: Paquete.PaqueteID
    }).done(function (data) {
        $("#InputCerrar")[0].checked = false;
        //AjaxCargarPaquetes(proyectoID, Paquete.PaqueteID);
    });

    loadingStop();
}