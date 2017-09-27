var iniciaFiltroSegundoNivel = true;
var spoolIDSelectTemp = 0;
var ventanaConfirmEdicion;
function SuscribirEventos() {
    suscribirEventoOrdenTrabajo();
    SuscribirEventoSpoolID();
    SuscribirEventosJunta();
    suscribirEventoAgregar();
    suscribirEventoGuardar();
    SuscribirEventoTubero();
    SuscribirEventoTaller();
    SuscribeEventosTipoCaptura();
    suscribirEventoChangeRadio();
    suscribirEventoChangeRadioTipoListado();
    SuscribirEventoMuestraJunta();
    GuardarDetalleAdicional();
    SuscribirEventoPlanchar();
    SuscribirEventoCancelarAdicionales();
    suscribirEventoAdicionales();
    suscribirEventoWindowsConfirmaCaptura();
};

function suscribirEventoWindowsConfirmaCaptura() {
    ventanaConfirmEdicion = $("#ventanaConfirmCaptura").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicion.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProy").click(function (e) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        eventoCambioTipoListado();
        ventanaConfirmEdicion.close();
        editado = false;
    });
    $("#noButtonProy").click(function (e) {
        eventoRegresarTipoListado();
        ventanaConfirmEdicion.close();
    });
}
function suscribirEventoAdicionales() {

    $(document).on('click', '.botonAdicionales', function (e) {
        e.preventDefault();

        if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

            var grid = $("#grid").data("kendoGrid"),
            dataItem = grid.dataItem($(e.target).closest("tr"));
            LlenarGridPopUp(dataItem);
        }
    });
}

function SuscribirEventoCancelarAdicionales() {
    $("#CancelarTrabajosAdicionales").click(function (e) {
        e.preventDefault();

        $("#windowGrid").data("kendoWindow").close();
    });
}

function SuscribirEventoPlanchar() {
    $("#ButtonPlanchar").click(function (e) {
        e.preventDefault();
        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {

            if ($('input:radio[name=LLena]:checked').val() == undefined) {
                MensajesSteelGO('LLenadoMasivo', '');
            }
            else if ($('input:radio[name=LLena]:checked').val() === "Todos") {
                //windowTemplate = kendo.template($("#windowTemplate").html());

                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
                    visible: false, //the window will not appear before its .open method is called
                    width: "auto",
                    height: "auto",
                    modal: true,
                    animation: false,
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.CapturaMensajeArmadoPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                             "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='confirm_yes btn btn-blue' id='noButton'> " + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    plancharTodo();
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            }
            else {
                plancharTodo();
            }
        }
    });
}

function plancharTodo() {
    loadingStart();
    if ($("#inputTubero").data("kendoComboBox").dataItem($("#inputTubero").data("kendoComboBox").select()) != undefined) {
        PlanchaTubero();
    }
    if ($("#inputTaller").data("kendoComboBox").dataItem($("#inputTaller").data("kendoComboBox").select()) != undefined) {
        PlanchaTaller();
    }
    if ($("#FechaArmado").val() != "") {
        PlanchaFecha();
    }
    loadingStop();
}

function GuardarDetalleAdicional() {
    $('#GuardarTrabajosAdicionales').click(function () {
        var trabajosCorrectos = true;
        var ds = $("#gridPopUp").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].TrabajoAdicional == "" && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
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
                modeloRenglon.TemplateMensajeTrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
            else
                modeloRenglon.TemplateMensajeTrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];

            $("#windowGrid").data("kendoWindow").close();
            $("#grid").data("kendoGrid").dataSource.sync();
        }
        else {
            displayNotify('CapturaArmadoTrabajoMandatorio', '', '2');
        }

    });
}

function SuscribeEventosTipoCaptura() {
    $("#presentationReporte").addClass("active");

    $('#aReporte').click(function () {
        $("#presentationReporte").addClass("active");
        $("#presentationReporteListado").removeClass("active");

    });
    $('#aListado').click(function () {
        $("#presentationReporteListado").addClass("active");
        $("#presentationReporte").removeClass("active");

    });
}

function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        if ($("#InputID").val() != "" && $("#inputordentrabajo").val() != "") {
            AjaxJunta($("#InputID").val());
        }
        FiltroMostrar(0);
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        if ($("#InputID").val() != "" && $("#InputOrdenTrabajo").val() != "") {
            AjaxJunta($("#InputID").val());
        }
        FiltroMostrar(1);
    });

}

function EventoGuardar() {
    var ds = $("#grid").data("kendoGrid").dataSource;
    if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
        
        AjaxValidarNumerosUnicos(ds._data, 0);
    }
    else if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0082[$("#language").data("kendoDropDownList").value()])
        opcionHabilitarView(false, "FieldSetView")
}

function suscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        EventoGuardar();
    });


    $('.accionGuardarYNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxValidarNumerosUnicos(ds._data, 1)

    });

}

function Limpiar() {

    $("#InputOrdenTrabajo").val("");


    $("#InputID").data("kendoComboBox").value("");

    $("#Junta").data("kendoComboBox").dataSource.data([]);

    $("#FechaArmado").data("kendoDatePicker").value("");
    $("#Junta").data("kendoComboBox").value("");
    $("#inputTubero").data("kendoComboBox").value("");

    $("#inputTaller").data("kendoComboBox").value("");


    $("#grid").data('kendoGrid').dataSource.data([]);

    $("#grid").data('kendoGrid').dataSource.sync();
    opcionHabilitarView(false, "FieldSetView");
}

function suscribirEventoAgregar() {
    $('#ButtonAgregar').click(function (e) {
        if ($('input:radio[name=TipoAgregado]:checked').val() != undefined) {
            if ($('input:radio[name=Muestra]:checked').val() != undefined) {
                if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
                    if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID!=0) {
                        $('#ButtonAgregar').prop("disabled", true);
                      
                        AjaxObtenerJSonGridArmado();
                    }
                    else
                        displayNotify("NoExisteSpoolID", '', '2');
                }
                else {
                    if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado" ) {
                        if ($("#Junta").data("kendoComboBox").select()>=0 && $("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()).Etiqueta != "") {
							if ($('input:radio[name=Muestra]:checked').val() != undefined) {
								if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
								    if ($("#Junta").data("kendoComboBox").select() >= 0 && $("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()).Etiqueta != "") {

										AjaxObtenerJSonGridArmado();

									}
									else
										displayNotify("JuntaSinSeleccionar", "", '2');
								}
							}
							else {
								MensajesSteelGO('radioMostrar', '')

							}
						}
						else {
							if ($('input:radio[name=Muestra]:checked').val() == "Todos") {
								displayNotify("CapturaArmadoNoExisteSpool", "", '2');
							}
							else
								displayNotify("CapturaArmadoNoExistenJuntasSpool", "", '1');
						}
                    }
                    else
                        displayNotify("JuntaSinSeleccionar", "", '2');
                }
            }
            else {
                MensajesSteelGO('radioMostrar', '')
            }
        }
        else {
            MensajesSteelGO('radioTipoAgregado', '')
        }
    });
}

function SuscribirEventoTubero() {
    $('#inputTubero').kendoComboBox({
        dataTextField: "Codigo",
        dataValueField: "ObreroID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputTubero").data("kendoComboBox").value("");
            }
        }
    });
    $('#inputTubero').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($("#inputTubero").data("kendoComboBox").dataItem($("#inputTubero").data("kendoComboBox").select()) != undefined) {

                //PlanchaTubero();
            }
            else
                $("#inputTubero").data("kendoComboBox").value("");
        }

    });
}

function SuscribirEventoTaller() {
    $('#inputTaller').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TallerID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputTaller").data("kendoComboBox").value("");
            }
        }
    });
    $('#inputTaller').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($("#inputTaller").data("kendoComboBox").dataItem($("#inputTaller").data("kendoComboBox").select()) != undefined) {
                //PlanchaTaller();
            }
            else {
                $("#inputTaller").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventosJunta() {
    $('#Junta').kendoComboBox({
        dataTextField: "Etiqueta",
        dataValueField: "JuntaSpoolID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined || dataItem.JuntaSpoolID==0) {
                $("#Junta").data("kendoComboBox").value("");
            }
        }
    });

    $('#Junta').closest('.k-widget').keydown(function (e) {

        if (e.keyCode == 37) {
            $("#InputID").data("kendoComboBox").input.focus();
            $("#Junta").val("");
        }
        else if (e.keyCode == 39) {
            $("#ButtonAgregar").focus();
        }
        else if (e.keyCode == 9) {

           

		} 
        else if (e.keyCode == 13) {
            if ($("#Junta").data("kendoComboBox").select() >= 0 && $("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()).Etiqueta != "") {
                if ($('input:radio[name=Muestra]:checked').val() != undefined) {
                    if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
                        if ($("#Junta").data("kendoComboBox").select() != -1) {
                            AjaxObtenerJSonGridArmado();
                        }
                    }
                    else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
                        if ($("#Junta").data("kendoComboBox").select() >= 0 && $("#Junta").data("kendoComboBox").dataItem($("#Junta").data("kendoComboBox").select()).Etiqueta != "") {
                            
                                AjaxObtenerJSonGridArmado();
                            
                        }
                        else
                            displayNotify("JuntaSinSeleccionar", "", '2');
                    }
                    else {
                        displayNotify("", "Favor de seleccionar un Tipo de Captura", '2');
                    }
                }
                else {
                    MensajesSteelGO('radioMostrar', '')

                }
            }
            else {
                if ($('input:radio[name=Muestra]:checked').val() == "Todos") {
                    displayNotify("CapturaArmadoNoExisteSpool", "", '2');
                }
                else
                    displayNotify("CapturaArmadoNoExistenJuntasSpool", "", '1');
            }
        }
    });

  
}

var caracteresEscritosEnPagina = '';


function suscribirEventoOrdenTrabajo() {
    $("#InputOrdenTrabajo").blur(function (e) {

        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
               // $("#InputID").data("kendoComboBox").enable(false);
                AjaxObtenerSpoolID();
            } catch (e) {
                displayNotify("Mensajes_error", e.message, '2');

            }
        } else {
            displayNotify("CapturaArmadoMensajeOrdenTrabajo", "", '1');
        }
    });

    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").value("");
        $("#Junta").data("kendoComboBox").value("");
        //$("#Junta").data("kendoComboBox").setDataSource();
        //$("#InputID").data("kendoComboBox").setDataSource();
    });
};

function SuscribirEventoSpoolID() {
    var dataItem;
    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        suggest: true,
        filter: "contains",
        index: 3,
        delay: 10,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined && dataItem.IDValido != "") {

                Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                $("#LabelProyecto").text(dataItem.Proyecto);
                if ($('input:radio[name=TipoAgregado]:checked').val() != "Reporte") {
                    AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
                }
                setTimeout(ObtenerCatalogos(), 100);
            }
            
        }
    });

    $("#InputID").data("kendoComboBox").input.on("focus", function () {
        caracteresEscritosEnPagina = '';
        $("#InputID").data("kendoComboBox").text("");
    });

   // $("#InputID").data("kendoComboBox").enable(false);

    $('#InputID').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 37) {
            $("#InputOrdenTrabajo").focus();
        }
        else if (e.keyCode == 39) {
            $("#Junta").data("kendoComboBox").input.focus();
        }
        else if (e.keyCode == 40 && $("#InputID").data("kendoComboBox").select() != -1) {
            $("#InputID").data("kendoComboBox").select();
            //caracteresEscritosEnPagina = '';
            AjaxJunta($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor);
        }
        else if (e.keyCode == 13) {
            if ($("#InputID").data("kendoComboBox").text() != '') {
                if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select().Valor) != undefined) {
                    if ($('input:radio[name=TipoAgregado]:checked').val() != undefined) {
                        if ($('input:radio[name=Muestra]:checked').val() != undefined) {
                            if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
                                if ($("#InputID").data("kendoComboBox").select() != -1) {
                                 
                                    AjaxObtenerJSonGridArmado();
                                }
                            }
                        }
                        else {
                            MensajesSteelGO('radioMostrar', '')
                        }
                    }
                    else {
                        MensajesSteelGO('radioTipoAgregado', '')
                    }
                }
                else
                    displayNotify("NoExisteSpoolID", '', '2');
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
                    AjaxObtenerListaTubero();
                    AjaxObtenerListaTaller();
                }
            }

        }

    });

    $('#InputID').blur(function (e) {
        var spoollIDValue = $("#InputID").val().trim();
        var listaSpoolID = $("#InputID").data("kendoComboBox").dataSource._data;
        var valorEncontrado = false;
        if (listaSpoolID.length > 0) {
            for (var i = 0; i < listaSpoolID.length; i++) {
                if (TryParseInt(spoollIDValue, 0) != 0 && (TryParseInt(spoollIDValue, 0) == TryParseInt(listaSpoolID[i].IDValido, 0))) {       
                    valorEncontrado = true;
                    $("#InputID").data("kendoComboBox").select(0);
                    $("#InputID").data("kendoComboBox").value(listaSpoolID[i].Valor);
                    break;
                }
            }
        }
    });
};


function SuscribirEventoEliminar(idtable) {
    $("#" + idtable + " .deleteRow").on("click", function () {
        var td = $(this).parent();
        var tr = td.parent();
        //change the background color to red before removing
        tr.css("background-color", "#FF3700");

        tr.fadeOut(400, function () {
            tr.remove();
        });
    });
};

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

function eventoCambioTipoListado() {

    if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
        $("#JuntaDiv").css('display', 'none');
        $("#MuestraDiv").css('display', 'block');
        Limpiar();
        AjaxCargarCamposPredeterminadosOcultaJunta();
    }
    else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
        $("#JuntaDiv").css('display', 'block');
        $("#MuestraDiv").css('display', 'block');
        Limpiar();
        AjaxCargarCamposPredeterminadosOcultaJunta();
    }
}

function suscribirEventoChangeRadioTipoListado() {

    $('input:radio[name=TipoAgregado]').change(function () {
        if ($('input:radio[name=TipoAgregado]:checked').val() == "Reporte") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoListado();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
        else if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoListado();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
    });
}

function SuscribirEventoMuestraJunta() {

    if ($('input:radio[name=TipoAgregado]').val() == "Reporte") {
        $("#JuntaDiv").hide();
        // $("#MuestraDiv").hide();
    }
    else if ($('input:radio[name=TipoAgregado]').val() == "Listado") {
        $("#JuntaDiv").show();
        // $("#MuestraDiv").show();
    }
}

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $('#divLlenadoMasivo').find('*').attr('disabled', true);
        $('#InputOrdenTrabajo').css('opacity', '0.6');
        $("#InputID").data("kendoComboBox").enable(false);
        $("#Junta").data("kendoComboBox").enable(false);
        $("#inputTubero").data("kendoComboBox").enable(false);
        $("#inputTaller").data("kendoComboBox").enable(false);
        $("#FechaArmado").data("kendoDatePicker").enable(false);
        $('#botonGuardar').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#DetalleAvisoLlegada0017").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#ButtonPlanchar').attr('disabled', true);
        $("#GuardarPie").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardarPie").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarPiePagina').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $('#divLlenadoMasivo').find('*').attr('disabled', false);
        $('#InputOrdenTrabajo').css('opacity', '1');
        $("#InputID").data("kendoComboBox").enable(true);
        $("#Junta").data("kendoComboBox").enable(true);
        $("#inputTubero").data("kendoComboBox").enable(true);
        $("#inputTaller").data("kendoComboBox").enable(true);
        $("#FechaArmado").data("kendoDatePicker").enable(true);
        $('#botonGuardar').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#DetalleAvisoLlegada0017").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#ButtonPlanchar').attr('disabled', false);
        $("#GuardarPie").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardarPie").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardarPiePagina').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);

    }
}