

function SuscribirEventos() {
    suscribirEventoChangeRadio();
    suscribirEventoGenerarReporte();
    suscribirEventoImprimir();
    SuscribirEventoComboProyecto();
    SuscribirEventoComboPrueba();
    SuscribirEventoComboProveedor();
    SuscribirEventoComboRequisicion();
    SuscribirEventoPlanchado();
};

function SuscribirEventoPlanchado() {



    $("#ButtonPlanchar").click(function () {
        if ($("#grid").data('kendoGrid').dataSource._data.lenght > 0) {
            ventanaConfirm = $("#ventanaConfirm").kendoWindow({
                iframe: true,
                title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                visible: false,
                width: "auto",
                height: "auto",
                modal: true,
                animation: false
            }).data("kendoWindow");

            ventanaConfirm.content(_dictionary.EntregaPlacasGraficasPlancharTodos[$("#language").data("kendoDropDownList").value()] +
                         "</br><center><button class='confirm_yes btn btn-blue' id='yesButton'>" + _dictionary.EntregaPlacasGraficasbotonSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='confirm_yes btn btn-blue' id='noButton'>" + _dictionary.EntregaPlacasGraficasbotonNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            ventanaConfirm.open().center();

            $("#yesButton").click(function (handler) {
                var elementoPlanchado = $("#seleccionarTodos").is(':checked');
                seleccionarTodo(elementoPlanchado);
                ventanaConfirm.close();
            });
            $("#noButton").click(function (handler) {
                ventanaConfirm.close();
            });
        }
        else {
            displayNotify("", "No hay elementos por seleccionar", "1")
        }
    });

}


function suscribirEventoGenerarReporte() {
    $('#Guardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
            // opcionHabilitarView(true, "FieldSetView");
            AjaxGenerarReporte(ds._data);
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
            //opcionHabilitarView(true, "FieldSetView");
            AjaxGenerarReporte(ds._data);
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxGenerarReporte(ds._data);
        //Limpiar();
    });


    $('#GuardarPie').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
            // opcionHabilitarView(true, "FieldSetView");
            AjaxGenerarReporte(ds._data);
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardar1').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
            //opcionHabilitarView(true, "FieldSetView");
            AjaxGenerarReporte(ds._data);
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo1').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxGenerarReporte(ds._data);
        //Limpiar();
    });

}

function suscribirEventoImprimir() {
    $('.Imprimir').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        AjaxImprimir(ds._data);
    });
}

function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        // AjaxCargarDatos();
        FiltroMostrar(0);
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        //AjaxCargarDatos();
        FiltroMostrar(1);
    });
}

function FiltroMostrar(mostrar) {
    var ds = $("#grid").data("kendoGrid").dataSource;

    if (mostrar == 0) {
        var curr_filters = ds.filter().filters;
        if (curr_filters[0].filters != undefined)
            ds.filter(curr_filters[0].filters[0])
        else
            ds.filter(curr_filters[0])
        ds.sync();


    }
    else {

        var curr_filters = ds.filter().filters;
        ds.filter(curr_filters[0])
        ds.sync();
        var filters = ds.filter();
        filters.logic = "or"

        filters.filters.push({ field: "Accion", operator: "eq", value: 2 });
        filters.filters.push({ field: "Accion", operator: "eq", value: 4 });
        ds.sync();
    }
}

function SuscribirEventoComboPrueba() {
    $('#inputPrueba').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "TipoPruebaID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                AjaxObtenerProveedor();
            }
            else {
                $("#inputPrueba").data("kendoComboBox").value("");
            }
            $("#inputProveedor").data("kendoComboBox").text("");
            $("#inputProveedor").data("kendoComboBox").setDataSource();
            $("#inputRequisicion").data("kendoComboBox").text("");
            $("#inputRequisicion").data("kendoComboBox").setDataSource();
            $("#grid").data('kendoGrid').dataSource.data([]);
        }
    });

};


function SuscribirEventoComboProveedor() {
    $('#inputProveedor').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProveedorID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                AjaxRequisicion();
            }
            else {
                $("#inputProveedor").data("kendoComboBox").value("");
            }
            $("#inputRequisicion").data("kendoComboBox").text("");
            $("#inputRequisicion").data("kendoComboBox").setDataSource();
            $("#grid").data('kendoGrid').dataSource.data([]);
        }
    });

};




function SuscribirEventoComboProyecto() {
    $('#inputProyecto').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined ) {
                AjaxPruebas();
            }
            else {
                $("#inputProyecto").data("kendoComboBox").value("");
            }
            
            $("#inputPrueba").data("kendoComboBox").setDataSource();
            

            //$("#inputRequisicion").data("kendoComboBox").setDataSource();
            

            //$("#inputProveedor").data("kendoComboBox").setDataSource();
            $("#grid").data('kendoGrid').dataSource.data([]);
            
            
        }
    });

};


function SuscribirEventoComboRequisicion() {
    $('#inputRequisicion').kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "NombreRequisicion",
        dataValueField: "RequisicionID",
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                AjaxCargarDatos();
            }
            else {
                $("#inputRequisicion").data("kendoComboBox").value("");
                $("#grid").data('kendoGrid').dataSource.data([]);
            }
        }
    });

};
