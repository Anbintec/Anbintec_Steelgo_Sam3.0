function ComboEstatusSolicitud(container, options) {
    var valores;
    $('<input  data-text-field="Descripcion" id=' + options.model.uid + ' data-value-field="Descripcion" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource:
                [
                    { Descripcion: "Aprobado" },
                    { Descripcion: "Rechazado" }
                ],            
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.ModificadoPorUsuario = true;
                    options.model.Resultado = dataItem.Descripcion;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }                
            }
        });
}
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
                    options.model.FechaInspeccion = value;
                else
                    options.model.FechaInspeccion = "";
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