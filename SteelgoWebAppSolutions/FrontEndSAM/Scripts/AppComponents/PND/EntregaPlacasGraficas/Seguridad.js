Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "54", { path: '/' });

var $EntregaPlacasGraficasModel = {
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
            visible: "#divPrueba ",
            editable: "#inputTipoPrueba",
            required: "#inputTipoPrueba",
        },
        inputProveedor: {
            visible: "#divProveedor ",
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