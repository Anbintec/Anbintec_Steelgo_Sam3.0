function RenderComboBoxInspeccion(container, options) {
    var dataItem;
    var valores;

    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: modeloRenglon.ListaInspeccion,// [{ InspeccionID: 0, Inspeccion: "" }, { InspeccionID: 1, Inspeccion: "Ok" }, { InspeccionID: 2, Inspeccion: "No ok " }],
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.InspeccionID = dataItem.InspeccionID;
                    options.model.Inspeccion = dataItem.Nombre;


                    $("#grid").data("kendoGrid").refresh();
                } else {
                    options.model.InspeccionID = 0;
                    options.model.Inspeccion = "";

                }
            }
        });

}

function RenderComboBoxUnidadMedidaCecilia(container, options) {
    var dataItem;
    var valores;

    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: modeloRenglon.ListaMedidas,// [{ MedidaID: 0, Nombre: "" }, { MedidaID: 1, Nombre: "Mts" }, { MedidaID: 2, Nombre: "Pies" }, { MedidaID: 3, Nombre: "Pza" }],
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.MedidaCeciliaID = dataItem.MedidaID;
                    options.model.MedidaCecilia = dataItem.Nombre;


                    $("#grid").data("kendoGrid").refresh();
                } else {
                    options.model.MedidaCeciliaID = 0;
                    options.model.MedidaCecilia = "";

                }
            }
        });

}

function RenderComboBoxUnidadMedidaGerez(container, options) {
    var dataItem;
    var valores;

    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: modeloRenglon.ListaMedidas,//  [{ MedidaID: 0, Nombre: "" }, { MedidaID: 1, Nombre: "Mts" }, { MedidaID: 2, Nombre: "Pies" }, { MedidaID: 3, Nombre: "Pza" }],
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.MedidaGerezID = dataItem.MedidaID;
                    options.model.MedidaGerez = dataItem.Nombre;


                    $("#grid").data("kendoGrid").refresh();
                } else {
                    options.model.MedidaGerezID = 0;
                    options.model.MedidaGerez = "";

                }
            }
        });

}



function RenderComboBoxUnidadMedidaSteelgo(container, options) {
    var dataItem;
    var valores;

    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: modeloRenglon.ListaMedidas,// [{ MedidaID: 0, Nombre: "" }, { MedidaID: 1, Nombre: "Mts" }, { MedidaID: 2, Nombre: "Pies" }, { MedidaID: 3, Nombre: "Pza" }],
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.MedidaSteelgoID = dataItem.MedidaID;
                    options.model.MedidaSteelgo = dataItem.Nombre;


                    $("#grid").data("kendoGrid").refresh();
                } else {
                    options.model.MedidaSteelgoID = 0;
                    options.model.MedidaSteelgo = "";

                }
            }
        });

}



function RenderCant(container, options) {
    var dataItem;
    $('<input data-text-field="Cant" id=' + options.model.uid + ' data-value-field="Cant" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });
}

function RenderCantG(container, options) {
    var dataItem;
    $('<input data-text-field="CantS" id=' + options.model.uid + ' data-value-field="CantS" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });
}

function RenderCantS(container, options) {
    var dataItem;
    $('<input data-text-field="CantS" id=' + options.model.uid + ' data-value-field="CantS" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });
}
