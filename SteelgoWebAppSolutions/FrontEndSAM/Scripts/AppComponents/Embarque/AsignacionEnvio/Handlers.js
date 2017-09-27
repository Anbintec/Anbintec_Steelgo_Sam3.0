var ChoferInicial = 0;
var proveedorInicial = 0;
var EmbarqueIncial = 0;
var divNuevoEmbarque;

function SuscribirEventos() {
    SuscribirEventoProyecto();
    suscribirEventoEmbarque();
    SuscribirEventoProveedor();
    SuscribirEventoTracto();
    SuscribirEventoChofer();
    SuscribirEventoProveedorEnvio();
    SuscribirEventoTractoEnvio();
    SuscribirEventoChoferEnvio();
    SuscribirEventoGuardarProveedorEnvio();
    SuscribirEventoGuardarTractoEnvio();
    SuscribirEventoGuardarChoferEnvio();
    suscribirEventoGuardar();
    SuscribirEventoCancelarPopup();    
    //SuscribirEventoFecha();
    //SuscribirEventoEliminarEmbarque();
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
            if (dataItem != undefined) {
                proyectoInicial = dataItem.ProyectoID;
                LimpiarSelectProyecto();
                if (dataItem.ProyectoID != 0) {                    
                    AjaxGetEmbarques(dataItem.ProyectoID);
                    //AjaxGetProveedoresEnvio(dataItem.ProyectoID);
                }
            }
            else {
                $("#Proyecto").data("kendoComboBox").value("");
            }
        }       
    });
}


function suscribirEventoEmbarque() {
    $("#Embarque").kendoComboBox({
        dataTextField: "Embarque",
        dataValueField: "EmbarqueID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {            
            var ds = this.dataSource._data;
            if (ds != undefined) {
                if (ds.length > 0) {
                    if (ds.length == 1) {
                        /*LLENO DATOS DE PROVEEDOR, TRACTO Y CHOFER DEL EMBARQUE*/
                        $("#Proveedor").data("kendoComboBox").dataSource.data(ds[0].ListaDatosEmbarque);
                        $("#Proveedor").data("kendoComboBox").value(ds[0].ListaDatosEmbarque[0].ProveedorID);
                        $("#Tracto").data("kendoComboBox").dataSource.data(ds[0].ListaDatosEmbarque);
                        $("#Tracto").data("kendoComboBox").value(ds[0].ListaDatosEmbarque[0].TractoID);
                        $("#Chofer").data("kendoComboBox").dataSource.data(ds[0].ListaDatosEmbarque);
                        $("#Chofer").data("kendoComboBox").value(ds[0].ListaDatosEmbarque[0].ChoferID);
                        /*LLENO DATOS DE PROVEEDOR, TRACTO Y CHOFER DE ENVIO*/
                        ObtenerTractoYChoferEnvio(ds);
                    }
                }                
            }
        }
    });
}

function ObtenerTractoYChoferEnvio(data) {
    var TractoE = 0, ChoferE = 0;
    if (data.length > 0) {
        if (data.length == 1) {
            for (var i = 0; i < data[0].ListaProveedorEnvio.length; i++) {
                if (data[0].ListaDatosEmbarque[0].ProveedorEnvioID == data[0].ListaProveedorEnvio[i].ProveedorEnvioID) {
                    $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data[0].ListaProveedorEnvio);
                    $("#ProveedorEnvio").data("kendoComboBox").value(data[0].ListaProveedorEnvio[i].ProveedorEnvioID);
                }
            }
            for (var j = 0; j < data[0].ListaDatosEmbarque[0].ListaTractoEnvio.length; j++) {
                if (data[0].ListaDatosEmbarque[0].TractoEnvioID == data[0].ListaDatosEmbarque[0].ListaTractoEnvio[j].TractoEnvioID) {
                    $("#TractoEnvio").data("kendoComboBox").dataSource.data(data[0].ListaDatosEmbarque[0].ListaTractoEnvio);
                    $("#TractoEnvio").data("kendoComboBox").value(data[0].ListaDatosEmbarque[0].ListaTractoEnvio[j].TractoEnvioID);
                }
            }
            for (var a = 0; a < data[0].ListaDatosEmbarque[0].ListaChoferEnvio.length; a++) {
                if (data[0].ListaDatosEmbarque[0].ChoferEnvioID == data[0].ListaDatosEmbarque[0].ListaChoferEnvio[a].ChoferEnvioID) {
                    $("#ChoferEnvio").data("kendoComboBox").dataSource.data(data[0].ListaDatosEmbarque[0].ListaChoferEnvio);
                    $("#ChoferEnvio").data("kendoComboBox").value(data[0].ListaDatosEmbarque[0].ListaChoferEnvio[a].ChoferEnvioID);
                }
            }
            
            data[0].ListaProveedorEnvio.splice(0, 0, { ProveedorEnvioID: -1, ProveedorEnvio: _dictionary.EmarquePreparacionAgregarProveedorEnvio[$("#language").data("kendoDropDownList").value()] });            
            data[0].ListaDatosEmbarque[0].ListaTractoEnvio.splice(0, 0, { TractoEnvioID: -1, TractoEnvio: _dictionary.EmarquePreparacionAgregarTractoEnvio[$("#language").data("kendoDropDownList").value()] });
            data[0].ListaDatosEmbarque[0].ListaChoferEnvio.splice(0, 0, { ChoferEnvioID: -1, ChoferEnvio: _dictionary.EmarquePreparacionAgregarChoferEnvio[$("#language").data("kendoDropDownList").value()] });            
        } else {
            $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data[0].ListaProveedorEnvio);
            $("#TractoEnvio").data("kendoComboBox").dataSource.data(data[0].ListaDatosEmbarque[0].ListaTractoEnvio);
            $("#ChoferEnvio").data("kendoComboBox").dataSource.data(data[0].ListaDatosEmbarque[0].ListaChoferEnvio);
        }        
    }
}

function SuscribirEventoProveedor() {
    $("#Proveedor").kendoComboBox({
        dataTextField: "Proveedor",
        dataValueField: "ProveedorID",
        suggest: true,
        filter: "contains",
        index: 3       
    });
}

function SuscribirEventoProveedorEnvio() {
    $("#ProveedorEnvio").kendoComboBox({
        dataTextField: "ProveedorEnvio",
        dataValueField: "ProveedorEnvioID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (dataItem.ProveedorEnvioID == -1) {
                    CargaPopupNuevoProveedor();
                } else {
                    AjaxGetTractoEnvio(dataItem.ProveedorEnvioID, null);
                    AjaxGetChoferesEnvio(dataItem.ProveedorEnvioID, null);                    
                }                              
            }
        }
    });
}


function SuscribirEventoTracto() {
    $("#Tracto").kendoComboBox({
        dataTextField: "Tracto",
        dataValueField: "TractoID",
        suggest: true,
        filter: "contains",
        index: 3
    });
}

function SuscribirEventoTractoEnvio() {
    $("#TractoEnvio").kendoComboBox({
        dataTextField: "TractoEnvio",
        dataValueField: "TractoEnvioID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (dataItem.TractoEnvioID == -1) {
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

function SuscribirEventoChofer() {
    $("#Chofer").kendoComboBox({
        dataTextField: "Chofer",
        dataValueField: "ChoferID",
        suggest: true,
        filter: "contains",
        index: 3
    });
}


function SuscribirEventoChoferEnvio() {
    $("#ChoferEnvio").kendoComboBox({
        dataTextField: "ChoferEnvio",
        dataValueField: "ChoferEnvioID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if (dataItem.ChoferEnvioID == -1) {
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


function suscribirEventoGuardar() {

    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            if ($("#Proyecto").data("kendoComboBox").text() != "") {
                if ($("#Embarque").data("kendoComboBox").text() != "") {
                    if ($("#ProveedorEnvio").data("kendoComboBox").text() != "") {
                        if ($("#TractoEnvio").data("kendoComboBox").text() != "") {
                            if ($("#ChoferEnvio").data("kendoComboBox").text() != "") {
                                GuardarCaptura(1);
                            } else {
                                displayNotify("MensajeSeleccionaChoferEnvio", "", "1");
                            }
                        } else {
                            displayNotify("MensajeSeleccionaTractoEnvio", "", "1");
                        }
                    } else {
                        displayNotify("MensajeSeleccionaProveedorEnvio", "", "1");
                    }
                } else {
                    displayNotify("MensajeSeleccionaEmbarque", "", "1");
                }
            } else {
                displayNotify("CapturaAvanceCuadranteNoProyecto", "", "1");
            }
        }
        else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "FieldSetView");
        }        
    });

    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            if ($("#Proyecto").data("kendoComboBox").text() != "") {
                if ($("#Embarque").data("kendoComboBox").text() != "") {
                    if ($("#ProveedorEnvio").data("kendoComboBox").text() != "") {
                        if ($("#TractoEnvio").data("kendoComboBox").text() != "") {
                            if ($("#ChoferEnvio").data("kendoComboBox").text() != "") {
                                GuardarCaptura(2);
                            } else {
                                displayNotify("MensajeSeleccionaChoferEnvio", "", "1");
                            }
                        } else {
                            displayNotify("MensajeSeleccionaTractoEnvio", "", "1");
                        }
                    } else {
                        displayNotify("MensajeSeleccionaProveedorEnvio", "", "1");
                    }
                } else {
                    displayNotify("MensajeSeleccionaEmbarque", "", "1");
                }
            } else {
                displayNotify("CapturaAvanceCuadranteNoProyecto", "", "1");
            }
        }
        else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "FieldSetView");
        }
    });
}


function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#Proyecto").data("kendoComboBox").enable(false);
        $("#Chofer").data("kendoComboBox").enable(false);
        $("#ChoferEnvio").data("kendoComboBox").enable(false);        
        $("#Tracto").data("kendoComboBox").enable(false);
        $("#TractoEnvio").data("kendoComboBox").enable(false);
        $("#Proveedor").data("kendoComboBox").enable(false);
        $("#ProveedorEnvio").data("kendoComboBox").enable(false);
        $("#Embarque").data("kendoComboBox").enable(false);
        
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
        $("#Tracto").data("kendoComboBox").enable(true);
        $("#TractoEnvio").data("kendoComboBox").enable(true);
        $("#Proveedor").data("kendoComboBox").enable(true);
        $("#ProveedorEnvio").data("kendoComboBox").enable(true);
        $("#Embarque").data("kendoComboBox").enable(true);
        
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function SuscribirEventoGuardarProveedorEnvio() {
    $('#GuardarNuevoProveedorEnvio').click(function (e) {
        if ($("#inputNombreNuevoProveedorEnvio").val() != "") {
            GuardarNuevoProveedorEnvio();
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorNombreProveedor', '', '1');
        }
    });
}

function SuscribirEventoGuardarTractoEnvio() {
    $('#GuardarNuevoTractoEnvio').click(function (e) {
        if ($("#inputNombreNuevoTractoEnvio").val() != "") {
            GuardarNuevoTractoEnvio();
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorNombreTracto', '', '1');
        }
    });
}


function SuscribirEventoGuardarChoferEnvio() {
    $('#GuardarNuevoChoferEnvio').click(function (e) {
        if ($("#inputNombreNuevoChoferEnvio").val() != "") {
            GuardarNuevoChoferEnvio();
        }
        else {
            displayNotify('EmarquePreparacionMensajeErrorNombreChofer', '', '1');
        }
    });
}


function SuscribirEventoCancelarPopup() {
    $('#CancelarNuevoProveedorEnvio').click(function (e) {
        $("#Proveedor").data("kendoComboBox").value("");
        windowNewProvider.close();
    });
    $('#CancelarNuevoTractoEnvio').click(function (e) {
        $("#Tracto").data("kendoComboBox").text("");
        windowNewTracto.close();
    });
    $('#CancelarNuevoChoferEnvio').click(function (e) {
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

    //$("#grid").data("kendoGrid").dataSource.data([]);
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


    $("#ProveedorEnvio").data("kendoComboBox").dataSource.data([]);
    $("#ProveedorEnvio").data("kendoComboBox").value("");

    $("#TractoEnvio").data("kendoComboBox").dataSource.data([]);
    $("#TractoEnvio").data("kendoComboBox").value("");

    $("#ChoferEnvio").data("kendoComboBox").dataSource.data([]);
    $("#ChoferEnvio").data("kendoComboBox").value("");    
    opcionHabilitarView(false, "");
    AjaxCargarProyecto();
}