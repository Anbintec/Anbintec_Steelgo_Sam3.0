var ModeloRenglon;

function SuscribirEventos() {

    SuscribirEventoProyecto();
    SuscribirEventoProveedor();
    SuscribirEventoTipoPrueba();
    SuscribirEventoRequisicion();
    SuscribirEventoRecibido();
    SuscribirEventoCondicionesFisicas();
    SuscribirEventoDefectos();
    SuscribirEventoPlanchado();
    SuscribirEventoGuardar();
    suscribirEventoChangeRadio();
    SuscribirEventoBuscar();
    suscribirEventoDetalleRec();
    suscribirEventoCancelar();
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
            var paramReq = getParameterByName('requisicion');

            if (dataItem != undefined) {
                if (!validaInformacionCapturada()) {
                    $("#inputProyecto").attr("proyectoAntrior", dataItem.ProyectoID);
                    if (paramReq == null) {
                        $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
                        $("#inputTipoPrueba").data("kendoComboBox").value("");
                        $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                        $("#inputProveedor").data("kendoComboBox").value("");
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        if (dataItem.ProyectoID != 0) {
                            AjaxCargaListaTipoPrueba(dataItem.ProyectoID);
                        }
                    }

                } else {
                    $("#ventanaConfirmCaptura").empty();
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: 450,
                        height: 70,
                        draggable: false,
                        modal: true,
                        animation: {
                            close: function () {
                                $("#inputProyecto").data("kendoComboBox").value($("#inputProyecto").attr("proyectoAntrior"));
                            },
                            open: false
                        },
                        actions: [
                            "Close"
                        ]
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {

                        $("#inputProyecto").attr("proyectoAntrior", dataItem.ProyectoID);

                        if (paramReq == null) {
                            $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
                            $("#inputTipoPrueba").data("kendoComboBox").value("");
                            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                            $("#inputProveedor").data("kendoComboBox").value("");
                            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                            $("#inputRequisicion").data("kendoComboBox").value("");
                            $("#grid").data("kendoGrid").dataSource.data([]);

                            if (dataItem.ProyectoID != 0) {
                                AjaxCargaListaTipoPrueba(dataItem.ProyectoID);

                            }
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonProy").click(function () {
                        $("#inputProyecto").data("kendoComboBox").value($("#inputProyecto").attr("proyectoAntrior"));
                        ventanaConfirm.close();
                    });
                }
            } else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoTipoPrueba() {
    $("#inputTipoPrueba").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TipoPruebaID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var paramReq = getParameterByName('requisicion');

            if (dataItem != undefined) {
                if (!validaInformacionCapturada()) {
                    $("#inputTipoPrueba").attr("tipoPruebaAntrior", dataItem.TipoPruebaID)
                    changeLabel(dataItem.TipoPruebaID);

                    if (paramReq == null) {
                        $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                        $("#inputProveedor").data("kendoComboBox").value("");
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#grid").data("kendoGrid").dataSource.data([]);

                        if (dataItem.TipoPruebaID != 0) {
                            AjaxCargaListaProveedores($("#inputProyecto").data("kendoComboBox").value(), dataItem.TipoPruebaID);
                        }
                    }
                } else {
                    $("#ventanaConfirmCaptura").empty();
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: 450,
                        height: 70,
                        draggable: false,
                        modal: true,
                        animation: {
                            close: function () {
                                $("#inputTipoPrueba").data("kendoComboBox").value($("#inputTipoPrueba").attr("tipoPruebaAntrior"));
                            },
                            open: false
                        },
                        actions: [
                            "Close"
                        ],
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonPro'>Si</button><button class='btn btn-blue' id='noButtonPro'>No</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonPro").click(function () {
                        $("#inputTipoPrueba").attr("tipoPruebaAntrior", dataItem.TipoPruebaID)

                        if (paramReq == null) {
                            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                            $("#inputProveedor").data("kendoComboBox").value("");
                            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                            $("#inputRequisicion").data("kendoComboBox").value("");
                            $("#grid").data("kendoGrid").dataSource.data([]);

                            if (dataItem.TipoPruebaID != 0) {
                                AjaxCargaListaProveedores($("#inputProyecto").data("kendoComboBox").value(), dataItem.tipoPruebaID);
                            }
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonPro").click(function () {
                        $("#inputTipoPrueba").data("kendoComboBox").value($("#inputTipoPrueba").attr("tipoPruebaAntrior"));
                        ventanaConfirm.close();
                    });
                }

            } else {
                $("#inputTipoPrueba").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoProveedor() {
    $("#inputProveedor").kendoComboBox({
        dataTextField: "NombreProveedor",
        dataValueField: "ProveedorID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var paramReq = getParameterByName('requisicion');

            if (dataItem != undefined) {
                if (!validaInformacionCapturada()) {
                    $("#inputProveedor").attr("proveedorAntrior", dataItem.ProveedorID);

                    if (paramReq == null) {
                        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                        $("#inputRequisicion").data("kendoComboBox").value("");
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        AjaxCargaListaRequisicion($("#inputProyecto").data("kendoComboBox").value(), $("#inputTipoPrueba").data("kendoComboBox").value(), dataItem.ProveedorID);
                    }
                } else {
                    $("#ventanaConfirmCaptura").empty();
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: 450,
                        height: 70,
                        draggable: false,
                        modal: true,
                        animation: {
                            close: function () {
                                $("#inputProveedor").data("kendoComboBox").value($("#inputProveedor").attr("proveedorAntrior"));
                            },
                            open: false
                        },
                        actions: [
                            "Close"
                        ],
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonPro'>Si</button><button class='btn btn-blue' id='noButtonPro'>No</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonPro").click(function () {
                        $("#inputProveedor").attr("proveedorAntrior", dataItem.ProveedorID);

                        if (paramReq == null) {
                            $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                            $("#inputRequisicion").data("kendoComboBox").value("");
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            AjaxCargaListaRequisicion($("#inputProyecto").data("kendoComboBox").value(), $("#inputTipoPrueba").data("kendoComboBox").value(), dataItem.ProveedorID);
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonPro").click(function () {
                        $("#inputProveedor").data("kendoComboBox").value($("#inputProveedor").attr("proveedorAntrior"));
                        ventanaConfirm.close();
                    });
                }

            } else {
                $("#inputProveedor").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoRequisicion() {
    $("#inputRequisicion").kendoComboBox({
        dataTextField: "NombreRequisicion",
        dataValueField: "RequisicionID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var paramReq = getParameterByName('requisicion');

            if (dataItem != undefined) {
                if (!validaInformacionCapturada()) {
                    $("#inputRequisicion").attr("requisicionAntrior", dataItem.RequisicionID);
                    if (paramReq == null) {
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        if (dataItem.RequisicionID != 0) {
                            if ($("#inputProveedor").data("kendoComboBox").value() == "0" || $("#inputProveedor").data("kendoComboBox").value() == "") {
                                $("#inputProveedor").data("kendoComboBox").value(dataItem.ProveedorID);
                            }
                        }
                    } else {
                        $("#inputProyecto").data("kendoComboBox").value(dataItem.ProyectoID);
                        $("#inputTipoPrueba").data("kendoComboBox").value(dataItem.TipoPruebaID);
                        $("#inputProveedor").data("kendoComboBox").value(dataItem.ProveedorID);
                    }

                } else {
                    $("#ventanaConfirmCaptura").empty();
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: 450,
                        height: 70,
                        draggable: false,
                        modal: true,
                        animation: {
                            close: function (e) {
                                $("#inputRequisicion").data("kendoComboBox").value($("#inputRequisicion").attr("requisicionAntrior"));
                            },
                            open: false
                        },
                        actions: [
                            "Close"
                        ],
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonReq'>Si</button><button class='btn btn-blue' id='noButtonReq'>No</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonReq").click(function () {
                        $("#inputRequisicion").attr("requisicionAntrior", dataItem.RequisicionID);

                        if (paramReq == null) {
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            if ($("#inputProveedor").data("kendoComboBox").value() == "0" || $("#inputProveedor").data("kendoComboBox").value() == "") {
                                $("#inputProveedor").data("kendoComboBox").value(dataItem.ProveedorID);
                            }
                        } else {
                            $("#inputProyecto").data("kendoComboBox").value(data.ProyectoID);
                            $("#inputProveedor").data("kendoComboBox").value(data.ProveedorID);
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonReq").click(function () {
                        $("#inputRequisicion").data("kendoComboBox").value($("#inputRequisicion").attr("requisicionAntrior"));
                        ventanaConfirm.close();
                    });
                }
            } else {
                $("#inputRequisicion").data("kendoComboBox").value("");
            }
        }
    });

    $("#inputRequisicion").closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 13) {

            var paramReq = getParameterByName('requisicion');
            var Requisicion = $("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select());
            if (Requisicion != undefined) {
                if (!validaInformacionCapturada()) {
                    $("#inputRequisicion").attr("requisicionAntrior", Requisicion.RequisicionID);
                    if (paramReq == null) {
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        if (Requisicion.RequisicionID != 0) {
                            if ($("#inputProveedor").data("kendoComboBox").value() == "0") {
                                $("#inputProveedor").data("kendoComboBox").value(Requisicion.ProveedorID);
                            }
                            AjaxObtieneDetalleRequisicion();
                        }
                    } else {
                        $("#inputProyecto").data("kendoComboBox").value(Requisicion.ProyectoID);
                        $("#inputProveedor").data("kendoComboBox").value(Requisicion.ProveedorID);
                        if (Requisicion.RequisicionID != 0) {
                            AjaxObtieneDetalleRequisicion();
                        }
                    }
                } else {
                    $("#ventanaConfirmCaptura").empty();
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: 450,
                        height: 70,
                        draggable: false,
                        modal: true,
                        animation: {
                            close: function (e) {
                                $("#inputRequisicion").data("kendoComboBox").value($("#inputRequisicion").attr("requisicionAntrior"));
                            },
                            open: false
                        },
                        actions: [
                            "Close"
                        ],
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonReq'>Si</button><button class='btn btn-blue' id='noButtonReq1'>No</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonReq").click(function () {
                        $("#inputRequisicion").attr("requisicionAntrior", Requisicion.RequisicionID);
                        if (paramReq == null) {
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            if (Requisicion.RequisicionID != 0) {
                                if ($("#inputProveedor").data("kendoComboBox").value() == "0") {
                                    $("#inputProveedor").data("kendoComboBox").value(Requisicion.ProveedorID);
                                }
                                AjaxObtieneDetalleRequisicion();
                            }
                        } else {
                            $("#inputProyecto").data("kendoComboBox").value(Requisicion.ProyectoID);
                            $("#inputProveedor").data("kendoComboBox").value(Requisicion.ProveedorID);
                            if (Requisicion.RequisicionID != 0) {
                                AjaxObtieneDetalleRequisicion();
                            }
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonReq1").click(function () {
                        $("#inputRequisicion").data("kendoComboBox").value($("#inputRequisicion").attr("requisicionAntrior"));
                        ventanaConfirm.close();
                    });
                }
            } else {
                $("#inputRequisicion").data("kendoComboBox").value("");
            }

        }
    });
}

function SuscribirEventoRecibido() {
    $("#inputDocumentoRecibido").kendoComboBox({
        dataTextField: "DocumentoRecibidoNombre",
        dataValueField: "DocumentoRecibidoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {

            } else {
                $("#inputDocumentoRecibido").data("kendoComboBox").value("");
            }
        }
    })
}

function SuscribirEventoCondicionesFisicas() {
    $("#inputCondicionesFisicas").kendoComboBox({
        dataTextField: "DocumentoEstatusNombre",
        dataValueField: "DocumentoEstatusID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (dataItem.DocumentoEstatusID == 2) {
                    $("#divDefectos").show();
                } else {
                    $("#inputDefectos").data("kendoComboBox").value("");
                    $("#divDefectos").hide();
                }

            } else {
                $("#inputCondicionesFisicas").data("kendoComboBox").value("");
            }
        }
    })
}

function SuscribirEventoDefectos() {
    $("#inputDefectos").kendoComboBox({
        dataTextField: "DocumentoDefectoNombre",
        dataValueField: "DocumentoDefectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {

            } else {
                $("#inputDefectos").data("kendoComboBox").value("");
            }
        }
    })
}

function SuscribirEventoPlanchado() {
    $("#ButtonPlanchar").click(function () {

        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            var tipoLlenado = $('input:radio[name=Planchar]:checked').val()
            if (tipoLlenado === "Todos") {
                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                             "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    plancharTodo(tipoLlenado);
                    limpiaPlanchado();
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            } else {
                plancharTodo(tipoLlenado);
                limpiaPlanchado();
            }
        }
    });
}

function plancharTodo(tipoLlenado) {

    var itemRecibido = $("#inputDocumentoRecibido").data("kendoComboBox").dataItem($("#inputDocumentoRecibido").data("kendoComboBox").select());
    var itemEstatus = $("#inputCondicionesFisicas").data("kendoComboBox").dataItem($("#inputCondicionesFisicas").data("kendoComboBox").select());
    var itemDefecto = $("#inputDefectos").data("kendoComboBox").dataItem($("#inputDefectos").data("kendoComboBox").select());

    if (itemRecibido != undefined && itemRecibido.DocumentoRecibidoID != 0) {
        PlanchaDocumentoRecibido(tipoLlenado);
    }
    if (itemEstatus != undefined && itemEstatus.DocumentoEstatusID != 0) {
        PlanchaDocumentoEstatus(tipoLlenado);
    }

    if (itemDefecto != undefined && itemDefecto.DocumentoDefectoID != 0 && itemEstatus.DocumentoEstatusID == 2) {
        PlanchaDocumentoDefecto(tipoLlenado);
    }
}

function SuscribirEventoGuardar() {
    //GuardarYNuevo
    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxGuardarCaptura(ds._data, true);
    });

    //Guardar
    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {
        if ($("#Guardar").text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            AjaxGuardarCaptura(ds._data, false);
        } else if ($("#Guardar").text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            disableEnableView(false);
        }
    });
}

function cleanView() {
    var paramReq = getParameterByName('requisicion');

    $("#inputProyecto").data("kendoComboBox").value("");
    $("#inputTipoPrueba").data("kendoComboBox").value("");
    $("#inputProveedor").data("kendoComboBox").value("");
    $("#inputRequisicion").data("kendoComboBox").value("");
    $("#inputDocumentoRecibido").data("kendoComboBox").value("");
    $("#inputCondicionesFisicas").data("kendoComboBox").value("");
    $("#inputDefectos").data("kendoComboBox").value("");
    $("#grid").data("kendoGrid").dataSource.data([]);
    if (paramReq == null) {
        AjaxCargaListaProyectos();
    } else {
        AjaxObtenerElementoRequisicion(paramReq);
    }
    disableEnableView(false);
    AjaxCargarCamposPredeterminados();
}

function limpiaPlanchado() {
    $("#inputDocumentoRecibido").data("kendoComboBox").value("");
    $("#inputCondicionesFisicas").data("kendoComboBox").value("");
    $("#inputDefectos").data("kendoComboBox").value("");
}

function disableEnableView(disable) {
    if (disable) {
        $(".addedSectionInLine").find('*').attr("disabled", true);

        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputProveedor").data("kendoComboBox").enable(false);
        $("#inputTipoPrueba").data("kendoComboBox").enable(false);
        $("#inputRequisicion").data("kendoComboBox").enable(false);
        $("input[name='Muestra']").attr("disabled", true);
        $("#ButtonBuscar").attr("disabled", true);

        $("#inputDocumentoRecibido").data("kendoComboBox").enable(false);
        $("#inputCondicionesFisicas").data("kendoComboBox").enable(false);
        $("#inputDefectos").data("kendoComboBox").enable(false);
        $("input[name='Planchar']").attr("disabled", true);
        $("#ButtonPlanchar").attr("disabled", true);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    } else {
        $(".addedSectionInLine").find('*').attr("disabled", false);

        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputTipoPrueba").data("kendoComboBox").enable(true);
        $("#inputProveedor").data("kendoComboBox").enable(true);
        $("#inputRequisicion").data("kendoComboBox").enable(true);
        $("input[name='Muestra']").attr("disabled", false);
        $("#ButtonBuscar").attr("disabled", false);

        $("#inputDocumentoRecibido").data("kendoComboBox").enable(true);
        $("#inputCondicionesFisicas").data("kendoComboBox").enable(true);
        $("#inputDefectos").data("kendoComboBox").enable(true);
        $("input[name='Planchar']").attr("disabled", false);
        $("#ButtonPlanchar").attr("disabled", false);

        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        FiltroMostrar(0);
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        FiltroMostrar(1);
    });
}

function SuscribirEventoBuscar() {
    $("#ButtonBuscar").click(function () {
        $("#grid").data("kendoGrid").dataSource.data([]);
        var Requisicion = $("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select());
        if (Requisicion != undefined) {
            $("#inputRequisicion").attr("requisicionAntrior", Requisicion.RequisicionID);
            if (Requisicion.RequisicionID != 0) {
                AjaxObtieneDetalleRequisicion();
            } else {
                $("#grid").data("kendoGrid").dataSource.data([]);
                displayNotify("EntregaPlacasGraficasErrorRequisicion", "", '2');
            }
        } else {
            $("#inputRequisicion").data("kendoComboBox").value("");
            displayNotify("EntregaPlacasGraficasErrorRequisicion", "", '2');
        }
    });

}

function changeLabel(tipoPruebaID) {
    if (tipoPruebaID == 12) {
        $("#grid th[data-field=DocumentoEstatus]").html(_dictionary.columnCondicionesFisicasDocto[$("#language").data("kendoDropDownList").value()]);
        $("#lblCondicionesPlacasOGraficas").text(_dictionary.lblCondicionesFisicasDocto[$("#language").data("kendoDropDownList").value()]);
    } else if (tipoPruebaID > 0 && tipoPruebaID < 12) {
        $("#grid th[data-field=DocumentoEstatus]").html(_dictionary.columnCondicionesFisicasSL[$("#language").data("kendoDropDownList").value()]);
        $("#lblCondicionesPlacasOGraficas").text(_dictionary.lblCondicionesFisicasSL[$("#language").data("kendoDropDownList").value()]);
    } else {
        $("#grid th[data-field=DocumentoEstatus]").html("N/A");
        $("#lblCondicionesPlacasOGraficas").text("N/A");
    }
}



function suscribirEventoDetalleRec() {
    $(document).on('click', '.EnlaceDetalle', function (e) {

        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));

            if (dataItem.Recibida != "" && dataItem.Recibida != undefined) {
                ModeloRenglon = dataItem;
                LlenarGridPopUpDetalleDan(dataItem);
            }
        }
    });


}

function suscribirEventoCancelar() {
    $(document).on('click', '#CancelarPlacasDanadas, #GuardarPlacasDanadas', function (e) {
        $("#windowGridDanadas").data("kendoWindow").close();
    });

}
