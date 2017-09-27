
Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10007", { path: '/' });

var $NuevoPQRModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        Nombre: {
            visible: "#PQRNombreDiv",
            editable: "#NombreId",
            required: "#NombreId",
        },
       
        EspesorRelleno: {
            visible: "#PQREspesoRellenorDiV",
            editable: "#EspesorRelleno",
            required: "#EspesorRelleno",
        },
        EspesorRaiz: {
            visible: "#PQREspesorRaizDiV",
            editable: "#EspesorRaiz",
            required: "#EspesorRaiz",
        },
        ProcesoSoldaduraRelleno: {
            visible: "#PQRProcesoSoldaduraRellenoDiv",
            editable: "#ProcesoSoldaduraRellenoID",
            required: "#ProcesoSoldaduraRellenoID",
        },
        ProcesoSoldaduraRaiz: {
            visible: "#PQRProcesoSoldaduraRaizDiv",
            editable: "#ProcesoSoldaduraRaizID",
            required: "#ProcesoSoldaduraRaizID"
        },
        GrupoPMaterialBase1: {
            visible: "#PQRGrupoPMaterialBase1Div",
            editable: "#GrupoPMaterialBase1ID",
            required: "#GrupoPMaterialBase1ID",
        },
        GrupoPMaterialBase2: {
            visible: "#PQRGrupoPMaterialBase2Div",
            editable: "#GrupoPMaterialBase2ID",
            required: "#GrupoPMaterialBase2ID",
        },
        Codigo: {
            visible: "#PQRCodigoDiv",
            editable: "#CodigoID",
            required: "#CodigoID",
        },
        //TipoPrueba: {
        //    visible: "#TipoPruebaDiV",
        //    editable: "#TipoPrueba",
        //    required: "#TipoPrueba",
        //},
        //TipoJunta: {
        //    visible: "#TipoJuntaDiV",
        //    editable: "#TipoJunta",
        //    required: "#TipoJunta",
        //}

    }
};
