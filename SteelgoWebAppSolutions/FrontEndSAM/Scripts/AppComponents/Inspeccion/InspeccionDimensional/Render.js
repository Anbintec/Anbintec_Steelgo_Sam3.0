function RenderMultiSelectJuntas(container, options) {
   
        $('<input  data-text-field="Junta" id=' + options.model.uid + ' data-value-field="JuntaID" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoMultiSelect({
                    autoBind: false,
                    dataSource: options.model.ListaJuntas,
                    template: "<i class=\"fa fa-#=data.Junta.toLowerCase()#\"></i> #=data.Junta#",
                    change: function (e) {
                        // options.model.TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                        options.model.TemplateRender = $("#language").data("kendoDropDownList").value() == "es-MX" ? "Existen " + options.model.ListaJuntasSeleccionadas.length + " Juntas" : "There are " + options.model.ListaJuntasSeleccionadas.length + " board";
                        this.dataSource.sync();
                    },
                    value: options.model.ListaJuntasSeleccionadas
                }).data("kendoMultiSelect");
    
}

function RenderOptionResultado(container, options) {
    loadingStart();
    var dataItem;
    console.log(options);
    $('<input required data-text-field="_Resultado" data-value-field="DefectoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaResultados,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data._Resultado.toLowerCase()#\"></i> #=data._Resultado#",
            
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Resultado = dataItem._Resultado;
                    options.model.ResultadoID = dataItem._ResultadoID;
                    if (options.model.ResultadoID == "1" || options.model.ResultadoID == "" || options.model.ResultadoID == "0" || options.model.ResultadoID == undefined) {
                        options.model.DefectosID = 0;
                        options.model.Defectos = "";
                        options.model.ListaJuntasSeleccionadas = [];
                        options.model.TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                       
                    }

                }
                else {
                    options.model.Resultado = "";
                    options.model.ResultadoID = 0;
                    options.model.DefectosID = 0;
                    options.model.Defectos = "";
                    options.model.ListaJuntasSeleccionadas = [];
                    options.model.TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                    //options.model.Resultado = ObtenerDescCorrectaResultado(options.model.ListaResultados, options.model.ResultadoID);
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




function RenderComboBoxDefectos(container, options) {
    loadingStart();
    var dataItem;

    $('<input required data-text-field="Nombre" data-value-field="DefectoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaDefectos,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
           
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Defectos = dataItem.Nombre;
                    options.model.DefectosID = dataItem.DefectoID;
                    options.model.IDDEFECTOTIPO = dataItem.IDDEFECTOTIPO;
                    options.model.TIPO = dataItem.TIPO;
                    options.model.ListaJuntasSeleccionadas = [];
                    options.model.TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                   
                }
                else {
                    //options.model.Defectos = ObtenerDescCorrectaDefectos(options.model.ListaDefectos, options.model.DefectoID);
                    options.model.Defectos = "";
                    options.model.DefectosID = 0;
                    options.model.IDDEFECTOTIPO = 0;
                    options.model.TIPO = "NoEspecificarJunta";
                    options.model.ListaJuntasSeleccionadas = [];
                    options.model.TemplateRender = _dictionary.NoExistenJuntasSel[$("#language").data("kendoDropDownList").value()];
                    
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

function RenderComboBoxInspector(container, options) {
    loadingStart();
    var dataItem;

    $('<input required data-text-field="Codigo" data-value-field="ObreroID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaInspector,
            suggest: true,
            filter: "contains",
            template: "<i class=\"fa fa-#=data.Codigo#\"></i> #=data.Codigo#",
           
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Inspector = dataItem.Codigo;
                    options.model.InspectorID = dataItem.ObreroID;
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
            return lista[i].Codigo;
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
                var fecha = kendo.toString(value, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
                if (fecha == null) {
                    options.model.FechaInspeccion = '';
                    this.value('');
                }

                $("#grid").data("kendoGrid").dataSource.sync();
            }
        });
}