function RenderGridDetalleOrdenTrabajo(e) {

    var detailRow = e.detailRow;

    var model = e.data;
    kendo.bind(detailRow, model);

    detailRow.find(".gridDetalleOrdenTrabajo").kendoGrid({
        edit: function (e) {
            //   this.closeCell();
        },
        autoBind: true,
        dataSource: {
            data: model.Spools,
            schema: {
                model: {
                    fields: {
                        SpoolNombre: { type: "string", editable: true },
                        Dibujo: { type: "string", editable: false },
                        DiametroMaximo: { type: "string", editable: false },
                        DiametroPromedio: { type: "string", editable: false },
                        Peso: { type: "string", editable: false },
                        Area: { type: "string", editable: false },
                        Juntas: { type: "string", editable: false },
                        Peqs: { type: "string", editable: false }
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
            pageSize: 20,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        navigatable: true,
        filterable: {
            extra: false
        },
        editable: true,
        autoHeight: true,
        sortable: true,
        scrollable: true,
        pageable: {
            refresh: false,
            pageSizes: [10, 15, 20],
            info: false,
            input: false,
            numeric: true,
        },
        columns: [
            { field: "SpoolNombre", title: "Identificador Spool", filterable: true },
            { field: "Dibujo", title: "Dibujo", filterable: true },
            { field: "DiametroMaximo", title: "Diametro Máximo", filterable: true },
            { field: "DiametroPromedio", title: "Diametro Promedio", filterable: true },
            { field: "Peso", title: "Kgs", filterable: true, width: "100px" },
            { field: "Area", title: "M2", filterable: true, width: "100px" },
            { field: "Juntas", title: "Juntas", filterable: true, width: "110px" },
            { field: "Peqs", title: "Peqs", filterable: true, width: "100px" },
            { command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCapturaDetalleSpool }, title: "", width: "99px" }
        ]
    });

    dataBound(e);
}

function dataBound(e) {
    var grid = $(".gridDetalleOrdenTrabajo").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;
    }
}
