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

function RenderComboBoxInspector(container, options) {
    loadingStart();
    var dataItem;

    $('<input required data-text-field="Codigo" data-value-field="ObreroID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: $("#inputInspector").data("kendoComboBox").dataSource._data,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Codigo#\"></i> #=data.Codigo#",

            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Inspector = dataItem.Codigo;
                    options.model.InspectorID = dataItem.ObreroID;
                    if (options.model.Accion == 4)
                        options.model.Accion = 2;
                }
                else {
                    options.model.Inspector = "";
                    options.model.InspectorID = 0;
                    //options.model.Inspector = ObtenerDescCorrectaInspector(options.model.ListaInspector, options.model.InspectorID);
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        );
    loadingStop();
    $(".k-combobox").on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = this;
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    });
};
function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
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