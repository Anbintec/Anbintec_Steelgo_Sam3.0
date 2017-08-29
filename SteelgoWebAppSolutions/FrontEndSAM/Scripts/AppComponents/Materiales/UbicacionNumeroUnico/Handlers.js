function SuscribirEventos() {
    suscribirEventoProyecto();
    SuscribirEventoRack();
    SuscribirEventoMostrar();
    SuscribirEventoRackUbicacion();
    SuscribirEventoRackPasillo();
    SuscribirEventoRackPiso();
    suscribirEventoGuardar();
    sucribirEventoAgregar();
    suscribirEventoRangoNU();
    
}


function suscribirEventoRangoNU() {
    $("#inputRangoNumeroUnico").keydown(function (e) {


        if (e.keyCode == 13) {
            agregarNU();
        }
    });

}
function SuscribirEventoMostrar() {
    $("input:radio[name=Mostrar]").change(function () {
        if ($('input:radio[name=Mostrar]:checked').val() == "Uno") {
            $("#MostrarUno").css("display", "block");
            $("#MostrarDos").css("display", "none");
            $("#inputRackUbicacion").data("kendoComboBox").text("");
            $("#inputRackPasillo").data("kendoComboBox").text("");
            $("#inputRackNivel").data("kendoComboBox").text("");
            var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
            if (Proyecto != null) {
                if (Proyecto.ProyectoID != 0) {
                    AjaxCargarRacks(Proyecto.ProyectoID);
                }
            }
        } else {
            $("#inputRack").data("kendoComboBox").value("");
            $("#MostrarUno").css("display", "none");
            $("#MostrarDos").css("display", "block");
            $("#inputRackUbicacion").data("kendoComboBox").dataSource.data([]);
            $("#inputRackPasillo").data("kendoComboBox").dataSource.data([]);
            $("#inputRackNivel").data("kendoComboBox").dataSource.data([]);
            $("#inputRackUbicacion").data("kendoComboBox").value("");
            $("#inputRackPasillo").data("kendoComboBox").value("");
            $("#inputRackNivel").data("kendoComboBox").value("");
            var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
            if (Proyecto != null) {
                if (Proyecto.ProyectoID != 0) {
                    AjaxCargarUbicacionRack(Proyecto.ProyectoID);
                }
            }
        }
    });
}
function sucribirEventoAgregar() {
    $("#btnAgregar").click(function () {
        agregarNU();
    });
}

function suscribirEventoGuardar() {
    $("#Guardar").click(function () {
        if ($('#Cuantificacion0003').text() == "Guardar") {
            if ($('input:radio[name=Mostrar]:checked').val() == "Uno") {
                var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
                var Rack = $("#inputRack").data("kendoComboBox").dataItem($("#inputRack").data("kendoComboBox").select());
                if (Proyecto != null && Proyecto != undefined) {
                    if (Rack != null) {
                        AjaxGuardar(Proyecto.ProyectoID, $("#grid").data("kendoGrid").dataSource._data, Rack.RackID, false);
                    } else {
                        displayMessage("", "Necesitas seleccionar un  rack", '1');
                    }
                } else {
                    displayMessage("", "Seleccione un Proyecto", "1");
                }
            } else {
                var proyectoID = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
                var ubicacion = $("#inputRackUbicacion").data("kendoComboBox").text();
                var pasillo = $("#inputRackPasillo").data("kendoComboBox").text();
                var nivel = $("#inputRackNivel").data("kendoComboBox").text();
                var rackID = $("#inputRackNivel").data("kendoComboBox").value();
                if (ubicacion != "" && pasillo != "" && nivel != "") {
                    if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
                        AjaxGuardar(proyectoID.ProyectoID, $("#grid").data("kendoGrid").dataSource._data, rackID, false);
                    } else {
                        displayMessage("", "Favor de agregar números únicos.", '1');
                    }
                } else {
                    displayMessage("", "Necesitas seleccionar un  rack", '1');
                }
            }
        }
        else {
            opcionHabilitarView(false);

        }

    });

    $("#GuardarNuevo").click(function () {
        if ($('#Cuantificacion0003').text() == "Guardar") {
            if ($('input:radio[name=Mostrar]:checked').val() == "Uno") {
                var Proyecto = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
                var Rack = $("#inputRack").data("kendoComboBox").dataItem($("#inputRack").data("kendoComboBox").select());
                if (Proyecto != null) {
                    if (Rack != null) {
                        AjaxGuardar(Proyecto.ProyectoID, $("#grid").data("kendoGrid").dataSource._data, Rack.RackID, true);
                    } else {
                        displayMessage("", "Necesitas seleccionar un  rack", '1');
                    }
                } else {
                    displayMessage("", "Seleccione un Proyecto", "1");
                }
            } else {
                var proyectoID = $("#inputProyecto").data("kendoComboBox").dataItem($("#inputProyecto").data("kendoComboBox").select());
                var ubicacion = $("#inputRackUbicacion").data("kendoComboBox").text();
                var pasillo = $("#inputRackPasillo").data("kendoComboBox").text();
                var nivel = $("#inputRackNivel").data("kendoComboBox").text();
                var rackID = $("#inputRackNivel").data("kendoComboBox").value();
                if (ubicacion != "" && pasillo != "" && nivel != "") {
                    if ($("#grid").data("kendoGrid").dataSource._data.length > 0) {
                        AjaxGuardar(proyectoID.ProyectoID, $("#grid").data("kendoGrid").dataSource._data, rackID, true);
                    } else {
                        displayMessage("", "Favor de agregar números únicos.", '1');
                    }
                } else {
                    displayMessage("", "Necesitas seleccionar un  rack", '1');
                }
            }
        }
        else {
            opcionHabilitarView(false);
        }
    });
}


function suscribirEventoProyecto() {

    $("#inputProyecto").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "ProyectoID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            $("#grid").data("kendoGrid").dataSource.data([]);
            $("#inputRangoNumeroUnico").val("");
            if ($("input:radio[name=Mostrar]:checked").val() == "Uno") { //Rack Completo
                $("#inputRack").data("kendoComboBox").value("");
                AjaxCargarRacks(dataItem.ProyectoID);
            } else {
                $("#inputRackUbicacion").data("kendoComboBox").dataSource.data([]);
                $("#inputRackPasillo").data("kendoComboBox").dataSource.data([]);
                $("#inputRackNivel").data("kendoComboBox").dataSource.data([]);
                $("#inputRackUbicacion").data("kendoComboBox").value("");
                $("#inputRackPasillo").data("kendoComboBox").value("");
                $("#inputRackNivel").data("kendoComboBox").value("");
                $("#grid").data("kendoGrid").dataSource.data([]);

                if (dataItem != undefined && dataItem.ProyectoID != 0 && dataItem.Nombre != "") {
                    AjaxCargarUbicacionRack(dataItem.ProyectoID);
                }
            }
        }
    });

    $('#inputProyecto').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 13) {

        }
    });
}

function SuscribirEventoRack() {
    $("#inputRack").kendoComboBox({
        dataTextField: "Nombre",
        dataValueField: "RackID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) { }
    });
}

function SuscribirEventoRackUbicacion() {
    $("#inputRackUbicacion").kendoComboBox({
        dataTextField: "Ubicacion",
        dataValueField: "RackID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            $("#inputRackPasillo").data("kendoComboBox").text("");
            $("#inputRackNivel").data("kendoComboBox").text("");
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != undefined) {
                AjaxCargarPasilloRack(dataItem.Ubicacion);
            }
        }
    });
}

function SuscribirEventoRackPasillo() {
    $("#inputRackPasillo").kendoComboBox({
        dataTextField: "Pasillo",
        dataValueField: "RackID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
            $("#inputRackNivel").data("kendoComboBox").text("");
            var dataItem = this.dataItem(e.sender.selectedIndex);
            var Ubicacion = $("#inputRackUbicacion").data("kendoComboBox").text();
            if (Ubicacion != undefined) {
                if (dataItem != undefined) {
                    AjaxCargarNivelRack(Ubicacion, dataItem.Pasillo);
                }
            }
        }
    });
}

function SuscribirEventoRackPiso() {
    $("#inputRackNivel").kendoComboBox({
        dataTextField: "Nivel",
        dataValueField: "RackID",
        suggest: true,
        delay: 10,
        filter: "contains",
        index: 3,
        change: function (e) {
        }
    });
}

function opcionHabilitarView(valor) {

    if (valor) {
        $('#FieldSetView').find('*').attr('disabled', true);
        $("#inputProyecto").data("kendoComboBox").enable(false);
        $('#RangoNuDiv').find('*').attr('disabled', true);
        $("#inputRackUbicacion").data("kendoComboBox").enable(false);
        $("#inputRackPasillo").data("kendoComboBox").enable(false);
        $("#inputRackNivel").data("kendoComboBox").enable(false);
        $("#Cuantificacion0003").text("Editar");


    }
    else {
        $('#FieldSetView').find('*').attr('disabled', false);
        $("#inputProyecto").data("kendoComboBox").enable(true);
        $('#RangoNuDiv').find('*').attr('disabled', false);
        $("#inputRackUbicacion").data("kendoComboBox").enable(true);
        $("#inputRackPasillo").data("kendoComboBox").enable(true);
        $("#inputRackNivel").data("kendoComboBox").enable(true);
        $("#Cuantificacion0003").text("Guardar");


    }
}

