function AjaxCargarCamposPredeterminados() {
    var tipoCampo = 3050;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: tipoCampo }).done(function (data) {
        if (data == "sin captura") {
            $('input:radio[name=Muestra]:nth(0)').trigger("click");
        }
        else if (data == "Todos") {
            $('input:radio[name=Muestra]:nth(1)').trigger("click");
        }
        AjaxCargaTipoLlenado();
    });
}
function AjaxCargaTipoLlenado() {
    var tipoCampo = 3051;
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: tipoCampo }).done(function (data) {
        if (data == "vacios") {
            $('input:radio[name=Planchar]:nth(0)').trigger("click");
        }
        else if (data == "Todos") {
            $('input:radio[name=Planchar]:nth(1)').trigger("click");
        }
    });
}

function AjaxObtenerElementoRequisicion(paramReq) {

    $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: paramReq }).done(function (data) {
        if (Error(data)) {
            if (data != null) {
                $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
                $("#inputProyecto").data("kendoComboBox").dataSource.data(data.listaProyecto);
                $("#inputProyecto").data("kendoComboBox").value(data.ProyectoID);

                $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
                $("#inputTipoPrueba").data("kendoComboBox").dataSource.data(data.listaTipoPrueba);
                $("#inputTipoPrueba").data("kendoComboBox").value(data.TipoPruebaID);

                $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
                $("#inputProveedor").data("kendoComboBox").dataSource.data(data.listaProveedor);
                $("#inputProveedor").data("kendoComboBox").value(data.ProveedorID);

                $("#inputRequisicion").data("kendoComboBox").dataSource.data([]);
                $("#inputRequisicion").data("kendoComboBox").dataSource.data(data.listaRequisicion);
                $("#inputRequisicion").data("kendoComboBox").value(data.RequisicionID);
                changeLabel(data.TipoPruebaID);
                AjaxObtieneDetalleRequisicion();
            }
        }
    });
}

function AjaxCargaListaProyectos() {
    $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            var proyectoID = 0;
            $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProyectoID != 0) {
                        proyectoID = data[i].ProyectoID;
                    }

                }
            }

            $("#inputProyecto").data("kendoComboBox").value(proyectoID);
            $("#inputProyecto").data("kendoComboBox").trigger("change");
        }
    });
}
function AjaxCargaListaTipoPrueba(ProyectoID) {
    $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ proyectoID: ProyectoID, token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            var tipoPruebaID = 0;
            $("#inputTipoPrueba").data("kendoComboBox").dataSource.data([]);
            $("#inputTipoPrueba").data("kendoComboBox").dataSource.data(data);

            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].TipoPruebaID != 0) {
                        tipoPruebaID = data[i].TipoPruebaID;
                    }
                }
            }

            $("#inputTipoPrueba").data("kendoComboBox").value(tipoPruebaID);
            $("#inputTipoPrueba").data("kendoComboBox").trigger("change");
        }
    });
}
function AjaxCargaListaProveedores(proyectoID, tipoPruebaID) {
    $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), ProyectoID: proyectoID, TipoPruebaID: tipoPruebaID }).done(function (data) {
        if (Error(data)) {
            $("#inputProveedor").data("kendoComboBox").dataSource.data([]);
            $("#inputProveedor").data("kendoComboBox").dataSource.data(data);

            var ProveedorID = 0;
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ProveedorID != 0) {
                        ProveedorID = data[i].ProveedorID;
                    }

                }
            }
            $("#inputProveedor").data("kendoComboBox").value(ProveedorID);
            $("#inputProveedor").data("kendoComboBox").trigger("change");;
        }
    });
}

function AjaxCargaListaRequisicion(proyectoID, tipoPruebaID, proveedorID) {

    $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), ProyectoID: proyectoID, lenguaje: $("#language").val(), proveedorID: proveedorID, tipoPruebaID: tipoPruebaID }).done(function (data) {
        if (Error(data)) {
            $("#inputRequisicion").data("kendoComboBox").dataSource.data(data);

            var RequisicionID = 0;
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].RequisicionID != 0) {
                        RequisicionID = data[i].RequisicionID;
                    }
                }
            }

            $("#inputRequisicion").data("kendoComboBox").value(RequisicionID);
            $("#inputRequisicion").data("kendoComboBox").trigger("change");
        }

    });
}

function AjaxCargaListaDocumentoRecibido() {
    var respuesta = 1;
    //$EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), numeroCatalogo: respuesta, lenguaje: $("#language").val() }).done(function (data) {
    //    $("#inputDocumentoRecibido").data("kendoComboBox").dataSource.data([]);
    //    $("#inputDocumentoRecibido").data("kendoComboBox").dataSource.data(data);

    //    var DocumentoRecibidoID = 0;
    //    if (data.length < 3) {
    //        for (var i = 0; i < data.length; i++) {
    //            if (data[i].DocumentoRecibidoID != 0) {
    //                DocumentoRecibidoID = data[i].DocumentoRecibidoID;
    //            }
    //        }
    //    }

    //    $("#inputDocumentoRecibido").data("kendoComboBox").value(DocumentoRecibidoID);
    //    $("#inputDocumentoRecibido").data("kendoComboBox").trigger("change");

    //    AjaxCargaListaDocumentoEstatus();
    //});

}

function AjaxCargaListaDocumentoEstatus() {
    var respuesta = 2;

    //$EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), numeroCatalogo: respuesta, lenguaje: $("#language").val() }).done(function (data) {
    //    $("#inputCondicionesFisicas").data("kendoComboBox").dataSource.data([]);
    //    $("#inputCondicionesFisicas").data("kendoComboBox").dataSource.data(data);
    //    var DocumentoEstatusID = 0;

    //    if (data.length < 3) {
    //        for (var i = 0; i < data.length; i++) {
    //            if (data[i].DocumentoEstatusID != 0) {
    //                DocumentoEstatusID = data[i].DocumentoEstatusID;
    //            }
    //        }
    //    }

    //    $("#inputCondicionesFisicas").data("kendoComboBox").value(DocumentoEstatusID);
    //    $("#inputCondicionesFisicas").data("kendoComboBox").trigger("change");

    //    AjaxCargaListaDocumentoDefecto();
    //});
}

function AjaxCargaListaDocumentoDefecto() {
    var respuesta = 3;
    $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), numeroCatalogo: respuesta, lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            $("#inputDefectos").data("kendoComboBox").dataSource.data([]);
            $("#inputDefectos").data("kendoComboBox").dataSource.data(data);

            var DocumentoDefectoID = 0;
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].DocumentoDefectoID != 0) {
                        DocumentoDefectoID = data[i].DocumentoDefectoID;
                    }
                }
            }

            $("#inputDefectos").data("kendoComboBox").value(DocumentoDefectoID);
            $("#inputDefectos").data("kendoComboBox").trigger("change");
        }
    });    
}

function AjaxObtieneDetalleRequisicion() {
    var ProyectoID = $("#inputProyecto").data("kendoComboBox").value();
    var TipoPruebaID = $("#inputTipoPrueba").data("kendoComboBox").value();
    var ProveedorID = $("#inputProveedor").data("kendoComboBox").value();
    var RequisicionID = $("#inputRequisicion").data("kendoComboBox").value();

    if (ProyectoID != 0 && TipoPruebaID != 0 && ProveedorID != 0 && RequisicionID != 0) {
        $EntregaPlacasGraficas.EntregaPlacasGraficas.read({ token: Cookies.get("token"), proyectoID: ProyectoID, proveedorID: ProveedorID, requisicionID: RequisicionID, lenguaje: $("#language").val(), tipoPruebaID: TipoPruebaID }).done(function (data) {
            if (Error(data)) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                var tipoPrueba;
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        ds.add(data[i]);
                        tipoPrueba = data[i].TipoPruebaID;
                    }
                }
            }
        });
    }
}

function AjaxGuardarCaptura(ds, guardarYNuevo) {
    if (ds.length > 0) {
        var RequisicionID = 0;
        var Captura = [];
        Captura[0] = { Detalles: "" }
        var listaDetalles = [];
        var cont = 0;
        for (var i = 0; i < ds.length; i++) {

            listaDetalles[cont] = {
                Accion: "",
                EntregaPlacasGraficasID: "",
                RequisicionID: "",
                OrdenTrabajoID: "",
                SpoolID: "",
                JuntaID: "",
                DocumentoRecibidoID: "",
                DocumentoEstatusID: "",
                DocumentoDefectoID: "",
                Estatus: 1
            };

            listaDetalles[cont].Accion = ds[i].Accion;
            listaDetalles[cont].EntregaPlacasGraficasID = ds[i].EntregaPlacasGraficasID;
            listaDetalles[cont].RequisicionID = ds[i].RequisicionID;
            listaDetalles[cont].OrdenTrabajoID = ds[i].OrdenTrabajoID;
            listaDetalles[cont].SpoolID = ds[i].SpoolID;
            listaDetalles[cont].JuntaID = ds[i].JuntaSpoolID;
            listaDetalles[cont].DocumentoRecibidoID = ds[i].DocumentoRecibidoID;
            listaDetalles[cont].DocumentoEstatusID = ds[i].DocumentoEstatusID;
            listaDetalles[cont].DocumentoDefectoID = ds[i].DocumentoDefectoID;

            if (listaDetalles[cont].DocumentoRecibidoID == 0 || listaDetalles[cont].DocumentoEstatusID == 0 || listaDetalles[cont].DocumentoDefectoID == 0) {
                if (listaDetalles[cont].Accion == 2) {
                    if (listaDetalles[cont].DocumentoRecibidoID == 0 && listaDetalles[cont].DocumentoEstatusID == 0 && listaDetalles[cont].DocumentoDefectoID == 0) {
                        listaDetalles[cont].Accion = 4;
                    } else {
                        if (listaDetalles[cont].DocumentoRecibidoID == 0) {
                            listaDetalles[cont].Estatus = 0;
                            $('tr[data-uid="' + ds[i].uid + '"] ').css("background-color", "#ffcccc");
                        }
                        if (listaDetalles[cont].DocumentoEstatusID == 0) {
                            listaDetalles[cont].Estatus = 0;
                            $('tr[data-uid="' + ds[i].uid + '"] ').css("background-color", "#ffcccc");
                        }

                        if (listaDetalles[cont].DocumentoEstatusID == 2 && listaDetalles[cont].DocumentoDefectoID == 0) {
                            listaDetalles[cont].Estatus = 0;
                            $('tr[data-uid="' + ds[i].uid + '"] ').css("background-color", "#ffcccc");
                        }
                    }

                } else {

                    if (listaDetalles[cont].DocumentoRecibidoID == 0) {
                        listaDetalles[cont].Estatus = 0;
                        $('tr[data-uid="' + ds[i].uid + '"] ').css("background-color", "#ffcccc");
                    }
                    if (listaDetalles[cont].DocumentoEstatusID == 0) {
                        listaDetalles[cont].Estatus = 0;
                        $('tr[data-uid="' + ds[i].uid + '"] ').css("background-color", "#ffcccc");
                    }

                    if (listaDetalles[cont].DocumentoEstatusID == 2 && listaDetalles[cont].DocumentoDefectoID == 0) {
                        listaDetalles[cont].Estatus = 0;
                        $('tr[data-uid="' + ds[i].uid + '"] ').css("background-color", "#ffcccc");
                    }
                }

            }
            if (ds[i].Accion != 2 && RequisicionID == 0) {
                RequisicionID = $("#inputRequisicion").data("kendoComboBox").value();
            }
            cont++;
        }

        loadingStart();
        setTimeout(function () {
            disableEnableView(true);
            displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", '0');
            loadingStop();
        }, 500);


        //var TipoPrueba = $("#inputTipoPrueba").data("kendoComboBox").dataItem($("#inputTipoPrueba").data("kendoComboBox").select());
        //if (!ExistRowEmpty(listaDetalles)) {
        //    Captura[0].Detalles = listaDetalles;
        //    $EntregaPlacasGraficas.EntregaPlacasGraficas.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), requisicionID: RequisicionID, tipoPruebaPorSpool: TipoPrueba.TipoPruebaPorSpool }).done(function (data) {
        //        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
        //            if (guardarYNuevo) {
        //                cleanView();
        //            } else {
        //                $("#grid").data("kendoGrid").dataSource.data([]);
        //                AjaxObtieneDetalleRequisicion();
        //                disableEnableView(true);
        //            }
        //            displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", '0');
        //        } else {
        //            displayNotify("EntregaPlacasGraficasMensajeGuardadoErroneo", "", '2');
        //        }

        //    });
        //} else {
        //    $("#ventanaConfirm").empty();
        //    var ventanaConfirm = $("#ventanaConfirm").kendoWindow({
        //        iframe: true,
        //        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
        //        visible: false,
        //        width: 450,
        //        height: 70,
        //        draggable: false,
        //        modal: true,
        //        animation: {
        //            close: false,
        //            open: false
        //        }
        //    }).data("kendoWindow");
        //    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
        //        "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'>No</button></center>");
        //    ventanaConfirm.open().center();
        //    $("#yesButton").click(function () {
        //        var x = 0;
        //        var listaDetallesGuardar = [];
        //        for (var i = 0; i < listaDetalles.length; i++) {
        //            if (listaDetalles[i].Estatus == 1) {
        //                listaDetallesGuardar[x] = {
        //                    Accion: "",
        //                    EntregaPlacasGraficasID: "",
        //                    RequisicionID: "",
        //                    OrdenTrabajoID: "",
        //                    SpoolID: "",
        //                    JuntaID: "",
        //                    DocumentoRecibidoID: "",
        //                    DocumentoEstatusID: "",
        //                    DocumentoDefectoID: "",
        //                    Estatus: 1
        //                };
        //                listaDetallesGuardar[x].Accion = listaDetalles[i].Accion;
        //                listaDetallesGuardar[x].EntregaPlacasGraficasID = listaDetalles[i].EntregaPlacasGraficasID;
        //                listaDetallesGuardar[x].RequisicionID = listaDetalles[i].RequisicionID;
        //                listaDetallesGuardar[x].OrdenTrabajoID = listaDetalles[i].OrdenTrabajoID;
        //                listaDetallesGuardar[x].SpoolID = listaDetalles[i].SpoolID;
        //                listaDetallesGuardar[x].JuntaID = listaDetalles[i].JuntaID;
        //                listaDetallesGuardar[x].DocumentoRecibidoID = listaDetalles[i].DocumentoRecibidoID;
        //                listaDetallesGuardar[x].DocumentoEstatusID = listaDetalles[i].DocumentoEstatusID;
        //                listaDetallesGuardar[x].DocumentoDefectoID = listaDetalles[i].DocumentoDefectoID;
        //                x++;
        //            }
        //        }
        //        Captura[0].Detalles = listaDetallesGuardar;
        //        if(Captura[0].Detalles.length>0){
        //            $EntregaPlacasGraficas.EntregaPlacasGraficas.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), requisicionID: RequisicionID, tipoPruebaPorSpool: TipoPrueba.TipoPruebaPorSpool }).done(function (data) {
        //                if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
        //                    if (guardarYNuevo) {
        //                        cleanView();
        //                    } else {
        //                        $("#grid").data("kendoGrid").dataSource.data([]);
        //                        AjaxObtieneDetalleRequisicion();
        //                        disableEnableView(true);
        //                    }
        //                    displayNotify("EntregaPlacasGraficasMensajeGuardadoExistoso", "", '0');
        //                } else {
        //                    displayNotify("EntregaPlacasGraficasMensajeGuardadoErroneo", "", '2');
        //                }
        //                ventanaConfirm.close();
        //            });
        //            ventanaConfirm.close();
        //        } else {
        //            ventanaConfirm.close();
        //            displayNotify("EntregaPlacasGraficasExcepcionGuardado", "", '1');
        //        }
        //    });
        //    $("#noButton").click(function () {
        //        ventanaConfirm.close();
        //    });
        //}
    } else {
        displayNotify("EntregaPlacasGraficasExcepcionGuardado", "", '2');
    }
}
