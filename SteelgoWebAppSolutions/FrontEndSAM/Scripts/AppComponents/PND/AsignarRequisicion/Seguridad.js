Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10042", { path: '/' });

var $AsignarRequisicionModel = {
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
            editable: "#inputPrueba",
            required: "#inputPrueba",
        }
    }
};