function RenderComboBoxPQRRaiz(container, options) {
    loadingStart();
    var dataItem;
    var auxNombrePQR = options.model.NombrePQRRaiz;
    var auxPQRID = options.model.PQRRaizId;
    $('<input data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.listadoRaizPQR,
            template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.Nombre != "") {
                    if (dataItem.CodigoRaiz != "N/A") {

                        options.model.NombrePQRRaiz = dataItem.Nombre;
                        options.model.PQRRaizId = dataItem.PQRID;
                        var cadenaError = "";
                        if (Boolean(options.model.PWHTRellenoId) != dataItem.PWHT) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorPWHT[$("#language").data("kendoDropDownList").value()];
                        }
                        if (!ContieneGruposMaterialBase(dataItem.GrupoPMaterialBase1, dataItem.GrupoPMaterialBase2, options.model.GrupoMaterialBase1RellenoUID, options.model.GrupoMaterialBase1RellenoDID)) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorGrupoP[$("#language").data("kendoDropDownList").value()];
                        }
                        if (options.model.PREHEATRellenoId != dataItem.PREHEAT) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorPREHEAT[$("#language").data("kendoDropDownList").value()];
                        }
                        if (options.model.CVNRelleno != dataItem.CVN) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorCVN[$("#language").data("kendoDropDownList").value()];
                        }
                        if (cadenaError != "") {
                            displayNotify("", cadenaError, "1");
                        }


                        options.model.PWHTRaiz = dataItem.PWHT;
                        options.model.PWHTRaizId = dataItem.PWHT == true ? 1 : 0;
                        options.model.GrupoMaterialBase1RaizUID = dataItem.GrupoPMaterialBase1;
                        options.model.GrupoMaterialBase1RaizU = dataItem.GrupoPMaterialBase1Nombre;
                        options.model.GrupoMaterialBase1RaizDID = dataItem.GrupoPMaterialBase2;
                        options.model.GrupoMaterialBase1RaizD = dataItem.GrupoPMaterialBase2Nombre;
                        options.model.GrupoPRaiz = dataItem.GrupoPMaterialBase1Nombre + " " + dataItem.GrupoPMaterialBase2Nombre;
                        options.model.PREHEATRaiz = dataItem.PREHEAT == 1 ? true : false;
                        options.model.PREHEATRaizId = dataItem.PREHEAT;
                        options.model.CVNRaiz = dataItem.CVN;
                        
                        
                        var Espesores = ObtenerEspesorCorrecto((parseFloat(dataItem.EspesorRaiz) + parseFloat(options.model.RellenoEspesorRelleno)), dataItem.PWHT, dataItem.CVN, dataItem.CodigoRaiz.trim(), options.model.QuitarCVN);

                        options.model.EspesorMaximo = Espesores[0].EspesorMaximo;
                        options.model.EspesorMinimo = Espesores[0].EspesorMinimo;

                        var EspesoresRaiz = ObtenerEspesorCorrecto(parseFloat(dataItem.EspesorRaiz) , dataItem.PWHT, dataItem.CVN, dataItem.CodigoRaiz.trim(), options.model.QuitarCVN);

                        options.model.EspesorMaximoRaiz = EspesoresRaiz[0].EspesorMaximo;
                        

                        options.model.RaizEspesorRaiz = parseFloat(dataItem.EspesorRaiz);
                        options.model.RaizEspesorRelleno = parseFloat(dataItem.EspesorRelleno);
                        
                        $("#grid").data("kendoGrid").dataSource.sync();
                    }
                    else {
                        displayNotify("WPSMensajeErrorPQRNoAplicaRaiz", "", "1");
                        options.model.NombrePQRRaiz = auxNombrePQR;
                        options.model.PQRRaizId = auxPQRID;
                        $("#grid").data("kendoGrid").dataSource.sync();
                    }
                }
                else {
                    options.model.NombrePQRRaiz = "";
                    options.model.PQRRaizId = 0;
                    options.model.PWHTRaiz = false;
                    options.model.PWHTRaizId = 0;
                    options.model.GrupoMaterialBase1RaizUID = 0;
                    options.model.GrupoMaterialBase1RaizU = "";
                    options.model.GrupoMaterialBase1RaizDID = 0;
                    options.model.GrupoMaterialBase1RaizD = "";
                    options.model.PREHEATRaiz = false;
                    options.model.PREHEATRaizId = 0;
                    options.model.EspesorMaximo = 0;
                    options.model.EspesorMaximoRaiz = 0;
                    options.model.EspesorMaximoRelleno = 0;
                    options.model.EspesorMinimo = 0;
                    options.model.RaizEspesorRaiz = 0;
                    options.model.RaizEspesorRelleno = 0;
                    try {
                        //$("#grid").data("kendoGrid").dataSource.sync();
                    }
                    catch (e) {
                    }
                    
                }
                options.model.EditadoUsuario = true;
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






function RenderComboBoxPQRRelleno(container, options) {
    loadingStart();
    var dataItem;
    var auxNombrePQR = options.model.NombrePQRRelleno;
    var auxPQRID = options.model.PQRRellenoId;
    $('<input data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.listadoRellenoPQR,
            template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
            change: function (e) {
                e.preventDefault();
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.Nombre != "") {
                    if (dataItem.CodigoRelleno != "N/A") {
                        options.model.NombrePQRRelleno = dataItem.Nombre;
                        options.model.PQRRellenoId = dataItem.PQRID;
                        var cadenaError = "";
                        if (Boolean(options.model.PWHTRaizId) != dataItem.PWHT) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorPWHT[$("#language").data("kendoDropDownList").value()];
                        }
                        if (!ContieneGruposMaterialBase(dataItem.GrupoPMaterialBase1, dataItem.GrupoPMaterialBase2, options.model.GrupoMaterialBase1RaizUID, options.model.GrupoMaterialBase1RaizDID)) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorGrupoP[$("#language").data("kendoDropDownList").value()];
                        }
                        if (options.model.PREHEATRaizId != dataItem.PREHEAT) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorPREHEAT[$("#language").data("kendoDropDownList").value()];
                        }
                        if (options.model.CVNRaiz != dataItem.CVN) {
                            cadenaError += "\n " + _dictionary.WPSMensajeErrorCVN[$("#language").data("kendoDropDownList").value()];
                        }
                        if (cadenaError != "") {
                            displayNotify("", cadenaError, "1");
                        }
                        options.model.PWHTRelleno = dataItem.PWHT;
                        options.model.PWHTRellenoId = dataItem.PWHT == true ? 1 : 0;
                        options.model.GrupoMaterialBase1RellenoUID = dataItem.GrupoPMaterialBase1;
                        options.model.GrupoMaterialBase1RellenoU = dataItem.GrupoPMaterialBase1Nombre;
                        options.model.GrupoMaterialBase1RellenoDID = dataItem.GrupoPMaterialBase2;
                        options.model.GrupoMaterialBase1RellenoD = dataItem.GrupoPMaterialBase2Nombre;
                        options.model.GrupoPRelleno = dataItem.GrupoPMaterialBase1Nombre + " " + dataItem.GrupoPMaterialBase2Nombre;
                        options.model.PREHEATRelleno = dataItem.PREHEAT == 1 ? true : false;
                        options.model.PREHEATRellenoId = dataItem.PREHEAT;
                        options.model.CVNRelleno = dataItem.CVN;

                        var Espesores = ObtenerEspesorCorrecto((parseFloat(options.model.RaizEspesorRaiz) + parseFloat(dataItem.EspesorRelleno)), dataItem.PWHT, dataItem.CVN, dataItem.CodigoRaiz.trim(), options.model.QuitarCVN);
                        options.model.EspesorMaximo = Espesores[0].EspesorMaximo;
                        options.model.EspesorMinimo = Espesores[0].EspesorMinimo;

                        var EspesoresRelleno = ObtenerEspesorCorrecto(parseFloat(dataItem.EspesorRelleno), dataItem.PWHT, dataItem.CVN, dataItem.CodigoRaiz.trim(), options.model.QuitarCVN);

                        options.model.EspesorMaximoRelleno = EspesoresRelleno[0].EspesorMaximo;



                        options.model.RellenoEspesorRaiz = parseFloat(dataItem.EspesorRaiz);
                        options.model.RellenoEspesorRelleno = parseFloat(dataItem.EspesorRelleno);
                        $("#grid").data("kendoGrid").dataSource.sync();
                    }
                    else {
                        displayNotify("WPSMensajeErrorPQRNoAplicaRelleno", "", "1");
                        options.model.NombrePQRRelleno = auxNombrePQR;
                        options.model.PQRRellenoId = auxPQRID;
                        $("#grid").data("kendoGrid").dataSource.sync();
                    }

                }
                else {
                    options.model.NombrePQRRelleno = "";
                    options.model.PQRRellenoId = 0;
                    options.model.PWHTRelleno = false;
                    options.model.PWHTRellenoId = 0;
                    options.model.GrupoMaterialBase1RellenoUID = 0;
                    options.model.GrupoMaterialBase1RellenoU = "";
                    options.model.GrupoMaterialBase1RellenoDID = 0;
                    options.model.GrupoMaterialBase1RellenoD = "";
                    options.model.PREHEATRelleno = false;
                    options.model.PREHEATRellenoId = 0;
                    options.model.EspesorMaximo = 0;
                    options.model.EspesorMaximoRaiz = 0;
                    options.model.EspesorMaximoRelleno = 0;
                    options.model.EspesorMinimo = 0;
                    options.model.RellenoEspesorRaiz = 0;
                    options.model.RellenoEspesorRelleno = 0;
                    try {
                        //$("#grid").data("kendoGrid").dataSource.sync();
                    }
                    catch (e) {
                    }

                }
                options.model.EditadoUsuario = true;
            }

        }
        );

    loadingStop();

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
function RendercomboBoxProyecto(container, options) {
    var dataItem;
    var valores;
    $('<input  data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaProyectos,
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            select: function (e) {
                e.preventDefault();
                dataItem = this.dataItem(e.item.index());
                if (dataItem != undefined) {
                    options.model.Proyecto = dataItem.Nombre;
                    options.model.ProyectoID = dataItem.ProyectoID;
                }
                else {

                }
            },
            change: function (e) {
                e.preventDefault();
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined) {
                    options.model.Proyecto = dataItem.Nombre;
                    options.model.ProyectoID = dataItem.ProyectoID;
                }
                else {

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


function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false;
}


function ContieneGruposMaterialBase(Base1Uno, Base2Uno, Base1Dos, Base2Dos) {


    if (Base1Uno == Base1Dos) {
        if (Base1Uno == Base2Dos) {
            return true;
        }
        else {
            if (Base2Uno == Base1Dos || Base2Uno == Base2Dos) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else if (Base1Uno == Base2Dos) {
        if (Base2Uno == Base1Dos || Base2Uno == Base2Dos) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        if (Base2Uno == Base1Dos && Base2Uno == Base2Dos) {
            return true;
        }
        else {
            return false;
        }
    }
    return false;
}