var windowSend;

function SuscribirEventos() {
    SuscribirEventoTab();
    SuscribirEventoEnviarEmbarque();
    SuscribirEventoImprimir();
    SuscribirEventoGuardar();
    SuscribirEventoPopUPEnviarEmbarque();
}

function SuscribirEventoTab() {
    $("#btnPendientes").click(function (e) {
        $(".btn-tabList").removeClass("active");
        $("#btnPendientes").addClass("active");
        AjaxObtenerDetalleListadoEmbarque(1);
    });
    $("#btnTransito").click(function (e) {
        $(".btn-tabList").removeClass("active");
        $("#btnTransito").addClass("active");
        AjaxObtenerDetalleListadoEmbarque(2);
    });
}


function SuscribirEventoPopUPEnviarEmbarque() {
    
    windowSend = $("#windowSend").kendoWindow({
            modal: true,
            title: "",
            resizable: false,
            visible: false,
            width: "45%",
            actions: [ //"Pin", "Minimize", "Maximize", "Close"
            ],
    }).data("kendoWindow");

    $("#btnEnviarEmbarque").click(function (e) {
        var NumEmb = $("#NumEmb").val();
        var NumEmbCliente = $("#NumEmbCliente").val();
        var FechaEnvio = $("#Fecha").val();
        var uid = $("#uidRow").val();
        var dataItem = $('#grid').data("kendoGrid").dataSource.getByUid(uid);
        var FolioPermiso = dataItem.RequierePermisoAduana ? dataItem.FolioSolicitudPermiso != "" && dataItem.FolioSolicitudPermiso != null ? true : false : true;
        var FechaPermiso = dataItem.RequierePermisoAduana ? dataItem.FechaSolicitudPermiso != "" && dataItem.FechaSolicitudPermiso != null ? true : false : true;

        if (NumEmb != "") {
            if (NumEmbCliente != "") {
                if (FechaEnvio != "") {
                    if (dataItem.DestinoID != 0) {
                        if (FolioPermiso) {
                            if (FechaPermiso) {
                                windowSend.close();
                                AjaxEnviarEmbarque(dataItem, NumEmb, NumEmbCliente, FechaEnvio);
                            } else
                                displayNotify("EmbarqueListadoMsjErrorEnviarFechaPermisoNoCapturado", "", "2");
                        } else
                            displayNotify("EmbarqueListadoMsjErrorEnviarSolicitudPermisoNoCapturado", "", "2");
                    } else
                        displayNotify("EmbarqueListadoMsjErrorEnviarDestinoNoCapturado", "", "2");
                } else
                    displayNotify("EmbarqueListadoMsjErrorFechaEnvio", "", "2");
            } else
                displayNotify("EmbarqueListadoMsjErrorNumEmbarqueCliente", "", "2");
        } else {
            displayNotify("EmbarqueListadoMsjErrorNumEmbarqueSteelgo", "", "2");
        }        
    });

    $("#btnCerrarPopUp").click(function (e) {
        var fecha = kendo.toString(new Date(), String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", ""))).trim();        
        $("#NumEmb").val("");
        $("#NumEmbCliente").val("");
        $("#Fecha").val(fecha);
        windowSend.close();
    })
}

function SuscribirEventoEnviarEmbarque() {
    $(document).on('click', '.enviarEmbarque', function (e) {
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            var dataItem = grid.dataItem($(e.target).closest("tr"));
            $("#uidRow").val(dataItem.uid);
            windowSend.title(_dictionary.EmbarqueListadoTitlePopUpEnviar[$("#language").data("kendoDropDownList").value()]);
            windowSend.center().open();
        }
    });
}

function SuscribirEventoImprimir() {
    $(document).on('click', '.imprimirPapelesCliente', function (e) {
        //AjaxImprimir();
    });

    $(document).on('click', '.imprimirPapelesAduana', function (e) {
        //AjaxImprimir();
    });
}

function isDate(string) { //string estará en formato dd/mm/yyyy (dí­as < 32 y meses < 13)
    var expReg;
    if ($("#language").val() == "es-MX") {
        expReg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
    }
    else {
        expReg = /([1-9]|1[012])[- /.]([1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d/;
    }

    var res = expReg.test(string);
    return res;
}

function SuscribirEventoGuardar() {
    $(".accionGuardar").click(function () {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($("#btnPendientes").hasClass("active")) {
            if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                if (ds._data.length > 0) {
                    AjaxGuardarCaptura(ds._data, 1);
                } else {
                    displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
                }
            } else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()])
                opcionHabilitarView(false, "");
        }
           
    });
}

function opcionHabilitarView(valor, name) {
    if (valor){
        $("#btnPendientes").attr('disabled', true);
        $("#btnTransito").attr('disabled', true);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $("#btnPendientes").attr('disabled', false);
        $("#btnTransito").attr('disabled', false);

        //$('#FieldSetView').find('*').attr('disabled', false);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#grid").data("kendoGrid").dataSource.sync();
    }
        
}