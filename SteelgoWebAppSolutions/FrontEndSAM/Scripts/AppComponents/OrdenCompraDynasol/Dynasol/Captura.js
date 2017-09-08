var modeloRenglon;
var esNormal;
/*PERFILES:
    1008 ---> CECILIA
    1009 ---> GEREZ
    1010 ---> STEELGO
*/
var PerfilLogueado = Cookies.get("PerfilID");
function changeLanguageCall() {
    CargarGrid();
    CargarGridPopUpDetalleCecilia()
    CargarGridPopUpDetalleGerez();
    CargarGridPopUpDetalleSteelgo();
    CargarGridPopUpDetalleInspeccion();
    inicio();

    //console.log("Usuario Cookie: " + Cookies.get("user"));
    //console.log("PerfilID: " + Cookies.get("PerfilID"));

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
                        Consecutivo: { type: "number", editable: false },
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
                        Cant: { type: "string", editable: false },
                        PrecioUnidad: { type: "string", editable: false },
                        Total: { type: "string", editable: false },
                        PackingList: { type: "string", editable: true },
                        Partida: { type: "string", editable: true },
                        Coladas: { type: "string", editable: false },
                        TemplateDetalleElemento: { type: "string", editable: false },

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
            { field: "Consecutivo", title: _dictionary.columnConsecutivo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "70px" },
            { field: "Rev", title: _dictionary.columnRev[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Descripcion", title: _dictionary.columnDescripcion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "160px" },
            { field: "MaterialNorma", title: _dictionary.columnMaterialNorma[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "130px" },
            { field: "Diametro1", title: _dictionary.columnD1[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Diametro2", title: _dictionary.columnD2[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "40px" },
            { field: "Schedule", title: _dictionary.columnSchedule[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Rating", title: _dictionary.columnRating[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
            { field: "PreparacionExtremos", title: _dictionary.columnPrepExt[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            //{ field: "Neodata", title: "Neodata", filterable: getGridFilterableCellMaftec(), width: "90px" },
            { field: "Cant", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "60px" },
            //{ field: "PrecioUnidad", title: _dictionary.columnPrecioU[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px" },
            //{ field: "Total", title: _dictionary.columnTotal[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "70px" },
            //{ field: "PackingList", title: "Packing List", filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Partida", title: _dictionary.columnPartida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
            { field: "Coladas", title: " ", filterable: false, width: "50px", template: "<div class='EnlaceDetalleColada' style='text-align:center;'> <img src='/Content/images/SAMC_ComplementoIcon.png'></img></div> " },
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

function CargarGridPopUpDetalleCecilia() {

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

    $("#gridPopUpCecilia").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [

            ],
            schema: {
                model: {
                    fields: {

                        Colada: { type: "string", editable: true },
                        Cant: { type: "number", editable: true },
                        InspeccionDetalle: { type: "string", editable: false },
                        Comentario: { type: "string", editable: false },


                    }
                }
            }, filter: {
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
        navigatable: true,
        filterable: {
            extra: false
        },
        click: function (e) {
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          //Cecilia
          { field: "Colada", title: _dictionary.columnColada[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
          { field: "MedidaCecilia", title: _dictionary.DynasolColumnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "73px", editor: RenderComboBoxUnidadMedidaCecilia },
          { field: "Cant", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "73px", editor: RenderCant, attributes: { style: "text-align:right;" } },
          
          { field: "InspeccionDetalle", title: _dictionary.columnInsp[$("#language").data("kendoDropDownList").value()], filterable: false, width: "70px", template: "<div class='EnlaceInspeccion' style='text-align:center;'><a href='javascript:void(0)'  > <span>#=InspeccionDetalle == '' ? 'Sin Inspeccion': InspeccionDetalle #</span></a></div> " },
          { field: "Comentario", title: _dictionary.columnComentarioDynasol[$("#language").data("kendoDropDownList").value()], filterable: false, width: "80px", },
          //Gerez


          {
              command: {
                  text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                  click: function (e) {
                      e.preventDefault();
                      var dataItem = $("#gridPopUpCecilia").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

                      if (dataItem.Accion != 2 ) {
                          $("#gridPopUpCecilia").data("kendoGrid").dataSource.remove(dataItem);
                      }
                      else {
                          dataItem.Accion = 3;
                      }

                      $("#gridPopUpCecilia").data("kendoGrid").dataSource.sync();
                  }
              },
              title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
              width: "50px", attributes: { style: "text-align:center;" }
          },

        ],
        editable: "incell",
        toolbar: [{ name: "create" }],
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },


    });
    CustomisaGrid($("#gridPopUpCecilia"));

};

function CargarGridPopUpDetalleGerez() {

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

    $("#gridPopUpGerez").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [

            ],
            schema: {
                model: {
                    fields: {

                        Colada: { type: "string", editable: false },
                        Cant: { type: "number", editable: false },
                        MedidaCecilia: { type: "string", editable: false },
                        InspeccionDetalle: { type: "string", editable: false },
                        Comentario: { type: "string", editable: false },

                        CantG: { type: "number", editable: true },
                        FechaRecibidoG: { type: "date", editable: true },
                        Camion: { type: "string", editable: true },
                        FacturaProveedor: { type: "string", editable: true },
                        Acuerdo: { type: "string", editable: true },
                        FechaEnvio: { type: "date", editable: true },
                        Pedimento: { type: "string", editable: true },
                        ShippingDate: { type: "date", editable: true },

                    }
                }
            }, filter: {
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
        navigatable: true,
        filterable: {
            extra: false
        },
        click: function (e) {
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          //Cecilia
          { field: "Colada", title: _dictionary.columnColada[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
          { field: "Cant", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "65px", editor: RenderCant, attributes: { style: "text-align:right;" } },
          { field: "MedidaCecilia", title: _dictionary.DynasolColumnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "73px", editor: RenderComboBoxUnidadMedidaCecilia },
          { field: "InspeccionDetalle", title: _dictionary.columnInsp[$("#language").data("kendoDropDownList").value()], filterable: false, width: "70px", template: "<div class='EnlaceInspeccion' style='text-align:center;'><a href='javascript:void(0)'  > <span>#=InspeccionDetalle == '' ? 'Sin Inspeccion': InspeccionDetalle #</span></a></div> " },
          { field: "Comentario", title: _dictionary.columnComentarioDynasol[$("#language").data("kendoDropDownList").value()], filterable: false, width: "80px", },
          //Gerez
          
          { field: "CantG", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "73px", editor: RenderCantG, attributes: { style: "text-align:right;" } },
          { field: "MedidaGerez", title: _dictionary.DynasolColumnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "65px", editor: RenderComboBoxUnidadMedidaGerez },
          { field: "FechaRecibidoG", title: _dictionary.columnFechaRecibido[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "82px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
          //{ field: "Camion", title: _dictionary.columnCamion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
          { field: "FacturaProveedor", title: _dictionary.columnFacturaProveedor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
          { field: "Acuerdo", title: _dictionary.columnAcuerdo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
          { field: "FechaEnvio", title: _dictionary.columnFechaEnvio[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "82px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
          { field: "Pedimento", title: _dictionary.columnPedimento[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "73px" },
          { field: "ShippingDate", title: _dictionary.columnShippingDate[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },

          

        ],
        editable: "incell",
        //toolbar: [{ name: "create" }]
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },
    });
    CustomisaGrid($("#gridPopUpGerez"));

};

function CargarGridPopUpDetalleSteelgo() {

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

    $("#gridPopUpSteelgo").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [

            ],
            schema: {
                model: {
                    fields: {

                        Colada: { type: "string", editable: false },
                        Cant: { type: "number", editable: false },
                        MedidaCecilia: { type: "string", editable: false },
                        InspeccionDetalle: { type: "string", editable: false },
                        Comentario: { type: "string", editable: false },

                        CantG: { type: "number", editable: false },
                        MedidaGerez: { type: "string", editable: false },
                        FechaRecibidoG: { type: "date", editable: false },
                        Camion: { type: "string", editable: false },
                        FacturaProveedor: { type: "string", editable: false },
                        Acuerdo: { type: "string", editable: false },
                        FechaEnvio: { type: "date", editable: false },
                        Pedimento: { type: "string", editable: false },
                        ShippingDate: { type: "date", editable: false },

                        CantS: { type: "number", editable: true },
                        FechaFactura: { type: "date", editable: true },
                        InspeccionS: { type: "string", editable: true },
                        FechaRecibidoS: { type: "date", editable: true },
                    }
                }
            }, filter: {
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
        navigatable: true,
        filterable: {
            extra: false
        },
        click: function (e) {
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          //Cecilia
          { field: "Colada", title: _dictionary.columnColada[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
          { field: "Cant", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "73px", editor: RenderCant, attributes: { style: "text-align:right;" } },
          { field: "MedidaCecilia", title: _dictionary.DynasolColumnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "60px", editor: RenderComboBoxUnidadMedidaCecilia },
          { field: "InspeccionDetalle", title: _dictionary.columnInsp[$("#language").data("kendoDropDownList").value()], filterable: false, width: "65px", template: "<div class='EnlaceInspeccion' style='text-align:center;'><a href='\\#'  > <span>#=InspeccionDetalle == '' ? 'Sin Inspeccion': InspeccionDetalle #</span></a></div> " },
          { field: "Comentario", title: _dictionary.columnComentarioDynasol[$("#language").data("kendoDropDownList").value()], filterable: false, width: "80px", },
          //Gerez
          
          { field: "CantG", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "73px", editor: RenderCantG, attributes: { style: "text-align:right;" } },
          { field: "MedidaGerez", title: _dictionary.DynasolColumnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "60px", editor: RenderComboBoxUnidadMedidaGerez },
          { field: "FechaRecibidoG", title: _dictionary.columnFechaRecibido[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "82px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
          //{ field: "Camion", title: _dictionary.columnCamion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "90px" },
          { field: "FacturaProveedor", title: _dictionary.columnFacturaProveedor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "95px" },
          { field: "Acuerdo", title: _dictionary.columnAcuerdo[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px" },
          { field: "FechaEnvio", title: _dictionary.columnFechaEnvio[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "82px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
          { field: "Pedimento", title: _dictionary.columnPedimento[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "73px" },
          { field: "ShippingDate", title: _dictionary.columnShippingDate[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "100px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
          //Steelgo
          
          { field: "CantS", title: _dictionary.columnCant[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "73px", editor: RenderCantS, attributes: { style: "text-align:right;" } },
          { field: "MedidaSteelgo", title: _dictionary.DynasolColumnUnidadMedida[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "60px", editor: RenderComboBoxUnidadMedidaSteelgo },
          { field: "FechaRecibidoS", title: _dictionary.columnFechaRecibido[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "82px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },
          { field: "InspeccionS", title: _dictionary.columnInsp[$("#language").data("kendoDropDownList").value()], filterable: false, width: "50px" },
          { field: "FechaFactura", title: _dictionary.columnFechaFactura[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "82px", format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()] },

        

        ],
        editable: "incell",
        //toolbar: [{ name: "create" }]
        beforeEdit: function (e) {
            var columnIndex = this.cellIndex(e.container);
            var fieldName = this.thead.find("th").eq(columnIndex).data("field");
            if (!isEditable(fieldName, e.model)) {
                e.preventDefault();
            }
        },

    });
    CustomisaGrid($("#gridPopUpSteelgo")); 


};


function isEditable(fieldName, model) {
    if (fieldName === "MedidaGerez") {
        if (modeloRenglon.EsTuberia == 0) {
            return false;
        }
    }

    else if (fieldName === "MedidaSteelgo") {
        if (modeloRenglon.EsTuberia == 0) {
            return false;
        }
    }
    else if (fieldName === "MedidaCecilia") {
        if (modeloRenglon.EsTuberia == 0) {
            return false;
        }
    }
    return true; // default to editable
}


function CargarGridPopUpDetalleInspeccion() {

    $("#gridPopUpInspeccion").kendoGrid({
        ////autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Inspeccion: { type: "string", editable: true },
                        Comentario: { type: "string", editable: true },
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
        selectable: true,
        filterable: getGridFilterableMaftec(),
        columns: [
          { field: "Inspeccion", title: "Inspección", filterable: getGridFilterableCellMaftec(), width: "100px", editor: RenderComboBoxInspeccion },
          { field: "Comentario", title: "Comentario", filterable: getGridFilterableCellMaftec(), width: "100px" },
          {
              command: {
                  text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()],
                  click: function (e) {
                      e.preventDefault();
                      var dataItem = $("#gridPopUpInspeccion").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));

                      if ((dataItem.Accion == 1) || (dataItem.Accion == 0)) {
                          $("#gridPopUpInspeccion").data("kendoGrid").dataSource.remove(dataItem);
                      }
                      else {
                          dataItem.Accion = 3;
                      }

                      $("#gridPopUpInspeccion").data("kendoGrid").dataSource.sync();
                  }
              },
              title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
              width: "50px", attributes: { style: "text-align:center;" }
          },

        ],
        editable: true,
        toolbar: [{ name: "create" }],
        navigatable: true,
        dataBound: function (a) {

        },


    });
    CustomisaGrid($("#gridPopUpInspeccion"));

};





function LlenarGridPopUpDetalleColadas(data) {
    modeloRenglon = data;

    //$("#gridPopUpSteelgo").data('kendoGrid').dataSource.data([]);

    //if ($("#gridPopUpSteelgo").data('kendoGrid').dataSource._filter == undefined) {
    //    //investigar porque al destruir una ventana se eliminan solo los filtros.
    //    $("#gridPopUpSteelgo").data('kendoGrid').dataSource._filter = {
    //        logic: "or",
    //        filters: [
    //          { field: "Accion", operator: "eq", value: 1 },
    //          { field: "Accion", operator: "eq", value: 2 },
    //            { field: "Accion", operator: "eq", value: 0 },
    //            { field: "Accion", operator: "eq", value: undefined }
    //        ]
    //    };
    //}

    if (PerfilLogueado != undefined) {
        if (PerfilLogueado == 1008 || PerfilLogueado == "1008") { //CECILIA
                $("#gridPopUpCecilia").data('kendoGrid').dataSource.data([]);
                var ds = $("#gridPopUpCecilia").data("kendoGrid").dataSource;
                var array = data.ListaDetalleColadas;

                for (var i = 0; i < array.length; i++) {
                    ds.add(array[i]);
                }
                $("#gridPopUpSteelgo").data('kendoGrid').dataSource.sync();
                VentanaModalDetalleCecilia();
        } else if (PerfilLogueado == 1009 || PerfilLogueado == "1009") { //GEREZ
            $("#gridPopUpGerez").data('kendoGrid').dataSource.data([]);
                var ds = $("#gridPopUpGerez").data('kendoGrid').dataSource;
                var array = data.ListaDetalleColadas;
                for (var i = 0; i < array.length; i++) {
                    ds.add(array[i]);
                }
                VentanaModalDetalleGerez();
        } else if (PerfilLogueado == 1010 || PerfilLogueado == "1010") { //STEELGO
            $("#gridPopUpSteelgo").data('kendoGrid').dataSource.data([]);
                var ds = $("#gridPopUpSteelgo").data("kendoGrid").dataSource;
                var array = data.ListaDetalleColadas;
                for (var i = 0; i < array.length; i++) {
                    ds.add(array[i]);
                }
                VentanaModalDetalleSteelgo();
        } else { //NINGUNO DE LOS 3 PUEDE SER SAMADMIN U OTROS
            $("#gridPopUpSteelgo").data('kendoGrid').dataSource.data([]);
                var ds = $("#gridPopUpSteelgo").data("kendoGrid").dataSource;
                var array = data.ListaDetalleColadas;
                for (var i = 0; i < array.length; i++) {
                    ds.add(array[i]);
                }
                VentanaModalDetalleSteelgo();
        }
    }

    //if ($('input:radio[name=Muestra]:nth(0)').prop('checked')) {
    //    $("#gridPopUpCecilia").data('kendoGrid').dataSource.data([]);
    //    var ds = $("#gridPopUpCecilia").data("kendoGrid").dataSource;
    //    var array = data.ListaDetalleColadas;

    //    for (var i = 0; i < array.length; i++) {
    //        ds.add(array[i]);
    //    }
    //    $("#gridPopUpSteelgo").data('kendoGrid').dataSource.sync();
    //    VentanaModalDetalleCecilia();

    //}
    //else if ($('input:radio[name=Muestra]:nth(1)').prop('checked')) {
    //    $("#gridPopUpGerez").data('kendoGrid').dataSource.data([]);
    //    var ds = $("#gridPopUpGerez").data('kendoGrid').dataSource;
    //    var array = data.ListaDetalleColadas;
    //    for (var i = 0; i < array.length; i++) {
    //        ds.add(array[i]);
    //    }
    //    VentanaModalDetalleGerez();
    //}
    //else if ($('input:radio[name=Muestra]:nth(2)').prop('checked')) {
    //    $("#gridPopUpSteelgo").data('kendoGrid').dataSource.data([]);

    //    var ds = $("#gridPopUpSteelgo").data("kendoGrid").dataSource;
    //    var array = data.ListaDetalleColadas;

    //    for (var i = 0; i < array.length; i++) {
    //        ds.add(array[i]);
    //    }

    //    VentanaModalDetalleSteelgo();
    //}




}

var coladaRow;
function LlenarGridPopUpDetalleInspeccion(data) {

    coladaRow = data;
    var ds;
    $("#gridPopUpInspeccion").data('kendoGrid').dataSource.data([]);
    ds = $("#gridPopUpInspeccion").data("kendoGrid").dataSource;


    var array = data.ListaDetalleInspeccion;

    if (array != undefined)
        for (var i = 0; i < array.length; i++) {
            ds.add(array[i]);
        }
    VentanaModalDetalleInspeccion();
}

function VentanaModalDetalleGerez() {

    var modalTitle = "";
    modalTitle = modeloRenglon.Descripcion;
    var window = $("#windowGridGerez");
    var win = window.kendoWindow({
        modal: true,
        title: "",
        resizable: false,
        visible: true,
        width: "82%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        animation: {
            close: false,
            open: false
        },

    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};

function VentanaModalDetalleCecilia() {

    var modalTitle = "";
    modalTitle = modeloRenglon.Descripcion;
    var window = $("#windowGridCecilia");
    var win = window.kendoWindow({
        modal: true,
        title: "",
        resizable: false,
        visible: true,
        width: "40%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        animation: {
            close: false,
            open: false
        },
        actions: [],
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};


function VentanaModalDetalleSteelgo() {

    var modalTitle = "";
    modalTitle = modeloRenglon.Descripcion;
    var window = $("#windowGridSteelgo");

    var win = window.kendoWindow({
        modal: true,
        title: "",
        resizable: false,
        visible: true,
        width: "105%",//tipo == 1 ? "30%" : tipo == 2 ? "80%" : "99%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        content: "texto texto texto y mas texto",
        animation: {
            close: false,
            open: false
        },
        actions: []
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();
    win.element.scrollTop(0);

};


function VentanaModalDetalleInspeccion() {

    var modalTitle = "";
    modalTitle = _dictionary.DynasolInspeccionTitulo[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowGridInspeccion");
    var win = window.kendoWindow({
        modal: true,
        title: "",
        resizable: false,
        visible: true,
        width: "59%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        content: "texto texto texto y mas texto",
        actions: [],
        animation: {
            close: false,
            open: false
        },
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};
