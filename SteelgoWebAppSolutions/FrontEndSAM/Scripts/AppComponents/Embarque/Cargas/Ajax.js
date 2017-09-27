var CuadrantePaqueteAnterior = 0;
var CuadranteSpoolAnterior = 0;
function AjaxCargarCamposPredeterminados() {
    loadingStart();
    var TipoMuestraPredeterminadoID = 3066;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {

        if (data == "Spool") {
            $('input:radio[name=EmbarqueCargaTipoSeleccion]:nth(0)').trigger("click");

        }
        else if (data == "Paquete") {
            $('input:radio[name=EmbarqueCargaTipoSeleccion]:nth(1)').trigger("click");
        }
        else if (data == "Codigo") {
            $('input:radio[name=EmbarqueCargaTipoSeleccion]:nth(2)').trigger("click");
        }
        loadingStop();
    });
};

function AjaxObtenerSpoolID() {

    var OrdenTrabajoOrigianl = $("#InputOrdenTrabajo").val();
    $Armado.Armado.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        dataSpoolArray = data;
        if (Error(data)) {
            if (data.OrdenTrabajo != "") {
                $("#InputOrdenTrabajo").val(data.OrdenTrabajo);
            }
            else {
                $("#InputOrdenTrabajo").val(OrdenTrabajoOrigianl);
                displayNotify("CapturaArmadoMensajeOrdenTrabajoNoEncontrada", "", '1');
            }

            $("#InputID").data("kendoComboBox").dataSource.data(data.idStatus);
            Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
            $("#InputID").data("kendoComboBox").enable(true);
            $("#InputID").data("kendoComboBox").input.focus();
        }
    });
}


function AjaxCargarProyecto() {
    loadingStart();
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
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
                }

                $("#inputProyecto").data("kendoComboBox").value(proyectoId);
                $("#inputProyecto").data("kendoComboBox").trigger("change");
            }
        }
        loadingStop();
    });
}

function AjaxCargarPaquetes(proyectoID) {
    $CargaPlana.CargaPlana.read({ token: Cookies.get("token"), ProyectoID: proyectoID }).done(function (data) {
        $("#inputPaquete").data("kendoComboBox").dataSource.data([]);
        var paqueteId = 0;

        if (data.length > 0) {
            $("#inputPaquete").data("kendoComboBox").dataSource.data(data);

            $("#inputPaquete").data("kendoComboBox").value(paqueteId);
            $("#inputPaquete").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}


function AjaxObtenerPlanas(ProveedorID, nuevaPlana) {
    loadingStart();
    var proyectoID = $("#inputProyecto").data("kendoComboBox").value();
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), ProyectoID: proyectoID, ProveedorID: ProveedorID }).done(function (data) {
        $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").dataSource.data([]);
        var PlanaID = 0;

        if (data.length > 0) {

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].PlanaID != 0) {
                        PlanaID = data[i].PlanaID;
                    }
                }
            }
            else {
                if (nuevaPlana != null) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].Nombre == nuevaPlana) {
                            PlanaID = data[i].PlanaID;
                        }
                    }
                }
            }

            data.splice(1, 0, {
                PlanaID: -1, Nombre: _dictionary.EmbarqueCargaNuevaPlana[$("#language").data("kendoDropDownList").value()]
            });
            $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").dataSource.data(data);
            $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value(PlanaID);
            $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}



function AjaxEmbarqueCargaProveedores(ProyectoID, nuevoProveedor) {
    loadingStart();

    $Proveedores.Proveedores.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, TipoProveedor: 1 }).done(function (data) {
        if (data.length > 0) {
            $("#inputProveedor").data("kendoComboBox").value("");
            $("#inputProveedor").data("kendoComboBox").trigger("change");
            var ProveedorId = 0;
            if (data.length > 0) {
                $("#inputProveedor").data("kendoComboBox").dataSource.data(data);
                if (data.length == 2) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ProveedorID != 0) {
                            ProveedorId = data[i].ProveedorID;

                        }
                    }
                }
                else {
                    if (nuevoProveedor != null) {
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Nombre == nuevoProveedor) {
                                ProveedorId = data[i].ProveedorID;
                            }
                        }
                    }
                }

                data.splice(1, 0, {
                    ProveedorID: -1, Nombre: _dictionary.EmbarqueCargaAgregarNuevoProveedor[$("#language").data("kendoDropDownList").value()]
                });
                $("#inputProveedor").data("kendoComboBox").dataSource.data(data);
                $("#inputProveedor").data("kendoComboBox").value(ProveedorId);
                $("#inputProveedor").data("kendoComboBox").trigger("change");
            }
            else {
                $("#inputProveedor").data("kendoComboBox").value("");
            }
        }
        loadingStop();
    });
}

function AjaxAgregarCarga() {
    var SpoolID = $("#InputID").data("kendoComboBox").value() != "" ? $("#InputID").data("kendoComboBox").value() : 0;
    var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
    if (!$("#inputCerrar").is(":checked")) {
        loadingStart();
        if ($("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").text() != '' && $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value() != undefined) {
            $CargaPlana.CargaPlana.read({ token: Cookies.get("token"), CargaPlanaID: 0, TipoConsulta: ObtenerTipoConsulta(), OrdenTrabajoSpoolID: SpoolID, NumeroControl: $("#inputCodigo").val() }).done(function (data) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = data;
                if (data.length > 0) {
                    for (var i = 0; i < array.length; i++) {
                        if (Proyecto.ProyectoID == array[i].ProyectoID) {
                            if (!ExisteSpool(array[i])) {
                                if (array[i].Cargado != 1) {
                                    if (array[i].Empaquetado == 0) {
                                        ds.insert(0, array[i]);
                                    }
                                    else {
                                        displayNotify('', _dictionary.EmbarqueCargaMsjErrorSpoolAgregarExistePaquete[$("#language").data("kendoDropDownList").value()]
                                            + array[i].Paquete, '1');
                                    }
                                }
                                else {
                                    if (array[i].PlanaCargado != "") {
                                        var res = array[i].PlanaCargado.split(":");
                                        if (res[0].trim() == 'MANUAL') {
                                            displayNotify('', _dictionary.EmbarqueCargaMsjErrorSpoolManualRevisionEmbarque[$("#language").data("kendoDropDownList").value()]
                                                 + res[1].trim(), '1');
                                        }
                                        else
                                        displayNotify('', _dictionary.EmbarqueCargaMsjErrorSpoolAgregar[$("#language").data("kendoDropDownList").value()]
                                             + array[i].PlanaCargado, '1');
                                    }
                                    else
                                        displayNotify('', _dictionary.EmbarqueCargaMsjErrorSpoolAgregarCarro[$("#language").data("kendoDropDownList").value()]
                                             + array[i].CarroCargado, '1');
                                }
                            } else {
                                displayNotify('EmbarqueCargaMsjErrorSpoolAgregarExiste', '', '1');
                            }
                        } else {
                            displayNotify('EmbarqueCargaMsjErrorSpoolAgregarProyectoIncorrecto', '', '1');
                        }
                    }
                } else {
                    displayNotify('NoExisteSpoolID', '', '1');
                }

                $("#InputID").data("kendoComboBox").value("");
                $("#inputCodigo").val("");
                ObtieneConsecutivo();
                ImprimirTotalToneladas(ds._data);
                ImprimirTotalPiezas(ds._data);
                ds.sync();
                loadingStop();
            });
        }


    }
    else {
        displayNotify('EmarqueCargaMensajePlanaCerrada', '', '1');
    }
}

function AjaxAgregarPaquete(Paquete) {
    if (!$("#inputCerrar").is(":checked")) {
        if ($("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").text() != '' && $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value() != undefined) {
            loadingStart();
            $CargaPlana.CargaPlana.read({ token: Cookies.get("token"), PaqueteID: Paquete.PaqueteID, lenguaje: $("#language").val() }).done(function (data) {
                if (data.length > 0) {
                    var ds = $("#grid").data("kendoGrid").dataSource;

                    var array = data;
                    if (array[0].Cargado != 1) {
                        for (var i = 0; i < array.length; i++) {
                            ds.insert(0, array[i]);
                        }
                    } else {
                        displayNotify('', _dictionary.EmbarqueCargaMsjErrorPaqueteCargado[$("#language").data("kendoDropDownList").value()]
                            + array[0].PlanaCargado, '2');
                    }
                }

                $("#inputPaquete").data("kendoComboBox").value("");
                ObtieneConsecutivo();
                ImprimirTotalToneladas(ds._data);
                ImprimirTotalPiezas(ds._data);
                ds.sync();
                loadingStop();
            });
        } else {
            displayNotify('EmarqueCargaMensajeEligePlana', '', '2');
        }
    }
    else {
        displayNotify('EmarqueCargaMensajePlanaCerrada', '', '2');
    }
}

function AjaxObtenerGrid() {
    if ($("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").text() != '' && $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value() != undefined) {
        loadingStart();
        $CargaPlana.CargaPlana.read({ token: Cookies.get("token"), PlanaID: $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value(), Todos: 1 }).done(function (data) {
            $("#grid").data("kendoGrid").dataSource.data([]);
            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = data;

            if (data.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    array[i].Consecutivo = $("#grid").data("kendoGrid").dataSource._data.length + 1;
                    ds.add(array[i]);
                }
            }

            ImprimirTotalToneladas(ds._data);
            ImprimirTotalPiezas(ds._data);
            ds.sync();
            loadingStop();
        });
    } else {
        displayNotify('EmarqueCargaMensajeEligePlana', '', '1');
    }
}

function AjaxCargarZona() {
    loadingStart();
    $Zona.Zona.read({ token: Cookies.get("token") }).done(function (data) {
        var ZonaId = 0;
        if (data.length > 0) {
            $("#inputZonaPopup").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ZonaID != 0) {
                        ZonaId = data[i].ZonaID;
                    }
                }
            }
            $("#inputZonaPopup").data("kendoComboBox").value(ZonaId);
            $("#inputZonaPopup").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}

function AjaxCargarZonaPaquete(proyectoID) {
    loadingStart();
    $Zona.Zona.read({ token: Cookies.get("token"), ProyectoID: proyectoID }).done(function (data) {

        var ZonaId = 0;
        if (data.length > 0) {
            $("#inputZonaPaquete").data("kendoComboBox").dataSource.data(data);
            $("#inputZonaPaqueteDescarga").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ZonaID != 0) {
                        ZonaId = data[i].ZonaID;
                    }
                }
            }
            $("#inputZonaPaquete").data("kendoComboBox").value(ZonaId);
            $("#inputZonaPaquete").data("kendoComboBox").trigger("change");
            $("#inputZonaPaqueteDescarga").data("kendoComboBox").value(ZonaId);
            $("#inputZonaPaqueteDescarga").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}


function AjaxCargarCuadrante(zonaID, DescargaSpool) {
    loadingStart();
    $Cuadrante.Cuadrante.read({ token: Cookies.get("token"), ZonaID: zonaID }).done(function (data) {
        var CuadranteId = 0;
        if (data.length > 0) {
            if (DescargaSpool == 1) {
                $("#inputCuadrantePopup").data("kendoComboBox").dataSource.data(data);
                if (data.length < 3 && CuadranteSpoolAnterior == 0) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].CuadranteID != 0) {
                            CuadranteId = data[i].CuadranteID;
                        }
                    }
                } else
                    CuadranteId = CuadranteSpoolAnterior;


                $("#inputCuadrantePopup").data("kendoComboBox").value(CuadranteId);
                $("#inputCuadrantePopup").data("kendoComboBox").trigger("change");
            } else if (DescargaSpool == 0) {
                $("#inputCuadrantePaquete").data("kendoComboBox").dataSource.data(data);
                if (data.length < 3 && CuadrantePaqueteAnterior == 0) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].CuadranteID != 0) {
                            CuadranteId = data[i].CuadranteID;
                        }
                    }
                } else
                    CuadranteId = CuadrantePaqueteAnterior;

                $("#inputCuadrantePaquete").data("kendoComboBox").value(CuadranteId);
                $("#inputCuadrantePaquete").data("kendoComboBox").trigger("change");
            } else {
                $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").dataSource.data(data);
                if (data.length < 3 && CuadrantePaqueteAnterior == 0) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].CuadranteID != 0) {
                            CuadranteId = data[i].CuadranteID;
                        }
                    }
                } else
                    CuadranteId = CuadrantePaqueteAnterior;

                $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").value(CuadranteId);
                $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").trigger("change");
            }
        }
    });
    loadingStop();
}


function AjaxDescargarSpool(dataItem, Cuadrante) {
    loadingStart();
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var elemento = 0;
    $CargaPlana.CargaPlana.read({
        token: Cookies.get("token"), DetalleCargaID: dataItem.DetalleCargaID, PaqueteID: dataItem.PaqueteID, SpoolID: dataItem.SpoolID,
        CuadranteID: Cuadrante.CuadranteID, CuadranteSam2ID: Cuadrante.CuadranteSam2ID, CuadranteAnterior: dataItem.CuadranteID
    }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
            elemento = parseInt(data.ReturnMessage[1]);
            dataSource.remove(dataItem);
            dataSource.sync();
            if (elemento == 0) {
                $("#detallePaquete").val(dataItem.PaqueteID);
                $("#CuadrantePaquete").val(dataItem.CuadranteID);
                CuadrantePaqueteAnterior = dataItem.CuadrantePaqueteAnteriorID;
                $("#inputZonaPaqueteDescarga").data("kendoComboBox").value(dataItem.ZonaPaqueteAnteriorID);
                $("#inputZonaPaqueteDescarga").data("kendoComboBox").trigger("change");

                windowPackageEmpty.title(_dictionary.EmbarqueEmpaquetadoAdvertenciaPaqueteVacio[$("#language").data("kendoDropDownList").value()]);
                windowPackageEmpty.open().center();
            }
            displayNotify("EmbarqueCargaMsjDescargaSpoolExito", "", "0");
        } else {
            displayNotify("EmbarqueCargaMsjDescargaSpoolError", "", "2");
        }

        ObtieneConsecutivo();
        ImprimirTotalToneladas(dataSource._data);
        ImprimirTotalPiezas(dataSource._data);
        loadingStop();
    });
}

function GuardarNuevoProveedor() {
    $Proveedores.Proveedores.read({ token: Cookies.get("token"), NombreProveedor: $("#inputNombreNuevoProveedor").val(), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), Descripcion: "", Direccion: "", Telefono: "", TipoProveedor: 1 }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            AjaxEmbarqueCargaProveedores($("#inputProyecto").data("kendoComboBox").value(), $("#inputNombreNuevoProveedor").val());
            windowNewProvider.close();
            displayNotify("MensajeGuardadoExistoso", "", "0");
        }
        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
            displayNotify("EmbarquePreparacionErrorExisteProveedor", "", '2');
        }
    });
}

function GuardarNuevaPlana() {
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), NombrePlana: $("#inputNombreNuevaPlana").val(), ProveedorID: $("#inputProveedor").data("kendoComboBox").value() }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            AjaxObtenerPlanas($("#inputProveedor").data("kendoComboBox").value(), $("#inputNombreNuevaPlana").val());
            windowNewPlate.close();
            displayNotify("MensajeGuardadoExistoso", "", "0");
        }
        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {

            displayNotify("EmbarqueCargaErrorExistePlana", "", '2');
        }
    });
}




function ajaxGuardar(arregloCaptura, tipoGuardar) {
    try {
        $("#grid").data("kendoGrid").dataSource.sync();
        var cerrar = 0;
        var CuadrantePlanaSam2 = $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").dataItem($("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").select()).CuadrantePlanaSam2;
        var CuadrantePlanaSam3 = $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").dataItem($("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").select()).CuadrantePlanaSam3;
        cerrar = $("#inputCerrar").is(":checked") ? 1 : 0;
        Captura = [];
        Captura[0] = { listaDetalle: "" };
        ListaDetalles = [];
        var i = 0;

        for (index = 0; index < arregloCaptura.length; index++) {
            ListaDetalles[i] = { Accion: "", DetalleCargaID: "", SpoolID: "", OrdenTrabajoID: "", CuadranteActualID: "", PaqueteID: "", EmpaquetadoInicial: "" };

            ListaDetalles[i].Accion = arregloCaptura[index].Accion;
            ListaDetalles[i].DetalleCargaID = arregloCaptura[index].DetalleCargaID;
            ListaDetalles[i].SpoolID = arregloCaptura[index].SpoolID;
            ListaDetalles[i].OrdenTrabajoID = arregloCaptura[index].OrdenTrabajoID;
            ListaDetalles[i].CuadranteActualID = arregloCaptura[index].CuadranteID;
            ListaDetalles[i].PaqueteID = arregloCaptura[index].PaqueteID;
            ListaDetalles[i].EmpaquetadoInicial = arregloCaptura[index].EmpaquetadoInicial;
            i++;

        }

        Captura[0].listaDetalle = ListaDetalles;

        if (Captura[0].listaDetalle.length > 0) {
            loadingStart();

            //validacion de carga plana
            $CargaPlana.CargaPlana.create(Captura[0], { token: Cookies.get("token"), PlanaID: $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value() }).done(function (dataValidacion) {

                var esCorrectaValidacion = true;
                for (var j = 0; j < arregloCaptura.length; j++) {
                    for (var i = 0; i < dataValidacion.length; i++) {


                        if (arregloCaptura[j].SpoolID == dataValidacion[i].SpoolID && dataValidacion[i].EsCorrecto == 0) {
                            $("#grid").data("kendoGrid").dataSource._data[j].RowOk = false;

                            $("#grid").data("kendoGrid").dataSource.sync();
                            esCorrectaValidacion = false;
                        }
                    }
                }


                //guardado de carga plana
                if (esCorrectaValidacion) {
                    $CargaPlana.CargaPlana.create(Captura[0], { token: Cookies.get("token"), PlanaID: $("#inputEmbarqueCargaPLacaPlana").data("kendoComboBox").value(), CerrarPlana: cerrar, CuadrantePlanaSam2: CuadrantePlanaSam2, CuadrantePlanaSam3: CuadrantePlanaSam3 }).done(function (data) {
                        editado = true;
                        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                            if (tipoGuardar == 1) {
                                Limpiar();
                            }
                            else {
                                $("#grid").data("kendoGrid").dataSource.data([]);
                                AjaxCargarPaquetes($("#inputProyecto").data("kendoComboBox").value());
                                AjaxObtenerGrid();
                                opcionHabilitarView(true, "FieldSetView");


                            }
                            displayNotify("MensajeGuardadoExistoso", "", "0");
                            editado = false;
                        }
                        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                            mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2"
                            displayNotify("MensajeGuardadoErroneo", "", '2');
                            loadingStop();
                        }
                    });
                }
                else {
                    displayNotify("EmbarqueCargaValidacionCargaPlana", "", "1");
                    loadingStop();
                }

            });



        }
        else {
            loadingStop();
        }


    } catch (e) {
        loadingStop();
        displayNotify("", e.message, '1');

    }

};

function AjaxDescargarPaquete(dataItem, eliminaFilas) {
    loadingStart();
    var ds = $("#grid").data("kendoGrid").dataSource;
    var cuadranteID = $("#inputCuadrantePaquete").data("kendoComboBox").value();
    if (cuadranteID == "" || cuadranteID == undefined) {
        cuadranteID = $("#inputCuadrantePaqueteDescarga").data("kendoComboBox").value();
    }

    $CargaPlana.CargaPlana.read({
        token: Cookies.get("token"), PaqueteID: dataItem, CuadranteID: cuadranteID, AbrirPaquete: eliminaFilas
    }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
            if (eliminaFilas == 1) {
                for (var x = 0; x < ds._data.length; x++) {
                    if (ds._data[x].PaqueteID != undefined && ds._data[x].PaqueteID == dataItem) {
                        ds.remove(ds._data[x]);
                        x--;
                    }
                }
            }

            ds.sync();
            displayNotify("EmbarqueCargaMsjDescargaPaqueteExito", "", "0");
        } else {
            displayNotify("EmbarqueCargaMsjDescargaPaqueteError", "", "2");
        }

        AjaxCargarPaquetes($("#inputProyecto").data("kendoComboBox").value());
        ObtieneConsecutivo();
        ImprimirTotalToneladas(ds._data);
        ImprimirTotalPiezas(ds._data);
        loadingStop();
    });
}

function AjaxEliminarPaquete(paqueteID, cuadrantePaqueteID) {
    loadingStart();
    $Empaquetado.Empaquetado.read({
        token: Cookies.get("token"), PaqueteID: paqueteID, CuadrantePaqueteSam2ID: 0,
        CuadrantePaqueteSam3ID: cuadrantePaqueteID
    }).done(function (data) {
        if (data.ReturnCode === 200) {
            displayNotify("EmbarqueEmpaquetadoMsjExitoEliminarPaquete", "", "0");
        } else if (data.ReturnCode === 201) {
            displayNotify("EmbarqueEmpaquetadoMsjErrorExistenSpoolsPaquete", "", "2");

        } else {
            displayNotify("EmbarqueEmpaquetadoMsjErrorEliminarPaquete", "", "2");
        }

        loadingStop();
    });
}