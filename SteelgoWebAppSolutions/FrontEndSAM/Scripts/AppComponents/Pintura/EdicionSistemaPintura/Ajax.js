function AjaxCargaDetalleSistemaPintura() {
    $ListadoSistemaPintura.ListadoSistemaPintura.read({ token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
        $("#grid").data("kendoGrid").dataSource.data([]);

        var ds = $("#grid").data("kendoGrid").dataSource;
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                ds.add(data[i]);
            }
        }
    });
}

function AjaxEliminaSistemaPintura(sistemaPinturaID,proyectoID) {
    $ListadoSistemaPintura.ListadoSistemaPintura.read({ token: Cookies.get("token"), SistemaPinturaID: sistemaPinturaID, ProyectoID: proyectoID }).done(function (data) {
        if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] == "Ok") {
            setTimeout(function () { AjaxCargaDetalleSistemaPintura(); }, 1100);
            displayNotify("SistemaPinturaEliminadoExitoso", "", '0');
        }
        else if (data.ReturnMessage.length > 0 && data.ReturnMessage[0] != "Ok") {
            displayNotify("SistemaPinturaErrorEliminado", "", '2');
        }
    });
}

function AjaxObtieneDetallePruebas(sistemaPinturaProyectoProcesoID) {
    $ListadoSistemaPintura.ListadoSistemaPintura.read({ token: Cookies.get("token"), SistemaPinturaProyectoProcesoID: sistemaPinturaProyectoProcesoID, lenguaje: $("#language").val() }).done(function (data) {
       
        var ds = $("#gridPopUp").data("kendoGrid").dataSource;
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                ds.add(data[i]);
            }
        }
        windowDetailTest = $("#windowDetailTest").kendoWindow({
            iframe: true,
            modal: true,
            title: "Detalle de Pruebas",
            resizable: false,
            visible: true,
            width: "50%",
            height: "auto",
            draggable: false,
            animation: {
                close: false,
                open: false
            },
            actions: [
                "Close"
            ],
            close: function onClose(e) {
                var gridDataSource = $("#gridPopUp").data("kendoGrid").dataSource;
                gridDataSource.data([]);
            }
        }).data("kendoWindow");


        windowDetailTest.open().center();
    });
}