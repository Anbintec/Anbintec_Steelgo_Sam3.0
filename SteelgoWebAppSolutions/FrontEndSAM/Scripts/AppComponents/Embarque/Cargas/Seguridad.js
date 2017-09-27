Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10040", { path: '/' });

var $CargaPlanaModel = {
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
        inputProveedor: {
            visible: "#divProveedor",
            editable: "#inputProveedor",
            required: "#inputProveedor",
        },
        inputPlana: {
            visible: "#divPlana",
            editable: "#inputEmbarqueCargaPLacaPlana",
            required: "#inputEmbarqueCargaPLacaPlana",
        }
    }
};