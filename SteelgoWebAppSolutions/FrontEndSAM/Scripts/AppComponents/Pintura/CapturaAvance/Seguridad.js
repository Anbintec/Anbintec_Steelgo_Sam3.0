
Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10028", { path: '/' });

var $CapturaAvanceModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {

        Carro: {
            visible: "#carroDiv",
            editable: "#inputCarro",
            required: "#inputCarro",
        },
        InputID: {
            visible: "#InputDiv",
            editable: "#InputID",
            required: "#InputID",
        },
        Componente: {
            visible: "#ComponenteDiv",
            editable: "#inputComponente",
            required: "#inputComponente",
        },
    }
};

