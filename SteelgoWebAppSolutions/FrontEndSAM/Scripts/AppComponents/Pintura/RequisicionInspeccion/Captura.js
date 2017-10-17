IniciaModuloRequisicionInspeccion();
function IniciaModuloRequisicionInspeccion() {
    setTimeout(function () { SuscribirEventos(); }, 1000);
    $('input:radio[name=Planchar]:nth(0)').prop("checked", true);
}
function changeLanguageCall() {
    CargarGrid();
    document.title = _dictionary.menuPinturaRequisicionInspeccion[$("#language").data("kendoDropDownList").value()];    
};


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
        save: function (e) {
            if (e.model.FechaSolicitud != e.values.FechaSolicitud) {
                e.model.ModificadoPorUsuario = true;
            } else {
                e.model.ModificadoPorUsuario = false;
            }
        },
        edit: function (e) {
            if ($("#btnGuardarSup").text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                if (e.model.Resultado.toUpperCase() == "APROBADO" && e.model.FechaInspeccion != "" && e.model.FechaSolicitud != "") {
                    this.closeCell();
                    displayNotify("msgPinturaRequisicionInspNoEditar", "", "1");
                }
            }            
            if ($('#btnGuardarSup').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();
        },
        dataSource: {
            schema: {
                model: {
                    fields: {                       
                        Accion: { type: "int", editable: false },
                        RequisicionInspeccionID: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        NumeroControl: { type: "string", editable: false },                        
                        SistemaPintura: { type: "string", editable: false },
                        Color: { type: "string", editable: false },
                        FechaSolicitud: { type: "date", editable: true },
                        Resultado: { type: "string", editable: false },
                        ModificadoPorUsuario: { type: "boolean", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: []
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        editable: true,
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
            { field: "NumeroControl", title: _dictionary.columnNumeroControl[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "SistemaPintura", title: _dictionary.labelPlanchadoSP[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Color", title: "Color", filterable: getGridFilterableCellMaftec(), width: "130px" },            
            { field: "FechaSolicitud", title: _dictionary.columnPinturaFechaSolicitudInsp[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), editor: RenderDatePicker, format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], width: "150px" },
            { field: "Resultado", title: _dictionary.columnResultado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura, }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }           
        ],
        dataBound: function (a) {            
        }
    });
    CustomisaGrid($("#grid"));
}

function eliminarCaptura(e) {
    e.preventDefault();
    if ($("#btnGuardarSup").text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
        var grid = $("#grid").data("kendoGrid");
        var dataItem = grid.dataItem($(e.currentTarget).closest("tr"));
        var dataSource = grid.dataSource;
        if (dataItem.Resultado.toUpperCase() == "APROBADO" && dataItem.FechaInspeccion != "") {
            e.preventDefault();
            displayNotify("msgPinturaRequisicionInspNoEliminar", "", "1");
        } else if (dataItem.Accion == 2 && ((dataItem.FechaSolicitud != "" && dataItem.FechaSolicitud != null))) {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: $("#language").val() == "es-MX" ? "Aviso!" : "Warning!",
                visible: false,
                animation: false,
                width: "auto",
                height: "auto",
                actions: [],
                modal: true
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.msgPinturaRequisicionInspPreguntaEliminar[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirm.open().center();
            $("#yesButtonProy").click(function () {
                ventanaConfirm.close();                
                dataItem.Accion = 3;
                dataItem.ModificadoPorUsuario = true;
                dataSource.sync();
                AjaxGuardarCaptura(dataSource._data, true, true);
                dataSource.remove(dataItem);
            });
            $("#noButtonProy").click(function () {
                ventanaConfirm.close();
            });
        } else if( dataItem.Accion == 1 && dataItem.FechaSolicitud == "" || dataItem.FechaSolicitud == null) {
            //ELIMINO LOS QUE SON NUEVOS Y NO TIENEN CAPTURA DE FECHA DE SOLICITUD
            dataSource.remove(dataItem);
        }
        dataSource.sync();
    }
}


function limpiarRenglon(e) {
    e.preventDefault();
    if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {        
        var grid = $("#grid").data("kendoGrid");        
        var data = grid.dataSource._data;
        var dataItem = grid.dataItem($(e.currentTarget).closest("tr"));
        if ((dataItem.FechaInspeccion != "" || dataItem.FechaInspeccion != null) && dataItem.Resultado.toUpperCase() === "APROBADO") {
            displayNotify("msgPinturaRequisicionInspNoEditar", "", "1");
        } else {            
            dataItem.FechaSolicitud = "";
            dataItem.ModificadoPorUsuario = true;
            grid.dataSource.sync();
        }        
    }
}


function HayCamposVacios() {
    var result = false;
    try {        
        var grid = $("#grid").data("kendoGrid");
        var ds = grid.dataSource.view();
        if (ds.length > 0) {
            for (var i = 0; i < ds.length; i++) {
                var currentUid = ds[i].uid;
                var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                if (ds[i].FechaSolicitud == "" || ds[i].FechaSolicitud == null) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
                    result = true;
                } else {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("k-alt");                    
                }
            }
        }
    } catch (e) {
        displayNotify("", "Error Verificando Campos Vacios: " + e.message, "2");        
    }
    return result;
}

function VerificarCamposVacios() {    
    var grid = $("#grid").data("kendoGrid");
    var ds = grid.dataSource.view();
    if (ds.length > 0) {
        for (var i = 0; i < ds.length; i++) {
            var currentUid = ds[i].uid;
            var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");            
            if (ds[i].FechaSolicitud == "" || ds[i].FechaSolicitud == null) {
                grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
            } else {
                grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                grid.table.find("tr[data-uid='" + currentUid + "']").addClass("k-alt");
            }
        }
    }    
}