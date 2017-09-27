Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10017", { path: '/' });

var $ValidacionResultadosModel = {
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
        },
        inputRequisicion: {
            visible: "#divRequisicion",
            editable: "#inputRequisicion",
            required: "#inputRequisicion",
        }
    }
};