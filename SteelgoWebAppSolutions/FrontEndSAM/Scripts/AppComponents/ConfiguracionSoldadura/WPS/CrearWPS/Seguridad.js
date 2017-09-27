Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "10015", { path: '/' });


var $WPSSaveModel = {
    listContainer: {
        create: "",
        list: "",
        detail: "",
        destroy: ""
    },
    properties: {
        NombreWPS: {
            visible: "#DivNombreWPS",
            editable: "#NomnreWPS",
            required: "#NomnreWPS"
        },
        PQRRaiz: {
            visible: "#DivNombrePQRRaiz",
            editable: "#PQRRaizNombre",
            required: "#PQRRaizNombre"
        },
        PQRRelleno: {
            visible: "#DivNombrePQRRelleno",
            editable: "#PQRRellenoNombre",
            required: "#PQRRellenoNombre"
        }
    }
};