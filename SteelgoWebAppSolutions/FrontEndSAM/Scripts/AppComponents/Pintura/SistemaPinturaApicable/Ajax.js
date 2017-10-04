function AjaxCargarCamposPredeterminados() {
    var TipoMuestraPredeterminadoID = 3057;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (data == "spool") {
            $('input:radio[name=TipoBusqueda]:nth(0)').trigger("click");
        }
        else if (data == "nc") {
            $('input:radio[name=TipoBusqueda]:nth(1)').trigger("click");
        }
    });
    AjaxCargarTipoLlenado();
}
function AjaxCargarTipoLlenado() {
    var TipoMuestraPredeterminadoID = 3058;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        if (data == "Vacios") {
            $('input:radio[name=Planchar]:nth(0)').trigger("click");
        }
        else if (data == "Todos") {
            $('input:radio[name=Planchar]:nth(1)').trigger("click");
        }
    });
}
function AjaxCargaProyecto() {
    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
        $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

        var proyectoId = 0;

        if (data.length < 3) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].ProyectoID != 0) {
                    proyectoId = data[i].ProyectoID;
                }
            }
        }

        $("#inputProyecto").data("kendoComboBox").value(proyectoId);
        $("#inputProyecto").data("kendoComboBox").trigger("change");
    });
}

function AjaxCargarSistemaPintura(proyectoID) {
    $SistemaPinturaAplicable.SistemaPinturaAplicable.read({ token: Cookies.get("token"), ProyectoID: proyectoID }).done(function (data) {
        
        $("#inputSistemaPintura").data("kendoComboBox").dataSource.data(data);

        var spid = 0;

        if (data.length ==2) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].SistemaPinturaID != 0) {
                    spid = data[i].SistemaPinturaID;
                }
            }
        }

        $("#inputSistemaPintura").data("kendoComboBox").value(spid);
        $("#inputSistemaPintura").data("kendoComboBox").trigger("change");
    });
}

function AjaxCargarColorPintura(sistemaPinturaID) {    
	$SistemaPinturaAplicable.SistemaPinturaAplicable.read({ token: Cookies.get("token"), SistemaPinturaID: sistemaPinturaID, Lenguaje: $("#language").val(), proyectoID: $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select()).ProyectoID }).done(function (data) {
        if (data.length > 1) {
            $("#inputColorPintura").data("kendoComboBox").dataSource.data(data);

            var cpid = 0;

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ColorPinturaID != 0) {
                        cpid = data[i].ColorPinturaID;
                    }
                }
            }

            $("#inputColorPintura").data("kendoComboBox").value(cpid);
            $("#inputColorPintura").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxCargarNumeroElementosPorBusqueda(proyectoID, tipoBusqueda, cadena) {
    $SistemaPinturaAplicable.SistemaPinturaAplicable.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoBusqueda: tipoBusqueda, Cadena: cadena}).done(function (data) {
        if (data != null) {
            if(data>100){
                var ventanaConfirmBusqueda = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "45%",
                    height: "auto",
                    draggable: false,
                    actions: [],
                    modal: true,
                    animation: {
                        close: false,
                        open: false
                    }
                }).data("kendoWindow");
                ventanaConfirmBusqueda.content('<center>' + _dictionary.SPAMensajeAlertaCantidadRegistros[$("#language").data("kendoDropDownList").value()] + '</center>' +
                    "</br><center><button class='btn btn-blue' id='btnContinuarBusqueda'>Si</button> <button class='btn btn-blue' id='btnCancelarBusqueda'>No</button></center>");

                ventanaConfirmBusqueda.open().center();
                $("#btnContinuarBusqueda").click(function () {
                    AjaxCargarDetalleSpool(proyectoID, tipoBusqueda, cadena);
                    ventanaConfirmBusqueda.close();
                });
                $("#btnCancelarBusqueda").click(function () {                    
                    ventanaConfirmBusqueda.close();
                });
            } else {
                AjaxCargarDetalleSpool(proyectoID, tipoBusqueda, cadena);
            }
        } else {

        }
    });
}

function AjaxCargarDetalleSpool(proyectoID, tipoBusqueda, cadena) {
    $SistemaPinturaAplicable.SistemaPinturaAplicable.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoBusqueda: tipoBusqueda, Cadena: cadena, Lenguaje: $("#language").val() }).done(function (data) {
        $("#grid").data("kendoGrid").dataSource.data([]);

        var ds = $("#grid").data("kendoGrid").dataSource;
        if (data.length > 0) {
            editado = true;
            for (var i = 0; i < data.length; i++) {
                if (data[i].ListaColorPintura == null)
                    data[i].ListaColorPintura = [];
                
                ds.add(data[i]);
            }
                ds.page(1);
        }
        else
        {
            displayNotify("ErrorSpoolAgregarProyectoIncorrecto", "", '1');
        }
    });
}

function AjaxCargarColorPinturaRender(sistemaPinturaID, options) {
	$SistemaPinturaAplicable.SistemaPinturaAplicable.read({ token: Cookies.get("token"), SistemaPinturaID: sistemaPinturaID, Lenguaje: $("#language").val(), proyectoID:$("#inputProyecto").val() }).done(function (data) {
        if(data.length>0){
            options.model.ListaColorPintura = data
            $("#grid").data("kendoGrid").dataSource.sync();
        }
        
   });
}

function AjaxGuardarCaptura(listaCaptura, GuardarYNuevo) {
    var Captura = { detalle: "" };
    var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());

    var listaDetalles = [];
    for (var i = 0; i < listaCaptura.length; i++) {
        listaCaptura[i].RowOk = true;
        listaDetalles[i] = {
            Accion: "",
            SpoolAplicableID: "",
            OrdenTrabajoID: "",
            SpoolID: "",
            ProyectoID : "",
            SistemaPinturaID: "",
            SistemaPinturaColorID: "",
            Estatus: 1
        };
            
        listaDetalles[i].Accion = listaCaptura[i].Accion;
        listaDetalles[i].SpoolAplicableID = listaCaptura[i].SpoolAplicableID;
        listaDetalles[i].OrdenTrabajoID = listaCaptura[i].OrdenTrabajoID;
        listaDetalles[i].SpoolID = listaCaptura[i].SpoolID;
        listaDetalles[i].ProyectoID = Proyecto.ProyectoID;
        listaDetalles[i].SistemaPinturaID = listaCaptura[i].SistemaPinturaID;
        listaDetalles[i].SistemaPinturaColorID = listaCaptura[i].SistemaPinturaColorID;

        if (listaDetalles[i].Accion === 1 || listaDetalles[i].Accion === 2) {
            if (listaDetalles[i].Accion === 2) {
                if (listaDetalles[i].SistemaPinturaID == 0 && listaDetalles[i].SistemaPinturaColorID == 0) {
                    listaDetalles[i].Accion = 4;
                } else {

                    if (listaDetalles[i].SistemaPinturaID == 0) {
                        listaDetalles[i].Estatus = 0;
                        listaCaptura[i].RowOk = false;
                        //$('tr[data-uid="' + listaCaptura[i].uid + '"] ').css("background-color", "#ffcccc");
                    }
                    if (listaDetalles[i].SistemaPinturaColorID == 0 && listaCaptura[i].ListaColorPintura.length > 1) {
                        listaDetalles[i].Estatus = 0;
                        listaCaptura[i].RowOk = false;
                        //$('tr[data-uid="' + listaCaptura[i].uid + '"] ').css("background-color", "#ffcccc");
                    }
                }
            }else{
                listaDetalles[i].Estatus = 0;
                listaCaptura[i].RowOk = false;
                    //$('tr[data-uid="' + listaCaptura[i].uid + '"] ').css("background-color", "#ffcccc");
            }
        }
    }
    if (!ExistRowEmpty(listaDetalles)) {
        Captura.detalle = listaDetalles;
        $SistemaPinturaAplicable.SistemaPinturaAplicable.create(Captura,{ token: Cookies.get("token")}).done(function (data) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {

                if (GuardarYNuevo) {
                    
                    LimpiarPantalla();
                    editado = false;
                } else {
                    var tipoBusqueda = 0;
                    var cadena = "";

                    $("#grid").data("kendoGrid").dataSource.data([]);
                    if ($("#styleSpool").hasClass("active")) {
                        tipoBusqueda = 1;
                        cadena = $('#inputSpool').val();
                    } else if ($("#styleNc").hasClass("active")) {
                        tipoBusqueda = 2;
                        cadena = $('#inputNc').val();
                    }
                    AjaxCargarDetalleSpool(Proyecto.ProyectoID, tipoBusqueda, cadena);;
                    opcionHabilitarView(true);
                }

                displayNotify("SistemaPinturaAplicableMensajeGuardadoExistoso", "", '0');
            } else {
                displayNotify("SistemaPinturaAplicableMensajeGuardadoErroneo", "", '2');
            }
        });
    } else {
        $("#grid").data("kendoGrid").dataSource.sync();
        ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
            iframe: true,
            title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
            visible: false,
            width: "auto",
            height: "auto",
            modal: true,
            resizable: false,
            animation: false,
            actions: []
        }).data("kendoWindow");

        ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
            "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


        ventanaConfirm.open().center();
        $("#yesButton").click(function () {
            var x = 0;
            var listaDetallesGuardar = [];
            for (var i = 0; i < listaDetalles.length; i++) {
                if (listaDetalles[i].Estatus == 1) {
                    listaDetallesGuardar[x] = {
                        Accion: "",
                        SpoolAplicableID: "",
                        OrdenTrabajoID: "",
                        SpoolID: "",
                        ProyectoID: "",
                        SistemaPinturaID: "",
                        SistemaPinturaColorID: "",
                        Estatus: 1
                    };
                    listaDetallesGuardar[x].Accion = listaDetalles[i].Accion;
                    listaDetallesGuardar[x].SpoolAplicableID = listaDetalles[i].SpoolAplicableID;
                    listaDetallesGuardar[x].OrdenTrabajoID = listaDetalles[i].OrdenTrabajoID;
                    listaDetallesGuardar[x].SpoolID = listaDetalles[i].SpoolID;
                    listaDetallesGuardar[x].ProyectoID = listaDetalles[i].ProyectoID;
                    listaDetallesGuardar[x].SistemaPinturaID = listaDetalles[i].SistemaPinturaID;
                    listaDetallesGuardar[x].SistemaPinturaColorID = listaDetalles[i].SistemaPinturaColorID;
                    listaDetallesGuardar[x].Estatus = listaDetalles[i].Estatus;

                    x++;
                }
            }

            Captura.detalle = listaDetallesGuardar;
            if (Captura.detalle.length > 0) {
                $SistemaPinturaAplicable.SistemaPinturaAplicable.create(Captura, { token: Cookies.get("token") }).done(function (data) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {

                        if (GuardarYNuevo) {
                            LimpiarPantalla();
                            editado = false;
                        } else {
                            var tipoBusqueda = 0;
                            var cadena = "";

                            $("#grid").data("kendoGrid").dataSource.data([]);
                            if ($("#styleSpool").hasClass("active")) {
                                tipoBusqueda = 1;
                                cadena = $('#inputSpool').val();
                            } else if ($("#styleNc").hasClass("active")) {
                                tipoBusqueda = 2;
                                cadena = $('#inputNc').val();
                            }
                            AjaxCargarDetalleSpool(Proyecto.ProyectoID, tipoBusqueda, cadena);;
                            opcionHabilitarView(true);
                        }

                        displayNotify("SistemaPinturaAplicableMensajeGuardadoExistoso", "", '0');
                    } else {
                        displayNotify("SistemaPinturaAplicableMensajeGuardadoErroneo", "", '2');
                    }
                });
                ventanaConfirm.close();
            } else {
                ventanaConfirm.close();
                displayNotify("SistemaPinturaAplicableExcepcionGuardado", "", '1');
            }

        });
        $("#noButton").click(function () {
            ventanaConfirm.close();
        });
    }
}

function AjaxGuardaCargaMasiva(data, tipoCarga) {
    var Captura = { detalle: data }
    loadingStart();
    $SistemaPinturaAplicable.SistemaPinturaAplicable.create(Captura, { token: Cookies.get("token"), TipoCarga: tipoCarga, Lenguaje: $("#language").val(), ProyectoID: $("#inputProyecto").data("kendoComboBox").value() }).done(function (data) {
        if(data.length>0){
            download(data, "ResultadoSistemaPinturaAplicable.csv", "text/csv");
            displayNotify("SistemaPinturaAplicableMensajeGuardadoExistoso", "", '0');
        } else {

            displayNotify("", _dictionary.notificationslabel0009[$("#language").data("kendoDropDownList").value()] + data.ReturnMessage[0], '2');
        }

        loadingStop();
    });
}