var TractoEmbarque = 0;
var ChoferEmbarque = 0;

function AjaxCargarProyecto() {
    loadingStart();
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#Proyecto").data("kendoComboBox").dataSource.data([]);
            var proyectoId = 0;

            if (data.length > 0) {
                $("#Proyecto").data("kendoComboBox").dataSource.data(data);
                if (data.length < 3) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ProyectoID != 0) {
                            proyectoId = data[i].ProyectoID;
                        }
                    }
                }

                $("#Proyecto").data("kendoComboBox").value(proyectoId);
                $("#Proyecto").data("kendoComboBox").trigger("change");
            }
        }
        loadingStop();
    });
}


function AjaxObtenerPlanas(ProyectoID) {
    loadingStart();
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, PlanaCerrada: 1 }).done(function (data) {
        $("#Plana").data("kendoComboBox").dataSource.data([]);
        var PlanaID = 0;

            if (data.length > 0) {
                $("#Plana").data("kendoComboBox").dataSource.data(data);

                $("#Plana").data("kendoComboBox").value(PlanaID);
                $("#Plana").data("kendoComboBox").trigger("change");
            }
        loadingStop();
    });
}



function AjaxEmbarqueCargaProveedores(ProyectoID, nuevoProveedor) {
    loadingStart();
    $Proveedores.Proveedores.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, TipoProveedor: 2 }).done(function (data) {
        var ProveedorId = 0;
        $("#Proveedor").data("kendoComboBox").dataSource.data([]);
        $("#ProveedorEnvio").data("kendoComboBox").dataSource.data([]);
        if (data.length > 0) {
            $("#Proveedor").data("kendoComboBox").dataSource.data(data);
            $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data);
            if (data.length == 2 && nuevoProveedor == null) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProveedorID != 0) {
                        ProveedorId = data[i].ProveedorID;
                    }
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Nombre == nuevoProveedor) {
                        ProveedorId = data[i].ProveedorID;
                    }
                }
            }
            data.splice(1, 0, {
                ProveedorID: -1, Nombre: _dictionary.EmarquePreparacionAgregarProveedor[$("#language").data("kendoDropDownList").value()]
            });
            $("#Proveedor").data("kendoComboBox").dataSource.data(data);            
            $("#Proveedor").data("kendoComboBox").value(ProveedorId);            
            $("#Proveedor").data("kendoComboBox").trigger("change");
            $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data);
            $("#ProveedorEnvio").data("kendoComboBox").value(ProveedorId);
            $("#ProveedorEnvio").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}

function AjaxEmbarqueCargaTractos(ProveedorID, nuevoTracto) {
    loadingStart();
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), ProveedorID: ProveedorID, Tractos: 1 }).done(function (data) {
        var TractoID = 0;
        $("#Tracto").data("kendoComboBox").dataSource.data([]);        
        if (data.length > 0) {
            if (data.length == 2 && nuevoTracto == null) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].TractoID != 0) {
                        TractoID = data[i].TractoID;

                    }
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Nombre == nuevoTracto || data[i].TractoID == TractoEmbarque) {
                        TractoID = data[i].TractoID;
                    }
                }
            }

            data.splice(1, 0, { TractoID: -1, Nombre: _dictionary.EmarquePreparacionAgregarTracto[$("#language").data("kendoDropDownList").value()] });
            $("#Tracto").data("kendoComboBox").dataSource.data(data);            
            $("#Tracto").data("kendoComboBox").value(TractoID);            
            $("#Tracto").data("kendoComboBox").trigger("change");                                                
        }
        loadingStop();
    });
}


function AjaxEmbarqueCargaTractosEnvio(ProveedorID, nuevoTracto) {
    loadingStart();
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), ProveedorID: ProveedorID, Tractos: 1 }).done(function (data) {
        var TractoID = 0;        
        $("#TractoEnvio").data("kendoComboBox").dataSource.data([]); //Add
        if (data.length > 0) {
            if (data.length == 2 && nuevoTracto == null) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].TractoID != 0) {
                        TractoID = data[i].TractoID;
                    }
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Nombre == nuevoTracto || data[i].TractoID == TractoEmbarque) {
                        TractoID = data[i].TractoID;
                    }
                }
            }
            data.splice(1, 0, { TractoID: -1, Nombre: _dictionary.EmarquePreparacionAgregarTracto[$("#language").data("kendoDropDownList").value()] });            
            $("#TractoEnvio").data("kendoComboBox").dataSource.data(data); //add            
            $("#TractoEnvio").data("kendoComboBox").value(TractoID); //add            
            $("#TractoEnvio").data("kendoComboBox").trigger("change"); //add
        }
        loadingStop();
    });
}



function AjaxEmbarqueCargaChofer(ProveedorID, nuevoChofer) {
    loadingStart();

    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), ProveedorID: ProveedorID, Chofer: 2 }).done(function (data) {
        $("#Chofer").data("kendoComboBox").dataSource.data([]);
        //$("#ChoferEnvio").data("kendoComboBox").dataSource.data([]); //add
        if (data.length > 0) {
            var ChoferID = 0;

            if (data.length == 2 && nuevoChofer == null) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ChoferID != 0) {
                        ChoferID = data[i].ChoferID;

                    }
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Nombre == nuevoChofer || data[i].ChoferID == ChoferEmbarque) {
                        ChoferID = data[i].ChoferID;
                    }
                }
            }

            data.splice(1, 0, {
                ChoferID: -1, Nombre: _dictionary.EmarquePreparacionAgregarChofer[$("#language").data("kendoDropDownList").value()]
            });
            $("#Chofer").data("kendoComboBox").dataSource.data(data);
            //$("#ChoferEnvio").data("kendoComboBox").dataSource.data(data); //add
            $("#Chofer").data("kendoComboBox").value(ChoferID);
            //$("#ChoferEnvio").data("kendoComboBox").value(ChoferID); //add
            $("#Chofer").data("kendoComboBox").trigger("change");                        
            //$("#ChoferEnvio").data("kendoComboBox").trigger("change"); //add
        }
        loadingStop();
    });
}


function AjaxEmbarqueCargaChoferEnvio(ProveedorID, nuevoChofer) {
    loadingStart();

    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), ProveedorID: ProveedorID, Chofer: 2 }).done(function (data) {        
        $("#ChoferEnvio").data("kendoComboBox").dataSource.data([]); //add
        if (data.length > 0) {
            var ChoferID = 0;
            if (data.length == 2 && nuevoChofer == null) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ChoferID != 0) {
                        ChoferID = data[i].ChoferID;

                    }
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Nombre == nuevoChofer || data[i].ChoferID == ChoferEmbarque) {
                        ChoferID = data[i].ChoferID;
                    }
                }
            }
            data.splice(1, 0, {
                ChoferID: -1, Nombre: _dictionary.EmarquePreparacionAgregarChofer[$("#language").data("kendoDropDownList").value()]
            });            
            $("#ChoferEnvio").data("kendoComboBox").dataSource.data(data); //add            
            $("#ChoferEnvio").data("kendoComboBox").value(ChoferID); //add            
            $("#ChoferEnvio").data("kendoComboBox").trigger("change"); //add
        }
        loadingStop();
    });
}

function AjaxObtenerEmbarque(ProveedorID, nombreEmbarque) {
    loadingStart();
    var proyectoID = $("#Proyecto").data("kendoComboBox").value();
    $PreparacionEmbarque.PreparacionEmbarque.read({ token: Cookies.get("token"), ProveedorID: ProveedorID, Lenguaje: $("#language").val(), ProyectoID: proyectoID }).done(function (data) {
        $("#Embarque").data("kendoComboBox").dataSource.data([]);
        var EmbarqueID = 0;

        if (data.length > 0) {
            if (data.length < 3 && nombreEmbarque == null) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].EmbarqueID != 0) {
                        EmbarqueID = data[i].EmbarqueID;
                    }
                }
            } else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Nombre == nombreEmbarque) {
                        EmbarqueID = data[i].EmbarqueID;
                    }
                }
            }

            $("#Embarque").data("kendoComboBox").dataSource.data(data);
            $("#Embarque").data("kendoComboBox").value(EmbarqueID);
            $("#Embarque").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}



function GuardarNuevoProveedor() {        
    $Proveedores.Proveedores.read({ token: Cookies.get("token"), NombreProveedor: $("#inputNombreNuevoProveedor").val(), ProyectoID: $("#Proyecto").data("kendoComboBox").value(), Descripcion: "", Direccion: "", Telefono: "", TipoProveedor: 2 }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                AjaxEmbarqueCargaProveedores($("#Proyecto").data("kendoComboBox").value(), $("#inputNombreNuevoProveedor").val());
                windowNewProvider.close();
                displayNotify("MensajeGuardadoExistoso", "", "0");
            }
            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                displayNotify("EmbarquePreparacionErrorExisteProveedor", "", '2');
            }
        }
    });
}



function GuardarNuevoTracto() {
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), NombreTracto: $("#inputNombreNuevoTracto").val(), ProveedorID: $("#Proveedor").data("kendoComboBox").value(), TipoProveedor: 2 }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                AjaxEmbarqueCargaTractos($("#Proveedor").data("kendoComboBox").value(), $("#inputNombreNuevoTracto").val());
                windowNewTracto.close();
                displayNotify("MensajeGuardadoExistoso", "", "0");
            }
            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {

                displayNotify("EmbarquePreparacionErrorExisteTracto", "", '2');
            }

        }
    });
}

function GuardarNuevoChofer() {
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), NombreChofer: $("#inputNombreNuevoChofer").val(), ProveedorID: $("#Proveedor").data("kendoComboBox").value() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                AjaxEmbarqueCargaChofer($("#Proveedor").data("kendoComboBox").value(), $("#inputNombreNuevoChofer").val());
                windowNewChofer.close();
                displayNotify("MensajeGuardadoExistoso", "", "0");
            }
            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {

                displayNotify("EmbarquePreparacionErrorExisteChofer", "", '2');
            }

        }
    });
}


function AjaxAgregaRenglon(cargaPlanaID) {

    loadingStart();

    $PreparacionEmbarque.PreparacionEmbarque.read({ token: Cookies.get("token"), CargaPlanaID: cargaPlanaID }).done(function (data) {
        var cadenaPlana = "";
        if (Error(data)) {
            if (data.length > 0) {
                var ds = $("#grid").data("kendoGrid").dataSource;

                var array = data;
                for (var i = 0; i < array.length; i++) {
                    if (!ExistePlana(array[i])) {
                        if (data[i].EmbarqueDetalleID == 0) {
                            ds.add(array[i]);
                        } else {
                            displayNotify("", _dictionary.EmbarquePreparacionErrorPlanaCargada[$("#language").data("kendoDropDownList").value()]
                                + data[i].NombreEmbarque, '1');
                        }
                    } else {
                        displayNotify("EmbarquePreparacionErrorExistePlana", "", '1');
                    }
                }
            }
        }
        loadingStop();
    });

}


function ExistePlana(row) {
    var jsonGrid = $("#grid").data("kendoGrid").dataSource._data;

    for (var i = 0; i < jsonGrid.length; i++) {
        if (jsonGrid[i].PlanaID == row.PlanaID) {
            return true
        }
    }
    return false;

}

function AjaxObtieneDetalle(embarqueID) {
    loadingStart();
    $PreparacionEmbarque.PreparacionEmbarque.read({ token: Cookies.get("token"), EmbarqueID: embarqueID, }).done(function (data) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        if(data!=null){
            var ds = $("#grid").data("kendoGrid").dataSource;

            for (var i = 0; i < data.length; i++) {
                ds.add(data[i]);
            }
        }
        loadingStop();
    });
}

function AbrirPopUpGuardar(Embarque, tipoGuardado) {
    var fechaPredeterminada;
    $("#InputTipoGuardado").val(tipoGuardado);
    if (Embarque.EmbarqueID == 0) {
        $("#inputNombreEmbarque").val("");
        $("#inputNombreEmbarque").attr("disabled", false);

        $("#inputNombreEmbarqueCliente").val("");
        $("#inputNombreEmbarqueCliente").attr("disabled", false);

        var idFechaPaquete = 3068;
        $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: idFechaPaquete }).done(function (data) {
            fechaPredeterminada = kendo.toString(data, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
            $("#inputFechaEmbarque").val(fechaPredeterminada);
        });
    } else {
        $("#inputNombreEmbarque").val(Embarque.Nombre);
        $("#inputNombreEmbarque").attr("disabled", true);

        $("#inputNombreEmbarqueCliente").val(Embarque.NombreCliente);
        $("#inputNombreEmbarqueCliente").attr("disabled", true);

        fechaPredeterminada = kendo.toString(Embarque.FechaCreacion, _dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()]);
        $("#inputFechaEmbarque").val(fechaPredeterminada);

    }
    divNuevoEmbarque.open().center();
}

function AjaxGuardarCaptura(ds, tipoGuardado, proveedorID) {
    loadingStart();
    Captura = [];
    Captura[0] = {
        Detalles : ""
    }
    var listaDetalle = [];
    var embarqueID = $("#Embarque").data("kendoComboBox").value();
    var nombreEmbarque = $("#inputNombreEmbarque").val();
    var nombreEmbarqueCliente = $("#inputNombreEmbarqueCliente").val();
    var tractoID = $("#Tracto").data("kendoComboBox").value();
    var choferID = $("#Chofer").data("kendoComboBox").value();
    var tractoEnvioID = $("#TractoEnvio").data("kendoComboBox").value();
    var choferEnvioID = $("#ChoferEnvio").data("kendoComboBox").value();
    var proveedorEnvioID = $("#ProveedorEnvio").data("kendoComboBox").value();
    var fechaCreacion = $("#inputFechaEmbarque").val();


    var cont = 0;

    if (ds.length > 0) {
        for (var i = 0; i < ds.length; i++) {
            listaDetalle [i] = {Accion: "", EmbarqueDetalleID: "", EmbarqueID: "", CargaPlanaID: ""};
            listaDetalle[i].Accion = ds[i].Accion;
            listaDetalle[i].EmbarqueDetalleID = ds[i].EmbarqueDetalleID;
            listaDetalle[i].EmbarqueID = ds[i].EmbarqueID;
            listaDetalle[i].CargaPlanaID = ds[i].CargaPlanaID;

            if (ds[i].Accion == 2 || ds[i].Accion == 1) {
                cont++;
            }
        }

    }

    Captura[0].Detalles = listaDetalle;
    if (cont > 0) {
        $PreparacionEmbarque.PreparacionEmbarque.create(Captura[0], {
            token: Cookies.get("token"), lenguaje: $("#language").val(), EmbarqueID: embarqueID,
            NombreEmbarque: nombreEmbarque, NombreEmbarqueCliente: nombreEmbarqueCliente, TractoID: tractoID, ChoferID: choferID, TractoEnvioID: tractoEnvioID, ChoferEnvioID: choferEnvioID, ProveedorEnvioID: proveedorEnvioID, FechaCreacion: fechaCreacion
        }).done(function (data) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                if (tipoGuardado != "1") {
                    Limpiar();
                } else {
                    var paqueteID = parseInt(data.ReturnMessage[1]);
                    opcionHabilitarView(true, "FieldSetView");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    AjaxObtenerEmbarque(proveedorID, nombreEmbarque);
                    AjaxObtenerPlanas($("#Proyecto").data("kendoComboBox").value());
                }
                displayNotify("MensajeGuardadoExistoso", "", '0');
            } else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "existe") {

                displayNotify("EmbarquePreparacionErrorExisteEmbarque", "", '2');
            }
            else {
                displayNotify("MensajeGuardadoErroneo", "", '2');
            }
        });
    } else {
        loadingStop();
        displayNotify("EmarquePreparacionMensajeErrorEmbarqueVacio", "", '2');
    }
        
    
}

function AjaxEliminarEmbarque(embarqueID) {
    $PreparacionEmbarque.PreparacionEmbarque.read({ token: Cookies.get("token"), EmbarqueID: embarqueID, Lenguaje: $("#language").val() }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            Limpiar();
            displayNotify("EmbarquePreparacionEmbarqueEliminadoCorrectamente", "", '0');
        }
        else {
            displayNotify("EmbarquePreparacionEmbarqueErrorEliminado", "", '2');
        }
    });
}