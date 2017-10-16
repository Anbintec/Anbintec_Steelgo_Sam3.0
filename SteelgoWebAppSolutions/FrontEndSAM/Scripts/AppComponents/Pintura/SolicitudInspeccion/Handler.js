var IdProyecto = 0;
function SuscribirEventos() {
    SuscribirEventoGuardar();
    SuscribirEventoID();
    SuscribirEventosOrdenTrabajo();
    SuscribirEventoAgregar();
    SuscribirEventoEnterSpoolID();
    SuscribirCampoFechaInspeccion();
    SuscribirEventoComboResultado();
    SuscribirEventoPlancharCampos();
}

function SuscribirEventoID() {
    var dataItem;
    $("#InputID").kendoComboBox({
        dataTextField: "ID",
        dataValueField: "SpoolID",
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
                $SolicitudInspeccion.SolicitudInspeccion.read({ token: Cookies.get("token"), OrdenTrabajo: $("#InputOrdenTrabajo").val(), Lenguaje: $("#language").val() }).done(function (data) {
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

function SuscribirEventoGuardar() {
    $("#btnGuardarSup, #btnGuardar2Sup, #btnGuardarInf, #btnGuardar2Inf").click(function (e) {
        if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource.data();
            if (ds.length > 0) {
                if (HayCamposVacios()) {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: $("#language").val() == "es-MX" ? "Aviso!" : "Warning!",
                        visible: false,
                        animation: false,
                        width: "auto",
                        height: "auto",
                        actions: [],
                        modal: true
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {
                        ventanaConfirm.close();
                        AjaxGuardarCaptura(ds, false);
                        //AjaxLlenarGridDespuesGuardado();
                    });
                    $("#noButtonProy").click(function () {
                        ventanaConfirm.close();
                    });
                } else {
                    AjaxGuardarCaptura(ds, false);
                    //AjaxLlenarGridDespuesGuardado();
                }
            } else {
                displayNotify("", "No Hay Datos Por Guardar", "1");
            }
        } else {
            if ($("#btnGuardarSup").text() == _dictionary.botonEditar[$("#language").val()]) {
                ActualizarVista(false);
                listaNumControl = [];
                NumerosControl.Detalle = {};
            }
        }
    });

    $("#btnGuardarNuevoSup, #btnGuardarNuevoInf").click(function () {
        if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var ds = $("#grid").data("kendoGrid").dataSource.data();
            if (ds.length > 0) {
                if (HayCamposVacios()) {
                    var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                        iframe: true,
                        title: $("#language").val() == "es-MX" ? "Aviso!" : "Warning!",
                        visible: false,
                        animation: false,
                        width: "auto",
                        height: "auto",
                        actions: [],
                        modal: true
                    }).data("kendoWindow");

                    ventanaConfirm.content(_dictionary.EntregaPlacasGraficasMensajePreguntaGuardado[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirm.open().center();
                    $("#yesButtonProy").click(function () {
                        ventanaConfirm.close();
                        AjaxGuardarCaptura(ds, true);
                    });
                    $("#noButtonProy").click(function () {
                        ventanaConfirm.close();
                    });
                } else {
                    AjaxGuardarCaptura(ds, true);
                }
            } else {
                displayNotify("", "No Hay Datos Por Guardar", "1");
            }
        }
    });
}

function SuscribirEventoAgregar() {
    $('#btnAgregar').click(function (e) {
        var OrdenTrabajo = $("#InputOrdenTrabajo").val();
        var InputID = $("#InputID").data("kendoComboBox").text();
        if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            if (OrdenTrabajo != "" && InputID != "") {
                AjaxObtenerInfoBySpool((OrdenTrabajo + "-" + InputID).toString());
                $("#InputID").data("kendoComboBox").value("");
            } else {
                displayNotify("", "Por Favor Ingrese Spool ID", "1");
            }
        }
    });
}

function SuscribirEventoEnterSpoolID() {
    $('#InputID').closest('.k-widget').keydown(function (e) {
        if (e.keyCode === 13) {
            var OrdenTrabajo = $("#InputOrdenTrabajo").val();
            var InputID = $("#InputID").data("kendoComboBox").text();
            if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
                if (OrdenTrabajo != "" && InputID != "") {
                    AjaxObtenerInfoBySpool((OrdenTrabajo + "-" + InputID).toString());
                    $("#InputID").data("kendoComboBox").value("");
                } else {
                    displayNotify("", "Por Favor Ingrese Spool ID", "1");
                }
            }
        }
    });
}



function SuscribirCampoFechaInspeccion() {
    $("#txtFechaInspeccion").kendoDatePicker({
        format: "dd/MM/yyyy",
        change: function (e) {
            if (ValidarFecha(this.value()))
                $("#txtFechaInspeccion").data("kendoDatePicker").value(this.value());
            else
                $("#txtFechaInspeccion").data("kendoDatePicker").value("");
        }
    });
}

function SuscribirEventoComboResultado() {
    $("#cmbResultado").kendoComboBox({        
        autoBind: false,
        dataTextField: "Descripcion",
        dataValueField: "Descripcion",
        dataSource:
            [
                { Descripcion: "" },
                { Descripcion: "Aprobado" },
                { Descripcion: "Rechazado" }
            ],            
        change: function (e) {
            e.preventDefault();
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                $("#cmbResultado").data("kendoComboBox").value(dataItem.Descripcion);                
            } else {
                $("#cmbResultado").data("kendoComboBox").value("");
            }
        }
    });
}

function SuscribirEventoPlancharCampos() {
    $("#btnPlanchar").click(function (e) {
        if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {                                    
            if ($('input:radio[name=Planchar]')[1].checked) {
                var ventanaConfirm = $("#ventanaConfirmCaptura").kendoWindow({
                    iframe: true,
                    title: $("#language").val() == "es-MX" ? "Aviso!" : "Warning!",
                    visible: false,
                    animation: false,
                    width: "auto",
                    height: "auto",
                    actions: [],
                    modal: true
                }).data("kendoWindow");

                ventanaConfirm.content(_dictionary.MensajeAdvertenciaPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonProy'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirm.open().center();
                $("#yesButtonProy").click(function () {
                    ventanaConfirm.close();
                    aplicarPlanchado(true);
                });
                $("#noButtonProy").click(function () {
                    ventanaConfirm.close();
                });
            } else {
                aplicarPlanchado(false);
            }
        }
    });
}


function aplicarPlanchado(todos) {
    var dataSource = $("#grid").data("kendoGrid").dataSource;
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    var FechaInspeccion = $("#txtFechaInspeccion").val();
    var Resultado = $("#cmbResultado").val();
    if (data.length > 0) {
        //SOLO FECHA DE INSPECCION
        if (FechaInspeccion !== "" && (Resultado == "" || Resultado == null)) {
            if (todos) {
                for (var index = 0; index < data.length; index++) {
                    data[index].FechaInspeccion = FechaInspeccion;
                    data[index].ModificadoPorUsuario = true
                }
            } else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].FechaInspeccion === "" || data[i].FechaInspeccion === null) {
                        data[i].FechaInspeccion = FechaInspeccion;
                        data[i].ModificadoPorUsuario = true;
                    }
                }
            }
        } else if ((FechaInspeccion == "" || FechaInspeccion == null) && Resultado != "") { //SOLO RESULTADO
            if (todos) {
                for (var j = 0; j < data.length; j++) {
                    data[j].Resultado = Resultado;
                    data[j].ModificadoPorUsuario = true;
                }
            } else {
                for (var j = 0; j < data.length; j++) {
                    if (data[j].Resultado == "" || data[j].Resultado == null) {
                        data[j].Resultado = Resultado;
                        data[j].ModificadoPorUsuario = true;
                    }
                }
            }
        } else if (FechaInspeccion != "" && Resultado != "") { //AMBOS
            if (todos) {
                for (var a = 0; a < data.length; a++) {
                    data[a].FechaInspeccion = FechaInspeccion;
                    data[a].Resultado = Resultado;
                    data[a].ModificadoPorUsuario = true;
                }
            } else {
                for (var a = 0; a < data.length; a++) {
                    if ((data[a].FechaInspeccion == "" || data[a].FechaInspeccion == null) && (data[a].Resultado == "" || data[a].Resultado == null)) {
                        data[a].FechaInspeccion = FechaInspeccion;
                        data[a].Resultado = Resultado;
                        data[a].ModificadoPorUsuario = true;
                    }
                }
            }
        } else { //VACIOS
            displayNotify("msgPinturaSolicitudInspNoHayCamposAPlanchar", "", "1");
        }
        $("#grid").data("kendoGrid").dataSource.sync();
    } else {
        displayNotify("msgPinturaSolicitudInspNoHayDatosGrid", "", "1");
    }
}

function EsSpoolRepetido(Spool) {
    var result = false;
    var dataGrid = $("#grid").data("kendoGrid").dataSource._data;
    for (var i = 0; i < dataGrid.length; i++) {
        if (dataGrid[i].Spool === Spool) {
            return true;
        }
    }
    return result;
}

function HayCamposVacios() {
    var result = false;
    try {
        var grid = $("#grid").data("kendoGrid");
        var ds = grid.dataSource.view();
        if (ds.length > 0) {
            for (var i = 0; i < ds.length; i++) {
                var currentUid = ds[i].uid;
                var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                if (ds[i].FechaInspeccion == "" || ds[i].Resultado == "") {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("k-alt");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("kRowError");
                    result = true;
                } else {
                    grid.table.find("tr[data-uid='" + currentUid + "']").removeClass("kRowError");
                    grid.table.find("tr[data-uid='" + currentUid + "']").addClass("k-alt");
                }
            }
        }
    } catch (e) {
        displayNotify("", "Error Verificando Campos Vacios: " + e.message, "2");
    }
    return result;
}