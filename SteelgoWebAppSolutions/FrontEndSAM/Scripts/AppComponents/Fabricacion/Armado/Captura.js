var endRangeDate;
var listadoJsonCaptura;
var anteriorlongitudTrabajosAdicionales;
var actuallongitudTrabajosAdicionales;
var ItemSeleccionado;
var ventanaConfirm;
var editado = false;
var esNormal;

IniciarCapturaArmado();
function IniciarCapturaArmado() {
	AltaFecha();
	asignarProyecto();
	setTimeout(function () { SuscribirEventos() }, 800);
}
function TryParseInt(str, defaultValue) {
	var retValue = defaultValue;
	if (str !== null) {
		if (str.length > 0) {
			if (!isNaN(str)) {
				retValue = parseInt(str);
			}
		}
	}
	return retValue;
}

function ObtenerCatalogos() {
	AjaxObtenerListaTubero();
	AjaxObtenerListaTaller();
}



function changeLanguageCall() {
	endRangeDate.data("kendoDatePicker").setOptions({
		format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
	});
	AjaxCargarCamposPredeterminados();
	CargarGrid();

	CargarGridPopUp();
	suscribirEventoWindowsConfirmaCaptura();
	//Limpiar();

	//opcionHabilitarView(false, "FieldSetView")
	document.title = _dictionary.CapturaArmadoBreadcrumb[$("#language").data("kendoDropDownList").value()];
}
function asignarProyecto() {
	//$("#InputOrdenTrabajo").val(Cookies.get('LetraProyecto') == undefined ? '' : Cookies.get('LetraProyecto'));
	$("#LabelProyecto").text('Proyecto :' + (Cookies.get('Proyecto') == undefined ? 'No hay ningun proyecto' : Cookies.get('Proyecto')));
}
function AltaFecha() {
	endRangeDate = $("#FechaArmado").kendoDatePicker({
		max: new Date(),
		//format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()],
		change: function (e) {
			ValidarFecha(e.sender._value)
		}
	});

	endRangeDate.on("keydown", function (e) {
		if (e.keyCode == 13) {
			ValidarFecha($("#FechaArmado").val());
		}
		//return false;
		if (e.keyCode == 9) {
			ValidarFecha($("#FechaArmado").data("kendoDatePicker").value());
		}
	});

	$("#FechaArmado").blur(function (e) {
		ValidarFecha($("#FechaArmado").data("kendoDatePicker").value());
	});

}
function ExisteJunta(Row) {
	var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;
	if ($('input:radio[name=TipoAgregado]:checked').val() == "Listado") {
		for (var i = 0; i < jsonGridArmado.length; i++) {
			if (Row.IdOrdenTrabajo + '-' + Row.IdVal == jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal && Row.JuntaID == jsonGridArmado[i].JuntaID) {
				return true;
			}
		}
	}
	return false;
}
function ExisteJuntaEnSpool(Row) {

	var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;

	for (var i = 0; i < jsonGridArmado.length; i++) {
		if (jsonGridArmado[i].JuntaID == (Row.JuntaID)) {
			return true;
		}
	}
	return false;
}
function ArregloListadoCaptura() {
	JsonCaptura = [];
	JsonCaptura[0] = { IDProyecto: "", Proyecto: "", IdOrdenTrabajo: "", OrdenTrabajo: "", IdVal: "", IdText: "", SpoolID: "", JuntaID: "", Junta: "", FechaArmado: "", TuberoID: "", Tubero: "", TallerID: "", Taller: "", SinCaptura: "" };
	//combobox.text()
	var fechaArmado = new Date($("#FechaArmado").data("kendoDatePicker").value());

	try {
		JsonCaptura[0].IDProyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID;
		JsonCaptura[0].Proyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Proyecto;
	} catch (e) {
		JsonCaptura[0].IDProyecto = dataSpoolArray.idStatus[spoolIDSelectTemp].ProyectoID;
		JsonCaptura[0].Proyecto = dataSpoolArray.idStatus[spoolIDSelectTemp].Proyecto;
	}

	JsonCaptura[0].IdOrdenTrabajo = $("#InputOrdenTrabajo").val();
	JsonCaptura[0].OrdenTrabajo = $("#InputOrdenTrabajo").val();
	JsonCaptura[0].IdVal = $("#InputID").val();
	JsonCaptura[0].IdText = $("#InputID").data("kendoComboBox").text();
	JsonCaptura[0].SpoolID = $("#InputOrdenTrabajo").val() + '-' + $("#InputID").val();
	JsonCaptura[0].JuntaID = $("#Junta").val() == "" ? "0" : $("#Junta").val();
	JsonCaptura[0].Junta = $("#Junta").data("kendoComboBox").text();
	JsonCaptura[0].FechaArmado = $("#FechaArmado").val();
	JsonCaptura[0].TuberoID = $("#inputTubero").val();
	JsonCaptura[0].Tubero = $("#inputTubero").data("kendoComboBox").text();
	JsonCaptura[0].TallerID = $("#inputTaller").val();
	JsonCaptura[0].Taller = $("#inputTaller").data("kendoComboBox").text();
	JsonCaptura[0].SinCaptura = $('input:radio[name=Muestra]:checked').val();

	return JsonCaptura[0];
}
function FiltroMostrar(mostrar) {
	var ds = $("#grid").data("kendoGrid").dataSource;

	if (mostrar == 0) {
		var curr_filters = ds.filter().filters;
		if (curr_filters[0].filters != undefined)
			ds.filter(curr_filters[0].filters[0])
		else
			ds.filter(curr_filters[0])
		ds.sync();
	}
	else {

		var curr_filters = ds.filter().filters;
		ds.filter(curr_filters[0])
		ds.sync();
		var filters = ds.filter();
		filters.logic = "or"

		filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
		filters.filters.push({ field: "Accion", operator: "eq", value: 4 });
		ds.sync();
	}
}
function CargarGrid() {

	$("#grid").kendoGrid({
		edit: function (e) {

			//if ($('#botonGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

			//var input = e.container.find(".k-input");
			//input.select();
			var inputName = e.container.find('input');

			inputName.select();
			//    var value = input.val();

			//    anteriorlongitudTrabajosAdicionales = e.model.ListaDetalleTrabajoAdicional.length;

			//    input.focus(function () {

			//        if (ItemSeleccionado.JuntaArmadoID !== 0)
			//        { ItemSeleccionado.Accion = 2; }
			//    });
			//} else
			//    this.closeCell();
			if ($('#botonGuardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
				this.closeCell();

			if ($(".k-grid-content td").css("white-space") == "normal") {
				esNormal = true;
			}
			else {
				esNormal = false;
			}

		},
		change: function () {
			ItemSeleccionado = this.dataSource.view()[this.select().index()];

			//alert('cambio');
		},
		dataSource: {
			schema: {
				model: {
					fields: {
						Accion: { type: "int", editable: false },
						IDProyecto: { type: "int", editable: false },
						SinCaptura: { type: "string", editable: false },
						Proyecto: { type: "string", editable: false },
						IdOrdenTrabajo: { type: "string", editable: false },
						OrdenTrabajo: { type: "string", editable: false },
						IdVal: { type: "string", editable: false },
						IdText: { type: "string", editable: false },
						SpoolID: { type: "string", editable: false },
						JuntaID: { type: "string", editable: false },
						Junta: { type: "string", editable: false },
						TipoJuntaID: { type: "int", editable: false },
						TipoJunta: { type: "string", editable: false },
						Diametro: { type: "number", editable: false },
						Cedula: { type: "string", editable: false },
						FechaArmado: { type: "date", editable: true },
						TuberoID: { type: "string", editable: true },
						Tubero: { type: "string", editable: true },
						TallerID: { type: "string", editable: true },
						Taller: { type: "string", editable: true },
						Localizacion: { type: "string", editable: false },
						FamiliaAcero: { type: "string", editable: false },
						NumeroUnico1ID: { type: "string", editable: true },
						NumeroUnico1: { type: "string", editable: true },
						NumeroUnico2ID: { type: "string", editable: true },
						NumeroUnico2: { type: "string", editable: true },
						TemplateMensajeTrabajosAdicionales: { type: "string", editable: true },
						InformacionDetalle: { type: "string", editable: false },
						DetalleJunta: { type: "string", editable: false }
					}
				}
			},
			filter: {
				logic: "or",
				filters: [
					{ field: "Accion", operator: "eq", value: 1 },
					{ field: "Accion", operator: "eq", value: 2 },
					{ field: "Accion", operator: "eq", value: 4 }
				]
			},
			pageSize: 10,
			serverPaging: false,
			serverFiltering: false,
			serverSorting: false
		},
		navigatable: true,
		editable: true,
		autoHeight: true,
		sortable: true,
		scrollable: true,
		selectable: true,
		pageable: {
			refresh: false,
			pageSizes: [10, 25, 50, 100],
			info: false,
			input: false,
			numeric: true
		},
		filterable: getGridFilterableMaftec(),
		columns: [
			{ field: "SpoolID", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
			{ field: "Junta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
			{ field: "DetalleJunta", title: _dictionary.columnDetalleJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
			{ field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "85px", attributes: { style: "text-align:right;" } },
			{ field: "FechaArmado", title: _dictionary.columnFecha[$("#language").data("kendoDropDownList").value()], filterable: { cell: { showOperators: false } }, editor: RenderDatePicker, width: "150px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
			{ field: "Tubero", title: _dictionary.columnTubero[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxTubero, width: "125px" },
			{ field: "Taller", title: _dictionary.columnTaller[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxTaller, width: "125px" },
			// { field: "LongitudMaterial1", title: 'Longitud NU1', filterable: false, width: "115px" },
			// { field: "LongitudMaterial2", title: 'Longitud NU2', filterable: false, width: "115px" },
			{ field: "NumeroUnico1", title: _dictionary.columnNumeroUnico1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxNumeroUnico1, width: "100px" },
			{ field: "NumeroUnico2", title: _dictionary.columnNumeroUnico2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxNumeroUnico2, width: "100px" },
			{ field: "InformacionDetalle", title: _dictionary.columnAdicionales[$("#language").data("kendoDropDownList").value()], filterable: false, width: "115px", template: "<div class='botonAdicionales'><a href='\\#'  > <span>#=TemplateMensajeTrabajosAdicionales#</span></a></div>" },

			{ command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } },
			{ command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }
		],
		dataBound: function () {
			var grid = $("#grid").data("kendoGrid");
			var gridData = grid.dataSource.view();

			for (var i = 0; i < gridData.length; i++) {
				var currentUid = gridData[i].uid;
				if (gridData[i].RowOk == false) {
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
					grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowErrorNU");
				}
				else if (gridData[i].RowOk) {
					if (i % 2 == 0)
						grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowErrorNU");
				}

				if (gridData[i].NUOk == false) {
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
					grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowErrorNU");
				}
				else if (gridData[i].NUOk) {
					if (i % 2 == 0)
						grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
					grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowErrorNU");
				}
			}

			if (esNormal) {
				$(".k-grid-content td").css("white-space", "normal");
			}
			else {
				$(".k-grid-content td").css("white-space", "nowrap");
			}

		}
	});
	CustomisaGrid($("#grid"));
}

function limpiarRenglon(e) {
	e.preventDefault();

	if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {
		AjaxListaDetalleTrabajosAdicionalesLimpiar($("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr")));

	}
}

function CargarGridPopUp() {

	$("#gridPopUp").kendoGrid({
		dataSource: {
			data: [],
			schema: {
				model: {
					fields: {

						Accion: { type: "int", editable: false },
						ArmadoTrabajoAdicionalID: { type: "string", editable: false },
						JuntaArmadoID: { type: "int", editable: false },
						TrabajoAdicionalID: { type: "int", editable: false },
						TrabajoAdicional: { type: "string", editable: true },
						ObreroID: { type: "int", editable: false },
						Tubero: { type: "int", editable: false },
						Observacion: { type: "string", editable: true }
					}
				}
			}, filter: {
				logic: "or",
				filters: [
					{ field: "Accion", operator: "eq", value: 1 },
					{ field: "Accion", operator: "eq", value: 2 },
					{ field: "Accion", operator: "eq", value: 0 },
					{ field: "Accion", operator: "eq", value: 4 },
					{ field: "Accion", operator: "eq", value: undefined }
				]
			}


		},
		selectable: true,
		filterable: getGridFilterableMaftec(),
		change: function (e) {
			grid = e.sender;
			var currentDataItem = grid.dataItem(this.select());
			var currentIndexRow = $("#gridPopUp").data("kendoGrid").items().index(this.select());
			// alert("evento change:" + currentIndexRow);
			console.log("evento change:" + currentIndexRow);
		},
		columns: [
			{ field: "TrabajoAdicional", title: _dictionary.columnAdicionales[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxTrabajoAdicional, filterable: getGridFilterableCellMaftecpopUp(), width: "150px" },
			{ field: "Observacion", title: _dictionary.columnObservacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "240px" },
			{
				command: {
					name: "",
					title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
					text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
					click: function (e) {
						e.preventDefault();
						var dataSource = this.dataSource;
						var dataItem = this.dataItem($(e.currentTarget).closest("tr"));


						if (dataItem.Accion == 1)
							dataSource.remove(dataItem);

						dataItem.Accion = 3;

						var filters = dataSource.filter();
						var allData = dataSource.data();
						var query = new kendo.data.Query(allData);
						var data = query.filter(filters).data;

						actuallongitudTrabajosAdicionales = data.length;

						if (actuallongitudTrabajosAdicionales == 0)
							modeloRenglon.TemplateMensajeTrabajosAdicionales = _dictionary.CapturaArmadoTemplateNoHayTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
						else
							modeloRenglon.TemplateMensajeTrabajosAdicionales = _dictionary.CapturaSoldaduraMensajeCambioLongitud[$("#language").data("kendoDropDownList").value()] + actuallongitudTrabajosAdicionales + _dictionary.CapturaSoldaduraMensajeCambioTrabajosAdicionales[$("#language").data("kendoDropDownList").value()];
						dataSource.sync();

					}, attributes: { style: "text-align:right;" }
				}, width: "50px", title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()]
			},
			{
				command: {
					name: "",
					title: "",
					text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()],
					click: function (e) {
						var itemToClean = $("#gridPopUp").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
						itemToClean.TrabajoAdicional = "";
						itemToClean.TrabajoAdicionalID = 0;
						itemToClean.Observacion = "";
						if (itemToClean.Accion == 2)
							itemToClean.Accion = 4;
						var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
						dataSource.sync();

					}, attributes: { style: "text-align:right;" }
				}, width: "50px", title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()]
			}
		],
		editable: true,
		navigatable: true,
		toolbar: [{ name: "create" }]
	});

	$("#gridPopUp table").on("keydown", function (e) {
		if (e.keyCode == 13) {
			e.preventDefault();
			//var grid = $("#gridPopUp").data("kendoGrid");
			//var length = grid.dataSource.view().length;
			//var currentIndexRow = $("#gridPopUp").data("kendoGrid").items().index($("#gridPopUp").data("kendoGrid").select());
			//if (currentIndexRow == length - 1) {
			var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
			var total = $("#gridPopUp").data("kendoGrid").dataSource.data().length;
			$("#gridPopUp").data("kendoGrid").dataSource.insert(total, {});
			$("#gridPopUp").data("kendoGrid").dataSource.page(dataSource.totalPages());
			$("#gridPopUp").data("kendoGrid").editRow($("#gridPopUp").data("kendoGrid").tbody.children().last());

			//}
		}
	});

	CustomisaGrid($("#gridPopUp"));
};
function LlenarGridPopUp(data) {
	$('#ContenedorGridPopUp').empty();
	$('#ContenedorGridPopUp').append($('<div id="gridPopUp" data-role="grid" class="k-grid k-widget">'));
	CargarGridPopUp();

	modeloRenglon = data;
	$("#gridPopUp").data('kendoGrid').dataSource.data([]);

	if (data.ListaDetalleTrabajoAdicional == undefined || data.ListaDetalleTrabajoAdicional == null)
		AjaxListaDetalleTrabajosAdicionales(data.JuntaID);
	else {
		var ds = $("#gridPopUp").data("kendoGrid").dataSource;
		var array = data.ListaDetalleTrabajoAdicional;
		if (array != null) {
			for (var i = 0; i < array.length; i++) {
				ds.add(array[i]);
			}
		}
		ds.sync();
	}

	VentanaModal();
}
function VentanaModal() {

	var modalTitle = "";
	modalTitle = _dictionary.CapturaSoldaduraHeaderAdicionales[$("#language").data("kendoDropDownList").value()];
	var window = $("#windowGrid");
	var win = window.kendoWindow({
		modal: true,
		title: modalTitle,
		resizable: false,
		visible: true,
		width: "40%",
		minWidth: 30,
		position: {
			top: "10%",
			left: "10%"
		},
		actions: [
		],
		close: function onClose(e) {
			var gridDataSource = $("#gridPopUp").data("kendoGrid").dataSource;
			gridDataSource.filter([]);

			//    gridDataSource.filter({ field: gridDataSource.options.fields[i].field, value: "" });


		}
	}).data("kendoWindow");
	window.data("kendoWindow").title(modalTitle);
	window.data("kendoWindow").center().open();

};
function AddRow(idTable) {
	var row = '';
	row += ' <tr>';
	row += '   <td><input style=" min-height:10px !important; height:auto;" placeholder="bisel" /></td>';
	row += '   <td><input style=" min-height:10px !important; height:auto;" placeholder="5" /></td> ';
	row += '   <td><input type="Button" class="deleteRow"  style=" width:10px; height:10px; background-image:url(../../Content/images/Delete_Grey.png); background-repeat:no-repeat; padding:0; margin:auto; background-position:center;"></td> ';
	row += ' </tr> ';

	$('#' + idTable).append(row);

	SuscribirEventoEliminar(idTable);
}
function AplicarAsignacionAutomaticaNumeroUnico(rowitem, textoAnterior, comboboxItemSeleccionado, posicionSiguiente, jsonGridArmado, cantidadItems) {
	if (seEncuentraEtiquetaMaterialOtrasJuntas(jsonGridArmado, comboboxItemSeleccionado.EtiquetaMaterial, rowitem)) {
		for (var i = 0; i < jsonGridArmado.length; i++) {
			//se evalua en NU1
			if (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) {
				if (jsonGridArmado[i].Localizacion.split('-')[0] == comboboxItemSeleccionado.EtiquetaMaterial) {
					jsonGridArmado[i].NumeroUnico1 = comboboxItemSeleccionado.Clave;
					jsonGridArmado[i].NumeroUnico1ID = comboboxItemSeleccionado.NumeroUnicoID;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = comboboxItemSeleccionado.JuntasEncontradas;
				}
				//se evalua en NU2
				else if (jsonGridArmado[i].Localizacion.split('-')[1] == comboboxItemSeleccionado.EtiquetaMaterial) {
					jsonGridArmado[i].NumeroUnico2 = comboboxItemSeleccionado.Clave;
					jsonGridArmado[i].NumeroUnico2ID = comboboxItemSeleccionado.NumeroUnicoID;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = comboboxItemSeleccionado.JuntasEncontradas;
				}
			}
		}

		SiguienteElemento = encontrarSiguienteItem(rowitem, comboboxItemSeleccionado);
		//fila del elemento encontrado,,numero unico autoseleccionado
		if (posicionSiguiente == 0 && SiguienteElemento != undefined && (SiguienteElemento[0] != undefined || SiguienteElemento[1] != undefined))
			AplicarAsignacionAutomaticaNumeroUnico(SiguienteElemento[0], textoAnterior, SiguienteElemento[1], 1, jsonGridArmado);
	}
	else {
		EliminarItemNUSeleccionado(jsonGridArmado, comboboxItemSeleccionado.NumeroUnicoID, rowitem);
		for (var i = 0; i < jsonGridArmado.length; i++) {
			//se evalua en NU1
			if (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) {
				if (jsonGridArmado[i].Localizacion.split('-')[0] == comboboxItemSeleccionado.EtiquetaMaterial) {
					jsonGridArmado[i].NumeroUnico1 = comboboxItemSeleccionado.Clave;
					jsonGridArmado[i].NumeroUnico1ID = comboboxItemSeleccionado.NumeroUnicoID;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = comboboxItemSeleccionado.JuntasEncontradas;
				}
				//se evalua en NU2
				else if (jsonGridArmado[i].Localizacion.split('-')[1] == comboboxItemSeleccionado.EtiquetaMaterial) {
					jsonGridArmado[i].NumeroUnico2 = comboboxItemSeleccionado.Clave;
					jsonGridArmado[i].NumeroUnico2ID = comboboxItemSeleccionado.NumeroUnicoID;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = comboboxItemSeleccionado.JuntasEncontradas;
				}
			}
		}
		SiguienteElemento = encontrarSiguienteItem(rowitem, comboboxItemSeleccionado);
		//fila del elemento encontrado,,numero unico autoseleccionado
		if (posicionSiguiente == 0 && cantidadItems == 3 && SiguienteElemento != undefined && SiguienteElemento[1] != undefined)
			AplicarAsignacionAutomaticaNumeroUnico(SiguienteElemento[0], textoAnterior, SiguienteElemento[1], 1, jsonGridArmado);
	}
}
function encontrarSiguienteItem(rowitem, comboboxItemSeleccionado) {
	var itemSiguienteMismoMaterial;
	var arrayListaNumerosUnicos;

	if (comboboxItemSeleccionado.Etiqueta == "2")
	{ arrayListaNumerosUnicos = rowitem.ListaNumerosUnicos2; }
	else if (comboboxItemSeleccionado.Etiqueta == "1")
	{ arrayListaNumerosUnicos = rowitem.ListaNumerosUnicos1; }

	for (var l = 0; l < arrayListaNumerosUnicos.length; l++) {
		if (comboboxItemSeleccionado.Clave != arrayListaNumerosUnicos[l].Clave && arrayListaNumerosUnicos[l].ItemCodeID != 0) {
			itemSiguienteMismoMaterial = arrayListaNumerosUnicos[l];
			return BuscarItemSiguienteEnGrid(itemSiguienteMismoMaterial);
		}
	}
	return [,];
}
function seEncuentraEtiquetaMaterialOtrasJuntas(jsonGridArmado, LocalizacionEtiquetaMaterial, rowitem) {
	var cantidadEtiquetaMaterialLocalizacionRepetido = 0;
	for (var i = 0; i < jsonGridArmado.length; i++) {
		for (var j = 0; j < jsonGridArmado[i].Localizacion.split('-').length; j++) {
			if (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal) && jsonGridArmado[i].Localizacion.split('-')[j] == LocalizacionEtiquetaMaterial)
				cantidadEtiquetaMaterialLocalizacionRepetido++;
		}
	};

	if (cantidadEtiquetaMaterialLocalizacionRepetido > 1)
		return true;
	return false;
};
function AplicarLimpiarAsignacionAutomatica(rowitem, textoAnterior, combobox, posicionSiguiente) {
	var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;

	//se asigna datos a nivel Etiqueta. NU1 NU2 1,1 -> 3,3 
	for (var i = 0; i < jsonGridArmado.length; i++) {
		if (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) {

			for (var j = 0; j < jsonGridArmado[i].ListaNumerosUnicos1.length; j++) {
				if (combobox.EtiquetaMaterial == jsonGridArmado[i].ListaNumerosUnicos1[j].EtiquetaMaterial) {
					jsonGridArmado[i].NumeroUnico1 = combobox.Clave;
					jsonGridArmado[i].NumeroUnico1ID = combobox.NumeroUnicoID;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = combobox.JuntasEncontradas;
				}
			}
			for (var k = 0; k < jsonGridArmado[i].ListaNumerosUnicos2.length; k++) {
				if (combobox.EtiquetaMaterial == jsonGridArmado[i].ListaNumerosUnicos2[k].EtiquetaMaterial) {
					jsonGridArmado[i].NumeroUnico2 = combobox.Clave;
					jsonGridArmado[i].NumeroUnico2ID = combobox.NumeroUnicoID;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = combobox.JuntasEncontradas;
				}
			}

		}
	}

	if (ExisteNUGrid(combobox.NumeroUnicoID, jsonGridArmado, rowitem)) {
		//proceso para borrar el dato seleccionado donde se encuentra y ponerlo en el actual.
		EliminarItemNUSeleccionado(jsonGridArmado, combobox.NumeroUnicoID, rowitem)
	}
	else {
		if (!ExisteSpoolJuntaEnGrid(combobox, jsonGridArmado, rowitem)) {
			for (var i = 0; i < jsonGridArmado.length; i++) {
				if (combobox.JuntasEncontradas != '' &&
					((jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) &&
					(jsonGridArmado[i].NumeroUnico2ID == combobox.NumeroUnicoID)) {
					jsonGridArmado[i].NumeroUnico2 = '';
					jsonGridArmado[i].NumeroUnico2ID = null;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = rowitem.Junta;
					MensajesSteelGO("AvisoNumeroUnicoYaAsignado", combobox.JuntasEncontradas);
				}
				else if (combobox.JuntasEncontradas != '' && ((jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) && (jsonGridArmado[i].NumeroUnico1ID == combobox.NumeroUnicoID)) {
					jsonGridArmado[i].NumeroUnico1 = '';
					jsonGridArmado[i].NumeroUnico1ID = null;
					jsonGridArmado[i].JuntaAnteriorNumeroUnicoGuardado = rowitem.Junta;
					MensajesSteelGO("AvisoNumeroUnicoYaAsignado", combobox.JuntasEncontradas);
				}
			}
		}
	}
};
function elNUSeEncuentraEnJuntasNoAgregadasGrid(combobox, jsonGridArmado, rowitem) {

	var juntasEncontradasGuardadas = combobox.JuntasEncontradas == "" ? 0 : combobox.JuntasEncontradas.split(',');
	var cantidadJuntasEncontradasGrid = 0;;

	if (combobox.JuntasEncontradas == "")
		return false;
	else {
		for (var i = 0; i < jsonGridArmado.length; i++) {
			for (var j = 0; j < juntasEncontradasGuardadas.length; j++) {
				if (jsonGridArmado[i].Accion != 3 && (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal + jsonGridArmado[i].Junta) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal + juntasEncontradasGuardadas[j].trim())) {
					cantidadJuntasEncontradasGrid++;
				}
			}
		}
	}

	if (juntasEncontradasGuardadas.length == cantidadJuntasEncontradasGrid)
		return false;
	return true;
};
function EliminarItemNUSeleccionado(jsonGridArmado, NumeroUnicoID, rowitem) {
	for (var i = 0; i < jsonGridArmado.length; i++) {
		if (jsonGridArmado[i].NumeroUnico1ID == NumeroUnicoID && (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal) && (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal + jsonGridArmado[i].JuntaID) != (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal + rowitem.JuntaID)) {
			jsonGridArmado[i].NumeroUnico1 = '';
			jsonGridArmado[i].NumeroUnico1ID = null;
		}
		if (jsonGridArmado[i].NumeroUnico2ID == NumeroUnicoID && (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal) && (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal + jsonGridArmado[i].JuntaID) != (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal + rowitem.JuntaID)) {
			jsonGridArmado[i].NumeroUnico2 = '';
			jsonGridArmado[i].NumeroUnico2ID = null;
		}
	}
}
function ExisteNUGrid(NumeroUnicoID, jsonGridArmado, rowitem) {
	var existe = false;
	for (var i = 0; i < jsonGridArmado.length; i++) {
		if ((jsonGridArmado[i].Accion != 3 &&
			(jsonGridArmado[i].NumeroUnico1ID == NumeroUnicoID || jsonGridArmado[i].NumeroUnico2ID == NumeroUnicoID)) &&
			(jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal) &&
			jsonGridArmado[i].JuntaID != rowitem.JuntaID)
			return true;
	}
	return false;
}
function DatoDefaultNumeroUnico1()
{ }
function DatoDefaultNumeroUnico2()
{ }
function BuscarItemSiguienteEnGrid(siguienteItemBuscar) {
	var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;


	//se busca el nuevo item en alguna junta con el mismo EtiquetaMaterial .
	for (var i = 0; i < jsonGridArmado.length; i++) {
		for (var j = 0; j < jsonGridArmado[i].ListaNumerosUnicos1.length; j++) {
			if (jsonGridArmado[i].ListaNumerosUnicos1[j].Clave == siguienteItemBuscar.Clave && jsonGridArmado[i].ListaNumerosUnicos1[j].EtiquetaMaterial == siguienteItemBuscar.EtiquetaMaterial && jsonGridArmado[i].NumeroUnico1 == undefined)
			{ return [jsonGridArmado[i], jsonGridArmado[i].ListaNumerosUnicos1[j]]; }
		}

		for (var j = 0; j < jsonGridArmado[i].ListaNumerosUnicos2.length; j++) {
			if (jsonGridArmado[i].ListaNumerosUnicos2[j].Clave == siguienteItemBuscar.Clave && jsonGridArmado[i].ListaNumerosUnicos2[j].EtiquetaMaterial == siguienteItemBuscar.EtiquetaMaterial && jsonGridArmado[i].NumeroUnico2 == undefined)
			{ return [jsonGridArmado[i], jsonGridArmado[i].ListaNumerosUnicos2[j]]; }
		}
	}

	//se busca el nuevo item en alguna junta con diferente EtiquetaMaterial .
	for (var i = 0; i < jsonGridArmado.length; i++) {
		for (var j = 0; j < jsonGridArmado[i].ListaNumerosUnicos1.length; j++) {
			if (jsonGridArmado[i].ListaNumerosUnicos1[j].Clave == siguienteItemBuscar.Clave && jsonGridArmado[i].ListaNumerosUnicos1[j].EtiquetaMaterial != siguienteItemBuscar.EtiquetaMaterial)
			{ return [jsonGridArmado[i], jsonGridArmado[i].ListaNumerosUnicos1[j]]; }
		}

		for (var j = 0; j < jsonGridArmado[i].ListaNumerosUnicos2.length; j++) {
			if (jsonGridArmado[i].ListaNumerosUnicos2[j].Clave == siguienteItemBuscar.Clave && jsonGridArmado[i].ListaNumerosUnicos2[j].EtiquetaMaterial != siguienteItemBuscar.EtiquetaMaterial)
			{ return [jsonGridArmado[i], jsonGridArmado[i].ListaNumerosUnicos2[j]]; }
		}
	}

	//en caso de no encontrar nada
	return undefined;
}
function agregarFila(idGrid) {
	var grid = $("#" + idGrid).data("kendoGrid");

}
function eliminarCaptura(e) {
	e.preventDefault();
	if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

		var filterValue = $(e.currentTarget).val();
		var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
		var spoolIDRegistro = dataItem.SpoolID;

		//var JuntaBorrada = dataItem.Junta;
		//var juntasAnteriores = "";
		//var eliminarFila = false;

		//for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
		//    if ($("#grid").data("kendoGrid").dataSource._data[i].IdOrdenTrabajo + '-' + $("#grid").data("kendoGrid").dataSource._data[i].IdVal == (dataItem.IdOrdenTrabajo + '-' + dataItem.IdVal))
		//    {
		//        if(juntasAnteriores=="")
		//            juntasAnteriores = $("#grid").data("kendoGrid").dataSource._data[i].JuntaAnteriorNumeroUnicoGuardado == null ? "" : $("#grid").data("kendoGrid").dataSource._data[i].JuntaAnteriorNumeroUnicoGuardado;
		//        else
		//            juntasAnteriores += "," +( $("#grid").data("kendoGrid").dataSource._data[i].JuntaAnteriorNumeroUnicoGuardado == null ? "" : $("#grid").data("kendoGrid").dataSource._data[i].JuntaAnteriorNumeroUnicoGuardado);
		//    }

		//}

		//for (var i = 0; i < juntasAnteriores.split(',').length; i++) {
		//    if (juntasAnteriores.split(',')[i].trim() == JuntaBorrada)
		//    {
		//        eliminarFila = true;
		//        break;
		//    }
		//}

		var dataSource = $("#grid").data("kendoGrid").dataSource;

		//if (!eliminarFila) {
		dataSource.remove(dataItem);
		dataSource.sync();
		//}
		//else {
		//    displayNotify("CapturaArmadoMensajeEliminarJuntaIncorrectaPorNU", "", '1');
		//}



	}

}
function PlanchaTubero() {
	var dataSource = $("#grid").data("kendoGrid").dataSource;
	var filters = dataSource.filter();
	var allData = dataSource.data();
	var query = new kendo.data.Query(allData);
	var data = query.filter(filters).data;
	if ($("#inputTubero").data("kendoComboBox").text() != "") {
		for (var i = 0; i < data.length; i++) {
			if ($('input:radio[name=LLena]:checked').val() === "Todos") {
				data[i].TuberoID = $("#inputTubero").val();
				data[i].Tubero = $("#inputTubero").data("kendoComboBox").text();
			}
			else {
				if (data[i].Tubero === "" || data[i].Tubero === null || data[i].Tubero === undefined) {
					data[i].TuberoID = $("#inputTubero").val();
					data[i].Tubero = $("#inputTubero").data("kendoComboBox").text();
				}
			}
		}
	}
	$("#grid").data("kendoGrid").dataSource.sync();
}
function PlanchaTaller() {
	var dataSource = $("#grid").data("kendoGrid").dataSource;
	var filters = dataSource.filter();
	var allData = dataSource.data();
	var query = new kendo.data.Query(allData);
	var data = query.filter(filters).data;
	if ($("#inputTaller").data("kendoComboBox").text() != "") {
		for (var i = 0; i < data.length; i++) {

			if ($('input:radio[name=LLena]:checked').val() === "Todos") {
				data[i].TallerID = $("#inputTaller").val();
				data[i].Taller = $("#inputTaller").data("kendoComboBox").text();
			}
			else {
				if (data[i].Taller === "" || data[i].Taller === null || data[i].Taller === undefined) {
					data[i].TallerID = $("#inputTaller").val();
					data[i].Taller = $("#inputTaller").data("kendoComboBox").text();
				}
			}
		}
	}
	$("#grid").data("kendoGrid").dataSource.sync();
}
function PlanchaFecha() {
	var dataSource = $("#grid").data("kendoGrid").dataSource;
	var filters = dataSource.filter();
	var allData = dataSource.data();
	var query = new kendo.data.Query(allData);
	var data = query.filter(filters).data;
	if (endRangeDate.val() != "") {
		for (var i = 0; i < data.length; i++) {
			if ($('input:radio[name=LLena]:checked').val() === "Todos") {
				//data[i].FechaArmado = String(endRangeDate.val()).trim();
				data[i].FechaArmado = new Date(ObtenerDato(endRangeDate.val(), 1), ObtenerDato(endRangeDate.val(), 2), ObtenerDato(endRangeDate.val(), 3));//año, mes, dia
			}
			else {
				if (data[i].FechaArmado === "" || data[i].FechaArmado === null || data[i].FechaArmado === undefined) {
					//data[i].FechaArmado = String(endRangeDate.val()).trim();
					data[i].FechaArmado = new Date(ObtenerDato(endRangeDate.val(), 1), ObtenerDato(endRangeDate.val(), 2), ObtenerDato(endRangeDate.val(), 3));//año, mes, dia
				}
			}
		}
	}
	$("#grid").data("kendoGrid").dataSource.sync();
}
function ObtenerDato(fecha, tipoDatoObtener) {
	var cultura = $("#language").val();

	switch (tipoDatoObtener) {
		case 1://anho
			return fecha.split('/')[2]
			break;
		case 2://mes
			if (cultura == 'es-MX')
				return fecha.split('/')[1] - 1
			else
				return fecha.split('/')[0] - 1
			break;
		case 3://dia
			if (cultura == 'es-MX')
				return fecha.split('/')[0]
			else
				return fecha.split('/')[1]
			break;
	}
}
function ArregloListadoReporte() {
	JsonCaptura = [];
	var lista = $("#Junta").data("kendoComboBox").dataSource._data;

	for (var i = 0; i < lista.length; i++) {
		JsonCaptura[i] = { IDProyecto: "", Proyecto: "", IdOrdenTrabajo: "", OrdenTrabajo: "", idVal: "", idText: "", SpoolID: "", JuntaID: "", Junta: "", FechaArmado: "", TuberoID: "", Tubero: "", TallerID: "", Taller: "", sinCaptura: "" };
		try {
			JsonCaptura[i].IDProyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID;
			JsonCaptura[i].Proyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Proyecto;
			JsonCaptura[i].idVal = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Valor;//$("#InputID").val();
		} catch (e) {
			JsonCaptura[i].IDProyecto = dataSpoolArray.idStatus[spoolIDSelectTemp].ProyectoID;
			JsonCaptura[i].Proyecto = dataSpoolArray.idStatus[spoolIDSelectTemp].Proyecto;
			JsonCaptura[i].idVal = dataSpoolArray.idStatus[spoolIDSelectTemp].Valor;
		}
		JsonCaptura[i].IdOrdenTrabajo = $("#InputOrdenTrabajo").val();
		JsonCaptura[i].OrdenTrabajo = $("#InputOrdenTrabajo").val();

		JsonCaptura[i].idText = $("#InputID").data("kendoComboBox").text();
		JsonCaptura[i].SpoolID = $("#InputOrdenTrabajo").val() + '-' + $("#InputID").val();
		JsonCaptura[i].JuntaID = lista[i].JuntaSpoolID;
		JsonCaptura[i].Junta = lista[i].Etiqueta;
		JsonCaptura[i].FechaArmado = kendo.toString($("#FechaArmado").val(), _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
		JsonCaptura[i].TuberoID = $("#inputTubero").val();
		JsonCaptura[i].Tubero = $("#inputTubero").data("kendoComboBox").text();
		JsonCaptura[i].TallerID = $("#inputTaller").val();
		JsonCaptura[i].Taller = $("#inputTaller").data("kendoComboBox").text();
		JsonCaptura[i].sinCaptura = "Todos";
	}
	return JsonCaptura;
}
function tieneClase(item) {

	var tieneClass = $(item).hasClass("k-state-border-up") || $(item).hasClass("k-state-border-down");
	return tieneClass;
}
function ArregloListadoJuntasCapturadas() {

	var dataSource = $("#grid").data("kendoGrid").dataSource;
	var data = dataSource._data
	JsonCaptura = [];
	var index = 0;
	for (var i = 0; i < data.length; i++) {

		JsonCaptura[index] = { OrdenTrabajoSpoolID: "", JuntaID: "" };
		JsonCaptura[index].OrdenTrabajoSpoolID = data[i].IdVal;
		JsonCaptura[index].JuntaID = data[i].JuntaID;
		index++;

	}
	return JsonCaptura;
}
function ValidarFecha(valor) {
	var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
	if (fecha == null) {
		$("#FechaArmado").data("kendoDatePicker").value('');
	}
}
function MensajesSteelGO(control, mensajeExepcionTecnico) {

	switch (control) {

		case 'InputOrdenTrabajo':// el InputOrdenTrabajo no tiene el formato correcto.
			displayNotify("OrdenTrabajoNoValida", "", '1');
			break;
		case 'Mensajes_error':
			//displayNotify("Mensajes_error", mensajeExepcionTecnico, '2');//muestra cualquier error indicando el error tecnico al usuario
			break;
		case 'InputID-SelectInvalid':
			displayNotify("NoExisteSpoolID", '', '2');//mensaje indicando que el id no es valido.
			break;
		case 'radioMostrar':
			displayNotify("radioMostrar", '', '2');//mensaje cuando el tipo de datos a Mostrar no se encuentre seleccionado
			break;
		case 'LLenadoMasivo':
			displayNotify("radioLLenadoMasivo", '', '2');//mensaje cuando el tipo de llenado masivo no esta seleccionado
			break;
		case 'ResultadoDimensional':
			displayNotify("radioResultadoDimensional", '', '2');//mensaje cuando el tipo de resultado dimensional no esta seleccionado
			break;
		case 'radioTipoAgregado':
			displayNotify("radioTipoAgregado", '', '2');//mensaje cuando el tipo de resultado dimensional no esta seleccionado
			break;
		case 'AvisoNumeroUnicoYaAsignado':
			displayNotify('', _dictionary.AvisoNumeroUnicoYaAsignado[$("#language").data("kendoDropDownList").value()].replace("?1", mensajeExepcionTecnico), '2');//mensaje cuando el numero unico ya se encuentra asignado.
			break;

	}
};
function ObjetoBorrado(item) {
	JsonCaptura = [];
	JsonCaptura[0] = { IDProyecto: "", Proyecto: "", IdOrdenTrabajo: "", OrdenTrabajo: "", IdVal: "", IdText: "", SpoolID: "", JuntaID: "", Junta: "", FechaArmado: "", TuberoID: "", Tubero: "", TallerID: "", Taller: "", SinCaptura: "" };

	JsonCaptura[0].IDProyecto = item.IDProyecto;
	JsonCaptura[0].Proyecto = item.Proyecto;
	JsonCaptura[0].IdOrdenTrabajo = item.IdOrdenTrabajo;
	JsonCaptura[0].OrdenTrabajo = item.OrdenTrabajo;
	JsonCaptura[0].IdVal = item.IdVal;
	JsonCaptura[0].IdText = item.IdText;
	JsonCaptura[0].SpoolID = item.SpoolID;
	JsonCaptura[0].JuntaID = item.JuntaID;
	JsonCaptura[0].Junta = item.Junta;
	JsonCaptura[0].FechaArmado = item.FechaArmado;
	JsonCaptura[0].TuberoID = item.TuberoID;
	JsonCaptura[0].Tubero = item.Tubero;
	JsonCaptura[0].TallerID = item.TallerID;
	JsonCaptura[0].Taller = item.Taller;
	JsonCaptura[0].SinCaptura = item.SinCaptura;

	return JsonCaptura[0];
}
function esCorrectaJunta(juntas) {
	if (juntas != null) {
		var arrayJuntas = juntas.split(',');
		var data = $("#grid").data("kendoGrid").dataSource._data;

		for (var i = 0; i < data.length; i++) {
			for (var j = 0; j < arrayJuntas.length; j++) {
				if ((data[i].FechaArmado == "" ||
					(data[i].TallerID == "" || data[i].TallerID == "0") ||
					(data[i].TuberoID == "" || data[i].TuberoID == "0") ||
					(data[i].NumeroUnico1ID == "" || data[i].NumeroUnico1ID == null || data[i].NumeroUnico1ID == "0") ||
					(data[i].NumeroUnico2ID == "" || data[i].NumeroUnico2ID == null || data[i].NumeroUnico2ID == "0")) &&
					data[i].Junta == arrayJuntas[j].trim())
					return false;
			}
		}
		return true;
	}
	return true;
}