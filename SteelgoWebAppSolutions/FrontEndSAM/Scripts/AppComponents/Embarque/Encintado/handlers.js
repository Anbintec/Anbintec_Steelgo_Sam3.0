var ZonaActual = 0;
var CuadranteActual = 0;
var SpoolIDActual = "";
var VistaActual = "";

function SuscribirEventos() {
    SuscribirEventoZona();
    SuscribirEventoCuadrante();
    SuscribirEventoCuadrantePlanchado();
    SuscribirEventoEncintadoPlanchado();
    SuscribirEventoCambiarVista();
    SuscribirEventoChangeRadio();
    SuscribirEventoMostrar();
    SuscribirEventoPlanchar();
    SuscribirEventoGuardar();
    SuscribirEventoImprimirTraveler();
}

function SuscribirEventoZona() {
    $("#InputZona").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ZonaID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (existenCambios()) {
                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    LimpiarCargarZona();
                    if (dataItem != undefined) {
                        ZonaActual = dataItem.ZonaID;
                        if (dataItem.ZonaID != 0) {
                            AjaxCargarCuadrante(dataItem.ZonaID);
                        }
                    }
                    else {
                        $("#InputZona").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    $('#InputZona').data("kendoComboBox").value(ZonaActual);
                    ventanaConfirm.close();
                });

            }
            else {
                LimpiarCargarZona();
                if (dataItem != undefined) {
                    ZonaActual = dataItem.ZonaID;
                    if (dataItem.ZonaID != 0) {
                        AjaxCargarCuadrante(dataItem.ZonaID);
                    }
                }
                else {
                    $("#InputZona").data("kendoComboBox").value("");
                }
            }

        }
    });
}

function SuscribirEventoCuadrante() {
    $("#InputCuadrante").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "CuadranteID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (existenCambios()) {
                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                          "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    if (dataItem != undefined) {
                        CuadranteActual = dataItem.CuadranteID;
                    }
                    else {
                        $("#InputCuadrante").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    $('#InputCuadrante').data("kendoComboBox").value(CuadranteActual);
                    ventanaConfirm.close();
                });

            }
            else {
                $("#grid").data("kendoGrid").dataSource.data([]);
                if (dataItem != undefined) {
                    CuadranteActual = dataItem.CuadranteID;
                }
                else {
                    $("#InputCuadrante").data("kendoComboBox").value("");
                }
            }
        }
    });

    $('#InputCuadrante').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            var Todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;
            var TipoBusqueda = 1;
            var ZonaID = $("#InputZona").data("kendoComboBox").value();
            var Cuadrante = $("#InputCuadrante").data("kendoComboBox").dataItem($("#InputCuadrante").data("kendoComboBox").select());

            if (!existenCambios()) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                
                if (ZonaID != "0" && ZonaID != "") {
                    if (Cuadrante != undefined) {
                        CuadranteActual = Cuadrante.CuadranteID;
                        AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, Cuadrante.CuadranteID, "", Todos);
                    }
                } else {
                    displayNotify("MensajeSeleccionarZona", "", '2');
                }
            } else {
                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                          "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    ventanaConfirm.close();
                    $("#grid").data("kendoGrid").dataSource.data([]);

                    if (ZonaID != "0" && ZonaID != "") {
                        if (Cuadrante != undefined) {
                            CuadranteActual = Cuadrante.CuadranteID;
                            AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, Cuadrante.CuadranteID, "", Todos);
                        }
                    } else {
                        displayNotify("MensajeSeleccionarZona", "", '2');
                    }
                });
                $("#noButton").click(function (handler) {
                    $('#InputCuadrante').data("kendoComboBox").value(CuadranteActual);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoCuadrantePlanchado() {
    $("#InputCuadrantePlanchado").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "CuadranteID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
            }
            else {
                $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
            }

        }
    });
}

function SuscribirEventoEncintadoPlanchado() {
    $("#InputColorCintaPlanchado").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ColorID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
        }
    });
}

function SuscribirEventoCambiarVista() {
    $('#styleZona').click(function () {

        if (!existenCambios()) {
            VistaActual = "Zona";
            $("#InputZona").data("kendoComboBox").value("");

            $("#InputCuadrante").data("kendoComboBox").dataSource.data([]);
            $("#InputCuadrante").data("kendoComboBox").value("");

            $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
            $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
            $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
            $('input:radio[name=SelectTodos]:nth(2)').trigger("click");

            $("#grid").data("kendoGrid").dataSource.data([]);

            $("#SpoolDiv").hide();
            $("#ZonaDiv").show();
            $("#CuadranteDiv").show();
        } else {
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.CapturaAvanceTitulo[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                modal: true,
                draggable: false,
                resizable: false,
                animation: {
                    close: false,
                    open: false
                },
                actions: []
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                          "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                VistaActual = "Zona";
                $("#InputZona").data("kendoComboBox").value("");

                $("#InputCuadrante").data("kendoComboBox").dataSource.data([]);
                $("#InputCuadrante").data("kendoComboBox").value("");

                $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
                $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
                $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
                $('input:radio[name=SelectTodos]:nth(2)').trigger("click");

                $("#grid").data("kendoGrid").dataSource.data([]);

                $("#SpoolDiv").hide();
                $("#ZonaDiv").show();
                $("#CuadranteDiv").show();
                $("#grid").data("kendoGrid").dataSource.data([]);
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                $("input[name='TipoBusqueda']").val(VistaActual);
                if (VistaActual == "Spool") {
                    $("#styleSpool").addClass("active");
                    $("#styleZona").removeClass("active");
                } else if (VistaActual == "Zona") {
                    $("#styleSpool").removeClass("active");
                    $("#styleZona").addClass("active");
                }
                ventanaConfirm.close();
            });
        }
    });
    $('#styleSpool').click(function () {
        if (!existenCambios()) {
            VistaActual = "Spool";
            $("#InputSpoolIDCOntiene").val("");

            $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
            $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
            $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
            $('input:radio[name=SelectTodos]:nth(2)').trigger("click");

            $("#grid").data("kendoGrid").dataSource.data([]);

            $("#ZonaDiv").hide();
            $("#CuadranteDiv").hide();
            $("#SpoolDiv").show();
        } else {
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                modal: true,
                draggable: false,
                resizable: false,
                animation: {
                    close: false,
                    open: false
                },
                actions: []
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                          "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                VistaActual = "Spool";
                $("#InputSpoolIDCOntiene").val("");

                $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
                $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
                $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
                $('input:radio[name=SelectTodos]:nth(2)').trigger("click");

                $("#grid").data("kendoGrid").dataSource.data([]);

                $("#ZonaDiv").hide();
                $("#CuadranteDiv").hide();
                $("#SpoolDiv").show();
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                $("input[name='TipoBusqueda']").val(VistaActual);
                if (VistaActual == "Spool") {
                    $("#styleSpool").addClass("active");
                    $("#styleZona").removeClass("active");
                } else if (VistaActual == "Zona") {
                    $("#styleSpool").removeClass("active");
                    $("#styleZona").addClass("active");
                }
                ventanaConfirm.close();
            });
        }
    });
}

function SuscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        //FiltroMostrar(0);
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        //FiltroMostrar(1);
    });
}

function SuscribirEventoMostrar() {
    $("#btnMostrar").click(function (e) {
        var TipoBusqueda = $('input:radio[name=TipoBusqueda]:checked').val() == "Zona" ? 1 : 2;
        var Todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;
        var ZonaID = 0;
        var CuadranteID = 0;
        var SpoolIDContiene = "";

        if (!existenCambios()) {
            $("#grid").data('kendoGrid').dataSource.data([]);
            if (TipoBusqueda == 1) {
                ZonaID = $("#InputZona").data("kendoComboBox").value();
                CuadranteID = $("#InputCuadrante").data("kendoComboBox").value();

                if (ZonaID != "0" && ZonaID != "") {
                    AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, CuadranteID == "" ? 0 : CuadranteID, SpoolIDContiene, Todos);
                } else {
                    displayNotify("MensajeSeleccionaZona", "", '1');
                }
            } else if (TipoBusqueda == 2) {
                SpoolIDContiene = $("#InputSpoolIDCOntiene").val();
                SpoolIDActual = $("#InputSpoolIDCOntiene").val();
                LimpiarCargaSpoolContiene();

                if (SpoolIDContiene != "") {
                    if (SpoolIDContiene.length > 2) {
                        AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, CuadranteID, SpoolIDContiene, Todos);
                    }
                    else {
                        displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                    }
                } else {
                    displayNotify("MensajeSeleccionaSpoolcontiene", "", '1');
                }
            }
        } else {
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                modal: true,
                draggable: false,
                resizable: false,
                animation: {
                    close: false,
                    open: false
                },
                actions: []
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                          "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                ventanaConfirm.close();
                $("#grid").data('kendoGrid').dataSource.data([]);
                if (TipoBusqueda == 1) {
                    ZonaID = $("#InputZona").data("kendoComboBox").value();
                    CuadranteID = $("#InputCuadrante").data("kendoComboBox").value();

                    if (ZonaID != "0" && ZonaID != "") {
                        AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, CuadranteID == "" ? 0 : CuadranteID, SpoolIDContiene, Todos);
                    } else {
                        displayNotify("MensajeSeleccionaZona", "", '1');
                    }
                } else if (TipoBusqueda == 2) {
                    SpoolIDContiene = $("#InputSpoolIDCOntiene").val();
                    SpoolIDActual = $("#InputSpoolIDCOntiene").val();
                    LimpiarCargaSpoolContiene();

                    if (SpoolIDContiene != "") {
                        if (SpoolIDContiene.length > 2) {
                            AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, CuadranteID, SpoolIDContiene, Todos);
                        }
                        else {
                            displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                        }
                        //AjaxCargarElementosPorConsulta(TipoBusqueda, ZonaID, CuadranteID, SpoolIDContiene, Todos);
                    } else {
                        displayNotify("MensajeSeleccionaSpoolcontiene", "", '1');
                    }
                }
            });
            $("#noButton").click(function (handler) {
                if (TipoBusqueda == 2) {
                    $("#InputSpoolIDCOntiene").val(SpoolIDActual);
                }
                ventanaConfirm.close();
            });
        }
        
    });

    $('#InputSpoolIDCOntiene').keydown(function (e) {
        if (e.keyCode == 13) {
            var Todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;
            var TipoBusqueda = 2;
            var SpoolIDContiene = $('#InputSpoolIDCOntiene').val();

            if (!existenCambios()) {
                LimpiarCargaSpoolContiene();               

                if (SpoolIDContiene != "") {
                    SpoolIDActual = SpoolIDContiene;
                    if (SpoolIDContiene.length > 2) {
                        AjaxCargarElementosPorConsulta(TipoBusqueda, 0, 0, SpoolIDContiene, Todos);
                    }
                    else {
                        displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                    }

                }
            } else {
                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                          "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    ventanaConfirm.close();
                    LimpiarCargaSpoolContiene();

                    if (SpoolIDContiene != "") {    
                        SpoolIDActual = SpoolIDContiene;
                        if (SpoolIDContiene.length > 2) {
                            AjaxCargarElementosPorConsulta(TipoBusqueda, 0, 0, SpoolIDContiene, Todos);
                        }
                        else {
                            displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                        }
                    }
                });
                $("#noButton").click(function (handler) {
                    $('#InputSpoolIDCOntiene').val(SpoolIDActual);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoPlanchar() {
    $("#btnPlanchar").click(function (e) {
        e.preventDefault();
        var Cuadrante = $("#InputCuadrantePlanchado").data("kendoComboBox").dataItem($("#InputCuadrantePlanchado").data("kendoComboBox").select());
        var Encintado = $('input:radio[name=SelectTodos]:checked').val();
        var ColorCinta = $("#InputColorCintaPlanchado").data("kendoComboBox").dataItem($("#InputColorCintaPlanchado").data("kendoComboBox").select());
        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
                ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.TituloPopupCancelar[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    draggable: false,
                    resizable: false,
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content("<center>" + _dictionary.MensajeAdvertenciaPlancharTodos[$("#language").data("kendoDropDownList").value()] + "</center>" +
                              "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                         + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                         + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    ventanaConfirm.close();
                    loadingStart();

                    if (Cuadrante != undefined && Cuadrante.CuadranteID != 0) {
                        PlancharCuadrante(Cuadrante);
                    }

                    if (Encintado != "Ninguno") {
                        PlanchaEncintado(Encintado);
                    }

                    if (ColorCinta != undefined && ColorCinta.ColorID != 0) {
                        PlanchaColorCinta(ColorCinta);
                    }

                    loadingStop();
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            }
            else if ($('input:radio[name=LLena]:checked').val() === "Vacios") {
                loadingStart();

                if (Cuadrante != undefined && Cuadrante.CuadranteID != 0) {
                    PlancharCuadrante(Cuadrante);
                }

                if (Encintado != "Ninguno") {
                    PlanchaEncintado(Encintado);
                }

                if (ColorCinta != undefined && ColorCinta.ColorID != 0) {
                    PlanchaColorCinta(ColorCinta);
                }

                loadingStop();
            } else {
                displayNotify("MensajeErrorTipoPlanchado", "", '2');
            }
        }
    });
}

function SuscribirEventoGuardar() {
    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            if (ds._data.length > 0) 
                AjaxGuardarCaptura(ds._data, 1);
            else 
                displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
        }
        else if ($('#Guardar').text() == _dictionary.textoEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false, "FieldSetView")
        }
             
    });

    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            AjaxGuardarCaptura(ds._data, 2);
        } else {
            displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
        }
    });
}

function SuscribirEventoImprimirTraveler() {

    $("#btnImprimirTravelSup, #btnImprimirTravelInf").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;

    });
}

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $('#FieldSetView2').find('*').attr('disabled', true);

        $("#InputZona").data("kendoComboBox").enable(false);
        $("#InputCuadrante").data("kendoComboBox").enable(false);
        $("#InputSpoolIDCOntiene").prop('disabled', false);
        $("#InputSpoolIDCOntiene").css('opacity', '0.6');

        $("#InputCuadrantePlanchado").data("kendoComboBox").enable(false);
        $("#InputColorCintaPlanchado").data("kendoComboBox").enable(false);

        $("#btnPlanchar").prop('disabled', true);
        $("#LlenaVacios").prop('disabled', true);
        $("#SelectTodos").prop('disabled', true);

        $("#LlenaTodos").prop('disabled', true);
        $("#SelectTodosSi").prop('disabled', true);
        $("#SelectTodosNo").prop('disabled', true);
        $("#SelectTodosNinguno").prop('disabled', true);

        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        
    } else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $('#FieldSetView2').find('*').attr('disabled', false);

        $("#InputZona").data("kendoComboBox").enable(true);
        $("#InputCuadrante").data("kendoComboBox").enable(true);
        $("#InputSpoolIDCOntiene").prop('disabled', false);
        $("#InputSpoolIDCOntiene").css('opacity', '1');
        

        $("#InputCuadrantePlanchado").data("kendoComboBox").enable(true);
        $("#InputColorCintaPlanchado").data("kendoComboBox").enable(true);

        $("#btnPlanchar").prop('disabled', false);
        $("#LlenaVacios").prop('disabled', false);
        $("#SelectTodos").prop('disabled', false);

        $("#LlenaTodos").prop('disabled', false);
        $("#SelectTodosSi").prop('disabled', false);
        $("#SelectTodosNo").prop('disabled', false);
        $("#SelectTodosNinguno").prop('disabled', false);


        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function LimpiarCargarZona() {
    $("#InputCuadrante").data("kendoComboBox").dataSource.data([]);
    $("#InputCuadrante").data("kendoComboBox").value("");

    $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
    $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
    $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
    $('input:radio[name=SelectTodos]:nth(2)').trigger("click");

    $("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiarCargaSpoolContiene() {
    $("#InputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
    $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
    $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
    $('input:radio[name=SelectTodos]:nth(2)').trigger("click");

    $("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiarCargaCuadrante() {
    $("#InputCuadrantePlanchado").data("kendoComboBox").value("");
    $("#InputColorCintaPlanchado").data("kendoComboBox").value("");
    $('input:radio[name=SelectTodos]:nth(2)').trigger("click");
    $("#grid").data("kendoGrid").dataSource.data([]);
}

function Limpiar() {
    $("#InputSpoolIDCOntiene").val("");
    $("#grid").data("kendoGrid").dataSource.data([]);
    opcionHabilitarView(false, "FieldSetView");
    AjaxCargarCamposPredeterminados();
}