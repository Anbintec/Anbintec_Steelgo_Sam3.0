function RenderEspesorRelleno(container, options) {
    var dataItem;
    $('<input data-text-field="EspesorRelleno" id=' + options.model.uid + ' data-value-field="EspesorRelleno" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });
}

function RenderEspesorRaiz(container, options) {
    var dataItem;
    $('<input data-text-field="EspesorRaiz" id=' + options.model.uid + ' data-value-field="EspesorRaiz" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });
}

function RenderComboBoxProcesoSoldaduraRaiz(container, options) {
    loadingStart();
    var dataItem;
    $('<input data-text-field="Codigo" id=' + options.model.uid + ' data-value-field="Codigo" data-bind="value:' + options.field + '"/>').appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.ListaProcesosSoldadura,
            template: "<i class=\"fa fa-#=data.Codigo#\"></i> #=data.Codigo#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
                options.model.CodigoRaiz = dataItem.Codigo,
                options.model.ProcesoSoldaduraRaizID = dataItem.ProcesoSoldaduraID
            },
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem.Codigo == "N/A") {
                    options.model.EspesorRaiz = 0;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else if (dataItem != undefined) {
                    options.model.CodigoRaiz = dataItem.Codigo,
                    options.model.ProcesoSoldaduraRaizID = dataItem.ProcesoSoldaduraID
                }

                else {
                    options.model.CodigoRaiz = "";
                    options.model.ProcesoSoldaduraRaizID = 0;
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

    loadingStop();
}

function ObtenerDescCorrectaSoldaduraRaiz(lista, procesoSoldaduraRaizID) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].ProcesoSoldaduraID == procesoSoldaduraRaizID)
            return lista[i].Codigo;
    }
    return "";
}


function RenderComboBoxProcesoSoldaduraRelleno(container, options) {
    loadingStart();
    var dataItem;
    $('<input data-text-field="Codigo" id=' + options.model.uid + ' data-value-field="Codigo" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                suggest: true,
                delay: 10,
                filter: "contains",
                autoBind: false,
                dataSource: options.model.ListaProcesosSoldadura,
                template: "<i class=\"fa fa-#=data.Codigo#\"></i> #=data.Codigo#",
                select: function (e) {
                    dataItem = this.dataItem(e.item.index());
                    options.model.CodigoRelleno = dataItem.Codigo;
                    options.model.ProcesoSoldaduraRellenoID = dataItem.ProcesoSoldaduraID;
                },
                change: function (e) {
                    dataItem = this.dataItem(e.sender.selectedIndex);
                    if (dataItem.Codigo == "N/A") {
                        options.model.EspesorRelleno = 0;
                        $("#grid").data("kendoGrid").dataSource.sync();
                    }
                    else if (dataItem != undefined) {
                        options.model.CodigoRelleno = dataItem.Codigo;
                        options.model.ProcesoSoldaduraRellenoID = dataItem.ProcesoSoldaduraID;
                        //AjaxActualizaSoldadoresRelleno(dataItem.ProcesoSoldaduraID, ItemSeleccionado.TipoJunta, ItemSeleccionado.Diametro, ItemSeleccionado.Espesor, ItemSeleccionado.Cedula);
                    }
                    else {
                        options.model.CodigoRelleno = "";
                        options.model.ProcesoSoldaduraRellenoID = 0;
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
    loadingStop();
}

function RenderComboBoxMaterialesBase1(container, options) {
    loadingStart();
    var dataItem;
    var materialBase1 = options.model.GrupoPMaterialBase1;
    var materialBase1Nombre = options.model.GrupoPMaterialBase1Nombre;
    $('<input data-text-field="GrupoP" id=' + options.model.uid + ' data-value-field="GrupoP" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                suggest: true,
                delay: 10,
                filter: "contains",
                autoBind: false,
                dataSource: options.model.ListaMaterialesBase,
                template: "<i class=\"fa fa-#=data.GrupoP#\"></i> #=data.GrupoP#",
                //select: function (e) {
                //    dataItem = this.dataItem(e.item.index());
                //    options.model.GrupoPMaterialBase1 = dataItem.GrupoPID,
                //    options.model.GrupoPMaterialBase1Nombre = dataItem.GrupoP
                //},
                change: function (e) {
                    dataItem = this.dataItem(e.sender.selectedIndex);
                    if (dataItem != undefined) {
                        if (options.model.RegistrosWPS == 0 ) {
                            options.model.GrupoPMaterialBase1 = dataItem.GrupoPID;
                            options.model.GrupoPMaterialBase1Nombre = dataItem.GrupoP;
                        }
                        else {
                            displayNotify("lblPQRAsignarGrupoP", "", 1);
                            options.model.GrupoPMaterialBase1 = materialBase1;
                            options.model.GrupoPMaterialBase1Nombre = materialBase1Nombre;
                            $("#grid").data("kendoGrid").dataSource.sync();

                        }
                    }
                    else {
                        options.model.GrupoPMaterialBase1 = 0;
                        options.model.GrupoPMaterialBase1Nombre = "";
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
    loadingStop();
}

function RenderComboBoxMaterialesBase2(container, options) {
    loadingStart();
    var dataItem;
    var materialBase2 = options.model.GrupoPMaterialBase2;
    var materialBase2Nombre = options.model.GrupoPMaterialBase1Nombre;
    $('<input data-text-field="GrupoP" id=' + options.model.uid + ' data-value-field="GrupoP" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                suggest: true,
                delay: 10,
                filter: "contains",
                autoBind: false,
                dataSource: options.model.ListaMaterialesBase,
                template: "<i class=\"fa fa-#=data.GrupoP#\"></i> #=data.GrupoP#",
                //select: function (e) {
                //    dataItem = this.dataItem(e.item.index());
                //    options.model.GrupoPMaterialBase2 = dataItem.GrupoPID,
                //    options.model.GrupoPMaterialBase2Nombre = dataItem.GrupoP
                //},
                change: function (e) {
                    dataItem = this.dataItem(e.sender.selectedIndex);
                    if (dataItem != undefined) {
                        if (options.model.RegistrosWPS == 0) {
                            options.model.GrupoPMaterialBase2 = dataItem.GrupoPID;
                            options.model.GrupoPMaterialBase2Nombre = dataItem.GrupoP;
                        }
                        else {
                            displayNotify("lblPQRAsignarGrupoP", "", 1);
                            options.model.GrupoPMaterialBase2 = materialBase2;
                            options.model.GrupoPMaterialBase2Nombre = materialBase2Nombre;
                            $("#grid").data("kendoGrid").dataSource.sync();

                        }
                    }
                    else {
                        options.model.GrupoPMaterialBase2 = 0;
                        options.model.GrupoPMaterialBase2Nombre = "";
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
    loadingStop();
}

function RenderComboBoxEspecificacion(container, options) {
    loadingStart();
    var dataItem;
    $('<input data-text-field="Especificacion" id=' + options.model.uid + ' data-value-field="Especificacion" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                suggest: true,
                delay: 10,
                filter: "contains",
                autoBind: false,
                dataSource: options.model.ListaCodigos,
                template: "<i class=\"fa fa-#=data.Especificacion#\"></i> #=data.Especificacion#",
                change: function (e) {
                    dataItem = this.dataItem(e.sender.selectedIndex);
                    if (dataItem != undefined) {
                        options.model.CodigoASMEID = dataItem.CodigoAsmeID,
                        options.model.Especificacion = dataItem.Especificacion
                    }
                    else {
                        options.model.CodigoASMEID = 0;
                        options.model.Especificacion = "";
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
    loadingStop();
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false;
}

function RenderComboBoxTipoPrueba(container, options) {
    var dataItem;
    $('<input  data-text-field="TipoDePrueba" data-value-field="TipoPruebaID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.listaTipoPrueba,
            template: "<i class=\"fa fa-#=data.TipoDePrueba#\"></i> #=data.TipoDePrueba#",
            change: function (e) {
                e.preventDefault();
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.TipoPrueba = dataItem.TipoDePrueba;
                    options.model.TipoPruebaID = dataItem.TipoPruebaID;
                }
                else {
                    // options.model.Taller = ObtenerDescCorrectaTaller(options.model.ListaTaller, options.model.TallerID);
                    options.model.TipoPrueba = "";
                    options.model.TipoPruebaID = null;
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

function RenderComboBoxTipoJunta(container, options) {
    var dataItem;
    $('<input  data-text-field="Nombre" data-value-field="TipoJuntaID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.listaTipoJunta,
            template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.TipoJunta = dataItem.Nombre;
                    options.model.TipoJuntaID = dataItem.TipoJuntaID;
                }
                else {
                    // options.model.Taller = ObtenerDescCorrectaTaller(options.model.ListaTaller, options.model.TallerID);
                    options.model.TipoJunta = "";
                    options.model.TipoJuntaID = null;
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