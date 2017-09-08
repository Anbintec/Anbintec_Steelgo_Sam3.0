var previousCurrentItem;

function SuscribirEventos() {
    suscribirEventoDetalleInspeccion();

    suscribirEventoDetalleDefectoPorPlaca();
    suscribirEventoCancelar();
    suscribirEventoGuardar();
    suscribirEventoComboOrdenCompra();
}


function suscribirEventoChangeRadio() {
    $('input:radio[name=Muestra]:nth(0)').change(function () {
        //Gerez

        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("CantG");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FechaRecibido");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("Camion");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FacturaProveedor");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("Acuerdo");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FechaEnvio");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("Pedimento");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("ShippingDate");

        //Steelgo
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("CantS");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FechaRecibidoS");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("InspeccionS");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FechaFactura");
    });
    $('input:radio[name=Muestra]:nth(1)').change(function () {
        //Gerez
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("CantG");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FechaRecibido");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("Camion");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FacturaProveedor");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("Acuerdo");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FechaEnvio");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("Pedimento");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("ShippingDate");

        //Steelgo

        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("CantS");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FechaRecibidoS");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("InspeccionS");
        $("#gridPopUpSteelgo").data("kendoGrid").hideColumn("FechaFactura");
    });
    $('input:radio[name=Muestra]:nth(2)').change(function () {
        //Gerez
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("CantG");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FechaRecibido");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("Camion");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FacturaProveedor");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("Acuerdo");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FechaEnvio");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("Pedimento");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("ShippingDate");

        //Steelgo

        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("CantS");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FechaRecibidoS");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("InspeccionS");
        $("#gridPopUpSteelgo").data("kendoGrid").showColumn("FechaFactura");
    });
}

function suscribirEventoComboOrdenCompra() {

    $('#inputOrdenCompra').kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "OrdenCompraID",
        dataSource: [],
        suggest: true,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                //pruebaOriginal = $("#inputOrdenCompra").data("kendoComboBox").value();
                AjaxCargarRevision();
            }
            else {
                $("#grid").data("kendoGrid").dataSource.data([]);
            }

        }
    });
}


function suscribirEventoDetalleDefectoPorPlaca() {

    $(document).on('click', '.EnlaceDetalleColada', function (e) {

        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()]) {
            var grid = $("#grid").data("kendoGrid");
            dataItem = grid.dataItem($(e.target).closest("tr"));

            LlenarGridPopUpDetalleColadas(dataItem);


        }
    });
}

function suscribirEventoDetalleInspeccion() {

    $(document).on('click', '.EnlaceInspeccion', function (e) {
        e.preventDefault();
        if ($('#Guardar').text() == _dictionary.MensajeGuardar[$("#language").data("kendoDropDownList").value()] && $('input:radio[name=Muestra]:nth(0)').prop('checked')) {
            var grid;

            $("#gridPopUpInspeccion").data('kendoGrid').dataSource.data([]);
            if ($('input:radio[name=Muestra]:nth(0)').prop('checked')) {
                grid = $("#gridPopUpCecilia").data("kendoGrid");
            }
            else if ($('input:radio[name=Muestra]:nth(1)').prop('checked')) {
                grid = $("#gridPopUpGerez").data("kendoGrid");
            }
            else if ($('input:radio[name=Muestra]:nth(2)').prop('checked')) {
                grid = $("#gridPopUpSteelgo").data("kendoGrid");
            }


            dataItem = grid.dataItem($(e.target).closest("tr"));

            LlenarGridPopUpDetalleInspeccion(dataItem);
        }
    });
}


function suscribirEventoCancelar() {
    $(document).on('click', '#CancelarPlacasCecilia', function (e) {
        $("#windowGridCecilia").data("kendoWindow").close();
    });
    $(document).on('click', '#CancelarPlacasGerez', function (e) {
        $("#windowGridGerez").data("kendoWindow").close();
    });

    $(document).on('click', '#CancelarDefectos', function (e) {
        $("#windowGridSteelgo").data("kendoWindow").close();
    });

    $(document).on('click', '#CancelarInspeccion', function (e) {

        $("#windowGridInspeccion").data("kendoWindow").close();
    });


}

function suscribirEventoGuardar() {
    $(document).on('click', '#GuardarPlacasSteelgo', function (e) {
        var coladasCorrectos = true;
        var ds = $("#gridPopUpSteelgo").data("kendoGrid").dataSource;
        var window = $("#windowGridSteelgo");

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if ((ds._data[i].Colada == "" || ds._data[i].Cant == 0 || ds._data[i].Cant == "") && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                coladasCorrectos = false;
        }
        var RegistrosNoRepetidos = true;
        for (var i = 0; i < ds._data.length; i++) {
            for (var j = 0; j < ds._data.length; j++) {
                if (ds._data[i].Colada.trim() == ds._data[j].Colada.trim() && i != j)
                    RegistrosNoRepetidos = false;
            }
        }

        if (RegistrosNoRepetidos) {
            if (coladasCorrectos) {
                modeloRenglon.ListaDetalleColadas = ds._data;
                $("#grid").data("kendoGrid").dataSource.sync();
                window.data("kendoWindow").close();
            }
            else
                displayNotify('DynasolColadaMandatorio', '', '2');

        }
        else
            displayNotify('DynasolColadasRegistrosRepetidos', '', '2');

    });


    $(document).on('click', '#GuardarPlacasGerez', function (e) {
        var coladasCorrectos = true;
        var ds = $("#gridPopUpGerez").data("kendoGrid").dataSource;
        var window = $("#windowGridGerez");

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if ((ds._data[i].Colada == "" || ds._data[i].Cant == 0 || ds._data[i].Cant == "") && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                coladasCorrectos = false;
        }
        var RegistrosNoRepetidos = true;
        for (var i = 0; i < ds._data.length; i++) {
            for (var j = 0; j < ds._data.length; j++) {
                if (ds._data[i].Colada == ds._data[j].Colada && i != j)
                    RegistrosNoRepetidos = false;
            }
        }

        if (RegistrosNoRepetidos) {
            if (coladasCorrectos) {
                modeloRenglon.ListaDetalleColadas = ds._data;
                $("#grid").data("kendoGrid").dataSource.sync();
                window.data("kendoWindow").close();
            }
            else
                displayNotify('DynasolColadaMandatorio', '', '2');

        }
        else
            displayNotify('DynasolColadasRegistrosRepetidos', '', '2');



    });

    $(document).on('click', '#GuardarPlacasCecilia', function (e) {
        var coladasCorrectos = true;
        var ds = $("#gridPopUpCecilia").data("kendoGrid").dataSource;
        var window = $("#windowGridCecilia");

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if ((ds._data[i].Colada == "" || ds._data[i].Cant == 0 || ds._data[i].Cant == "") && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                coladasCorrectos = false;
        }

        var RegistrosNoRepetidos = true;
        for (var i = 0; i < ds._data.length; i++) {
            for (var j = 0; j < ds._data.length; j++) {
                if (ds._data[i].Colada.trim() == ds._data[j].Colada.trim() && i != j)
                    RegistrosNoRepetidos = false;
            }
        }

        if (RegistrosNoRepetidos) {
            if (coladasCorrectos) {
                modeloRenglon.ListaDetalleColadas = ds._data;
                $("#grid").data("kendoGrid").dataSource.sync();
                window.data("kendoWindow").close();
            }
            else
                displayNotify('DynasolColadaMandatorio', '', '2');

        }
        else
            displayNotify('DynasolColadasRegistrosRepetidos', '', '2');


    });


    $(document).on('click', '#GuardarInspeccion', function (e) {
        var inspeccionCorrecta = true;
        var ds = $("#gridPopUpInspeccion").data("kendoGrid").dataSource;
        var window = $("#windowGridInspeccion");


        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Accion == undefined || ds._data[i].Accion == 0)
                ds._data[i].Accion = 1;
            if ((ds._data[i].Inspeccion == "") && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                inspeccionCorrecta = false;
        }
        if (inspeccionCorrecta) {
            coladaRow.ListaDetalleInspeccion = ds._data;

            var longitudInspeccion = ds._data.length;

            if (longitudInspeccion > 0) {
                coladaRow.InspeccionDetalle = ds._data[0].Inspeccion;
                coladaRow.Comentario = ds._data[0].Comentario;
            }
            else {
                coladaRow.InspeccionDetalle = "Sin Inspeccion";
                coladaRow.Comentario = "";
            }


            if ($('input:radio[name=Muestra]:nth(0)').prop('checked')) {
                $("#gridPopUpCecilia").data("kendoGrid").dataSource.sync();
            }
            else if ($('input:radio[name=Muestra]:nth(1)').prop('checked')) {
                $("#gridPopUpGerez").data("kendoGrid").dataSource.sync();
            }
            else if ($('input:radio[name=Muestra]:nth(2)').prop('checked')) {
                $("#gridPopUpSteelgo").data("kendoGrid").dataSource.sync();
            }

            window.data("kendoWindow").close();
        }
        else
            displayNotify('DynasolInspeccionMandatorio', '', '2');



    });
    //----------------------------------------GUARDAR CAPTURA TOTAL

    $('.accionGuardar').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
            if ($('#Guardar').text() == "Guardar" || $('#Guardar').text() == "Save") {
                AjaxGuardarCaptura(ds._data, 0);
            }
            else if ($('#Guardar').text() == "Editar" || $('#Guardar').text() == "Edit") {
                opcionHabilitarView(false)
            }
        }
        else {
                displayNotify("AdverteciaExcepcionGuardado", "", '1');
        }

        });

    $('.accionGuardarNuevo').click(function (e) {
        var ds = $("#grid").data("kendoGrid").dataSource;
        if (ds._data.length > 0) {
                AjaxGuardarCaptura(ds._data, 1);
        }
        else {
            displayNotify("AdverteciaExcepcionGuardado", "", '1');
        }

        });

    }



function opcionHabilitarView(valor, name) {

    if(valor) {
        $('#FieldSetView').find('*').attr('disabled', true);

        $("#inputOrdenCompra").data("kendoComboBox").enable(false);


        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

        }
        else {
        $('#FieldSetView').find('*').attr('disabled', false);

        $("#inputOrdenCompra").data("kendoComboBox").enable(true);


        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);


        }
        }
