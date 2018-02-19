var proyectoInicial = 0;
var pruebaOriginal = 0;
var requisicionOriginal = 0;
var ventanaNumeroComponentes;
var ventanaProcesosPintura;
var ventanaConfirmBorrarProcesoNoPintable;
var ventanaConfirmGuardado;
var ventanaConfirmConColor;
var ventanaConfirmEliminarSP;

function SuscribirEventos() {
    GuardarDetallePruebas();
    suscribirEventoGuardar();
    SuscribirEventoComboColor();
    suscribirEventoDetallePruebas();
    suscribirEventoDetalleComponentes();
    SuscribirEventoCerrarPopUpPruebas();
    suscribirEventoProyecto();
    SuscribirEventoComboProyecto();
    suscribirEventoChangeAplicable();
    SuscribirEventoEliminarSistemaPintura();
    SuscribirEventoGuardarDetalleComponentes();
    SuscribirEventoCancelarDetalleComponentes();
    suscribirEventoVentanaCambioNumeroDeComponentes();
    suscribirEventoVentanaCambioProcesosPintura();
};


function suscribirEventoVentanaCambioProcesosPintura() {
    ventanaProcesosPintura = $("#ventanaConfirmProcesos").kendoWindow({
        iframe: true,
        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
        visible: false, //the window will not appear before its .open method is called
        width: "auto",
        height: "auto",
        modal: true,
        animation: {
            close: false,
            open: false
        },
        actions: []
    }).data("kendoWindow");

    ventanaProcesosPintura.content("Se eliminaran los datos del proceso, ¿desea continuar?" +
        "</br><center><button class='btn btn-blue' id='yesButton'>Si</button><button class='btn btn-blue' id='noButton'> No</button></center>");

    $("#yesButton").click(function () {
        dataItemProcesoPintura.Agregar = false;
        dataItemProcesoPintura.MetrosLote = 0;
        dataItemProcesoPintura.NumeroPruebas = 0;
        dataItemProcesoPintura.NumeroComponentes = 0;
        dataItemProcesoPintura.ListaDetalleComponentesAgregados = [];
        //dataItem.
        dataItemProcesoPintura.Reductor = "";
        dataItemProcesoPintura.ReductorID = "";
        dataItemProcesoPintura.listadoPruebasDetalle = [];
        $("#grid").data("kendoGrid").dataSource.sync();
        ventanaProcesosPintura.close();
       
    });
    $("#noButton").click(function () {
       
        dataItemProcesoPintura.Agregar = true;
        // $(this)[0].checked = true;
        $("#grid").data("kendoGrid").dataSource.sync();
        ventanaProcesosPintura.close();
    });
}



function suscribirEventoVentanaCambioNumeroDeComponentes()
{
    ventanaNumeroComponentes = $("#ventanaConfirmCompontes").kendoWindow({
        iframe: true,
        title: _dictionary.WarningTitle[$("#language").data("kendoDropDownList").value()],
        visible: false, //the window will not appear before its .open method is called
        width: "auto",
        height: "auto",
        modal: true,
        animation: {
            close: false,
            open: false
        },
        actions: []
    }).data("kendoWindow");

    ventanaNumeroComponentes.content(_dictionary.SistemaPinturaModificaNumeroComponentes[$("#language").data("kendoDropDownList").value()] +
                "</br><center><button class='btn btn-blue' id='yesButtonComponente'>Si</button><button class='btn btn-blue' id='noButtonComponente'> No</button></center>");

   

    $("#yesButtonComponente").click(function (e) {

        agregarComponentesAutomaticos();
        ventanaNumeroComponentes.close();
    });
    $("#noButtonComponente").click(function (e) {
       
        dataItemRender.NumeroComponentes = numeroPlacasComponentesElemento.NumeroComponentes;
        $("#grid").data("kendoGrid").refresh();
        ventanaNumeroComponentes.close();
    });
}

function suscribirEventoChangeAplicable() {
    $('#inputNoAplicable').change(function () {

        var isEmptyGrid = false;
        var isEmptyColor = false;

		
        var ds = $("#grid").data("kendoGrid").dataSource;
       

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].Agregar) {
                isEmptyGrid = true;
            }
        }

        if ($("#inputColor").data("kendoMultiSelect")._values.length == 0) {
            isEmptyColor = true;
        }
        if (tieneAvance)
        {
            if (($("#inputNoAplicable").is(':checked')))
            {
                $('#inputNoAplicable')[0].checked = false;
            }
            else
            {
                $('#inputNoAplicable')[0].checked = true;
            }
            displayNotify("MensajeSpoolAsignado", "", '1');
            
        }
        else if (($("#inputNoAplicable").is(':checked'))) {
            if (isEmptyGrid) {

                ventanaConfirmBorrarProcesoNoPintable = $("#ventanaConfirm").kendoWindow({
                    iframe: true,
                    title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                    visible: false, //the window will not appear before its .open method is called
                    width: "auto",
                    height: "auto",
                    modal: true,
                   
                    animation: {
                        close: false,
                        open: false
                    },
                    actions: []
                }).data("kendoWindow");

                ventanaConfirmBorrarProcesoNoPintable.content(_dictionary.MensajeEliminarColoresYProcesosSistemaNoPintable[$("#language").data("kendoDropDownList").value()] +
                    "</br><center><button class='btn btn-blue' id='yesButtonNoPintable'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonNoPintable'> " + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                ventanaConfirmBorrarProcesoNoPintable.open().center();



                $("#yesButtonNoPintable").click(function () {
                    LimpiarGrid();
                    $("#inputNoAplicable").prop("checked", true);
                    ventanaConfirmBorrarProcesoNoPintable.close();
                });
                $("#noButtonNoPintable").click(function () {
                    ventanaConfirmBorrarProcesoNoPintable.close();
                    $("#inputNoAplicable").prop("checked", false);

                });



            }
            else {
                if (!isEmptyColor) {
                    ventanaConfirmConColor = $("#ventanaConfirm").kendoWindow({
                        iframe: true,
                        title: _dictionary.EntregaPlacasGraficasTituloPopup[$("#language").data("kendoDropDownList").value()],
                        visible: false, //the window will not appear before its .open method is called
                        width: "auto",
                        height: "auto",
                        modal: true,

                        animation: {
                            close: false,
                            open: false
                        },
                        actions: []
                    }).data("kendoWindow");

                    ventanaConfirmConColor.content(_dictionary.MensajeEliminarColoresSistemaNoPintable[$("#language").data("kendoDropDownList").value()] +
                        "</br><center><button class='btn btn-blue' id='yesButtonConColor'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButtonConColor'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

                    ventanaConfirmConColor.open().center();



                    $("#yesButtonConColor").click(function () {
                        LimpiarGrid();
                        $("#inputNoAplicable").prop("checked", true);
                        $("#inputColor").data("kendoMultiSelect").value([]);
                        $("#inputColor").data("kendoMultiSelect").enable(false);
                        ventanaConfirmConColor.close();
                    });
                    $("#noButtonConColor").click(function () {
                        ventanaConfirmConColor.close();
                        $("#inputNoAplicable").prop("checked", false);

                    });
                }
                else {
                    $("#inputColor").data("kendoMultiSelect").value([]);
                    $("#inputColor").data("kendoMultiSelect").enable(false);
                }

            }
        }
        else {
            $("#inputColor").data("kendoMultiSelect").enable(true);

        }

    });

}

function LimpiarGrid() {
    var ds = $("#grid").data("kendoGrid").dataSource;

    for (var i = 0; i < ds._data.length; i++) {
        if (ds._data[i].Agregar) {
            ds._data[i].Agregar = false;
            ds._data[i].MetrosLote = 0;
            ds._data[i].NumeroComponentes = 0;
            ds._data[i].ListaDetalleComponentesAgregados = [];
            ds._data[i].ReductorID = 0;
            ds._data[i].Reductor = "";
            ds._data[i].NumeroPruebas = 0;
            ds._data[i].listadoPruebasDetalle = [];

        }
    }
    ds.sync();
    $("#inputColor").data("kendoMultiSelect").value([]);
    $("#inputColor").data("kendoMultiSelect").enable(false);
}



function suscribirEventoProyecto() {

    $("#inputProyecto").kendoMultiSelect({
        dataSource: '',
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        filter: "contains",
        change: function (e) {
        }
    }).data("kendoMultiSelect");

}

function GuardarDetallePruebas() {
    $('#GuardarDetallePruebas').click(function () {

        var ds = $("#gridPopUp").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            if (ds._data[i].UnidadMinima == "" || ds._data[i].UnidadMaxima == "" || ds._data[i].ProyectoProcesoPrueba == "" || ds._data[i].UnidadMedida == "") {
                displayNotify("SistemaPinturaMensajeCamposMandatorios", "", "1");
                return;
            }
            else if (parseInt(ds._data[i].UnidadMinima) >= parseInt(ds._data[i].UnidadMaxima)) {
                displayNotify("SistemaPinturaMensajeUnidadMedidaError", "", "1");
                return;
            }

            
            ds._data[i].SistemaPinturaProyectoProcesoID = ds._data[i].SistemaPinturaProyectoProcesoID == undefined ? 0 : ds._data[i].SistemaPinturaProyectoProcesoID;
            ds._data[i].ProyectoProcesoPruebaID = ds._data[i].ProyectoProcesoPruebaID == undefined ? 0 : ds._data[i].ProyectoProcesoPruebaID;
            ds._data[i].Accion = ds._data[i].ProyectoProcesoPruebaID == 0 ? 1 : 2;
        }
        modeloRenglon.listadoPruebasDetalle = ds._data;
        $("#windowGrid").data("kendoWindow").close();
        $("#grid").data("kendoGrid").dataSource.sync();

    });
}


function suscribirEventoDetalleComponentes() {

    $(document).on('click', '.EnlaceDetalleComponentes', function (e) {
        e.preventDefault();
        if (!($("#inputNoAplicable").is(':checked'))) {
            if ($('#botonGuardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {

                var grid = $("#grid").data("kendoGrid"),
                dataItem = grid.dataItem($(e.target).closest("tr"));
                if (dataItem.Agregar && dataItem.NumeroComponentes > 0) {
                    LlenarGridPopUpComponentesAgregados(dataItem);
                }

            }
        }
    });
}

function suscribirEventoDetallePruebas() {

    $(document).on('click', '.EnlaceDetallePruebas', function (e) {
        e.preventDefault();
        if (!($("#inputNoAplicable").is(':checked'))) {
            if ($('#botonGuardar').text() == _dictionary.lblGuardar[$("#language").data("kendoDropDownList").value()]) {

                var grid = $("#grid").data("kendoGrid"),
                dataItem = grid.dataItem($(e.target).closest("tr"));
                if (dataItem.Agregar) {
                    LlenarGridPopUp(dataItem);
                }

            }
        }
    });
}

function SuscribirEventoCerrarPopUpPruebas() {
    $("#CerrarDetallePruebas").click(function (e) {
        e.preventDefault();

        $("#windowGrid").data("kendoWindow").close();
    });
}




function SuscribirEventoComboColor() {
    $("#inputColor").kendoMultiSelect({
        dataSource: '',
        dataTextField: "Nombre",
        dataValueField: "ColorID",
        suggest: true,
        filter: "contains",
        change: function (e) {
        }
    }).data("kendoMultiSelect");

};

function SuscribirEventoComboProyecto() {
    $("#comboProyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                $("#inputNoAplicable").prop("checked", false);
                
                $("#grid").data("kendoGrid").dataSource.data([]);
                AjaxCargarEdicionSistemaPintura();
            }
            else {
                $("#comboProyecto").data("kendoComboBox").value("");
            }
        }
    });
}


function Limpiar() {

    $("#grid").data("kendoGrid").dataSource.data([]);
    opcionHabilitarView(false, "FieldSetView");
    $("#inputNombre").val("");
    $("#inputhiddenSistemaPinturaID").val("");
    $("#inputSistemaPinturaID").val("");
    $("#divComboProyecto").css("display", "none");
    $("#divMultiselectProyecto").css("display", "block");
    $("#inputNoAplicable").prop("checked", false);
    
}


function SuscribirEventoGuardarDetalleComponentes() {
    $('#GuardarDetalleComponenteAgregado').click(function (e) {
        e.preventDefault();
        var componentesCorrectos = true;
        var ds = $("#gridPopUpComponentesAgregados").data("kendoGrid").dataSource;

        for (var i = 0; i < ds._data.length; i++) {
            ds._data[i].RowOk = true;
            if (ds._data[i].Nombre == "" && !(ds._data[i].Accion == 3 || ds._data[i].Accion == 4))
                ds._data[i].RowOk = false;
        }

        ////
        if (!ExistRowErrors(ds._data)) {
            if (ds._data.length > 0) {
                if (SincronizarOrigen(ds._data));
                $("#windowGridComponenteAgregado").data("kendoWindow").close();
            }
            else {
                loadingStop();
            }
        }
        else {
            //loadingStop();
            $("#gridPopUpComponentesAgregados").data("kendoGrid").dataSource.sync();
            //ventanaConfirm = $("#ventanaConfirm").kendoWindow({
            //    iframe: true,
            //    title: _dictionary.TituloPopUpError[$("#language").data("kendoDropDownList").value()],
            //    visible: false, //the window will not appear before its .open method is called
            //    width: "auto",
            //    height: "auto",
            //    modal: true,
            //    animation: {
            //        close: false,
            //        open: false
            //    },
            //    actions: []
            //}).data("kendoWindow");

            //ventanaConfirm.content(_dictionary.MensajeConfirmacionGuardadoGeneral[$("#language").data("kendoDropDownList").value()] +
            //    "</br><center><button class='btn btn-blue' id='yesButton'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='btn btn-blue' id='noButton'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

            //ventanaConfirm.open().center();

            //$("#yesButton").click(function () {
            //    loadingStart();

            //    ArregloGuardado = [];
            //    var ds = $("#gridPopUpComponentesAgregados").data("kendoGrid").dataSource;

            //    SincronizarOrigen(ds._data);
            //    loadingStop();
            //    ventanaConfirm.close();
            //    $("#windowGridComponenteAgregado").data("kendoWindow").close();
            //});

            //$("#noButton").click(function () {
            //    ventanaConfirm.close();
            //});
            displayNotify("MensajeCamposIncorrectorEnRojo", "", '2');
        }
    });
}

function SuscribirEventoCancelarDetalleComponentes() {
    $("#CerrarDetalleComponenteAgregado").click(function (e) {
        e.preventDefault();
        $("#windowGridComponenteAgregado").data("kendoWindow").close();
    });
}

function suscribirEventoGuardar() {
    $('#Guardar').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
			
            if ($("#inputSistemaPinturaID").val() == "") {
                AjaxVerificarNombre($("#inputNombre").val(), ds._data, 0);
            } else {
                AjaxGuardarCaptura(ds._data, 0);
            }
			
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardar').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
			
            if ($("#inputSistemaPinturaID").val() == "") {
                AjaxVerificarNombre($("#inputNombre").val(), ds._data, 0);
            } else {
                AjaxGuardarCaptura(ds._data, 0);
            }
			
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo').click(function (e) {

        e.preventDefault();
	
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($("#inputSistemaPinturaID").val() == "") {
            AjaxVerificarNombre($("#inputNombre").val(), ds._data, 1);
        } else {
            AjaxGuardarCaptura(ds._data, 1);
        }
		

    });


    $('#GuardarPie').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
			
            if ($("#inputSistemaPinturaID").val() == "") {
                AjaxVerificarNombre($("#inputNombre").val(), ds._data, 0);
            } else {
                AjaxGuardarCaptura(ds._data, 0);
            }
			
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardar1').click(function (e) {
        e.preventDefault();
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($('#botonGuardar').text() == "Guardar") {
		
            if ($("#inputSistemaPinturaID").val() == "") {
                AjaxVerificarNombre($("#inputNombre").val(), ds._data, 0);
            } else {
                AjaxGuardarCaptura(ds._data, 0);
            }
			
        }
        else if ($('#botonGuardar').text() == "Editar")
            opcionHabilitarView(false, "FieldSetView")
    });

    $('#btnGuardarYNuevo1').click(function (e) {
        e.preventDefault();
		
        var ds = $("#grid").data("kendoGrid").dataSource;
        if ($("#inputSistemaPinturaID").val() == "") {
            AjaxVerificarNombre($("#inputNombre").val(), ds._data, 1);
        } else {
            AjaxGuardarCaptura(ds._data, 1);
        }
		
    });
}

function SuscribirEventoEliminarSistemaPintura() {
    $('.eliminarSistema').click(function (e) {
       
        e.preventDefault();
        var SistemaPinturaID = $("#inputhiddenSistemaPinturaID").val();
        ventanaConfirmEliminarSP = $("#ventanaConfirm").kendoWindow({
            iframe: true,
            title: _dictionary.PinturaCargaTitulo[$("#language").data("kendoDropDownList").value()],
            visible: false, //the window will not appear before its .open method is called
            width: "auto",
            height: "auto",
            modal: true,
            animation: {
                close: false,
                open: false
            },
            actions: []
        }).data("kendoWindow");

        ventanaConfirmEliminarSP.content(_dictionary.SistemaPinturaMensajeConfirmaEliminar[$("#language").data("kendoDropDownList").value()] +
                     "</br><center><button class='confirm_yes btn btn-blue' id='yesButtonEliminar'>" + _dictionary.lblSi[$("#language").data("kendoDropDownList").value()] + "</button><button class='confirm_yes btn btn-blue' id='noButtonEliminar'>" + _dictionary.lblNo[$("#language").data("kendoDropDownList").value()] + "</button></center>");

        ventanaConfirmEliminarSP.open().center();

        $("#yesButtonEliminar").click(function () {
            
            if ($("#comboProyecto").data("kendoComboBox").select() > 0) {
                if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
                    if (tieneAvance) {
                        AjaxEliminaSistemaPintura(SistemaPinturaID, $("#comboProyecto").data("kendoComboBox").dataItem($("#comboProyecto").data("kendoComboBox").select()).ProyectoID);
                    }
                    else {
                        displayNotify("SistemaPinturaErrorEliminado", "", '2');
                    }
                }
                ventanaConfirmEliminarSP.close();
            }
            else
                displayNotify("SistemaPinturaMensajeErrorProyecto", "", "1");
        });
        $("#noButtonEliminar").click(function () {
            ventanaConfirmEliminarSP.close();
        });
    });
}

function opcionHabilitarView(valor, name) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);

        $("#inputColor").data("kendoMultiSelect").enable(false);

        $("#comboProyecto").data("kendoComboBox").enable(false);
       

        $('#botonGuardar2').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonEditar[$("#language").data("kendoDropDownList").value()]);

    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputNombre").attr('disabled', false);
        
        if($("#inputNoAplicable").is(':checked'))
            $("#inputColor").data("kendoMultiSelect").enable(false);
        else
            $("#inputColor").data("kendoMultiSelect").enable(true);

        $("#comboProyecto").data("kendoComboBox").enable(true);
       

        $('#botonGuardar2').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $("#botonGuardar3").text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
        $('#botonGuardar4').text(_dictionary.botonGuardar[$("#language").data("kendoDropDownList").value()]);
    }
}


