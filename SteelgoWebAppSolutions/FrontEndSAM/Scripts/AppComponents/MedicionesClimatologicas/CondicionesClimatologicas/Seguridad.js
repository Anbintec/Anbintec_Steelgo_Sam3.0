Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10030", { path: '/' });


var $CondicionClimatologica = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        PatioID: {
            visible: "divinputPatio",
            editable: "#inputPatio",
            required: "#inputPatio"
        },
        ZonaID: {
            visible: "divZona",
            editable: "#inputZona",
            required: "#inputZona"
        },
        FechaToma: {
            visible: "divinputMedicionesfechaToma",
            editable: "#inputMedicionesfechaToma",
            required: "#inputMedicionesfechaToma"
        },
        HoraToma: {
            visible: "divinputMedicionesHoraToma",
            editable: "#inputMedicionesHoraToma",
            required: "#inputMedicionesHoraToma"
        },


        TemperaturaAmbiente: {
            visible: "divinputMedicionesTempAmbiente",
            editable: "#inputMedicionesTempAmbiente",
            required: "#inputMedicionesTempAmbiente"
        },

        HerramientaTempAmbienteID: {
            visible: "divinputEquipoTomaTempAmbID",
            editable: "#inputEquipoTomaTempAmb",
            required: "#inputEquipoTomaTempAmb"
        },
        PorcentajeHumedad: {
            visible: "divinputMedicionesHumedad",
            editable: "#inputMedicionesHumedad",
            required: "#inputMedicionesHumedad"
        },
        HerramientaHumedadID: {
            visible: "divinputEquipoTomaHumedadID",
            editable: "#inputEquipoTomaHumedad",
            required: "#inputEquipoTomaHumedad"
        },
        PuntoRocio: {
            visible: "divinputMedicionesPuntoRocio",
            editable: "#inputMedicionesPuntoRocio",
            required: "#inputMedicionesPuntoRocio"
        },
        HerramientaPuntoRocioID: {
            visible: "divinputEquipoTomaPtoRocioID",
            editable: "#inputEquipoTomaPtoRocio",
            required: "#inputEquipoTomaPtoRocio"
        },
        CampoX: {
            visible: "divinputMedicionesCampoX",
            editable: "#inputMedicionesCampoX",
            required: "#inputMedicionesCampoX"
        },
        HerramientaCampoXID: {
            visible: "divinputEquipoTomaCampoXID",
            editable: "#inputEquipoTomaCampoX",
            required: "#inputEquipoTomaCampoX"
        }

    }
};