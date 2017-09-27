var SpoolContiene = "";   
var ProyectoIDAnterior = 0;
var TipoMuestraPredeterminadoID = 3052;
var SpoolNombre = "";
var tmpData;
function AjaxCargarCamposPredeterminados() {
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (Error(data)) {
            if (data == "sin captura") {
                $('input[name="Muestra"][value="SinCaptura"]').prop('checked', true);
            }
            else if (data == "Todos") {
                $('input[name="Muestra"][value="Todos"]').prop('checked', true);
            }
        }
        loadingStop();
    });
    $("#InputOrdenTrabajo").val("");
    $("#InputID").data("kendoComboBox").value("");
    $("#Proyecto").data("kendoComboBox").value("");
    $("#InputNumeroControl").val("");
    AjaxGetListaProyectos();
};

function AjaxGetListaProyectos() {
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#Proyecto").data("kendoComboBox").dataSource.data(data);
            if ($("#Proyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#Proyecto").data("kendoComboBox").select(1);
                $("#Proyecto").data("kendoComboBox").trigger("change");
                ProyectoIDAnterior = data[1].ProyectoID;
            }
            else {
                ProyectoIDAnterior = 0;
            }
        }
    });
}

function AjaxCheckNumControl(ProyectoID, NumControl) {
    loadingStart();
    LimpiaFiltro();
    $OkDocumental.OkDocumental.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, NumControl: NumControl }).done(function (data) {
        //if (Error(data)) {
            if (data === 1) { //Si Pertenece
                AjaxGetNumeroElementos(ProyectoID, NumControl);
            } else {
                //No pertenece al proyecto
                displayNotify("EmbarqueCargaMsjErrorSpoolAgregarProyectoIncorrecto", "", "1");
                $("#grid").data("kendoGrid").dataSource.data([]);
            }
        //}        
    });
}

function EjecutarGuardadoCaptura(Captura, IdProyecto, tipoGuardado, TipoAgregado) {
    //TipoAgregado: 1 --> NombreSpool 0 --> SpoolID    
    $OkDocumental.OkDocumental.create(Captura[0], { Lenguaje: $("#language").val(), ProyectoID: IdProyecto, token: Cookies.get("token"), Relleno: true }).done(function (data) {
        if (Error(data)) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                if (data.ReturnMessage[0] != undefined) {
                    if (tipoGuardado == 1) { //Guardar y Nuevo
                        Limpiar();
                        opcionHabilitarView(false, "FieldSetView");
                        $("#Proyecto").data("kendoComboBox").value("");
                    }
                    else { //Guardar
                        if (TipoAgregado == 1)
                            AjaxObtenerDetalleXNombreSpool(IdProyecto, $("#InputNumeroControl").val());                        
                        opcionHabilitarView(true, "FieldSetView");
                    }
                    displayNotify("MensajeGuardadoExistoso", "", "0");
                }
            } else {
                opcionHabilitarView(false, "FieldSetView");
            }
        }
    });    
}

function AjaxGuardarCaptura(arregloCaptura, tipoGuardado) {
    var grid = $("#grid").data("kendoGrid");
    var ds = grid.dataSource;
    var IdProyecto = 0;
    Captura = [];
    Captura[0] = { Detalle: "" };    
    ListaCaptura = [];
    var cont = 0;
    for (var i = 0; i < arregloCaptura.length; i++) {
        ListaCaptura[cont] = {
            SpoolID: 0,
            OrdenTrabajoSpoolID: 0,
            OkDocumental: 0            
        }
        ListaCaptura[cont].SpoolID = arregloCaptura[i].SpoolID;
        ListaCaptura[cont].OrdenTrabajoSpoolID = arregloCaptura[i].OrdenTrabajoSpoolID;
        ListaCaptura[cont].OkDocumental = arregloCaptura[i].OkDocumental;
        ListaCaptura[cont].ModificadoPorUsuario = arregloCaptura[i].ModificadoPorUsuario;
        cont++;
    }

    Captura[0].Detalle = ListaCaptura;
    if ($("input:radio[name=TipoAgregado]:checked").val() == "SpoolContiene") { //POR NOMBRE DE SPOOL
        IdProyecto = $("#Proyecto").data("kendoComboBox").value();
        if (IdProyecto != 0) {
            if ($("#InputNumeroControl").val() != "") {
                EjecutarGuardadoCaptura(Captura, IdProyecto, tipoGuardado, 1);
            } else {
                displayNotify("SPAMensajeIngresaSpool", "", "1");
            }
        } else {
            displayNotify("MensajeSeleccionaProyecto", "", "1");
        }

    } else { // POR SPOOL ID
        var OrdenTrabajo = $("#InputOrdenTrabajo").val();
        var InputID = $("#InputID").data("kendoComboBox").text();

        if (OrdenTrabajo != "" && InputID != "") {
            EjecutarGuardadoCaptura(Captura, IdProyecto2, tipoGuardado, 0);
        } else {
            displayNotify("EmbarqueEmpaquetadoMsjErrorSpoolID", "", "1");
        }
    }
}

function AjaxGuardadoMasivo(data) {
    var OK = false;
    CapturaMasiva = [];
    CapturaMasiva[0] = { Detalle: "" };
    CapturaMasiva[0].Detalle = JSON.stringify(data);
    var proyectoID = 0;
    if ($("#Proyecto").data("kendoComboBox").value() != undefined) {
        if ($("#Proyecto").data("kendoComboBox").value() != 0) {
            proyectoID = $("#Proyecto").data("kendoComboBox").value();
        }
    } else {
        proyectoID = IdProyecto2;
    }    
    $OkDocumental.OkDocumental.create(CapturaMasiva[0], { Lenguaje: $("#language").val(), token: Cookies.get("token"), ProyectoID: proyectoID }).done(function (data) {
        if (Error(data)) {
            download(data, "ResultadoCargaMasiva.csv", "text/csv");
            if (!$("#InputNumeroControl").val().length > 0 && !$("#grid").data("kendoGrid").dataSource.data().length > 0 ) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#InputNumeroControl").val("");
            } else {
                if ($("#Proyecto").data("kendoComboBox").value() != 0 && $("#InputNumeroControl").val() != "") {
                    AjaxObtenerDetalleXNombreSpool($("#Proyecto").data("kendoComboBox").value(), $("#InputNumeroControl").val());
                }                
            }
            displayNotify("MensajeGuardadoExistoso", "", "0");
            OK = true;
        }
    });
    return OK;
};

function AjaxObtenerJuntas(SpoolID) {
    try {
        if (SpoolID != undefined || SpoolID != "" || SpoolID != null) {
            $OkDocumental.OkDocumental.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), SpoolID: SpoolID }).done(function (data) {                
                if (Error) {
                    LlenarGridPopUp(data);                    
                }
            });
        }
    } catch (e) {
        displayNotify("", "Ocurrion un Error: " + e.message, "2");
        return;
    }
}

function AjaxGetNumeroElementosPorNombre(ProyectoID, NombreSpool) {
    $OkDocumental.OkDocumental.read({ token: Cookies.get("token"), ProyectoID: ProyectoID, Nombre: NombreSpool, Muestra: $('input:radio[name=Muestra]:checked').val() }).done(function (data) {
        if (data > 100) {
            var ventanaConfirmBusqueda = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "45%",
                height: "auto",
                draggable: false,
                actions: [],
                modal: true,
                animation: {
                    close: true,
                    open: false
                }
            }).data("kendoWindow");

            ventanaConfirmBusqueda.content('<center>' + _dictionary.EmbarqueAlertaCantidadRegistros[$("#language").data("kendoDropDownList").value()] + '</center>' +
                "</br><center><button class='btn btn-blue' id='btnContinuarBusqueda'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button> <button class='btn btn-blue' id='btnCancelarBusqueda'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirmBusqueda.open().center();
            $("#btnContinuarBusqueda").click(function (e) {
                ventanaConfirmBusqueda.close();
                AjaxObtenerDetalleXNombreSpool(ProyectoID, NombreSpool);
            });

            $("#btnCancelarBusqueda").click(function () {
                ventanaConfirmBusqueda.close();
                loadingStop();
            });
        } else {
            AjaxObtenerDetalleXNombreSpool(ProyectoID, NombreSpool);
        }
    });
}

function AjaxObtenerDetalleXNombreSpool(ProyectoID, NombreSpool) {
    SpoolNombre = NombreSpool;
    if (ProyectoID != 0 && NombreSpool != "") {
        $OkDocumental.OkDocumental.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), ProyectoID: ProyectoID, Nombre: NombreSpool, Muestra: $('input:radio[name=Muestra]:checked').val() }).done(function (data) {
            try {
                $("#grid").data("kendoGrid").dataSource.data([]);
                var dataSource = $("#grid").data("kendoGrid").dataSource;
                if (Error(data)) {
                    tmpData = data;
                    $("#InputNumeroControl").val(SpoolNombre.toString().toUpperCase());
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            dataSource.add(data[i]);
                        }
                        $("#grid").data("kendoGrid").dataSource.sync();
                    } else {
                        displayNotify("NODATA", "", "1");
                        $("#grid").data("kendoGrid").dataSource.data([]);
                    }                    
                }
                loadingStop();
            } catch (e) {
                displayNotify("", "Error: " + e.message, "2");
            }
        });
    }
}
function AjaxObtenerElementosPorOrdenTrabajo(IdProyecto, OrdenTrabajo) {
    loadingStart();
    if (IdProyecto != 0 && OrdenTrabajo != "") {        
        $OkDocumental.OkDocumental.read({ token: Cookies.get("token"), Lenguaje: $("#language").val(), ProyectoID: IdProyecto, OrdenTrabajo: OrdenTrabajo, Muestra: $('input:radio[name=Muestra]:checked').val(), Relleno: true }).done(function (data) {
            try {                
                if (Error(data)) {
                    if (data.length > 0) {
                        var ds = $("#grid").data("kendoGrid").dataSource;                        
                        for (var i = 0; i < data.length; i++) {
                            if (!ExisteSpool(data[i])) {
                                ds.insert(0, data[i]);
                            } else {
                                displayNotify('EmbarqueCargaMsjErrorSpoolAgregarExiste', '', '1');
                            }
                        }
                        ds.sync();
                    } else {
                        displayNotify('NoExisteSpoolID', '', '1');
                    }                  
                }
                loadingStop();
            } catch (e) {
                loadingStop();
                displayNotify("", "Error: " + e.message, "2");                
            }
        }); 
    }    
}