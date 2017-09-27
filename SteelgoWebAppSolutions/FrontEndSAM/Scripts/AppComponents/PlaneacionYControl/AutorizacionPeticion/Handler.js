function SuscribirEventos() {
    SuscribirEventoComboBoxPeticiones();
    SuscribirEventoPopUp();
    cambioSumatoria();
}

function SuscribirEventoPopUp() {
    $(document).on('click', '.linkDetalle', function (e) {
        VentanaModal();
    });



    $("#btnGuarda").click(function () {
        $("#windowDetalle").data("kendoWindow").close();
    });

    $("#btnCancela").click(function () {
        $("#windowDetalle").data("kendoWindow").close();
    });

}
function cambioSumatoria() {
    $(".itemSumatoria").change(function (e) {
        $("#total").text(parseInt($("#uno").val()) + parseInt($("#dos").val()) + parseInt($("#tres").val()) +
        parseInt($("#cuatro").val()) + parseInt($("#cinco").val()) + parseInt($("#seis").val()) + "");
    });
}


function VentanaModal() {

    var modalTitle = "";
    modalTitle = _dictionary.CapturaSoldaduraHeaderAdicionales[$("#language").data("kendoDropDownList").value()];
    var window = $("#windowDetalle");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "50%",
        minWidth: 30,
        position: {
            top: "1%",
            left: "1%"
        },
        actions: [
            "Close"
        ],
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();

};


function SuscribirEventoComboBoxPeticiones() {
    var peticiones = [{ Peticion: "Peticion 1", PeticionID: "1" }, { Peticion: "Peticion 2", PeticionID: "2" }]

    $("#inputPeticiones").kendoComboBox({
        dataSource: peticiones,
        dataTextField: "Peticion",
        dataValueField: "PeticionID ",
        suggest: true,
        filter: "contains",
        index: 3
    });
}