kendo.ui.Upload.fn._supportsDrop = function () { return false; }
Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10025", { path: '/' });

var endRangeDate;
var listadoJsonCaptura;
var anteriorlongitudTrabajosAdicionales;
var actuallongitudTrabajosAdicionales;
var esNormal;

function IniciarCapturaInspecion() {

    asignarProyecto();
    SuscribirEventos();

}

IniciarCapturaInspecion();
IniciarPreCarga();

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

function IniciarPreCarga() {
    setTimeout(function () { AjaxObtenerListaInspector() }, 1000);
    setTimeout(function () { AjaxObtenerListaDefectos() }, 2000);
}

//Cambia lenguaje
function changeLanguageCall() {
    CargarGrid();
    endRangeDate.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    });
    AjaxObtenerListaDefectos();
    AjaxCargaCamposPredetrminados();
    limpiar();
    $('#Guardar1').text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
    $("#Guardar").text(_dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]);
    document.title = _dictionary.InspeccionDimensionalBreadCrum[$("#language").data("kendoDropDownList").value()];
    opcionHabilitarView(false, "FieldSetView");
}

function asignarProyecto() {
    $("#InputOrdenTrabajo").val(Cookies.get('LetraProyecto') == undefined ? '' : Cookies.get('LetraProyecto'));
    $("#LabelProyecto").text('Proyecto :' + (Cookies.get('Proyecto') == undefined ? 'No hay ningun proyecto' : Cookies.get('Proyecto')));
}




function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        $("#FechaInspeccion").data("kendoDatePicker").value('');
    }
}

function CargarGrid() {
    kendo.ui.Grid.fn.editCell = (function (editCell) {
        return function (cell) {
            cell = $(cell);

            var that = this,
                column = that.columns[that.cellIndex(cell)],
                model = that._modelForContainer(cell),
                event = {
                    container: cell,
                    model: model,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

            if (model && typeof this.options.beforeEdit === "function") {
                this.options.beforeEdit.call(this, event);
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);
    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {
         
            var inputName = e.container.find('input');
            inputName.select();

            if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                this.closeCell();
            }
            else {
                var inputName = e.container.find('input');
                inputName.select();
            }

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        change: function () {
            var dataItem = this.dataSource.view()[this.select().index()];
            if (dataItem.Accion == 4)
                dataItem.Accion = 2;
        },
        dataSource: {
            data: '',
            schema: {
                model: {
                    fields: {
                        Accion: { type: "int", editable: false },
                        InspeccionDimensionalID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "string", editable: false },
                        OrdenTrabajoSpool: { type: "string", editable: false },
                        Resultado: { type: "string", editable: true },
                        ResultadoID: { type: "string", editable: true },
                        DefectosID: { type: "string", editable: true },
                        Defectos: { type: "string", editable: true },
                        InspectorID: { type: "string", editable: true },
                        Inspector: { type: "string", editable: true },
                        FechaInspeccion: { type: "date", editable: true }
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
            { field: "OrdenTrabajoSpool", title: _dictionary.DimensionalVisualHeaderSpoolID[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderOptionResultado, width: "140px" },
             { field: "Resultado", title: _dictionary.DimensionalVisualHeadeResultado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderOptionResultado, width: "140px" },
             { field: "Defectos", title: _dictionary.DimensionalVisualHeaderDefecto[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxDefectos, width: "140px" },
             { field: "Inspector", title: _dictionary.DimensionalVisualHeaderInspectorDimesional[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxInspector, width: "140px" },
             { field: "FechaInspeccion", title: _dictionary.DimensionalVisualHeaderFechaDimesional[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), editor: RenderDatePicker, format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], width: "200px" },
             { field: "ListaJuntasSeleccionadas", title: _dictionary.DimensionalVisualHeaderListaJUnta[$("#language").data("kendoDropDownList").value()], filterable: false, editor: RenderMultiSelectJuntas, template: "#:TemplateRender#", width: "300px" },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, title: _dictionary.tituloEliminar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, title: _dictionary.tituloLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }
        ],

        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },
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


function isEditable(fieldName, model) {
    if (fieldName === "Defectos") {
        // condition for the field "ProductName"
        var validarDefecto = true;
        if (model.ResultadoID == "" || model.ResultadoID == 0 || model.ResultadoID == undefined || model.ResultadoID == "1") {
            displayNotify('mensajeInspeccionVisualDimensionalSeleccionarResultado', '', '1');
            validarDefecto = false;
        }
        

        return validarDefecto;
    }
    else if (fieldName === "ListaJuntasSeleccionadas") {
        // condition for the field "ProductName"
        //alert(model.TIPO );

        if (model.ResultadoID == "1"  || model.ResultadoID == undefined || model.ResultadoID == "0" || model.ResultadoID == 0) {
            displayNotify('mensajeInspeccionVisualDimensionalNoAdmiteJuntasDefectoSpoolAprobado', '', '1');
        }
        else if (model.Defectos == "") {
            displayNotify('mensajeInspeccionVisualDimensionalNoAdmiteJuntasDefectoSeleccionarDefecto', '', '1');
        }
        else {
            if (model.TIPO == "NoEspecificarJunta") {
                displayNotify("mensajeInspeccionVisualDimensionalNoAdmiteJuntasDefecto", '', '1');
            }

        }

        return model.TIPO !== "NoEspecificarJunta" && model.ResultadoID !== "1" && model.ResultadoID !== "0";
    }

    return true; // default to editable
}

function ArregloListadoCaptura() {

    JsonCaptura = [];
    JsonCaptura[0] = { Accion: "", InspeccionDimensionalID: "", ProyectoID: "", Proyecto: "", OrdenTrabajoSpoolID: "", OrdenTrabajoSpool: "", ResultadoID: "", Resultado: "", DefectosID: "", Defectos: "", InspectorID: "", Inspector: "", FechaInspeccion: "" };

    var fechaInspeccion = new Date($("#FechaInspeccion").data("kendoDatePicker").value());
    JsonCaptura[0].Accion = "0";
    JsonCaptura[0].InspeccionDimensionalID = "0";
    JsonCaptura[0].ProyectoID = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).ProyectoID;
    JsonCaptura[0].Proyecto = $("#InputID").data("kendoComboBox").dataItem($("#InputID").data("kendoComboBox").select()).Proyecto;
    JsonCaptura[0].OrdenTrabajoSpoolID = $("#InputID").val();
    JsonCaptura[0].OrdenTrabajoSpool = $("#InputOrdenTrabajo").val() + '-' + $("#InputID").data("kendoComboBox").text();
    JsonCaptura[0].ResultadoID = $('input:radio[name=ResultadoDimensional]:checked').val() == "Aprobado" ? 1 : 2;
    JsonCaptura[0].Resultado = $('input:radio[name=ResultadoDimensional]:checked').val();
    JsonCaptura[0].DefectosID = $("#inputDefecto").val();
    JsonCaptura[0].Defectos = $("#inputDefecto").data("kendoComboBox").text();
    JsonCaptura[0].InspectorID = $("#inputInspector").val();
    JsonCaptura[0].Inspector = $("#inputInspector").data("kendoComboBox").text();
    JsonCaptura[0].FechaInspeccion = $("#FechaInspeccion").val();
    return JsonCaptura[0];
}





function cancelarCaptura(e) {
    e.preventDefault();
    var filterValue = $(e.currentTarget).val();
    var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
    if ($("#language").val() == "es-MX") {
        if ($('#Guardar').text().trim() != "Editar") {
            //windowTemplate = kendo.template($("#windowTemplate").html());

            //ventanaConfirm = $("#ventanaConfirm").kendoWindow({
            //    iframe: true,
            //    title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
            //    visible: false, //the window will not appear before its .open method is called
            //    width: "auto",
            //    height: "auto",
            //    modal: true,
            //    animation: {
            //        close: false,
            //        open: false
            //    }
            //}).data("kendoWindow");

            //ventanaConfirm.content(_dictionary.CapturaInspeccionPreguntaBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
            //            "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

            //ventanaConfirm.open().center();

            //$("#yesButton").click(function () {

            var dataSource = $("#grid").data("kendoGrid").dataSource;

            if (dataItem.Accion == 1) {
                dataSource.remove(dataItem);
            }
            else {
                dataItem.Accion = 3;
                for (var i = 0; i < dataItem.ListaJuntasSeleccionadas.length; i++) {
                    dataItem.ListaJuntasSeleccionadas[i].Accion = dataItem.ListaJuntasSeleccionadas[i].Accion == 2 ? 3 : dataItem.ListaJuntasSeleccionadas[i].Accion;
                }

                for (var i = 0; i < dataItem.ListaJuntasSeleccionadasInicial.length; i++) {
                    dataItem.ListaJuntasSeleccionadasInicial[i].Accion = dataItem.ListaJuntasSeleccionadasInicial[i].Accion == 2 ? 3 : dataItem.ListaJuntasSeleccionadasInicial[i].Accion;
                }
            }

            $("#grid").data("kendoGrid").dataSource.sync();

            //    ventanaConfirm.close();
            //});
            //$("#noButton").click(function () {
            //    ventanaConfirm.close();
            //});

        }
    }
    else {
        if ($('#Guardar').text().trim() != "Edit") {

            //ventanaConfirm = $("#ventanaConfirm").kendoWindow({
            //    iframe: true,
            //    title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
            //    visible: false, //the window will not appear before its .open method is called
            //    width: "auto",
            //    height: "auto",
            //    modal: true,
            //    animation: {
            //        close: false,
            //        open: false
            //    }
            //}).data("kendoWindow");

            //ventanaConfirm.content(_dictionary.CapturaInspeccionPreguntaBorradoCaptura[$("#language").data("kendoDropDownList").value()] +
            //            "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

            //ventanaConfirm.open().center();

            //$("#yesButton").click(function () {
            var dataSource = $("#grid").data("kendoGrid").dataSource;

            if (dataItem.Accion == 1)
                dataSource.remove(dataItem);
            else
                dataItem.Accion == 3;
            dataSource.remove(dataItem);

            $("#grid").data("kendoGrid").dataSource.sync();

            //    ventanaConfirm.close();
            //});
            //$("#noButton").click(function () {
            //    ventanaConfirm.close();
            //});
        }
    }

}


function limpiarRenglon(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

        var itemToClean = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        itemToClean.Defectos = "";
        itemToClean.DefectosID = 0;
        itemToClean.Resultado = "";
        itemToClean.ResultadoID = 0;

        itemToClean.FechaInspeccion = "";
        itemToClean.InspectorID = 0;
        itemToClean.Inspector = "";
        itemToClean.ListaJuntasSeleccionadas = [];
        if (itemToClean.Accion == 2)
            itemToClean.Accion = 4;

        itemToClean.TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.sync();
        //alert(itemToClean);
    }


}

function PlanchaDefecto() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    if ($("#inputDefecto").data("kendoComboBox").text() != "") {
        for (var i = 0; i < data.length; i++) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
                if (data[i].ResultadoID != "1") {
                    if (data[i].Accion == 4)
                        data[i].Accion = 2;
                    data[i].DefectosID = $("#inputDefecto").data("kendoComboBox").text() == "" ? "" : $("#inputDefecto").val();
                    data[i].Defectos = $("#inputDefecto").data("kendoComboBox").text();
                    data[i].ListaJuntasSeleccionadas = [];
                    data[i].TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                    data[i].IDDEFECTOTIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].IDDEFECTOTIPO;
                    data[i].TIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].TIPO;
                }

            }
            else {
                if (data[i].Defectos == "" || data[i].Defectos == null || data[i].Defectos == undefined) {
                    if (data[i].ResultadoID != "1") {
                        if (data[i].Accion == 4)
                            data[i].Accion = 2;
                        data[i].DefectosID = $("#inputDefecto").data("kendoComboBox").text() == "" ? "" : $("#inputDefecto").val();
                        data[i].Defectos = $("#inputDefecto").data("kendoComboBox").text();
                        data[i].ListaJuntasSeleccionadas = [];
                        data[i].TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                        data[i].IDDEFECTOTIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].IDDEFECTOTIPO;
                        data[i].TIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].TIPO;
                    }
                }
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
}

function PlanchaInspector() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            if ($("#inputInspector").data("kendoComboBox").text() != "") {
                data[i].InspectorID = $("#inputInspector").val();
                data[i].Inspector = $("#inputInspector").data("kendoComboBox").text();
                if (data[i].Accion == 4)
                    data[i].Accion = 2;
            }

        }
        else {
            if (data[i].Inspector == "" || data[i].Inspector == undefined || data[i].Inspector == null) {
                if ($("#inputInspector").data("kendoComboBox").text() != "") {
                    data[i].InspectorID = $("#inputInspector").val();
                    data[i].Inspector = $("#inputInspector").data("kendoComboBox").text();
                    if (data[i].Accion == 4)
                        data[i].Accion = 2;
                }
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
}

function PlanchadoResultadoDimensional() {
    try {
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        var filters = dataSource.filter();
        var allData = dataSource.data();
        var query = new kendo.data.Query(allData);
        var data = query.filter(filters).data;

        for (var i = 0; i < data.length; i++) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos" && $('input:radio[name=ResultadoDimensional]:checked').val() != "Ninguno") {
                data[i].ResultadoID = $('input:radio[name=ResultadoDimensional]:checked').val() == "Aprobado" ? 1 : 2;
                data[i].Resultado = $('input:radio[name=ResultadoDimensional]:checked').val();
                if ($("#inputDefecto").data("kendoComboBox").text() != "" || $('input:radio[name=ResultadoDimensional]:checked').val() == "Aprobado" ) {
                    data[i].DefectosID = $("#inputDefecto").data("kendoComboBox").text() == "" ? "" : $("#inputDefecto").val();
                    data[i].Defectos = $("#inputDefecto").data("kendoComboBox").text();
                    data[i].ListaJuntasSeleccionadas = [];
                    data[i].TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                    data[i].IDDEFECTOTIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].IDDEFECTOTIPO;
                    data[i].TIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].TIPO;

                }
                if (data[i].Accion == 4)
                    data[i].Accion = 2;

            }
            else {
                if ((data[i].Resultado == "" || data[i].Resultado == null || data[i].Resultado == undefined) && $('input:radio[name=ResultadoDimensional]:checked').val() != "Ninguno") {
                    data[i].ResultadoID = $('input:radio[name=ResultadoDimensional]:checked').val() == "Aprobado" ? 1 : 2;
                    data[i].Resultado = $('input:radio[name=ResultadoDimensional]:checked').val();
                    if ($("#inputDefecto").val() != "") {
                        data[i].DefectosID = $("#inputDefecto").data("kendoComboBox").text() == "" ? "" : $("#inputDefecto").val();
                        data[i].Defectos = $("#inputDefecto").data("kendoComboBox").text();
                        data[i].ListaJuntasSeleccionadas = [];
                        data[i].TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                        data[i].IDDEFECTOTIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].IDDEFECTOTIPO;
                        data[i].TIPO = $('#inputDefecto').data("kendoComboBox").dataSource._data[$('#inputDefecto').data("kendoComboBox").selectedIndex].TIPO;
                        if (data[i].Accion == 4)
                            data[i].Accion = 2;

                    }


                }
            }
        }
        $("#grid").data("kendoGrid").dataSource.sync();
    }
    catch (e) { }
}

function PlanchaFecha() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        var m = ObtenerDato(endRangeDate.val(), 2);
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            data[i].FechaInspeccion = new Date(ObtenerDato(endRangeDate.val(), 1), m, ObtenerDato(endRangeDate.val(), 3));//año, mes, dia;           
            if (data[i].Accion == 4)
                data[i].Accion = 2;
        }
        else {
            if (data[i].FechaInspeccion == "" || data[i].FechaInspeccion == null || data[i].FechaInspeccion == undefined) {
                data[i].FechaInspeccion = new Date(ObtenerDato(endRangeDate.val(), 1), m, ObtenerDato(endRangeDate.val(), 3));//año, mes, dia;
                if (data[i].Accion == 4)
                    data[i].Accion = 2;
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
}

function MensajesSteelGO(control, mensajeExepcionTecnico) {

    switch (control) {

        case 'InputOrdenTrabajo':// el InputOrdenTrabajo no tiene el formato correcto.
            displayNotify("OrdenTrabajoNoValida", "", '1');
            break;
        case 'Mensajes_error':
            displayNotify("", mensajeExepcionTecnico, '2');//muestra cualquier error indicando el error tecnico al usuario
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
        case 'Result//AjaxEmpty':
            displayNotify("Result//AjaxEmpty", '', '1');//mensaje indicando que el result al hacer //Ajax no se obtuvo nada
            break;
        case 'SpoolIDExistente':
            displayNotify("SpoolIDExistente", '', '1');//mensaje indicando que el result al hacer //Ajax no se obtuvo nada
            break;
        case 'LoadSuccesfull':
            displayNotify("", _dictionary.DimensionalSpool[$("#language").data("kendoDropDownList").value()] + mensajeExepcionTecnico, '0');//mensaje indicando que el result al hacer //Ajax no se obtuvo nada
            break;

    }
};

function ObtenerDato(fecha, tipoDatoObtener) {
    var cultura = $("#language").val();

    switch (tipoDatoObtener) {
        case 1://anho
            return fecha.split('/')[2]
            break;
        case 2://mes
            if (cultura = 'es-MX')
                return fecha.split('/')[1] - 1;
            else
                return fecha.split('/')[0] - 1;
            break;
        case 3://dia
            if (cultura = 'es-MX')
                return fecha.split('/')[0]
            else
                return fecha.split('/')[1]
            break;
    }
}

function JuntasCorrectasGuardar(listaJuntas) {
    if (listaJuntas != undefined) {
        for (var i = 0; i < listaJuntas.length; i++) {
            if (listaJuntas[i].Accion == 1 || listaJuntas[i].Accion == 2 || listaJuntas[i].Accion == 4)
                return true;
        }
        return false;
    }
    else {
        return false;
    }
}

function JuntasEliminadasGuardar(listaJuntas) {
    for (var i = 0; i < listaJuntas.length; i++) {
        if (listaJuntas[i].Accion != 3)
            return false;
    }
    return true;
}