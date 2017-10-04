var editado = false;
var ListaPruebas = [];
var ListaUnidadMedida = [];

var dataItemProcesoPintura;
var paramReqProyectoID = 0;
var esNormal;
var tieneAvance = false;
var registroCompleto=null;

function changeLanguageCall() {

    var paramReq = getParameterByName('SistemaPinturaID') == undefined || getParameterByName('SistemaPinturaID') == null ? "" : getParameterByName('SistemaPinturaID').trim();
    paramReqProyectoID = (getParameterByName('ProyectoID') == undefined || getParameterByName('ProyectoID') == null) ? 0 : parseInt(getParameterByName('ProyectoID').trim());
    paramReq = paramReq == null ? "" : paramReq;
    $("#inputSistemaPinturaID").val(paramReq);


    CargarGrid();
    
    CargarGridPopUpComponenteAgregado();
    document.title = _dictionary.lblSistemaPinturaSiguientePasoHeader[$("#language").data("kendoDropDownList").value()];

    setTimeout(function () { AjaxObtenerColor(); }, 100);
    if (paramReq == null || paramReq == "")
        Limpiar();
};

function getParameterByName(name, url) {

    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

IniciarSistemaPintura();

function IniciarSistemaPintura() {
    setTimeout(function () { SuscribirEventos() }, 100);
};

function getParameterByName(name, url) {

    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function SiguienteProceso(paramReq) {
    var url = "";
    if (paramReq == null) {
        url = "/PND/CapturaReporteRT?leng=" + $("#language").data("kendoDropDownList").value();
    } else {
        url = "/PND/CapturaReporteRT?leng=" + $("#language").data("kendoDropDownList").value()
            + "&requisicion=" + paramReq;
    }

    $("#EntregaresultadosSup").attr("href", url);
    $("#EntregaresultadosINF").attr("href", url);
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

            if ($('#botonGuardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()] || ($("#inputNoAplicable").is(':checked'))) {
                this.closeCell();
            }
            if (tieneAvance) {
                this.closeCell();
            }


            setTimeout(function () {
                var inputName = e.container.find('input');
                inputName.select();
            });

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Agregar: { type: "boolean", editable: false },
                        Proceso: { type: "string", editable: false },
                        //MetrosLote: { type: "number", editable: true },
                        //NumeroPruebas: { type: "number", editable: true },
                        NumeroComponentes: { type: "number", editable: true },
                        TemplateDetalleComponentes: { type: "string", editable: false },
                        Reductor: { type: "string", editable: true },
                        Pruebas: { type: "string", editable: false },
                        
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

        filterable: getGridFilterableMaftec(),
        columns: [
            {
                field: "Agregar", title: _dictionary.columnEtiquetadoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                }, template: "<input name='fullyPaid' class='chk-agregar' type='checkbox' data-bind='checked: Agregar' #= Agregar ? checked='checked' : '' #/>", width: "60px", attributes: { style: "text-align:center;" }
            },
            { field: "Proceso", title: _dictionary.columnprocesoPintura[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
            { field: "NumeroComponentes", title: _dictionary.columnNumeroComponentes[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", editor: RenderNumeroComponentes, attributes: { style: "text-align:right;" }, format: "{0: }" },
            { field: "TemplateDetalleComponentes", title: _dictionary.columnDetalleComponentes[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "80px", attributes: { style: "text-align:center;" }, template: "<div class='EnlaceDetalleComponentes' style='text-align:center;'><a href='\\#/'  > <span>#=TemplateDetalleComponentes#</span></a></div>", filterable: false },
            { field: "Reductor", title: _dictionary.columnReductor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px", attributes: { style: "text-align:left;" }, editor: RenderReductores },

            { field: "Pruebas", title: _dictionary.columnPrueba[$("#language").data("kendoDropDownList").value()], template: "<div class='EnlaceDetallePruebas' style='text-align:center;'><a href='\\#/'  > <span>Detalle Pruebas</span></a></div>", filterable: false, width: "90px" }
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

    $("#grid .k-grid-content").on("change", "input.chk-agregar", function (e) {
        if (!($("#inputNoAplicable").is(':checked'))) {
            if ($("#language").val() == "es-MX") {
                if ($('#Guardar').text() != "Editar") {
                    var grid = $("#grid").data("kendoGrid")
                    dataItem = grid.dataItem($(e.target).closest("tr"));
                    if (tieneAvance) {
                        if ($(this)[0].checked) {
                            $(this)[0].checked = false;
                        }
                        else {
                            $(this)[0].checked = true;
                        }
                        displayNotify("MensajeSpoolAsignado", "", '1');
                    }
                    else if ($(this)[0].checked) {
                        dataItem.Agregar = true;

                    }
                    else {
                        //if (!(dataItem.MetrosLote == 0 && dataItem.NumeroPruebas == 0 && dataItem.listadoPruebasDetalle.length == 0)) {
                        //    dataItemProcesoPintura = dataItem;

                        //    ventanaProcesosPintura.open().center();
                        //}
                        //else {
                        //    dataItem.Agregar = false;
                        //}

                    }

                }
                else {
                    if ($(this)[0].checked) {
                        $(this)[0].checked = false;
                    }
                    else {
                        $(this)[0].checked = true;
                    }
                }
            }
            else {
                if ($('#Guardar').text() != "Edit") {
                    var grid = $("#grid").data("kendoGrid")
                    dataItem = grid.dataItem($(e.target).closest("tr"));
                    if (tieneAvance) {
                        if ($(this)[0].checked) {
                            $(this)[0].checked = false;
                        }
                        else {
                            $(this)[0].checked = true;
                        }
                        displayNotify("MensajeSpoolAsignado", "", '1');
                    } else if ($(this)[0].checked) {
                        dataItem.Agregar = true;
                    }
                    else {
                        dataItem.Agregar = false;

                    }
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    if ($(this)[0].checked) {
                        $(this)[0].checked = false;
                    }
                    else {
                        $(this)[0].checked = true;
                    }
                }
            }
        }
        else {
            if ($(this)[0].checked) {
                $(this)[0].checked = false;
            }
            else {
                $(this)[0].checked = true;
            }
            displayNotify("MensajeSistemaPinturaNoPintable", "", '1');
        }

    });

};

function isEditable(fieldName, model) {
    //if (fieldName === "MetrosLote") {
    //    if (!model.Agregar)
    //        return false;

    //}
    //if (fieldName === "NumeroPruebas") {
    //    if (!model.Agregar)
    //        return false;

    //}
    if (fieldName === "NumeroComponentes") {
        if (!model.Agregar)
            return false;

    }
    if (fieldName === "TemplateDetalleComponentes") {
        if (!model.Agregar)
            return false;

    }
    if (fieldName === "Reductor") {
        if (!model.Agregar)
            return false;

    }

    return true; // default to editable
}

function deshabilitar() {
    $('#FieldSetView').find('*').attr('disabled', true);
    $("#inputColor").data("kendoMultiSelect").enable(false);
    $("#comboProyecto").data("kendoComboBox").enable(false);
}

function agregarComponentesAutomaticos() {
    if (dataItemRender.ListaDetalleComponentesAgregados != undefined) {
        var longitudElementos = dataItemRender.ListaDetalleComponentesAgregados.length;
        for (var i = 0; i < longitudElementos; i++) {
            if (dataItemRender.ListaDetalleComponentesAgregados[i].Accion == 1) {
                dataItemRender.ListaDetalleComponentesAgregados.splice(i, 1);// eliminamos el elemento.
                i = -1;
                longitudElementos = dataItemRender.ListaDetalleComponentesAgregados.length;
            }
            else
                dataItemRender.ListaDetalleComponentesAgregados[i].Accion = 3;
        }
    }

    var arrayModelComponentesAgregados = [];
    dataItemRender.ListaDetalleComponentesAgregados = dataItemRender.ListaDetalleComponentesAgregados == null ? [] : dataItemRender.ListaDetalleComponentesAgregados;
    for (var i = 0; i < dataItemRender.NumeroComponentes; i++) {
        arrayModelComponentesAgregados[i] = { ComponenteAgregadoID: "", ComponenteID: "", Nombre: "", Accion: 1, ListadoComponentes: "", RowOk: "", ProcesoPinturaID: "" };
        arrayModelComponentesAgregados[i].ComponenteAgregadoID = i + 1;
        arrayModelComponentesAgregados[i].Nombre = "";
        arrayModelComponentesAgregados[i].ListadoComponentes = dataItemRender.ListadoComponentes;
        arrayModelComponentesAgregados[i].RowOk = true;
        dataItemRender.ListaDetalleComponentesAgregados.push(arrayModelComponentesAgregados[i]);
    }

    $("#grid").data("kendoGrid").refresh();
}






function LlenarGridPopUp() {
    //modeloRenglon = data;
    //$("#gridPopUp").data('kendoGrid').dataSource.data([]);
    //var ds = $("#gridPopUp").data("kendoGrid").dataSource;
    //ListaPruebas = data.listadoPruebasProceso;
    //ListaUnidadMedida = data.listadoUnidadesMedida;
    //var array = data.listadoPruebasDetalle;
    //for (var i = 0; i < array.length; i++) {
    //    ds.add(array[i]);
    //}
    VentanaModal();
}


function CargarGridPopUpComponenteAgregado() {

    $("#gridPopUpComponentesAgregados").kendoGrid({
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();
            if (tieneAvance) {
                this.closeCell();
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        ProcesoPinturaID: { type: "number", editable: false },
                        ComponenteAgregadoID: { type: "number", editable: false },
                        Nombre: { type: "string", editable: true }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: undefined },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 4 }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false

        },
        pageable: false,
        selectable: true,
        filterable: getGridFilterableMaftec(),

        columns: [
            { field: "ComponenteAgregadoID", title: _dictionary.CapturaSistemaPinturaComponentesID[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "130px" },
            { field: "Nombre", title: _dictionary.CapturaSistemaPinturaComponenteNombre[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftecpopUp(), width: "130px", editor: renderComboboxNombreComponente }

        ],
        editable: true,
        navigatable: true,
        dataBound: function () {
            var grid = $("#gridPopUpComponentesAgregados").data("kendoGrid");
            var gridData = grid.dataSource.view();

            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (gridData[i].RowOk == false) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").css("background-color", "#ffcccc");
                }
                else if (gridData[i].RowOk) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").css("background-color", "#ffffff");
                }

            }
        }
    });
    CustomisaGrid($("#gridPopUpComponentesAgregados"));
};

function LlenarGridPopUpComponentesAgregados(data) {
    $("#gridPopUpComponentesAgregados").data('kendoGrid').dataSource.data([]);
    if ($("#gridPopUpComponentesAgregados").data('kendoGrid').dataSource._filter == undefined) {
        //investigar porque al destruir una ventana se eliminan solo los filtros.
        $("#gridPopUpComponentesAgregados").data('kendoGrid').dataSource._filter = {
            logic: "or",
            filters: [
              { field: "Accion", operator: "eq", value: 1 },
              { field: "Accion", operator: "eq", value: 2 },
                { field: "Accion", operator: "eq", value: 0 },
                { field: "Accion", operator: "eq", value: undefined }
            ]
        };
    }

    var ds = $("#gridPopUpComponentesAgregados").data("kendoGrid").dataSource;
    var array = data.ListaDetalleComponentesAgregados;

    for (var i = 0; i < array.length; i++) {
        array[i].ProcesoPinturaID = data.ProcesoPinturaID;
        ds.add(array[i]);
    }
    $("#gridPopUpComponentesAgregados").data("kendoGrid").dataSource.sync();
    VentanaModalComponentesAgregados();
}

function VentanaModalComponentesAgregados() {

    var modalTitle = "";
    modalTitle = "Detalle Componentes";
    var window = $("#windowGridComponenteAgregado");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "40%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        actions: [],

        close: function onClose(e) {
            var gridDataSource = $("#gridPopUpComponentesAgregados").data("kendoGrid").dataSource;
            gridDataSource.filter([]);
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

function VentanaModal() {
  

    var modalTitle = "";
    modalTitle = "Detalle pruebas";
    var window = $("#windowGrid");
    window.kendoWindow({
        modal: true,
        iframe: true,
        content: {
            url: "/Pintura/SistemaPinturaProcesoPruebasPopup",
        },
        title: modalTitle,
        resizable: false,
        visible: false,
        width: "80%",
        height:"20%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        actions: ["Close"],
        open: onOpen,
       // close: AsignarPaginaActualCookie,
        refresh: onRefresh
    }).data("kendoWindow");
    
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

function SincronizarOrigen(data) {
    var dataGrid = $("#grid").data("kendoGrid").dataSource._data;
    var procesoPinturaID = data[0].ProcesoPinturaID;
    var sincronizacionok = false;
    for (var i = 0; i < dataGrid.length; i++) {
        if (dataGrid[i].ProcesoPinturaID == procesoPinturaID) {
            dataGrid[i].ListaDetalleComponentesAgregados = data;
            sincronizacionok = true;
            break;
        }
    }
    return sincronizacionok;
}

function sincronizarlistasPruebas(listaHijo)
{
    registroCompleto.listadoPruebasDetalle = listaHijo
    $("#grid").data("kendoGrid").dataSource.sync();
}