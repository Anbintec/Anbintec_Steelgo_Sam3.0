var listadoSoldadoresParaRaiz = [];
var listadoSoldadoresParaRelleno = [];
var listadoColada = [];



function SuscribirEventos() {
    suscribirEventoGuardar();
    suscribirEventoAgregar();
    suscribirEventoAplicar();
    suscribirEventoCancelar();
    SuscribirEventoTaller();
    SuscribirEventosJunta();
    SuscribirEventoID();
    SuscribirEventosOrdenTrabajo();
    suscribirEventoChangeRadio();
    suscribirEventoChangeRadioTipoListado();
    suscribirEventoWindowsConfirmaCaptura();
    SuscribirEventoMuestraJunta();


    suscribirEventoGridPopupTrabajosAdicionales();
    suscribirEventoGridPopupSoldadoresRaiz();
    suscribirEventoGridPopupSoldadoresRelleno();

    GuardarDetalleAdicional();
    GuardarSoldadoresRaiz();
    GuardarSoldadoresRelleno();
};



function suscribirEventoGuardar() {
    $('#btnGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                AjaxGuardarCaptura(ds._data, 0);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false)
            }
        }


    });

    $("#Guardar").click(function () {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                AjaxGuardarCaptura(ds._data, 0);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false)
            }
        }
    });


    $('#btnGuardarYNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {

            AjaxGuardarCaptura(ds._data, 1);

        }

    });


    //pie pagina
    $('#GuardarPie').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                AjaxGuardarCaptura(ds._data, 0);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false)
            }
        }
    });

    $("#btnGuardarPie").click(function () {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                AjaxGuardarCaptura(ds._data, 0);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false)
            }
        }
    });


    $('#btnGuardarYNuevoPie').click(function (e) {

        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            AjaxGuardarCaptura(ds._data, 1);
        }

    });
}

function suscribirEventoAgregar() {
    $('#ButtonAgregar').click(function (e) {

        if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
            if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()) != undefined) {
                $('#ButtonAgregar').prop("disabled", true);
                AjaxJuntaModoSpool($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
            }
            else {
                if ($("#InputID").val() == "") {
                    displayNotify("CapturaSoldaduraSpoolNoCapturado", "", '1');
                }
                else
                    displayNotify("CapturaSoldaduraNoExisteSpoolID", "", '1');
            }
        }
        else {
            if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado" && $("#Junta").data("kendoComboBox").text() != "") {
                if ($("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()) != undefined) {
                    $('#ButtonAgregar').prop("disabled", true);
                    ObtenerJSonGridSoldadura();
                }
            }
            else
                desplegarNotificacion();
        }
    });
}

function suscribirEventoAplicar() {
    $('#ButtonAplicar').click(function (e) {

        if ($('input:radio[name=LLena]:checked').val() === "Todos") {


            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
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

            ventanaConfirm.content(_dictionary.CapturaMensajeArmadoPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                if ($("#inputTaller").data("kendoComboBox").dataItem($("#inputTaller").data("kendoComboBox").select()) != undefined)
                    PlanchaTaller();
                //if ($("#inputColada").data("kendoComboBox").dataItem($("#inputColada").data("kendoComboBox").select()) != undefined)
                //    PlanchaColada();
                if (endRangeDate.val() != "")
                    PlanchaFecha();

                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });
        }
        else {
            if ($("#inputTaller").data("kendoComboBox").dataItem($("#inputTaller").data("kendoComboBox").select()) != undefined)
                PlanchaTaller();
            //if ($("#inputColada").data("kendoComboBox").dataItem($("#inputColada").data("kendoComboBox").select()) != undefined)
            //    PlanchaColada();
            if (endRangeDate.val() != "")
                PlanchaFecha();
        }
    });
};

function suscribirEventoCancelar() {
    $('#btnCancelar').click(function (e) {
        Limpiar();
        $('#InputOrdenTrabajo').val('');
        $('#InputID').data('kendoComboBox').text("");
        $('#Junta').data('kendoComboBox').text("");
        //AjaxCargarCamposPredeterminados();
    });
    $('#CancelarSoldadoresRaiz').click(function (e) {
        $("#windowGridSoldadorRaiz").data("kendoWindow").close();
    });
    $('#CancelarSoldadoresRelleno').click(function (e) {
        $("#windowMultiselectSoldadorRelleno").data("kendoWindow").close();
    });
    $('#CancelarTrabajosAdicionales').click(function (e) {
        $("#windowGrid").data("kendoWindow").close();
    });
}

function SuscribirEventoTaller() {
    $('#inputTaller').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TallerID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3
    });

    $('#inputTaller').blur(function (e) {
        dataItem = $("#inputTaller").data("kendoComboBox").dataItem($("#inputTaller").data("kendoComboBox").select());
        if (dataItem == undefined) {
            $("#inputTaller").data("kendoComboBox").value("");
            $("#inputTaller").data("kendoComboBox").select(0);
        }

    });
}

function SuscribirEventosJunta() {
    $('#Junta').kendoComboBox({
        dataTextField: "Etiqueta",
        dataValueField: "JuntaSpoolID",
        suggest: true,
        delay: 10,
        filter: "endswith",
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
                if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
                    if ($("#Junta").data("kendoComboBox").select() != -1) {
                        ObtenerJSonGridSoldadura();
                        $("#Junta").data("kendoComboBox").text("");
                    }
                }
                else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
                    if ($("#Junta").val() != "") {
                        if ($("#Junta").data("kendoComboBox").select() != -1) {
                            var button = $(this);
                            button.attr('disabled', 'disabled');
                            setTimeout(function () {
                                button.removeAttr('disabled');
                            }, 500);

                            ObtenerJSonGridSoldadura();

                        }
                    }
                    else
                        displayNotify("JuntaSinSeleccionar", "", '1');
                }
                else {
                    displayNotify("Mensajes_error", "Favor de seleccionar un Tipo de Captura", '1');
                }
            }
            else {
                desplegarNotificacion();
            }
        }
    });
}

function SuscribirEventoID() {
    var dataItem;

    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        suggest: true,
        delay: 10,
        filter: "endswith",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if ($("#InputID").val().length == 1) {
                    $("#InputID").data("kendoComboBox").value(("00" + $("#InputID").val()).slice(-3));
                    AjaxObtenerListaTaller();
                }
                if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
                    Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                    $("#LabelProyecto").text(dataItem.Proyecto);
                    if ($('input:radio[name=TipoAgregado]:checked').val() != "Reporte") {
                        AjaxJunta($("#InputID").val());
                    }
                    AjaxObtenerListaTaller();
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
        else if (e.keyCode == 13) {
            if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()) != undefined) {
                if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
                    if ($("#InputID").data("kendoComboBox").select() != -1) {
                        AjaxJuntaModoSpool($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
                    }
                }
            }
            else {
                if ($("#InputID").val() == "") {
                    displayNotify("CapturaSoldaduraSpoolNoCapturado", "", '1');
                } else {
                    displayNotify("CapturaSoldaduraNoExisteSpoolID", "", '1');
                }
            }
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
                    AjaxObtenerListaTaller();
                }
            }
            else {
                $("#Junta").data("kendoComboBox").dataSource.data([]);
                $("#Junta").data("kendoComboBox").value("");
            }
        }
    });

    $('#InputID').blur(function (e) {

        var spoollIDValue = $("#InputID").val();
        var listaSpoolID = $("#InputID").data("kendoComboBox").dataSource._data;
        var valorEncontrado = false;
        if (spoollIDValue != "" && listaSpoolID.length > 0) {
            for (var i = 0; i < listaSpoolID.length; i++) {


                if (TryParseInt(spoollIDValue, 0) != 0 && (TryParseInt(spoollIDValue, 0) == TryParseInt(listaSpoolID[i].IDValido, 0))) {
                    valorEncontrado = true;
                    $("#InputID").data("kendoComboBox").select(0);
                    $("#InputID").data("kendoComboBox").value(listaSpoolID[i].Valor);
                    if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
                        $("#ButtonAgregar").trigger("click");
                    }

                    AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
                    break;
                }
            }

        }

        dataItem = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select());
        if (!valorEncontrado && dataItem == undefined && spoollIDValue != "") {
            $("#Junta").data("kendoComboBox").dataSource.data([]);
            $("#Junta").data("kendoComboBox").select(0);
            displayNotify("NoExisteSpoolID", "", '1');
        }

    });
}

function SuscribirEventosOrdenTrabajo() {
    $("#InputOrdenTrabajo").blur(function (e) {

        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                var OrdenTrabajoOrigianl = $("#InputOrdenTrabajo").val();

                $Soldadura.Soldadura.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
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
                    }
                });

            } catch (e) {
                displayNotify("Mensajes_error", e.message, '1');

            }
        } else {
            displayNotify("CapturaArmadoMensajeOrdenTrabajo", "", '1');
        }
    });

    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputID").data("kendoComboBox").setDataSource([]);
        $("#InputOrdenTrabajo").val("");
        $('#InputID').data("kendoComboBox").text("");
        $("#Junta").data("kendoComboBox").text("");

    });
}

function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {

        if ($("#InputID").val() != "" && $("#InputID").val() != "0" && $("#inputordentrabajo").val() != "") {
            AjaxJunta($("#InputID").val());
        }
        FiltroMostrar(0);
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {

        if ($("#InputID").val() != "" && $("#InputID").val() != "0" && $("#InputOrdenTrabajo").val() != "") {
            AjaxJunta($("#InputID").val());
        }
        FiltroMostrar(1);
    });
}

function suscribirEventoChangeRadioTipoListado() {

    $('input:radio[name=TipoAgregado]').change(function () {

        if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
            console.log("noabrepop");

            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                $('#InputOrdenTrabajo').val('');
                $('#InputID').data('kendoComboBox').text("");
                $('#Junta').data('kendoComboBox').text("");
                CambioTipoListado();
                AjaxCargarCamposPredeterminadosCambiaTipoVista();
            }
            else {
                suscribirEventoWindowsConfirmaCaptura();
                ventanaConfirm.open().center();
            }
        }
        else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {

            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                $('#InputOrdenTrabajo').val('');
                $('#InputID').data('kendoComboBox').text("");
                $('#Junta').data('kendoComboBox').text("");
                CambioTipoListado();
                AjaxCargarCamposPredeterminadosCambiaTipoVista();
            }
            else {
                suscribirEventoWindowsConfirmaCaptura();
                ventanaConfirm.open().center();
            }
        }
    });
}

function SuscribirEventoMuestraJunta() {

    if ($('input:radio[name=TipoAgregado]').val() == "Reporte") {
        $("#JuntaDiv").hide();
    }
    else if ($('input:radio[name=TipoAgregado]').val() == "Listado") {
        $("#JuntaDiv").show();
    }
}

function suscribirEventoGridPopupSoldadoresRaiz() {

    $(document).on('click', '.botonSoldadoresRaiz', function (e) {
        e.preventDefault();
        if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.ProcesoSoldaduraRaiz != "" && dataItem.WPSNombre != "" && dataItem.ProcesoSoldaduraRaiz != "N/A")
                AjaxObtenerListadoColadas(dataItem, 1);
        }
    });
}

function suscribirEventoGridPopupSoldadoresRelleno() {

    $(document).on('click', '.botonSoldadoresRelleno', function (e) {
        e.preventDefault();
        if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.ProcesoSoldaduraRelleno != "" && dataItem.WPSNombre != "" && dataItem.ProcesoSoldaduraRelleno != "N/A")
                AjaxObtenerListadoColadas(dataItem, 0);

        }
    });
}





function GuardarDetalleAdicional() {
    $('#GuardarTrabajosAdicionales').click(function () {
        var trabajosCorrectos = true;
        var ds = $("#gridPopUp").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if ((ds._data[i].TrabajoAdicional == "" || ds._data[i].Soldador == "") && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                trabajosCorrectos = false;
        }
        if (trabajosCorrectos) {
            modeloRenglon.ListaDetalleTrabajoAdicional = ds._data;

            var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
            var filters = dataSource.filter();
            var allData = dataSource.data();
            var query = new kendo.data.Query(allData);
            var data = query.filter(filters).data;

            actuallongitudTrabajosAdicionales = data.length;
            if (actuallongitudTrabajosAdicionales == 0)
                modeloRenglon.TemplateTrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
            else
                modeloRenglon.TemplateTrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];

            $("#grid").data("kendoGrid").dataSource.sync();

            $("#windowGrid").data("kendoWindow").close();

        }
        else {
            displayNotify('CapturaSoldaduraTrabajoMandatorio', '', '1');
        }
    });
}

function GuardarSoldadoresRaiz() {
    $('#GuardarSoldadoresRaiz').click(function () {
        var RaizCorrectos = true;
        var RegistrosNoRepetidos = true;
        var ds = $("#inputSoldadoresRaiz").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if (ds._data[i].Colada == "" || ds._data[i].Soldador == "" && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                RaizCorrectos = false;
        }
        if (RaizCorrectos) {
            for (var i = 0; i < ds._data.length; i++) {
                for (var j = 0; j < ds._data.length; j++) {
                    if (ds._data[i].Colada == ds._data[j].Colada && ds._data[i].Soldador == ds._data[j].Soldador && i != j && ds._data[i].Accion != 3 && ds._data[j].Accion != 3)
                        RegistrosNoRepetidos = false;
                }
            }
        }

        if (RaizCorrectos) {
            if (RegistrosNoRepetidos) {
                modeloRenglon.ListaSoldadoresRaizCapturados = ds._data;

                var dataSource = $("#inputSoldadoresRaiz").data("kendoGrid").dataSource;
                var filters = dataSource.filter();
                var allData = dataSource.data();
                var query = new kendo.data.Query(allData);
                var data = query.filter(filters).data;

                actuallongitudSoldadoresRaiz = data.length;
                if (actuallongitudSoldadoresRaiz == 0)
                    modeloRenglon.TemplateSoldadoresRaiz = _dictionary.CapturaArmadoTemplateNoHaySoldadoresRaiz[$("#language").data("kendoDropDownList").value()];
                else
                    modeloRenglon.TemplateSoldadoresRaiz = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudSoldadoresRaiz + _dictionary.CapturaSoldaduraMensajeCambioSoldadoresRaiz[$("#language").data("kendoDropDownList").value()];

                $("#grid").data("kendoGrid").dataSource.sync();



                $("#windowGridSoldadorRaiz").data("kendoWindow").close();
            }
            else {
                displayNotify('CapturaSoldaduraGridSoldadoresRegistrosRepetidos', '', '1');
            }
        }
        else {
            displayNotify('CapturaSoldaduraGridSoldadores', '', '1');
        }
    });
}

function GuardarSoldadoresRelleno() {
    $('#GuardarSoldadoresRelleno').click(function () {
        var RaizCorrectos = true;
        var RegistrosNoRepetidos = true;
        var ds = $("#inputSoldadoresRelleno").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if (ds._data[i].Colada == "" || ds._data[i].Soldador == "" && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                RaizCorrectos = false;
        }

        if (RaizCorrectos) {
            for (var i = 0; i < ds._data.length; i++) {
                for (var j = 0; j < ds._data.length; j++) {
                    if (ds._data[i].Colada == ds._data[j].Colada && ds._data[i].Soldador == ds._data[j].Soldador && i != j && ds._data[i].Accion != 3 && ds._data[j].Accion != 3)
                        RegistrosNoRepetidos = false;
                }
            }
        }


        if (RaizCorrectos) {
            if (RegistrosNoRepetidos) {
                modeloRenglon.ListaSoldadoresRellenoCapturados = ds._data;

                var dataSource = $("#inputSoldadoresRelleno").data("kendoGrid").dataSource;
                var filters = dataSource.filter();
                var allData = dataSource.data();
                var query = new kendo.data.Query(allData);
                var data = query.filter(filters).data;

                actuallongitudSoldadoresRelleno = data.length;
                if (actuallongitudSoldadoresRelleno == 0)
                    modeloRenglon.TemplateSoldadoresRelleno = _dictionary.CapturaArmadoTemplateNoHaySoldadoresRelleno[$("#language").data("kendoDropDownList").value()];
                else
                    modeloRenglon.TemplateSoldadoresRelleno = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudSoldadoresRelleno + _dictionary.CapturaSoldaduraMensajeCambioSoldadoresRelleno[$("#language").data("kendoDropDownList").value()];

                $("#grid").data("kendoGrid").dataSource.sync();

                $("#windowMultiselectSoldadorRelleno").data("kendoWindow").close();

            }
            else {
                displayNotify('CapturaSoldaduraGridSoldadoresRegistrosRepetidos', '', '1');
            }
        }
        else {
            displayNotify('CapturaSoldaduraGridSoldadores', '', '1');
        }
    });
}

function suscribirEventoGridPopupTrabajosAdicionales() {
    $(document).on('click', '.botonAdicionales', function (e) {
        e.preventDefault();

        if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));
            if (dataItem.ListaSoldadoresRaizCapturados.length > 0 || dataItem.ListaSoldadoresRellenoCapturados.length > 0)
                GridPopUpTrabajosAdicionales(dataItem);
        }
    });
}


function suscribirEventoWindowsConfirmaCaptura() {
    ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProy").click(function (e) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        $('#InputOrdenTrabajo').val('');
        $('#InputID').data('kendoComboBox').text("");
        $('#Junta').data('kendoComboBox').text("");
        CambioTipoListado();
        AjaxCargarCamposPredeterminadosCambiaTipoVista();
        ventanaConfirm.close();
        editado = false;
    });
    $("#noButtonProy").click(function (e) {
        eventoRegresarTipoListado();
        CambioTipoListado();

        AjaxCargarCamposPredeterminadosCambiaTipoVista();
        ventanaConfirm.close();
    });
}


function eventoRegresarTipoListado() {

    if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
        $("#styleReporte").removeClass("active");
        $("#styleListado").addClass("active");
        $('input:radio[name=TipoAgregado]:nth(1)').attr('checked', true);
        $('input:radio[name=TipoAgregado]:nth(1)').trigger("click");
    }
    else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
        $("#styleReporte").addClass("active");
        $("#styleListado").removeClass("active");
        $('input:radio[name=TipoAgregado]:nth(0)').attr('checked', true);
        $('input:radio[name=TipoAgregado]:nth(0)').trigger("click");
    }
}


