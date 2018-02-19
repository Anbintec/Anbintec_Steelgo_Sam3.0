
function RenderLote(container, options) {
    numeroPlacasComponentesElemento = { NumeroComponentes: options.model.NumeroComponentes };

    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
        var dataItem;
        var textLote = $('<input data-text-field="Lote" id=' + options.model.uid + ' data-value-field="Lote" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoAutoComplete({
        });

        textLote.blur(function () {
            $("#grid").data("kendoGrid").dataSource.sync();
        });
    };
}

function RenderUnidad(container, options) {
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
        $('<input  data-text-field="Unidad" data-value-field="UnidadID" data-bind="value:' + options.field + '"/>')
       .appendTo(container)
       .kendoComboBox({
           suggest: true,
           delay: 10,
           filter: "contains",
           autoBind: true,
           dataSource: arrayUnidadesMedida,
           template: "<i class=\"fa fa-#=data.Unidad.toLowerCase()#\"></i> #=data.Unidad#",
           change: function (e) {
               dataItem = this.dataItem(e.sender.selectedIndex);
               if (dataItem != undefined && dataItem.UnidadID != 0) {
                   options.model.UnidadID = dataItem.UnidadID;
                   options.model.Unidad = dataItem.Unidad;
               }
               $("#grid").data("kendoGrid").dataSource.sync();
           }
       });
    };
}

function renderCantidad(container, options) {
    var inputCantidad = $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                decimals: 0,
                step: 1,
                min: 0,
                format: "#"
            });
    inputCantidad.focus(function () {
        this.select();
    });
};


function renderReductor(container, options) {
    var dataItem;
    $('<input  data-text-field="Reductor" data-value-field="ReductorID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: true,
            dataSource: ListaCatalogoReductores,
            template: "<i class=\"fa fa-#=data.Reductor.toLowerCase()#\"></i> #=data.Reductor#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.ReductorID != 0) {
                    options.model.Reductor = dataItem.Reductor;
                    options.model.ReductorID = dataItem.ReductorID;
                    //options.model.Unidad = dataItem.Unidad;
                    options.model.Accion = options.model.Accion == 0 ? 1 : options.model.Accion;
                }
                else {
                    options.model.Reductor = "";
                    options.model.ReductorID = 0;
                    options.model.Unidad = "";
                    options.model.Accion = options.model.Accion == 2 ? 2 : options.model.Accion;
                
                }
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        );
    $(".k-combobox").parent().on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = $(this).find(".k-combobox")[0];
        if (item != undefined) {
            if (!tieneClase(item)) {
                $(container).trigger(e);
            }
        }
    });
}


