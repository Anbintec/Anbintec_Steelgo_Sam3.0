
Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10012", { path: '/' });

var $SoldaduraModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {

        InputOrdenTrabajo: {
            visible: "#containerDiv",
        },
        InputID: {
            visible: "#InputIDDiv",
            editable: "#InputID",
            required: "#InputID",
        },
        Junta: {
            visible: "#JuntaDiv",
            editable: "#Junta",
            required: "#Junta",
        },
        FechaSoldadura: {
            visible: "#AgregadoDiv",
        },
        inputTaller: {
            visible: "#AgregadoDiv",
        },

    }
};