function AjaxObtenerProyectos() {
	$Proyecto.Proyecto.read({ token: Cookies.get("token") }).done(function (result) {
		ControlErroresObjetosComboBox("inputProyecto", result);
	});
};

function AjaxCargarRacks(proyectoID) {
	$UbicacionNumeroUnico.UbicacionNumeroUnico.read({ token: Cookies.get("token"), proyectoID: proyectoID }).done(function (result) {
		ControlErroresObjetosComboBox("inputRack", result);
	});
}

function AjaxObtenerNumeroUnicoConRack(proyectoID, arrayNumeroUnico) {
	var cadenaNumerosUnicos = "";
	for (var i = 0; i < arrayNumeroUnico.length; i++) {
		cadenaNumerosUnicos += arrayNumeroUnico[i];
		if (i < arrayNumeroUnico.length - 1)
			cadenaNumerosUnicos += ",";
	}

	$UbicacionNumeroUnico.UbicacionNumeroUnico.read({ arrayNumeroUnico: cadenaNumerosUnicos, token: Cookies.get("token"), proyectoID: proyectoID }).done(function (data) {
		if (Error(data)) {
			//elementos no encontrados
			if (data[1].length > 0) {
				var cadena = "";
				for (var i = 0; i < data[1].length; i++) {
					cadena += data[1][i].NumeroUnicoID;
					if (i < data[1].length - 1)
						cadena += ", ";
				}
				displayMessage("", "Numeros unicos no encontrados :" + cadena, '2');
			}

			//elementos encontrados

			if (data[0].length > 0) {
				var ds = $("#grid").data("kendoGrid").dataSource;
				//$("#grid").data("kendoGrid").dataSource.data(data[0]);
				for (var i = 0; i < data[0].length; i++) {
					if (!existeCaptura(data[0][i]))
					ds.insert(0, data[0][i]);
				}
			}
			$("#inputRangoNumeroUnico").val("");

		}
	});
}

function existeCaptura(elemento)
{
	var ds = $("#grid").data("kendoGrid").dataSource;
	for (var i = 0; i < ds._data.length; i++) {
		if (ds._data[i].Consecutivo == elemento.Consecutivo)
			return true;
	}
	return false;
}

function AjaxGuardar(proyectoID, data, ranckID,esGuardaryNuevo) {
	var cadenaNumerosUnicos = "";
	for (var i = 0; i < data.length; i++) {
		cadenaNumerosUnicos += data[i].Consecutivo;
		if (i < data.length - 1)
			cadenaNumerosUnicos += ",";
	}

	Captura = [];
	Captura[0] = { Detalles: "" };

	ListaDetalle = [];
	
	
	for (var i = 0; i < data.length; i++) {
		ListaDetalle[i] = { Consecutivo: ""};
		ListaDetalle[i].Consecutivo = data[i].Consecutivo;
	}

	Captura[0].Detalles = ListaDetalle;



	$UbicacionNumeroUnico.UbicacionNumeroUnico.create(Captura[0], { token: Cookies.get("token"), proyectoID: proyectoID, rackID: ranckID }).done(function (data) {
		if (Error(data)) {
		
			
			displayMessage("", "Informacion guardada con exito", '0');
			if (esGuardaryNuevo)
			{
				$("#inputProyecto").data("kendoComboBox").value("");
				$("#inputRangoNumeroUnico").val("");
				$("#inputRack").data("kendoComboBox").value("");
				$("#grid").data("kendoGrid").dataSource.data([]);
			}
			else
			{
				opcionHabilitarView(true);
				$("#grid").data("kendoGrid").dataSource.data([]);
				$("#grid").data("kendoGrid").dataSource.data(data[0]);
			}
			
		}
	});
}