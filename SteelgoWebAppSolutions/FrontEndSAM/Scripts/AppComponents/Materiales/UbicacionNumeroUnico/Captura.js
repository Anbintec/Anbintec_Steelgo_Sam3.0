function changeLanguageCall() {
	SuscribirEventos();
	AjaxObtenerProyectos();
	LoadGrid();
	$('input:radio[name=Mostrar]:nth(0)').prop("checked", true);
	$("#styleUno").addClass("active");
};

function agregarNU()
{
	if (validarRequeridosAgregar()) {
		if (validarCaracteresRango($("#inputRangoNumeroUnico").val())) {
			var rangoNumerosUnicos = $("#inputRangoNumeroUnico").val();
			var arrayNumerosUnicosBuscar = rangoNumerosUnicos.split(",");
			var cadena = "";
			var intervalo;
			var rangoIncorrecto = false;
			var intevaloInicial;
			var intevaloFinal;
			var subArray;
			var arrayNumerosUnicos = [];

			for (var i = 0; i < arrayNumerosUnicosBuscar.length; i++) {
				intervalo = arrayNumerosUnicosBuscar[i].split("-");
				if (intervalo.length == 1 && intervalo[0].trim() != "") {
					arrayNumerosUnicos.push(intervalo[0]);
				}
				else if (intervalo.length == 2) {
					intevaloInicial = parseInt(intervalo[0]);
					intevaloFinal = parseInt(intervalo[1]);
					if (intevaloInicial > intevaloFinal) {
						rangoIncorrecto = true;
						arrayNumerosUnicos = [];
						displayMessage("", "El rango de numeros unicos es incorrecto", '1');
						break;
					}
					else {
						//for (var j = intevaloInicial; j <= intevaloFinal; j++) {
						//	arrayNumerosUnicos.push(j);
						//}
						arrayNumerosUnicos.push(arrayNumerosUnicosBuscar[i]);
					}
				}
				else {
					rangoIncorrecto = true;
					arrayNumerosUnicos = [];
					displayMessage("", "El rango de numeros unicos es incorrecto", '1');
					break;

				}
			}
			//console.log(arrayNumerosUnicos);

			//for (var i = 0; i < $("#grid").data("kendoGrid").dataSource._data.length; i++) {
			//	arrayNumerosUnicos.push($("#grid").data("kendoGrid").dataSource._data[i].Consecutivo);
			//}

			if (!rangoIncorrecto) {
				var proyectoID = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID;
				//var rackID = $("#inputRack").data("kendoComboBox").dataItem($("#inputRack").data("kendoComboBox").select()).RackID;
				AjaxObtenerNumeroUnicoConRack(proyectoID, arrayNumerosUnicos);
			}
		}
		else
			displayMessage("", "El rango de numeros unicos es incorrecto", '1');

	} else {
		displayMessage("notificationslabel0031", "", '1');
	}
}

function getGridFilterableCellNumberMaftec() {
	return {
		extra: true,
		cell: {
			operator: "equals",
			template: function (args) {
				$(args).prop('type', 'number');
				args.css("width", "95%").addClass("general-input").keydown(function (e) {
					setTimeout(function () {
						$(e.target).trigger("change");
					});
				});
			},
			showOperators: false
		}
	}
}

function getGridFilterableCellMaftec() {
	return {
		cell: {
			operator: "contains",
			template: function (args) {
				args.css("width", "95%").addClass("general-input").keydown(function (e) {
					setTimeout(function () {
						$(e.target).trigger("change");
					});
				});
			},
			showOperators: false
		}
	}
}

function getGridFilterableMaftec(val) {
	return {
		mode: "menu, row",
		extra: false,
		operators: {
			string: {
				startswith: _dictionary.KendoGridFilterable0001[$("#language").data("kendoDropDownList").value()],
				eq: _dictionary.KendoGridFilterable0002[$("#language").data("kendoDropDownList").value()],
				neq: _dictionary.KendoGridFilterable0003[$("#language").data("kendoDropDownList").value()],
			},
		},
		cell: {
			showOperators: false,
			operator: "contains"
		}
	}
}

function validarCaracteresRango(cadena) {
	var caracterInvalido = false;

	for (var i = 0; i < cadena.length; i++) {
		if (!isInt(cadena[i]) && (cadena[i] != "," && cadena[i] != "-")) {
			caracterInvalido = true;
			break;
		}
	}
	return !caracterInvalido;
}

function isInt(value) {
	return !isNaN(value) && (function (x) { return (x | 0) === x; })(parseFloat(value))
}

function ControlErroresObjetosComboBox(control, result) {
	if (Error(result)) {
		$("#" + control).data("kendoComboBox").dataSource.data(result);
	} else {
		$("#" + control).data("kendoComboBox").dataSource.data([]);
	};
};

function Error(data) {
	if (data.ReturnCode) {
		if (data.ReturnCode != 200) {
			if (data.ReturnCode == 401) {
				removeUserSession();
				return false;
			} else {
				displayMessage("notificationslabel0008", data.ReturnMessage, '2');
				return false;
			}
		} else {
			return true;
		}
	} else {
		return true;
	}
};

function validarRequeridosAgregar() {
	var bool = true;
	$("#formUbicacionNumeroUnico .security_required").each(function (i, elem) {
		if (elem.tagName.toLowerCase() == 'input') {
			if (!$(this).val()) {
				bool = false;
				$(this).closest("div").find("label").addClass("error");
			} else {
				$(this).closest("div").find("label").removeClass("error");
			};
		};
	});
	return bool;
};

function LoadGrid() {
	$("#grid").kendoGrid({
		dataSource: {
			autoSync: true,
			schema: {
				model: {
					fields: {
						Consecutivo: { type: "number", editable: false },
						ItemCode: { type: "string", editable: false },
						DescipcionItemCode: { type: "string", editable: false},
						D1: { type: "number", editable: false },
						D2: { type: "number", editable: false },
						Rack: { type: "string", editable: false }
					}
				}
			},
			serverPaging: false,
			serverFiltering: false,
			serverSorting: false
		},
		filterable: getGridFilterableMaftec(),
		autoHeight: true,
		sortable: true,
		scrollable: true,
		pageable: false,
		selectable: true,
		editable: true,
		navigatable: true,
		columns: [
			{ field: "Consecutivo", title: "Número Único", /*filterable: true,*/ width: "150px", filterable: getGridFilterableCellNumberMaftec() },
			{ field: "ItemCode", title: _dictionary.Cuantificacion0028[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "150px" },
			{ field: "DescipcionItemCode", title: _dictionary.Cuantificacion0029[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "150px" },
			{ field: "D1", title: _dictionary.Cuantificacion0030[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "150px" },
			{ field: "D2", title: _dictionary.Cuantificacion0031[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "150px" },
			{ field: "Rack", title: "Rack Actual", filterable: getGridFilterableCellMaftec(), width: "200px" },
			{ command: { text: _dictionary.Cuantificacion0043[$("#language").data("kendoDropDownList").value()], click: CancelarInformacionItem }, title: "Eliminar", width: "99px" }
		],
		height: 411,
        width:100
	});
}

function CancelarInformacionItem(e) {

	if ($('#Cuantificacion0003').text() == "Guardar") {
		var grid = $("#grid").data("kendoGrid");
		var tr = $(e.currentTarget).closest("tr");
		grid.removeRow(tr);
	}


	
}