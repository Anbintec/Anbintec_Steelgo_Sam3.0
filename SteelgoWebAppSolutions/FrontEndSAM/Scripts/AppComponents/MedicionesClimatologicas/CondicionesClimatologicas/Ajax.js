var TipoMuestraPredeterminadoID = 3069;

function AjaxCargarCamposPredeterminados() {
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {
        $('#inputMedicionesfechaToma').val(data);

        loadingStop();
    });

    AjaxGetListaPatio();
};

function AjaxGetListaPatio() {
    $Patios.Patios.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputPatio").data("kendoComboBox").dataSource.data(data);

        if ($("#inputPatio").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputPatio").data("kendoComboBox").select(1);
            $("#inputPatio").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxGetListaZona() {
    $Zona.Zona.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputZona").data("kendoComboBox").dataSource.data(data);

        if ($("#inputZona").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputZona").data("kendoComboBox").select(1);
            $("#inputZona").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxCargarEquiposToma() {
    $CondicionesClimatologicas.CondicionesClimatologicas.read({ token: Cookies.get("token") }).done(function (data) {
        $("#inputEquipoTomaTempAmb").data("kendoComboBox").dataSource.data(data[0].Equipos[0].EquiposTemperaturaAmbiente);

        if ($("#inputEquipoTomaTempAmb").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputEquipoTomaTempAmb").data("kendoComboBox").select(1);
            $("#inputEquipoTomaTempAmb").data("kendoComboBox").trigger("change");
        }

        $("#inputEquipoTomaHumedad").data("kendoComboBox").dataSource.data(data[0].Equipos[0].EquiposHumedad);

        if ($("#inputEquipoTomaHumedad").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputEquipoTomaHumedad").data("kendoComboBox").select(1);
            $("#inputEquipoTomaHumedad").data("kendoComboBox").trigger("change");
        }

        $("#inputEquipoTomaPtoRocio").data("kendoComboBox").dataSource.data(data[0].Equipos[0].EquiposPuntoRocio);

        if ($("#inputEquipoTomaPtoRocio").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputEquipoTomaPtoRocio").data("kendoComboBox").select(1);
            $("#inputEquipoTomaPtoRocio").data("kendoComboBox").trigger("change");
        }

        $("#inputEquipoTomaCampoX").data("kendoComboBox").dataSource.data(data[0].Equipos[0].EquiposCampoX);

        if ($("#inputEquipoTomaCampoX").data("kendoComboBox").dataSource._data.length == 2) {
            $("#inputEquipoTomaCampoX").data("kendoComboBox").select(1);
            $("#inputEquipoTomaCampoX").data("kendoComboBox").trigger("change");
        }
    });
}

function AjaxGuardarCaptura() {
    var FechaToma = $('#inputMedicionesfechaToma').val();
    var HoraToma = $('#inputMedicionesHoraToma').val();
    var PatioID = $('#inputPatio').val();
    var ZonaID = $('#inputZona').val();
    var TempAmb = $('#inputMedicionesTempAmbiente').val();
    var EquipoTomaTemAmbID = $('#inputEquipoTomaTempAmb').val();
    var Humedad = $('#inputMedicionesHumedad').val();
    var EquipoTomaHumedadID = $('#inputEquipoTomaHumedad').val();
    var PuntoRocio = $('#inputMedicionesPuntoRocio').val();
    var EquipoTomaPuntoRocioID = $('#inputEquipoTomaPtoRocio').val();
    var CampoX = $('#inputMedicionesCampoX').val();
    var EquipoTomaCampoXID = $('#inputEquipoTomaCampoX').val();

    if (FechaToma != "" && HoraToma != "" && PatioID > 0 && PatioID != undefined && ZonaID > 0 && ZonaID != undefined
        && TempAmb != "" && EquipoTomaTemAmbID > 0 && EquipoTomaTemAmbID != undefined
        && Humedad != "" && EquipoTomaHumedadID > 0 && EquipoTomaHumedadID != undefined
        && PuntoRocio != "" && EquipoTomaPuntoRocioID > 0 && EquipoTomaPuntoRocioID != undefined
        && CampoX != "" && EquipoTomaCampoXID > 0 && EquipoTomaCampoXID != undefined) {

        ajaxGuardar();
    }
    else displayNotify("", "Todos los campos deben ser capturados", '1');
}

function ajaxGuardar() {
    loadingStart();
    displayNotify("", "se guardo correctamente la informacion", '0');
    opcionHabilitarView(true, "FieldSetView");


    setTimeout(function () { loadingStop() }, 500);

};