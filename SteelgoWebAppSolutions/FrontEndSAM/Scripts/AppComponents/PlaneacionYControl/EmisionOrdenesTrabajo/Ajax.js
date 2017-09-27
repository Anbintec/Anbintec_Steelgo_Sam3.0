function AjaxObtenerProyectos() {
    loadingStart();
    $EmisionOT.EmisionOT.read({ token: Cookies.get("token"), idCatalogo: 1 }).done(function (data) {
        $("#inputProyecto").data("kendoComboBox").value("");

        if (data.length > 0) {
            $("#inputProyecto").data("kendoComboBox").dataSource.data(data);
        }


    });
}

function AjaxMostrarSpoolsDeProyecto() {
    loadingStart();
    $EmisionOT.EmisionOT.read({ token: Cookies.get("token"), idProyecto: $("#inputProyecto").data("kendoComboBox").value(), idPatio: $("#inputPatio").data("kendoComboBox").value() }).done(function (data) {
        $("#grid").data('kendoGrid').dataSource.data([]);
        var ds = $("#grid").data("kendoGrid").dataSource;

        for (var i = 0; i < data.length; i++) {
            ds.add(data[i]);
        }
        loadingStop();
    });
}

function AjaxObtenerTalleresPorPatio() {
    $(".taller-proyecciones-encabezado").remove();
    $(".taller-capacidad-contenedor").remove();
    loadingStart();
    $EmisionOT.EmisionOT.read({ token: Cookies.get("token"), talleres: "taller", idPatio: $("#inputPatio").data("kendoComboBox").value() }).done(function (data) {
        CrearContenedorProyecciones(data);
        CrearContenedorCapacidad(data);
        CrearArregloTalleres(data);

        $("#inputTalleresWindow").data("kendoComboBox").value("");
        $("#inputTalleresWindow").data("kendoComboBox").dataSource.data(data);

        //  loadingStop();
    });
}