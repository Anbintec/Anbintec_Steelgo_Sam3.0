
var ventanaConfirmEdicionCaptura;
var editado=false;
var disableDates;
var gridRow;
var LineaCaptura = { ProyectoIDSeleccionado: "", ProcesoIDSeleccionado: "", SistemaPinturaIDSeleccionado: "",ColorIDSeleccionado:"", PruebaIDSeleccionado: "",FechaSeleccionada:"",LoteIDSeleccionada:"" }
var ventanaConfirmEdicionSinTipoBusqueda;
var EjecutaChange = 0;


function isInArray(date, dates) {
	for (var idx = 0, length = dates.length; idx < length; idx++) {
		var d = dates[idx];
		if (date.getFullYear() == d.getFullYear() &&
			date.getMonth() == d.getMonth() &&
			date.getDate() == d.getDate()) {
			return true;
		}
	}

	return false;
}

function sumarPruebasTotalesPorSpool()
{
    var numeroPruebas = 0;
    var rows = $("#gridPopUp").data('kendoGrid').dataSource._data;
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Accion == undefined || rows[i].Accion == 1 || rows[i].Accion == 2 || rows[i].Accion == 0)
        numeroPruebas += 1;
    }
    return numeroPruebas;
}

function sumarPruebasTotales() {
    var numeroPruebas = 0;
    var rows = $("#grid").data('kendoGrid').dataSource._data;
    for (var i = 0; i < rows.length; i++) {
            numeroPruebas += rows[i].PruebasEjecutadas;
    }

    $("#labelPruebasEjecutadas").text(numeroPruebas);
}

function Limpiar() {
    $("#labelPruebasEjecutadas").text("");
    $("#labelPruebasRequeridas").text("");
    $("#inputProyecto").data("kendoComboBox").value("");

    $("#inputProceso").data("kendoComboBox").dataSource.data([]);
    $("#inputProceso").data("kendoComboBox").value("");

    $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputSistemaPintura").data("kendoComboBox").value("");

    $("#inputColor").data("kendoComboBox").dataSource.data([]);
    $("#inputColor").data("kendoComboBox").value("");

    $("#inputPrueba").data("kendoComboBox").dataSource.data([]);
    $("#inputPrueba").data("kendoComboBox").value("");

    $("#inputFechaLote").data("kendoDatePicker").value("");


    $("#inputLote").data("kendoComboBox").dataSource.data([]);
    $("#inputLote").data("kendoComboBox").value("");

    $("#grid").data("kendoGrid").dataSource.data([]);

}


function LimpiarDespuesCambioCaptura() {
	$("#grid").data('kendoGrid').dataSource.data([]);
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

function changeLanguageCall() {
	SuscribirEventos();//validar porque tantas veces se cambie el lenguaje se generarian los eventos.
	AjaxCargarProyecto();
	CargarGrid();
	CargarGridPopUp();
	AjaxCargarCamposPredeterminados();
	//$("#inputFechaLote").data("kendoDatePicker").setOptions({
	//	format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
	//});
}

function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();

        },
        autoBind: true,
        dataSource: {
            schema: {
                model: {
                    fields: {
                        PruebaLoteID: { type: "number", editable: false },
                        Accion: { type: "number", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        SistemaPintura: { type: "string", editable: false },
                        Color: { type: "string", editable: false },
                        Area: { type: "String", editable: false },
                        PruebasRequeridas: { type: "number", editable: false },
                        PruebasEjecutadas: { type: "number", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Template: { type: "string", editable: false },
                        Medida: { type: "string", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        filterable: {
            extra: false
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "SistemaPintura", title: _dictionary.columnSistemaPintura[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Color", title: _dictionary.columnColor[$("#language").data("kendoDropDownList").value()], width: "150px", filterable: getGridFilterableCellMaftec(), attributes: { style: "text-align:center;" } },
            { field: "PruebasEjecutadas", title: _dictionary.columnPruebasEjecutadas[$("#language").data("kendoDropDownList").value()], width: "150px", filterable: getGridFilterableCellNumberMaftec() },
            { field: "Area", title: _dictionary.columnM2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), format: "{0:n2}", width: "95px", attributes: { style: "text-align:right;" } },
            { field: "Cuadrante", title: _dictionary.columnCuadrante[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec() },
            { field: "Template", title: _dictionary.columnSeRealizoPrueba[$("#language").data("kendoDropDownList").value()], filterable: false, template: "<div class='EnlaceDetallePrueba' style='text-align:center;'><a href='\\#'  > <span>#=Template#</span></a></div>", filterable: false, width: "190px" }
        ],
            dataBound: function () {
                var grid = $("#grid").data("kendoGrid");
                var gridData = grid.dataSource.view();

                for (var i = 0; i < gridData.length; i++) {
                    var currentUid = gridData[i].uid;
                    if (gridData[i].RowOk == false) {
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                        grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
                     
                    }
                    else if (gridData[i].RowOk) {
                        if (i % 2 == 0)
                            grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                    }
                }
            }
    });
    CustomisaGrid($("#grid"));
}

function CargarGridPopUp() {

    $("#gridPopUp").kendoGrid({
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();

        },
        autoBind: true,
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Accion: { type: "number", editable: false },
                        SpoolID: { type: "number", editable: false },
                        ProyectoProcesoPruebaID: { type: "number", editable: false },
                        UnidadMaxima: { type: "number", editable: false },
                        UnidadMinima: { type: "number", editable: false },
                        Medida: { type: "string", editable: false },
                        FechaPrueba: { type: "date", editable: true },
                        UnidadMedida: { type: "number", editable: true },
                        ResultadoEvaluacion: { type: "boolean", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
					{ field: "Accion", operator: "eq", value: 1 },
					{ field: "Accion", operator: "eq", value: 2 },
					{ field: "Accion", operator: "eq", value: 4 },
                    { field: "Accion", operator: "eq", value: undefined },
                    { field: "Accion", operator: "eq", value: null },
                    { field: "Accion", operator: "eq", value: 0 }
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
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
                       { field: "FechaPrueba", title: _dictionary.columnFechaPrueba[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), editor: RenderDatePicker, width: "12px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
                       { field: "UnidadMedida", editor: RenderAprobado, title: "Valor U. Medida", filterable: getGridFilterableCellNumberMaftecPopup(), width: "25px", attributes: { style: "text-align:right;" }, editor: RenderMedida },
                       {
                           field: "ResultadoEvaluacion", title: "Aprobado", filterable: {
                               multi: true,
                               messages: {
                                   isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                                   isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                                   style: "max-width:120px;"
                               },
                               dataSource: [{ ResultadoEvaluacion: true }, { ResultadoEvaluacion: false }]
                           }, template: "#= ResultadoEvaluacion ? 'Si' : 'No' #", width: "15px", attributes: { style: "text-align:center;" }
                       },
                      { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "10px", attributes: { style: "text-align:center;" } }
        ],
        dataBound: function () {
            var grid = $("#gridPopUp").data("kendoGrid");
            var gridData = grid.dataSource.view();

            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].RowOk == false) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");

                }
                else if (gridData[i].RowOk) {
                    if (i % 2 == 0)
                        grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                }
            }
        },
        toolbar: [{ name: "create" }]
    });
    CustomisaGrid($("#gridPopUp"));


};


function eliminarCaptura(e) {
    e.preventDefault();
    if ($('#botonGuardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

        var filterValue = $(e.currentTarget).val();
        var dataItem = $("#gridPopUp").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;

        if (dataItem.Accion!=2)
            dataSource.remove(dataItem);
        else
            dataItem.Accion=3
        
        dataSource.sync();
    }

}



function verVentanaPruebasPorSpool(NombrePrueba,unidadmedida) {
    windowPopupPruebasSpool.setOptions({
        title : NombrePrueba + '(' + unidadmedida + ')'
    });

    windowPopupPruebasSpool.center().open();
};