function RenderComboBoxDestino(container, options) {
    var valores;
    $('<input  data-text-field="Destino" id=' + options.model.uid + ' data-value-field="Destino" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.listaDestino,            
            template: "<i class=\"fa fa-#=data.Destino.toLowerCase()#\"></i> #=data.Destino#",
            change: function (e) {
                //e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);
                
                if (dataItem != undefined && dataItem.DestinoID != 0) {
                    options.model.Destino = dataItem.Destino;
                    options.model.DestinoID = dataItem.DestinoID;
                    options.model.ModificadoPorUsuario = true;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.Destino = "";
                    options.model.DestinoID = 0;
                    options.model.ModificadoPorUsuario = true;                    
                }
                SetValueEnviar(options.model) ? options.model.Enviar = true : options.model.Enviar = false;                
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        });
}

function SetValueEnviar(obj) {
    var retorno = false;
    if (obj != undefined) {
        if (!obj.RequierePapCliente && (obj.Destino != "" && obj.Destino != null && obj.Destino != undefined) && !obj.RequierePermisoAduana && obj.RequiereRevisionCliente && !obj.OkClienteEmbarque && !obj.OkCliente && obj.OkEmbarque) { //crossover
            retorno = true; 
        } else if (!obj.RequierePapCliente && (obj.Destino != "" && obj.Destino != null && obj.Destino != undefined) && (obj.FolioSolicitudPermiso != "" && obj.FolioSolicitudPermiso != null && noHayFolio) && (obj.FechaSolicitudPermiso != "" && obj.FechaSolicitudPermiso != null) && (obj.AprobadoAduanaDesc == "Aprobado") && obj.RequierePermisoAduana && obj.RequiereRevisionCliente && !obj.OkClienteEmbarque && !obj.OkCliente && obj.OkEmbarque) { //pesqueria
            retorno = true;            
        } else if (obj.RequierePapCliente && (obj.Destino != "" && obj.Destino != null && obj.Destino != undefined) && !obj.RequierePermisoAduana && obj.RequiereRevisionCliente && obj.OkClienteEmbarque && obj.OkCliente && obj.OkEmbarque) { //ramones            
            retorno = true;            
        } else if (obj.RequierePapCliente && obj.DestinoID != 0 && (obj.FolioSolicitudPermiso != "" && obj.FolioSolicitudPermiso != null) && (obj.FechaSolicitudPermiso != "" && obj.FechaSolicitudPermiso != null) && (obj.AprobadoAduanaDesc == "Aprobado") && obj.RequierePermisoAduana && !obj.RequiereRevisionCliente && obj.OkClienteEmbarque && obj.OkCliente && obj.OkEmbarque) { //salamanca y etileno
            retorno = true;
        }        
    }
    return retorno;
}

function RenderComboBoxAprobacionAduana(container, options) {
    var valores;
    $('<input  data-text-field="Descripcion" id=' + options.model.uid + ' data-value-field="Descripcion" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.listaEstatus,
            template: "<i class=\"fa fa-#=data.Descripcion.toLowerCase()#\"></i> #=data.Descripcion#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined && dataItem.OpcionValidacionID != 0) {
                        options.model.AprobadoAduanaDesc = dataItem.Descripcion;
                        options.model.AprobadoAduana = dataItem.OpcionValidacionID;
                        options.model.ModificadoPorUsuario = true;

                   if (SetValueEnviar(options.model))
                        options.model.Enviar = true;
                   else
                        options.model.Enviar = false;

                    $("#grid").data("kendoGrid").dataSource.sync();
                }
                else {
                    options.model.AprobadoAduanaDesc = "";
                    options.model.AprobadoAduana = 0;
                    options.model.ModificadoPorUsuario = true;
                }
            }
        });
}

function RenderDatePicker(container, options) {
    var dataItem;
    $('<input   data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDatePicker({
            max: new Date(),
            change: function (e) {
                var value = this.value();
                
                if (ValidarFecha(value))
                    options.model.FechaSolicitudPermiso = value;
                else
                    options.model.FechaSolicitudPermiso = "";

                if (SetValueEnviar(options.model)) {
                    options.model.Enviar = true;
                    $("#grid").data("kendoGrid").dataSource.sync();
                } else {
                    options.model.Enviar = false;
                    $("#grid").data("kendoGrid").dataSource.sync();
                }
            }
        }
    );
}

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null)
        return false;
    else
        return true;
}