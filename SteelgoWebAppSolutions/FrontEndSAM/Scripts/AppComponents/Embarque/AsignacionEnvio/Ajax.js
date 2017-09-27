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


function AjaxGetEmbarques(ProyectoID) {
    loadingStart();
    var EmbarqueID = 0;
    var TractoE = 0, ChoferE = 0; ProveedorE = 0;
    $AsignacionEnvio.AsignacionEnvio.read({token: Cookies.get("token"), ProyectoID: ProyectoID}).done(function (data) {
        $("#Embarque").data("kendoComboBox").dataSource.data([]);        
        if (data.length > 0) {
            if (data.length == 1) {                
                $("#Embarque").data("kendoComboBox").dataSource.data(data);
                $("#Embarque").data("kendoComboBox").value(data[0].EmbarqueID);
                $("#Embarque").data("kendoComboBox").select(1);
                $("#Embarque").data("kendoComboBox").trigger("change");
            } else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].EmbarqueID != 0) {
                        EmbarqueID = data[i].EmbarqueID;
                    }
                }
                $("#Embarque").data("kendoComboBox").dataSource.data(data);
                $("#Embarque").data("kendoComboBox").value(EmbarqueID);
                $("#Embarque").data("kendoComboBox").trigger("change");
            }
        }
    });
    loadingStop();
}

function AjaxGetProveedoresEnvio(proyectoID) {
    loadingStart();
    $AsignacionEnvio.AsignacionEnvio.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoProveedor: 2 }).done(function (data) {
        $("#ProveedorEnvio").data("kendoComboBox").dataSource.data([]);
        var ProveedorId = 0;        
        if (data.length > 0) {
            $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data);
            if (data.length == 1) {                
                if (data[i].ProveedorEnvioID != 0) {                    
                    $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data);
                    $("#ProveedorEnvio").data("kendoComboBox").value(data[0].ProveedorEnvioID);
                    $("#ProveedorEnvio").data("kendoComboBox").select(1);
                    $("#ProveedorEnvio").data("kendoComboBox").trigger("change");
                }                
            } else {
                $("#ProveedorEnvio").data("kendoComboBox").dataSource.data(data);
                //$("#ProveedorEnvio").data("kendoComboBox").trigger("change");
            }
            data.splice(0, 0, { ProveedorEnvioID: -1, ProveedorEnvio: _dictionary.EmarquePreparacionAgregarProveedorEnvio[$("#language").data("kendoDropDownList").value()] });
        }        
        loadingStop();
    });
}

function AjaxGetTractoEnvio(ProveedorID, nuevoTracto) {
    loadingStart();
    $AsignacionEnvio.AsignacionEnvio.read({ token: Cookies.get("token"), ProveedorID: ProveedorID, A: "", B: "" }).done(function (data) {
        var TractoEnvioID = 0;
        $("#TractoEnvio").data("kendoComboBox").dataSource.data([]);        
        if (data.length > 0) {
            if (data.length == 1) {
                for (var i = 0; i < data.length; i++) {
                    TractoEnvioID = data[i].TractoEnvioID;
                    $("#TractoEnvio").data("kendoComboBox").dataSource.data(data);
                    $("#TractoEnvio").data("kendoComboBox").value(TractoEnvioID);
                    $("#TractoEnvio").data("kendoComboBox").select(1);
                    $("#TractoEnvio").data("kendoComboBox").trigger("change");
                }
            } else {
                for (var j = 0; j < data.length; j++) {
                    TractoEnvioID = data[j].TractoEnvioID;
                }
                $("#TractoEnvio").data("kendoComboBox").dataSource.data(data);
                $("#TractoEnvio").data("kendoComboBox").value(TractoEnvioID);
            }            
            data.splice(0, 0, { TractoEnvioID: -1, TractoEnvio: _dictionary.EmarquePreparacionAgregarTractoEnvio[$("#language").data("kendoDropDownList").value()] });
            $("#TractoEnvio").data("kendoComboBox").dataSource.data(data);            
        } else {
            data.splice(0, 0, { TractoEnvioID: -1, TractoEnvio: _dictionary.EmarquePreparacionAgregarTractoEnvio[$("#language").data("kendoDropDownList").value()] });
            $("#TractoEnvio").data("kendoComboBox").dataSource.data(data);
            $("#TractoEnvio").data("kendoComboBox").value("");
        }
        loadingStop();
    });
}

function AjaxGetChoferesEnvio(proveedorID, nuevoChofer) {
    loadingStart();
    $AsignacionEnvio.AsignacionEnvio.read({ token: Cookies.get("token"), ProveedorID: proveedorID, A: "", B: "", C: "" }).done(function (data) {
        $("#ChoferEnvio").data("kendoComboBox").dataSource.data([]); //add
        if (data.length > 0) {
            var ChoferEnvioID = 0;            
            if (data.length == 1) {
                for (var i = 0; i < data.length; i++) {
                    ChoferEnvioID = data[i].ChoferEnvioID;
                    $("#ChoferEnvio").data("kendoComboBox").dataSource.data(data);
                    $("#ChoferEnvio").data("kendoComboBox").value(ChoferEnvioID);
                    $("#ChoferEnvio").data("kendoComboBox").select(1);
                }                
            } else {
                for (var j = 0; j < data.length; j++) {
                    ChoferEnvioID = data[j].ChoferEnvioID;
                }
                data.splice(0, 0, { ChoferEnvioID: -1, ChoferEnvio: _dictionary.EmarquePreparacionAgregarChoferEnvio[$("#language").data("kendoDropDownList").value()] });
                $("#ChoferEnvio").data("kendoComboBox").dataSource.data(data); //add                        
                $("#ChoferEnvio").data("kendoComboBox").value(ChoferEnvioID); //add                        
            }
            data.splice(0, 0, { ChoferEnvioID: -1, ChoferEnvio: _dictionary.EmarquePreparacionAgregarChoferEnvio[$("#language").data("kendoDropDownList").value()] });                        
        } else {
            data.splice(0, 0, { ChoferEnvioID: -1, ChoferEnvio: _dictionary.EmarquePreparacionAgregarChoferEnvio[$("#language").data("kendoDropDownList").value()] });
            $("#ChoferEnvio").data("kendoComboBox").dataSource.data(data); //add            
            $("#ChoferEnvio").data("kendoComboBox").value("");
        }
        loadingStop();
    });
}

function GuardarNuevoProveedorEnvio() {        
    $Proveedores.Proveedores.read({ token: Cookies.get("token"), NombreProveedor: $("#inputNombreNuevoProveedorEnvio").val(), ProyectoID: $("#Proyecto").data("kendoComboBox").value(), Descripcion: "", Direccion: "", Telefono: "", TipoProveedor: 2 }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                AjaxGetEmbarques($("#Proyecto").data("kendoComboBox").value());
                //AjaxEmbarqueCargaProveedores($("#Proyecto").data("kendoComboBox").value(), $("#inputNombreNuevoProveedor").val());
                windowNewProvider.close();
                displayNotify("MensajeGuardadoExistoso", "", "0");
            }
            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                displayNotify("EmbarquePreparacionErrorExisteProveedor", "", '2');
            }
        }
    });
}



function GuardarNuevoTractoEnvio() {
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), NombreTracto: $("#inputNombreNuevoTractoEnvio").val(), ProveedorID: $("#ProveedorEnvio").data("kendoComboBox").value(), TipoProveedor: 2 }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {                
                AjaxGetTractoEnvio($("#ProveedorEnvio").data("kendoComboBox").value() == undefined ? 0 : $("#ProveedorEnvio").data("kendoComboBox").value(), null);
                windowNewTracto.close();
                displayNotify("MensajeGuardadoExistoso", "", "0");
            }
            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {

                displayNotify("EmbarquePreparacionErrorExisteTracto", "", '2');
            }

        }
    });
}

function GuardarNuevoChoferEnvio() {
    $EmbarqueGeneral.EmbarqueGeneral.read({ token: Cookies.get("token"), NombreChofer: $("#inputNombreNuevoChoferEnvio").val(), ProveedorID: $("#ProveedorEnvio").data("kendoComboBox").value() }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {                
                AjaxGetChoferesEnvio($("#ProveedorEnvio").data("kendoComboBox").value() == undefined ? 0 : $("#ProveedorEnvio").data("kendoComboBox").value(), null);
                windowNewChofer.close();
                displayNotify("MensajeGuardadoExistoso", "", "0");
            }
            else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {

                displayNotify("EmbarquePreparacionErrorExisteChofer", "", '2');
            }

        }
    });
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


function GuardarCaptura(tipoGuardado) {
    loadingStart();    
    var embarqueID = $("#Embarque").data("kendoComboBox").value();
    var proveedorE = $("#ProveedorEnvio").data("kendoComboBox").value();
    var tractoE = $("#TractoEnvio").data("kendoComboBox").value();
    var choferE = $("#ChoferEnvio").data("kendoComboBox").value();
    $AsignacionEnvio.AsignacionEnvio.read({ token: Cookies.get("token"), EmbarqueID: embarqueID, ProveedorEnvioID: proveedorE, TractoEnvioID: tractoE, ChoferEnvioID: choferE, Relleno: true }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            if (tipoGuardado == 1) {
                opcionHabilitarView(true);
            } else {
                Limpiar();
            }
            displayNotify("MensajeGuardadoExistoso", "", '0');
        } else {
            displayNotify("MensajeGuardadoErroneo", "", '2');
        }
    });
    loadingStop();
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