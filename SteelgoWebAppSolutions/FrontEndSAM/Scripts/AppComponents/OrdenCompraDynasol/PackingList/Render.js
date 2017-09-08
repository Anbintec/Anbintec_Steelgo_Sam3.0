function RenderCantPL(container, options) {
    var dataItem;
    $('<input data-text-field="Cant" id=' + options.model.uid + ' data-value-field="Cant" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0,
        max: parseInt(options.model.CantDisponible) 
    });
}