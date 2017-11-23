function changeLanguageCall() {
    setTimeout(function () { AjaxCargarCamposPredeterminados() }, 1000);
    setTimeout(function () { AjaxCargarEquiposToma() }, 1100);

    
    document.title = "Condiciones Climatologicas";
};

function ValidarFecha(valor) {
    var fecha = kendo.toString(valor, String(_dictionary.FormatoFecha[$("#language").data("kendoDropDownList").value()].replace('{', '').replace('}', '').replace("0:", "")));
    if (fecha == null) {
        $("#inputMedicionesfechaToma").data("kendoDatePicker").value('');
        return false;
    }
    return true;
}