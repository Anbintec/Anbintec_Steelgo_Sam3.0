


function AjaxCargarPeriodos() {
    loadingStart();
    $Periodo.Periodo.read({ token: Cookies.get("token"), Lenguaje: $("#language").val() }).done(function (data) {
        if (Error(data)) {
            $("#InputPeriodo").data("kendoComboBox").dataSource.data([]);

            if (data.length > 0) {
                $("#InputPeriodo").data("kendoComboBox").dataSource.data(data);

                $("#InputPeriodo").data("kendoComboBox").value(0);
                $("#InputPeriodo").data("kendoComboBox").trigger("change");
            }
        }
        loadingStop();
    });
}


function AjaxCargarRangoFechas(dataItem) {
    loadingStart();
    $Periodo.Periodo.read({
        token: Cookies.get("token"), Lenguaje: $("#language").val(), Minuendo: dataItem.Minuendo,
        Sustraendo: dataItem.Sustraendo, FechaFinal: $("#inputFechaFinal").val()
    }).done(function (data) {
        if (Error(data)) {
            if (data != undefined) {

                $("#inputFechaInicial").val(data.FechaInicio);
                $("#inputFechaFinal").val(data.FechaFin);
            }
        }
        loadingStop();
    });
}


function AjaxObtenerProyectos() {
    loadingStart();

    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {
            $("#inputProyecto").data("kendoComboBox").value("");
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);

            if ($("#inputProyecto").data("kendoComboBox").dataSource._data.length == 2) {
                $("#inputProyecto").data("kendoComboBox").select(1);
                AjaxPruebas();
            }
            else {
                $("#inputProyecto").data("kendoComboBox").select(0);
                loadingStop();
            }
        }


    });
}

function AjaxGetClientes() {
    loadingStart();
    $CreacionOC.CreacionOC.read({ token: Cookies.get("token"), OrdenCompraID: 0 }).done(function (data) {
        if (Error(data)) {
            $("#inputCliente").data("kendoComboBox").setDataSource([]);
            $("#inputCliente").data("kendoComboBox").text("");
            if (data.length == 1) {
                $("#inputCliente").data("kendoComboBox").dataSource.data(data);
                $("#inputCliente").data("kendoComboBox").value(data.ClienteID);
                $("#inputCliente").data("kendoComboBox").select(1);
            } else {
                $("#inputCliente").data("kendoComboBox").dataSource.data(data);
            }
            
        }
        loadingStop();
    });

}




function AjaxCargarHeaderDashboard() {
    
    var proyectoID = ($("#inputProyecto").data("kendoComboBox").value() == "" || $("#inputProyecto").data("kendoComboBox").value() == undefined) ? 0 : $("#inputProyecto").data("kendoComboBox").value();
    var clienteID = ($("#inputCliente").data("kendoComboBox").value() == "" || $("#inputCliente").data("kendoComboBox").value() == undefined) ? 0 : $("#inputCliente").data("kendoComboBox").value();

    $DashboardPintura.DashboardPintura.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), 
        ProyectoID: proyectoID, ClienteID: clienteID, FechaInicial: $("#inputFechaInicial").val(), FechaFinal: $("#inputFechaFinal").val()
    }).done(function (data) {
        if (data.length > 0) {
            //$("#tabEstatus").html("");
            //var tab = '';
            //var option = '';
            


            $("#SPorCargar").text(data[0].Spools1);
            $("#SPorCapturar").text(data[0].Spools2);
            $("#SPorProbar").text(data[0].Spools3);
            $("#SPorDescargar").text(data[0].Spools12);
            $("#PPorCapturar").text(data[0].Spools4);
            $("#PPorProbar").text(data[0].Spools5);
            $("#PPorDescargar").text(data[0].Spools6);
            $("#IPorCapturar").text(data[0].Spools7);
            $("#IPorProbar").text(data[0].Spools8);
            $("#APorCapturar").text(data[0].Spools9);
            $("#APorProbar").text(data[0].Spools10);
            $("#OkPintura").text(data[0].Spools11);

            $("#SPorCargar4").text(data[0].Spools1);
            $("#SPorCapturar4").text(data[0].Spools2);
            $("#SPorProbar4").text(data[0].Spools3);
            $("#SPorDescargar4").text(data[0].Spools12);
            $("#PPorCapturar4").text(data[0].Spools4);
            $("#PPorProbar4").text(data[0].Spools5);
            $("#PPorDescargar4").text(data[0].Spools6);
            $("#IPorCapturar4").text(data[0].Spools7);
            $("#IPorProbar4").text(data[0].Spools8);
            $("#APorCapturar4").text(data[0].Spools9);
            $("#APorProbar4").text(data[0].Spools10);
            $("#OkPintura4").text(data[0].Spools11);



            $("#SPorCargar2").text(data[0].M21);
            $("#SPorCapturar2").text(data[0].M22);
            $("#SPorProbar2").text(data[0].M23);
            $("#SPorDescargar2").text(data[0].M212);
            $("#PPorCapturar2").text(data[0].M24);
            $("#PPorProbar2").text(data[0].M25);
            $("#PPorDescargar2").text(data[0].M26);
            $("#IPorCapturar2").text(data[0].M27);
            $("#IPorProbar2").text(data[0].M28);
            $("#APorCapturar2").text(data[0].M29);
            $("#APorProbar2").text(data[0].M210);
            $("#OkPintura2").text(data[0].M211);

            $("#SPorCargar5").text(data[0].M21);
            $("#SPorCapturar5").text(data[0].M22);
            $("#SPorProbar5").text(data[0].M23);
            $("#SPorDescargar5").text(data[0].M212);
            $("#PPorCapturar5").text(data[0].M24);
            $("#PPorProbar5").text(data[0].M25);
            $("#PPorDescargar5").text(data[0].M26);
            $("#IPorCapturar5").text(data[0].M27);
            $("#IPorProbar5").text(data[0].M28);
            $("#APorCapturar5").text(data[0].M29);
            $("#APorProbar5").text(data[0].M210);
            $("#OkPintura5").text(data[0].M211);


            $("#SPorCargar3").text(data[0].Toneladas1);
            $("#SPorCapturar3").text(data[0].Toneladas2);
            $("#SPorProbar3").text(data[0].Toneladas3);
            $("#SPorDescargar3").text(data[0].Toneladas12);
            $("#PPorCapturar3").text(data[0].Toneladas4);
            $("#PPorProbar3").text(data[0].Toneladas5);
            $("#PPorDescargar3").text(data[0].Toneladas6);
            $("#IPorCapturar3").text(data[0].Toneladas7);
            $("#IPorProbar3").text(data[0].Toneladas8);
            $("#APorCapturar3").text(data[0].Toneladas9);
            $("#APorProbar3").text(data[0].Toneladas10);
            $("#OkPintura3").text(data[0].Toneladas11);

            $("#SPorCargar6").text(data[0].Toneladas1);
            $("#SPorCapturar6").text(data[0].Toneladas2);
            $("#SPorProbar6").text(data[0].Toneladas3);
            $("#SPorDescargar6").text(data[0].Toneladas12);
            $("#PPorCapturar6").text(data[0].Toneladas4);
            $("#PPorProbar6").text(data[0].Toneladas5);
            $("#PPorDescargar6").text(data[0].Toneladas6);
            $("#IPorCapturar6").text(data[0].Toneladas7);
            $("#IPorProbar6").text(data[0].Toneladas8);
            $("#APorCapturar6").text(data[0].Toneladas9);
            $("#APorProbar6").text(data[0].Toneladas10);
            $("#OkPintura6").text(data[0].Toneladas11);



            mostrarFiltro();
            OcultarCampos(false);


            //option = option + '<button '
            //    + 'onclick="ActivarRefrescarGrid(' + data[i].Estatus_DashboardID + ');" '
            //    + 'id="' + data[i].Estatus_DashboardID
            //    + '" class="btn btn-tab btn-Requisicion">'
            //    + '<label>'
            //    + data[i].Descripcion
            //    + '</label><span id="span' + i +
            //    +data[i].Estatus_DashboardID
            //    + '" class="porElemento" >' + data[i].NumeroElementos + '</span>'
            //    + '<span id="span' + i + i
            //    + data[i].Estatus_DashboardID
            //    + '" class="porRequisicion" style="display: none;" >' + data[i].NumeroToneladas + '</span>'
            //    + '<span id="span' + i + i
            //    + data[i].Estatus_DashboardID
            //    + '" class="porRequisicion" style="display: none;" >' + data[i].NumeroM2 + '</span>'
            //    + '</button>';
            //$("#tabEstatus").append(option);
            //AgregarStatusDinamicos(data);
            //AjaxObtenerProyectos();
        }

    });
}


function AjaxAccionesListado(estatusID) {
    loadingStart();
    if ($("#inputProyecto").data("kendoComboBox").value() != "") {

        var proyectoID = $('#inputProyecto').data("kendoComboBox").text() == "" ? 0 : $('#inputProyecto').data("kendoComboBox").value();
        var clienteID = $('#inputCliente').data("kendoComboBox").text() == "" ? 0 : $('#inputCliente').data("kendoComboBox").value();

        $DashboardPintura.DashboardPintura.read({
            token: Cookies.get("token"), lenguaje: $("#language").val(), ProyectoID: proyectoID, EstatusID: estatusID, 
            ClienteID: clienteID, FechaInicial: $("#inputFechaInicial").val(), FechaFinal: $("#inputFechaFinal").val()
        }).done(function (data) {
            if (Error(data)) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = data;
                for (var i = 0; i < array.length; i++) {
                    //array[i].Fecha = new Date(ObtenerDato(array[i].Fecha, 1), ObtenerDato(array[i].Fecha, 2), ObtenerDato(array[i].Fecha, 3));//año, mes, dia
                    ds.add(array[i]);
                }
            }
            loadingStop();
        });
    }
    else
        loadingStop();
}


function ObtenerDato(fecha, tipoDatoObtener) {
    var cultura = $("#language").val();

    switch (tipoDatoObtener) {
        case 1://anho
            return fecha.split('/')[2]
            break;
        case 2://mes
            if (cultura = 'es-MX')
                return fecha.split('/')[1]
            else
                return fecha.split('/')[0]
            break;
        case 3://dia
            if (cultura = 'es-MX')
                return fecha.split('/')[0]
            else
                return fecha.split('/')[1]
            break;
    }
}