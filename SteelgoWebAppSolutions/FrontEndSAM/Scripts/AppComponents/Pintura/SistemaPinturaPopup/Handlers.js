function SuscribirEventos()
{
    SuscribirEventoCerrarPopUpPruebas();
    GuardarDetallePruebas();
}



function SuscribirEventoCerrarPopUpPruebas() {
    $("#CerrarDetallePruebas").click(function (e) {
        e.preventDefault();
        window.parent.$("#windowGrid").data("kendoWindow").close();
        //$("#windowGrid").data("kendoWindow").close();
    });
}

function GuardarDetallePruebas() {
    $('#GuardarDetallePruebas').click(function () {

        var ds = $("#gridPopUp").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            //if (ds._data[i].UnidadMinima == "" || ds._data[i].UnidadMaxima == "" || ds._data[i].ProyectoProcesoPrueba == "" || ds._data[i].UnidadMedida == "") {
            //    displayNotify("SistemaPinturaMensajeCamposMandatorios", "", "1");
            //    return;
            //}
            //else if (parseInt(ds._data[i].UnidadMinima) >= parseInt(ds._data[i].UnidadMaxima)) {
            //    displayNotify("SistemaPinturaMensajeUnidadMedidaError", "", "1");
            //    return;
            //}


            //ds._data[i].SistemaPinturaProyectoProcesoID = ds._data[i].SistemaPinturaProyectoProcesoID == undefined ? 0 : ds._data[i].SistemaPinturaProyectoProcesoID;
            //ds._data[i].ProyectoProcesoPruebaID = ds._data[i].ProyectoProcesoPruebaID == undefined ? 0 : ds._data[i].ProyectoProcesoPruebaID;
            //ds._data[i].Accion = ds._data[i].ProyectoProcesoPruebaID == 0 ? 1 : 2;
        }
        //window.parent.registroCompleto.listadoPruebasDetalle = ds._data;
        //window.parent.registroCompleto.listadoPruebasDetalle = [];
        //for (var i = 0; i < ds._data.length; i++) {
        //    window.parent.registroCompleto.listadoPruebasDetalle[i] = ds._data[i];
        //}

        var listaPruebasHijo = ds._data;
        window.parent.sincronizarlistasPruebas(ds._data);
        window.parent.$("#windowGrid").data("kendoWindow").close();
        //window.parent.$("#grid").data("kendoGrid").dataSource.sync();

    });
}