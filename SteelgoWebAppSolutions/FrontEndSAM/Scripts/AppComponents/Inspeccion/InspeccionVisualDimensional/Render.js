function RenderComboBoxTaller(container, options) {

    var dataItem;
    $('<input  data-text-field="Nombre" data-value-field="TallerID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaTaller,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Taller = dataItem.Nombre
                    options.model.TallerID = dataItem.TallerID,
                    options.model.Accion = options.model.Accion == 4 ? 2 : options.model.Accion;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.Taller = ObtenerDescCorrectaTaller(options.model.ListaTaller, options.model.TallerID);
                }
            },
            open: function (e) {
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

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}



function RenderComboBoxInspector(container, options) {
    var dataItem;

    $('<input required data-text-field="NombreCompleto" data-value-field="ObreroID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaInspector,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.NombreCompleto#\"></i> #=data.NombreCompleto#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Inspector = dataItem.NombreCompleto;
                    options.model.InspectorID = dataItem.ObreroID;
                    options.model.Accion = options.model.Accion == 4 ? 2 : options.model.Accion;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.Inspector = ObtenerDescCorrectaInspector(options.model.ListaInspector, options.model.InspectorID);
                }
            }
        }
        );
    $(".k-combobox").on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = this;
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    });
};

function RenderComboBoxDefectos(container, options) {
    var dataItem;

    $('<input required data-text-field="Nombre" data-value-field="DefectoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaDefectos,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Defectos = dataItem.Nombre;
                    options.model.DefectosID = dataItem.DefectoID;
                    options.model.Accion = options.model.Accion == 4 ? 2 : options.model.Accion;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.Defectos = ObtenerDescCorrectaDefectos(options.model.ListaDefectos, options.model.DefectoID);
                }
            }
        }
        );
    $(".k-combobox").on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = this;
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    });
};

function RenderComboBoxNumeroUnico1(container, options) {
    var dataItem;
    var textAnterior;
    $('<input required data-text-field="Clave" data-value-field="NumeroUnicoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaNumerosUnicos1,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Clave#\"></i> #=data.Clave#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
                if (dataItem != undefined) {
                    dataItem = this.dataItem(e.item.index());
                    //options.model.NumeroUnico1 = String(dataItem.Clave);
                    //options.model.NumeroUnico1ID = dataItem.NumeroUnicoID;
                    textAnterior = e.sender._prev;
                }
            }
            ,
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.Etiqueta != "") {
                    var combobox = dataItem;
                    var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;
                    var rowitem = options.model;

                    if (elNUSeEncuentraEnJuntasNoAgregadasGrid(combobox, jsonGridArmado, rowitem)) {
                        for (var i = 0; i < jsonGridArmado.length; i++) {
                            if (//combobox.JuntasEncontradas != '' &&
                                ((jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) &&
                                (jsonGridArmado[i].Junta == rowitem.Junta)) {
                                jsonGridArmado[i].NumeroUnico1 = '';
                                jsonGridArmado[i].NumeroUnico1ID = null;
                            }
                        }
                        if (combobox.JuntasEncontradas != '')
                            MensajesSteelGO("AvisoNumeroUnicoYaAsignado", combobox.JuntasEncontradas);
                    } else {
                        var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;
                        AplicarAsignacionAutomaticaNumeroUnico(options.model, textAnterior, dataItem, 0, jsonGridArmado, options.model.ListaNumerosUnicos1.length);
                    }
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.NumeroUnico1 = "";
                    options.model.NumeroUnico1ID = 0;
                    //$("#grid").data("kendoGrid").dataSource.sync();
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
};

function RenderComboBoxNumeroUnico2(container, options) {
    var dataItem;
    var textAnterior;
    $('<input required data-text-field="Clave" data-value-field="NumeroUnicoID" data-bind="value:' + options.field + '"/>')
         .appendTo(container)
         .kendoComboBox({
             autoBind: false,
             dataSource: options.model.ListaNumerosUnicos2,
             suggest: true,
             filter: "contains",
             template: "<i class=\"fa fa-#=data.Clave#\"></i> #=data.Clave#",
             select: function (e) {
                 dataItem = this.dataItem(e.item.index());
                 if (dataItem != undefined) {
                     dataItem = this.dataItem(e.item.index());
                     //options.model.NumeroUnico2 = String(dataItem.Clave);
                     //options.model.NumeroUnico2ID = dataItem.NumeroUnicoID;
                     textAnterior = e.sender._prev;
                 }
             },
             change: function (e) {
                 dataItem = this.dataItem(e.sender.selectedIndex);
                 if (dataItem != undefined && dataItem.Etiqueta != "") {
                     var combobox = dataItem;
                     var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;
                     var rowitem = options.model;

                     if (elNUSeEncuentraEnJuntasNoAgregadasGrid(combobox, jsonGridArmado, rowitem)) {
                         for (var i = 0; i < jsonGridArmado.length; i++) {
                             if (//combobox.JuntasEncontradas != '' &&
                               ((jsonGridArmado[i].IdOrdenTrabajo + '-' + jsonGridArmado[i].IdVal) == (rowitem.IdOrdenTrabajo + '-' + rowitem.IdVal)) &&
                               (jsonGridArmado[i].Junta == rowitem.Junta)) {
                                 jsonGridArmado[i].NumeroUnico2 = '';
                                 jsonGridArmado[i].NumeroUnico2ID = null;
                             }
                         }
                         if (combobox.JuntasEncontradas != '')
                             MensajesSteelGO("AvisoNumeroUnicoYaAsignado", combobox.JuntasEncontradas);
                     } else {
                         var jsonGridArmado = $("#grid").data("kendoGrid").dataSource._data;
                         AplicarAsignacionAutomaticaNumeroUnico(options.model, textAnterior, dataItem, 0, jsonGridArmado, options.model.ListaNumerosUnicos2.length);
                     }
                     $("#grid").data("kendoGrid").dataSource.sync();
                 }
                 else {
                     options.model.NumeroUnico2 = "";
                     options.model.NumeroUnico2ID = 0;
                     // $("#grid").data("kendoGrid").dataSource.sync();
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
};

function RenderOptionResultado(container, options) {
    var dataItem;
    console.log(options);
    $('<input  data-text-field="_Resultado" data-value-field="DefectoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaResultados,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data._Resultado#\"></i> #=data._Resultado#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Resultado = dataItem._Resultado;
                    options.model.ResultadoID = dataItem._ResultadoID;
                    if (options.model.ResultadoID == "1") {
                        options.model.DefectosID = 0;
                        options.model.Defectos = "";
                        options.model.Accion = options.model.Accion == 4 ? 2 : options.model.Accion;
                       
                    }
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.Resultado = ObtenerDescCorrectaResultado(options.model.ListaResultados, options.model.ResultadoID);
                }

            }
        }
        );
    $(".k-combobox").on('mouseleave', function (send) {
        var e = $.Event("keydown", { keyCode: 27 });
        var item = this;
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    });
};


function ObtenerDescCorrectaTaller(lista, TallerID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].TallerID == TallerID)
            return lista[i].Taller;
    }
    return "";
}
function ObtenerDescCorrectaResultado(lista, ResultadoID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].ResultadoID == ResultadoID)
            return lista[i].Resultado;
    }
    return "";
}


function ObtenerDescCorrectaDefectos(lista, DefectoID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].DefectoID == DefectoID)
            return lista[i].Nombre;
    }
    return "";
}

function ObtenerDescCorrectaInspector(lista, InspectorID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].ObreroID == InspectorID)
            return lista[i].NombreCompleto;
    }
    return "";
}


function ObtenerDescCorrectaNumeroUnico(lista, NumeroUnicoID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].NumeroUnicoID == NumeroUnicoID)
            return String(lista[i].Clave);
    }
    return "";
}

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
                options.model.FechaInspeccion = value;

                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
        );

}