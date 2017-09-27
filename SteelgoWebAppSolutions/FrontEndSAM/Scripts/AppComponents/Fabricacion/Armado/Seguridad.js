
Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10013", { path: '/' });

var $ArmadoModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {

        InputOrdenTrabajo: {
            visible: "#InputOrdenTrabajoDiv",
            editable: "#InputOrdenTrabajo",
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
        FechaArmado: {
            visible: "#FechaArmadoDiv",
            editable: "#FechaArmado",
        },
        inputTaller: {
            visible: "#inputTallerDiv",
            editable: "#inputTaller",
        },
        inputTubero: {
            visible: "#inputTuberoDiv",
            editable: "#inputTubero",
        },
    }
};
