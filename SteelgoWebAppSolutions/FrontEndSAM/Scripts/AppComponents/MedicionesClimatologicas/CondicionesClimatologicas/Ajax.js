var TipoMuestraPredeterminadoID = 3069;

function AjaxCargarCamposPredeterminados() {
    $CamposPredeterminados.CamposPredeterminados.read({ token: Cookies.get("token"), lenguaje: $("#language").val(), id: TipoMuestraPredeterminadoID }).done(function (data) {

        $('#inputMedicionesfechaToma').val(data);
        AjaxGetListaPatio();
        loadingStop();
    });


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

function AjaxGuardarCaptura( tipoGuardado) {
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
    

    Captura = [];
    Captura[0] = { Detalles: "" };

    ListaDetalle = [];
    i = 0;
    ListaDetalle[i] = { FechaToma: "", HoraToma: "", PatioID: "", ZonaID: "", TempAmb: "", EquipoTomaTemAmbID: "", Humedad: "", EquipoTomaHumedadID: "", PuntoRocio: "", EquipoTomaPuntoRocioID: "", CampoX: "", EquipoTomaCampoXID: "" };
    ListaDetalle[i].FechaToma = FechaToma;
    ListaDetalle[i].HoraToma = HoraToma;
    ListaDetalle[i].PatioID = PatioID;
    ListaDetalle[i].ZonaID = ZonaID;
    ListaDetalle[i].TempAmb = TempAmb;
    ListaDetalle[i].EquipoTomaTemAmbID = EquipoTomaTemAmbID;
    ListaDetalle[i].Humedad = Humedad;
    ListaDetalle[i].EquipoTomaHumedadID = EquipoTomaHumedadID;
    ListaDetalle[i].PuntoRocio = PuntoRocio;
    ListaDetalle[i].EquipoTomaPuntoRocioID = EquipoTomaPuntoRocioID;
    ListaDetalle[i].CampoX = CampoX;
    ListaDetalle[i].EquipoTomaCampoXID = EquipoTomaCampoXID;


    Captura[0].Detalles = ListaDetalle;

    if (FechaToma != "" && HoraToma != "" && PatioID > 0 && PatioID != undefined && ZonaID > 0 && ZonaID != undefined
        && TempAmb != "" && EquipoTomaTemAmbID > 0 && EquipoTomaTemAmbID != undefined
        && Humedad != "" && EquipoTomaHumedadID > 0 && EquipoTomaHumedadID != undefined
        && PuntoRocio != "" && EquipoTomaPuntoRocioID > 0 && EquipoTomaPuntoRocioID != undefined
        && CampoX != "" && EquipoTomaCampoXID > 0 && EquipoTomaCampoXID != undefined) {

     
        $CondicionesClimatologicas.CondicionesClimatologicas.create(Captura[0], { token: Cookies.get("token"), lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                displayNotify("PinturaCargaGuardar", "", '0');
                if (tipoGuardado == 1) {
                    opcionHabilitarView(true, "FieldSetView");
                }
                else {
                    $("#Cancelar").trigger("click");
                }
            }
        });
    }
    else displayNotify("", "Todos los campos deben ser capturados", '1');
}

function ConsultarExistenciaCondicionesClimatologicas() {
    var FechaToma = $('#inputMedicionesfechaToma').val();
    var HoraToma = $('#inputMedicionesHoraToma').val();
    var PatioID = $('#inputPatio').val();
    var ZonaID = $('#inputZona').val();

    if (FechaToma != "", HoraToma != "", PatioID != "", ZonaID != "") {
        loadingStart();
        $CondicionesClimatologicas.CondicionesClimatologicas.read({ token: Cookies.get("token"), fechatoma: FechaToma, horatoma: HoraToma, patioid: PatioID, zonaid: ZonaID, lenguaje: $("#language").val() }).done(function (data) {
            if (Error(data)) {
                if (data.length > 0) {
                    displayNotify("", "Ya existe informacion para esta fecha, hora, patio y zona por lo tanto se actualizará automaticamente", '1');
                    $('#inputMedicionesTempAmbiente').data("kendoNumericTextBox").value(data[0].TempAmb);
                    $("#inputEquipoTomaTempAmb").data("kendoComboBox").value(data[0].NombreEquipoTemAmb);
                    $('#inputMedicionesHumedad').data("kendoNumericTextBox").value(data[0].Humedad)
                    $("#inputEquipoTomaHumedad").data("kendoComboBox").value(data[0].NombreEquipoHumedad);
                    $('#inputMedicionesPuntoRocio').data("kendoNumericTextBox").value(data[0].PuntoRocio)
                    $("#inputEquipoTomaPtoRocio").data("kendoComboBox").value(data[0].NombreEquipoPuntoRocio);
                    $('#inputMedicionesCampoX').data("kendoNumericTextBox").value(data[0].CampoX)
                    $("#inputEquipoTomaCampoX").data("kendoComboBox").value(data[0].NombreEquipoCampoX);
                }
            }
            loadingStop();
        });
    }
}