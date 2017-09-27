var ZonaActual = 0;
var CuadranteActual = 0;
var SpoolIDActual = "";
var VistaActual = "";

function SuscribirEventos() {
    SuscribirEventoZona();
    SuscribirEventoCuadrante();
    SuscribirEventoCuadrantePlanchado();
    SuscribirEventoMostrar();
    SuscribirEventoPlanchar();
    suscribirEventoImprimirEtiqueta();
    suscribirEventoImprimirTravelerMasivo();
    SuscribirEventoGuardar();
    SuscribirEventoCambiarVista();
    SuscribirEventoMostrarDetalle();
}

SuscribirEventos();


function SuscribirEventoZona() {
    $("#inputZona").kendoComboBox({
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
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    LimpiarCargarZona();
                    if (dataItem != undefined) {
                        ZonaActual = dataItem.ZonaID
                        if (dataItem.ZonaID != 0) {
                            AjaxGetListaCuadrantes(dataItem.ZonaID);
                        }
                    }
                    else {
                        $("#inputZona").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    $('#inputZona').data("kendoComboBox").value(ZonaActual);
                    ventanaConfirm.close();
                });

            }
            else {
                LimpiarCargarZona();
                if (dataItem != undefined) {
                    ZonaActual = dataItem.ZonaID
                    if (dataItem.ZonaID != 0) {
                        AjaxGetListaCuadrantes(dataItem.ZonaID);
                    }
                }
                else {
                    $("#inputZona").data("kendoComboBox").value("");
                }
            }

        }
    });
}

function SuscribirEventoCuadrante() {
    $("#inputCuadrante").kendoComboBox({
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
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    if (dataItem != undefined) {
                        CuadranteActual = dataItem.CuadranteID;
                    }
                    else {
                        $("#inputCuadrante").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    $('#inputCuadrante').data("kendoComboBox").value(CuadranteActual);
                    ventanaConfirm.close();
                });
            }
            else {
                if (dataItem != undefined) {
                    CuadranteActual = dataItem.CuadranteID;
                }
                else {
                    $("#inputCuadrante").data("kendoComboBox").value("");
                }
            }


        }
    });

    $('#inputCuadrante').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {
            var Todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;
            var TipoBusqueda = 1;
            var ZonaID = $("#inputZona").data("kendoComboBox").value();
            var Cuadrante = $("#inputCuadrante").data("kendoComboBox").dataItem($("#inputCuadrante").data("kendoComboBox").select());

            if (!existenCambios()) {
                LimpiarCargaCuadrante();

                if (ZonaID != "0" && ZonaID != "") {
                    if (Cuadrante != undefined) {
                        CuadranteActual = Cuadrante.CuadranteID;
                        AjaxGetElementosPorConsulta(TipoBusqueda, Todos, ZonaID, Cuadrante.CuadranteID, "");
                    }
                } else {
                    displayNotify("MensajeSeleccionaZona", "", '2');
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
                    LimpiarCargaCuadrante();

                    if (ZonaID != "0" && ZonaID != "") {
                        if (Cuadrante != undefined) {
                            CuadranteActual = Cuadrante.CuadranteID;
                            AjaxGetElementosPorConsulta(TipoBusqueda, Todos, ZonaID, Cuadrante.CuadranteID, "");
                        }
                    } else {
                        displayNotify("MensajeSeleccionaZona", "", '2');
                    }
                });
                $("#noButton").click(function (handler) {
                    $('#inputCuadrante').data("kendoComboBox").value(CuadranteActual);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoCuadrantePlanchado() {
    $("#inputCuadrantePlanchado").kendoComboBox({
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
                $("#inputCuadrantePlanchado").data("kendoComboBox").value("");
            }

        }
    });
}

function SuscribirEventoMostrar() {
    $('#btnMostrar').click(function (e) {
        var TipoBusqueda = $('input:radio[name=TipoBusqueda]:checked').val() == "Zona" ? 1 : 2;
        var ZonaID = 0;
        var CuadranteID = 0;
        var SpoolIDContiene = "";
        var Todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;

        if (!existenCambios()) {
            if (TipoBusqueda === 1) {
                ZonaID = $("#inputZona").data("kendoComboBox").value();
                CuadranteID = $("#inputCuadrante").data("kendoComboBox").value();
                LimpiarCargaCuadrante();

                if (ZonaID != "0" && ZonaID != "") {
                    AjaxGetElementosPorConsulta(TipoBusqueda, Todos, ZonaID, CuadranteID == "" ? 0 : CuadranteID, SpoolIDContiene);
                } else {
                    displayNotify("MensajeSeleccionaZona", "", '1');
                }
            } else if (TipoBusqueda === 2) {
                SpoolIDContiene = $("#SpoolIDCOntiene").val();
                SpoolIDActual = $("#SpoolIDCOntiene").val();
                LimpiarCargaSpoolContiene();

                if (SpoolIDContiene != "") {
                    if (SpoolIDContiene.length > 2) {
                        AjaxGetElementosPorConsulta(TipoBusqueda, Todos, ZonaID, CuadranteID == "" ? 0 : CuadranteID, SpoolIDContiene);
                    }
                    else {
                        displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                    }
                } else {
                    displayNotify("MensajeSeleccionaSpoolcontiene", "", '1');
                }
            }
        }
        else {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
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
            $("#yesButtonProy").click(function () {
                ventanaConfirm.close();
                if (TipoBusqueda === 1) {
                    ZonaID = $("#inputZona").data("kendoComboBox").value();
                    CuadranteID = $("#inputCuadrante").data("kendoComboBox").value();
                    LimpiarCargaCuadrante();

                    if (ZonaID != "0" && ZonaID != "") {
                        AjaxGetElementosPorConsulta(TipoBusqueda, Todos, ZonaID, CuadranteID == "" ? 0 : CuadranteID, SpoolIDContiene);
                    } else {
                        displayNotify("MensajeSeleccionaZona", "", '1');
                    }
                } else if (TipoBusqueda === 2) {
                    SpoolIDActual = $("#SpoolIDCOntiene").val();
                    SpoolIDContiene = $("#SpoolIDCOntiene").val();
                    LimpiarCargaSpoolContiene();

                    if (SpoolIDContiene != "") {
                        if (SpoolIDContiene.length > 2)
                            AjaxGetElementosPorConsulta(TipoBusqueda, Todos, ZonaID, CuadranteID == "" ? 0 : CuadranteID, SpoolIDContiene);
                        else {
                            displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                        }
                    } else {
                        displayNotify("MensajeSeleccionaSpoolcontiene", "", '1');
                    }
                }
            });
            $("#noButtonProy").click(function () {
                if (TipoBusqueda == 2) {
                    $("#SpoolIDCOntiene").val(SpoolIDActual);
                }
                ventanaConfirm.close();
            });
        }
    });

    $('#SpoolIDCOntiene').keydown(function (e) {
        if (e.keyCode == 13) {
            if (!existenCambios()) {
                var todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;
                var tipoBusqueda = 0;
                var spoolIDContiene = $('#SpoolIDCOntiene').val();
                LimpiarCargaSpoolContiene();

                if (spoolIDContiene != "") {
                    if (spoolIDContiene.length > 2) {
                        SpoolIDActual = spoolIDContiene;
                        AjaxGetElementosPorConsulta(tipoBusqueda, todos, 0, 0, spoolIDContiene);
                    }
                    else
                        displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
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
                    var todos = $('input:radio[name=Muestra]:checked').val() == "Todos" ? 1 : 2;
                    var tipoBusqueda = 0;
                    var spoolIDContiene = $('#SpoolIDCOntiene').val();
                    LimpiarCargaSpoolContiene();

                    if (spoolIDContiene != "") {
                        if (spoolIDContiene.length > 2) {
                            SpoolIDActual = spoolIDContiene;
                            AjaxGetElementosPorConsulta(tipoBusqueda, todos, 0, 0, spoolIDContiene);
                        }
                        else
                            displayNotify("EmbarqueEtiquetadoMensajeLongitudCadenaSpoolCorta", "", '1');
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    $('#SpoolIDCOntiene').val(SpoolIDActual);
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoGuardar() {
    $("#Guardar, #btnGuardar, #GuardarPie, #btnGuardar1").click(function (e) {
        if ($(this).text().toLowerCase() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()].toLowerCase()) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if (ds._data.length > 0) {
                AjaxGuardarCaptura(ds._data, 0);
            } else {
                displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
            }
        }
        if ($(this).text().toLowerCase() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()].toLowerCase()) {
            opcionHabilitarView(false, "FieldSetView");
        }

    });

    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            AjaxGuardarCaptura(ds._data, 1);
        } else {
            displayNotify("MensajeAdverteciaExcepcionGuardado", "", '2');
        }
    });
}

function SuscribirEventoPlanchar() {
    $("#ButtonPlanchar").click(function (e) {
        e.preventDefault();

        var Cuadrante = $("#inputCuadrantePlanchado").data("kendoComboBox").dataItem($("#inputCuadrantePlanchado").data("kendoComboBox").select());
        var Etiquetado = $('input:radio[name=SelectTodos]:checked').val();

        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            if ($('input:radio[name=LLena]:checked').val() === "Todos") {
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

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                             "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()]
                             + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()]
                             + "</button></center>");

                ventanaConfirm.open().center();

                $("#yesButton").click(function (handler) {
                    if (Cuadrante != undefined) {
                        if (Cuadrante.CuadranteID != 0)
                            PlanchaCuadrante(Cuadrante);
                    }
                    if (Etiquetado != "Ninguno") {
                        PlanchaEtiquedo(Etiquetado);
                    }
                    ventanaConfirm.close();
                });
                $("#noButton").click(function (handler) {
                    ventanaConfirm.close();
                });
            }
            else {
                if (Cuadrante != undefined) {
                    if (Cuadrante.CuadranteID != 0)
                        PlanchaCuadrante(Cuadrante);
                }

                if (Etiquetado != "Ninguno") {
                    PlanchaEtiquedo(Etiquetado);
                }

            }
        }
    });
}

function SuscribirEventoCambiarVista() {
    $('#styleZona').click(function () {

        if (!existenCambios()) {
            VistaActual = "Zona"
            $("#inputZona").data("kendoComboBox").value("");

            $("#inputCuadrante").data("kendoComboBox").dataSource.data([]);
            $("#inputCuadrante").data("kendoComboBox").value("");

            $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
            $("#inputCuadrantePlanchado").data("kendoComboBox").value("");

            $("#SpoolIDDiv").hide();
            $("#ZonaDiv").show();
            $("#CuadranteDiv").show();
            $("#grid").data("kendoGrid").dataSource.data([]);
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
                $("#inputZona").data("kendoComboBox").value("");
                $("#inputCuadrante").data("kendoComboBox").dataSource.data([]);
                $("#inputCuadrante").data("kendoComboBox").value("");

                $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
                $("#inputCuadrantePlanchado").data("kendoComboBox").value("");

                $("#SpoolIDDiv").hide();
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
            $("#SpoolIDCOntiene").val("");

            $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
            $("#inputCuadrantePlanchado").data("kendoComboBox").value("");

            $("#ZonaDiv").hide();
            $("#CuadranteDiv").hide();
            $("#SpoolIDDiv").show();
            $("#grid").data("kendoGrid").dataSource.data([]);
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
                VistaActual = "Spool";
                $("#SpoolIDCOntiene").val("");
                $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
                $("#inputCuadrantePlanchado").data("kendoComboBox").value("");

                $("#ZonaDiv").hide();
                $("#CuadranteDiv").hide();
                $("#SpoolIDDiv").show();
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
}

function suscribirEventoImprimirEtiqueta() {
    $('#ImprimirEtiqueta').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;

        AjaxImprimirEtiqueta(ds._data[0].SpoolID);
    });
}

function suscribirEventoImprimirTravelerMasivo() {
    $('#ImprimirTravel').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;

        AjaxImprimirTravelerMasivo(ds._data[0].SpoolID);
    });
}

function SuscribirEventoMostrarDetalle() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        //FiltroMostrar(0);
    });

    $('input:radio[name=Muestra]:nth(1)').change(function () {
        //FiltroMostrar(1);
    });
}

function opcionHabilitarView(valor, name) {
    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $(".chkbx").attr('disabled', true);
        $("#inputZona").data("kendoComboBox").enable(false);
        $("#inputCuadrante").data("kendoComboBox").enable(false);
        $("#inputCuadrantePlanchado").data("kendoComboBox").enable(false);
        $("#ButtonPlanchar").prop('disabled', true);
        $("#LlenaVacios").prop('disabled', true);
        $("#SelectTodos").prop('disabled', true);
        $("#SpoolIDCOntiene").css('opacity', '0.6');

        $("#LlenaTodos").prop('disabled', true);
        $("#SelectTodosSi").prop('disabled', true);
        $("#SelectTodosNo").prop('disabled', true);
        $("#SelectTodosNinguno").prop('disabled', true);

        $('#Guardar').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#GuardarPie').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $(".chkbx").attr('disabled', false);
        $("#inputZona").data("kendoComboBox").enable(true);
        $("#inputCuadrante").data("kendoComboBox").enable(true);
        $("#inputCuadrantePlanchado").data("kendoComboBox").enable(true);
        $("#ButtonPlanchar").prop('disabled', false);
        $("#LlenaVacios").prop('disabled', false);
        $("#SelectTodos").prop('disabled', false);
        $("#SpoolIDCOntiene").css('opacity', '1');

        $("#LlenaTodos").prop('disabled', false);
        $("#SelectTodosSi").prop('disabled', false);
        $("#SelectTodosNo").prop('disabled', false);
        $("#SelectTodosNinguno").prop('disabled', false);

        $('#Guardar').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#GuardarPie').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function LimpiarBusquedaZona() {
    $("#inputCuadrante").data("kendoComboBox").dataSource.data([]);
    $("#inputCuadrante").val(0);
    $("#inputCuadrante").data("kendoComboBox").value("");
    $("#inputZona").val(0);
    $("#inputZona").data("kendoComboBox").value("");
    $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
    $("#inputCuadrantePlanchado").val(0);
    $("#inputCuadrantePlanchado").data("kendoComboBox").value("");
}

function LimpiarCargarZona() {
    $("#inputCuadrante").data("kendoComboBox").dataSource.data([]);
    $("#inputCuadrante").data("kendoComboBox").value("");

    $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
    $("#inputCuadrantePlanchado").data("kendoComboBox").value("");
    $('input:radio[name=SelectTodos]:nth(2)').trigger("click");
    $("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiarCargaSpoolContiene() {
    $("#inputCuadrantePlanchado").data("kendoComboBox").dataSource.data([]);
    $("#inputCuadrantePlanchado").data("kendoComboBox").value("");
    $('input:radio[name=SelectTodos]:nth(2)').trigger("click");
    $("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiarCargaCuadrante() {
    $("#inputCuadrantePlanchado").data("kendoComboBox").value("");
    $('input:radio[name=SelectTodos]:nth(2)').trigger("click");
    $("#grid").data("kendoGrid").dataSource.data([]);
}

function Limpiar() {
    $("#grid").data("kendoGrid").dataSource.data([]);
    opcionHabilitarView(false, "FieldSetView");
    AjaxCargarCamposPredeterminados();
}