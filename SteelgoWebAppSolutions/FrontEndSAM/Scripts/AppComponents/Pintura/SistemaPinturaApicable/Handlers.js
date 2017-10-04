var ventanaConfirmPlanchar;
var ventanaConfirmEdicion;
var ventanaConfirmEdicionSinTipoBusqueda;

function SuscribirEventos() {
    SuscribirEventoProyecto();
    SuscribirEventoSistemaPintura();
    SuscribirEventoColor();
    SuscribirEventoPlanchado();
    SuscribirEventoTipoBusqueda();
    SuscribirEventoCargarCsv();
    SuscribirEventoBusqueda();
    SuscribirEventoGuardar();
    SuscribirEventoDescargarCsv();
    suscribirEventoWindowsConfirmaCaptura();
    suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda();
}

function suscribirEventoWindowsConfirmaCaptura() {
    ventanaConfirmEdicion = $("#ventanaConfirmCapturaCambioTipo").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicion.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProyTipoB'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProyTipoB'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProyTipoB").click(function (e) {
        $("#grid").data("kendoGrid").dataSource.data([]);
        eventoCambioTipoListado();
        ventanaConfirmEdicion.close();
        editado = false;
    });
    $("#noButtonProyTipoB").click(function (e) {
        eventoRegresarTipoListado();
        ventanaConfirmEdicion.close();
    });
}

function suscribirEventoWindowsConfirmaCapturaSinCambiarTipoBusqueda() {
    ventanaConfirmEdicionSinTipoBusqueda = $("#ventanaConfirmMostrar").kendoWindow({
        iframe: true,
        title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false,
        width: "auto",
        height: "auto",
        modal: true,
        animation: false,
        actions: []
    }).data("kendoWindow");

    ventanaConfirmEdicionSinTipoBusqueda.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
        "</br><center><button class='btn btn-blue' id='yesButtonProySinTipoBusqueda'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProySinTipoBusqueda'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");


    $("#yesButtonProySinTipoBusqueda").click(function (e) {
        ventanaConfirmEdicionSinTipoBusqueda.close();
        editado = false;
        GenerarBusqueda();


    });
    $("#noButtonProySinTipoBusqueda").click(function (e) {
        ventanaConfirmEdicionSinTipoBusqueda.close();
        if ($("#styleSpool").hasClass("active")) {
            $("#inputSpool").val(lineaCaptura.dato);
        }
        else if ($("#styleNc").hasClass("active")) {
            $("#inputNc").val(lineaCaptura.dato);
        }
    });
}

function SuscribirEventoProyecto() {
    $("#inputProyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (!editado) {
                LimpiaCargaProyecto();
                if (dataItem != undefined) {
                    lineaCaptura.ProyectoID = dataItem.ProyectoID;
                    $("#inputProyecto").attr("proyectoAntrior", dataItem.ProyectoID);
                    if (dataItem.ProyectoID != 0) {
                        AjaxCargarSistemaPintura(dataItem.ProyectoID);
                    }
                } else {
                    $("#inputProyecto").data("kendoComboBox").value("");
                }
            } else {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.CapturaArmadoTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    width: "auto",
                    height: "auto",
                    modal: true,
                    animation: false,
                    actions: []
                }).data("kendoWindow");

                ventanaConfirm.content('<center>' + _dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] + '</center>' +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button> <button class='btn btn-blue' id='noButtonProy'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    LimpiaCargaProyecto();
                    if (dataItem != undefined) {
                        $("#inputProyecto").attr("proyectoAntrior", dataItem.ProyectoID);
                        if (dataItem.ProyectoID != 0) {
                            AjaxCargarSistemaPintura(dataItem.ProyectoID);
                        }
                    } else {
                        $("#inputProyecto").data("kendoComboBox").value("");
                    }
                    ventanaConfirm.close();
                });
                $("#noButtonProy").click(function () {
                    $("#inputProyecto").data("kendoComboBox").value($("#inputProyecto").attr("proyectoAntrior"));
                    ventanaConfirm.close();
                });
            }
        }
    });
}

function SuscribirEventoSistemaPintura() {
    $("#inputSistemaPintura").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "SistemaPinturaID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            LimpiaCargaSP();
            if (dataItem != undefined) {
                if (dataItem.SistemaPinturaID != 0 && !dataItem.NoPintable) {
                    AjaxCargarColorPintura(dataItem.SistemaPinturaID);
                }
            } else {
                $("#inputSistemaPintura").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoColor() {
    $("#inputColorPintura").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ColorPinturaID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {

            } else {
                $("#inputColorPintura").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoPlanchado() {
    $("#btnPlanchar").click(function () {

        if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
            var tipoLlenado = $('input:radio[name=Planchar]:checked').val()
            if (tipoLlenado === "Todos") {
                var itemSistemaPintura = $("#inputSistemaPintura").data("kendoComboBox").dataItem($("#inputSistemaPintura").data("kendoComboBox").select());
                var itemColor = $("#inputColorPintura").data("kendoComboBox").dataItem($("#inputColorPintura").data("kendoComboBox").select());
                if ((itemSistemaPintura != undefined && itemSistemaPintura.SistemaPinturaID != 0) || (itemColor != undefined && itemColor.ColorPinturaID != 0)) {
                    ventanaConfirmPlanchar = $("#ventanaConfirmPlanchar").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        width: "25%",
                        height: "auto",
                        actions: [],
                        draggable: false,
                        modal: true,
                        animation: {
                            open: false,
                            close: false
                        },
                    }).data("kendoWindow");

                    ventanaConfirmPlanchar.content('<center>' + _dictionary.EntregaPlacasGraficasPlancharTodos[$("#language").data("kendoDropDownList").value()] + '</center>' +
                                 "</br><center><button class='confirm_yes btn btn-blue' id='yesButtonPlanchar'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button>  <button class='confirm_yes btn btn-blue' id='noButtonPlanchar'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirmPlanchar.open().center();

                    $("#yesButtonPlanchar").click(function (handler) {
                        plancharTodo(tipoLlenado);
                        ventanaConfirmPlanchar.close();
                    });
                    $("#noButtonPlanchar").click(function (handler) {
                        ventanaConfirmPlanchar.close();
                    });
                }
            } else {
                plancharTodo(tipoLlenado);
            }
        }
    });
}

function SuscribirEventoTipoBusqueda() {
    $('input:radio[name=TipoBusqueda]').change(function () {
        if ($('input:radio[name=TipoBusqueda]:checked').val() == "spool") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoListado();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
        else if ($('input:radio[name=TipoBusqueda]:checked').val() == "nc") {
            if (!editado) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                eventoCambioTipoListado();
            }
            else {
                ventanaConfirmEdicion.open().center();
            }
        }
    });
}
function eventoRegresarTipoListado() {
    if ($('input:radio[name=TipoBusqueda]:checked').val() == "spool") {
        $("#divSpool").removeClass("active");
        $("#divNc").addClass("active");
        $('input:radio[name=TipoBusqueda]:nth(1)').attr('checked', true);
        $('input:radio[name=TipoBusqueda]:nth(1)').trigger("click");
    }
    else if ($('input:radio[name=TipoBusqueda]:checked').val() == "nc") {
        $("#divSpool").addClass("active");
        $("#divNc").removeClass("active");
        $('input:radio[name=TipoBusqueda]:nth(0)').attr('checked', true);
        $('input:radio[name=TipoBusqueda]:nth(0)').trigger("click");
    }
}

function eventoCambioTipoListado() {
    if ($('input:radio[name=TipoBusqueda]:checked').val() == "spool") {
        $("#divNc").css('display', 'none');
        $("#divSpool").css('display', 'block');
        LimpiaTipoBusqueda();
    }
    else if ($('input:radio[name=TipoBusqueda]:checked').val() == "nc") {
        $("#divNc").css('display', 'block');
        $("#divSpool").css('display', 'none');
        LimpiaTipoBusqueda();
    }
}

function SuscribirEventoCargarCsv() {
    $("#btnCargaCsv, #btnCargaCsv1").click(function (e) {
        windowLoadFile = $("#windowLoadFile").kendoWindow({
            iframe: true,
            title: _dictionary.SPATituloCargarCsv[$("#language").data("kendoDropDownList").value()],
            visible: false,
            width: "50%",
            height: "auto",
            draggable: false,
            modal: true,
            actions: [],
            animation: {
                open: false,
                close: false
            },
            close: function onClose(e) {
                $('input[value="spool"]').prop("checked", true);
                $("#inputFile").val("");
                $("#lblRutaArchivo").text("");
            }
        }).data("kendoWindow");

        windowLoadFile.open().center();
    });

    $("#btntFile").on('click', function () {
        $("#inputFile").val("");
        $("#inputFile").click();
    });

    $("#inputFile").on('change', function (e) {
        $("#lblRutaArchivo").text("");

        if ($(this).val() != "") {
            if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
                displayNotify("ListadoCatalogos0007", "", '2');
            } else {
                if ($(this).val().substring($(this).val().lastIndexOf(".")) == ".csv") {
                    $("#lblRutaArchivo").text($(this)[0].files['0'].name);
                } else {
                    displayNotify("", "El formato del archivo debe ser .CSV", '1');
                }
            }
        }
    });

    $("#btnGuardarCarga").click(function (e) {
        var ProyectoID = $("#inputProyecto").data("kendoComboBox").text();
        var tipoCarga = $('input[name="TipoCarga"]:checked').val() == "spool" ? 1 : 2;

        if (ProyectoID != "") {
            if ($("#inputFile").val() != "") {
                var data = [];
                var dt = $("#inputFile");
                var reader = new FileReader();
                reader.readAsText(dt[0].files[0]);
                reader.onload = function (event) {
                    var csvData = event.target.result;
                    var csvJsonData = csvToJson(csvData, "NombreSpool", tipoCarga);
                    if (csvJsonData.length > 0) {
                        csvJsonData.forEach(function (c) {
                            data.push(c);
                        });
                        AjaxGuardaCargaMasiva(data, tipoCarga);
                    }
                };
                reader.onerror = function (event) {
                    displayNotify("SPAExcepcionArchivo1", "", '2');
                };

            } else {
                displayNotify("ListadoCatalogos0010", "", '2');
            }
            windowLoadFile.close();
        } else {
            windowLoadFile.close();
            displayNotify("SPAProyectoCargaMasiva", "", '1');
        }

    });

    $("#btnCerrarPopup").click(function (e) {
        $('input[value="spool"]').prop("checked", true);
        $("#inputFile").val("");
        $("#lblRutaArchivo").text("");
        windowLoadFile.close();
    });
}

function csvToJson(data, idField, tipoCarga) {
    data = data.split("\r\n");
    data.shift();

    var encabezados = [];
    encabezados[0] = "NombreSpool";
    encabezados[1] = "NumeroControl";
    encabezados[2] = "SistemaPintura";
    encabezados[3] = "Color";

    var csv = [];
    try {
        data.forEach(function (d, i) {
            if (d.substring(0, d.length).split(",").length === encabezados.length - 1) {
                var tmp = {};
                tmp[idField] = null;
                d.split(",").forEach(function (cell, z) {
                    if (tipoCarga == 1) {
                        if (z == 0) {
                            tmp[encabezados[z]] = cell;
                            tmp[encabezados[z + 1]] = "";
                        }
                        else {
                            tmp[encabezados[z + 1]] = cell;
                        }

                    }
                    else {
                        if (z == 0) {
                            tmp[encabezados[z]] = "";
                        }
                        tmp[encabezados[z + 1]] = cell;
                    }

                });
                csv.push(tmp);
            }
            else if (d.substring(0, d.length).split(";").length === encabezados.length - 1) {
                var tmp = {};
                tmp[idField] = null;
                d.split(";").forEach(function (cell, z) {
                    if (tipoCarga == 1) {
                        if (z == 0) {
                            tmp[encabezados[z]] = "";
                            tmp[encabezados[z + 1]] = cell;
                        }
                        else {
                            tmp[encabezados[z + 1]] = cell;
                        }

                    }
                    else {
                        if (z == 0) {
                            tmp[encabezados[z]] = "";
                        }
                        tmp[encabezados[z + 1]] = cell;
                    }
                });
                csv.push(tmp);
            }

            else {
                if (d.substring(0, d.length).split(",").length != 1) {
                    throw -1;
                    csv = [];
                }


            }
        });
    } catch (e) {
        if (e !== -1) {
            throw e;
        } else {
            displayNotify("SPAExcepcionArchivo", "", '2');
        }
        csv = [];
    }
    return csv;
}

function SuscribirEventoDescargarCsv() {
    $("#btnDescargaCsv, #btnDescargaCsv1").click(function (e) {
        window.location.href = "/PlantillaSistemaPinturaAplicable.csv";
    });
}

function SuscribirEventoBusqueda() {
    $("#btnBuscar").click(function (e) {
        if (!editado) {
            GenerarBusqueda();
        }
        else {
            ventanaConfirmEdicionSinTipoBusqueda.open().center();
        }
    });

    $("#inputSpool").keydown(function (e) {
        if (e.keyCode == 13) {

            if (!editado) {

                GenerarBusqueda();
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }

    });
    $("#inputNc").keydown(function (e) {
        if (e.keyCode == 13) {

            if (!editado) {

                GenerarBusqueda();
            }
            else {
                ventanaConfirmEdicionSinTipoBusqueda.open().center();
            }
        }
    });
}
function SuscribirEventoGuardar() {
    $("#Guardar, #btnGuardar, #Guardar1, #btnGuardar1").click(function (e) {
        if ($('#Guardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            if (ds._data.length) {
                AjaxGuardarCaptura(ds._data, false);
            } else {
                displayNotify("SistemaPinturaAplicableExcepcionGuardado", "", '1');
            }
        } else if ($('#Guardar').text() == _dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]) {
            opcionHabilitarView(false);
        }

    });
    $("#btnGuardarYNuevo, #btnGuardarYNuevo1").click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length) {
            AjaxGuardarCaptura(ds._data, true);
        } else {
            displayNotify("SistemaPinturaAplicableExcepcionGuardado", "", '1');
        }
    });
}

function opcionHabilitarView(disable) {
    if (disable) {
        $('#FieldSetView').find('*').attr('disabled', true);
        //$(".addedSectionInLine").find('*').attr("disabled", true);
        $("#styleSpool").attr("disabled", true);
        $("#styleNc").attr("disabled", true);
        $("#inputProyecto").data("kendoComboBox").enable(false);
        $("#inputSpool").css('opacity', '0.6');
        $("#inputSpool").attr("disabled", true);
        $("#inputNc").css('opacity', '0.6');
        $("#inputNc").attr("disabled", true);
        $("#inputSistemaPintura").data("kendoComboBox").enable(false);
        $("#inputColorPintura").data("kendoComboBox").enable(false);


        $("input[name='Planchar']").attr("disabled", true);
        $("#btnPlanchar").attr("disabled", true);

        $("#btnBuscar").attr("disabled", true);
        $("#Guardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    } else {
        $('#FieldSetView').find('*').attr('disabled', false);
        //$(".addedSectionInLine").find('*').attr("disabled", false);
        $("#styleSpool").attr("disabled", false);
        $("#styleNc").attr("disabled", false);
        $("#inputProyecto").data("kendoComboBox").enable(true);
        $("#inputSpool").css('opacity', '1');
        $("#inputSpool").attr("disabled", false);
        $("#inputNc").css('opacity', '1');
        $("#inputNc").attr("disabled", false);
        $("#inputSistemaPintura").data("kendoComboBox").enable(true);
        $("#inputColorPintura").data("kendoComboBox").enable(true);


        $("input[name='Planchar']").attr("disabled", false);
        $("#btnPlanchar").attr("disabled", false);

        $("#btnBuscar").attr("disabled", false);
        $("#Guardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#btnGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#Guardar1").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#btnGuardar1').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}

function LimpiaTipoBusqueda() {
    $("#inputSpool").val("");
    $("#inputNc").val("");
    $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputSistemaPintura").data("kendoComboBox").value("");
    $("#inputColorPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputColorPintura").data("kendoComboBox").value("");
    $("#inputProyecto").data("kendoComboBox").select(0);

    $("#grid").data("kendoGrid").dataSource.data([]);
}


function LimpiaCargaProyecto() {
    editado = false;
    $("#inputSpool").val("");
    $("#inputNc").val("");
    $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputSistemaPintura").data("kendoComboBox").value("");
    $("#inputColorPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputColorPintura").data("kendoComboBox").value("");


    $("#grid").data("kendoGrid").dataSource.data([]);
}

function LimpiaCargaSP() {
    $("#inputColorPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputColorPintura").data("kendoComboBox").value("");
}

function LimpiarPantalla() {
    $("#inputProyecto").data("kendoComboBox").dataSource.data([]);
    $("#inputProyecto").data("kendoComboBox").value("");
    $("#inputSistemaPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputSistemaPintura").data("kendoComboBox").value("");
    $("#inputColorPintura").data("kendoComboBox").dataSource.data([]);
    $("#inputColorPintura").data("kendoComboBox").value("");
    $("#inputSpool").val("");
    $("#inputNc").val("");
    $("#grid").data("kendoGrid").dataSource.data([]);
    opcionHabilitarView(false);
    //AjaxCargarCamposPredeterminados();
    AjaxCargaProyecto();
}


//Convert ced erasing "" to null
function validaVacios(c) {
    for (var key in c) {
        c[key] === "" ? c[key] = null : 0;
    }
    return c;
}