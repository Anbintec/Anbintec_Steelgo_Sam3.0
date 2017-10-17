IniciaModuloRequisicionInspeccion();
function IniciaModuloRequisicionInspeccion() {
    setTimeout(function () { SuscribirEventos(); }, 1000);
    $('input:radio[name=Planchar]:nth(0)').prop("checked", true);
}
function changeLanguageCall() {
    CargarGrid();
    document.title = _dictionary.menuPinturaSolicitudInspeccion[$("#language").data("kendoDropDownList").value()];
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
            if ($('#btnGuardarSup').text() != _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()])
                this.closeCell();
        },
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Accion: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        SistemaPintura: { type: "string", editable: false },
                        Color: { type: "string", editable: false },
                        FechaSolicitud: { type: "string", editable: false },
                        FechaInspeccion: { type: "date", editable: true },
                        Resultado: { type: "string", editable: true }
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
            { field: "FechaSolicitud", title: _dictionary.columnPinturaFechaSolicitudInsp[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "150px" },
            { field: "FechaInspeccion", title: _dictionary.columnPinturaFechaInspeccion[$("#language").data("kendoDropDownList").value()], filterable: getKendoGridFilterableDateMaftec(), editor: RenderDatePicker, format: _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()], width: "150px" },
            { field: "Resultado", title: _dictionary.columnResultado[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), editor: ComboEstatusSolicitud, width: "130px" },
            {
                command:
                  {
                      text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura,
                      click: function (e) {
                          e.preventDefault();
                          if ($("#btnGuardarSup").text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                              var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                              var dataSource = this.dataSource;
                              if (dataItem.FechaSolicitud != "") {
                                  e.preventDefault();
                                  displayNotify("msgPinturaSolicitudInspEliminar", "", "1");
                              } else if (dataItem.Accion = 2 && (dataItem.FechaInspeccion != "" || dataItem.Resultado != "")) {
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

                                  ventanaConfirm.content(_dictionary.msgPinturaSolicitudInspPreguntaDatosConCaptura[$("#language").data("kendoDropDownList").value()] +
                                      "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                                  ventanaConfirm.open().center();
                                  $("#yesButtonProy").click(function () {
                                      ventanaConfirm.close();
                                      dataItem.Accion = 3;
                                      dataSource.sync();
                                      AjaxGuardarCaptura(dataSource._data, true);
                                  });
                                  $("#noButtonProy").click(function () {
                                      ventanaConfirm.close();
                                  });                                                                
                              } else {
                                  //ELIMINO LOS QUE SON NUEVOS Y NO TIENEN CAPTURA DE FECHA DE INSPECCION O RESULTADO
                                  dataSource.remove(dataItem);
                              }
                              dataSource.sync();
                          }
                      }
                  },
                title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()],
                width: "50px",
                attributes: { style: "text-align:center;" }
            },
            { command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarRenglon }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "50px", attributes: { style: "text-align:center;" } }
        ],
        dataBound: function (a) {
        }
    });
    CustomisaGrid($("#grid"));
}

function eliminarCaptura(e) {
    e.preventDefault();
    if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
        var dataItem = $("#grid").data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
        var dataSource = $("#grid").data("kendoGrid").dataSource;
        dataSource.remove(dataItem);
        dataSource.sync();
    }
}


function limpiarRenglon(e) {
    e.preventDefault();
    if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
        //Limpia Renglon          
        var grid = $("#grid").data("kendoGrid");
        var currentData = grid.dataItem($(e.currentTarget).closest("tr"));
        currentData.FechaInspeccion = "";
        currentData.Resultado = "";
        grid.dataSource.sync();
    }
}
