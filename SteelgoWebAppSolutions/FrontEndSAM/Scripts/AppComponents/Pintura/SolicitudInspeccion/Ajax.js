var NumerosControl = { Detalle: "" };
var listaNumControl = [];

function AjaxObtenerInfoBySpool(numControl) {
    try {
        loadingStart();
        $SolicitudInspeccion.SolicitudInspeccion.read({ NumeroControl: numControl, token: Cookies.get("token") }).done(function (data) {
            if (Error(data)) {
                var ds = $("#grid").data("kendoGrid").dataSource;
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        if (!SpoolRepetido(data[i].SpoolID)) {
                            ds.insert(0, data[i]);
                        } else {
                            displayNotify('EmbarqueCargaMsjErrorSpoolAgregarExiste', '', '1');
                        }
                    }
                } else {
                    displayNotify("NODATA", "", "1");
                }
                ds.sync();
            }
        });
        loadingStop();
    } catch (e) {
        displayNotify("", "Error Obteniendo Info de Spool: " + e.message, "2");
        loadingStop();
    }
}

function AjaxObtenerDatosGrid(opc) {
    loadingStart();
    try {
        $SolicitudInspeccion.SolicitudInspeccion.read({ token: Cookies.get("token"), Opc: opc, Relleno: true }).done(function (data) {
            if (Error(data)) {
                var dataSource = $("#grid").data("kendoGrid").dataSource;
                dataSource.data([]);
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        dataSource.add(data[i]);
                    }
                } else {
                    displayNotify("", $("#language").val() == "es-MX" ? "No Hay Datos Por Mostrar" : "No Data For Display", "1");
                }
                dataSource.sync();
            }
            loadingStop();
        });
    } catch (e) {
        displayNotify("", "Error Cargando Datos Grid: " + e.message, "2");
        loadingStop();
    }
}

function AjaxGuardarCaptura(data, esNuevo) {
    try {
        loadingStart();
        var Captura = { Detalle: "" };
        var listaDetalle = [];
        for (var i = 0; i < data.length; i++) {
            listaDetalle[i] = {
                Accion: 0,
                RequisicionInspeccionID: 0,
                SpoolID: 0,
                NumeroControl: "",
                SistemaPintura: "",
                Color: "",
                FechaSolicitud: "",
                FechaInspeccion: "",
                Resultado: "",
                ModificadoPorUsuario: false
            }

            listaDetalle[i].Accion = data[i].Accion;
            listaDetalle[i].RequisicionInspeccionID = data[i].RequisicionInspeccionID;
            listaDetalle[i].SpoolID = data[i].SpoolID;
            listaDetalle[i].NumeroControl = data[i].NumeroControl;
            listaDetalle[i].SistemaPintura = data[i].SistemaPintura;
            listaDetalle[i].Color = data[i].Color;
            listaDetalle[i].FechaSolicitud = data[i].FechaSolicitud;
            listaDetalle[i].FechaInspeccion = (data[i].FechaInspeccion == "" || data[i].FechaInspeccion == null) ? null : ConvertirDateTime(data[i].FechaInspeccion);
            listaDetalle[i].Resultado = data[i].Resultado;
            listaDetalle[i].ModificadoPorUsuario = data[i].ModificadoPorUsuario;
            
            listaNumControl[i] = {
                NumeroControl: ""
            }
            listaNumControl[i].NumeroControl = data[i].NumeroControl; //Lleno Objetos para traer estos mismos datos al grid
        }

        Captura.Detalle = listaDetalle;
        $SolicitudInspeccion.SolicitudInspeccion.create(Captura, { Lenguaje: "es-MX", token: Cookies.get("token") }).done(function (data) {
            if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "OK") {
                displayNotify("MensajeGuardadoExistoso", "", "0");
                if (esNuevo) {
                    $("#grid").data("kendoGrid").dataSource.data([]);                    
                    ActualizarVista(false);
                } else {
                    ActualizarVista(true);
                }
                //OBTENGO LOS DATOS QUE SE GUARDARON
                NumerosControl.Detalle = listaNumControl;
                AjaxObtenerSpoolsGuardados(NumerosControl);
                listaNumControl = [];
                NumerosControl.Detalle = {};
            } else {
                displayNotify("MensajeGuardadoErroneo", "", "2")
            }
        });                  
    } catch (e) {
        displayNotify("", "Error Guardando Captura: " + e.message, "2");
        loadingStop();
    }
    loadingStop();
}

function AjaxObtenerSpoolsGuardados(ListaSpools) {
    try {
        $RequisicionInspeccion.RequisicionInspeccion.create(ListaSpools, { token: Cookies.get("token"), Param: true, OtroParam: true }).done(function (data) {
            if (Error(data)) {
                {
                    var dataSource = $("#grid").data("kendoGrid").dataSource;                    
                    dataSource.data([]);
                    if (data.length > 0) {                                                
                        for (var i = 0; i < data.length; i++) {
                            dataSource.add(data[i]);
                        }                        
                    }
                    dataSource.sync();
                }
            }
        });
    } catch (e) {
        displayNotify("", "Error Obteniendo Spools Guardados: " + e.message, "2");
    }
}

function ConvertirDateTime(value) {
    var fecha = new Date(value);
    var hoy = new Date();
    var horas = hoy.getHours();
    var HH = horas < 10 ? '0' + horas : horas;
    var minutos = hoy.getMinutes();
    var mm = (minutos < 10) ? '0' + minutos : minutos;
    var segundos = hoy.getSeconds();
    var ss = (segundos < 10) ? '0' + segundos : segundos;
    return fecha.getDate() + "/" + (fecha.getMonth() + 1) + "/" + fecha.getFullYear() + " " + HH + ":" + mm + ":" + ss;
}

function SpoolRepetido(SpoolID) {
    var result = false;
    var dataGrid = $("#grid").data("kendoGrid").dataSource._data;
    for (var i = 0; i < dataGrid.length; i++) {
        if (dataGrid[i].SpoolID === SpoolID) {
            return true;
        }
    }
    return result;
}



function ActualizarVista(valor) {
    if (valor) {
        $("#AgregadoDiv").find('*').attr('disabled', true);
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").dataSource.data([]);
        $("#InputID").data("kendoComboBox").value("");
        $("#InputID").data("kendoComboBox").enable(false);
        $("#btnGuardarSup, #btnGuardar2Sup, #btnGuardarInf, #btnGuardar2Inf").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
    }
    else {
        $('#AgregadoDiv').find('*').attr('disabled', false);
        $("#InputID").data("kendoComboBox").enable(true);
        $("#InputID").data("kendoComboBox").dataSource.data([]);
        $("#InputOrdenTrabajo").val("");
        $("#InputID").data("kendoComboBox").value("");
        $("#btnGuardarSup, #btnGuardarSup2, #btnGuardarInf, #btnGuardarInf2").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}
