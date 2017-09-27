var transportistaEmb, choferEmb, tractoEmb;
var bandera = false;
var EmbarqueID = 0;
var DestinoGuardado = 0;
var FechaEmbarque;
var esNormal;

function changeLanguageCall() {
    //CargarGrid();
    AjaxCargarProyecto();
    document.title = _dictionary.titleAsignacionEnvio[$("#language").data("kendoDropDownList").value()];
    //FechaEmbarque.data("kendoDatePicker").setOptions({
    //    format: _dictionary.FormatoFecha2[$("#language").data("kendoDropDownList").value()]
    //});
    opcionHabilitarView(false, "FieldSetView");
    SuscribirEventoPopUpGuardarEmbarque();
    CamposBloqueados();
};

function CamposBloqueados() {
    /*READONLY*/
    $("#Proveedor").data("kendoComboBox").enable(false);
    $("#Tracto").data("kendoComboBox").enable(false);
    $("#Chofer").data("kendoComboBox").enable(false);
}

function CargaPopupNuevoProveedor(e) {
    $("#inputNombreNuevoProveedorEnvio").val("");
    windowNewProvider = $("#divNuevoProveedorEnvio").kendoWindow({
        modal: true,
        resizable: false,
        visible: true,
        width: "500px",
        height: "auto",
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        position: {
            top: "1%",
            left: "1%"
        },
        actions:[]
    }).data("kendoWindow");
    $("#divNuevoProveedorEnvio").data("kendoWindow").title(_dictionary.EmbarqueCargaNuevoProveedor[$("#language").data("kendoDropDownList").value()]);
    $("#divNuevoProveedorEnvio").data("kendoWindow").center().open();
    $("#inputNombreNuevoProveedorEnvio").focus();
}

function CargaPopupNuevoTracto(e) {
    $("#inputNombreNuevoTracto").val("");

    windowNewTracto = $("#divNuevoTractoEnvio").kendoWindow({
        modal: true,
        resizable: false,
        visible: true,
        width: "500px",
        height: "auto",
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        position: {
            top: "1%",
            left: "1%"
        },
        actions: []
    }).data("kendoWindow");
    $("#divNuevoTractoEnvio").data("kendoWindow").title(_dictionary.EmbarquePreparacionTractoNuevo[$("#language").data("kendoDropDownList").value()]);
    $("#divNuevoTractoEnvio").data("kendoWindow").center().open();

    $("#inputNombreNuevoTracto").focus();
}

function CargaPopupNuevoChofer(e) {
    $("#inputNombreNuevoChoferEnvio").val("");

    windowNewChofer = $("#divNuevoChoferEnvio").kendoWindow({
        modal: true,
        resizable: false,
        visible: true,
        width: "500px",
        height: "auto",
        draggable: false,
        resizable: false,
        animation: {
            close: false,
            open: false
        },
        position: {
            top: "1%",
            left: "1%"
        },
        actions: []
    }).data("kendoWindow");
    $("#divNuevoChoferEnvio").data("kendoWindow").title(_dictionary.EmbarquePreparacionChoferNuevo[$("#language").data("kendoDropDownList").value()]);
    $("#divNuevoChoferEnvio").data("kendoWindow").center().open();

    $("#inputNombreNuevoChoferEnvio").focus();
}
