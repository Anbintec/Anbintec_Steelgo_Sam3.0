function RenderDatePicker(container, options) {
    var dataItem;

    $('<input   data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({
            max: new Date(),
            change: function () {
                var value = this.value();
                options.model.FechaProceso = value;
            }
        }
        );

}

function RenderAprobado(container, options) {
    var dataItem;
    $('<input   id=' + options.model.uid + '  data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "0",
            change: function () {
                var value = this.value();
                options.model.Aprobado = value >= options.model.MetrosLoteProcesoPinturaID && value <= options.model.MetrosLoteProcesoPinturaID ? "Aprobado" : "Rechazado";
                $("#gridPopUp").data("kendoGrid").dataSource.sync();
            }
        });
}

function RenderMedida(container, options) {
  
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {

        var dataItem;
        var numeroComponentesNumeric = $('<input data-text-field="UnidadMedida" id=' + options.model.uid + ' data-value-field="UnidadMedida" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "#",
            min: 0,
            change: function (e) {
                var value = this.value();
                options.model.ResultadoEvaluacion = (parseFloat(value) >= parseFloat(gridRow.UnidadMinima) && parseFloat(value) <= parseFloat(gridRow.UnidadMaxima)) ? true : false;
                $("#gridPopUp").data("kendoGrid").dataSource.sync();
            }
        });

        numeroComponentesNumeric.focus(function () {
            this.select();
        });
    };
}