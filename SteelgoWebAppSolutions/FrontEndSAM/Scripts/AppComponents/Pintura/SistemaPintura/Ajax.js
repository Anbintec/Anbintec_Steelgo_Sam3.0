function AjaxObtenerProyectos() {
    loadingStart();
    
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputProyecto").data("kendoMultiSelect").dataSource.data([]);
            $("#inputProyecto").data("kendoMultiSelect").value("");
            data.shift();
            $("#inputProyecto").data("kendoMultiSelect").dataSource.data(data);
            AjaxCargarNuevoSistemaPintura();
        }
    });
}

function AjaxObtenerColor() {
    loadingStart();
    
    $PinturaGeneral.PinturaGeneral.read({ token: Cookies.get("token"), Lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            $("#inputColor").data("kendoMultiSelect").dataSource.data([]);
            $("#inputColor").data("kendoMultiSelect").value("");
            data.shift();
            $("#inputColor").data("kendoMultiSelect").dataSource.data(data);

            if ($("#inputSistemaPinturaID").val() != "") {//muestro combobox
                $("#divComboProyecto").css("display", "block");
                $("#divMultiselectProyecto").css("display", "none");
                $('.eliminarSistema').css("display", "block");
                $("#inputNombre").attr('disabled', true);
                AjaxCargarEdicionSistemaPintura();

            }
            else {
                AjaxObtenerProyectos();
                $('.eliminarSistema').css("display", "none");
            }
        }
    });
}

function AjaxCargarNuevoSistemaPintura() {
    var SistemaPintura = "", ProyectoID = 0;

    if ($("#inputSistemaPinturaID").val() != "") {
        SistemaPintura = $("#inputSistemaPinturaID").val();
    }

    if ($("#comboProyecto").data("kendoComboBox").value() != "")
        ProyectoID = $("#comboProyecto").data("kendoComboBox").value() == "" ? 0 : parseInt($("#comboProyecto").data("kendoComboBox").value());
    //actividad:1 es una variable que sirve para ibtener un nuevo SP o editar un SP.

    tieneAvance = false; // es false porque al ser nuevo el SP entonces no tiene capturado el avance
    loadingStart();
    $SistemaPintura.SistemaPintura.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), SistemaPintura: SistemaPintura, ProyectoID: ProyectoID, actividad: 1 }).done(function (data) {
        if (Error(data)) {
            $("#grid").data("kendoGrid").dataSource.data([]);
            var ds = $("#grid").data("kendoGrid").dataSource;
            var array = data;

            for (var i = 0; i < array.length; i++) {
                ds.add(array[i]);
            }
        }
        loadingStop();

    });

}

function AjaxCargarEdicionSistemaPintura() {
    var SistemaPintura = "", ProyectoID = 0;;
    SistemaPintura = $("#inputSistemaPinturaID").val();
    if ($("#comboProyecto").data("kendoComboBox").value() == "" && paramReqProyectoID != 0) {
        ProyectoID = paramReqProyectoID;
    }
    else if ($("#inputProyecto").data("kendoMultiSelect")._values.length == 1)
        ProyectoID = $("#inputProyecto").data("kendoMultiSelect")._values[0];
    else if ($("#comboProyecto").data("kendoComboBox").value() != "") {
        ProyectoID = parseInt($("#comboProyecto").data("kendoComboBox").value());
    }

    loadingStart();

    //$("#comboProyecto").data("kendoComboBox").dataItem($("#comboProyecto").data("kendoComboBox").select())
    $SistemaPintura.SistemaPintura.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), SistemaPintura: SistemaPintura, ProyectoID: ProyectoID, actividad: 2 }).done(function (data) {
        if (Error(data)) {
            if (data.length > 0) {
                $("#inputColor").data("kendoMultiSelect").value("")
                $("#inputNombre").val(data[0].Nombre);
                $("#inputSistemaPinturaID").val(data[0].Nombre.split('~')[0]);
                $("#inputhiddenSistemaPinturaID").val(data[0].SistemaPinturaID);

                if (data[0].NoPintable) {
                    $("#inputNoAplicable").prop("checked", true);
                    $("#inputColor").data("kendoMultiSelect").enable(false);
                }
                if (data[0].listadoColor.length > 0) {
                    var valores = [];
                    for (var i = 0; i < data[0].listadoColor.length; i++) {
                        valores[i] = data[0].listadoColor[i].ColorID;
                    };

                    $("#inputColor").data("kendoMultiSelect").value(valores);
                }

                if ($("#comboProyecto").data("kendoComboBox").value() != "" && $("#comboProyecto").data("kendoComboBox").value() != "0") {

                    AjaxCargarNuevoSistemaPintura();
                }
                else {
                    $("#comboProyecto").data("kendoComboBox").dataSource.data(data[0].listadoProyectos);

                    if ($("#comboProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                        $("#comboProyecto").data("kendoComboBox").select(1);
                        $("#comboProyecto").data("kendoComboBox").trigger("change");
                        AjaxCargarNuevoSistemaPintura();
                    }
                    else {
                        if (paramReqProyectoID == 0)
                            $("#comboProyecto").data("kendoComboBox").select(0);
                        else {
                            $("#comboProyecto").data("kendoComboBox").value(paramReqProyectoID)
                            $("#comboProyecto").data("kendoComboBox").trigger("change");
                        }
                    }

                }
            }
            if (ProyectoID != 0) {
                AjaxObtenerEstatusTieneAvance(SistemaPintura, ProyectoID);
            }

        }
    });
}


function AjaxObtenerEstatusTieneAvance(SistemaPintura, ProyectoID) {
    $SistemaPintura.SistemaPintura.read({ token: Cookies.get("token"), SistemaPintura: SistemaPintura, ProyectoID: ProyectoID }).done(function (result) {
        if (Error(result)) {
            tieneAvance = result; // es false porque al ser nuevo el SP entonces no tiene capturado el avance
            if (result) {
                deshabilitar();

                //$("#divComboProyecto").css("display", "none");
                //$("#divMultiselectProyecto").css("display", "block");
                //$("#inputNoAplicable").prop("checked", false);
            }
        }
        loadingStop();
    });
}

function AjaxGuardarCaptura(arregloCaptura, tipoGuardar) {
    try {
        $("#grid").data("kendoGrid").dataSource.sync();
        var SistemaPintura = 0, Nombre = "", ProyectoID = 0, NoPintable = 0;

        Captura = [];
        ListasCaptura = [];
        Captura[0] = { Detalles: "", ListadoColor: "", ListadoProyectos: "" };
        ListaSPNuevo = [];
        ListaSPNuevo[0] = { Nombre: "", Accion: "" };

        ListasCaptura[0] = { ListaSPNuevo: "", ListaSPProyecto: "", ListaSPColor: "", ListaSPProyectoProceso: "" };
        ListaDetalles = [];
        ListaColor = [];
        ListaProyectos = [];

        var ds = $("#grid").data("kendoGrid").dataSource;
        //if (ds._data.length > 0) {
        //    for (var i = 0; i < ds._data.length; i++) {
        //        if (ds._data[i].AsignadoSpool) {
        //            asignadoSpool = true;
        //            break;
        //        }
        //    } 
        //}

        NoPintable = ($("#inputNoAplicable").is(':checked')) ? 1 : 0;
        var necesitaColor = false;
        var ds = $("#grid").data("kendoGrid").dataSource;
        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Agregar && (ds._data[i].Proceso == "Acabado")) {
                necesitaColor = true;
            }
        }


        SistemaPintura = $("#inputSistemaPinturaID").val() == "" ? 0 : $("#inputSistemaPinturaID").val();
        Nombre = $("#inputNombre").val();
        if (Nombre == "") {
            displayNotify("SistemaPinturaMensajeErrorNombre", "", '1');
            return;
        }

        if ($("#inputColor").data("kendoMultiSelect")._values.length == 0 && necesitaColor) {
            displayNotify("SistemaPinturaMensajeErrorColor", "", '1');
            return;
        }
        else {
            if ($('#divMultiselectProyecto').is(":visible")) {
                var row = 0;
                for (var j = 0; j < $("#inputProyecto").data("kendoMultiSelect")._values.length; j++) {
                    for (var i = 0; i < $("#inputColor").data("kendoMultiSelect")._values.length; i++) {
                        ListaColor[row] = { Accion: "", ColorID: "", SistemaPintura: "", ProyectoID: "" };
                        ListaColor[row].ColorID = $("#inputColor").data("kendoMultiSelect")._values[i];
                        ListaColor[row].Accion = 1;
                        ListaColor[row].ProyectoID = $("#inputProyecto").data("kendoMultiSelect")._values[j];
                        ListaColor[row].SistemaPintura = SistemaPintura == 0 ? 0 : $("#inputhiddenSistemaPinturaID").val();
                        row++;
                    }

                }
            }
            else {
                var row = 0;

                for (var i = 0; i < $("#inputColor").data("kendoMultiSelect")._values.length; i++) {
                    ListaColor[row] = { Accion: "", ColorID: "", SistemaPintura: "", ProyectoID: "" };
                    ListaColor[row].ColorID = $("#inputColor").data("kendoMultiSelect")._values[i];
                    ListaColor[row].Accion = 1;
                    ListaColor[row].ProyectoID = $("#comboProyecto").val();
                    ListaColor[row].SistemaPintura = SistemaPintura == 0 ? 0 : $("#inputhiddenSistemaPinturaID").val();
                    row++;
                }


            }
        }

        if ($("#inputSistemaPinturaID").val() != "") {
            if ($("#comboProyecto").data("kendoComboBox").text() != "") {
                ProyectoID = $("#comboProyecto").data("kendoComboBox").value();

                ListaProyectos[0] = { ProyectoID: "", Accion: "", NoPintable: "" };
                ListaProyectos[0].ProyectoID = ProyectoID;
                ListaProyectos[0].Accion = tieneAvance ? 2 : 1;
                ListaProyectos[0].NoPintable = NoPintable
            }
            else {
                displayNotify("SistemaPinturaMensajeErrorProyecto", "", '1');
                return;
            }
        }
        else {
            if ($("#inputProyecto").data("kendoMultiSelect")._values.length == 0) {
                displayNotify("SistemaPinturaMensajeErrorListadoProyecto", "", '1');
                return;
            }
            else {
                for (var i = 0; i < $("#inputProyecto").data("kendoMultiSelect")._values.length; i++) {
                    ListaProyectos[i] = { ProyectoID: "", Accion: "", NoPintable: "" };
                    ListaProyectos[i].ProyectoID = $("#inputProyecto").data("kendoMultiSelect")._values[i];
                    ListaProyectos[i].Accion = tieneAvance ? 2 : 1;
                    ListaProyectos[i].NoPintable = NoPintable
                }
            }
        }

        ListaSPNuevo[0].Nombre = Nombre;
        //ListaSPNuevo[0].NoPintable = NoPintable;
        ListaSPNuevo[0].Accion = tieneAvance ? 2 : 1;

        var i = 0;
        for (var k = 0; k < ListaProyectos.length; k++) {
            for (index = 0; index < arregloCaptura.length; index++) {
                arregloCaptura[index].RowOk = true;
                if (arregloCaptura[index].Agregar) {
                    ListaDetalles[i] = { Accion: "", ProcesoPinturaID: ""/*, MetrosLote: "", NumeroPruebas: ""*/, ProyectoID: "", ListadoPruebas: [], Estatus: 1, NumeroComponentes: "", ReductorID: "", ListaDetalleComponentesAgregados: [] };

                    ListaDetalles[i].Accion = tieneAvance ? 2 : 1;
                    ListaDetalles[i].ProcesoPinturaID = arregloCaptura[index].ProcesoPinturaID;
                    //ListaDetalles[i].MetrosLote = arregloCaptura[index].MetrosLote;
                    //ListaDetalles[i].NumeroPruebas = arregloCaptura[index].NumeroPruebas;
                    ListaDetalles[i].ProyectoID = ListaProyectos[k].ProyectoID;
                    ListaDetalles[i].NumeroComponentes = arregloCaptura[index].NumeroComponentes;
                    ListaDetalles[i].ReductorID = arregloCaptura[index].ReductorID;

                    if (arregloCaptura[index].ListaDetalleComponentesAgregados != null) {
                        for (var g = 0; g < arregloCaptura[index].ListaDetalleComponentesAgregados.length; g++) {
                            ListaDetalles[i].ListaDetalleComponentesAgregados[g] = { ProyectoID: "", ProcesoPinturaID: "", ComponenteAgregadoID: "", ComponenteID: "", Accion: "" };
                            ListaDetalles[i].ListaDetalleComponentesAgregados[g].ProyectoID = ListaProyectos[k].ProyectoID;
                            ListaDetalles[i].ListaDetalleComponentesAgregados[g].ProcesoPinturaID = arregloCaptura[index].ProcesoPinturaID;
                            ListaDetalles[i].ListaDetalleComponentesAgregados[g].ComponenteAgregadoID = arregloCaptura[index].ListaDetalleComponentesAgregados[g].ProyectoProcesoComponenteID;
                            ListaDetalles[i].ListaDetalleComponentesAgregados[g].ComponenteID = arregloCaptura[index].ListaDetalleComponentesAgregados[g].ComponenteID;
                            ListaDetalles[i].ListaDetalleComponentesAgregados[g].Accion = arregloCaptura[index].ListaDetalleComponentesAgregados[g].Accion;
                        }
                    } else {
                        ListaDetalles[i].Estatus = 0;
                        arregloCaptura[index].RowOk = false;
                    }


                    for (var j = 0; j < arregloCaptura[index].listadoPruebasDetalle.length; j++) {
                        ListaDetalles[i].ListadoPruebas[j] = { Accion: "", UnidadMedidaID: "", UnidadMinima: "", UnidadMaxima: "", ProyectoID: "", ProcesoPinturaID: "", PruebaProcesoPinturaID: "" };
                        ListaDetalles[i].ListadoPruebas[j].Accion = arregloCaptura[index].listadoPruebasDetalle[j].Accion;
                        ListaDetalles[i].ListadoPruebas[j].ProcesoPinturaID = ListaDetalles[i].ProcesoPinturaID;
                        ListaDetalles[i].ListadoPruebas[j].ProyectoID = ListaProyectos[k].ProyectoID;
                        ListaDetalles[i].ListadoPruebas[j].UnidadMedidaID = arregloCaptura[index].listadoPruebasDetalle[j].UnidadMedidaID;
                        ListaDetalles[i].ListadoPruebas[j].UnidadMinima = arregloCaptura[index].listadoPruebasDetalle[j].UnidadMinima;
                        ListaDetalles[i].ListadoPruebas[j].UnidadMaxima = arregloCaptura[index].listadoPruebasDetalle[j].UnidadMaxima;
                        ListaDetalles[i].ListadoPruebas[j].PruebaProcesoPinturaID = arregloCaptura[index].listadoPruebasDetalle[j].PruebaProcesoPinturaID;
                    }
                    if (arregloCaptura[index].Agregar && (/*arregloCaptura[index].MetrosLote == null || arregloCaptura[index].MetrosLote == 0 || arregloCaptura[index].NumeroPruebas == 0 || arregloCaptura[index].NumeroPruebas == null || arregloCaptura[index].listadoPruebasDetalle.length == 0 ||*/ arregloCaptura[index].NumeroComponentes == 0 || tieneComponentesSinCaptura(arregloCaptura[index].ListaDetalleComponentesAgregados))) {
                        ListaDetalles[i].Estatus = 0;
                        arregloCaptura[index].RowOk = false;
                    }
                    i++;
                }
            }
        }


        ListasCaptura[0].ListaSPNuevo = ListaSPNuevo;
        ListasCaptura[0].ListaSPProyecto = ListaProyectos;
        ListasCaptura[0].ListaSPColor = ListaColor;
        ListasCaptura[0].ListaSPProyectoProceso = ListaDetalles;
        Captura[0].Detalles = ListasCaptura;

        if (tieneAvance && NoPintable == 1)
            displayNotify("SistemaPinturaMensajeErrorNoPuedeSerPintable", "", '1');
        else if (NoPintable == 0 && ListaDetalles.length == 0)
            displayNotify("SistemaPinturaMensajeErrorRequiereProceso", "", '1');
        else
            if (!ExistRowErrors(arregloCaptura)) {
                loadingStart();
                $SistemaPintura.SistemaPintura.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), tieneAvance: tieneAvance ? true : false }).done(function (data) {
                    if (Error(data)) {
                        var SistemaPintura = "", ProyectoID = 0;;
                        SistemaPintura = $("#inputSistemaPinturaID").val();
                        if ($("#comboProyecto").data("kendoComboBox").value() == "" && paramReqProyectoID != 0) {
                            ProyectoID = paramReqProyectoID;
                        }
                        else if ($("#inputProyecto").data("kendoMultiSelect")._values.length == 1)
                            ProyectoID = $("#inputProyecto").data("kendoMultiSelect")._values[0];
                        else if ($("#comboProyecto").data("kendoComboBox").value() != "") {
                            ProyectoID = parseInt($("#comboProyecto").data("kendoComboBox").value());
                        }
                        $SistemaPintura.SistemaPintura.read({ token: Cookies.get("token"), SistemaPintura: SistemaPintura, ProyectoID: ProyectoID }).done(function (result) {

                            tieneAvance = result; // es false porque al ser nuevo el SP entonces no tiene capturado el avance

                            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                                if (tipoGuardar == 1) {
                                    AjaxObtenerColor();
                                    $("#btnCancelar").trigger("click");
                                    //Limpiar();
                                }
                                else {
                                    if (result) {
                                        deshabilitar();
                                    }
                                    else {
                                        $("#grid").data("kendoGrid").dataSource.data([]);
                                        opcionHabilitarView(true, "FieldSetView");
                                        $("#inputhiddenSistemaPinturaID").val(data.ReturnMessage[1]);
                                        $("#inputSistemaPinturaID").val(data.ReturnMessage[2]);
                                        $("#inputNombre").attr('disabled', true);
                                        AjaxObtenerColor();
                                    }
                                }
                                displayNotify("MensajeGuardadoExistoso", "", "0");
                            }
                            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                                mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2"
                                displayNotify("MensajeGuardadoErroneo", "", '2');
                            }

                            loadingStop();
                        });

                    }
                });
            }
            else {
                loadingStop();

                $("#grid").data("kendoGrid").dataSource.sync();
                ventanaConfirmGuardado = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false, //the window will not appear before its .open method is called
                    width: "auto",
                    height: "auto",
                    modal: true,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirmGuardado.content(_dictionary.EntregaPlacasGraficasMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
					"</br><center><button class='btn btn-blue' id='yesButtonConfirmarGuardado'>Si</button><button class='btn btn-blue' id='noButtonConfirmarGuardado'> No</button></center>");

                ventanaConfirmGuardado.open().center();



                $("#yesButtonConfirmarGuardado").click(function () {
                    loadingStart();

                    ArregloGuardado = [];
                    var indice = 0;
                    for (var i = 0; i < Captura[0].Detalles[0].ListaSPProyectoProceso.length; i++) {
                        if (Captura[0].Detalles[0].ListaSPProyectoProceso[i].Estatus == 1) {
                            ArregloGuardado[indice] = Captura[0].Detalles[0].ListaSPProyectoProceso[i];
                            indice++;
                        }
                    }

                    Captura[0].Detalles[0].ListaSPProyectoProceso = [];
                    Captura[0].Detalles[0].ListaSPProyectoProceso = ArregloGuardado;


                    if (ArregloGuardado.length > 0) {
                        loadingStart();
                        $SistemaPintura.SistemaPintura.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), tieneAvance: tieneAvance }).done(function (data) {
                            if (Error(data)) {
                                var SistemaPintura = "", ProyectoID = 0;;
                                SistemaPintura = $("#inputSistemaPinturaID").val();
                                if ($("#comboProyecto").data("kendoComboBox").value() == "" && paramReqProyectoID != 0) {
                                    ProyectoID = paramReqProyectoID;
                                }
                                else if ($("#inputProyecto").data("kendoMultiSelect")._values.length == 1)
                                    ProyectoID = $("#inputProyecto").data("kendoMultiSelect")._values[0];
                                else if ($("#comboProyecto").data("kendoComboBox").value() != "") {
                                    ProyectoID = parseInt($("#comboProyecto").data("kendoComboBox").value());
                                }
                                $SistemaPintura.SistemaPintura.read({ token: Cookies.get("token"), SistemaPintura: SistemaPintura, ProyectoID: ProyectoID }).done(function (result) {

                                    tieneAvance = result; // es false porque al ser nuevo el SP entonces no tiene capturado el avance


                                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                                        if (tipoGuardar == 1) {
                                            $("#grid").data("kendoGrid").dataSource.data([]);
                                            opcionHabilitarView(false, "FieldSetView");
                                            AjaxObtenerColor();
                                            $("#inputNombre").val("");
                                            $("#inputSistemaPinturaID").val("");
                                            $("#divComboProyecto").css("display", "none");
                                            $("#divMultiselectProyecto").css("display", "block");
                                            $("#inputNoAplicable").prop("checked", false);
                                        }
                                        else {
                                            if (result) {
                                                deshabilitar();

                                            }
                                            else {
                                                $("#grid").data("kendoGrid").dataSource.data([]);
                                                opcionHabilitarView(true, "FieldSetView");
                                                $("#inputSistemaPinturaID").val(data.ReturnMessage[2]);
                                                $("#inputhiddenSistemaPinturaID").val(data.ReturnMessage[1]);
                                                AjaxObtenerColor();
                                            }
                                        }
                                        displayNotify("MensajeGuardadoExistoso", "", "0");
                                    }
                                    else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                                        mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2"
                                        displayNotify("MensajeGuardadoErroneo", "", '2');
                                    }

                                    loadingStop();
                                });
                            }
                        });
                    }
                    else {
                        displayNotify("MensajeAdverteciaExcepcionGuardado", "", '1');
                    }
                    opcionHabilitarView(false, "FieldSetView");
                    ventanaConfirmGuardado.close();
                });
                $("#noButtonConfirmarGuardado").click(function () {
                    ventanaConfirmGuardado.close();
                    opcionHabilitarView(false, "FieldSetView");
                    $("#inputNombre").attr('disabled', false);
                });
            }

    } catch (e) {
        loadingStop();
        displayNotify("", e.message, '1');

    }

};

function tieneComponentesSinCaptura(data) {
    var ComponenteIncompleto = false;
    for (var i = 0; i < data.length; i++) {
        if (data[i].Nombre == "" && data[i].Accion != 3) {
            ComponenteIncompleto = true;
            break;
        }
    }
    return ComponenteIncompleto;
}

function AjaxEliminaSistemaPintura(sistemaPintura, proyectoid) {

    $ListadoSistemaPintura.ListadoSistemaPintura.read({ token: Cookies.get("token"), SistemaPinturaID: sistemaPintura, ProyectoID: proyectoid }).done(function (data) {

        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {

            $("#inputNombre").val("");
            $("#inputSistemaPinturaID").val("");
            $("#divComboProyecto").css("display", "none");
            $("#comboProyecto").data("kendoComboBox").value("");
            $("#comboProyecto").data("kendoComboBox").dataSource.data([]);
            $("#inputNombre").attr('disabled', false);
            $("#divMultiselectProyecto").css("display", "block");
            $("#inputNoAplicable").prop("checked", false);
            $("#inputhiddenSistemaPinturaID").val("");
            opcionHabilitarView(false, "FieldSetView");
            $("#inputNombre").attr('disabled', false);
            displayNotify("SistemaPinturaEliminadoExitoso", "", '0');

            AjaxObtenerColor();

        }
        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
            displayNotify("SistemaPinturaErrorEliminado", "", '2');
        }
    });
}

function AjaxVerificarNombre(Nombre, arregloCaptura, tipoGuardar) {

    Existencia = [];
    Existencia[0] = { Detalles: "" };
    ListaValidarNombre = [];

    var necesitaColor = false;
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (Nombre == "") {
        displayNotify("SistemaPinturaMensajeErrorNombre", "", '1');
        return;
    }

    if ($("#inputProyecto").data("kendoMultiSelect")._values.length == 0) {
        displayNotify("SistemaPinturaMensajeErrorListadoProyecto", "", '1');
        return;
    }


    for (var i = 0; i < $("#inputProyecto").data("kendoMultiSelect")._values.length; i++) {
        ListaValidarNombre[i] = { Nombre: "", ProyectoID: "", Existe: "" };
        ListaValidarNombre[i].Nombre = Nombre;
        ListaValidarNombre[i].ProyectoID = $("#inputProyecto").data("kendoMultiSelect")._values[i];

    }
    Existencia[0].Detalles = ListaValidarNombre;
    loadingStart();
    $SistemaPintura.SistemaPintura.create(Existencia[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), comprobar: 1 }).done(function (data) {
        if (Error(data)) {
            var proyectosExistentes = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].Existe == 1) {
                    proyectosExistentes = data[i].Proyecto;
                    if (data[i].length - 1 != i) {
                        proyectosExistentes += ",";
                    }
                }
            }

            if (proyectosExistentes != "") {
                displayNotify("SistemaPinturaMensajeErrorNombreConProyectoExistente", proyectosExistentes + "", '1');
            }
            else {
                AjaxGuardarCaptura(arregloCaptura, tipoGuardar);
            }
        }
    });
}