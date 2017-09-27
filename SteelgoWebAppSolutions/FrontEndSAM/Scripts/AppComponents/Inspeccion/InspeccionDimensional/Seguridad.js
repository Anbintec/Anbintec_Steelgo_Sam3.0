
Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10008", { path: '/' });

var $DimensionalModel = {
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
            required: "#InputID",
        },
        inputDefecto: {
            visible: "#inputDefectoDiv",
            editable: "#inputDefecto",
            required: "#inputDefecto"
        },
        inputInspector: {
            visible: "#inputInspectorDiv",
            editable: "#inputInspector",
            required: "#inputInspector"
        },
        FechaInspeccion: {
            visible: "#FechaInspeccionDiv",
            editable: "#FechaInspeccion",
            required: "#FechaInspeccion"
        },

    }
};
