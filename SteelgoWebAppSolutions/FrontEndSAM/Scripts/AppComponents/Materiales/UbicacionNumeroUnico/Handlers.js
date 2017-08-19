function SuscribirEventos() {
	suscribirEventoProyecto();
	suscribirEventoRacks();
	suscribirEventoGuardar();
	sucribirEventoAgregar();
	suscribirEventoRangoNU();
}

function suscribirEventoRangoNU()
{
	$("#inputRangoNumeroUnico").keydown(function (e) {
		
		
		if (e.keyCode == 13) {
			agregarNU();
		}
	});

}
function sucribirEventoAgregar() {
	$("#btnAgregar").click(function () {
		agregarNU();
	});
}

function suscribirEventoGuardar() {
	$("#Guardar").click(function () {
		if ($('#Cuantificacion0003').text() == "Guardar") {
			var proyectoID = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
			var rackID = $("#inputRack").data("kendoComboBox").dataItem($("#inputRack").data("kendoComboBox").select());
			if (rackID != undefined && rackID.RanckID != 0) {
				if ($("#grid").data("kendoGrid").dataSource._data.length > 0)
					AjaxGuardar(proyectoID.ProyectoID, $("#grid").data("kendoGrid").dataSource._data, rackID.RackID,false);
				else
					displayMessage("", "Favor de agregar numeros unicos.", '1');
			}
			else
				displayMessage("", "Necesitas seleccionar un  ranck", '1');
		}
		else {
			opcionHabilitarView(false);

		}

	});

	$("#GuardarNuevo").click(function () {
		
			var proyectoID = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
			var rackID = $("#inputRack").data("kendoComboBox").dataItem($("#inputRack").data("kendoComboBox").select());
			if (rackID != undefined && rackID.RanckID != 0) {
				if ($("#grid").data("kendoGrid").dataSource._data.length > 0)
					AjaxGuardar(proyectoID.ProyectoID, $("#grid").data("kendoGrid").dataSource._data, rackID.RackID,true);
				else
					displayMessage("", "Favor de agregar numeros unicos.", '1');
			}
			else
				displayMessage("", "Necesitas seleccionar un  ranck", '1');

	});
}


function suscribirEventoProyecto() {

	$("#inputProyecto").kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "ProyectoID",
		suggest: true,
		delay: 10,
		filter: "contains",
		index: 3,
		change: function (e) {
			var dataItem = this.dataItem(e.sender.selectedIndex);

			$("#inputRangoNumeroUnico").val("");
			$("#inputRack").data("kendoComboBox").value("");
			$("#grid").data("kendoGrid").dataSource.data([]);
			if (dataItem != undefined && dataItem.ProyectoID != 0 && dataItem.Nombre != "") {
				AjaxCargarRacks(dataItem.ProyectoID);
			}
		}
	});

	$('#inputProyecto').closest('.k-widget').keydown(function (e) {
		if (e.keyCode == 13) {
			
		}
	});
}


function suscribirEventoRacks() {
	$("#inputRack").kendoComboBox({
		dataTextField: "Nombre",
		dataValueField: "RackID",
		suggest: true,
		delay: 10,
		filter: "contains",
		index: 3,
		change: function (e) {

		}
	});

	$('#inputRack').closest('.k-widget').keydown(function (e) {
		if (e.keyCode == 13) {
		}
	});
}


function opcionHabilitarView(valor) {

	if (valor) {
		$('#FieldSetView').find('*').attr('disabled', true);
		$("#inputProyecto").data("kendoComboBox").enable(false);
		$('#RangoNuDiv').find('*').attr('disabled', true);
		$("#inputRack").data("kendoComboBox").enable(false);
		$("#Cuantificacion0003").text("Editar");


	}
	else {
		$('#FieldSetView').find('*').attr('disabled', false);
		$("#inputProyecto").data("kendoComboBox").enable(true);
		$('#RangoNuDiv').find('*').attr('disabled', false);
		$("#inputRack").data("kendoComboBox").enable(true);
		$("#Cuantificacion0003").text("Guardar");


	}
}

