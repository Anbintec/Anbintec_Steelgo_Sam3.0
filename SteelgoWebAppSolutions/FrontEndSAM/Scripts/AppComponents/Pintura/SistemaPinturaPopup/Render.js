
function RenderNumeroPruebas(container, options) {
    
        var dataItem;
        var numeroPruebasNumerico = $('<input data-text-field="NumeroPruebas" id=' + options.model.uid + ' data-value-field="NumeroPruebas" data-bind="value:' + options.field + '"/>')
         .appendTo(container)
         .kendoNumericTextBox({
             format: "#",
             min: 0
         });

        numeroPruebasNumerico.focus(function () {
            this.select();
        });
   
}


function comboBoxPruebas(container, options) {
    var dataItem;

    $('<input required data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: window.parent.ListaPruebas == undefined ? [] : window.parent.ListaPruebas,
            dataTextField: "Nombre",
            dataValueField: "PruebaProcesoPinturaID",
            template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.PruebaProcesoPinturaID != undefined) {
                    options.model.PruebaProcesoPinturaID = dataItem.PruebaProcesoPinturaID;
                    options.model.ProyectoProcesoPrueba = dataItem.Nombre;
                    options.model.UnidadMedidaID = dataItem.UnidadMedidaID;
                    options.model.UnidadMedida = dataItem.UnidadMedida;
                    var numeroVecesExiste = 0;
                    for (var i = 0; i < $("#gridPopUp").data("kendoGrid").dataSource._data.length; i++) {
                        if (options.model.PruebaProcesoPinturaID == $("#gridPopUp").data("kendoGrid").dataSource._data[i].PruebaProcesoPinturaID) {
                            numeroVecesExiste++;

                        }
                    }

                    if (numeroVecesExiste > 1) {
                        options.model.PruebaProcesoPinturaID.PruebaProcesoPinturaID = 0;
                        options.model.ProyectoProcesoPrueba = "";
                        displayNotify("CapturaSistemaPinturaAgregarPruebas", "", "1");
                        var dataSource = $("#gridPopUp").data("kendoGrid").dataSource;
                        dataSource.remove(options.model);

                    }
                }
                $("#gridPopUp").data("kendoGrid").dataSource.sync();
            }
        });
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


function RenderUnidadMinima(container, options) {
   

        var dataItem;
        var unidadMinimaNumeric = $('<input data-text-field="UnidadMinima" id=' + options.model.uid + ' data-value-field="UnidadMinima" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "#",
            min: 0,
            change: function (e) {
                //hayDatosCapturados = true;
            }
        });

        unidadMinimaNumeric.focus(function () {
            this.select();
        });
   
}


function RenderUnidadMaxima(container, options) {
    
        var dataItem;
        var unidadMaximaNumeric = $('<input data-text-field="UnidadMaxima" id=' + options.model.uid + ' data-value-field="UnidadMaxima" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoNumericTextBox({
            format: "#",
            min: 0,
            change: function (e) {
                //hayDatosCapturados = true;
            }
        });

        unidadMaximaNumeric.focus(function () {
            this.select();
        });
   
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}