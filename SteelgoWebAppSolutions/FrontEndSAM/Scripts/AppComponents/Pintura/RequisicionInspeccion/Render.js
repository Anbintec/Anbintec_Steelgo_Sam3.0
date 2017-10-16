function RenderDatePicker(container, options) {
    var dataItem;
    $('<input data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({
            format: "dd/MM/yyyy",
            //max: new Date(),
            change: function (e) {
                var value = this.value();

                if (ValidarFecha(value))
                    options.model.FechaSolicitud = value;                                    
                else
                    options.model.FechaSolicitud = "";
            }
        }
    );
}

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null)
        return false;
    else
        return true;
}