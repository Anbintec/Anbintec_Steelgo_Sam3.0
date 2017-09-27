var ChoferInicial = 0;
var proveedorInicial = 0;
var EmbarqueIncial = 0;
var divNuevoEmbarque;

function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoProveedor();
    SuscribirEventoProveedorEnvio();
    SuscribirEventoTracto();
    SuscribirEventoChofer();
    SuscribirEventoTractoEnvio();
    SuscribirEventoChoferEnvio();
    suscribirEventoEmbarque();
    SuscribirEventoPlana();
    suscribirEventoAgregar();
    suscribirEventoGuardar();
    SuscribirEventoCancelarPopup();
    SuscribirEventoGuardarProveedor();
    SuscribirEventoGuardarTracto();
    SuscribirEventoGuardarChofer();
    SuscribirEventoFecha();
    SuscribirEventoEliminarEmbarque();
}

SuscribirEventos();

function SuscribirEventoProyecto() {
    $("#Proyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource;

            if (ds._data.length == 0) {
                if (dataItem != undefined) {
                    proyectoInicial = dataItem.ProyectoID;
                    LimpiarSelectProyecto();
                    if (dataItem.ProyectoID != 0) {
                        AjaxObtenerPlanas(dataItem.ProyectoID, null);
                        AjaxEmbarqueCargaProveedores(dataItem.ProyectoID, null);
                    }
                }
                else {
                    $("#Proyecto").data("kendoComboBox").value("");
                }
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                    + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                    + "</button></center>");
                ventanaConfirm.open().center();

                $("#yesButtonProy").click(function () {
                    if (dataItem != undefined) {
                        proyectoInicial = dataItem.ProyectoID;
                        LimpiarSelectProyecto();
                        if (dataItem.ProyectoID != 0) {
                            AjaxObtenerPlanas(dataItem.ProyectoID, null);
                            AjaxEmbarqueCargaProveedores(dataItem.ProyectoID, null);
                        }
                    }
                    else {
                        $("#Proyecto").data("kendoComboBox").value("");
                    }

                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#Proyecto").data("kendoComboBox").value(proyectoInicial);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoProveedor() {
    $("#Proveedor").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProveedorID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource;

            if (ds._data.length == 0) {
                if (dataItem != undefined) {
                    proveedorInicial = $("#Proveedor").data("kendoComboBox").value();
                    LimpiarSelectProveedor();
                    if (dataItem.ProveedorID == -1) {
                        CargaPopupNuevoProveedor();
                    } else {
                        if (dataItem.ProveedorID != 0) {
                            AjaxEmbarqueCargaTractos(dataItem.ProveedorID, null);
                            AjaxEmbarqueCargaChofer(dataItem.ProveedorID, null);
                            AjaxObtenerEmbarque(dataItem.ProveedorID, null);
                        }
                    }
                }
                else {
                    $("#Proveedor").data("kendoComboBox").value("");
                }
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                    + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                    + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    if (dataItem != undefined) {
                        proveedorInicial = $("#Proveedor").data("kendoComboBox").value();
                        LimpiarSelectProveedor();
                        if (dataItem.ProveedorID == -1) {
                            CargaPopupNuevoProveedor();

                        } else {
                            if (dataItem.ProveedorID != 0) {
                                AjaxEmbarqueCargaTractos(dataItem.ProveedorID, null);
                                AjaxEmbarqueCargaChofer(dataItem.ProveedorID, null);
                                AjaxObtenerEmbarque(dataItem.ProveedorID, null);
                            }
                        }
                    }
                    else {
                        $("#Proveedor").data("kendoComboBox").value("");
                    }

                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#Proveedor").data("kendoComboBox").value(proveedorInicial);
                    ventanaConfirm.close();
                });
            }

        }
    });
}


function SuscribirEventoProveedorEnvio() {
    $("#ProveedorEnvio").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProveedorID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource;

            if (ds._data.length == 0) {
                if (dataItem != undefined) {
                    proveedorInicial = $("#ProveedorEnvio").data("kendoComboBox").value();
                    LimpiarSelectProveedorEnvio();
                    if (dataItem.ProveedorID == -1) {
                        CargaPopupNuevoProveedor();
                    } else {
                        if (dataItem.ProveedorID != 0) {
                            AjaxEmbarqueCargaTractosEnvio(dataItem.ProveedorID, null);
                            AjaxEmbarqueCargaChoferEnvio(dataItem.ProveedorID, null);
                            //AjaxObtenerEmbarque(dataItem.ProveedorID, null);
                        }
                    }
                }
                else {
                    $("#ProveedorEnvio").data("kendoComboBox").value("");
                }
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                    + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                    + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    if (dataItem != undefined) {
                        proveedorInicial = $("#ProveedorEnvio").data("kendoComboBox").value();
                        LimpiarSelectProveedorEnvio();
                        if (dataItem.ProveedorID == -1) {
                            CargaPopupNuevoProveedor();

                        } else {
                            if (dataItem.ProveedorID != 0) {
                                AjaxEmbarqueCargaTractosEnvio(dataItem.ProveedorID, null);
                                AjaxEmbarqueCargaChoferEnvio(dataItem.ProveedorID, null);
                                //AjaxObtenerEmbarque(dataItem.ProveedorID, null);
                            }
                        }
                    }
                    else {
                        $("#ProveedorEnvio").data("kendoComboBox").value("");
                    }

                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#ProveedorEnvio").data("kendoComboBox").value(proveedorInicial);
                    ventanaConfirm.close();
                });
            }

        }
    });
}


function SuscribirEventoTracto() {
    $("#Tracto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TractoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                if (dataItem.TractoID == -1) {
                    CargaPopupNuevoTracto();
                   $("#Tracto").data("kendoComboBox").value("");
                }
            }
            else {
                $("#Tracto").data("kendoComboBox").value("");
             }            
        }
    });
}

function SuscribirEventoTractoEnvio() {
    $("#TractoEnvio").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TractoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (dataItem.TractoID == -1) {
                    CargaPopupNuevoTracto();
                    $("#TractoEnvio").data("kendoComboBox").value("");
                }
            }
            else {
                $("#TractoEnvio").data("kendoComboBox").value("");
            }
        }
    });
}

function suscribirEventoGuardar() {

    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            if ($("#Proyecto").data("kendoComboBox").text() != "") {
                if ($("#Proveedor").data("kendoComboBox").text() != "") {                    
                    if ($("#Tracto").data("kendoComboBox").text() != "") {                        
                        if ($("#Chofer").data("kendoComboBox").text() != "") {
                            var ds = $("#grid").data("kendoGrid").dataSource;
                            if (ds._data.length > 0) {
                                var Embarque = $("#Embarque").data("kendoComboBox").dataItem($("#Embarque").data("kendoComboBox").select());
                                AbrirPopUpGuardar(Embarque, 1);
                            }
                            else {
                                displayNotify('EmarquePreparacionMensajeErrorEmbarqueVacio', '', '2');
                            }
                        }
                        else {
                            displayNotify('MensajeSeleccionaChofer', '', '2');
                        }
                    }
                    else {
                        displayNotify('MensajeSeleccionaTracto', '', '2');
                    }
                }
                else {
                    displayNotify('MensajeSeleccionaProveedor', '', '2');
                }
            }
            else {
                displayNotify('MensajeSeleccionaProyecto', '', '2');
            }
        }
        else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "FieldSetView");
        }        
    });

    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        if ($("#Proyecto").data("kendoComboBox").text() != "") {
            if ($("#Proveedor").data("kendoComboBox").text() != "") {
                if ($("#Tracto").data("kendoComboBox").text() != "") {
                    if ($("#Chofer").data("kendoComboBox").text() != "") {
                        var ds = $("#grid").data("kendoGrid").dataSource;
                        if (ds._data.length > 0) {
                            var Embarque = $("#Embarque").data("kendoComboBox").dataItem($("#Embarque").data("kendoComboBox").select());
                            AbrirPopUpGuardar(Embarque, 2);
                        }
                        else {
                            displayNotify('EmarquePreparacionMensajeErrorEmbarqueVacio', '', '2');
                        }
                    }
                    else {
                        displayNotify('MensajeSeleccionaChofer', '', '2');
                    }
                }
                else {
                    displayNotify('MensajeSeleccionaTracto', '', '2');
                }
            }
            else {
                displayNotify('MensajeSeleccionaProveedor', '', '2');
            }
        }
        else {
            displayNotify('MensajeSeleccionaProyecto', '', '2');
        }
    });
}

function suscribirEventoAgregar() {
    $('#btnAgregar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length < 2) {
            if ($("#Plana").data("kendoComboBox").text() != "" && $("#Plana").data("kendoComboBox").text() != undefined) {
                var cargaPlanaID = $("#Plana").data("kendoComboBox").dataItem($("#Plana").data("kendoComboBox").select()).CargaPlanaID;
                AjaxAgregaRenglon(cargaPlanaID);
                $("#Plana").data("kendoComboBox").value("");
            }
            else {
                displayNotify('EmarquePreparacionErrorSeleccionaPlana', '', '1');
            }
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorCargaMaxima', '', '1');
        }
    });
}

function suscribirEventoEmbarque() {
    $("#Embarque").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "EmbarqueID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource;

            if (ds._data.length == 0) {
                if (dataItem != undefined) {
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    EmbarqueIncial = dataItem.EmbarqueID;

                    if (dataItem.EmbarqueID != 0) {
                        AjaxObtieneDetalle(dataItem.EmbarqueID);
                        TractoEmbarque = dataItem.TractoID;
                        ChoferEmbarque = dataItem.ChoferID;

                        $("#Tracto").data("kendoComboBox").value(dataItem.TractoID);
                        $("#Chofer").data("kendoComboBox").value(dataItem.ChoferID);
                    }
                }
                else {
                    $("#Embarque").data("kendoComboBox").value("");
                    $("#Tracto").data("kendoComboBox").value("");
                    $("#Chofer").data("kendoComboBox").value("");
                }
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                    + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                    + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    if (dataItem != undefined) {
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        EmbarqueIncial = dataItem.EmbarqueID;

                        if (dataItem.EmbarqueID != 0) {
                            AjaxObtieneDetalle(dataItem.EmbarqueID);
                        }
                    }
                    else {
                        $("#Embarque").data("kendoComboBox").value("");
                    }

                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#Embarque").data("kendoComboBox").value(EmbarqueIncial);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoChofer() {
    $("#Chofer").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ChoferID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {                
                if (dataItem.ChoferID == -1) {
                    CargaPopupNuevoChofer();
                    $("#Chofer").data("kendoComboBox").value("");
                }
            }
            else {
                $("#Chofer").data("kendoComboBox").value("");
            }
        }
    });
}


function SuscribirEventoChoferEnvio() {
    $("#ChoferEnvio").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ChoferID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                if (dataItem.ChoferID == -1) {
                    CargaPopupNuevoChofer();
                    $("#ChoferEnvio").data("kendoComboBox").value("");
                }
            }
            else {
                $("#ChoferEnvio").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoPlana() {
    $("#Plana").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "PlanaID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
            }
            else {
                $("#Plana").data("kendoComboBox").value("");
            }
        }
    });

    $('#Plana').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if ($("#Proveedor").data("kendoComboBox").value() != "" && $("#Proveedor").data("kendoComboBox").value() != "0") {
                if (ds._data.length < 2) {
                    if ($("#Plana").data("kendoComboBox").text() != "" && $("#Plana").data("kendoComboBox").text() != undefined) {
                        var cargaPlanaID = $("#Plana").data("kendoComboBox").dataItem($("#Plana").data("kendoComboBox").select()).CargaPlanaID;
                        AjaxAgregaRenglon(cargaPlanaID);

                        $("#Plana").data("kendoComboBox").value("");
                    }
                    else {
                        displayNotify('EmarquePreparacionMensajeEligePlana', '', '1');
                    }
                }
                else {
                    displayNotify('EmarquePreparacionMensajeErrorCargaMaxima', '', '1');
                }
            }else
                displayNotify('MensajeSeleccionaProveedor', '', '1');
        }
    });

}


function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#Proyecto").data("kendoComboBox").enable(false);
        $("#Chofer").data("kendoComboBox").enable(false);
        $("#ChoferEnvio").data("kendoComboBox").enable(false);
        $("#Plana").data("kendoComboBox").enable(false);
        $("#Tracto").data("kendoComboBox").enable(false);
        $("#TractoEnvio").data("kendoComboBox").enable(false);
        $("#Proveedor").data("kendoComboBox").enable(false);
        $("#ProveedorEnvio").data("kendoComboBox").enable(false);
        $("#Embarque").data("kendoComboBox").enable(false);

        $("#btnAgregar").prop('disabled', true);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#Proyecto").data("kendoComboBox").enable(true);
        $("#Chofer").data("kendoComboBox").enable(true);
        $("#ChoferEnvio").data("kendoComboBox").enable(true);
        $("#Plana").data("kendoComboBox").enable(true);
        $("#Tracto").data("kendoComboBox").enable(true);
        $("#TractoEnvio").data("kendoComboBox").enable(true);
        $("#Proveedor").data("kendoComboBox").enable(true);
        $("#ProveedorEnvio").data("kendoComboBox").enable(true);
        $("#Embarque").data("kendoComboBox").enable(true);

        $("#btnAgregar").prop('disabled', false);

        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function SuscribirEventoGuardarProveedor() {
    $('#GuardarNuevoProveedor').click(function (e) {
        if ($("#inputNombreNuevoProveedor").val() != "") {
            GuardarNuevoProveedor();
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorNombreProveedor', '', '1');
        }
    });
}

function SuscribirEventoGuardarTracto() {
    $('#GuardarNuevoTracto').click(function (e) {
        if ($("#inputNombreNuevoTracto").val() != "") {
            GuardarNuevoTracto();
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorNombreTracto', '', '1');
        }
    });
}


function SuscribirEventoGuardarChofer() {
    $('#GuardarNuevoChofer').click(function (e) {
        if ($("#inputNombreNuevoChofer").val() != "") {
            GuardarNuevoChofer();
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorNombreChofer', '', '1');
        }
    });
}


function SuscribirEventoCancelarPopup() {
    $('#CancelarNuevoProveedor').click(function (e) {
        $("#Proveedor").data("kendoComboBox").value("");
        windowNewProvider.close();
    });
    $('#CancelarNuevoTracto').click(function (e) {
        $("#Tracto").data("kendoComboBox").text("");
        windowNewTracto.close();
    });
    $('#CancelarNuevoChofer').click(function (e) {
        $("#Chofer").data("kendoComboBox").text("");
        windowNewChofer.close();
    });
}

function SuscribirEventoFecha() {
    FechaEmbarque = $("#inputFechaEmbarque").kendoDatePicker({
        max: new Date(),
        change: function (e) {
            ValidarFecha(e.sender._value)
        }
    });
}

function SuscribirEventoPopUpGuardarEmbarque() {

    divNuevoEmbarque = $("#divNuevoEmbarque").kendoWindow({
        title: _dictionary.EmbarquePreparacionEmbarqueNuevo[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "50%",
        height: "auto",
        draggable: false,
        resizable: false,
        modal: true,
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        actions: []
   }).data("kendoWindow");

    $("#GuardarNuevoEmbarque").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource._data;
        var tipoGuardado = $("#InputTipoGuardado").val();
        var proveedorID = $("#Proveedor").data("kendoComboBox").value();
        var nombreEmbarque = $("#inputNombreEmbarque").val();
        var nombreEmbarqueCliente = $("#inputNombreEmbarqueCliente").val();
        var fechaCreacion = $("#inputFechaEmbarque").val();

        if (nombreEmbarque != "") {
            if (fechaCreacion != undefined && fechaCreacion != "") {
                divNuevoEmbarque.close();
                AjaxGuardarCaptura(ds, tipoGuardado, proveedorID);
            } else {
                displayNotify("EmbarquePreparacionNumEmbarqueSteelgoError", "", '2');
            }
        } else {
            displayNotify("EmbarquePreparacionNumEmbarqueSteelgoError", "", '2');
        }
    });

    $("#CancelarNuevoEmbarque").click(function (e) {
        divNuevoEmbarque.close();
    });
                    
}

function SuscribirEventoEliminarEmbarque() {
    $("#btnELiminarEmbarque, #btnELiminarEmbarque1").click(function (e) {
        if ($('#Guardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            if ($("#Embarque").data("kendoComboBox").value() != "" && $("#Embarque").data("kendoComboBox").value() != "0") {
                AjaxEliminarEmbarque($("#Embarque").data("kendoComboBox").value());
            } else {
                displayNotify("MensajeSeleccionaEmbarque", "", '2');
            }
        }
    });
}


function LimpiarSelectProveedor() {

    $("#Tracto").data("kendoComboBox").dataSource.data([]);
    $("#Tracto").data("kendoComboBox").value("");

    $("#Chofer").data("kendoComboBox").dataSource.data([]);
    $("#Chofer").data("kendoComboBox").value("");

    $("#Embarque").data("kendoComboBox").dataSource.data([]);
    $("#Embarque").data("kendoComboBox").value("");

    $("#grid").data("kendoGrid").dataSource.data([]);
}
function LimpiarSelectProveedorEnvio() {

    $("#TractoEnvio").data("kendoComboBox").dataSource.data([]);
    $("#TractoEnvio").data("kendoComboBox").value("");

    $("#ChoferEnvio").data("kendoComboBox").dataSource.data([]);
    $("#ChoferEnvio").data("kendoComboBox").value("");    
    //$("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiarSelectProyecto() {


    $("#Proveedor").data("kendoComboBox").dataSource.data([]);
    $("#Proveedor").data("kendoComboBox").value("");

    $("#Tracto").data("kendoComboBox").dataSource.data([]);
    $("#Tracto").data("kendoComboBox").value("");

    $("#Chofer").data("kendoComboBox").dataSource.data([]);
    $("#Chofer").data("kendoComboBox").value("");

    $("#Embarque").data("kendoComboBox").dataSource.data([]);
    $("#Embarque").data("kendoComboBox").value("");


    $("#Plana").data("kendoComboBox").dataSource.data([]);
    $("#Plana").data("kendoComboBox").value("");

    $("#grid").data("kendoGrid").dataSource.data([]);
}

function Limpiar() {
    $("#Proyecto").data("kendoComboBox").dataSource.data([]);
    $("#Proyecto").data("kendoComboBox").value("");

    $("#Proveedor").data("kendoComboBox").dataSource.data([]);
    $("#Proveedor").data("kendoComboBox").value("");

    $("#Tracto").data("kendoComboBox").dataSource.data([]);
    $("#Tracto").data("kendoComboBox").value("");

    $("#Chofer").data("kendoComboBox").dataSource.data([]);
    $("#Chofer").data("kendoComboBox").value("");

    $("#Embarque").data("kendoComboBox").dataSource.data([]);
    $("#Embarque").data("kendoComboBox").value("");


    $("#Plana").data("kendoComboBox").dataSource.data([]);
    $("#Plana").data("kendoComboBox").value("");
    
    $("#grid").data("kendoGrid").dataSource.data([]);

    opcionHabilitarView(false, "");
    AjaxCargarProyecto();
}