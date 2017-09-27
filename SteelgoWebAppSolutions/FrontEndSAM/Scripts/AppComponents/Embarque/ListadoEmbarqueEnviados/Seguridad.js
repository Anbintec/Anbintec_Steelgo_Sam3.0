Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "11046", { path: '/' });

var $ListadoEmbarqueEnviadosModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        Proyecto: {
            visible: "#divProyecto",
            editable: "#InputProyecto",
            required: "#InputProyecto",
        }
    }
};