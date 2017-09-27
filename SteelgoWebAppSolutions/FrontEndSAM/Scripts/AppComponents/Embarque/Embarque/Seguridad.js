Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10020", { path: '/' });

var $EmbarqueModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        Proyecto: {
            visible: "#ProyectoDiv",
            editable: "#Proyecto",
            required: "#Proyecto",
        },
        Proveedor: {
            visible: "#ProveedorDiv",
            editable: "#Proveedor",
            required: "#Proveedor",
        },
        Tracto: {
            visible: "#TractoDiv",
            editable: "#Tracto",
            required: "#Tracto",
        },
        Chofer: {
            visible: "#ChoferDiv",
            editable: "#Chofer",
            required: "#Chofer",
        },
        Embarque: {
            visible: "#EmbarqueDiv",
            editable: "#Embarque",
            required: "#Embarque",
        },
        TractoEnvio: {
            visible: "#TractoEnvioDiv",
            editable: "#TractoEnvio",
            required: "#TractoEnvio",
        },
        ChoferEnvio: {
            visible: "#ChoferEnvioDiv",
            editable: "#ChoferEnvio",
            required: "#ChoferEnvio",
        },
    }
};