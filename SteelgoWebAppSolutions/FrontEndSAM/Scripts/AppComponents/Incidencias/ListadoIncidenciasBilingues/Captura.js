var FechaInicio;
var FechaFin;

IniciarListadoIncidenciasBilingues();
function IniciarListadoIncidenciasBilingues() {
    SuscribirEventos();
}

function changeLanguageCall() {
    CargarGrid();

    document.title = "Listado Incidencias Bilingües"//_dictionary.EmbarqueDashboardTituloPagina[$("#language").data("kendoDropDownList").value()];
    AjaxCargarProyecto();
    AjaxCargarPeriodos();

    FechaInicio.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });

    FechaFin.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });

    $("#grid").data("kendoGrid").hideColumn("Titulo");
    $("#grid").data("kendoGrid").hideColumn("TituloIngles");
    $("#grid").data("kendoGrid").hideColumn("Descripcion");
    $("#grid").data("kendoGrid").hideColumn("DescripcionIngles");


    $("#grid").data("kendoGrid").hideColumn("Respuesta");
    $("#grid").data("kendoGrid").hideColumn("RespuestaIngles");
    $("#grid").data("kendoGrid").hideColumn("DetalleResolucion");
    $("#grid").data("kendoGrid").hideColumn("DetalleResolucionIngles");
    $("#grid").data("kendoGrid").hideColumn("MotivoCancelacion");
    $("#grid").data("kendoGrid").hideColumn("MotivoCancelacionIngles");
}

function CargarGrid() {
    $("#grid").kendoGrid({
        dataSource: {
            schema: {
                model: {
                    fields: {
                        FolioIncidenciaID: { type: "string" },
                        Clasificacion: { type: "string" },
                        TipoIncidencia: { type: "string" },
                        Estatus: { type: "string" },
                        RegistradoPor: { type: "string" },
                        FechaRegistro: { type: "date" }
                    }
                },
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        editable: false,
        navigatable: true,
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
        columns: [
            { field: "FolioOriginalID", title: _dictionary.ListadoIncidencias0008[$("#language").data("kendoDropDownList").value()], template: folioTemplate, width: "150px", filterable: getGridFilterableCellMaftec() },
            { field: "Clasificacion", title: _dictionary.ListadoIncidencias0009[$("#language").data("kendoDropDownList").value()], width: "200px", filterable: getGridFilterableCellMaftec() },
            { field: "TipoIncidencia", title: _dictionary.ListadoIncidencias0010[$("#language").data("kendoDropDownList").value()], width: "200px", filterable: getGridFilterableCellMaftec() },
            { field: "Estatus", title: _dictionary.ListadoIncidencias0011[$("#language").data("kendoDropDownList").value()], width: "180px", filterable: getGridFilterableCellMaftec() },
            { field: "RegistradoPor", title: _dictionary.ListadoIncidencias0012[$("#language").data("kendoDropDownList").value()], width: "180px", filterable: getGridFilterableCellMaftec() },
            { field: "FechaRegistro", title: _dictionary.columnFecha[$("#language").data("kendoDropDownList").value()], format: "{0:dd/MM/yyyy}", width: "180px", filterable: { cell: { showOperators: false } } },
            //{ field: "FolioIncidenciaID", title: _dictionary.ListadoIncidencias0008[$("#language").data("kendoDropDownList").value()], template: "<a class='detailLink' onclick='mostrarDetalle(#:FolioIncidenciaID#)'>#:FolioIncidenciaID#</a>", hidden: true },
            { field: "Titulo", title: "Titulo", width: "180px", filterable: getGridFilterableCellMaftec() },
            { field: "TituloIngles", title: "Titulo Ingles", width: "180px", filterable: getGridFilterableCellMaftec() },
            { field: "Descripcion", title: "Descripcion", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "DescripcionIngles", title: "Descripcion Ingles", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "Respuesta", title: "Respuesta", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "RespuestaIngles", title: "Respuesta Ingles", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "DetalleResolucion", title: "Resolucion", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "DetalleResolucionIngles", title: "Resolucion Ingles", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "MotivoCancelacion", title: "Mot. Cancelacion", width: "400px", filterable: getGridFilterableCellMaftec() },
            { field: "MotivoCancelacionIngles", title: "Mot Cancelacion Ingles", width: "400px", filterable: getGridFilterableCellMaftec() },

        ],
    });
    CustomisaGrid($("#grid"));
};



function folioTemplate(dataItem) {
    return "<a class='detailLink' onclick='mostrarDetalle(" + dataItem.FolioIncidenciaID + ")'>" + (dataItem.FolioOriginalID !== null && $.isNumeric(dataItem.FolioOriginalID) ? dataItem.FolioOriginalID : dataItem.FolioConfiguracionIncidencia) + "</a>"
}

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        return false;
    }
    return true;
}



function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#ProyectoID").data("kendoComboBox").enable(false);
        $("#EtapaID").data("kendoComboBox").enable(false);
        $("#PeriodoTiempo").data("kendoComboBox").enable(false);

        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#ProyectoID").data("kendoComboBox").enable(true);
        $("#EtapaID").data("kendoComboBox").enable(true);
        $("#PeriodoTiempo").data("kendoComboBox").enable(true);

        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);


    }
}