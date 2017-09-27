var CampoMuestra = 29;

function AjaxCargarCamposPredeterminados(capturaNormal) {
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
    suscribirEventoChangeRadio();
}

function AjaxObtenerProyectos() {
    loadingStart();

    $Proyectos.Proyectos.read({ token: Cookies.get("token") }).done(function (data) {
        if (Error(data)) {

            $("#inputRequisicion").data("kendoComboBox").setDataSource([]);
            $("#inputRequisicion").data("kendoComboBox").text("");
            $("#inputPrueba").data("kendoComboBox").setDataSource([]);
            $("#inputPrueba").data("kendoComboBox").text("");
            $("#inputPrueba").data("kendoComboBox").setDataSource([]);
            $("#inputProyecto").data("kendoComboBox").text("");

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


function AjaxPruebas() {
    if ($("#inputProyecto").data("kendoComboBox").text() != "") {
        loadingStart();
        $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                $("#inputPrueba").data("kendoComboBox").value("");
                $("#inputPrueba").data("kendoComboBox").dataSource.data(data);
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").setDataSource([]);

                if ($("#inputPrueba").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputPrueba").data("kendoComboBox").select(1);
                }
            }

        });
    }
};


function AjaxRequisicion() {
    if ($("#inputPrueba").data("kendoComboBox").text() != "") {
        loadingStart();
        $ServiciosTecnicosGeneral.ServiciosTecnicosGeneral.read({ token: Cookies.get("token"), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), TipoPruebaID: $("#inputPrueba").data("kendoComboBox").value(), estatusID: 2, lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#inputRequisicion").data("kendoComboBox").dataSource.data(data);

                if ($("#inputRequisicion").data("kendoComboBox").dataSource._data.length == 2) {
                    $("#inputRequisicion").data("kendoComboBox").select(1);
                    AjaxCargarElementosRequisicion();
                }
                else {
                    AjaxCargarElementosRequisicion();
                }
            }

        });
    }
};



function AjaxCargarElementosRequisicion() {

    if ($("#inputProyecto").data("kendoComboBox").value() != "") {
        loadingStart();
        $AsignarRequisicion.AsignarRequisicion.read({ lenguaje: $("#language").val(), token: Cookies.get("token"), TipoPruebaID: $("#inputPrueba").val(), ProyectoID: $("#inputProyecto").data("kendoComboBox").value(), RequisicionID: $("#inputRequisicion").val() }).done(function (data) {
            if (Error(data)) {
                $("#grid").data("kendoGrid").dataSource.data([]);
                //$("#grid").data("kendoGrid").dataSource.data(data);
                var ds = $("#grid").data("kendoGrid").dataSource;
                var array = data;
                for (var i = 0; i < array.length; i++) {
                    ds.add(array[i]);
                }
            }
            loadingStop();
        });

    }

}