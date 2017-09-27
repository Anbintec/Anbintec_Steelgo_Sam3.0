function RenderComboBoxCuadrante(container, options) {
    var valores;
    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaCuadrantes,
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    // Actualiza el nuevo cuadrante seleccionado
                    options.model.CuadranteID = dataItem.CuadranteID;
                    options.model.CuadranteSam2ID = dataItem.CuadranteSam2ID;
                    options.model.Cuadrante = dataItem.Nombre;
                    options.model.ModificadoPorUsuario = true;

                    //if (options.model.CuadranteAnteriorSam3ID === dataItem.CuadranteID) {
                    //    if ((options.model.Accion == 1 && !options.model.Encintado &&
                    //        options.model.ColorID == options.model.ColorAnteriorID) || (options.model.Accion == 2
                    //        && options.model.Encintado && options.model.ColorID == options.model.ColorAnteriorID))
                    //        options.model.ModificadoPorUsuario = false;
                    //    else
                    //        options.model.ModificadoPorUsuario = true;
                    //} else 
                    
                    $("#grid").data("kendoGrid").dataSource.sync();
                } else {
                    options.model.CuadranteID = 0;
                    options.model.CuadranteSam2ID = 0;
                    options.model.Cuadrante = "";
                    options.model.ModificadoPorUsuario = true;
                    $("#grid").data("kendoGrid").dataSource.sync();
                    //if ((options.model.Accion == 1 && !options.model.Etiquetado && options.model.ColorID == options.model.ColorAnteriorID)
                    //        || (options.model.Accion == 2 && options.model.Etiquetado && options.model.ColorID == options.model.ColorAnteriorID))
                    //    options.model.ModificadoPorUsuario = false;
                    //else

                }
            }
        });
}

function RenderComboBoxColorCinta(container, options) {
    var valores;
    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaColoresCinta,
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.ColorID != 0) {
                    options.model.NombreColor = dataItem.Nombre;
                    options.model.ColorID = dataItem.ColorID;
                    options.model.ModificadoPorUsuario = true;
                   
                    //if (options.model.ColorAnteriorID === dataItem.ColorID) {
                    //    if ((options.model.Accion == 1 && !options.model.Encintado &&
                    //        options.model.CuadranteID == options.model.CuadranteAnteriorSam3ID) || (options.model.Accion == 2
                    //        && options.model.Encintado && options.model.CuadranteID == options.model.CuadranteAnteriorSam3ID))
                    //        options.model.ModificadoPorUsuario = false;
                    //    else
                    //        options.model.ModificadoPorUsuario = true;

                    //} else
                    $("#grid").data("kendoGrid").refresh();
                }
                else {
                    options.model.NombreColor = "";
                    options.model.ColorID = 0;
                    options.model.ModificadoPorUsuario = true;

                    //if (options.model.ColorAnteriorID === options.model.ColorID) {
                    //    if ((options.model.Accion == 1 && !options.model.Encintado &&
                    //        options.model.CuadranteID == options.model.CuadranteAnteriorSam3ID) || (options.model.Accion == 2
                    //        && options.model.Encintado && options.model.CuadranteID == options.model.CuadranteAnteriorSam3ID))
                    //        options.model.ModificadoPorUsuario = false;
                    //    else
                    //        options.model.ModificadoPorUsuario = true;

                    //} else
                }
            }
        });
}

function ObtieneCuadranteAnterior(ListaCuadrantes, CuadranteID) {
    var cuadrante = "";
    if (ListaCuadrantes.length > 0) {
        for (var i = 0; i < ListaCuadrantes.length; i++) {
            if (ListaCuadrantes[i].CuadranteID == CuadranteID) {
                cuadrante = ListaCuadrantes[i].Nombre;
            }
        }
    }
    return cuadrante;
}