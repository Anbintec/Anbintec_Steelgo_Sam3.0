function RenderComboBoxProveedor(container, options) {
    var dataItem;
    var textAnterior;
    $('<input  data-text-field="Nombre" data-value-field="ProveedorID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            delay: 10,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.ListaProveedor,
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",
            select: function (e) {
                dataItem = this.dataItem(e.item.index());
                textAnterior = e.sender._prev; 
            },
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                editado = true;
                if (dataItem != undefined) {
                    options.model.ProveedorID = dataItem.ProveedorID;
                    options.model.Proveedor = dataItem.Nombre;
                    options.model.ListaTurnoLaboral = [];
                    if (dataItem.Nombre != "") {
                        if (!options.model.RequiereEquipo) {
                            options.model.ListaTurnoLaboral = obtenerTurnoLaboralProveedor(options.model, dataItem.TipoPruebaProveedorID);
                            if (options.model.ListaTurnoLaboral.length == 2) {

                                var JuntasAsignadasBusqueda = 0;
                                if (options.model.RequiereEquipo) {
                                    JuntasAsignadasBusqueda = getNumeroJuntasAsignadasEquipo(options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID);
                                }
                                else {
                                    JuntasAsignadasBusqueda = getNumeroJuntasAsignadasProveedor(options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID)
                                }


                                var JuntasAsignadasFinal = parseInt(JuntasAsignadasBusqueda) + parseInt(options.model.CantidadJuntas);

                                if (!options.model.RequiereEquipo) {
                                    
                                    options.model.CapacidadTurnoProveedorID = options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID;
                                    options.model.CapacidadTurnoProveedorAnteriorID = options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID;

                                    setJuntasAsignatdasCapacidadTurnoProveedor(JuntasAsignadasFinal, options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID);
                                }
                                else {

                                    options.model.CapacidadTurnoEquipoID = options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID;
                                    options.model.CapacidadTurnoEquipoAnteriorID = options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID;

                                    setJuntasAsignatdasCapacidadTurnoEquipo(JuntasAsignadasFinal, options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID);

                                }

                                options.model.TurnoLaboral = options.model.ListaTurnoLaboral[1].Nombre;
                                options.model.TurnoLaboralID = options.model.ListaTurnoLaboral[1].TurnoLaboralID;
                                options.model.Capacidad = options.model.ListaTurnoLaboral[1].Capacidad;
                                options.model.JuntasAsignadas = parseInt(JuntasAsignadasFinal).toString();
                                $("#grid").data("kendoGrid").dataSource.sync();

                            }
                            else {

                                var JuntasAsignadasBusqueda = 0;
                                if (options.model.RequiereEquipo) {
                                    JuntasAsignadasBusqueda = getNumeroJuntasAsignadasEquipo(options.model.CapacidadTurnoEquipoID);
                                }
                                else {
                                    JuntasAsignadasBusqueda = getNumeroJuntasAsignadasProveedor(options.model.CapacidadTurnoProveedorID)
                                }


                                var JuntasAsignadasFinal = parseInt(JuntasAsignadasBusqueda) - parseInt(options.model.CantidadJuntas);

                                if (!options.model.RequiereEquipo) 
                                    setJuntasAsignatdasCapacidadTurnoProveedor(JuntasAsignadasFinal, options.model.CapacidadTurnoProveedorID);
                                else 
                                    setJuntasAsignatdasCapacidadTurnoEquipo(JuntasAsignadasFinal, options.model.CapacidadTurnoEquipoID);



                                options.model.TurnoLaboralID = 0;
                                options.model.TurnoLaboral = "";
                                options.model.Equipo = "";
                                options.model.Capacidad = "";
                                options.model.JuntasAsignadas = "";
                            }
                        }
                        else {
                            options.model.ListaEquipos = obtenerProveedorEquipo(options.model, dataItem.TipoPruebaProveedorID);
                        }
                        options.model.TipoPruebaProveedorID = dataItem.TipoPruebaProveedorID;

                        
                    }
                    else {
                        options.model.TurnoLaboralID = 0;
                        options.model.TurnoLaboral = "";
                        options.model.Equipo = "";
                        options.model.Capacidad = "";
                        options.model.JuntasAsignadas = "";

                        if (options.model.TurnoLaboral != "") {
                            var JuntasAsignadasFinal = parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas);

                            if (!options.model.RequiereEquipo) {
                                setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                                options.model.CapacidadTurnoProveedorID = 0;
                                options.model.CapacidadTurnoProveedorAnteriorID = 0;
                            }
                            else {
                                setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                                options.model.CapacidadTurnoEquipoID = 0;
                                options.model.CapacidadTurnoEquipoAnteriorID = 0;
                            }
                        }
                    }
                    

                    

                    if (options.model.Accion == 4)
                        options.model.Accion = 2;


                    $("#grid").data("kendoGrid").dataSource.sync();

                }
                else {

                    if (options.model.TurnoLaboral != "") {
                        var JuntasAsignadasFinal = parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas);

                        if (!options.model.RequiereEquipo) {
                            setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                            options.model.CapacidadTurnoProveedorID = 0;
                            options.model.CapacidadTurnoProveedorAnteriorID = 0;
                        }
                        else {
                            setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                            options.model.CapacidadTurnoEquipoID = 0;
                            options.model.CapacidadTurnoEquipoAnteriorID = 0;
                        }
                    }


                    options.model.ProveedorID = 0;
                    options.model.Proveedor = "";
                    options.model.TipoPruebaProveedorID = 0;

                    options.model.TurnoLaboralID = 0;
                    options.model.TurnoLaboral = "";
                    options.model.Equipo = "";
                    options.model.Capacidad = "";
                    options.model.JuntasAsignadas = "";
                    options.model.JuntasAsignadasOriginal = 0;
                    $("#grid").data("kendoGrid").dataSource.sync();
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
};

function obtenerTurnoLaboralProveedor(model, TipoPruebaProveedorID) {
    var lista = model.ListaTurnoLaboralTotal;
    var listaFinalTurnoLaboral = [];
    listaFinalTurnoLaboral[0] = { TurnoLaboralID: "", Nombre: "", TipoPruebaProveedorID: "", CapacidadTurnoProveedorID: "", CapacidadTurnoEquipoID: "" };
    var cont = 1;
    listaFinalTurnoLaboral[0].TurnoLaboralID = 0;
    listaFinalTurnoLaboral[0].TipoPruebaProveedorID = 0;
    listaFinalTurnoLaboral[0].Nombre = "";
    listaFinalTurnoLaboral[0].CapacidadTurnoProveedorID = 0;
    listaFinalTurnoLaboral[0].CapacidadTurnoEquipoID = 0;

    for (var i = 0; i < lista.length; i++) {
        if (lista[i].TipoPruebaProveedorID == TipoPruebaProveedorID && lista[i].CapacidadTurnoEquipoID == 0) {
            listaFinalTurnoLaboral[cont] = lista[i];
            cont++;
        }
    }

    return listaFinalTurnoLaboral;
}


function obtenerProveedorEquipo(model, TipoPruebaProveedorID) {
    var lista = model.ListaEquiposTotal;
    var listaFinalEquipo = [];
    listaFinalEquipo[0] = { EquipoID: "", Nombre: "", ProveedorEquipoID: "", ProveedorTipoPruebaProveedorIDID: "" };
    var cont = 1;
    listaFinalEquipo[0].EquipoID = 0;
    listaFinalEquipo[0].ProveedorEquipoID = 0;
    listaFinalEquipo[0].Nombre = "";
    listaFinalEquipo[0].TipoPruebaProveedorID = 0;

    for (var i = 0; i < lista.length; i++) {
        if (lista[i].TipoPruebaProveedorID == TipoPruebaProveedorID) {
            listaFinalEquipo[cont] = lista[i];
            cont++;
        }
    }

    return listaFinalEquipo;
}


function obtenerTurnoLaboralEquipo(model, ProveedorEquipoID) {
    var lista = model.ListaTurnoLaboralTotal;
    var listaFinalTurnoLaboral = [];
    listaFinalTurnoLaboral[0] = { TurnoLaboralID: "", Nombre: "", TipoPruebaProveedorID: "", CapacidadTurnoProveedorID: "", CapacidadTurnoEquipoID: "" };
    var cont = 1;
    listaFinalTurnoLaboral[0].TurnoLaboralID = 0;
    listaFinalTurnoLaboral[0].TipoPruebaProveedorID = 0;
    listaFinalTurnoLaboral[0].Nombre = "";
    listaFinalTurnoLaboral[0].CapacidadTurnoProveedorID = 0;
    listaFinalTurnoLaboral[0].CapacidadTurnoEquipoID = 0;

    for (var i = 0; i < lista.length; i++) {
        if (lista[i].ProveedorEquipoID == ProveedorEquipoID) {
            listaFinalTurnoLaboral[cont] = lista[i];
            cont++;
        }
    }

    return listaFinalTurnoLaboral;
}

function RenderComboBoxHerramientaPrueba(container, options) {
    //container  contiene las propiedades de la celda
    //options contiene el modelo del datasource ejemplo options.model.Junta
    var dataItem;

    $('<input  data-text-field="Nombre" data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.ListaEquipos,
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",

            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                editado = true;
                if (dataItem != undefined && dataItem.Nombre != "") {
                    if (options.model.TurnoLaboral != "") {
                        var JuntasAsignadasFinal = parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas);

                        if (!options.model.RequiereEquipo) {
                            setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                            options.model.CapacidadTurnoProveedorID = 0;
                            options.model.CapacidadTurnoProveedorAnteriorID = 0;
                        }
                        else {
                            setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                            options.model.CapacidadTurnoEquipoID = 0;
                            options.model.CapacidadTurnoEquipoAnteriorID = 0;
                        }
                        
                    }


                    options.model.Equipo = dataItem.Nombre;
                    options.model.TurnoLaboral = "";
                    options.model.TurnoLaboralID = 0;
                    options.model.JuntasAsignadas = "";
                    options.model.Capacidad = "";
                    options.model.ProveedorEquipoID = dataItem.ProveedorEquipoID;
                    options.model.ListaTurnoLaboral = obtenerTurnoLaboralEquipo(options.model, dataItem.ProveedorEquipoID);
                    

                    if (options.model.ListaTurnoLaboral.length == 2) {

                        var JuntasAsignadasBusqueda = 0;
                        if (options.model.RequiereEquipo) {
                            JuntasAsignadasBusqueda = getNumeroJuntasAsignadasEquipo(options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID);
                        }
                        else {
                            JuntasAsignadasBusqueda = getNumeroJuntasAsignadasProveedor(options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID)
                        }


                        var JuntasAsignadasFinal = parseInt(JuntasAsignadasBusqueda) + parseInt(options.model.CantidadJuntas);

                        if (!options.model.RequiereEquipo) {
                            if (options.model.TurnoLaboral != "") {
                                setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                            }
                            options.model.CapacidadTurnoProveedorID = options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID;
                            options.model.CapacidadTurnoProveedorAnteriorID = options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID;

                            setJuntasAsignatdasCapacidadTurnoProveedor(JuntasAsignadasFinal, options.model.ListaTurnoLaboral[1].CapacidadTurnoProveedorID);
                        }
                        else {
                            if (options.model.TurnoLaboral != "") {
                                setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                            }

                            options.model.CapacidadTurnoEquipoID = options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID;
                            options.model.CapacidadTurnoEquipoAnteriorID = options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID;

                            setJuntasAsignatdasCapacidadTurnoEquipo(JuntasAsignadasFinal, options.model.ListaTurnoLaboral[1].CapacidadTurnoEquipoID);

                        }

                        options.model.TurnoLaboral = options.model.ListaTurnoLaboral[1].Nombre;
                        options.model.TurnoLaboralID = options.model.ListaTurnoLaboral[1].TurnoLaboralID;
                        options.model.Capacidad = options.model.ListaTurnoLaboral[1].Capacidad;
                        options.model.JuntasAsignadas = parseInt(JuntasAsignadasFinal);
                    }

                    $("#grid").data("kendoGrid").dataSource.sync();

                }
                else {
                    if (options.model.TurnoLaboral != "") {

                        var JuntasAsignadasFinal = parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas);

                        if (!options.model.RequiereEquipo) {
                            setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                            options.model.CapacidadTurnoProveedorID = 0;
                            options.model.CapacidadTurnoProveedorAnteriorID = 0;
                        }
                        else {
                            setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                            options.model.CapacidadTurnoEquipoID = 0;
                            options.model.CapacidadTurnoEquipoAnteriorID = 0;
                        }
                    }

                    options.model.ProveedorEquipoID = 0
                    options.model.EquipoID = 0;
                    options.model.Equipo = "";
                    options.model.TurnoLaboralID = 0;
                    options.model.TurnoLaboral = "";
                    options.model.Capacidad = "";
                    options.model.JuntasAsignadas = "";
                    options.model.JuntasAsignadasOriginal = 0;
                    options.model.ListaElementosAsignadosTurno = [];
                    $("#grid").data("kendoGrid").dataSource.sync();
                }

            },
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
};



function RenderComboBoxTurnoLaboral(container, options) {
    //container  contiene las propiedades de la celda
    //options contiene el modelo del datasource ejemplo options.model.Junta
    var dataItem;

    $('<input  data-text-field="Nombre" data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            suggest: true,
            filter: "contains",
            autoBind: false,
            dataSource: options.model.ListaTurnoLaboral,
            template: "<i class=\"fa fa-#=data.Nombre.toLowerCase()#\"></i> #=data.Nombre#",

            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                editado = true;
                if (dataItem != undefined) {
                    if (dataItem.Nombre != "") {

                        var JuntasAsignadasBusqueda = 0;
                        if (options.model.RequiereEquipo) {
                            JuntasAsignadasBusqueda = getNumeroJuntasAsignadasEquipo(dataItem.CapacidadTurnoEquipoID);
                        }
                        else {
                            JuntasAsignadasBusqueda = getNumeroJuntasAsignadasProveedor(dataItem.CapacidadTurnoProveedorID)
                        }


                        var JuntasAsignadasFinal = parseInt(JuntasAsignadasBusqueda) + parseInt(options.model.CantidadJuntas);

                        if (!options.model.RequiereEquipo) {
                            if (options.model.TurnoLaboral != "" && options.model.JuntasAsignadas != "") {
                                setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                            }
                            options.model.CapacidadTurnoProveedorID = dataItem.CapacidadTurnoProveedorID;
                            options.model.CapacidadTurnoProveedorAnteriorID = dataItem.CapacidadTurnoProveedorID;

                            setJuntasAsignatdasCapacidadTurnoProveedor(JuntasAsignadasFinal, dataItem.CapacidadTurnoProveedorID);
                        }
                        else {
                            if (options.model.TurnoLaboral != "") {
                                setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                            }

                            options.model.CapacidadTurnoEquipoID = dataItem.CapacidadTurnoEquipoID;
                            options.model.CapacidadTurnoEquipoAnteriorID = dataItem.CapacidadTurnoEquipoID;

                            setJuntasAsignatdasCapacidadTurnoEquipo(JuntasAsignadasFinal, dataItem.CapacidadTurnoEquipoID);

                        }

                        options.model.TurnoLaboral = dataItem.Nombre;
                        options.model.TurnoLaboralID = dataItem.TurnoLaboralID;
                        options.model.Capacidad = dataItem.Capacidad;
                        options.model.JuntasAsignadas = parseInt(JuntasAsignadasFinal);

                        loadingStop();
                    }
                    else {
                        loadingStart();
                        var JuntasAsignadasFinal = parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas);

                        if (!options.model.RequiereEquipo) {
                            setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                            options.model.CapacidadTurnoProveedorID = 0;
                            options.model.CapacidadTurnoProveedorAnteriorID = 0;
                        }
                        else {
                            setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                            options.model.CapacidadTurnoEquipoID = 0;
                            options.model.CapacidadTurnoEquipoAnteriorID = 0;
                        }


                        options.model.TurnoLaboralID = 0;
                        options.model.TurnoLaboral = "";
                        options.model.Capacidad = "";
                        options.model.JuntasAsignadas = "";
                        options.model.ListaElementosAsignadosTurno = [];
                        loadingStop();
                    }
                }
                else {
                    loadingStart();
                    var JuntasAsignadasFinal = parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas);

                    if (!options.model.RequiereEquipo) {
                        setJuntasAsignatdasCapacidadTurnoProveedor(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoProveedorAnteriorID);
                        options.model.CapacidadTurnoProveedorID = 0;
                        options.model.CapacidadTurnoProveedorAnteriorID = 0;
                    }
                    else {
                        setJuntasAsignatdasCapacidadTurnoEquipo(parseInt(options.model.JuntasAsignadas) - parseInt(options.model.CantidadJuntas), options.model.CapacidadTurnoEquipoAnteriorID);
                        options.model.CapacidadTurnoEquipoID = 0;
                        options.model.CapacidadTurnoEquipoAnteriorID = 0;
                    }

                    options.model.TurnoLaboralID = 0;
                    options.model.TurnoLaboral = "";
                    options.model.Capacidad = "";
                    options.model.JuntasAsignadas = "";
                    loadingStop();
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
};



function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}