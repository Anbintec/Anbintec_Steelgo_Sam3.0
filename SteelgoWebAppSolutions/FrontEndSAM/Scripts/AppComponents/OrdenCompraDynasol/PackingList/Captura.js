var modeloRenglon;
var esNormal;

function changeLanguageCall() {
    CargarGrid();

    inicio();
};


function inicio() {
    SuscribirEventos();
    AjaxObtenerOrdenesCompra();
}


function CargarGrid() {

    $("#grid").kendoGrid({
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {

                        Rev: { type: "string", editable: false },
                        Descripcion: { type: "string", editable: false },
                        MaterialNorma: { type: "string", editable: false },
                        Diametro1: { type: "string", editable: false },
                        Diametro2: { type: "string", editable: false },
                        Registro: { type: "string", editable: false },
                        Schedule: { type: "string", editable: false },
                        Rating: { type: "string", editable: false },
                        PreparacionExtremos: { type: "string", editable: false },
                        Neodata: { type: "string", editable: false },
                        Cant: { type: "number", editable: false },
                        CantC: { type: "number", editable: false },
                        CantPL: { type: "number", editable: true },
                        PrecioUnidad: { type: "string", editable: false },
                        Total: { type: "string", editable: false },
                        PackingList: { type: "string", editable: true },
                        Partida: { type: "string", editable: false },
                        Colada: { type: "string", editable: false },    
                        Agregar: { type: "boolean", editable: false }

                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 3 },
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
            { field: "Rev", title: _dictionary.columnRev[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Descripcion", title: _dictionary.columnDescripcion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "MaterialNorma", title: _dictionary.columnMaterialNorma[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Diametro1", title: _dictionary.columnD1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Diametro2", title: _dictionary.columnD2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Schedule", title: _dictionary.columnSchedule[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Rating", title: _dictionary.columnRating[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
            { field: "PreparacionExtremos", title: _dictionary.columnPrepExt[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            { field: "Partida", title: _dictionary.columnPartida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Colada", title: _dictionary.columnColada[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "90px", template: "<div class='EnlaceDetalleColada' style='text-align:center;'><a href='\\#'  > <span>#=Colada#</span></a></div> " },
            { field: "Cant", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "60px" },
            { field: "CantC", title: _dictionary.columnCantS[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "70px" },
            { field: "CantPL", title: _dictionary.columnCantPL[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "70px", editor: RenderCantPL, attributes: { style: "text-align:right;" } },
            
            //{ field: "PackingList", title: "Packing List", filterable: getGridFilterableCellMaftec(), width: "100px" },
            
            {
                field: "Agregar", title: _dictionary.columnAgregar[$("#language").data("kendoDropDownList").value()], filterable: {
                    multi: true,
                    messages: {
                        isTrue: _dictionary.lblVerdadero[$("#language").data("kendoDropDownList").value()],
                        isFalse: _dictionary.lblFalso[$("#language").data("kendoDropDownList").value()],
                        style: "max-width:50px;"
                    },

                }, template: "<input name='fullyPaid' class='ob-paid' type='checkbox' data-bind='checked: Agregar' #= Agregar ? checked='checked' : '' # />", width: "112px", attributes: { style: "text-align:center;" }
            },


        ],

        editable: true,
        navigatable: true,
        dataBound: function (a) {
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



            $(".ob-paid").bind("change", function (e) {
                if ($('#botonGuardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
                     grid = $("#grid").data("kendoGrid"),
                        dataItem = grid.dataItem($(e.target).closest("tr"));
                     if (e.target.checked == true) {
                         if (dataItem.Accion == 3) {
                             dataItem.Accion = 2;
                         }
                         dataItem.Agregar = true;
                     }
                     else {
                         if (dataItem.Accion == 2) {
                             dataItem.Accion = 3;
                         }
                         dataItem.Agregar = false;
                     }
                }
                else {
                    if (e.target.checked) {
                        e.target.checked = false;
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = false;
                    }
                    else {
                        e.target.checked = true;
                        $("#grid").data("kendoGrid").dataItem($(e.target).closest("tr")).Agregar = true;
                    }
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            });
        }
    });



    CustomisaGrid($("#grid")); 5
};