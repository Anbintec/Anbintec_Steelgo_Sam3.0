function RenderComboBoxDocumentoRecibido(container, options) {
    var dataItem;
    var textAnterior;
    $('<input  data-text-field="DocumentoRecibidoNombre" data-value-field="DocumentoRecibidoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.ListaRecibido,
            template: "<i class=\"fa fa-#=data.DocumentoRecibidoNombre#\"></i> #=data.DocumentoRecibidoNombre#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
                textAnterior = e.sender._prev;
            }
            ,
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.DocumentoRecibido = dataItem.DocumentoRecibidoNombre;
                    options.model.DocumentoRecibidoID = dataItem.DocumentoRecibidoID;
                    options.model.EstatusCaptura = 1;
                }
                else {
                    options.model.DocumentoRecibido = "";
                    options.model.DocumentoRecibidoID = 0;
                }
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

function RenderComboBoxDocumentoEstatus(container, options) {
    var dataItem;
    var textAnterior;
    $('<input  data-text-field="DocumentoEstatusNombre" data-value-field="DocumentoEstatusID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.ListaEstatusDocumento,
            template: "<i class=\"fa fa-#=data.DocumentoEstatusNombre#\"></i> #=data.DocumentoEstatusNombre#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
                textAnterior = e.sender._prev;
            }
            ,
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.DocumentoEstatus = dataItem.DocumentoEstatusNombre;
                    options.model.DocumentoEstatusID = dataItem.DocumentoEstatusID;
                    options.model.EstatusCaptura = 1;

                    if (dataItem.DocumentoEstatusID != 2) {
                        options.model.DocumentoDefectoID = 0;
                        options.model.DocumentoDefecto = "";
                    }
                        
                }
                else {
                    options.model.DocumentoEstatus = ObtenerDocumentoEstatusCorrecto(options.model.ListaEstatusDocumento, options.model.DocumentoEstatusID);
                }
                $("#grid").data("kendoGrid").dataSource.sync();
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

function RenderComboBoxDefectoDocumento(container, options) {

    var dataItem;
    var textAnterior;
    $('<input  data-text-field="DocumentoDefectoNombre" data-value-field="DocumentoDefectoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: modeloRenglon.ListaDefectoDocumento,
            template: "<i class=\"fa fa-#=data.DocumentoDefectoNombre#\"></i> #=data.DocumentoDefectoNombre#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.DocumentoDefecto = dataItem.DocumentoDefectoNombre;
                    options.model.DocumentoDefectoID = dataItem.DocumentoDefectoID;
                    options.model.EstatusCaptura = 1;
                }
                else {
                    options.model.DocumentoDefecto = ObtenerDocumentoDefectoCorrecto(options.model.ListaDefectoDocumento, options.model.DocumentoDefectoID);
                }
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

function ObtenerDocumentoRecibidoCorrecto(lista, DocumentoRecibidoID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].DocumentoRecibidoID == DocumentoRecibidoID)
            return lista[i].DocumentoRecibidoNombre;
    }
    return "";
}

function ObtenerDocumentoEstatusCorrecto(lista, DocumentoEstatusID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].DocumentoEstatusID == DocumentoEstatusID)
            return lista[i].DocumentoEstatusNombre;
    }
    return "";
}

function ObtenerDocumentoDefectoCorrecto(lista, DocumentoDefectoID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].DocumentoDefectoID == DocumentoDefectoID)
            return lista[i].DocumentoDefectoNombre;
    }
    return "";
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}



function RenderComboBoxPlaca(container, options) {
    var dataItem;
    var textAnterior;
    $('<input  data-text-field="Placa" data-value-field="CapturaResultadoPlacaID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: ModeloRenglon.ListaPlacas,
            template: "<i class=\"fa fa-#=data.Placa#\"></i> #=data.Placa#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
                textAnterior = e.sender._prev;
            }
            ,
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Placa = dataItem.Placa;
                    options.model.CapturaResultadoPlacaID = dataItem.CapturaResultadoPlacaID;
                    options.model.EstatusCaptura = 1;
                }
                else {
                    options.model.Placa = "";
                    options.model.PlacaID = 0;
                }
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


function RenderRecibida(container, options) {
    var dataItem;
    $('<input data-text-field="Recibida" id=' + options.model.uid + ' data-value-field="Recibida" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0,
        max: parseInt(options.model.Cantplacas)
    });
}

