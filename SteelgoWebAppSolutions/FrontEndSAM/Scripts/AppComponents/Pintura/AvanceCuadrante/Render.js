function RenderDatePicker(container, options) {
    //container  contiene las propiedades de la celda
    //options contiene el modelo del datasource ejemplo options.model.Junta
    var dataItem;

    $('<input   data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({
            max: new Date(),
            change: function () {
                var value = this.value();
                options.model.FechaProceso = value;

                $("#grid").data("kendoGrid").refresh();
            }
        }
        );

}

function RendercomboBoxPintor(container, options) {
    options.model.ListaObreros = $("#inputPintor").data("kendoMultiSelect").dataSource._data;

   $('<input  data-text-field="Codigo" id=' + options.model.uid + ' data-value-field="ObreroID" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoMultiSelect({
                autoBind: true,
                dataTextField: "Codigo",
                dataValueField: "ObreroID",
                dataSource: options.model.ListaObreros,
                template: "<i class=\"fa fa-#=data.Codigo.toLowerCase()#\"></i> #=data.Codigo#",
                change: function (e) {
                    options.model.plantillaObrero = _dictionary.CapturaAvancePintoresShotblastExistentes[$("#language").data("kendoDropDownList").value()] + options.model.ListaObrerosSeleccionados.length;
                    $("#grid").data("kendoGrid").refresh();
                },
                value: options.model.ListaObrerosSeleccionados //[{ "Accion": "2", "AvanceCarroObreroId": "1", "Codigo	": "T-133", "ObreroID": "22" }]
            });

   $(".k-multiselect").on('mouseleave', function (send) {
       var e = $.Event("keydown", { keyCode: 27 });
       var item = this;
       if (!tieneClase(item)) {
           $(container).trigger(e);
       }
   });
  
}

function renderComboboxComponenteDinamico(container, options) {
    var ListadoLotesComponentes;
    for (var i = 0; i < ComponentesDinamicosJSON.length; i++) {
        if (ComponentesDinamicosJSON[i].NombreColumna == options.field) {
            ListadoLotesComponentes = ComponentesDinamicosJSON[i].ListadoLotes;
        }
    }

    $('<input  data-text-field="NombreLote" id=' + options.model.uid + ' data-value-field="NombreLote" data-bind="value:' + options.field + '"/>')
           .appendTo(container)
           .kendoComboBox({
               dataTextField: "NombreLote",
               dataValueField: "NombreLote",
               dataSource: ListadoLotesComponentes,
               suggest: true,
               filter: "contains",
               change: function (e) {
                   dataItem = this.dataItem(e.sender.selectedIndex);
                   if (dataItem != undefined && dataItem.NombreLote != "") {
                       options.model[options.field] = dataItem.NombreLote
                   }
                   // $("#grid").data("kendoGrid").dataSource.sync();
               }
           });

    $(".k-combobox").on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = this;
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    });
}

function RendercomboReductor(container, options) {
    var ListadoLotesReductor;
    for (var i = 0; i < ReductoresDinamicosJSON.length; i++) {
        if (ReductoresDinamicosJSON[i].NombreColumna == options.field) {
            ListadoLotesReductor = ReductoresDinamicosJSON[i].ListadoLotes;

        }
    }
    $('<input  data-text-field="NombreLote" id=' + options.model.uid + ' data-value-field="NombreLote" data-bind="value:' + options.field + '"/>')
         .appendTo(container)
         .kendoComboBox({
             dataTextField: "NombreLote",
             dataValueField: "NombreLote",
             dataSource: ListadoLotesReductor,
             suggest: true,
             filter: "contains",
             change: function (e) {
                 dataItem = this.dataItem(e.sender.selectedIndex);
                 if (dataItem != undefined && dataItem.NombreLote != "") {
                     options.model[options.field] = dataItem.NombreLote;
                 }
             }
         });

    $(".k-combobox").on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = this;
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    });
}