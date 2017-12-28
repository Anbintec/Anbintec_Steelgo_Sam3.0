var ventanaConfirmEdicion;
var ventanaConfirmEdicionCambioProcesoPintura;

function SuscribirEventos() {
	suscribirEventoCarro();
	suscribirEventoGuardarCarro();
	suscribirEventoShotBlastero();
	SuscribirEventoSpoolID();
	suscribirEventoDescargar();
	SuscribirEventoAgregar();
	SuscribirEventoPlanchar();
	suscribirEventoSeleccionMuestra();
	suscribirEventoSeleccionProcesoPintura();
	SuscribirEventoMostrar();
	suscribirEventoWindowsConfirmaCapturaCambioProcesoPintura();
	suscribirEventoWindowsConfirmaCapturaCambioCarro();
	suscribirEventoVersion();
	suscribirEventoDescargarCarro();
	SuscribirEventoZona();
	SuscribirEventoCuadrante();
	SuscribirEventoEliminaRegistro();
	SuscribirEventoColor();
}

function SuscribirEventoColor() {
	$('#inputColor').kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "ColorID",
		suggest: true,
		index: 3,
		change: function (e) {
			ejecutadoPor = 1;
			dataItem = this.dataItem(e.sender.selectedIndex);
			if (dataItem.ColorID != undefined && dataItem.ColorID != "0") {
				if (!editado) {
					if (dataItem != undefined && dataItem.ColorID != "0") {
						if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()) != undefined) {
							BuscarDetalleCarro()

						}
						else {
							$("#inputColor").data("kendoComboBox").value("");
						}
					}
					else {
						$("#inputColor").data("kendoComboBox").value("");
					}
				}
				else {
					ventanaConfirmEdicion.open().center();
				}
			}
			else
			{
				$("#inputColor").data("kendoComboBox").value(ColorAnterior == undefined ? "" : ColorAnterior.ColorID);
			}
		}
	});
	$('#inputColor').closest('.k-widget').keydown(function (e) {
		if (e.keyCode == 13) {
			if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()) != undefined) {
				BuscarDetalleCarro();
			}
			else {
				$("#inputCarro").data("kendoComboBox").value("");
			}
		}
	});
}

function SuscribirEventoEliminaRegistro() {
	$(document).on('click', '.k-grid-Cancelar', function (e) {

		if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
			var dataSource = $("#grid").data("kendoGrid").dataSource;
			var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

			if (dataItem.Accion == 1 || dataItem.Accion == undefined) {
				dataSource.remove(dataItem);
				if (dataSource._data.length == 0) // si borro y es el ultimo elemento entonces se dice que el usuario no ah editado nada.
					editado = false;
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
			}
			else {
				$("#inputZonaPopup").data("kendoComboBox").value("");
			}
		}
	});
}


function SuscribirEventoCuadrante() {
	$('#inputCuadrantePopup').kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "CuadranteID",
		suggest: true,
		filter: "contains",
		change: function (e) {
			var dataItem = this.dataItem(e.sender.selectedIndex);
			if (dataItem != undefined && dataItem.Cuadrante != "") {
			    dataItemCuadranteNuevo = dataItem;
			}
			else {
			    $("#inputCuadrantePopup").data("kendoComboBox").value("");
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
				AjaxDescargarSpool(dataItemCuadranteNuevo, Cuadrante);
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

function suscribirEventoVersion() {

	$("#inputVersion").kendoComboBox({
		dataTextField: "Version",
		dataValueField: "Version",
		suggest: true,
		filter: "contains",
		change: function (e) {
			if ($("#inputVersion").data("kendoComboBox").dataItem($("#inputVersion").data("kendoComboBox").select()) != undefined) {
			}
			else {
				$("#inputVersion").data("kendoComboBox").value("");
			}
		}
	});
}

function suscribirEventoWindowsConfirmaCapturaCambioProcesoPintura() {
	ventanaConfirmEdicionCambioProcesoPintura = $("#ventanaConfirmCapturaProcesoPintura").kendoWindow({
		iframe: true,
		title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
		visible: false,
		width: "auto",
		height: "auto",
		modal: true,
		animation: false,
		actions: []
	}).data("kendoWindow");

	ventanaConfirmEdicionCambioProcesoPintura.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
		"</br><center><button class='btn btn-blue' id='yesButtonCambioProcesoPintura'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonCambioProcesoPintura'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


	$("#yesButtonCambioProcesoPintura").click(function (e) {
		LimpiarDespuesCambioProcesoPintura();
		ventanaConfirmEdicionCambioProcesoPintura.close();
		editado = false;

		CambiarProcesoPintura();
	});
	$("#noButtonCambioProcesoPintura").click(function (e) {
		eventoRegresarTipoListado();
		ventanaConfirmEdicionCambioProcesoPintura.close();
	});
}

function suscribirEventoWindowsConfirmaCapturaCambioCarro() {
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
		ventanaConfirmEdicion.close();
		editado = false;
		BuscarDetalleCarro();
	});
	$("#noButtonProy").click(function (e) {
		$("#inputCarro").data("kendoComboBox").value(CarroAnterior.MedioTransporteID);
		$("#inputColor").data("kendoComboBox").value(ColorAnterior==undefined?"": ColorAnterior.ColorID);
		ventanaConfirmEdicion.close();
	});
}


function eventoRegresarTipoListado() {

	switch (procesoPinturaSeleccionadoAnterior) {
		case "1":
			$('input:radio[name=ProcesoPintura]:nth(0)').attr('checked', true);
			$('input:radio[name=ProcesoPintura]:nth(0)').trigger("click");
			break;
		case "2":
			$('input:radio[name=ProcesoPintura]:nth(1)').attr('checked', true);
			$('input:radio[name=ProcesoPintura]:nth(1)').trigger("click");
			break;
		case "3":
			$('input:radio[name=ProcesoPintura]:nth(2)').attr('checked', true);
			$('input:radio[name=ProcesoPintura]:nth(2)').trigger("click");
			break;
		case "4":
			$('input:radio[name=ProcesoPintura]:nth(3)').attr('checked', true);
			$('input:radio[name=ProcesoPintura]:nth(3)').trigger("click");
			break;
	}
}

function SuscribirEventoPlanchar() {
	$("#ButtonPlanchar").click(function (e) {
		if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
			if ($('input:radio[name=LLena]:checked').val() === "Todos") {
				windowTemplate = kendo.template($("#windowTemplate").html());

				ventanaConfirm = $("#ventanaConfirm").kendoWindow({
					iframe: true,
					title: _dictionary.PinturaCargaTitulo[$("#language").data("kendoDropDownList").value()],
					visible: false,
					width: "auto",
					height: "auto",
					modal: true,
					animation: false,
					actions: []
				}).data("kendoWindow");

				ventanaConfirm.content(_dictionary.EntregaPlacasGraficasPlancharTodos[$("#language").data("kendoDropDownList").value()] +
					"</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>Si</button><button class='confirm_yes btn btn-blue' id='noButton'> No</button></center>");

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


	if ($("#inputShotBlastero").data("kendoMultiSelect")._dataItems.length > 0) {
		PlancharTrabajadores($("#grid").data("kendoGrid").dataSource._data);
	}
	if ($("#FechaShotBlast").val() != "") {
		PlanchaFechaProceso();
	}

	PlanchaElementosDinamicos();
}

function suscribirEventoDescargar() {
	$('#CapturaAvanceDescargar').click(function (e) {
		ajaxAplicarDescarga(currentDataItemGridDownload)
		win.close();
	});
}

function suscribirEventoGuardarCarro() {
	$("#btnGuardarYNuevo, #btnGuardarYNuevo2").click(function () {
		var ds = $("#grid").data("kendoGrid").dataSource;
		AjaxGuardarAvanceCarro(ds._data, true);
	});

	$('.accionGuardar').click(function (e) {
		if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
			var ds = $("#grid").data("kendoGrid").dataSource;
			AjaxGuardarAvanceCarro(ds._data, false);
		}
		else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
			opcionHabilitarView(false, "FieldSetView")
		}
	});
}

function opcionHabilitarView(valor, name) {
	if (valor) {
		$('.FieldSetView').find('*').attr('disabled', true);


		$("#inputCarro").data("kendoComboBox").enable(false);
		$("#InputOrdenTrabajo").attr('disabled', true);
		$("#InputID").data("kendoComboBox").enable(false);
		$("#FechaShotBlast").data("kendoDatePicker").enable(false);
		$("#inputColor").data("kendoComboBox").enable(false);
		
		$("input[name='Muestra']").attr("disabled", true);

		$("#stylePPShotBlast").attr("disabled", true);
		$("#stylePPPrimario").attr("disabled", true);
		$("#stylePPIntermedio").attr("disabled", true);
		$("#stylePPAvanzado").attr("disabled", true);
		BloquearElementosDinamicos(false);


		$("#inputShotBlastero").data("kendoMultiSelect").enable(false);

		$("#btnMostrar").attr("disabled", true);
		$("input[name='LLena']").attr("disabled", true);
		$("#btnAgregar").attr("disabled", true);
		$("#ButtonPlanchar").attr("disabled", true);

		$("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
		$("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
		$("#GuardarPie").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
		$("#btnGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
	}
	else {
		$('.FieldSetView').find('*').attr('disabled', false);


		$("#inputColor").data("kendoComboBox").enable(true);
		$("#inputCarro").data("kendoComboBox").enable(true);
		$("input[name='Muestra']").attr("disabled", false);
		$("#stylePPShotBlast").attr("disabled", false);
		$("#stylePPPrimario").attr("disabled", false);
		$("#stylePPIntermedio").attr("disabled", false);
		$("#stylePPAvanzado").attr("disabled", false);
		$("#btnMostrar").attr("disabled", false);
		BloquearElementosDinamicos(true);
		$("#InputOrdenTrabajo").attr('disabled', false);
		$("#InputID").data("kendoComboBox").enable(true);
		$("#FechaShotBlast").data("kendoDatePicker").enable(true);
		$("#inputShotBlastero").data("kendoMultiSelect").enable(true);

		$("input[name='LLena']").attr("disabled", false);
		$("#btnAgregar").attr("disabled", false);
		$("#ButtonPlanchar").attr("disabled", false);

		$("#Guardar").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
		$("#btnGuardar").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
		$("#GuardarPie").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
		$("#btnGuardar4").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
	}
}

function BloquearElementosDinamicos(bloqueado) {
	for (var i = 0; i < ComponentesDinamicosJSON.length; i++) {
		$('#' + ComponentesDinamicosJSON[i].NombreColumna).data("kendoComboBox").enable(bloqueado);
	}

	for (var i = 0; i < ReductoresDinamicosJSON.length; i++) {
		$('#' + ReductoresDinamicosJSON[i].NombreColumna).data("kendoComboBox").enable(bloqueado);
	}
}


function suscribirEventoCarro() {

	$("#inputCarro").kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "MedioTransporteID",
		suggest: true,
		filter: "contains",
		change: function (e) {
			ejecutadoPor = 0;
			dataItem = this.dataItem(e.sender.selectedIndex);
			if (dataItem != undefined && dataItem.MedioTransporteID != "0") {
				if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()) != undefined) {
					// var dataItem = this.dataItem(e.sender.selectedIndex);
					AjaxCargarShotBlastero();
					BuscarDetalleCarro();
				}
				else {
					$("#inputCarro").data("kendoComboBox").value("");
				}
			}
			else {
				$("#inputCarro").data("kendoComboBox").value("");
			}
		}
	});


	$('#inputCarro').closest('.k-widget').keydown(function (e) {
		if (e.keyCode == 13) {
			if ($("#inputCarro").data("kendoComboBox").dataItem($("#inputCarro").data("kendoComboBox").select()) != undefined) {
				BuscarDetalleCarro();
			}
			else {
				$("#inputCarro").data("kendoComboBox").value("");
			}
		}

	});

}




function suscribirEventoShotBlastero() {

	$("#inputShotBlastero").kendoMultiSelect({
		dataSource: '',
		dataTextField: "Codigo",
		dataValueField: "ObreroID",
		suggest: true,
		filter: "contains"
	}).data("kendoMultiSelect");
}

function SuscribirEventoAgregar() {
	$("#btnAgregar").click(function () {
		if ($("#InputID").data("kendoComboBox").value() > 0) {
			AjaxAgregarSpool($("#InputID").data("kendoComboBox").value());
		}
		else {
			displayNotify("PinturaCargaSeleccionaSpool", "", '1');
		}
	});
}

function SuscribirEventoMostrar() {
	$("#btnMostrar").click(function () {
		BuscarDetalleCarro();
	});
}


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

		    if (dataItem != undefined) {
		        if (dataItem.Status != 1) {
		            displayNotify("", dataItem.Status, '1');
		            $("#InputID").data("kendoComboBox").value("");
		        }
		    }
		    else {
		        $("#InputID").data("kendoComboBox").value("");
		    }

		}
	});

	$("#InputOrdenTrabajo").blur(function (e) {

		if ($("#InputOrdenTrabajo").val() != "") {
			
				try {
					AjaxObtenerSpoolID();
				} catch (e) {
					displayNotify("", e.message, '2');
				}
			
		}

	});


	$("#InputOrdenTrabajo").focus(function (e) {
		$("#InputOrdenTrabajo").val("");
		$("#InputID").data("kendoComboBox").value("");
		$("#InputID").data("kendoComboBox").setDataSource();
	});

	$('#InputID').closest('.k-widget').keydown(function (e) {

		if (e.keyCode == 37) {
			$("#InputOrdenTrabajo").focus();

		}
		else if (e.keyCode == 40) {
			$("#InputID").data("kendoComboBox").select();
		}
		else if (e.keyCode == 13) {
			if ($('#InputID').data("kendoComboBox").value() != undefined) {
				AjaxAgregarSpool($("#InputID").data("kendoComboBox").value());
			}
			else {
				$("#InputID").data("kendoComboBox").value("");
				displayNotify("PinturaCargaNoExisteSpoolID", '', '2');
			}
		}
	});

}



function suscribirEventoSeleccionProcesoPintura() {
	$('input:radio[name=ProcesoPintura]').change(function () {
		CambiarProcesoPintura();
	});
}

function suscribirEventoSeleccionMuestra() {
	$('input:radio[name=Muestra]:nth(0)').change(function () {
		FiltroMostrar(0);
	});
	$('input:radio[name=Muestra]:nth(1)').change(function () {
		FiltroMostrar(1);
	});
}

function Limpiar() {
	$("#inputCarro").data("kendoComboBox").value("");
	$("#InputOrdenTrabajo").val("");
	$("#InputID").data("kendoComboBox").dataSource.data([]);
	$("#InputID").data("kendoComboBox").value("");
	$("#FechaShotBlast").data("kendoDatePicker").value(new Date());
	$("#inputShotBlastero").data("kendoMultiSelect").value("");

	for (var i = 0; i < ComponentesDinamicosJSON.length; i++) {
		$('#' + ComponentesDinamicosJSON[i].NombreColumna).data("kendoComboBox").value("");
	}

	for (var i = 0; i < ReductoresDinamicosJSON.length; i++) {
		$('#' + ReductoresDinamicosJSON[i].NombreColumna).data("kendoComboBox").value("");
	}

	$("#grid").data('kendoGrid').dataSource.data([]);
	opcionHabilitarView(false, "FieldSetView");
}