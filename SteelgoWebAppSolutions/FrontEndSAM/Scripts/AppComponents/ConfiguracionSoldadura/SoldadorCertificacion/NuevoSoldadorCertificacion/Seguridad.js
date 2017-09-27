Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10010", { path: '/' });


var $SoldadorCertificacionSaveModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        CodigoObrero: {
            visible: "#SCSoldadorDiv",
            editable: "#inputSoldador",
            required: "#inputSoldador"
        },
        NombreWPS: {
            visible: "#SCNombreWPSDiv",
            editable: "#inputNombreWPS",
            required: "#inputNombreWPS"
        },
        FechaInicioCertificado: {
            visible: "#SCFechaInicioDiV",
            editable: "#inputFechaInicioCertificado",
            required: "#inputFechaInicioCertificado"
        },
        FechaFinCertificado: {
            visible: "#SCFechaFinDiV",
            editable: "#inputFechaFinCertificado",
            required: "#inputFechaFinCertificado"
        },
        EspesorMinimo: {
            visible: "#SCEspesorMinimoDiv",
            editable: "#inputEspesorMinimo",
            required: "#inputEspesorMinimo"
        },
        EspesorMaximo: {
            visible: "#SCEspesorMaximoDiv",
            editable: "#inputEspesorMaximo",
            required: "#inputEspesorMaximo"
        },
        DiametroCalificado: {
            visible: "#SCDiametroCalificadoDiv",
            editable: "#inputDiametroCalificado",
            required: "#inputDiametroCalificado"
        },
        CedulaTubo: {
            visible: "#SCCedulaDiv",
            editable: "#inputCedulaTuboPQR",
            required: "#inputCedulaTuboPQR"
        },
        ProcesoSoldadura: {
            visible: "#SCProcesoSoldaduraDiv",
            editable: "#inputProcesoSol",
            required: "#inputProcesoSol"
        },
        Posicion: {
            visible: "#SCPosicionDiv",
            editable: "#inputPosicionPQR",
            required: "#inputPosicionPQR"
        },
        TipoDePrueba: {
            visible: "#SCTipoPruebaDiv",
            editable: "#inputTipoPrueba",
            required: "#inputTipoPrueba"
        },
        PasosSoldadura: {
            visible: "#SCPasosSoldaduraDiv",
            editable: "#inputPasosSoldadura",
            required: "#inputPasosSoldadura"
        },
    }
};