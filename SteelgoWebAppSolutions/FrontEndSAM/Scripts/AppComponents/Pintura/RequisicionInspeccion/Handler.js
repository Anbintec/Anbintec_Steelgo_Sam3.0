var IdProyecto = 0;
function SuscribirEventos() {    
    SuscribirEventoGuardar();
    SuscribirEventoID();    
    SuscribirEventosOrdenTrabajo();    
    SuscribirEventoAgregar();
    SuscribirEventoEnterSpoolID();
    SuscribirCampoFechaSolicitud();
    SuscribirEventoPlanchar();
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
                $RequisicionInspeccion.RequisicionInspeccion.read({ token: Cookies.get("token"), OrdenTrabajo: $("#InputOrdenTrabajo").val(), Lenguaje: $("#language").val() }).done(function (data) {
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
                        AjaxGuardarCaptura(ds, false, false);                        
                    });
                    $("#noButtonProy").click(function () {                   
                        ventanaConfirm.close();
                    });
                } else {
                    AjaxGuardarCaptura(ds, false, false);                    
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
                        AjaxGuardarCaptura(ds, true, false);                        
                    });
                    $("#noButtonProy").click(function () {
                        ventanaConfirm.close();
                    });
                } else {
                    AjaxGuardarCaptura(ds, true, false);
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

function SuscribirCampoFechaSolicitud() {    
    $("#txtFechaSolicitudPlanchado").kendoDatePicker({
        format: "dd/MM/yyyy",        
        change: function (e) {            
            if (ValidarFecha(this.value()))
                $("#txtFechaSolicitudPlanchado").data("kendoDatePicker").value(this.value());
            else
                $("#txtFechaSolicitudPlanchado").data("kendoDatePicker").value("");
        }
    });
}
function SuscribirEventoPlanchar() {
    $("#btnPlanchar").click(function (e) {
        if ($('#btnGuardarSup').text() == _dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]) {
            var FechaSolicitud = $("#txtFechaSolicitudPlanchado").data("kendoDatePicker").value();            
            var ds = $("#grid").data("kendoGrid").dataSource;
            var Check = $('input:radio[name=Planchar]')[1].checked; //todos
            if(Check){            
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
                    aplicarPlanchado(ds, FechaSolicitud, true);
                });
                $("#noButtonProy").click(function () {
                    ventanaConfirm.close();
                });
            } else {
                aplicarPlanchado(ds, FechaSolicitud, false);
            }
        }
    });
}


function aplicarPlanchado(dataSource, value, todos) {    
    var filters = dataSource.filter();
    var allData = dataSource.data();
    var query = new kendo.data.Query(allData);
    var data = query.filter(filters).data;
    if (data.length > 0) {
        if (value == "" || value == null) {
            displayNotify("msgPinturaSolicitudInspNoHayCamposAPlanchar", "", "1");
        } else {
            if (todos) {
                for (index = 0; index < data.length; index++) {
                    data[index].FechaSolicitud = value;
                    data[index].ModificadoPorUsuario = true
                }
            } else {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].FechaSolicitud === "" || data[i].FechaSolicitud === null) {
                        data[i].FechaSolicitud = value;
                        data[i].ModificadoPorUsuario = true;
                    }
                }
            }
        }        
    } else {
        displayNotify("msgPinturaSolicitudInspNoHayDatosGrid", "", "1");
    }
    
    $("#grid").data("kendoGrid").dataSource.sync();
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