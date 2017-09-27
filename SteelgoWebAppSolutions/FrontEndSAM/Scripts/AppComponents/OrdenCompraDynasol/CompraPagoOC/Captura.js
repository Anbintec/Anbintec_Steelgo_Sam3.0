var modeloRenglon;
var esNormal;
var cambiosCheckOK = 0;

function changeLanguageCall() {
    CargarGrid();
    inicio();
    document.title = _dictionary.titleCompraPagoOC[$("#language").data("kendoDropDownList").value()];
};

function inicio() {
    SuscribirEventos();
    AjaxObtenerOrdenesCompra();
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
            
            if (e.values.CantC != undefined) {
                if (!ValidaCantCompra(e.model, e.values)) {
                    e.model.RowOk = false;
                    grid.dataSource.sync();
                    displayNotify("msgErrorCantidadCompraMayor", "", "2");
                }else{
                    e.model.RowOk = true;
                    grid.dataSource.sync();
                }
            } else if (e.values.CantP != undefined) {
                if (!ValidaCantPago(e.model, e.values)) {
                    e.model.RowOk = false;
                    grid.dataSource.sync();
                    displayNotify("msgErrorCantidadPagoMayor", "", "2");
                }else{
                    e.model.RowOk = true;
                    grid.dataSource.sync();
                }
            }
          
            if (e.model.CantC != e.values.CantC || e.model.CantP != e.values.CantP ) {
                e.model.ModificadoPorUsuario = true;
                cambiosCheckOK++;
            } else {
                e.model.ModificadoPorUsuario = false;
            }

        },
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (isEditable(fieldName)) {
                if (!VerificaCantidadSteelgo(e.model)) {
                    displayNotify("msgErrorCantidadSteelgoCero", "", "1");
                    e.preventDefault();
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
                        ItemCode: { type: "string", editable: false },
                        Descripcion: { type: "string", editable: false },
                        MaterialNorma: { type: "string", editable: false },
                        Diametro1: { type: "string", editable: false },
                        Diametro2: { type: "string", editable: false },
                        Schedule: { type: "string", editable: false },
                        Rating: { type: "string", editable: false },
                        PrepExt: { type: "string", editable: false },
                        Partida: { type: "string", editable: true },
                        ColadaID: { type: "int", editable: false },
                        Colada: { type: "string", editable: false },
                        CantS: { type: "string", editable: false },
                        CantC: { type: "number", editable: true, validation: { min: 0, decimals: 0, format: "#", step: 1 } },
                        CantP: { type: "number", editable: true, validation: { min: 0, decimals: 0, format: "#", step: 1 } }                        
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
        },
        edit: function (e) {
            setTimeout(function () {
                var inputName = e.container.find('input');
                inputName.select();
            });
            if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
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
            { field: "ItemCode", title: _dictionary.columnRev[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Descripcion", title: _dictionary.columnDescripcion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "MaterialNorma", title: _dictionary.columnMaterialNorma[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Diametro1", title: _dictionary.columnD1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Diametro2", title: _dictionary.columnD2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Schedule", title: _dictionary.columnSchedule[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Rating", title: _dictionary.columnRating[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
            { field: "PrepExt", title: _dictionary.columnPrepExt[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            { field: "Partida", title: _dictionary.columnPartida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Colada", title: _dictionary.columnColada[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            { field: "CantS", title: _dictionary.columnCantSteelgo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px" },
            { field: "CantC", title: _dictionary.columnCantC[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px" },
            { field: "CantP", title: _dictionary.columnCantP[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px" }
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
            if (esNormal)
                $(".k-grid-content td").css("white-space", "normal");
            else
                $(".k-grid-content td").css("white-space", "nowrap");
        },
        editable: true,
        navigatable: true
    });
    CustomisaGrid($("#grid"));
};

function isEditable(fieldName) {
    if (fieldName === "CantC" || fieldName === "CantP") {
        return true;
    }
    return false;
}
function ValidaCantCompra(model, value) {    
    var CantC = value.CantC;
    if (isNaN(value.CantC || value.CantC == undefined)) {
        CantC = 0;
    }
    if ( parseInt(CantC) > parseInt(model.CantS)) {
        return false;
    } else {
        return true;
    }
}
function ValidaCantPago(model, value) {
    var CantP = value.CantP;
    if (isNaN(value.CantP || value.CantP == undefined)) {
        CantP = 0;
    }
    if (parseInt(CantP) > parseInt(model.CantS)) {
        return false;
    } else {
        return true;
    }
}

function VerificaCantidadSteelgo(model) {
    var CantS = (model.CantS == undefined || isNaN(model.CantS)) ? 0 : model.CantS;
    if (CantS > 0 && CantS != '') {
        return true;
    } else {
        return false;
    }
}