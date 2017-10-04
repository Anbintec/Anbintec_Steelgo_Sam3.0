Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10034", { path: '/' });


var $SistemaPinturaModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {

        inputNombre: {
            visible: "#inputNombreDiv",
            editable: "#inputNombre",
            required: "#inputNombre",
        },
        inputColor: {
            visible: "#inputColorDiv",
            editable: "#inputColor",
            required: "#inputColor",
        },
        inputProyecto: {
            visible: "#divMultiselectProyecto",
            editable: "#inputProyecto",
            required: "#inputProyecto",
        },
        comboProyecto: {
            visible: "#divComboProyecto",
            editable: "#comboProyecto",
            required: "#comboProyecto",
        },
        inputNoAplicable: {
            visible: "#inputNoAplicableDiv",
            editable: "#inputNoAplicable",
        },
    }
};
