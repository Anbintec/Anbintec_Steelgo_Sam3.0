var cambioAlgoGrid = false;
var esNormal;

function changeLanguageCall() {
    CargarGrid();
    AjaxCargarCamposPredeterminados();
    document.title = _dictionary.EmbarqueEtiquetadoTituloPagina[$("#language").data("kendoDropDownList").value()];
    opcionHabilitarView(false, "FieldSetView");
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
                        Accion: { type: "number", editable: false },
                        Proyecto: { type: "string", editable: false },
                        Spool: { type: "string", editable: false },
                        Cuadrante: { type: "string", editable: true },
                        OkPnd: { type: "boolean", editable: false },
                        OkPintura: { type: "boolean", editable: false },
                        Etiquetado: { type: "boolean", editable: true }
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
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        navigatable: true,
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
            { field: "Proyecto", title: _dictionary.columnProyecto[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Spool", title: _dictionary.columnSpoolIDEmbarque[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Cuadrante", title: _dictionary.columnCuadranteEmbarque[$("#language").data("kendoDropDownList").value()], editor: RenderComboBoxCuadrante, filterable: getGridFilterableCellMaftec(), width: "100px" },
            {
                field: "OkPnd", title: _dictionary.columnOkPND[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ OkPND: true }, { OkPND: false }]
                }, template: "<input name='fullyPaid' class='chk-Lectura' type='checkbox' data-bind='checked: OkPnd' #= OkPnd ? checked='checked' : '' # disabled/>", width: "70px", attributes: { style: "text-align:center;" }
            },
            {
                field: "OkPintura", title: _dictionary.columnOkPintura[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ OkPintura: true }, { OkPintura: false }]
                }, template: "<input name='fullyPaid' class='chk-Lectura' type='checkbox' data-bind='checked: OkPintura' #= OkPintura ? checked='checked' : '' # disabled/>", width: "80px", attributes: { style: "text-align:center;" }
            },
            {
                field: "Etiquetado", title: _dictionary.columnEtiquetadoEmbarque[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:100px;"
                    },
                    dataSource: [{ Etiquetado: true }, { Etiquetado: false }]
                }, template: "<input name='fullyPaid' class='chk-Etiquetado' type='checkbox' data-bind='checked: Etiquetado' #= Etiquetado ? checked='checked' : '' #/>", width: "80px", attributes: { style: "text-align:center;" }
            },
        ],
        dataBound: function (e) {
            if (esNormal) {
                $(".k-grid-content td").css("white-space", "normal");
            }
            else {
                $(".k-grid-content td").css("white-space", "nowrap");
            }
        }
    });

    $("#grid").on("change", ":checkbox", function (e) {
        e.preventDefault();
        if ($(this)[0].className == "chk-Etiquetado" || $(this)[0].name == "Etiquetado") {
            if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {

                var grid = $("#grid").data("kendoGrid");
                var dataItem = grid.dataItem($(e.target).closest("tr"));

                if ($(this)[0].checked) {
                    dataItem.Etiquetado = true;

                    if (dataItem.Accion == 1)//|| (dataItem.Accion == 2 && dataItem.CuadranteID != dataItem.CuadranteAnteriorSam3ID)
                        dataItem.ModificadoPorUsuario = true;
                    //else
                    //    dataItem.ModificadoPorUsuario = false;
                }
                else {
                    dataItem.Etiquetado = false;

                    if (dataItem.Accion == 2)//|| (dataItem.Accion == 1 && dataItem.CuadranteID != dataItem.CuadranteAnteriorSam3ID)
                        dataItem.ModificadoPorUsuario = true;
                    //else
                    //    dataItem.ModificadoPorUsuario = false;
                }
            }
            else {
                if (e.target.checked)
                    $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Etiquetado = false;
                else
                    $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Etiquetado = true;
            }
        }       

        $("#grid").data("kendoGrid").refresh();
    });

    CustomisaGrid($("#grid"));
};

function existenCambios() {
    var ds = $("#grid").data("kendoGrid").dataSource._data;
    if (ds.length > 0) {

        for (var i = 0; i < ds.length; i++) {
            if (ds[i].ModificadoPorUsuario == true)
                return true;
        }
    }

    return false;
}

function PlanchaEtiquedo(Etiquetado) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() == "Todos") {
            if (Etiquetado == "Si") {
                data[i].Etiquetado = true;
                data[i].ModificadoPorUsuario = true;

                //if (data[i].Accion == 1|| (data[i].Accion == 2 && data[i].CuadranteID != data[i].CuadranteAnteriorSam3ID))
                //    data[i].ModificadoPorUsuario = true;
                //else
                //    data[i].ModificadoPorUsuario = false;
            } else if (Etiquetado == "No") {
                data[i].Etiquetado = false;
                data[i].ModificadoPorUsuario = true;

                //if (data[i].Accion == 2 || (data[i].Accion == 1 && data[i].CuadranteID != data[i].CuadranteAnteriorSam3ID))
                //    data[i].ModificadoPorUsuario = true;
                //else 
                //    data[i].ModificadoPorUsuario = false;
            }
        } else if ($('input:radio[name=LLena]:checked').val() == "Vacios") {
            if (!data[i].Etiquetado) {
                if (Etiquetado == "Si") {
                    data[i].Etiquetado = true;
                    data[i].ModificadoPorUsuario = true;

                    //if (data[i].Accion == 1 || (data[i].Accion == 2 && data[i].CuadranteID != data[i].CuadranteAnteriorSam3ID))
                    //    data[i].ModificadoPorUsuario = true;
                     //else 
                     //   data[i].ModificadoPorUsuario = false;
                }
            }
        }
    }

    $("#grid").data("kendoGrid").refresh();
}

function PlanchaCuadrante(Cuadrante) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;

    for (var i = 0; i < data.length; i++) {
        if ($('input:radio[name=LLena]:checked').val() == "Todos") {
            data[i].CuadranteID = Cuadrante.CuadranteID;
            data[i].CuadranteSam2ID = Cuadrante.CuadranteSam2ID;
            data[i].Cuadrante = Cuadrante.Nombre;
            data[i].ModificadoPorUsuario = true;

            //if (data[i].CuadranteAnteriorSam3ID != Cuadrante.CuadranteID) {
            //    if ((data[i].Accion == 1 && !data[i].Etiquetado) || (data[i].Accion == 2 && data[i].Etiquetado))
            //        data[i].ModificadoPorUsuario = false;
            //    else
            //        data[i].ModificadoPorUsuario = true;
            //}
            //else            
        }
        else if ($('input:radio[name=LLena]:checked').val() == "Vacios") {
            if (data[i].Cuadrante == "" || data[i].Cuadrante === null || data[i].Cuadrante === undefined) {
                data[i].CuadranteID = Cuadrante.CuadranteID;
                data[i].CuadranteSam2ID = Cuadrante.CuadranteSam2ID;
                data[i].Cuadrante = Cuadrante.Nombre;
                data[i].ModificadoPorUsuario = true;

                //if (data[i].CuadranteAnteriorSam3ID === Cuadrante.CuadranteID) {
                //    if ((data[i].Accion == 1 && !data[i].Etiquetado) || (data[i].Accion == 2 && data[i].Etiquetado))
                //        data[i].ModificadoPorUsuario = false;
                //    else
                //        data[i].ModificadoPorUsuario = true;
                //}
                //else
            }
        }
    }

    $("#grid").data("kendoGrid").refresh();
}

function FiltroMostrar(mostrar) {
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (mostrar == 0) {
        var curr_filters = ds.filter().filters;
        if (curr_filters[0].filters != undefined)
            ds.filter(curr_filters[0].filters[0])
        else
            ds.filter(curr_filters[0])
        $("#grid").data("kendoGrid").refresh();
    }
    else {
        var curr_filters = ds.filter().filters;
        ds.filter(curr_filters[0])
        $("#grid").data("kendoGrid").refresh();
        var filters = ds.filter();
        filters.logic = "or"

        filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
        $("#grid").data("kendoGrid").refresh();
    }
}