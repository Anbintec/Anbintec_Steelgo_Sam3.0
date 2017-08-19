Cookies.set("home", true, { path: '/' });
Cookies.set("navegacion", "53", { path: '/' });


var $UbicacionNumeroUnicoModel = {
	listContainer: {
		create: "",
		list: "",
		detail: "",
		destroy: ""
	},
	properties: {
		inputProyecto: {
			visible: "ProyectoDiv",
			editable: "#inputProyecto",
			required: "#inputProyecto"
		},
		inputRangoNumeroUnico: {
			visible: "RangoNuDiv",
			editable: "#inputRangoNumeroUnico",
			required: "#inputRangoNumeroUnico"
		}
		//,
		//inputRack: {
		//	visible: "RackDiv",
		//	editable: "#inputRack",
		//	required: "#inputRack"
		//}
	}
};