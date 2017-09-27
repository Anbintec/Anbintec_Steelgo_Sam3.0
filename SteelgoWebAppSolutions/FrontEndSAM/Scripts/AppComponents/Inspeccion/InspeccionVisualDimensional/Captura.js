var endRangeDate;
var endRangeDateV;
var listadoJsonCaptura;
var anteriorlongitudTrabajosAdicionales;
var actuallongitudTrabajosAdicionales;
var editado = false;
var spooolAnterior;
var ventanaConfirm;
var ventanaConfirmCambiarCaptura;
var esNormal;
var ordentrabajoSpoolID = 0;
var ObjetoCapturaCliente=[];
var OrdenTrabajoAnterior;
var ventanaConfirmGuardar;
var OrdenTrabajoSpoolAnterior;
var OrdenTrabajoSpoolActual;

IniciarCapturaInspecion();

function EjecutarCambioElemento(dataItem) {
    spooolAnterior = dataItem;
    OrdenTrabajoAnterior = $("#InputOrdenTrabajo").val();

    if ($("#InputOrdenTrabajo").val() != '') {
        ordentrabajoSpoolID = dataItem;
        ajaxobtenerDetalleDimensional(dataItem.Valor);
        ajaxObtenerJSonGrid();
        ajaxObtenerListaTaller();
        Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
    }
}

function esSpoolMismoCaptura()
{
    if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
        if ($("#grid").data("kendoGrid").dataSource._data[0].OrdenTrabajoSpoolID == ordentrabajoSpoolID.Valor)
            return true;
        else
            return false;
    }
    else
        return true;
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

//Cambia lenguaje
function changeLanguageCall() {
    endRangeDate.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]
    });
    endRangeDateV.data("kendoDatePicker").setOptions({
        format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]
    });
    suscribirEventoWindowsConfirmaCaptura();
    CargarGrid();
    limpiar();
    $('#Guardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    opcionHabilitarView(false, "FieldSetView");

    IniciarPreCargas();
};
function IniciarCapturaInspecion() {

    asignarProyecto();
    SuscribirEventos();

};

function IniciarPreCargas() {

    ajaxObtenerListaDefectosDimensionales();
    ajaxObtenerListaDefectosVisuales();
    ajaxObtenerListaInspector();
    ajaxObtenerListaInspectorVisual();
    ajaxCargaCamposPredeterminados();

}

function asignarProyecto() {
    //    $("#InputOrdenTrabajo").val(Cookies.get('LetraProyecto') == undefined ? '' : Cookies.get('LetraProyecto'));
    $("#LabelProyecto").text('Proyecto :' + (Cookies.get('Proyecto') == undefined ? 'No hay ningun proyecto' : Cookies.get('Proyecto')));
}

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

function ExisteJunta(Spool) {

    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var jsonGrid = query.filter(filters).data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].JuntaID == Spool) {
            return false;
        }
    }
    return true;
}
function DatoDefaultNumeroUnico1() {

}
function DatoDefaultNumeroUnico2() {

    return "";
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

                // don't edit if prevented in beforeEdit
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);

    $("#grid").kendoGrid({
        autoBind: true,
        autoSync: true,
        edit: function (e) {


            if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                this.closeCell();
            }
            else {
                var inputName = e.container.find('input');
                inputName.select();
            }

            if ($('#Guardar').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        change: function () {
            var dataItem = this.dataSource.view()[this.select().index()];
        },
        dataSource: {

            data: '',
            schema: {
                model: {
                    fields: {
                        Junta: { type: "string", editable: false },
                        DetalleJunta: { type: "string", editable: false },
                        Diametro: { type: "number", editable: false },
                        Taller: { type: "string", editable: true },
                        Resultado: { type: "string", editable: true },
                        Defectos: { type: "string", editable: true },
                        Inspector: { type: "string", editable: true },
                        FechaInspeccion: { type: "date", editable: true },
                        NumeroUnico1: { type: "string", editable: true },
                        NumeroUnico2: { type: "string", editable: true }
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
            numeric: true,
        },
        columns: [
            { field: "Junta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "55px" },
            { field: "DetalleJunta", title: _dictionary.columnDetalleJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
            { field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
            { field: "Taller", title: _dictionary.columnTaller[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxTaller, width: "80px" },
            { field: "Resultado", title: _dictionary.columnResultado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderOptionResultado, width: "85px" },
            { field: "Defectos", title: _dictionary.columnDefectos[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxDefectos, width: "80px" },
            { field: "Inspector", title: _dictionary.columnInspector[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxInspector, width: "80px" },
            { field: "FechaInspeccion", title: _dictionary.columnFecha[$("#language").data("kendoDropDownList").value()], filterable: { cell: { showOperators: false } }, format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], width: "80px", editor: RenderDatePicker },
            { field: "NumeroUnico1", title: _dictionary.columnNumeroUnico1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxNumeroUnico1, width: "80px" },
            { field: "NumeroUnico2", title: _dictionary.columnNumeroUnico2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: RenderComboBoxNumeroUnico2, width: "80px" },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: cancelarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "35px" },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarCaptura }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "35px" }
        ],
        filterable: getGridFilterableMaftec(),
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");

            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },
        dataBound: function (e) {
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
};

function isEditable(fieldName, model) {
    if (fieldName == "Defectos") {
        // condition for the field "ProductName"
        return (model.Resultado == "Aprobado" || model.Resultado == null || model.Resultado == "") ? false : true;
    }

    return true; // default to editable
}

function seEncuentraEtiquetaMaterialOtrasJuntas(jsonGridArmado, LocalizacionEtiquetaMaterial, rowitem) {
    var cantidadEtiquetaMaterialLocalizacionRepetido = 0;
    for (var i = 0; i < jsonGridArmado.length; i++) {
        for (var j = 0; j < jsonGridArmado[i].Localizacion.split('-').length ; j++) {
            if (jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal) && jsonGridArmado[i].Localizacion.split('-')[j] == LocalizacionEtiquetaMaterial)
                cantidadEtiquetaMaterialLocalizacionRepetido++;
        }
    };

    if (cantidadEtiquetaMaterialLocalizacionRepetido > 1)
        return true;
    return false;
};

function ArregloListadoCaptura() {

    ObjetoCapturaCliente = [];
    ObjetoCapturaCliente[0] = { ProyectoID: "", Proyecto: "", OrdenTrabajoID: "", OrdenTrabajo: "", OrdenTrabajoSpoolID: "", OrdenTrabajoSpool: "", SpoolID: "", FechaInspeccion: "", InspectorID: "", Inspector: "" };
    var fechaInspeccion = new Date($("#FechaInspeccion").data("kendoDatePicker").value());

    ObjetoCapturaCliente[0].ProyectoID = ordentrabajoSpoolID.ProyectoID;
    ObjetoCapturaCliente[0].Proyecto = ordentrabajoSpoolID.Proyecto;
    ObjetoCapturaCliente[0].OrdenTrabajoID = $("#InputOrdenTrabajo").val();
    ObjetoCapturaCliente[0].OrdenTrabajo = $("#InputOrdenTrabajo").val();
    ObjetoCapturaCliente[0].OrdenTrabajoSpoolID = ordentrabajoSpoolID.Valor;
    ObjetoCapturaCliente[0].OrdenTrabajoSpool = ordentrabajoSpoolID.IDValido;
    ObjetoCapturaCliente[0].SpoolID = $("#InputOrdenTrabajo").val() + '-' + $("#InputID").val();
    ObjetoCapturaCliente[0].FechaInspeccion = $("#FechaInspeccion").val();
    ObjetoCapturaCliente[0].InspectorID = $("#inputInspector").val();
    ObjetoCapturaCliente[0].Inspector = $("#inputInspector").data("kendoComboBox").text();

   
    return ObjetoCapturaCliente[0];
};
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
        if (posicionSiguiente == 0 && cantidadItems == 3 && SiguienteElemento != undefined)
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
    return [, ];
}

function ExisteNUGrid(NumeroUnicoID, jsonGridVisualDimensional, rowitem) {
    var existe = false;
    for (var i = 0; i < jsonGridVisualDimensional.length; i++) {
        if ((jsonGridVisualDimensional[i].Accion != 3 &&
            (jsonGridVisualDimensional[i].NumeroUnico1ID == NumeroUnicoID || jsonGridVisualDimensional[i].NumeroUnico2ID == NumeroUnicoID)) &&
            (jsonGridVisualDimensional[i].IdOrdenTrabajo + '-' + jsonGridVisualDimensional[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal) &&
            jsonGridVisualDimensional[i].JuntaID != rowitem.JuntaID)
            return true;
    }
    return false;
}

function ExisteSpoolJuntaEnGrid(combobox, jsonGridVisualDimensional, rowitem) {
    for (var i = 0; i < jsonGridVisualDimensional.length; i++) {
        if (jsonGridVisualDimensional[i].Accion != 3 && (jsonGridVisualDimensional[i].OrdenTrabajoID + '-' + jsonGridVisualDimensional[i].OrdenTrabajoSpool + jsonGridVisualDimensional[i].Junta) == (rowitem.OrdenTrabajoID + '-' + rowitem.OrdenTrabajoSpool + combobox.JuntasEncontradas.trim())) {
            return true;
        }
    }
    return false;
};

function MensajesSteelGO(control, mensajeExepcionTecnico) {

    switch (control) {

        case 'InputOrdenTrabajo':// el InputOrdenTrabajo no tiene el formato correcto.
            displayNotify("OrdenTrabajoNoValida", "", '1');
            break;
        case 'Mensajes_error':
            displayNotify("Mensajes_error", mensajeExepcionTecnico, '2');//muestra cualquier error indicando el error tecnico al usuario
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

function BuscarItemSiguienteEnGrid(siguienteItemBuscar) {
    var jsonGridVisualDimensional = $("#grid").data("kendoGrid").dataSource._data;


    //se busca el nuevo item en alguna junta con el mismo EtiquetaMaterial .
    for (var i = 0; i < jsonGridVisualDimensional.length; i++) {
        for (var j = 0; j < jsonGridVisualDimensional[i].ListaNumerosUnicos1.length; j++) {
            if (jsonGridVisualDimensional[i].ListaNumerosUnicos1[j].Clave == siguienteItemBuscar.Clave && jsonGridVisualDimensional[i].ListaNumerosUnicos1[j].EtiquetaMaterial == siguienteItemBuscar.EtiquetaMaterial && jsonGridVisualDimensional[i].NumeroUnico1 == undefined)
                return [jsonGridVisualDimensional[i], jsonGridVisualDimensional[i].ListaNumerosUnicos1[j]];
        }

        for (var j = 0; j < jsonGridVisualDimensional[i].ListaNumerosUnicos2.length; j++) {
            if (jsonGridVisualDimensional[i].ListaNumerosUnicos2[j].Clave == siguienteItemBuscar.Clave && jsonGridVisualDimensional[i].ListaNumerosUnicos2[j].EtiquetaMaterial == siguienteItemBuscar.EtiquetaMaterial && jsonGridVisualDimensional[i].NumeroUnico2 == undefined)
                return [jsonGridVisualDimensional[i], jsonGridVisualDimensional[i].ListaNumerosUnicos2[j]];
        }
    }

    //se busca el nuevo item en alguna junta con diferente EtiquetaMaterial .
    for (var i = 0; i < jsonGridVisualDimensional.length; i++) {
        for (var j = 0; j < jsonGridVisualDimensional[i].ListaNumerosUnicos1.length; j++) {
            if (jsonGridVisualDimensional[i].ListaNumerosUnicos1[j].Clave == siguienteItemBuscar.Clave && jsonGridVisualDimensional[i].ListaNumerosUnicos1[j].EtiquetaMaterial != siguienteItemBuscar.EtiquetaMaterial)
                return [jsonGridVisualDimensional[i], jsonGridVisualDimensional[i].ListaNumerosUnicos1[j]];
        }

        for (var j = 0; j < jsonGridVisualDimensional[i].ListaNumerosUnicos2.length; j++) {
            if (jsonGridVisualDimensional[i].ListaNumerosUnicos2[j].Clave == siguienteItemBuscar.Clave && jsonGridVisualDimensional[i].ListaNumerosUnicos2[j].EtiquetaMaterial != siguienteItemBuscar.EtiquetaMaterial)
                return [jsonGridVisualDimensional[i], jsonGridVisualDimensional[i].ListaNumerosUnicos2[j]];
        }
    }

    //en caso de no encontrar nada
    return undefined;
}
function cancelarCaptura(e) {
    e.preventDefault();
    if ($('#Guardar').text().trim() != "Editar") {
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var spoolIDRegistro = dataItem.SpoolID;
        var modalTitle = "";

        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.remove(dataItem);
        dataSource.sync();
    }
};

function limpiarCaptura(e) {
    e.preventDefault();
    if ($('#Guardar').text() == _dictionary.DetalleAvisoLlegada0017[$("#language").data("kendoDropDownList").value()]) {

        var itemToClean = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        itemToClean.Taller = "";
        itemToClean.TallerID = "";
        itemToClean.Resultado = "";
        itemToClean.InspectorID = "";
        itemToClean.Defectos = "";
        itemToClean.Inspector = "";
        itemToClean.FechaInspeccion = "";
        //itemToClean.NumeroUnico1 = "";
        //itemToClean.NumeroUnico2 = "";
        //itemToClean.NumeroUnico1ID = "";
        //itemToClean.NumeroUnico2ID = "";

        if (itemToClean.Accion == 1)
            itemToClean.Accion = 1;
        if (itemToClean.Accion == 2)
            itemToClean.Accion = 4;

        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.sync();

    }
}

function FiltroMostrar(mostrar) {
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (mostrar == 0) {
        var curr_filters = ds.filter().filters;
        ds.filter(curr_filters[0])
        ds.sync();
    }
    else {
        var filters = ds.filter();
        filters.logic = "or"
        filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
        ds.sync();
    }
}


function PlanchaTaller() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            data[i].TallerID = $("#inputTaller").val();
            data[i].Taller = $("#inputTaller").data("kendoComboBox").text();
        }
        else {
            if (data[i].Taller == "" || data[i].Taller == null || data[i].Taller == undefined) {
                data[i].TallerID = $("#inputTaller").val();
                data[i].Taller = $("#inputTaller").data("kendoComboBox").text();
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};
function PlanchaInspector() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            data[i].InspectorID = $("#inputInspectorVisual").val();
            data[i].Inspector = $("#inputInspectorVisual").data("kendoComboBox").text();
        }
        else {
            if (data[i].Inspector == "" || data[i].Inspector == null || data[i].Inspector == undefined) {
                data[i].InspectorID = $("#inputInspectorVisual").val();
                data[i].Inspector = $("#inputInspectorVisual").data("kendoComboBox").text();
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};
function PlanchaDefecto() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            data[i].DefectosID = $("#inputDefectosVisual").val();
            data[i].Defectos = $("#inputDefectosVisual").data("kendoComboBox").text();
        }
        else {
            if ((data[i].Defectos == "" || data[i].Defectos == null || data[i].Defectos == undefined) && data[i].Resultado != "Aprobado") {
                data[i].DefectosID = $("#inputDefectosVisual").val();
                data[i].Defectos = $("#inputDefectosVisual").data("kendoComboBox").text();
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};
function PlanchaFecha() {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() === "Todos") {
            data[i].FechaInspeccion = new Date(ObtenerDato(endRangeDateV.val(), 1), ObtenerDato(endRangeDateV.val(), 2), ObtenerDato(endRangeDateV.val(), 3));//año, mes, dia;
        }
        else {
            if (data[i].FechaInspeccion == "" || data[i].FechaInspeccion == null || data[i].FechaInspeccion == undefined) {
                data[i].FechaInspeccion = new Date(ObtenerDato(endRangeDateV.val(), 1), ObtenerDato(endRangeDateV.val(), 2), ObtenerDato(endRangeDateV.val(), 3));//año, mes, dia;
            }
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
};
function PlanchadoResultadoVisual() {
    try {
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        var filters = dataSource.filter();
        var allData = dataSource.data();
        var query = new kendo.data.Query(allData);
        var data = query.filter(filters).data;

        for (var i = 0; i < data.length; i++) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos" && $('input:radio[name=ResultadoVisual]:checked').val() != "Ninguno") {
                data[i].ResultadoID = $('input:radio[name=ResultadoVisual]:checked').val() == "Aprobado" ? 1 : 2;
                if (data[i].ResultadoID == 1)
                    data[i].Resultado = _dictionary.DimensionalVisualAporbadoVisual[$("#language").data("kendoDropDownList").value()];
                else
                    data[i].Resultado = _dictionary.DimensionalVisualRechazadoVisual[$("#language").data("kendoDropDownList").value()];

                if (data[i].Resultado == "Aprobado") {
                    data[i].DefectosID = "";
                    data[i].Defectos = "";
                }
            }
            else {
                if ((data[i].Resultado == "" || data[i].Resultado == null || data[i].Resultado == undefined) && $('input:radio[name=ResultadoVisual]:checked').val() != "Ninguno") {
                    data[i].ResultadoID = $('input:radio[name=ResultadoVisual]:checked').val() == "Aprobado" ? 1 : 2;

                    if (data[i].ResultadoID == 1)
                        data[i].Resultado = _dictionary.DimensionalVisualAporbadoVisual[$("#language").data("kendoDropDownList").value()];
                    else
                        data[i].Resultado = _dictionary.DimensionalVisualRechazadoVisual[$("#language").data("kendoDropDownList").value()];

                    if (data[i].Resultado == "Aprobado") {
                        data[i].DefectosID = "";
                        data[i].Defectos = "";
                    }
                }
            }
        }
        $("#grid").data("kendoGrid").dataSource.sync();
    }
    catch (e) { }
};