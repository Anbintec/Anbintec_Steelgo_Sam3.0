var ProyectoInicial = 0;
var EmbarqueInicial = 0;
var windowDownloadSpool;
var hayCambios = 0;
var windowDownloadPaquete;
var Destino = "";
var Cerrado = false;

function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoEmbarque();
    SuscribirEventoBuscar();
    SuscribirEventoCerrar();
    SuscribirEventoChangeRadioTipoListado();
    SuscribirEventoSpoolID();
    SuscribirEventoPaquete();
    SuscribirEventoAgregar();
    SuscribirEventoGuardar();
    SuscribirEventoMostrarDetalle();
    SuscribirEventoPopUpDescarga();
    SuscribirEventoEnterCodigo();
    SuscribirEventoDescargarPaquete();
}

function SuscribirEventoDescargarPaquete() {
    $(document).on('click', '.descargarPaquete', function (e) {                
        if (!$("#InputCerrar").is(":checked")) { //Si la revision esta abierta, permite eliminar
            if ($('#Guardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
                var grid = $("#grid").data("kendoGrid");
                var dataItem = grid.dataItem($(e.currentTarget).closest("tr"));
                if (dataItem.Paquete !== "NA" && dataItem.CapturaManual) {
                    windowDownloadPaquete = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.DetalleAvisoLlegada0047[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        animation: false,
                        width: "auto",
                        height: "auto",
                        actions: [],
                        modal: true,
                        position: {
                            'left': (($(window).width() / 2) - ($(this).width() / 2) - 50) + 'px',
                            'top': (($(window).height() / 2) - ($(this).height() / 2)) + 'px'
                        }
                    }).data("kendoWindow");
                    windowDownloadPaquete.content(_dictionary.EmbarqueRevisionMsjConfirmaEliminaPaquete[$("#language").data("kendoDropDownList").value()] + "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");
                    //windowDownloadPaquete.open().center();                        
                    windowDownloadPaquete.open();
                    $("#yesButtonProy").click(function (e) {                        
                        var Accion = dataItem.Accion;
                        var CapturaManual = dataItem.CapturaManual;                        
                        if (Accion == 2 && CapturaManual) { // Ya esan guardados en bd pero son manuales 
                            if (dataItem.Paquete != "NA" && dataItem.Paquete != "") {
                                EliminarPaquete(dataItem, 1); //El paquete ya esta guardado en la BD
                            }                            
                        } else if (Accion == 1 && CapturaManual) {
                            if (dataItem.Paquete != "NA" && dataItem.Paquete != "") {
                                EliminarPaquete(dataItem, 0); //El paquete solamente se agregó al grid pero no se ha guardado
                            }
                        }                        
                        windowDownloadPaquete.close();
                    });
                    $("#noButtonProy").click(function (e) {
                        windowDownloadPaquete.close();
                    });
                } else {
                    e.preventDefault();
                }
            }
        } else {
            displayNotify("EmbarqueRevisionMsjRevisionCerrada", "", "1");
        }
    });    
}

function SuscribirEventoPopUpDescarga() {
    windowDownloadSpool = $("#windowDownloadSpool").kendoWindow({
        iframe: true,
        title: "",
        visible: false,
        width: "35%",
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

    $("#btnDescargarSpool").click(function (e) {
        var tipoDescarga = $("#inputTipoDescarga").val();
        if(tipoDescarga == "1"){
            AjaxDescargarSpool();
        } else if (tipoDescarga == "2") {
            //AjaxDescargarPaquete();
        }
    });
    $("#btnCancelarDescarga").click(function (e) {
        $("#spanDescargaSpool").text("");
        $("#inputTipoDescarga").val("");
        jsonSpoolAgregar = "";
        arrayPaqueteAgregar = [];
        windowDownloadSpool.close();
    });
}

function SuscribirEventoProyecto() {
    $("#Proyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource._data;

            if (!existenCambios(ds)) {
                LimpiarListaProyecto();
                if (dataItem != undefined) {
                    ProyectoInicial = dataItem.ProyectoID;
                    if (dataItem.ProyectoID != 0) {
                        AjaxCargarEmbarques(dataItem.ProyectoID);
                        AjaxCargarPaquetes(dataItem.ProyectoID);                        
                    }
                }
                else {
                    $("#Proyecto").data("kendoComboBox").value("");
                }
            } else {
                ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
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
                    LimpiarListaProyecto();
                    if (dataItem != undefined) {
                        ProyectoInicial = dataItem.ProyectoID;
                        if (dataItem.ProyectoID != 0)
                            AjaxCargarEmbarques(dataItem.ProyectoID);

                    }
                    else {
                        $("#Proyecto").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#Proyecto").data("kendoComboBox").value(ProyectoInicial);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoEmbarque() {
    $('#Embarque').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "EmbarqueID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var ds = $("#grid").data("kendoGrid").dataSource._data;
             
            if (!existenCambios(ds)) {
                LimpiarListaEmbarque();
                if (dataItem != undefined) {
                    EmbarqueInicial = dataItem.EmbarqueID;
                    if (dataItem.EmbarqueID != 0) {
                        Destino = dataItem.Destino;
                        Cerrado = dataItem.RevisionCerrada ? true : false;
                    }                    
                }
                else {
                    $("#Embarque").data("kendoComboBox").value("");
                }
            } else {
                ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
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
                    LimpiarListaEmbarque();
                    if (dataItem != undefined) {
                        EmbarqueInicial = dataItem.EmbarqueID;
                        if (dataItem.EmbarqueID != 0)
                            $("#EmbarqueDestino").text(dataItem.Destino);
                    }
                    else {
                        $("#Embarque").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#Embarque").data("kendoComboBox").value(EmbarqueInicial);
                    ventanaConfirm.close();
                });
            }
        }
    });

    $('#Embarque').closest('.k-widget').keydown(function (e) {
        var Embarque = $("#Embarque").data("kendoComboBox").dataItem($("#Embarque").data("kendoComboBox").select());
        if (e.keyCode == 13) {

            var ds = $("#grid").data("kendoGrid").dataSource._data;
            if (!existenCambios(ds)) {
                LimpiarListaEmbarque();
                if (Embarque != undefined && Embarque.EmbarqueID != 0) {
                    EmbarqueInicial = Embarque.EmbarqueID;
                    $("#EmbarqueDestino").text(Embarque.Destino);
                    if (Embarque.RevisionCerrada)
                        $("#InputCerrar")[0].checked = true;
                    else
                        $("#InputCerrar")[0].checked = false;

                    AjaxObtieneDetalle(Embarque.EmbarqueID);
                } else {
                    displayNotify("MensajeSeleccionaEmbarque", "", '1');
                    $("#Embarque").data("kendoComboBox").value("");
                }
            } else {
                ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
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
                    LimpiarListaEmbarque();
                    if (Embarque != undefined && Embarque.EmbarqueID != 0) {
                        EmbarqueInicial = Embarque.EmbarqueID;
                        AjaxObtieneDetalle(Embarque.EmbarqueID);
                    } else {
                        displayNotify("MensajeSeleccionaEmbarque", "", '1');
                        $("#Embarque").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#Embarque").data("kendoComboBox").value(EmbarqueInicial);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoChangeRadioTipoListado() {
    $('input:radio[name=RevisionEmbarqueTipoSeleccion]:nth(0)').change(function () {
        $("#divSpool").show();
        $("#divPaquete").hide();
        $("#divCodigo").hide();
    });
    $('input:radio[name=RevisionEmbarqueTipoSeleccion]:nth(1)').change(function () {
        $("#divSpool").hide();
        $("#divPaquete").show();
        $("#divCodigo").hide();

    });
    $('input:radio[name=RevisionEmbarqueTipoSeleccion]:nth(2)').change(function () {
        $("#divSpool").hide();
        $("#divPaquete").hide();
        $("#divCodigo").show();

    });
}

function ObtenerTipoConsulta() {
    var radioButtonsLLena = document.getElementsByName('RevisionEmbarqueTipoSeleccion');
    for (var x = 0; x < radioButtonsLLena.length; x++) {
        if (radioButtonsLLena[x].checked) {
            return (x + 1);
        }
    }
    return -1;
}


function SuscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        if ($('#Guardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource._data;
            var proyectoID = $("#Proyecto").data("kendoComboBox").value();
            var embarqueID = $("#Embarque").data("kendoComboBox").value();

            if (proyectoID != "" && proyectoID != undefined && proyectoID != "0") {
                if (embarqueID != "" && embarqueID != undefined && embarqueID != "0") {
                    if (ds.length > 0) {
                        if (validarCamposVaciosRevisionEmbarque(ds)) {
                            embarqueID = parseInt(embarqueID);
                            AjaxGuardarCaptura(ds, embarqueID, proyectoID, 1);
                            hayCambios = 0;
                        } else {
                            displayNotify("EmbarqueRevisionMsjErrorNoPuedeGuardarGridSinCaptura", "", "1");
                            $("#InputCerrar").prop("checked", false);
                        }
                    } else {
                        displayNotify("MensajeAdverteciaExcepcionGuardado", "", "2");
                    }
                } else
                    displayNotify('MensajeSeleccionaEmbarque', '', '1');
            } else
                displayNotify('MensajeSeleccionaProyecto', '', '1');
            
        } else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "");
        }       

    });

    $('.accionGuardarNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource._data;
        var proyectoID = $("#Proyecto").data("kendoComboBox").value();
        var embarqueID = $("#Embarque").data("kendoComboBox").value();

        if (proyectoID != "" && proyectoID != undefined && proyectoID != "0") {
            if (embarqueID != "" && embarqueID != undefined && embarqueID != "0") {
                if (ds.length > 0) {
                    embarqueID = parseInt(embarqueID);
                    AjaxGuardarCaptura(ds, embarqueID, proyectoID, 2);
                    hayCambios = 0;                    
                } else {
                    displayNotify("MensajeAdverteciaExcepcionGuardado", "", "2");
                }
            }else
                displayNotify('MensajeSeleccionaEmbarque', '', '1');
        }else
            displayNotify('MensajeSeleccionaProyecto', '', '1');
    });

};

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('.linea').find('*').attr('disabled', true);        
        $("#Proyecto").data("kendoComboBox").enable(false);
        $("#Embarque").data("kendoComboBox").enable(false);
        $("#inputPaquete").data("kendoComboBox").enable(false);
        $("#InputID").data("kendoComboBox").enable(false);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('.linea').find('*').attr('disabled', false);        
        $("#Proyecto").data("kendoComboBox").enable(true);
        $("#Embarque").data("kendoComboBox").enable(true);
        $("#inputPaquete").data("kendoComboBox").enable(true);
        $("#InputID").data("kendoComboBox").enable(true);

        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function SuscribirEventoBuscar() {
    $('#btnBuscar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource._data;
        if (!existenCambios(ds)) {
            if ($("#Proyecto").data("kendoComboBox").text() != "") {
                if ($("#Embarque").data("kendoComboBox").text() != "") {
                    AjaxObtieneDetalle($("#Embarque").data("kendoComboBox").value());                    
                    $("#EmbarqueDestino").text(Destino);
                    $("#InputCerrar")[0].checked = Cerrado;                    
                }
                else {
                    displayNotify('MensajeSeleccionaEmbarque', '', '1');
                }
            } else {
                displayNotify("EmbarqueCargaMensajeErrorProyecto", "", "1");
            }
            
        } else {
            ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
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
                if ($("#Embarque").data("kendoComboBox").text() != "") {
                    AjaxObtieneDetalle($("#Embarque").data("kendoComboBox").value());
                }
                else {
                    displayNotify('MensajeSeleccionaEmbarque', '', '1');
                }
                ventanaConfirm.close();
            });
            $("#noButtonProy").click(function () {
                ventanaConfirm.close();
            });
        }        
    });
}

function SuscribirEventoCerrar() {
    $("#InputCerrar").change(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource._data;
        if (ds.length > 0) {
            if ($(this).is(":checked")) {

            }
        } else {
            $(this)[0].checked = false;
        }

    });
}

function SuscribirEventoEnterCodigo() {
    $("#inputCodigo").keydown(function (e) {
        if (e.keyCode == 13) {
            var Proyecto = $("#Proyecto").data("kendoComboBox").dataItem($("#Proyecto").data("kendoComboBox").select());
            var Embarque = $("#Embarque").data("kendoComboBox").dataItem($("#Embarque").data("kendoComboBox").select());
            var RevisionCerrado = $("#InputCerrar").is(":checked");
        
            if (Proyecto != undefined && Proyecto.ProyectoID != 0) {
                if (Embarque != undefined && Embarque.EmbarqueID != 0) {
                    if (!RevisionCerrado) {                        
                        var TipoConsulta = ObtenerTipoConsulta();
                        var codigo = "";
                        var ordenTrabajoSpoolID = 0;
                        var paqueteID = 0;

                        if (TipoConsulta == 1) {
                            ordenTrabajoSpoolID = $("#InputID").data("kendoComboBox").value();

                            if (ordenTrabajoSpoolID != "" && ordenTrabajoSpoolID != undefined && ordenTrabajoSpoolID != "0") {
                                AjaxAgregarDetalleSpool(TipoConsulta, ordenTrabajoSpoolID, codigo);
                            }

                        }else if(TipoConsulta == 2){
                            codigo = $("#inputCodigo").val();

                            if (codigo != "" && codigo != undefined) {                                
                                AjaxAgregarDetalleSpool(TipoConsulta, ordenTrabajoSpoolID, codigo.toString().trim().toUpperCase());
                            }
                        }
                        else if (TipoConsulta == 3) {
                            paqueteID = $("#inputPaquete").data("kendoComboBox").value();

                            if (paqueteID != "" && paqueteID != undefined && paqueteID != "0") {
                                paqueteID = parseInt(paqueteID);
                                AjaxAgregarDetallePaquete(paqueteID);
                            }
                        } else {

                        }
                    } else {
                        displayNotify('EmbarqueRevisionMsjRevisionCerrada', '', '1');
                    }
                }else
                    displayNotify('MensajeSeleccionaEmbarque', '', '1');
            } else {
                displayNotify('MensajeSeleccionaProyecto', '', '1');
            }                
        }
    });    
}

function SuscribirEventoAgregar() {
    $('#btnAgregar').click(function (e) {
        var Proyecto = $("#Proyecto").data("kendoComboBox").dataItem($("#Proyecto").data("kendoComboBox").select());
        var Embarque = $("#Embarque").data("kendoComboBox").dataItem($("#Embarque").data("kendoComboBox").select());
        var RevisionCerrado = $("#InputCerrar").is(":checked");
        
        if (Proyecto != undefined && Proyecto.ProyectoID != 0) {
            if (Embarque != undefined && Embarque.EmbarqueID != 0) {
                if (!RevisionCerrado) {
                    var TipoConsulta = ObtenerTipoConsulta();
                    var codigo = "";
                    var ordenTrabajoSpoolID = 0;
                    var paqueteID = 0;

                    if (TipoConsulta == 1) {
                        ordenTrabajoSpoolID = $("#InputID").data("kendoComboBox").value();

                        if (ordenTrabajoSpoolID != "" && ordenTrabajoSpoolID != undefined && ordenTrabajoSpoolID != "0") {
                            AjaxAgregarDetalleSpool(TipoConsulta, ordenTrabajoSpoolID, codigo);
                        }

                    }else if(TipoConsulta == 2){
                        codigo = $("#inputCodigo").val();

                        if (codigo != "" && codigo != undefined) {
                            AjaxAgregarDetalleSpool(TipoConsulta, ordenTrabajoSpoolID, codigo.toString().trim().toUpperCase());
                        }
                    }
                    else if (TipoConsulta == 3) {
                        paqueteID = $("#inputPaquete").data("kendoComboBox").value();

                        if (paqueteID != "" && paqueteID != undefined && paqueteID != "0") {
                            paqueteID = parseInt(paqueteID);
                            AjaxAgregarDetallePaquete(paqueteID);
                        }
                    } else {

                    }
                } else {
                    displayNotify('EmbarqueRevisionMsjRevisionCerrada', '', '1');
                }
            }else
                displayNotify('MensajeSeleccionaEmbarque', '', '1');
        }else
            displayNotify('MensajeSeleccionaProyecto', '', '1');
    });
}

function SuscribirEventoPaquete() {
    $("#inputPaquete").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "PaqueteID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
            }
            else {
                $("#inputPaquete").data("kendoComboBox").value("");
            }
        }
    });
}


function SuscribirEventoSpoolID() {

    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        suggest: true,
        filter: "contains",
        index: 3,
        select: function (e) {

            var dataItem = this.dataItem(e.item.index());

            if (dataItem.Status != "1") {
                e.preventDefault();
                $("#InputID").val("");
                displayNotify("Mensajes_error", dataItem.Status, '1');

            }
            else {
                $("#InputID").val(dataItem.IDValido);
                Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                $("#LabelProyecto").text(dataItem.Proyecto);
            }

        }
        ,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if ($("#InputID").val().length == 1) {
                $("#InputID").data("kendoComboBox").value(("00" + $("#InputID").val()).slice(-3));
            }
            if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
                Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                $("#LabelProyecto").text(dataItem.Proyecto);

            }
        }
    });

    $("#InputOrdenTrabajo").blur(function (e) {

        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                AjaxObtenerSpoolID();
            } catch (e) {
                displayNotify("Mensajes_error", e.message, '2');
            }
        } else {
            displayNotify("MensajeOrdenTrabajoNoValida", "", '1');
        }
    });

    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").dataSource.data([]);
        $("#InputID").data("kendoComboBox").value("");
    });

    $('#InputID').closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 37) {
            $("#InputOrdenTrabajo").focus();

        }
        else if (e.keyCode == 40) {
            $("#InputID").data("kendoComboBox").select();
        }
        else if (e.keyCode == 13) {
            if (!$("#InputCerrar").is(":checked")) {

                var ordenTrabajoSpoolID = $('#InputID').data("kendoComboBox").value();
                if (ordenTrabajoSpoolID != "" && ordenTrabajoSpoolID != undefined && ordenTrabajoSpoolID != "0") {
                    AjaxAgregarDetalleSpool(1, ordenTrabajoSpoolID, "");
                }
            } else
                displayNotify('EmbarqueRevisionMsjRevisionCerrada', '', '1');
        }
    });

};

function SuscribirEventoMostrarDetalle() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        var ds = $("#grid").data("kendoGrid").dataSource._data;
        if (existenCambios(ds)) {
            ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
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
                $("#grid").data("kendoGrid").dataSource.data([]);
                ventanaConfirm.close();
            });
            $("#noButtonProy").click(function () {
                $('input[name=Muestra]:nth(1)').prop('checked', true);
                ventanaConfirm.close();                
            });
        } else {
            $("#grid").data("kendoGrid").dataSource.data([]);
        }       
    });

    $('input:radio[name=Muestra]:nth(1)').change(function () {                
        var ds = $("#grid").data("kendoGrid").dataSource._data;
        if (existenCambios(ds)) {
            ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
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
                $("#grid").data("kendoGrid").dataSource.data([]);
                ventanaConfirm.close();
            });
            $("#noButtonProy").click(function () {
                $('input[name=Muestra]:nth(0)').prop('checked', true);
                ventanaConfirm.close();
            });
        } else {
            $("#grid").data("kendoGrid").dataSource.data([]);
        }
    });
}

function ObtenerTipoConsulta() {
    var tipoConsulta = $("input:radio[name=RevisionEmbarqueTipoSeleccion]:checked").val();
    var valor = 0;
    if(tipoConsulta == "Spool"){
        valor = 1;
    } else if (tipoConsulta == "Codigo") {
        valor = 2;
    } else if (tipoConsulta == "Paquete") {
        valor = 3
    }
   
    return valor;
}

function LimpiarListaProyecto() {
    $("#Embarque").data("kendoComboBox").dataSource.data([]);
    $("#Embarque").data("kendoComboBox").value("");
    $("#InputCerrar")[0].checked = false;
    $("#EmbarqueDestino").text("");

    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#InputID").data("kendoComboBox").value("");
    $("#inputCodigo").val("");
    $("#inputPaquete").data("kendoComboBox").dataSource.data([]);
    $("#inputPaquete").data("kendoComboBox").value("");


    $("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiarListaEmbarque() {

    $("#InputCerrar")[0].checked = false;
    $("#EmbarqueDestino").text("");
    $("#InputOrdenTrabajo").val("");
    $("#inputPaquete").data("kendoComboBox").value("");
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#InputID").data("kendoComboBox").value("");
    $("#grid").data("kendoGrid").dataSource.data([]);
}

function Limpiar() {
    $("#Proyecto").data("kendoComboBox").dataSource.data([]);
    $("#Proyecto").data("kendoComboBox").value("");

    $("#Embarque").data("kendoComboBox").dataSource.data([]);
    $("#Embarque").data("kendoComboBox").value("");

    $("#InputCerrar")[0].checked = false;
    $("#EmbarqueDestino").text("");

    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").dataSource.data([]);
    $("#InputID").data("kendoComboBox").value("");
    $("#inputPaquete").data("kendoComboBox").dataSource.data([]);
    $("#inputPaquete").data("kendoComboBox").value("");
    $("#inputCodigo").val("");


    $("#grid").data("kendoGrid").dataSource.data([]);

    AjaxCargarCamposPredeterminados();
    AjaxCargarProyecto();
}
function validarCamposVaciosRevisionEmbarque(Lista) {        
    var grid = $("#grid").data("kendoGrid");            
    var Ejecutar = true;
    if ($("#InputCerrar").is(":checked")) {
        for (var i = 0; i < Lista.length; i++) {
            var currentUid = Lista[i].uid;
            if (Lista[i].Llego || (Lista[i].LlegoComentario && Lista[i].Comentario !== null) || Lista[i].NoLlego) {
                if (grid.table.find("tr[data-uid='" + currentUid + "']").hasClass("kRowError")) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("k-alt");
                }
                Lista[i].RowOk = true;
                continue;
            } else {                                
                grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
                Lista[i].RowOk = false;
                Ejecutar = false;
            }
        }
    }            
    return Ejecutar;
}