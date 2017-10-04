Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10033", { path: '/' });

var $SistemaPinturaAplicableModel = {
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
        inputSpool: {
            visible: "#divSpool",
            editable: "#inputSpool",
            required: "#inputSpool",
        },
        inputNumeroControl: {
            visible: "#divNc",
            editable: "#inputNc",
            required: "#inputNc",
        }
    }
};