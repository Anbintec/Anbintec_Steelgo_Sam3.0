var tiposCSV = ["application/csv", "application/excel", "application/lotus123", "application/msexcel", "application/vnd.lotus-1-2-3", "application/vnd.ms-excel", "application/vnd.ms-works", "application/vnd.msexcel", "application/wk1", "application/wks", "application/x-123", "application/x-dos_ms_excel", "application/x-excel", "application/x-lotus123", "application/x-ms-excel", "application/x-msexcel", "application/x-msworks", "application/x-wks", "application/x-xls", "application/xlc", "application/xls", "text/anytext", "text/comma-separated-values", "text/csv", "zz-application/zz-winassoc-wk1"];
var proyectoInicial = 0;
var pruebaInicial = 0;
var requisicionOriginal = 0;
var error = 0;
var cambiosCheckOK = 0;
var IdProyecto = 0;
var IdProyecto2 = 0;
var SpoolIDGlobal = 0;

function SuscribirEventos() {
    SuscribirEventoTipoBusqueda();
    SuscribirEventosOrdenTrabajo();
    suscribirEventoProyecto();
    SuscribirEventoID();
    SuscribirEventoBuscar();
    suscribirEventoGuardar();
    //SuscribirEventoAplicar();
    suscribirEventoCancelar();    
    suscribirEventoCarGaCSV();
    suscribirEventoDescarGaCSV();
    suscribirEventoChangeRadio();
    suscribirEventoNumeroControl();
    suscribirEventoElementosAsignados();    
    SuscribirEventoCerrarPopUpJuntas();
    suscribirEventoDesactivaCheckOK();
    SuscribirEventoCheckEnDetalle();
}

function SuscribirEventoTipoBusqueda() {
    $("input:radio[name=TipoAgregado]").change(function () {
        if (cambiosCheckOK > 0) {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                visible: false,
                animation: false,
                width: "auto",
                height: "auto",
                actions: [],
                modal: true
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirm.open().center();
            $("#yesButtonProy").click(function () {
                ventanaConfirm.close();                
                Limpiar();
                cambiosCheckOK = 0;
                if ($('input:radio[name=TipoAgregado]:checked').val() == "SpoolContiene") {
                    $("#divSpoolContiene").css("display", "block");                    
                    $("#Proyecto").data("kendoComboBox").value("");
                    $("#InputNumeroControl").val("");
                    $("#ProyectoDiv").css("display", "block");                    
                    $("#divOrdenTrabajo").css("display", "none");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    $("#grid thead [data-field=NombreSpool] .k-link").html("Spool");
                } else {
                    $("#divSpoolContiene").css("display", "none");
                    $("#ProyectoDiv").css("display", "none");
                    $("#InputOrdenTrabajo").val("");
                    $("#InputID").data("kendoComboBox").value("");
                    $("#divOrdenTrabajo").css("display", "block");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    $("#grid thead [data-field=NombreSpool] .k-link").html("Spool ID");
                }
            });

            $("#noButtonProy").click(function () {
                $("#InputNumeroControl").val(SpoolContiene);
                $("#Proyecto").data("kendoComboBox").value(ProyectoIDAnterior);
                if ($('input:radio[name=TipoAgregado]:checked').val() == "SpoolContiene") {
                    $('input:radio[name=TipoAgregado]:nth(1)').prop("checked", true);
                    $("#styleSpoolContiene").removeClass("active");
                    $("#styleNombreSpool").addClass("active");
                } else {
                    $('input:radio[name=TipoAgregado]:nth(0)').prop("checked", true);
                    $("#styleSpoolContiene").addClass("active");
                    $("#styleNombreSpool").removeClass("active");
                }
                ventanaConfirm.close();
            });
        } else {
            if ($('input:radio[name=TipoAgregado]:checked').val() == "SpoolContiene") {                
                $("#divSpoolContiene").css("display", "block");                
                $("#Proyecto").data("kendoComboBox").value("");
                $("#InputNumeroControl").val("");
                $("#ProyectoDiv").css("display", "block");                
                $("#divOrdenTrabajo").css("display", "none");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid thead [data-field=NombreSpool] .k-link").html("Spool");
            } else {
                $("#divSpoolContiene").css("display", "none");                
                $("#ProyectoDiv").css("display", "none");
                $("#InputOrdenTrabajo").val("");
                $("#InputID").data("kendoComboBox").value("");
                $("#divOrdenTrabajo").css("display", "block");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid thead [data-field=NombreSpool] .k-link").html("Spool ID");
            }
        }        
    });    
}

function suscribirEventoGuardar() {    
    $('#BotonGuardar, #BotonGuardar2, #BotonGuardar3, #BotonGuardar4').click(function (e) {        
        //$("#grid").data("kendoGrid").dataSource.sync();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($("#BotonGuardar").text() === _dictionary.lblGuardar[$("#language").val()] ) {
            AjaxGuardarCaptura(ds._data, 0);
            cambiosCheckOK = 0;
        }
        else if ($("#BotonGuardar").text() == _dictionary.botonEditar[$("#language").val()]) {
            opcionHabilitarView(false, "FieldSetView");
        }            
    });

    $('#BotonGuardarYNuevo, #BotonGuardarYNuevo1').click(function (e) {
        //$("#grid").data("kendoGrid").dataSource.sync();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#BotonGuardar').text() === _dictionary.lblGuardar[$("#language").val()] ) {
            AjaxGuardarCaptura(ds._data, 1);
            cambiosCheckOK = 0;
        }
        else if ($("#BotonGuardar").text() == _dictionary.botonEditar[$("#language").val()]) {
            opcionHabilitarView(false, "FieldSetView");
        }            
    });  
}

function suscribirEventoCancelar() {
    $('#btnCancelar').click(function (e) {
        Limpiar();
    });
}

function suscribirEventoProyecto() {
    var dataItem;
    $("#Proyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,        
        //delay: 10,        
        change: function (e) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            var proyectoID = $("#Proyecto").data("kendoComboBox").value();
            var NumControl = $("#InputNumeroControl").val();
            var elementoActual = this.dataItem(e.sender.selectedIndex);

            //if (existenCambios(ds._data)) {
            if(cambiosCheckOK > 0){
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false,
                    animation: false,
                    width: "auto",
                    height: "auto",
                    actions: [],
                    modal: true
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    ventanaConfirm.close();
                    ProyectoIDAnterior = proyectoID;
                    Limpiar();
                    cambiosCheckOK = 0;
                });

                $("#noButtonProy").click(function () {
                    $("#InputNumeroControl").val(SpoolContiene);
                    $("#Proyecto").data("kendoComboBox").value(ProyectoIDAnterior);
                    ventanaConfirm.close();
                });
            }
            else {
                if (elementoActual == undefined) {
                    $("#Proyecto").data("kendoComboBox").value("");
                }
                ProyectoIDAnterior = proyectoID;
                Limpiar();
            }                        
        }
    });
}

function SuscribirEventoBuscar() {
    $('#ButtonBuscar').click(function (e) {        
        var ds = $("#grid").data("kendoGrid").dataSource;
        var proyectoID = $("#Proyecto").data("kendoComboBox").value();
        var NombreSpool = $("#InputNumeroControl").val();                

        if ($("input:radio[name=TipoAgregado]:checked").val() == "SpoolContiene") { //NOMBRE SPOOL
            if (proyectoID != 0 && proyectoID != undefined) {
                if (NombreSpool != "" && NombreSpool != undefined) {
                    if (NombreSpool.length >= 3) {
                        if (cambiosCheckOK == 0) {
                            AjaxGetNumeroElementosPorNombre(proyectoID, NombreSpool);
                        } else {
                            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                                iframe: true,
                                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                                visible: false,
                                animation: false,
                                width: "auto",
                                height: "auto",
                                actions: [],
                                modal: true
                            }).data("kendoWindow");

                            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                            ventanaConfirm.open().center();
                            $("#yesButtonProy").click(function () {
                                ventanaConfirm.close();
                                AjaxGetNumeroElementosPorNombre(proyectoID, NombreSpool);
                                cambiosCheckOK = 0;
                            });

                            $("#noButtonProy").click(function () {
                                $("#InputNumeroControl").val(SpoolContiene);
                                ventanaConfirm.close();
                            });
                        }
                    } else {
                        displayNotify("", $("#language").val() == "es-MX" ? "Ingrese mínimo 3 caracteres" : "Minimum entry 3 characters", "1");
                    }
                } else {
                    displayNotify("SPAMensajeIngresaSpool", "", "1");
                }
            } else {
                displayNotify("MensajeSeleccionaProyecto", "", "1");
            }
        } else { //SPOOL ID
            var OrdenTrabajo = $("#InputOrdenTrabajo").val();
            var InputID = $("#InputID").data("kendoComboBox").text();

            if (OrdenTrabajo != "") {
                if (InputID != "") {
                    if (cambiosCheckOK == 0) {
                        //Obtener Datos Por Orden de Trabajo
                        AjaxObtenerElementosPorOrdenTrabajo(IdProyecto, (OrdenTrabajo+'-'+InputID));
                    } else {
                        var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                            iframe: true,
                            title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                            visible: false,
                            animation: false,
                            width: "auto",
                            height: "auto",
                            actions: [],
                            modal: true
                        }).data("kendoWindow");

                        ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                            "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                        ventanaConfirm.open().center();
                        $("#yesButtonProy").click(function () {
                            ventanaConfirm.close();
                            //AjaxObtenerElementosPorOrdenTrabajo(IdProyecto, (OrdenTrabajo + '-' + InputID));
                            cambiosCheckOK = 0;
                        });

                        $("#noButtonProy").click(function () {
                            $("#InputNumeroControl").val(SpoolContiene);
                            ventanaConfirm.close();
                        });
                    }
                } else {
                    displayNotify("PinturaCargaSeleccionaSpool", "", "1");
                }
            } else {
                displayNotify("notificationslabel0059", "", "1");
            }
        }
    });    
}

function SuscribirEventoAplicar() {
    $('#ButtonPlanchar').click(function (e) {        
        var Check = $("#InputSeleccionTodos")[0].checked;
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds.data().length > 0) {
            aplicarPlanchado(ds._data, Check);
            ds.sync();
        } else {
            displayNotify("PinturaCargaBackLogMensajeSeleccionaSpool", "", "1");            
        }
    });
};

function Limpiar() {    
    $("#InputNumeroControl").val("");
    $("#Proyecto").val();
    $("#grid").data('kendoGrid').dataSource.data([]);
}

function opcionHabilitarView(valor, name) {
    if (valor) {        
        $('#AgregadoDiv, #containerDiv').find('*').attr('disabled', true);
        $("#Proyecto").data("kendoComboBox").enable(false);        
        $("#InputNumeroControl").addClass("k-state-disabled");        
        $("#DetalleAvisoLlegada0017").text("Editar");
        $("#BotonGuardar, #BotonGuardar2, #BotonGuardar3, #BotonGuardar4").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);        
        $(".formNav").find("#Acciones").css("display", "none");
    }
    else {        
        $('#AgregadoDiv, #containerDiv').find('*').attr('disabled', false);
        $("#Proyecto").data("kendoComboBox").enable(true);
        $("#InputNumeroControl").removeClass("k-state-disabled");                
        $("#DetalleAvisoLlegada0017").text("Guardar");
        $("#BotonGuardar, #BotonGuardar2, #BotonGuardar3, #BotonGuardar4").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);        
        $(".formNav").find("#Acciones").css("display", "block");
    }
}

function suscribirEventoDescarGaCSV() {
    $("#btnDescargaCsv, #btnDescargaCsv1").click(function (e) {
        window.location.href = "/TemplateOKFAB.csv";        
    });
}

function suscribirEventoCarGaCSV() {
    $('#btnCargaCsv, #btnCargaCsv1').click(function (e) {
        if ($('#BotonGuardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {
            var proyectoID = $("#Proyecto").data("kendoComboBox").value();
            if (proyectoID != 0 && proyectoID != undefined && proyectoID != "") {
                if (cambiosCheckOK == 0) {
                    $("#files").val("");
                    $("#files").click();
                } else {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false,
                        animation: false,
                        width: "auto",
                        height: "auto",
                        actions: [],
                        modal: true
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {
                        ventanaConfirm.close();
                        $("#grid").data("kendoGrid").dataSource.data([]);
                        $("#files").val("");
                        $("#files").click();
                        cambiosCheckOK = 0;
                    });
                    $("#noButtonProy").click(function () {                        
                        ventanaConfirm.close();
                    });
                }
            }
            else {
                displayNotify("SPAProyectoCargaMasiva", "", '1');
            }
        }
    });

    document.getElementById("files").addEventListener("change", function (evt) {
        if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
            displayNotify("ListadoCatalogos0007", "", '2');
        } else {
            var data = [];
            var file = evt.target.files[0];
            try {
                if (tiposCSV.indexOf(file.type.toLowerCase()) == -1) {
                    this.value = null;
                    displayNotify("ListadoCatalogos0008", "", '2');
                } else {
                    var reader = new FileReader();
                    reader.readAsText(file);
                    reader.onload = function (event) {
                        var csvData = event.target.result;

                        csvToJson(csvData, "").forEach(function (c) {
                            data.push(c);
                        });

                        if (error == 0)                            
                            //OBTENEMOS UN ARRAY CON UN ORDEN DE EJECUCION 
                            var newData = ObtenerNewData(data);
                            if (newData != undefined) {
                                if (newData.length > 0) {
                                    AjaxGuardadoMasivo(newData)
                                    cambiosCheckOK = 0;
                                } else {
                                    displayNotify("EditarRequisicionExcepcionGuardado", "", "1");
                                }
                            } else {
                                displayNotify("notificationslabel0084", "", "2");
                            }
                                
                        error = 0;
                    };
                    reader.onerror = function () {
                        alert('Unable to read ' + file.fileName);
                    };
                }
            } catch (e) { }
        }
    });
}


function csvToJson(data, field) {       
    data = data.split("\n");
    data.shift();
    //data.pop();
    data = data.join("\n");
    data = data.split("\r").join("");
    var encabezados = Object.keys($("#gridMasivo").data("kendoGrid").options.dataSource.schema.model.fields);
    var csv = [];
    try {
        data.split("\n").forEach(function (d, i) {        
            if (d.substring(0, d.length).split(",").length === encabezados.length) {
                var tmp = {};
                d.split(",").forEach(function (cell, z) {
                    //Verificamos celdas vacias
                    if (cell !== "") {
                        tmp[encabezados[z]] = cell;
                    } else {                                                                                                
                        //si la celda esta vacía le agrego un espacio o un caracter
                        tmp[encabezados[z]] = ' ';
                    }
                });
                csv.push(tmp);
            } else {                
                if (d.substring(0, d.length).split(",").length != 1) {
                    throw -1;
                    csv = [];
                    displayNotify("ListadoCatalogos0012", "", "2");
                }
            }
        })
    } catch (e) {
        if (e !== -1) {
            error = 1;
            throw e;
        } else {            
            displayNotify("ErrorColumnaTieneRegistroVacio", "", "2");
            error = 1;
        }
        csv = [];
    }
    return csv;
}

function suscribirEventoElementosAsignados() {
    $(document).on('click', '.EnlaceDetalleJunta', function (e) {
        e.preventDefault();
        if ($('#BotonGuardar').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid"),
            dataItem = grid.dataItem($(e.target).closest("tr"));            
            AjaxObtenerJuntas(dataItem.SpoolID);
            SpoolIDGlobal = dataItem.SpoolID;            
            MostrarEstatusCheck(dataItem.SpoolID);
        }
    });
}

function MostrarEstatusCheck(SpoolID) {
    var grid = $("#grid").data("kendoGrid").dataSource._data;
    var objCheckFabricacion = $("#checkOkFabricacion");
    for (var i = 0; i < grid.length; i++) {
        if (grid[i].SpoolID == SpoolID)
            objCheckFabricacion.prop("checked", grid[i].OK);                        
    }
}

function SuscribirEventoCheckEnDetalle() {
    $("#checkOkFabricacion").change(function (e) {
        e.preventDefault();
        var valor = this.checked;
        var grid = $("#grid").data("kendoGrid").dataSource._data;
        for (var i = 0; i < grid.length; i++) {
            if (grid[i].SpoolID == SpoolIDGlobal) {
                grid[i].OK = valor;
                grid[i].ModificadoPorUsuario = true;
                cambiosCheckOK++;
                $("#grid").data("kendoGrid").dataSource.sync();
            }            
        }
        
    });
}
function suscribirEventoDesactivaCheckOK() {
    $("#grid").on('click', '.ob-paid', function (e) {
        if ($('#BotonGuardar').text() != _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            cambiosCheckOK = 0;
            e.preventDefault();
        } else {
            cambiosCheckOK++;            
        }
    });
}
function SuscribirEventoCerrarPopUpJuntas() {
    $("#CerrarDetalleJunta").click(function (e) {
        e.preventDefault();
        $("#windowGrid").data("kendoWindow").close();        
    });
}

function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        if (cambiosCheckOK > 0) {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                visible: false,
                animation: false,
                width: "auto",
                height: "auto",
                actions: [],
                modal: true
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

            ventanaConfirm.open().center();
            $("#yesButtonProy").click(function () {
                ventanaConfirm.close();                
                $("#grid").data("kendoGrid").dataSource.data([]);
                cambiosCheckOK = 0;
            });

            $("#noButtonProy").click(function () {
                $("#InputNumeroControl").val(SpoolContiene);
                $('input[name=Muestra]:nth(1)').prop('checked', true);                
                ventanaConfirm.close();
            });
        } else {
            $("#grid").data("kendoGrid").dataSource.data([]);
        }        
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        if (cambiosCheckOK > 0) {
            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                visible: false,
                animation: false,
                width: "auto",
                height: "auto",
                actions: [],
                modal: true
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

            ventanaConfirm.open().center();
            $("#yesButtonProy").click(function () {
                ventanaConfirm.close();                
                $("#grid").data("kendoGrid").dataSource.data([]);
                cambiosCheckOK = 0;
            });

            $("#noButtonProy").click(function () {
                $("#InputNumeroControl").val(SpoolContiene);
                $('input[name=Muestra]:nth(0)').prop('checked', true);                
                ventanaConfirm.close();
            });
        } else {
            $("#grid").data("kendoGrid").dataSource.data([]);
        }        
    });
}
function suscribirEventoNumeroControl() {
    $('#InputNumeroControl').keydown(function (e) {
        if (e.keyCode == 13) {
            var ds = $("#grid").data("kendoGrid").dataSource;
            var proyectoID = $("#Proyecto").data("kendoComboBox").value();
            var NombreSpool = $("#InputNumeroControl").val();

            if ($("input:radio[name=TipoAgregado]:checked").val() == "SpoolContiene") { //NOMBRE SPOOL
                if (proyectoID != 0 && proyectoID != undefined) {
                    if (NombreSpool != "" && NombreSpool != undefined) {
                        if (NombreSpool.length >= 3) {
                            if (cambiosCheckOK == 0) {
                                AjaxGetNumeroElementosPorNombre(proyectoID, NombreSpool);
                            } else {
                                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                                    iframe: true,
                                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                                    visible: false,
                                    animation: false,
                                    width: "auto",
                                    height: "auto",
                                    actions: [],
                                    modal: true
                                }).data("kendoWindow");

                                ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                                ventanaConfirm.open().center();
                                $("#yesButtonProy").click(function () {
                                    ventanaConfirm.close();
                                    AjaxGetNumeroElementosPorNombre(proyectoID, NombreSpool);
                                    cambiosCheckOK = 0;
                                });

                                $("#noButtonProy").click(function () {
                                    $("#InputNumeroControl").val(SpoolContiene);
                                    ventanaConfirm.close();
                                });
                            }
                        } else {
                            displayNotify("", $("#language").val() == "es-MX" ? "Ingrese mínimo 3 caracteres" : "Minimum entry 3 characters", "1");
                        }
                    } else {
                        displayNotify("SPAMensajeIngresaSpool", "", "1");
                    }
                } else {
                    displayNotify("MensajeSeleccionaProyecto", "", "1");
                }
            } else { //SPOOL ID
                var OrdenTrabajo = $("#InputOrdenTrabajo").val();
                var InputID = $("#InputID").data("kendoComboBox").text();

                if (OrdenTrabajo != "") {
                    if (InputID != "") {
                        if (cambiosCheckOK == 0) {
                            //Obtener Datos Por Orden de Trabajo
                            AjaxObtenerElementosPorOrdenTrabajo(IdProyecto, (OrdenTrabajo + '-' + InputID));
                        } else {
                            var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                                iframe: true,
                                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                                visible: false,
                                animation: false,
                                width: "auto",
                                height: "auto",
                                actions: [],
                                modal: true
                            }).data("kendoWindow");

                            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajeDatosCapturadosNoGuardados[$("#language").data("kendoDropDownList").value()] +
                                "</br><center><button class='btn btn-blue' id='yesButtonProy'>Si</button><button class='btn btn-blue' id='noButtonProy'>No</button></center>");

                            ventanaConfirm.open().center();
                            $("#yesButtonProy").click(function () {
                                ventanaConfirm.close();
                                AjaxObtenerElementosPorOrdenTrabajo(IdProyecto, (OrdenTrabajo + '-' + InputID));
                                cambiosCheckOK = 0;
                            });

                            $("#noButtonProy").click(function () {
                                $("#InputNumeroControl").val(SpoolContiene);
                                ventanaConfirm.close();
                            });
                        }
                    } else {
                        displayNotify("PinturaCargaSeleccionaSpool", "", "1");
                    }
                } else {
                    displayNotify("notificationslabel0059", "", "1");
                }
            }
        }
    });
}

function ObtenerNewData(data) {    
    try {
        var tmpData = [];
        elementos = [];
        contador = {};
        var tmpNumControl = {};        
        if (data.length > 0) {
            for (n in data) {
                tmpData[n] = {
                    NumeroControl: "",                    
                    OK: 0,
                    OE: 0
                };
                if (data[n].OKFAB.toString().trim() === "") { data[n].OKFAB = 0 };

                if (!isNaN(parseInt(data[n].OKFAB.toString().trim()))) {
                    if (parseInt(data[n].OKFAB.toString().trim()) === 0 || parseInt(data[n].OKFAB.toString().trim()) === 1) {
                        tmpNumControl[n] = data[n].NumeroControl.toString().toUpperCase().trim();
                        
                        if (!(tmpNumControl[n] in contador))
                            contador[tmpNumControl[n]] = 0;
                        contador[tmpNumControl[n]] += 1;

                        tmpData[n].NumeroControl = tmpNumControl[n].toString().toUpperCase().trim();
                        tmpData[n].OK = parseInt(data[n].OKFAB.toString().trim());
                        tmpData[n].OE = parseInt(contador[tmpNumControl[n]]);
                        //console.log("\t\t\t" + tmpData[n].NumeroControl + "\t\t\t" + tmpData[n].OK + "\t\t\t" + tmpData[n].OE);
                    } else {
                        displayNotify("ErrorColumnaNoEsNumero", "", "2");
                        return;
                    }
                } else {
                    displayNotify("ErrorColumnaTieneLetras", "", "2");
                    return;
                }
                
            }            
        } else {
            //no hay datos
            tmpData = [];            
        }
    } catch (e) {
        if (e !== -1) {
            error = 1;
            throw e;
        } else {            
            displayNotify("ErrorColumnaTieneRegistroVacio", "", "2");
            error = 1;
        }
    }
    loadingStop();
    return tmpData;
}

function SuscribirEventoID() {
    var dataItem;
    $("#InputID").kendoComboBox({
        dataTextField: "IDValido",
        dataValueField: "Valor",
        suggest: true,
        delay: 10,
        filter: "endswith",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                if ($("#InputID").val().length == 1) {
                    $("#InputID").data("kendoComboBox").value(("00" + $("#InputID").val()).slice(-3));                    
                }
                if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
                    Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
                    $("#LabelProyecto").text(dataItem.Proyecto);
                    //if ($('input:radio[name=TipoAgregado]:checked').val() != "Reporte") {
                    //    AjaxJunta($("#InputID").val());
                    //}                    
                }
            }
        }
    });
}

function SuscribirEventosOrdenTrabajo() {
    $("#InputOrdenTrabajo").blur(function (e) {
        if ($("#InputOrdenTrabajo").val().match("^[a-zA-Z][0-9]*$")) {
            try {
                var OrdenTrabajoOrigianl = $("#InputOrdenTrabajo").val();
                $Soldadura.Soldadura.read({ ordenTrabajo: $("#InputOrdenTrabajo").val(), tipo: '1', token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
                    if (Error(data)) {
                        if (data.OrdenTrabajo != "") {
                            $("#InputOrdenTrabajo").val(data.OrdenTrabajo);
                        }
                        else {
                            $("#InputOrdenTrabajo").val(OrdenTrabajoOrigianl);
                            displayNotify("CapturaArmadoMensajeOrdenTrabajoNoEncontrada", "", '1');
                        }
                        $("#InputID").data("kendoComboBox").dataSource.data(data.idStatus);
                        Cookies.set("LetraProyecto", data.OrdenTrabajo.substring(0, 1), { path: '/' });
                        if (data.idStatus.length > 1) {
                            IdProyecto = data.idStatus[1].ProyectoID;
                            IdProyecto2 = data.idStatus[1].ProyectoID;
                        } else {
                            IdProyecto = 0;
                            IdProyecto2 = 0;
                        }                        
                    }
                });
            } catch (e) {
                displayNotify("Mensajes_error", e.message, '1');
            }
        } else {
            displayNotify("CapturaArmadoMensajeOrdenTrabajo", "", '1');
        }
    });
    $("#InputOrdenTrabajo").focus(function (e) {
        $("#InputID").data("kendoComboBox").setDataSource([]);
        $("#InputOrdenTrabajo").val("");
        $('#InputID').data("kendoComboBox").text("");        
    });
}