var folioGenerado;
var CampoMuestra = 3056;

function AjaxCargarDatos() {
    
    $ImpresionPruebas.ImpresionPruebas.read({ token: Cookies.get("token"), mostrar: $('input:radio[name=Muestra]:checked').val(), RequisicionID: $("#inputRequisicion").data("kendoComboBox").text() == "" ? 0 : $("#inputRequisicion").val(), ProveedorID: $("#inputProveedor").val() }).done(function (data) {
        $("#grid").data('kendoGrid').dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;
        var array = data;
        for (var i = 0; i < array.length; i++) {
            ds.add(array[i]);
        }
    });
};

function AjaxImprimir(arregloJuntas) {
    loadingStart();
    Captura = [];
    ReportePath = "";
    NombreReporte = '';
    impresionCorrecta = true;
    var encontrado = false;

    for (var i = 0; i < arregloJuntas.length; i++) {
        if (arregloJuntas[i].Seleccionado && encontrado== false) {
            if (arregloJuntas[i].Reporte == "" ) {
                impresionCorrecta = false;
                break;
            }
            else {
                encontrado = true;
                ReportePath = arregloJuntas[i].Url;
                NombreReporte = arregloJuntas[i].Reporte;
            }
            
        }
        if (encontrado)
            break;
    }



    if (NombreReporte != "") {
        if (impresionCorrecta) {

            SolicitarImpresion(NombreReporte.replace('***', NombreReporte));
            loadingStop();
        }
        else {
            displayNotify("", "Hay elementos seleccionados sin asignar en un reporte", "1");
            loadingStop();
        }
    }
    else {
        displayNotify("", "No hay elementos seleccionados para imprimir", "1");
        loadingStop();
    }
}

function SolicitarImpresion(nombreReporte) {
   // window.open(url, "_blank");


    document.location.target = "#";
    document.location.href = '/Reportes/ObtenerReportes?path=/Steelgo/Reportes/' + nombreReporte;
};

function AjaxGenerarReporte(arregloJuntas) {
    loadingStart();
    Captura = [];
    Captura[0] = { Detalles: "" };
    ListaDetalles = [];
    contador = 0;
    ReporteIDConsecutivo;
    reporteReqID = 0;

    for (var i = 0; i < arregloJuntas.length; i++) {
        if (arregloJuntas[i].Seleccionado && arregloJuntas[i].ReporteRequisicionID != 0) {
            reporteReqID = arregloJuntas[i].ReporteRequisicionID;
        }
    }

        for (var i = 0; i < arregloJuntas.length; i++) {
            if (arregloJuntas[i].Seleccionado ) {
                ListaDetalles[contador] = { ReporteRequisicionID: "", ElementoPorClasificacionPNDID: "", Accion: "" };
                ListaDetalles[contador].Accion =  arregloJuntas[i].ReporteRequisicionID == 0 ? 1 : 2;
                ListaDetalles[contador].ElementoPorClasificacionPNDID = arregloJuntas[i].ElementoPorClasificacionPNDID;
                ListaDetalles[contador].ReporteRequisicionID = reporteReqID;
                contador++;
            }
        }

        Captura[0].Detalles = ListaDetalles;
        if (Captura[0].Detalles.length > 0) {

            var modalTitle = "";
            modalTitle = "Reporte";
            var ventanaConfirm = $("#ventanaConfirm");
            var window = ventanaConfirm.kendoWindow({
                modal: true,
                title: modalTitle,
                resizable: false,
                visible: true,
                width: "50%",
                minWidth: 30,
                position: {
                    top: "1%",
                    left: "1%"
                },
                animation: false

            }).data("kendoWindow");

            window.content('<div  z-index: inherit">' +
                                '<div class="col-sm-11 col-md-11 col-lg-11">' +
                                    '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                        '<label id=""><span>' + "Reporte" + '</span></label>' +
                                        '<input id="NombreReporte" class="form-control" />' +
                                    '</div>' +
                                    '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                        '<label id=""><span>' + "Fecha" + '</span></label>' +
                                        '<input id="FechaReporte" class="form-control" value="25/11/2015">  </input>' +
                                    '</div>' +
                                    '<div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-12">' +
                                        '<center><button class="btn btn-blue" id="YesButton"> Guardar</button>&nbsp;<button class="btn btn-blue" id="NoButton"> Cancelar</button></center>' +
                                    '</div>' +
                                '</div>' +
                            '</div>');

            ventanaConfirm.data("kendoWindow").title(modalTitle);
            ventanaConfirm.data("kendoWindow").center().open();

            

$("#FechaReporte").kendoDatePicker({
    max: new Date(),
    //format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()],
    change: function (e) {
        ValidarFecha(e.sender._value)
    }
});

             $("#YesButton").click(function (handler) {
                            Captura[0].NombreReporte = $("#NombreReporte").val();
                            Captura[0].FechaReporte = $("#FechaReporte").val();

                            $ImpresionPruebas.ImpresionPruebas.read({ token: Cookies.get("token"), nombre: $("#NombreReporte").val() }).done(function (data) {
                                if (data == "no") {
                                    $ImpresionPruebas.ImpresionPruebas.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val(), RequisicionID: $("#inputRequisicion").val() }).done(function (data) {
                                        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                                            AjaxCargarDatos();
                                            mensaje = "Se guardo correctamente la informacion" + "-0";
                                            displayNotify("MensajeGuardadoExistoso", "", '0');
                                        }
                                        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                                            mensaje = "No se guardo la informacion el error es: " + data.ReturnMessage[0] + "-2"
                                            displayNotify("MensajeGuardadoErroneo", data.ReturnMessage[0], '2');
                                        }
                                        loadingStop();


                                    });
                                }
                                else {
                                    displayNotify("", "Nombre de reporte repetido", '2');

                                }

                            });
                           

                    window.close();
                        });
             $("#NoButton").click(function (handler) {
                 window.close();
                 loadingStop();
             });

           
        }
        else {
            loadingStop();
        }
   
}

function SolicitarImpresion(nombreReporte) {
    // window.open(url, "_blank");

    //window.open('/Reportes/ObtenerReportes?path=/Steelgo/Reportes/' + nombreReporte);
    window.open('../../../FormatoImpresionResultados.pdf');
    //document.location.target = "_blank";
    //document.location.href = '/Reportes/ObtenerReportes?path=/Steelgo/Reportes/' + nombreReporte;

};
function AjaxObtenerProyectos() {
    loadingStart();

    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputProyecto").data("kendoComboBox").value("");
        $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

        if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputProyecto").data("kendoComboBox").select(1);
            $("#inputProyecto").data("kendoComboBox").trigger("change");
        }
        else {
            $("#inputProyecto").data("kendoComboBox").select(0);
            loadingStop();
        }


    });
}

function AjaxPruebas() {
    if ($("#inputProyecto").data("kendoComboBox").text() != "") {
        loadingStart();
        $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                $("#inputPrueba").data("kendoComboBox").value("");
                $("#inputPrueba").data("kendoComboBox").dataSource.data(data);

                if ($("#inputPrueba").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputPrueba").data("kendoComboBox").select(1);
                }

            }
            
        });
    }
};

function AjaxObtenerProveedor() {
    if ($("#inputPrueba").data("kendoComboBox").value() != "") {
        loadingStart();
        var patioID = $('#inputProyecto').data("kendoComboBox").dataSource._data[$('#inputProyecto').data("kendoComboBox").selectedIndex].PatioID;
        $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), PatioID: patioID, TipoPruebaID: $("#inputPrueba").data("kendoComboBox").value() }).done(function (data) {
            if (Error(data)) {
                $("#inputProveedor").data("kendoComboBox").value("");
                $("#inputProveedor").data("kendoComboBox").dataSource.data(data);

                if ($("#inputProveedor").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputProveedor").data("kendoComboBox").select(1);
                    $("#inputProveedor").data("kendoComboBox").trigger("change");
                    var RequiereEquipo = $('#inputPrueba').data("kendoComboBox").dataSource._data[$('#inputPrueba').data("kendoComboBox").selectedIndex].RequiereEquipo;
                    if (!RequiereEquipo) {
                        

                    }
                    else {
                        

                    }
                }
                else {

                }
            }
            loadingStop();
        });
    }
}

function AjaxRequisicion() {

    if ($("#inputPrueba").data("kendoComboBox").text() != "") {
        loadingStart();
        $ImpresionPruebas.ImpresionPruebas.read({ token: Cookies.get("token"), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), TipoPruebaID: $("#inputPrueba").data("kendoComboBox").value(), ProveedorID: $("#inputProveedor").data("kendoComboBox").value() }).done(function (data) {
            if (Error(data)) {
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").dataSource.data(data);

                //if ($("#inputRequisicion").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputRequisicion").data("kendoComboBox").select(0);
                    $("#inputRequisicion").data("kendoComboBox").trigger("change");
                //}
            }

        });
    }
};

function AjaxCargarCamposPredeterminados() {
    loadingStart();
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: CampoMuestra }).done(function (data) {
        if (Error(data)) {
            if (data == "sin captura") {
                $('input:radio[name=Muestra]:nth(0)').trigger("click");

                $("#styleSinCaptura").addClass("active");
                $("#styleTodos").removeClass("active");
            }
            else if (data == "Todos") {

                $('input:radio[name=Muestra]:nth(1)').trigger("click");
                $("#styleTodos").addClass("active");
                $("#styleSinCaptura").removeClass("active");
            }

        }
        loadingStop();
    });
}