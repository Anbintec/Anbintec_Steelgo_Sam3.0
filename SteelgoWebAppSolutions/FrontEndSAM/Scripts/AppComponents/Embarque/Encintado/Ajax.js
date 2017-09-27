function AjaxCargarCamposPredeterminados() {
    var TipoMuestraPredeterminadoTipoBusqueda = 3063;
    var TipoMuestraPredeterminadoMuestra = 3064;
    var TipoMuestraPredeterminadoPlanchado = 3065;
    var TipoMuestraPredeterminadoSelecTodos = 3062;

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoPlanchado }).done(function (data) {
        if (data == "Todos") {
            $('input:radio[name=LLena]:nth(0)').trigger("click");
        }
        else if (data == "Vacios") {
            $('input:radio[name=LLena]:nth(1)').trigger("click");
        }
        loadingStop();
    });

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoSelecTodos }).done(function (data) {
        if (data == "Si") {
            $('input:radio[name=SelectTodos]:nth(0)').trigger("click");
        }
        else if (data == "No") {
            $('input:radio[name=SelectTodos]:nth(1)').trigger("click");
        }
        else if (data == "Ninguno") {
            $('input:radio[name=SelectTodos]:nth(2)').trigger("click");
        }
        loadingStop();
    });

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoTipoBusqueda }).done(function (data) {
        if (data == "Zona") {
            $('input:radio[name=TipoBusqueda]:nth(0)').trigger("click");
        }
        else if (data == "Spool") {
            $('input:radio[name=TipoBusqueda]:nth(1)').trigger("click");
        }
        loadingStop();
    });

    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoMuestra }).done(function (data) {
        if (data == "Todos") {
            $('input:radio[name=Muestra]:nth(0)').trigger("click");
        }
        else if (data == "Vacios") {
            $('input:radio[name=Muestra]:nth(1)').trigger("click");
        }
        loadingStop();
    });

    AjaxCargarZona();
    AjaxObtenerColores();
}

function AjaxCargarZona() {
    loadingStart();
    $Zona.Zona.read({ token: Cookies.get("token") }).done(function (data) {
        $("#InputZona").data("kendoComboBox").dataSource.data([]);

        var zonaid = 0;
        if (data.length > 0) {
            $("#InputZona").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].ZonaID != 0) {
                        zonaid = data[i].ZonaID;
                    }
                }
            }

            $("#InputZona").data("kendoComboBox").value(zonaid);
            $("#InputZona").data("kendoComboBox").trigger("change");
        }

        loadingStop();
    });
}

function AjaxCargarCuadrante(zonaID) {
    loadingStart();
    $Cuadrante.Cuadrante.read({ token: Cookies.get("token"), ZonaID: zonaID }).done(function (data) {
        var cuadranteid = 0;
        if (data.length > 0) {
            $("#InputCuadrante").data("kendoComboBox").dataSource.data(data);
            if (data.length < 3) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].CuadranteID != 0) {
                        cuadranteid = data[i].CuadranteID;
                    }
                }
            }

            $("#InputCuadrante").data("kendoComboBox").value(cuadranteid);
            $("#InputCuadrante").data("kendoComboBox").trigger("change");

            if ($("#styleZona").hasClass("active")) {
                $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data(data);

                $("#InputCuadrantePlanchado").data("kendoComboBox").value(cuadranteid);
                $("#InputCuadrantePlanchado").data("kendoComboBox").trigger("change");
            }
        }

        loadingStop();
    });
}

function AjaxCargarCuadranteSpool(spool) {
    loadingStart();
    if (spool != "") {
        $Cuadrante.Cuadrante.read({ token: Cookies.get("token"), Spool: spool }).done(function (data) {

            var cuadranteid = 0;
            if (data.length > 0) {
                $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data(data);
                if (data.length < 3) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].CuadranteID != 0) {
                            cuadranteid = data[i].CuadranteID;

                        }
                    }
                }
                $("#InputCuadrantePlanchado").data("kendoComboBox").value(cuadranteid);
                $("#InputCuadrantePlanchado").data("kendoComboBox").trigger("change");
            }

            loadingStop();
        });
    }
}

function AjaxCargarElementosPorConsulta(tipoBusqueda, zonaID, cuadranteID, spoolContiene, todos) {
    loadingStart();    
    $Encintado.Encintado.read({
        token: Cookies.get("token"), TipoConsulta: tipoBusqueda, ZonaID: zonaID, CuadranteID: cuadranteID,
        NumeroControl: spoolContiene
    }).done(function (data) {
        if (data != null) {
            if (data > 100) {
                if (data < 5000) {
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
                            close: false,
                            open: false
                        }
                    }).data("kendoWindow");
                    ventanaConfirmBusqueda.content('<center>' + _dictionary.EmbarqueAlertaCantidadRegistros[$("#language").data("kendoDropDownList").value()] + '</center>' +
                        "</br><center><button class='btn btn-blue' id='btnContinuarBusqueda'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button> <button class='btn btn-blue' id='btnCancelarBusqueda'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirmBusqueda.open().center();
                    $("#btnContinuarBusqueda").click(function () {
                        ventanaConfirmBusqueda.close();
                        AjaxCargarDetalleEncintado(tipoBusqueda, zonaID, cuadranteID, spoolContiene, todos);


                    });
                    $("#btnCancelarBusqueda").click(function () {
                        ventanaConfirmBusqueda.close();
                        loadingStop();
                    });

                } else {
                    displayNotify("EmbarqueEtiquetadoMensajeNumeroElementosExcedente", "", '1');
                    loadingStop();
                }


            } else {
                AjaxCargarDetalleEncintado(tipoBusqueda, zonaID, cuadranteID, spoolContiene, todos);

            }
        }
    });
}

function AjaxCargarDetalleEncintado(tipoBusqueda, zonaID, cuadranteID, spoolContiene, todos) {
    loadingStart();
    $Encintado.Encintado.read({
        token: Cookies.get("token"), TipoConsulta: tipoBusqueda, Todos: todos, ZonaID: zonaID, CuadranteID: cuadranteID,
        SpoolContiene: spoolContiene, lenguaje: $("#language").val()
    }).done(function (data) {
        $("#grid").data('kendoGrid').dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;
        loadingStart();
        if (data.length > 0) {
            ds.data(data);
            ds.page(1);
            if (tipoBusqueda != 1)
                AjaxCargarCuadranteSpool(spoolContiene);


        } else {
            ds.page(0);
        }
        ds.sync();


    });
}

function AjaxGuardarCaptura(rows, tipoGuardar) {
    loadingStart();

    var Captura = [];
    Captura[0] = { listaDetalle: "" };
    var ListaDetalles = [];
    var index = 0;
    for (var i = 0; i < rows.length; i++) {
        ListaDetalles[index] = {
            Accion: "", EncintadoID: 0, SpoolID: 0, Encintado: false,
            ColorID: 0, ColorAnteriorID: 0, CuadranteID: 0, CuadranteSam2ID: 0, CuadranteAnteriorID: 0,
            CuadranteAnteriorSam2ID: 0, Estatus: 1
        };

        ListaDetalles[index].Accion = rows[i].Accion;
        ListaDetalles[index].EncintadoID = rows[i].EncintadoID;
        ListaDetalles[index].SpoolID = rows[i].SpoolID;
        ListaDetalles[index].Encintado = rows[i].Encintado;
        ListaDetalles[index].ColorID = rows[i].ColorID;
        ListaDetalles[index].CuadranteID = rows[i].CuadranteID;
        ListaDetalles[index].CuadranteSam2ID = rows[i].CuadranteSam2ID;

        if (rows[i].CuadranteID != null && rows[i].CuadranteID == rows[i].CuadranteAnteriorSam3ID) {
            ListaDetalles[index].CuadranteAnteriorID = 0;
            ListaDetalles[index].CuadranteAnteriorSam2ID = 0;
        } else {
            ListaDetalles[index].CuadranteAnteriorID = rows[i].CuadranteAnteriorSam3ID == null ? -1 : rows[i].CuadranteAnteriorSam3ID;
            ListaDetalles[index].CuadranteAnteriorSam2ID = rows[i].CuadranteAnteriorSam2ID == null ? -1 : rows[i].CuadranteAnteriorSam2ID;
        }

        if (rows[i].ColorID == rows[i].ColorAnteriorID)
            ListaDetalles[index].ColorAnteriorID = 0;
        else
            ListaDetalles[index].ColorAnteriorID = rows[i].ColorAnteriorID;

        if (rows[i].Accion === 1 || rows[i].Accion === 2) {
            if (rows[i].Accion === 2) {
                if (!rows[i].Encintado && rows[i].ColorID == 0) {
                    ListaDetalles[index].Accion = 3;
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = true;
                } else {
                    if (rows[i].Encintado && rows[i].ColorID == 0) {
                        ListaDetalles[index].Estatus = 0;
                        $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                        //$('tr[data-uid="' + rows[i].uid + '"] ').css("background-color", "#ffcccc");
                    } else
                        $("#grid").data("kendoGrid").dataSource._data[i].RowOk = true;
                }
            } else if (rows[i].Accion === 1) {
                if (rows[i].Encintado && rows[i].ColorID == 0) {
                    ListaDetalles[index].Estatus = 0;
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = false;
                    //$('tr[data-uid="' + rows[i].uid + '"] ').css("background-color", "#ffcccc");
                } else
                    $("#grid").data("kendoGrid").dataSource._data[i].RowOk = true;
            }
        }

        index++;
    };

    $("#grid").data("kendoGrid").dataSource.sync();
    Captura[0].listaDetalle = ListaDetalles;

    if (Captura[0].listaDetalle.length > 0) {
        if (!ExistRowEmpty(ListaDetalles)) {
            $Encintado.Encintado.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
                if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                    if (tipoGuardar == 1) {
                        var TipoBusqueda = $('input:radio[name=TipoBusqueda]:checked').val() == "Zona" ? 1 : 2;
                        var Todos = 1;
                        var ZonaID = 0;
                        var CuadranteID = 0;
                        var SpoolIDContiene = "";

                        if (TipoBusqueda == 1) {
                            ZonaID = $("#InputZona").data("kendoComboBox").value();
                            CuadranteID = $("#InputCuadrante").data("kendoComboBox").value();
                        } else {
                            SpoolIDContiene = $("#InputSpoolIDCOntiene").val();
                        }
                        opcionHabilitarView(true, "FieldSetView");
                        AjaxCargarDetalleEncintado(TipoBusqueda, ZonaID, CuadranteID, SpoolIDContiene, Todos);
                    } else {
                        Limpiar();
                        loadingStop();
                    }
                    displayNotify("MensajeGuardadoExistoso", "", '0');
                } else {
                    displayNotify("MensajeGuardadoErroneo", "", '2');
                    loadingStop();
                }
            });
        } else {
            loadingStop();
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                draggable: false,
                resizable: false,
                modal: true,
                animation: {
                    close: false,
                    open: false
                },
                actions: []
            }).data("kendoWindow");

            ventanaConfirm.content('<center>' + _dictionary.MensajeConfirmacionGuardadoGeneral[$("#language").data("kendoDropDownList").value()] + '</center>' +
                "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (e) {
                loadingStart();
                var x = 0;
                var listaDetallesGuardar = [];

                for (var i = 0; i < ListaDetalles.length; i++) {
                    if (ListaDetalles[i].Estatus == 1) {

                        listaDetallesGuardar[x] = {
                            Accion: "", EncintadoID: 0, SpoolID: 0, Encintado: false,
                            ColorID: 0, ColorAnteriorID: 0, CuadranteID: 0, CuadranteSam2ID: 0, CuadranteAnteriorID: 0,
                            CuadranteAnteriorSam2ID: 0, Estatus: 1
                        };

                        listaDetallesGuardar[x].Accion = ListaDetalles[i].Accion;
                        listaDetallesGuardar[x].EncintadoID = ListaDetalles[i].EncintadoID;
                        listaDetallesGuardar[x].SpoolID = ListaDetalles[i].SpoolID;
                        listaDetallesGuardar[x].Encintado = ListaDetalles[i].Encintado;
                        listaDetallesGuardar[x].ColorID = ListaDetalles[i].ColorID;
                        listaDetallesGuardar[x].ColorAnteriorID = ListaDetalles[i].ColorAnteriorID;
                        listaDetallesGuardar[x].CuadranteID = ListaDetalles[i].CuadranteID;
                        listaDetallesGuardar[x].CuadranteSam2ID = ListaDetalles[i].CuadranteSam2ID;
                        listaDetallesGuardar[x].CuadranteAnteriorID = ListaDetalles[i].CuadranteAnteriorID;
                        listaDetallesGuardar[x].CuadranteAnteriorSam2ID = ListaDetalles[i].CuadranteAnteriorSam2ID;
                        x++;
                    }
                }

                Captura[0].listaDetalle = listaDetallesGuardar;

                if (Captura[0].listaDetalle.length > 0) {
                    ventanaConfirm.close();
                    var TipoBusqueda = $('input:radio[name=TipoBusqueda]:checked').val() == "Zona" ? 1 : 2;
                    var Todos = 1;
                    var ZonaID = 0;
                    var CuadranteID = 0;
                    var SpoolIDContiene = "";
                    if (TipoBusqueda == 1) {
                        ZonaID = $("#InputZona").data("kendoComboBox").value();
                        CuadranteID = $("#InputCuadrante").data("kendoComboBox").value();
                    } else {
                        SpoolIDContiene = $("#InputSpoolIDCOntiene").val();
                    }

                    $Encintado.Encintado.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
                        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                            if (tipoGuardar == 1) {
                                var TipoBusqueda = $('input:radio[name=TipoBusqueda]:checked').val() == "Zona" ? 1 : 2;
                                var Todos = 1;
                                var ZonaID = 0;
                                var CuadranteID = 0;
                                var SpoolIDContiene = "";

                                if (TipoBusqueda == 1) {
                                    ZonaID = $("#InputZona").data("kendoComboBox").value();
                                    CuadranteID = $("#InputCuadrante").data("kendoComboBox").value();
                                } else {
                                    SpoolIDContiene = $("#InputSpoolIDCOntiene").val();
                                }

                                opcionHabilitarView(true, "FieldSetView");
                                AjaxCargarDetalleEncintado(TipoBusqueda, ZonaID, CuadranteID, SpoolIDContiene, Todos);
                            } else {
                                Limpiar();
                                loadingStop();
                            }
                            displayNotify("MensajeGuardadoExistoso", "", '0');
                        } else {
                            displayNotify("MensajeGuardadoErroneo", "", '2');
                            loadingStop();
                        }
                    });
                    loadingStop();
                } else {
                    ventanaConfirm.close();
                    loadingStop();
                    displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
                }

            });
            $("#noButton").click(function (e) {
                ventanaConfirm.close();
            });
        }

    }
    else {
        loadingStop();
        displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
    }
}

function AjaxObtenerColores() {
    loadingStart();
    $Encintado.Encintado.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            $("#InputColorCintaPlanchado").data("kendoComboBox").dataSource.data([]);
            $("#InputColorCintaPlanchado").data("kendoComboBox").dataSource.data(data);
        }
        loadingStop();
    });
}