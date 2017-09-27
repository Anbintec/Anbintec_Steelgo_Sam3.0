Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10027", { path: '/' });

var $RevisionEmbarqueModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        Proyecto: {
            visible: "#divProyecto",
            editable: "#Proyecto",
            required: "#Proyecto",
        },
        Embarque: {
            visible: "#divEmbarque",
            editable: "#Embarque",
            required: "#Embarque",
        }
    }
};