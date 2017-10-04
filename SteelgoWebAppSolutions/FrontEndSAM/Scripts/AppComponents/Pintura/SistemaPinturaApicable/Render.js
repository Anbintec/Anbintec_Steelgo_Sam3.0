function comboBoxSistemaPintura(container, options) {
    var dataItem;
    if (options.model.ListaSistemPintura.length > 1) {
        $('<input required data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                autoBind: false,
                dataSource: options.model.ListaSistemPintura,
                dataTextField: "Nombre",
                dataValueField: "SistemaPinturaID",
                template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
                change: function (e) {
                    dataItem = this.dataItem(e.sender.selectedIndex);
                    if (dataItem != undefined) {
                        options.model.SistemaPinturaID = dataItem.SistemaPinturaID;
                        options.model.NoPintable = dataItem.NoPintable;
                        options.model.SistemaPintura = dataItem.Nombre;
                        options.model.EstatusCaptura = 1;

                        options.model.ListaColorPintura = [];
                        options.model.ColorPinturaID = 0;
                        options.model.SistemaPinturaColorID = 0;
                        options.model.Color = "";

                        if (dataItem.SistemaPinturaID != 0 && !dataItem.NoPintable) {
                            AjaxCargarColorPinturaRender(dataItem.SistemaPinturaID, options);
                        }

                    } else {
                        options.model.SistemaPintura = ObtenerSistemaPinturaCorrecto(options.model.ListaSistemPintura, options.model.SistemaPinturaID);
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
}

function comboBoxColor(container, options) {
    var dataItem;
    if (options.model.ListaColorPintura.length > 1) {
        $('<input required data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                autoBind: false,
                dataSource: options.model.ListaColorPintura,
                dataTextField: "Nombre",
                dataValueField: "ColorPinturaID",
                template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
                change: function (e) {
                    dataItem = this.dataItem(e.sender.selectedIndex);
                    if (dataItem != undefined) {
                        options.model.ColorPinturaID = dataItem.ColorPinturaID;
                        options.model.SistemaPinturaColorID = dataItem.SistemaPinturaColorID;
                        options.model.Color = dataItem.Nombre;
                        options.model.EstatusCaptura = 1;
                    } else {
                        options.model.Color = ObtenerColorPinturaCorrecto(options.model.ListaColorPintura, options.model.ColorPinturaID);
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
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}

function ObtenerSistemaPinturaCorrecto(lista, SistemaPinturaID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].SistemaPinturaID == SistemaPinturaID)
            return lista[i].Nombre;
    }
    return "";
}

function ObtenerColorPinturaCorrecto(lista, ColorPinturaID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].ColorPinturaID == ColorPinturaID)
            return lista[i].Nombre;
    }
    return "";
}

function Diametro(container, options) {
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
        var dataItem;
        var Diametro = $('<input data-text-field="Diametro" id=' + options.model.uid + ' data-value-field="Diametro" data-bind="value:' + options.field + '"/>')
         .appendTo(container)
         .kendoNumericTextBox({
             format: "#",
             min: 0
         });

        Diametro.focus(function () {
            this.select();
        });
    };
}