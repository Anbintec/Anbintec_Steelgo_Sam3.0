function RenderGridDetalleSpool(e) {

    var detailRow = e.detailRow;

    var model = e.data;
    kendo.bind(detailRow, model);
    //model.bind('change', function (e) {
    //    var tr = $('tr[data-uid=' + model.uid + ']');
    //    $('#grid').data().kendoGrid.expandRow(tr);
    //})

    detailRow.find(".gridDetalleSpool").kendoGrid({
        edit: function (e) {
            //   this.closeCell();
        },
        autoBind: true,
        dataSource: {
            data: [
                {
                    Peso: "350",
                    Area: "623",
                    M2: "623",
                    Juntas: "4",
                    Peqs: "162"
                }
            ],
            schema: {
                model: {
                    fields: {
                        Peso: { type: "string", editable: false },
                        Area: { type: "string", editable: false },
                        M2: { type: "string", editable: false },
                        Juntas: { type: "string", editable: false },
                        Peqs: { type: "string", editable: false }
                    }
                }
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
            { field: "Peso", title: "Kgs", filterable: true, width: "100px" },
            { field: "Area", title: "Area", filterable: true, width: "100px" },
            { field: "M2", title: "M2", filterable: true, width: "100px" },
            { field: "Juntas", title: "Juntas", filterable: true, width: "110px" },
            { field: "Peqs", title: "Peqs", filterable: true, width: "100px" },
            //{ command: { text: _dictionary.botonCancelar[$("#language").data("kendoDropDownList").value()], click: eliminarCapturaDetalleSpool }, title: "", width: "99px" }
        ]
    });

    dataBound(e);
}

function dataBound(e) {
    var grid = $(".gridDetalleSpool").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;

        if (gridData[i].Proyectado != 1) {
            var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
            var editButton = $(currenRow).find(".k-grid-Cancelar");

            editButton.hide();
        }
    }
}

function RenderComboBoxAcciones(container, options) {
    loadingStart();

    var dataItem;
    $('<input  data-text-field="Accion" id=' + options.model.uid + ' data-value-field="AccionID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaAcciones,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Accion.toLowerCase()#\"></i> #=data.Accion#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
            },
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
            }
        }
        );
    loadingStop();
}