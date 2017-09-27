Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10043", { path: '/' });

var $ImpresionResultadosModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        inputProyecto: {
            visible: "#divProyecto",
            editable: "#inputProyecto",
            required: "#inputProyecto",
        },
        inputTipoPrueba: {
            visible: "#divPrueba",
            editable: "#inputPrueba",
            required: "#inputPrueba",
        },
        inputProveedor: {
            visible: "#divProveedor",
            editable: "#inputProveedor",
            required: "#inputProveedor",
        }
    }
};