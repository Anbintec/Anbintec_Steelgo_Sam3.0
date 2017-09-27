function AjaxCargarHeaderDashboard() {    
    //$Dashboard.Dashboard.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), modulo: 1}).done(function (data) {
    $Dashboard.Dashboard.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), modulo: 1, ProyectoID: ($("#inputProyecto").data("kendoComboBox").value() == "" || $("#inputProyecto").data("kendoComboBox").value() == undefined) ? 0 : $("#inputProyecto").data("kendoComboBox").value()  }).done(function (data) {
        if (data.length > 0) {
            $("#tabEstatus").html("");
            var tab = '';
            var option = '';
            for (var i = 0; i < data.length; i++) {
                option = option + '<button '
                    + 'onclick="ActivarRefrescarGrid(' + data[i].Estatus_DashboardID + ');" '
                    +'id="' + data[i].Estatus_DashboardID 
                    + '" class="btn btn-tab btn-Requisicion">'
                    + '<label>' 
                    + data[i].Descripcion
                    + '</label><span id="span'+i+ 
                    + data[i].Estatus_DashboardID
                    + '" class="porElemento" >' + data[i].NumeroElementos + '</span>'
                    +'<span id="span' +i+i
                    + data[i].Estatus_DashboardID
                    + '" class="porRequisicion" style="display: none;" >' + data[i].NumeroRequisiciones + '</span>'
                    +'</button>';
                
            }
            $("#tabEstatus").append(option);
            AgregarStatusDinamicos(data);
            //AjaxObtenerProyectos();
        }

    });
}


function AjaxCargarPeriodos() {
    loadingStart();
    $Periodo.Periodo.read({ token: Cookies.get("token"), Lenguaje: $("#language").val() }).done(function (data) {
        $("#InputPeriodo").data("kendoComboBox").dataSource.data([]);

        if (data.length > 0) {
            $("#InputPeriodo").data("kendoComboBox").dataSource.data(data);

            $("#InputPeriodo").data("kendoComboBox").value(0);
            $("#InputPeriodo").data("kendoComboBox").trigger("change");
        }
        loadingStop();
    });
}


function AjaxCargarRangoFechas(dataItem) {
    loadingStart();
    $Periodo.Periodo.read({
        token: Cookies.get("token"), Lenguaje: $("#language").val(), Minuendo: dataItem.Minuendo,
        Sustraendo: dataItem.Sustraendo, FechaFinal: $("#inputFechaFin").val()
    }).done(function (data) {
        if (data != undefined) {
            $("#inputFechaInicio").val(data.FechaInicio);
            $("#inputFechaFin").val(data.FechaFin);
        }
        loadingStop();
    });
}


function AjaxObtenerProyectos() {
    loadingStart();

    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
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


    });
}


function AjaxPruebas() {
    if ($("#inputProyecto").val() != "") {
        loadingStart();
        $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                $("#inputTipoPrueba").data("kendoComboBox").value("");
                $("#inputTipoPrueba").data("kendoComboBox").dataSource.data(data);

                if ($("#inputTipoPrueba").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputTipoPrueba").data("kendoComboBox").select(1);
                    AjaxObtenerProveedor();
                }
                else {
                    
                }

            }

        });
    }
};

function AjaxObtenerProveedor() {
    if ($("#inputTipoPrueba").data("kendoComboBox").value() != "") {
        loadingStart();
        var patioID = $('#inputProyecto').data("kendoComboBox").dataSource._data[$('#inputProyecto').data("kendoComboBox").selectedIndex].PatioID;
        $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), PatioID: patioID, TipoPruebaID: $("#inputTipoPrueba").data("kendoComboBox").value() }).done(function (data) {
            if (Error(data)) {
                $("#inputProveedor").data("kendoComboBox").value("");
                $("#inputProveedor").data("kendoComboBox").dataSource.data(data);

                if ($("#inputProveedor").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputProveedor").data("kendoComboBox").select(1);
                    $("#inputProveedor").data("kendoComboBox").trigger("change");
                    
                }
                else {

                }
            }
            loadingStop();
        });
    }
}


function AjaxAccionesListado(estatusID) {
    loadingStart();
    if ($("#inputProyecto").data("kendoComboBox").value() != "") {
        var pruebaID = $('#inputTipoPrueba').data("kendoComboBox").text() == "" ? 0 : $('#inputTipoPrueba').data("kendoComboBox").value();
        var proveedorID = $('#inputProveedor').data("kendoComboBox").text() == "" ? 0 : $('#inputProveedor').data("kendoComboBox").value();

        $Dashboard.Dashboard.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), EstatusID: estatusID, TipoPruebaID: pruebaID, ProveedorID: proveedorID, FechaInicial: $("#inputFechaInicio").val(), FechaFinal: $("#inputFechaFin").val() }).done(function (data) {
            if (Error(data)) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = data;
                for (var i = 0; i < array.length; i++) {
                    array[i].Fecha = new Date(ObtenerDato(array[i].Fecha, 1), ObtenerDato(array[i].Fecha, 2), ObtenerDato(array[i].Fecha, 3));//año, mes, dia
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