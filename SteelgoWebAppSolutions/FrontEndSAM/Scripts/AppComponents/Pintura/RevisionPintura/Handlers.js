var ventanaConfirmEdicion;
var ventanaConfirmEdicionSinTipoBusqueda;

function SuscribirEventos() {
    suscribirEventoOrdenTrabajo();
    SuscribirEventoSpoolID();
    SuscribirEventoProyecto();
    suscribirEventoChangeRadioTipoBusqueda();
    SuscribirEventoBusqueda();
    SuscribirEventoGuardar();
    suscribirEventoWindowsConfirmaCaptura();
    suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda();
    suscribirEventoPlanchadoSistemaPintura();
    suscribirEventoPlanchadoSistemaPinturaColor();
    suscribirEventoPlanchadoMotivo();
	SuscribirEventoPlanchar();
	SuscribirEventoZona();
	SuscribirEventoCuadrante();
	suscribirEventoDescargarCarro();

	
}


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
       
    });
};

function SuscribirEventoSpoolID() {
    var hold = false;
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
                if (dataItem.Status != "1") {
                    e.preventDefault();
                    $("#InputID").data("kendoComboBox").value("0");
                    $("#InputID").data("kendoComboBox").text("");
                    displayNotify("notificationslabel0057", "", 1)
                    hold = true;

                    $("#InputID").data("kendoComboBox").close();
                }
            }

        }
    });

    $("#InputID").data("kendoComboBox").input.on("focus", function () {
        caracteresEscritosEnPagina = '';
        $("#InputID").data("kendoComboBox").text("");
    });

    
    $('#InputID').closest('.k-widget').keydown(function (e) {
         if (e.keyCode == 13) {
            if ($("#InputID").data("kendoComboBox").text() != '') {
                if ($("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select().Valor) != undefined) {
                    
                    $("#btnBuscar").trigger("click");
                    
                }
                else {
                    if (!hold)
                        displayNotify("NoExisteSpoolID", '', '2');
                }
            }
            //}
        }
        else if (e.keyCode == 9) {
            if (tieneClase(e.currentTarget)) {
                $("#InputID").data("kendoComboBox").select(0);

                if ($('#InputID').data("kendoComboBox").dataSource._data[$('#InputID').data("kendoComboBox").selectedIndex].Status != "1") {
                    e.preventDefault();
                    $("#InputID").data("kendoComboBox").value("0");
                    $("#InputID").data("kendoComboBox").text("");
                    displayNotify("notificationslabel0057", "", 1)
                    hold = true;

                    $("#InputID").data("kendoComboBox").close();
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


function SuscribirEventoCuadrante() {
	$('#inputCuadrantePopup').kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "CuadranteID",
		suggest: true,
		filter: "contains",
		change: function (e) {
			var dataItem = this.dataItem(e.sender.selectedIndex);
			if (dataItem != undefined) {

			}
			else {
				$("#inputCuadrantePopup").data("kendoComboBox").value("");
			}
		}
	});
}

function SuscribirEventoZona() {
	$('#inputZonaPopup').kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "ZonaID",
		suggest: true,
		filter: "contains",
		change: function (e) {
			var dataItem = this.dataItem(e.sender.selectedIndex);
			$("#inputCuadrantePopup").data("kendoComboBox").dataSource.data([]);
			$("#inputCuadrantePopup").data("kendoComboBox").value("");

			if (dataItem != undefined) {
				if (dataItem.ZonaID != 0) {
					AjaxCargarCuadrante(dataItem.ZonaID);
				}
				else
					windowDownload.open().center();
			}
			else {
				$("#inputZonaPopup").data("kendoComboBox").value("");
			}
		}
	});
}

function suscribirEventoDescargarCarro() {
	windowDownload = $("#windowDownload").kendoWindow({
		iframe: true,
		title: _dictionary.PinturaCargaTitulo[$("#language").data("kendoDropDownList").value()],
		visible: false,
		width: "auto",
		height: "auto",
		modal: true,
		animation: {
			close: false,
			open: false
		},
		actions: [
			"Close"
		],
	}).data("kendoWindow");
	$("#btnDescargar").click(function (handler) {


		var Zona = $("#inputZonaPopup").data("kendoComboBox").dataItem($("#inputZonaPopup").data("kendoComboBox").select());
		var Cuadrante = $("#inputCuadrantePopup").data("kendoComboBox").dataItem($("#inputCuadrantePopup").data("kendoComboBox").select());

		if (Zona != undefined && Zona.ZonaID != 0) {
			if (Cuadrante != undefined && Cuadrante.CuadranteID != 0) {
				windowDownload.close();
				AjaxDescargarSpool(dataItem, Cuadrante);
			} else {
				displayNotify("EmbarqueCargaMsjErrorCuadrante", "", "1");
			}
		} else {
			displayNotify("EmbarqueCargaMsjErrorZona", "", "1");
		}

	});

	$("#btnCerrarPopup").click(function () {
		windowDownload.close();
	});
}


function suscribirEventoPlanchadoSistemaPintura()
{
    $('#inputPlanchadoSP').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "SistemaPinturaProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputPlanchadoSP").data("kendoComboBox").value("");
            }
            else {
                AjaxCargarColorPinturaPlanchado(dataItem.SistemaPinturaProyectoID);
            }
        }
    });
    
}

function suscribirEventoPlanchadoSistemaPinturaColor()
{
    $('#inputPlanchadoColor').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "SistemaPinturaColorID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputPlanchadoSP").data("kendoComboBox").value("");
            }
        }
    });
   
}

function suscribirEventoPlanchadoMotivo()
{
    $('#inputPlanchadoMotivo').kendoComboBox({
        dataTextField: "Rechazo",
        dataValueField: "TipoRechazoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#inputPlanchadoSP").data("kendoComboBox").value("");
            }
        }
    });
   
}

function suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda()
{
    ventanaConfirmEdicionSinTipoBusqueda = $("#ventanaConfirmCaptura").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicionSinTipoBusqueda.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProySinTipoBusqueda'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProySinTipoBusqueda'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


	$("#yesButtonProySinTipoBusqueda").click(function (e) {
		//tipoBusquedaSeleccionada = $('input:radio[name=TipoBusqueda]:checked').val() == "spool" ? 1 : 2;
		//datoSeleccionado = tipoBusquedaSeleccionada == 1 ? $("#inputSpool").val() : $("#inputNc").val();
		//AjaxConsultarSpoolsConSP(tipoBusquedaSeleccionada, datoSeleccionado);
      

        ventanaConfirmEdicionSinTipoBusqueda.close();
        editado = false;

        $("#grid").data("kendoGrid").dataSource.data([]);
        switch (EjecutaChange) {
            case 1:
                $("#inputProyecto").data("kendoComboBox").trigger("change");
                break;
            case 2:
                tipoBusquedaSeleccionada = $('input:radio[name=TipoBusqueda]:checked').val() == "spool" ? 1 : 2;
                datoSeleccionado = tipoBusquedaSeleccionada == 1 ? $("#inputSpool").val() : $("#inputNc").val();
                AjaxConsultarSpoolsConSP(tipoBusquedaSeleccionada, datoSeleccionado);
                break;
            case 3:
                $("#btnBuscar").trigger("click");
                break;

        }
    });
	$("#noButtonProySinTipoBusqueda").click(function (e) {
	    $("#inputProyecto").data("kendoComboBox").value(LineaCaptura.ProyectoIDSeleccionado);
        ventanaConfirmEdicionSinTipoBusqueda.close();
    });
}

function suscribirEventoWindowsConfirmaCaptura() {
    ventanaConfirmEdicion = $("#ventanaConfirmCapturaCambioTipo").kendoWindow({
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
        "</br><center><button class='btn btn-blue' id='yesButtonProyTipoB'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProyTipoB'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProyTipoB").click(function (e) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        eventoCambioTipoListado();
        ventanaConfirmEdicion.close();
        editado = false;
    });
    $("#noButtonProyTipoB").click(function (e) {
        eventoRegresarTipoListado();
        ventanaConfirmEdicion.close();
    });
}

function SuscribirEventoGuardar() {
    $('.accionGuardar').click(function (e) {
        //e.stopPropagation();
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            //opcionHabilitarView(true, "FieldSetView");
            var ds = $("#grid").data("kendoGrid").dataSource;
            AjaxGuardar(ds._data, false);
        }
        else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "FieldSetView")
        }
    });

    $('.accionGuardarYNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxGuardar(ds._data, true);
    });
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
            EjecutaChange = 1;
            if (!editado) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem == undefined) {
                   
                    $("#inputProyecto").data("kendoComboBox").value("");
                }
                else {

                    LineaCaptura.ProyectoIDSeleccionado = dataItem.ProyectoID;
                    if (tipoBusquedaSeleccionada == 1)
                        $("#inputSpool").val("");
                    else
                        $("#inputNc").val("");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    AjaxObtenerCatalogosPlanchado();
                }
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });
}

function suscribirEventoChangeRadioTipoBusqueda() {

    $('input:radio[name=TipoBusqueda]').change(function () {
        if ($('input:radio[name=TipoBusqueda]:checked').val() == "spool") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoListado();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
        else if ($('input:radio[name=TipoBusqueda]:checked').val() == "nc") {
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

function eventoCambioTipoListado() {
    if ($('input:radio[name=TipoBusqueda]:checked').val() == "spool") {
        $("#divNc").css('display', 'none');
        $("#divSpool").css('display', 'block');
        $("#inputSpool").val("");
        $("#inputNc").val("");

        $("#inputProyecto").data("kendoComboBox").value("");
    }
    else if ($('input:radio[name=TipoBusqueda]:checked').val() == "nc") {
        $("#divNc").css('display', 'block');
        $("#divSpool").css('display', 'none');
        $("#inputSpool").val("");
        $("#inputNc").val("");
        $("#inputProyecto").data("kendoComboBox").value("");
    }
}

function eventoRegresarTipoListado() {
    if ($('input:radio[name=TipoBusqueda]:checked').val() == "spool") {
        $("#divSpool").removeClass("active");
        $("#divNc").addClass("active");
        $('input:radio[name=TipoBusqueda]:nth(1)').attr('checked', true);
        $('input:radio[name=TipoBusqueda]:nth(1)').trigger("click");
    }
    else if ($('input:radio[name=TipoBusqueda]:checked').val() == "nc") {
        $("#divSpool").addClass("active");
        $("#divNc").removeClass("active");
        $('input:radio[name=TipoBusqueda]:nth(0)').attr('checked', true);
        $('input:radio[name=TipoBusqueda]:nth(0)').trigger("click");
    }
}

function SuscribirEventoBusqueda() {
    $("#btnBuscar").click(function (e) {
        EjecutaChange=3
        if (!editado) {
            var Proyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select());
            var tipoBusqueda = 0;
            var cadena = "";

            if (Proyecto != undefined && Proyecto.ProyectoID != "" && Proyecto.ProyectoID != 0) {
                tipoBusquedaSeleccionada = 1;
                datoSeleccionado = $("#InputID").val();
                AjaxConsultarSpoolsConSP(1, $("#InputID").val());

            } else {
                displayNotify("MensajeSeleccionaProyecto", "", '1');
            }
        }
        else {
            ventanaConfirmEdicionSinTipoBusqueda.open().center();
        }
    });

   
}

function SuscribirEventoPlanchar() {
    $("#ButtonPlanchar").click(function (e) {
        e.preventDefault();
        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            displayNotify("RevisionPinturaGenerarRevision", "", '1');
             if ($('input:radio[name=LLena]:checked').val() === "Todos") {
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
    if ($("#inputPlanchadoSP").data("kendoComboBox").dataItem($("#inputPlanchadoSP").data("kendoComboBox").select()) != undefined) {
        PlanchaSistemaPintura();
    }
    if ($("#inputPlanchadoColor").data("kendoComboBox").dataItem($("#inputPlanchadoColor").data("kendoComboBox").select()) != undefined) {
        PlanchaColor();
    }
    if ($("#inputPlanchadoMotivo").data("kendoComboBox").dataItem($("#inputPlanchadoMotivo").data("kendoComboBox").select()) != undefined) {
        PlanchaMotivo();
    }
    if ($('input:radio[name=SelectTodos]:checked').val() != "Ninguno" || $('input:radio[name=SelectTodos]:checked').val()=="") {
        PlancharGenerarRevision();
    }

    loadingStop();
}