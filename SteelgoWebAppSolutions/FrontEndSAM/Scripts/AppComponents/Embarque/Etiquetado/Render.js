function RenderComboBoxCuadrante(container, options) {
    var dataItem;
    var valores;
    if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
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
                        //    if ((options.model.Accion == 1 && !options.model.Etiquetado) || (options.model.Accion == 2 && options.model.Etiquetado))
                        //        options.model.ModificadoPorUsuario = false;
                        //    else
                        //        options.model.ModificadoPorUsuario = true;
                        //}
                        //else                        
                        $("#grid").data("kendoGrid").dataSource.sync();
                    } else {
                        options.model.CuadranteID = 0;
                        options.model.CuadranteSam2ID = 0;
                        options.model.Cuadrante = "";
                        options.model.ModificadoPorUsuario = true;
                        $("#grid").data("kendoGrid").dataSource.sync();
                        //if ((options.model.Accion == 1 && !options.model.Etiquetado) || (options.model.Accion == 2 && options.model.Etiquetado))
                        //    options.model.ModificadoPorUsuario = false;
                        //else                          
                    }
                }
            });
    };
}

function ObtieneCuadranteAnterior(ListaCuadrantes, CuadranteID) {
    var cuadrante = "";
    if (ListaCuadrantes.length > 0) {
        for (var i = 0; i < ListaCuadrantes.length; i++) {
            if(ListaCuadrantes[i].CuadranteID == CuadranteID){
                cuadrante = ListaCuadrantes[i].Nombre;
            }
        }
    }
    return cuadrante;
}