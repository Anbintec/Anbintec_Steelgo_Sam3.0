Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "55", { path: '/' });

var $EditarRequisicionModel = {
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
            editable: "#inputTipoPrueba",
            required: "#inputTipoPrueba",
        },
        inputRequisicion: {
            visible: "#divRequisicion",
            editable: "#inputRequisicion",
            required: "#inputRequisicion",
        }
    }
};