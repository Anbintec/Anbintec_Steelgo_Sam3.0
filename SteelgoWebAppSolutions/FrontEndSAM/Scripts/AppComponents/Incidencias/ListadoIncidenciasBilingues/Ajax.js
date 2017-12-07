function AjaxCargarProyecto() {
    loadingStart();
    $ListadoIncidencias.ListadoIncidencias.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#ProyectoID").data("kendoComboBox").dataSource.data([]);
            var proyectoId = 0;

            if (data.length > 0) {
                $("#ProyectoID").data("kendoComboBox").dataSource.data(data);
                if (data.length < 3) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ProyectoID != 0) {
                            proyectoId = data[i].ProyectoID;
                        }
                    }
                }

                $("#ProyectoID").data("kendoComboBox").value(proyectoId);
                $("#ProyectoID").data("kendoComboBox").trigger("change");
            }
        }
        loadingStop();
    });
}

function AjaxCargarPeriodos() {
    loadingStart();
    $Periodo.Periodo.read({ token: Cookies.get("token"), Lenguaje: $("#language").val() }).done(function (data) {
        $("#PeriodoTiempo").data("kendoComboBox").dataSource.data([]);

        if (data.length > 0) {
            $("#PeriodoTiempo").data("kendoComboBox").dataSource.data(data);

            $("#PeriodoTiempo").data("kendoComboBox").value(0);
            $("#PeriodoTiempo").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}

function AjaxCargarRangoFechas(dataItem) {
    loadingStart();
    $Periodo.Periodo.read({
        token: Cookies.get("token"), Lenguaje: $("#language").val(), Minuendo: dataItem.Minuendo,
        Sustraendo: dataItem.Sustraendo, FechaFinal: $("#FechaFinal").val()
    }).done(function (data) {
        if (data != undefined) {
            $("#FechaInicial").val(data.FechaInicio);
            $("#FechaFinal").val(data.FechaFin);

        }
        loadingStop();
    });
}

function AjaxCargarListado() {
    loadingStart();

    $ListadoIncidencias.ListadoIncidencias.read({
        token: Cookies.get("token"), lenguaje: $("#language").val(), ProyectoID: $("#ProyectoID").data("kendoComboBox").value(),
        FechaInicial: $("#FechaInicial").val(), FechaFinal: $("#FechaFinal").val(), EstatusID: $("#EtapaID").data("kendoComboBox").value(), Mostrar: $('input:radio[name=Muestra]:checked').val()
    }).done(function (data) {

        $("#grid").data("kendoGrid").dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;
        var array = data;
        for (var i = 0; i < array.length; i++) {
            ds.add(array[i]);
        }
        loadingStart();
    });

};




function AjaxGuardarCaptura(arregloCaptura, tipoGuardar) {
    try {
        $("#grid").data("kendoGrid").dataSource.sync();
        var pruebas = false;
        Captura = [];
        Captura[0] = { listaDetalle: "" };
        ListaDetalles = [];
        var i = 0;

        for (index = 0; index < arregloCaptura.length; index++) {
            if (arregloCaptura[index].Modificado == 1) {
                ListaDetalles[i] = {
                    IncidenciaID: "",
                    Titulo: "", Descripcion: "", Respuesta: "", DetalleResolucion: "", MotivoCancelacion: "",
                    TituloIngles: "", DescripcionIngles: "", RespuestaIngles: "", DetalleResolucionIngles: "", MotivoCancelacionIngles: ""
                };

                ListaDetalles[i].IncidenciaID = arregloCaptura[index].FolioIncidenciaID;

                ListaDetalles[i].Titulo = arregloCaptura[index].Titulo;
                ListaDetalles[i].Descripcion = arregloCaptura[index].Descripcion;
                ListaDetalles[i].Respuesta = arregloCaptura[index].Respuesta;
                ListaDetalles[i].DetalleResolucion = arregloCaptura[index].DetalleResolucion;
                ListaDetalles[i].MotivoCancelacion = arregloCaptura[index].MotivoCancelacion;

                ListaDetalles[i].TituloIngles = arregloCaptura[index].TituloIngles;
                ListaDetalles[i].DescripcionIngles = arregloCaptura[index].DescripcionIngles;
                ListaDetalles[i].RespuestaIngles = arregloCaptura[index].RespuestaIngles;
                ListaDetalles[i].DetalleResolucionIngles = arregloCaptura[index].DetalleResolucionIngles;
                ListaDetalles[i].MotivoCancelacionIngles = arregloCaptura[index].MotivoCancelacionIngles;


                i++;
            }

        }


        Captura[0].listaDetalle = ListaDetalles;


        if (Captura[0].listaDetalle.length > 0) {
            loadingStart();
            $ListadoIncidencias.ListadoIncidencias.create(Captura[0], { token: Cookies.get("token")}).done(function (data) {
                editado = true;
                if (Error(data)) {
                    if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
                        if (tipoGuardar == 1) {
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            
                            opcionHabilitarView(false, "FieldSetView");
                        }
                        else {
                            $("#grid").data("kendoGrid").dataSource.data([]);
                            AjaxCargarListado();
                            opcionHabilitarView(true, "FieldSetView");

                        }
                        displayNotify("MensajeGuardadoExistoso", "", "0");
                        editado = false;
                    }
                    else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
                        displayNotify("MensajeGuardadoErroneo", "", '2');
                    }
                }
            });
            loadingStop();
        }
        else {
            loadingStop();
            displayNotify("MensajeAdverteciaExcepcionGuardado", "", '1');
        }



    } catch (e) {
        loadingStop();
        displayNotify("", e.message, '1');

    }

};
