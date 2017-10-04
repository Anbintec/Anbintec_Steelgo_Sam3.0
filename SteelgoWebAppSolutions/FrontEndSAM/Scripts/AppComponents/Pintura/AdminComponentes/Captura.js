var ListaCatalogoComponentes = null;

function changeLanguageCall() {

    CargarGrid();
    AjaxDetalleGridComponentes();
    AjaxObtenerCatalogoComponentes();
    mostrarConfirmacionVentanaModal();
   
    document.title = _dictionary.CapturaAdminComponentesTituloPagina[$("#language").data("kendoDropDownList").value()];
};

setTimeout(function () { suscribirEventos(); }, 100);

 
function CargarGrid() {
    $("#grid").kendoGrid({
        edit: function (e) {
           

            if ($('#Guardar').text() != _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
                this.closeCell();
            }

            setTimeout(function () {
                var inputName = e.container.find('input');
                inputName.select();
            });
        },
        autoBind: true,
        dataSource: {
            schema: {
                model: {
                    fields: {
                        Componente: { type: "string", editable: true },
                        Lote: { type: "string", editable: true },
                        Cantidad: { type: "number", editable: true},
                        Unidad: { type: "string", editable: false },
                        RowOk: { type: "boolean", editable: false },
                        Accion: { type: "number", editable: false },
                        AdminComponentesID: { type: "number", editable: false }
                    }
                }
            },
            filter: {
                logic: "or",
                filters: [
                  { field: "Accion", operator: "eq", value: 1 },
                  { field: "Accion", operator: "eq", value: 2 },
                  { field: "Accion", operator: "eq", value: 0 },
                  { field: "Accion", operator: "eq", value: 4 },
                  { field: "Accion", operator: "eq", value: undefined }
                ]
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        filterable: getGridFilterableMaftec(),
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        columns: [
            { field: "Componente", title: _dictionary.columnComponente[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "150px", editor: renderComponente },
            { field: "Lote", title: _dictionary.columnLote[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px", attributes: { style: "text-align:left;" }, editor: RenderLote },
            { field: "Cantidad", title: _dictionary.columnCantidad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), editor: Cantidad, width: "80px", attributes: { style: "text-align:right;" }},
            { field: "Unidad", title: _dictionary.columnUnidad[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "50px", attributes: { style: "text-align:left;" } },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCaptura }, title: _dictionary.columnELM[$("#language").data("kendoDropDownList").value()], width: "40px", attributes: { style: "text-align:center;" } }//,
            //{ command: { text: _dictionary.botonLimpiar[$("#language").data("kendoDropDownList").value()], click: limpiarCaptura }, title: _dictionary.columnLimpiar[$("#language").data("kendoDropDownList").value()], width: "40px", attributes: { style: "text-align:center;" } }
        ],
        dataBound: function () {
            var grid = $("#grid").data("kendoGrid");
            var gridData = grid.dataSource.view();

            for (var i = 0; i < gridData.length; i++) {
                var currentUid = gridData[i].uid;
                if (!gridData[i].RowOk) {
                    grid.table.find("tr[data-uid='" + currentUid + "']").css("background-color", "#ffffff");
                }
                else  {
                    grid.table.find("tr[data-uid='" + currentUid + "']").css("background-color", "#ffcccc");
                }
               

            }
        },
        toolbar: [{ name: "create" }]
    });

    $(".k-grid-add").addClass("k-add-button");

    $("#grid table").on("keydown", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            //var grid = $("#gridPopUp").data("kendoGrid");
            //var length = grid.dataSource.view().length;
            //var currentIndexRow = $("#gridPopUp").data("kendoGrid").items().index($("#gridPopUp").data("kendoGrid").select());
            //if (currentIndexRow == length - 1) {
            var dataSource = $("#grid").data("kendoGrid").dataSource;
            var total = $("#grid").data("kendoGrid").dataSource.data().length;
            $("#grid").data("kendoGrid").dataSource.insert(total, {});
            $("#grid").data("kendoGrid").dataSource.page(dataSource.totalPages());
            $("#grid").data("kendoGrid").editRow($("#grid").data("kendoGrid").tbody.children().last());

            //}
        }
    });
    CustomisaGrid($("#grid"));
};

function tieneClase(item) {

    var tieneClass = $(item).hasClass("k-state-border-up") || $(item).hasClass("k-state-border-down");
    return tieneClass;
}

function ValidarValoresRepetidos(data) {
    var fila, encontroRepetido = false;
    for (var i = 0; i < data.length; i++) {
        fila = 0;
        data[i].RowOk = false;
  
        for (var j = 0; j < data.length; j++) {
            if ((data[i].ComponenteID == data[j].ComponenteID && data[i].Lote == data[j].Lote && data[i].Cantidad == data[j].Cantidad && data[i].Unidad == data[j].Unidad) && (data[i].Accion == 1 || data[i].Accion == 2 || data[i].Accion == undefined))
                fila++;
        }

        if (fila > 1) {
            data[i].RowOk = true;
            encontroRepetido = true;
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
    return encontroRepetido;
};



function ValoresBlanco(data) {
    var valorEncontradoVacio = false;
    for (var i = 0; i < data.length; i++) {
        data[i].RowOk = false;
        data[i].ModificacionAutomatica = true;
        if ((data[i].ComponenteID == 0 && data[i].Lote == "" && data[i].Cantidad == 0 && data[i].Unidad == "") && (data[i].Accion != 3)) {
            data[i].Accion = 4;
        }
        else if (!(data[i].ComponenteID != 0 && data[i].Lote != "" && data[i].Cantidad != 0 && data[i].Unidad != "") && data[i].Accion != 3) {
            data[i].RowOk = true;
            valorEncontradoVacio = true;
            data[i].Accion = data[i].Accion == 4 ? 2 : data[i].Accion;
        }
    }
    $("#grid").data("kendoGrid").dataSource.sync();
    return valorEncontradoVacio;
}


function opcionHabilitarView(valor, name) {

    if (valor) {
        $("#botonGuardar2").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar3').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $(".k-add-button").addClass("k-state-disabled").removeClass("k-grid-add");
    }
    else {
        $("#botonGuardar2").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar3').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $(".k-add-button").removeClass("k-state-disabled").addClass("k-grid-add");
    }
}


