function SuscribirEventos() {
    SuscribirEventoComboBoxPeticiones();
}

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