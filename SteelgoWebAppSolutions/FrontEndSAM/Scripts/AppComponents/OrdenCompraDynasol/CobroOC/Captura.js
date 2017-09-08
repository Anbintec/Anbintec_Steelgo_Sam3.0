var modeloRenglon;
var esNormal;
var cambiosCheckOK = 0;

function changeLanguageCall() {
    CargarGrid();
    inicio();
    document.title = _dictionary.titleCobroOC[$("#language").data("kendoDropDownList").value()];
};

function inicio() {
    SuscribirEventos();
    AjaxObtenerOrdenesCompra();
    //AjaxObtenerCatalogoMoneda();
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
        save: function (e) {
            var grid = $("#grid").data("kendoGrid");
            //if (!ValidaCantPorCobrar(e.model, e.values)) {
            //    e.model.RowOk = false;                
            //    displayNotify("msgErrorCantidadPagoMayor", "", "1");
            //} else {
            //    e.model.RowOk = true;                       
            //}
            //if (e.model.Cobrado != e.values.Cobrado) {
            //    e.model.ModificadoPorUsuario = true;
            //} else {
                //e.model.ModificadoPorUsuario = false;
            //}
            grid.dataSource.sync();
        },
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (isEditable(fieldName)) {
                if (!VerificaCantidadSteelgo(e.model)) {
                    displayNotify("msgErrorCantidadSteelgoCero", "", "1");
                    e.preventDefault();
                } else {
                    if (ChecaCantidadIgualSteelgo(e.model)) {
                        displayNotify("msgAvisoMismaCantidadSteelgo", "", "1");
                        e.preventDefault();
                    } else {
                        if (VerificaDeficit(e.model)) {
                            displayNotify("msgAvisoDeficit", "", "1");
                            e.preventDefault();
                        }
                    }
                }
            } else {
                e.preventDefault();
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        RevisionID: { type: "int", editable: false },
                        OrdenCompraID: { type: "int", editable: false },
                        ColadaID: { type: "int", editable: false },
                        Partida: { type: "string", editable: true },
                        ItemCode: { type: "string", editable: false },
                        Descripcion: { type: "string", editable: false },
                        MaterialNorma: { type: "string", editable: false },
                        Diametro1: { type: "string", editable: false },
                        Diametro2: { type: "string", editable: false },
                        Colada: { type: "string", editable: false },
                        Schedule: { type: "string", editable: false },
                        Rating: { type: "string", editable: false },
                        PrepExt: { type: "string", editable: false },
                        CantOC: { type: "int", editable: false },
                        CantC: { type: "int", editable: false },
                        CantG: { type: "int", editable: false },
                        CantS: { type: "int", editable: false },
                        Cobrado: { type: "int", editable: false },
                        PorCobrar: { type: "number", editable: true },
                        Deficit: { type: "int", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                    { field: "Accion", operator: "eq", value: 0 },
                    { field: "Accion", operator: "eq", value: undefined }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false,
            aggregate: [
                { field: "CantOC", aggregate: "sum" },
                { field: "CantC", aggregate: "sum" },
                { field: "CantG", aggregate: "sum" },
                { field: "CantS", aggregate: "sum" },
                { field: "Deficit", aggregate: "sum" },
                { field: "Cobrado", aggregate: "sum" }
            ]
        },
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');
                inputName.select();
            });
            if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()] || $("#inputCerrar").is(":checked")) {
                this.closeCell();
            };
            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        selectable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "Partida", title: _dictionary.columnPartida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "ItemCode", title: _dictionary.columnRev[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Descripcion", title: _dictionary.columnDescripcion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },            
            { field: "Diametro1", title: _dictionary.columnD1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Diametro2", title: _dictionary.columnD2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "CantOC", title: "Cant OC", filterable: getGridFilterableCellNumberMaftec(), width: "90px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", attributes: { style: "text-align:right;" } },
            { field: "CantC", title: "Cant Cecilia", filterable: getGridFilterableCellNumberMaftec(), width: "90px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", },
            { field: "CantG", title: "Cant Gerez", filterable: getGridFilterableCellNumberMaftec(), width: "90px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", },
            { field: "CantS", title: "Cant Steelgo", filterable: getGridFilterableCellNumberMaftec(), width: "90px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", },
            { field: "Cobrado", title: "Cobrado", filterable: getGridFilterableCellNumberMaftec(), width: "90px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", },
            { field: "PorCobrar", title: _dictionary.columnPorCobrar[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "73px", editor: RenderPorCobrar, attributes: { style: "text-align:right;" } },
            { field: "Deficit", title: "Deficit", filterable: getGridFilterableCellNumberMaftec(), width: "90px", aggregates: ["sum"], footerTemplate: "<div style='text-align:right;'>SUM: #= kendo.toString(sum, 'n') #</div>", }
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
            if (esNormal){
                $(".k-grid-content td").css("white-space", "normal");
            }               
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }                        
        },
        //editable: true,
        editable: "incell",
        navigatable: true
    });
    CustomisaGrid($("#grid"));
};

function isEditable(fieldName) {
    if (fieldName === "PorCobrar") {
        return true;
    }
    return false;
}

function VerificaCantidadSteelgo(model) {
    var CantS = (model.CantS == undefined || isNaN(model.CantS)) ? 0 : model.CantS;
    if (CantS > 0 && CantS != '') {
        return true;
    } else {
        return false;
    }
}
function ChecaCantidadIgualSteelgo(model) {
    var CantS = (model.CantS == undefined || isNaN(model.CantS)) ? 0 : model.CantS;
    var Cobrado = (model.Cobrado == undefined || isNaN(model.Cobrado)) ? 0 : model.Cobrado;
    if (Cobrado == CantS) {
        return true;
    } else {        
        return false;
    }
}

function VerificaDeficit(model) {
    var CantS = (model.CantS == undefined || isNaN(model.CantS)) ? 0 : model.CantS;
    var Cobrado = (model.Cobrado == undefined || isNaN(model.Cobrado)) ? 0 : model.Cobrado;
    if (Cobrado > CantS) {
        return true;
    } else {
        return false;
    }
}

function ValidaCantPorCobrar(model, value) {
    var porCobrar = value.PorCobrar;
    var cobrado = model.Cobrado == undefined ? 0 : model.Cobrado;
    if (isNaN(porCobrar) || porCobrar == undefined) {
        porCobrar = 0;
    }
    if ((porCobrar + cobrado) > model.CantS) {        
        return false;
    } else {
        model.Cobrado = (cobrado + porCobrar);
        return true;
    }
}


function VentanaModal(Cliente) {
    var modalTitle = "";
    modalTitle = _dictionary.lblAgregarCliente[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "60%",
        minWidth: "50px",
        position: {
            top: "10px",
            left: "10px"
        },
        actions: [
            "Close"
        ],
        close: function onClose(e) {            
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();
    $("#inputNombreCliente").val(Cliente);
};