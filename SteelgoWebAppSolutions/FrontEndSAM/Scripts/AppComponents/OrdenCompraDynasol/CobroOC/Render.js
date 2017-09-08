

function RenderPorCobrar(container, options) {
    var max = options.model.CantS - options.model.Cobrado;
    if (max < 0)
        max = 0;
    var dataItem;
    $('<input data-text-field="PorCobrar" id=' + options.model.uid + ' data-value-field="PorCobrar" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0,
        max: max,
        change: function (e) {
            options.model.ModificadoPorUsuario = true;
        }
    });
}
