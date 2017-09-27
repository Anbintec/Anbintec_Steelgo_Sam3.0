function SuscribirEventos() {
    SuscribirEventoRequisicion();
    SuscribirEventoProyecto();
    SuscribirEventoTipoPrueba();    
    suscribirOrdenTrabajo();
    SuscribirEventoSpoolID();
    SuscribirEventoJuntaSpool();
    suscribirEventoChangeRadio();
    SuscribirEventoGuardar();
    SuscribirEventoEliminar();
    suscribirEventoAgregar();
    
}


function suscribirEventoAgregar() {
    $('#ButtonAgregar').click(function (e) {        
        if ($("#Junta").data("kendoComboBox").value() != "") {
            AgregarJuntaNueva();
        } else {
            displayNotify("JuntaSinSeleccionar", "", "1");
        }

    });
}

function suscribirOrdenTrabajo() {
    $("#InputOrdenTrabajo").blur(function (e) {
        
        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                AjaxObtenerSpoolID();
            } catch (e) {
                displayNotify("", e.message, '2');
            }
        } else {
            displayNotify("MensajeOrdenTrabajoNoValida", "", '1');
        }
    });

    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").text("");
        $("#Junta").data("kendoComboBox").text("");
        $("#Junta").data("kendoComboBox").setDataSource();
        $("#InputID").data("kendoComboBox").setDataSource();
    });



};

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
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        if (dataItem.ProyectoID != 0) {
                            AjaxCargaListaTipoPrueba();
                        } else {
                            $("#inputRequisicion").data("kendoComboBox").value("");
                            $("#inputTipoPrueba").data("kendoComboBox").value("");
                        }
                    }
                } else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "auto",
                        height: "auto",
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
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();

                    $("#yesButtonProy").click(function () {
                        $("#inputProyecto").attr("proyectoAntrior", dataItem.ProyectoID);

                        if (paramReq == null) {
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            if (dataItem.ProyectoID != 0) {
                                AjaxCargaListaTipoPrueba();
                            } else {
                                $("#inputRequisicion").data("kendoComboBox").value("");
                                $("#inputTipoPrueba").data("kendoComboBox").value("");
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
                if(paramReq==null){
                    $("#inputRequisicion").data("kendoComboBox").value("");
                    $("#inputTipoPrueba").data("kendoComboBox").value("");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                }
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
            var proyectoID = $("#inputProyecto").data("kendoComboBox").value();

            if (dataItem != undefined) {
                if (!validaInformacionCapturada()) {
                    $("#inputTipoPrueba").attr("tipoPruebaAntrior", dataItem.TipoPruebaID);
                    if (paramReq == null) {
                        AjaxCargaListaRequisicion(dataItem.TipoPruebaID, proyectoID);
                    }
                } else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "auto",
                        height: "auto",
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
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                    ventanaConfirm.open().center();

                    $("#yesButtonProy").click(function () {
                        $("#inputTipoPrueba").attr("tipoPruebaAntrior", dataItem.TipoPruebaID);
                        if (paramReq == null) {
                            AjaxCargaListaRequisicion(dataItem.TipoPruebaID, proyectoID);
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonProy").click(function () {
                        $("#inputTipoPrueba").data("kendoComboBox").value($("#inputTipoPrueba").attr("tipoPruebaAntrior"));
                        ventanaConfirm.close();
                    });
                }
            } else {
                $("#inputTipoPrueba").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").value("");
                if(paramReq!=null){
                    $("#inputProyecto").data("kendoComboBox").value("");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                } 
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
                        if (dataItem.RequisicionID != 0) {
                            AjaxCargaDetalleRequisicion(dataItem.RequisicionID, dataItem.TipoPruebaID, dataItem.ProyectoID, 1);
                            if ($("#inputTipoPrueba").data("kendoComboBox").value() == 0 || $("#inputTipoPrueba").data("kendoComboBox").value() == "") {
                                $("#inputTipoPrueba").data("kendoComboBox").value(dataItem.TipoPruebaID)
                            }
                            ocultaDivAgregaSpool(true);
                        } else {
                            ocultaDivAgregaSpool(false);
                            $("#grid").data("kendoGrid").dataSource.data([]);
                        }
                    } else {
                        if (dataItem.RequisicionID != 0) {

                            $("#inputProyecto").data("kendoComboBox").value(dataItem.ProyectoID);
                            $("#inputTipoPrueba").data("kendoComboBox").value(dataItem.TipoPruebaID);

                            AjaxCargaDetalleRequisicion(dataItem.RequisicionID, dataItem.TipoPruebaID, dataItem.ProyectoID, 1);
                            ocultaDivAgregaSpool(true);
                        } else {
                            ocultaDivAgregaSpool(false);
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            $("#inputProyecto").data("kendoComboBox").value("");
                            $("#inputTipoPrueba").data("kendoComboBox").value("");
                        }
                    }
                } else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "auto",
                        height: "auto",
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
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                    ventanaConfirm.open().center();

                    $("#yesButtonProy").click(function () {
                        $("#inputRequisicion").attr("requisicionAntrior", dataItem.RequisicionID);
                        if (paramReq == null) {
                            if (dataItem.RequisicionID != 0) {
                                AjaxCargaDetalleRequisicion(dataItem.RequisicionID, dataItem.TipoPruebaID, dataItem.ProyectoID, 1);
                                if ($("#inputTipoPrueba").data("kendoComboBox").value() == 0 || $("#inputTipoPrueba").data("kendoComboBox").value() == "") {
                                    $("#inputTipoPrueba").data("kendoComboBox").value(dataItem.TipoPruebaID)
                                }
                                ocultaDivAgregaSpool(true);
                            } else {
                                ocultaDivAgregaSpool(false);
                                $("#grid").data("kendoGrid").dataSource.data([]);
                            }
                        } else {
                            if (dataItem.RequisicionID != 0) {

                                $("#inputProyecto").data("kendoComboBox").value(dataItem.ProyectoID);
                                $("#inputTipoPrueba").data("kendoComboBox").value(dataItem.TipoPruebaID);

                                AjaxCargaDetalleRequisicion(dataItem.RequisicionID, dataItem.TipoPruebaID, dataItem.ProyectoID, 1);
                                ocultaDivAgregaSpool(true);
                            } else {
                                ocultaDivAgregaSpool(false);
                                $("#grid").data("kendoGrid").dataSource.data([]);
                                $("#inputProyecto").data("kendoComboBox").value("");
                                $("#inputTipoPrueba").data("kendoComboBox").value("");
                            }
                        }
                        ventanaConfirm.close();
                    });
                    $("#noButtonProy").click(function () {
                        $("#inputRequisicion").data("kendoComboBox").value($("#inputRequisicion").attr("requisicionAntrior"));
                        ventanaConfirm.close();
                    });
                }
            } else {
                $("#inputRequisicion").data("kendoComboBox").value("");
                if (paramReq != null) {
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    $("#inputProyecto").data("kendoComboBox").value("");
                    $("#inputTipoPrueba").data("kendoComboBox").value("");
                }
                ocultaDivAgregaSpool(true);
            }
        }
    });
}

function SuscribirEventoSpoolID() {
    var dataItem;
    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        delay: 10,
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if ($("#InputID").val().length == 1) {
                    $("#InputID").data("kendoComboBox").value(("00" + $("#InputID").val()).slice(-3));
                }
                if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
                    Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                    $("#LabelProyecto").text(dataItem.Proyecto);
                    AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
                }
            }
        }
    });



    $('#InputID').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 37) {
            $("#InputOrdenTrabajo").focus();
        }
        else if (e.keyCode == 39) {
            $("#Junta").data("kendoComboBox").input.focus();
        }
        else if (e.keyCode == 40 && $("#InputID").data("kendoComboBox").select() != -1) {
            $("#InputID").data("kendoComboBox").select();
            AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
        }
        else if (e.keyCode == 13) {
            if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select(0)) != undefined) {
                if ($("#InputID").data("kendoComboBox").select() != -1) {
                    AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
                }
            }
            else
                displayNotify("NoExisteSpoolID", '', '2');
        }
        else if (e.keyCode == 9) {
            if (tieneClase(e.currentTarget)) {
                $("#InputID").data("kendoComboBox").select(0);
                AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
            }
            dataItem = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select());
            if (dataItem != undefined) {
                if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
                    Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                    $("#LabelProyecto").text(dataItem.Proyecto);
                    AjaxJunta($("#InputID").val());
                }
            }
        }
    });
};

function SuscribirEventoJuntaSpool() {
    $("#Junta").kendoComboBox({
        dataTextField: "Etiqueta",
        dataValueField: "JuntaSpoolID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3
    });

    $('#Junta').closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 37) {
            $("#InputID").data("kendoComboBox").input.focus();
            $("#Junta").val("");
        }
        else if (e.keyCode == 39) {
            $("#ButtonAgregar").focus();
        }
        else if (e.keyCode == 13) {
            if ($("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()) != undefined) {

                AgregarJuntaNueva();
            }
            else
                $("#Junta").data("kendoComboBox").value("");
        }
    });
}


function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        if ($("#inputProyecto").data("kendoComboBox").value() != "" && $("#inputProyecto").data("kendoComboBox").value() != 0) {
            var tipoPrueba = $("#inputTipoPrueba").data("kendoComboBox").value() == "" ? 0 : $("#inputTipoPrueba").data("kendoComboBox").value();
            var RequisicionID = $("#inputRequisicion").data("kendoComboBox").value() == "" ? 0 : $("#inputRequisicion").data("kendoComboBox").value();
            var ProyectoID = $("#inputProyecto").data("kendoComboBox").value() == "" ? 0 : $("#inputProyecto").data("kendoComboBox").value();

            var ds = $("#grid").data("kendoGrid").dataSource;
            if (!existenCambios(ds._data)) {
                AjaxCargaDetalleRequisicion(RequisicionID, tipoPrueba, ProyectoID, 1);
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    close: function () {
                        $('input[name="Muestra"][value="Todos"]').prop('checked', true);
                    }
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    ventanaConfirm.close();
                    AjaxCargaDetalleRequisicion(RequisicionID, tipoPrueba, ProyectoID, 1);
                });

                $("#noButtonProy").click(function () {
                    $('input[name="Muestra"][value="Todos"]').prop('checked', true);
                    ventanaConfirm.close();
                });
            }
        }
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        if ($("#inputProyecto").data("kendoComboBox").value() != "" && $("#inputProyecto").data("kendoComboBox").value() != 0) {
            var tipoPrueba = $("#inputTipoPrueba").data("kendoComboBox").value() == "" ? 0 : $("#inputTipoPrueba").data("kendoComboBox").value();
            var RequisicionID = $("#inputRequisicion").data("kendoComboBox").value() == "" ? 0 : $("#inputRequisicion").data("kendoComboBox").value();
            var ProyectoID = $("#inputProyecto").data("kendoComboBox").value() == "" ? 0 : $("#inputProyecto").data("kendoComboBox").value();

            var ds = $("#grid").data("kendoGrid").dataSource;
            if (!existenCambios(ds._data)) {
                AjaxCargaDetalleRequisicion(RequisicionID, tipoPrueba, ProyectoID, 1);
            }
            else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    close: function () {
                        $('input[name="Muestra"][value="SinCaptura"]').prop('checked', true);
                    }
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    AjaxCargaDetalleRequisicion(RequisicionID, tipoPrueba, ProyectoID, 1);
                    ventanaConfirm.close();
                });

                $("#noButtonProy").click(function () {
                    $('input[name="Muestra"][value="SinCaptura"]').prop('checked', true);
                    ventanaConfirm.close();
                });
            }
        }
    });
   
}

function SuscribirEventoGuardar() {
    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        var proyectoID = $("#inputProyecto").data("kendoComboBox").value();
        var tipoPruebaID = $("#inputTipoPrueba").data("kendoComboBox").value();
        var requisicionID = $("#inputRequisicion").data("kendoComboBox").value();

        var ds = $("#grid").data("kendoGrid").dataSource;
        if (proyectoID != 0 && proyectoID != "") {
            if (tipoPruebaID != 0 && tipoPruebaID != "") {
                if (requisicionID != 0 && requisicionID != "") {
                    AjaxGuardaCaptura(ds._data, true);                    
                } else {
                    displayNotify("MensajeSeleccionaRequisicion", "", "1");
                }
            } else {
                displayNotify("MensajeSeleccionaTipoPrueba", "", "1");
            }
        } else {
            displayNotify("MensajeSeleccionaProyecto", "", "1");
        }
    });

    //Guardar
    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {
        var proyectoID = $("#inputProyecto").data("kendoComboBox").value();
        var tipoPruebaID = $("#inputTipoPrueba").data("kendoComboBox").value();
        var requisicionID = $("#inputRequisicion").data("kendoComboBox").value();

        if ($("#Guardar").text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if (proyectoID != 0 && proyectoID != "") {
                if (tipoPruebaID != 0 && tipoPruebaID != "") {
                    if(requisicionID!=0 && requisicionID!=""){
                        AjaxGuardaCaptura(ds._data, false);
                    } else {
                        displayNotify("MensajeSeleccionaRequisicion", "", "1");
                    }
                } else {
                    displayNotify("MensajeSeleccionaTipoPrueba", "", "1");
                }
            } else {
                displayNotify("MensajeSeleccionaProyecto", "", "1");
            }
        } else if ($("#Guardar").text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, '');
        }
    });
}

function SuscribirEventoEliminar() {
    $("#btnEliminaRequiscion, #btnEliminaRequiscion2").click(function (e) {
        var itemRequisicion = $("#inputRequisicion").data("kendoComboBox").dataItem($("#inputRequisicion").data("kendoComboBox").select());
        if (itemRequisicion.RequisicionID != "" && itemRequisicion.RequisicionID != "0") {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
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
            ventanaConfirm.content(_dictionary.EditarRequisicionMensajeConfirmaEliminar[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButtonProy").click(function () {
                AjaxEliminaRequisicion(itemRequisicion);
                ventanaConfirm.close();
            });
            $("#noButtonProy").click(function () {
                
                ventanaConfirm.close();
            });
        } else {
            displayNotify("MensajeSeleccionaRequisicion", "", "1");
        }
    });
}

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('.addedSectionInLine').find('*').attr('disabled', true);
        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputTipoPrueba").data("kendoComboBox").enable(false);
        $("#inputRequisicion").data("kendoComboBox").enable(false);
        $("input[name='Muestra']").attr('disabled', true);

        $("#InputID").data("kendoComboBox").enable(false);
        $("#Junta").data("kendoComboBox").enable(false);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('.addedSectionInLine').find('*').attr('disabled', false);

        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputTipoPrueba").data("kendoComboBox").enable(true);
        $("#inputRequisicion").data("kendoComboBox").enable(true);
        $("input[name='Muestra']").attr('disabled', false);

        $("#InputID").data("kendoComboBox").enable(true);
        $("#Junta").data("kendoComboBox").enable(true);
        $('#botonGuardar').text("Guardar");

        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function Limpiar() {
    var paramReq = getParameterByName('requisicion');

    AjaxCargarCamposPredeterminados();
    if (paramReq != null) {
        AjaxObtenerElementoRequisicion(paramReq);
        ocultaDivAgregaSpool(true);
    } else {
        $("#inputProyecto").data("kendoComboBox").value("");
        $("#inputTipoPrueba").data("kendoComboBox").value("");
        $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
        $("#inputRequisicion").data("kendoComboBox").value("");
        $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);

        AjaxCargaListaProyecto();
        ocultaDivAgregaSpool(false);
        $("#grid").data("kendoGrid").dataSource.data([]);
    }
    opcionHabilitarView(false, '');
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#Junta").data("kendoComboBox").value("");
    $("#Junta").data("kendoComboBox").dataSource.data([]);
}

function ocultaDivAgregaSpool(valor) {
    var TipoPrueba = $("#inputTipoPrueba").data("kendoComboBox").dataItem($("#inputTipoPrueba").data("kendoComboBox").select());
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#Junta").data("kendoComboBox").value("");
    $("#Junta").data("kendoComboBox").dataSource.data([]);
    if (valor) {
        $("#containerDiv").show();
        if (TipoPrueba.TipoPruebaPorSpool==1) {
            $("#divJuntaSpool").hide();
        } else {
            $("#divJuntaSpool").show();
        }
    } else {
        $("#containerDiv").hide();
    }
}

