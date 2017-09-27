Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10026", { path: '/' });

var $VisualDimensionalModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {

        InputID: {
            visible: "#InputIDDiv",
            editable: "#InputID",
            required: "#InputIDDiv",
        },
        inputDefecto: {
            visible: "#inputDefectoDiv",
            editable: "#inputDefecto",
        },
        inputInspector: {
            visible: "#inputInspectorDiv",
            editable: "#inputInspector",
            required: "#inputInspectorDiv"
        },
        FechaInspeccion: {
            visible: "#FechaInspeccionDiv",
            editable: "#FechaInspeccion",
            required: "#FechaInspeccionDiv"
        },
        inputTaller: {
            visible: "#inputTallerDiv",
            editable: "#inputTaller",
        },
        inputDefectosVisual: {
            visible: "#inputDefectosVisualDiv",
            editable: "#inputDefectosVisual",
        },
        inputInspectorVisual: {
            visible: "#inputInspectorVisualDiv",
            editable: "#inputInspectorVisual",
        },
        inputFechaVisual: {
            visible: "#inputFechaVisualDiv",
            editable: "#inputFechaVisual",
        },
    }
};