var EmbarquePlanaID = 0;
var esNormal;
IniciarCapturaEmbarqueCarga();
function IniciarCapturaEmbarqueCarga() {
    SuscribirEventos();
}

function changeLanguageCall() {
    CargarGrid();
    document.title = _dictionary.EmbarqueRevisionTituloPagina[$("#language").data("kendoDropDownList").value()];
    AjaxCargarProyecto();
    AjaxCargarCamposPredeterminados();
    opcionHabilitarView(false, "")
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
        edit: function (e) {
            var inputName = e.container.find('input');
            inputName.select();

            if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            }

            if ($(".k-grid-content td").css("white-space") == "normal") {
                esNormal = true;
            }
            else {
                esNormal = false;
            }
        },
        dataSource: {
            schema: {
                model: {
                    fields: {                        
                        NumeroControl: { type: "string", editable: false },                        
                        Paquete: { type: "string", editable: false },
                        Llego: { type: "boolean", editable: true },
                        LlegoComentario: { type: "boolean", editable: true },
                        NoLlego: { type: "boolean", editable: true },
                        Comentario: { type: "string", editable: true },
                        EstuvoEnPaquete: { type: "boolean", editable: false }
                    }
                }
            },            
            filter: {
                logic: "or",                
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 }
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
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "NumeroControl", title: _dictionary.columnSpool[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "110px" },
            { field: "Paquete", title: _dictionary.columnPaqueteEmbarque[$("#language").data("kendoDropDownList").value()], template: "<div class='descargarPaquete' style='text-align:center;'><a href='\\#'> <span>#=Paquete#</span></a></div>", filterable: getGridFilterableCellMaftec(), width: "150px" },
            //{ field: "Paquete", title: _dictionary.columnPaqueteEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "110px" },
            {
                field: "Llego", title: _dictionary.columnLlegoSpool[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:90px;"
                    },
                    dataSource: [{ Llego: true }, { Llego: false }]
                }, template: '<input class="chkbx" type="checkbox" name="Llego" #= Llego ? "checked=checked" : ""# ></input>', width: "130px", attributes: { style: "text-align:center;" }
            },
            {
                field: "LlegoComentario", title: _dictionary.columnLlegoComentariosSpool[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:150px;"
                    },
                    dataSource: [{ LlegoComentario: true }, { LlegoComentario: false }]
                }, template: '<input class="chkbx" type="checkbox" name="LlegoComentario" #= LlegoComentario ? "checked=checked" : "" # ></input>', width: "130px", attributes: { style: "text-align:center;" }
            },
            {
                field: "NoLlego", title: _dictionary.columnNoLlegoSpool[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ NoLlego: true }, { NoLlego: false }]
                }, template: '<input  class="chkbx" type="checkbox" name="NoLlego" #= NoLlego ? "checked=checked" : "" # ></input>', width: "130px", attributes: { style: "text-align:center;" }
            },
            { field: "Comentario", title: _dictionary.columnComentario[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "170px" },
            {
                command: {
                    text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                    click: function (e) {
                        e.preventDefault();
                        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                            if (!$("#InputCerrar").is(":checked")) {
                                e.preventDefault();                                
                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));                                
                                var dataSource = this.dataSource;
                                
                                if (dataItem.Paquete === "NA") { //solo spool manual
                                    if (dataItem.Accion == 1) {
                                        dataSource.remove(dataItem);
                                    }
                                    if (dataItem.Accion == 2) {
                                        dataItem.Accion = 3;
                                        dataItem.ModificadoPorUsuario = true;
                                    } else {
                                        dataSource.remove(dataItem);
                                    }                                    
                                } else {                                    
                                    e.preventDefault();
                                }
                                dataSource.sync();
                            }else
                                displayNotify('EmbarqueRevisionMsjRevisionCerrada', '', '1');
                        }                        
                    }
                },
                title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
                width: "50px",
                attributes: { style: "text-align:center;" }
            }
        ],
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (fieldName == "NoLlego") { e.preventDefault(); }
            if (!isEditable(fieldName, e.model)) {            
                e.preventDefault();
            }
            if (fieldName == "Llego" || fieldName == "LlegoComentario" || fieldName == "NoLlego") {
                if ($("#InputCerrar").is(":checked")) {                                    
                    e.preventDefault();
                }                
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
                }
                else if (gridData[i].RowOk) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");                    
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                }
                
                var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                var editButton = $(currenRow).find(".k-button");
                if (gridData[i].CapturaManual && gridData[i].Paquete == "NA" && !gridData[i].Empaquetado ) {
                    var classDescarga = $("#language").val() == "es-MX" ? "k-grid-Cancelar" : "k-grid-Cancel";
                    editButton[0].outerHTML = '<a class="k-button k-button-icontext ' + classDescarga + '" href="#/"><span class=""></span>' +
                        _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()] + '</a>';

                } else {
                    var classDescarga = $("#language").val() == "es-MX" ? "k-grid-Cancelar" : "k-grid-Cancel";
                    editButton[0].outerHTML = '<a class="k-button k-button-icontext ' + classDescarga + '" style="display:none;" href="#/"><span class=""></span>' +
                        _dictionary.botonDescarga[$("#language").data("kendoDropDownList").value()] + '</a>';
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

    $("#grid").on("change", ":checkbox", function (e) {
        if ($('#Guardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            var dataItem = grid.dataItem($(e.target).closest("tr"));
            if (!$("#InputCerrar").is(":checked")) {
                switch (this.name) {
                    case 'Llego':
                        dataItem.set("Llego", this.checked);
                        dataItem.set("NoLlego", false);
                        dataItem.set("LlegoComentario", false);
                        dataItem.Comentario = "";
                        dataItem.ModificadoPorUsuario = true;
                        grid.dataSource.sync();

                        break;
                    case 'NoLlego':
                        if (!dataItem.CapturaManual) {
                            dataItem.set("Llego", false);
                            dataItem.set("NoLlego", this.checked);
                            dataItem.set("LlegoComentario", false);
                            dataItem.Comentario = "";
                            dataItem.ModificadoPorUsuario = true;
                        }
                        if (dataItem.CapturaManual && dataItem.EstuvoEnPaquete) {
                            dataItem.set("Llego", false);
                            dataItem.set("NoLlego", this.checked);
                            dataItem.set("LlegoComentario", false);
                            dataItem.Comentario = "";
                            dataItem.ModificadoPorUsuario = true;
                        }
                        e.stopPropagation();
                        this.checked = false;
                        grid.dataSource.sync();                        
                        break;
                    case 'LlegoComentario':
                        dataItem.set("Llego", false);
                        dataItem.set("NoLlego", false);
                        dataItem.set("LlegoComentario", this.checked);                                                
                        dataItem.ModificadoPorUsuario = true;
                        grid.dataSource.sync();

                        break;
                }
            } else {
                displayNotify('EmbarqueRevisionMsjRevisionCerrada', '', '1');                
                grid.dataSource.sync();
            }            
        }
        else {
            switch (this.name) {
                case 'Llego':
                    if (e.target.checked)
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Llego = false;
                    else
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Llego = true;

                    break;
                case 'NoLlego':
                    if (e.target.checked)
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).NoLlego = false;
                    else
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).NoLlego = true;
                    break;
                case 'LlegoComentario':
                    if (e.target.checked)
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).LlegoComentario = false;
                    else
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).LlegoComentario = true;
                    break;
            }
            $("#grid").data("kendoGrid").dataSource.sync();
        }
    });
};

function isEditable(fieldName, model) {
    if (fieldName == "Comentario") {
        if(!model.LlegoComentario){
            return false;
        }
    }    
    return true;
}

function ExisteSpool(row) {    
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;
    for (var i = 0; i < jsonGrid.length; i++) {
        if(jsonGrid[i].SpoolID == row[0].SpoolID && jsonGrid[i].Accion == 2){        
            return true;
        } else if ((row[0].Accion == 1 && jsonGrid[i].Accion != 3) && (jsonGrid[i].SpoolID == row[0].SpoolID)) {
            return true;
        }
    }
    return false;    
}

function ExistePaquete(paquete) {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;
    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].Paquete == paquete) {
            return true
        }
    }
    return false;
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
        ds.sync();
    }
}

function existenCambios(ds) {
    if (ds.length > 0) {
        for (var i = 0; i < ds.length; i++) {
            if(ds[i].ModificadoPorUsuario){
                return true;
            }            
        }        
    }
    return false;
}