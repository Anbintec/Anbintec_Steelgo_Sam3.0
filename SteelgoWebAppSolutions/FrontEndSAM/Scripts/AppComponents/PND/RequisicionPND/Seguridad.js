Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10016", { path: '/' });

var $GenerarRequisicion = {
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
            visible: "#divTipoPrueba",
            editable: "#inputTipoPrueba",
            required: "#inputTipoPrueba",
        }
    }
};