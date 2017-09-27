var JuntasSoldador1 = [{ JuntaSpoolID: 0, Junta: "" }, { JuntaSpoolID: 1, Junta: "X010-008 - 5" }, { JuntaSpoolID: 2, Junta: "X010-010 - 17" },
                        { JuntaSpoolID: 3, Junta: "X021-005 - 11" }, { JuntaSpoolID: 3, Junta: "X090-003 - 20" }, { JuntaSpoolID: 3, Junta: "X010-002 - 10A" }];

var JuntasSoldador2 = [{ JuntaSpoolID: 0, Junta: "" }, { JuntaSpoolID: 1, Junta: "X003-006 - 5" }, { JuntaSpoolID: 2, Junta: "X007-007 - 7" }, ];

function RenderComboBoxSoldador(container, options) {
    var dataItem;
    var valores;

    $('<input  data-text-field="Soldador" id=' + options.model.uid + ' data-value-field="Soldador" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            suggest: true,
            delay: 10,
            filter: "contains",
            dataSource: [{ SoldadorID: 0, Soldador: "" }, { SoldadorID: 1, Soldador: "A-73 Arturo Cruz" }, { SoldadorID: 2, Soldador: "O-34 Ricardo Farias" }],
            template: "<i class=\"fa fa-#=data.Soldador.toLowerCase()#\"></i> #=data.Soldador#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {
                    options.model.SoldadorID = dataItem.SoldadorID;
                    options.model.Soldador = dataItem.Soldador;
                    options.model.JtaSeg1 = "";
                    options.model.JtaSeg2 = "";

                    var ds = $("#gridPopUp").data("kendoGrid").dataSource;


                    var ds = $("#gridPopUp").data("kendoGrid").dataSource;
                    options.model.EsRepetido = placaSoldadorRepetido(ds, indiceRenglon(ds, options.model.uid), dataItem.Soldador);


                } else {
                    options.model.SoldadorID = 0;
                    options.model.Soldador = "";

                }
                editado = true;
                $("#gridPopUp").data("kendoGrid").refresh();
            }
        });
}

function RenderComboBoxJunta(container, options) {
    var dataItem;
    var valores;
    
        options.model.listaJuntas = JuntasSoldador1;
    
    $('<input  data-text-field="Junta" id=' + options.model.uid + ' data-value-field="Junta" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.listaJuntas,
            template: "<i class=\"fa fa-#=data.Junta.toLowerCase()#\"></i> #=data.Junta#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {

                    if (options.model.JtaSeg2 != dataItem.Junta) {
                        options.model.JuntaSpoolID = dataItem.JuntaSpoolID;
                        options.model.JtaSeg1 = dataItem.Junta;
                    }
                    else {
                        options.model.JuntaSpoolID = 0;
                        options.model.JtaSeg1 = "";
                    }


                    $("#grid").data("kendoGrid").refresh();
                } else {
                    options.model.JuntaSpoolID = 0;
                    options.model.JtaSeg1 = "";

                }
                editado = true;
            }
        });
}


function RenderComboBoxJunta2(container, options) {
    var dataItem;
    var valores;
    
        options.model.listaJuntas = JuntasSoldador2;
    
    $('<input  data-text-field="Junta" id=' + options.model.uid + ' data-value-field="Junta" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.listaJuntas,
            template: "<i class=\"fa fa-#=data.Junta.toLowerCase()#\"></i> #=data.Junta#",
            change: function (e) {
                e.preventDefault();
                var dataItem = this.dataItem(e.sender.selectedIndex);

                if (dataItem != undefined) {


                    if (options.model.JtaSeg1 != dataItem.Junta) {
                        options.model.JuntaSpool2ID = dataItem.JuntaSpoolID;
                        options.model.JtaSeg2 = dataItem.Junta;
                    }
                    else {
                        options.model.JuntaSpool2ID = 0;
                        options.model.JtaSeg2 = "";
                    }


                    $("#grid").data("kendoGrid").refresh();
                } else {
                    options.model.JuntaSpool2ID = 0;
                    options.model.JtaSeg2 = "";

                }
                editado = true;
            }
        });
}




function RenderInicioMM(container, options) {
    var dataItem;
    $('<input data-text-field="InicioMM" id=' + options.model.uid + ' data-value-field="InicioMM" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "{0: }",
        min: 0
    });

}

function RenderFinMM(container, options) {
    var dataItem;
    $('<input data-text-field="FinMM" id=' + options.model.uid + ' data-value-field="FinMM" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoNumericTextBox({
        format: "{0: }",
        min: 0
    });

}


function placaSoldadorRepetido(ds, index, soldador) {

    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i].Soldador == soldador && i != index) {
            if (ds._data[i].JtaSeg1 != "" && ds._data[i].JtaSeg2 != "")
                return true;
        }
    }
    return false;
}



function indiceRenglon(ds, uid) {
    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i].uid == uid) {
            return i;
        }
    }
    return 0;
}