function changeLanguageCall() {
    document.title = _dictionary.NuevoPQRBreadcrumb[$("#language").data("kendoDropDownList").value()];
    Limpiar();
    setTimeout(function () { ObtenerListasPQR() }, 500);
}

function CargaInicial() {
    ConvertirCombos();
    
};

CargaInicial();

function ConvertirCombos() {

    $("#EspesorRelleno").kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });

    $("#EspesorRaiz").kendoNumericTextBox({
        format: "#",
        decimals: 0,
        min: 0
    });

    $("#ProcesoSoldaduraRellenoID").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "Codigo",
        dataValueField: "ProcesoSoldaduraID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
        },
        change: function (e) {
            if ($('#ProcesoSoldaduraRellenoID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()) != undefined && $('#ProcesoSoldaduraRellenoID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()).Codigo == "N/A") {
                $('#EspesorRelleno').data("kendoNumericTextBox").value("0");
                $("#EspesorRelleno").data("kendoNumericTextBox").readonly(true);
                $("#EspesorRelleno").data("kendoNumericTextBox").enable(false);
                $("#EspesorRelleno").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();

            } else {
                $("#EspesorRelleno").data("kendoNumericTextBox").readonly(false);
                $("#EspesorRelleno").data("kendoNumericTextBox").enable(true);
                $("#EspesorRelleno").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
            }

            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#ProcesoSoldaduraRellenoID").data("kendoComboBox").value("");
            }
        }
    });

    $("#ProcesoSoldaduraRellenoID").blur(function (e) {
        if ($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").dataItem($("#ProcesoSoldaduraRellenoID").data("kendoComboBox").select()) == undefined) {
            $("#ProcesoSoldaduraRellenoID").val("");
            $("#ProcesoSoldaduraRellenoID").data("kendoComboBox").text("");
        }
    });

    $("#ProcesoSoldaduraRaizID").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "Codigo",
        dataValueField: "ProcesoSoldaduraID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
        },
        change: function (e) {
            if ($('#ProcesoSoldaduraRaizID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()) != undefined && $('#ProcesoSoldaduraRaizID').data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()).Codigo == "N/A") {
                $('#EspesorRaiz').data("kendoNumericTextBox").value("0");
                $("#EspesorRaiz").data("kendoNumericTextBox").readonly(true);
                $("#EspesorRaiz").data("kendoNumericTextBox").enable(false);
                $("#EspesorRaiz").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").hide();
            } else {
                $("#EspesorRaiz").data("kendoNumericTextBox").wrapper.find(".k-numeric-wrap").find(".k-select").show();
                $("#EspesorRaiz").data("kendoNumericTextBox").readonly(false);
                $("#EspesorRaiz").data("kendoNumericTextBox").enable(true);
            }
            dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem == undefined) {
                $("#ProcesoSoldaduraRaizID").data("kendoComboBox").value("");
            }
        }
    });

    $("#ProcesoSoldaduraRaizID").blur(function (e) {
        if ($("#ProcesoSoldaduraRaizID").data("kendoComboBox").dataItem($("#ProcesoSoldaduraRaizID").data("kendoComboBox").select()) == undefined) {
            $("#ProcesoSoldaduraRaizID").val("");
            $("#ProcesoSoldaduraRaizID").data("kendoComboBox").text("");
        }
    });

    $("#GrupoPMaterialBase1ID").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "GrupoP",
        dataValueField: "GrupoPID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
            var PQRIDBuscar = dataItem.PQRID;
        },
        change: function (e) {
            if (tieneClase(e.currentTarget)) {
                $("#GrupoPMaterialBase1ID").data("kendoComboBox").select(0);
            }
            if ($('#GrupoPMaterialBase1ID').data("kendoComboBox").dataItem($("#GrupoPMaterialBase1ID").data("kendoComboBox").select()) == undefined) {
                $("#GrupoPMaterialBase1ID").data("kendoComboBox").value("");
            }
        }
    });

    $('#GrupoPMaterialBase1ID').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 9)
            if (tieneClase(e.currentTarget)) {
                $("#GrupoPMaterialBase1ID").data("kendoComboBox").select(0);
            }
    });

    $("#GrupoPMaterialBase1ID").blur(function (e) {
        if (tieneClase(e.currentTarget)) {
            $("#GrupoPMaterialBase1ID").data("kendoComboBox").select(0);
        }
        if ($("#GrupoPMaterialBase1ID").data("kendoComboBox").dataItem($("#GrupoPMaterialBase1ID").data("kendoComboBox").select()) == undefined) {
            $("#GrupoPMaterialBase1ID").val("");
            $("#GrupoPMaterialBase1ID").data("kendoComboBox").text("");
        }
    });

    $("#GrupoPMaterialBase2ID").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "GrupoP",
        dataValueField: "GrupoPID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
            var PQRIDBuscar = dataItem.PQRID;
        }, change: function (e) {
            if (tieneClase(e.currentTarget)) {
                $("#GrupoPMaterialBase2ID").data("kendoComboBox").select(0);
            }
            if ($('#GrupoPMaterialBase2ID').data("kendoComboBox").dataItem($("#GrupoPMaterialBase2ID").data("kendoComboBox").select()) == undefined) {
                $("#GrupoPMaterialBase2ID").data("kendoComboBox").value("");
            }
        }
    });

    $('#GrupoPMaterialBase2ID').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 9)
            if (tieneClase(e.currentTarget)) {
                $("#GrupoPMaterialBase2ID").data("kendoComboBox").select(0);
            }
    });

    $("#GrupoPMaterialBase2ID").blur(function (e) {
        if (tieneClase(e.currentTarget)) {
            $("#GrupoPMaterialBase1ID").data("kendoComboBox").select(0);
        }
        if ($("#GrupoPMaterialBase2ID").data("kendoComboBox").dataItem($("#GrupoPMaterialBase2ID").data("kendoComboBox").select()) == undefined) {
            $("#GrupoPMaterialBase2ID").val("");
            $("#GrupoPMaterialBase2ID").data("kendoComboBox").text("");
        }
    });

    $("#CodigoID").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "Especificacion",
        dataValueField: "CodigoAsmeID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
        }, change: function (e) {
            if (tieneClase(e.currentTarget)) {
                $("#CodigoID").data("kendoComboBox").select(0);
            }
            if ($('#CodigoID').data("kendoComboBox").dataItem($("#CodigoID").data("kendoComboBox").select()) == undefined) {
                $("#CodigoID").data("kendoComboBox").value("");
            }
        }
    });

    $("#CodigoID").blur(function (e) {
        if ($("#CodigoID").data("kendoComboBox").dataItem($("#CodigoID").data("kendoComboBox").select()) == undefined) {
            $("#CodigoID").val("");
            $("#CodigoID").data("kendoComboBox").text("");
        }
    });

    $('#CodigoID').closest('.k-widget').keydown(function (e) {
        if (e.keyCode == 9)
            if (tieneClase(e.currentTarget)) {
                $("#CodigoID").data("kendoComboBox").select(0);
            }
    });

    $("#TipoJunta").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "Nombre",
        dataValueField: "TipoJuntaID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
            var PQRIDBuscar = dataItem.PQRID;
        },
        change: function (e) {
            if (tieneClase(e.currentTarget)) {
                $("#TipoJunta").data("kendoComboBox").select(0);
            }
            if ($('#TipoJunta').data("kendoComboBox").dataItem($("#TipoJunta").data("kendoComboBox").select()) == undefined) {
                $("#TipoJunta").data("kendoComboBox").value("");
            }
        }
    });

    $("#TipoPrueba").kendoComboBox({
        suggest: true,
        delay: 10,
        filter: "contains",
        autoBind: false,
        dataTextField: "TipoDePrueba",
        dataValueField: "TipoPruebaID",
        select: function (e) {
            dataItem = this.dataItem(e.item.index());
            var PQRIDBuscar = dataItem.PQRID;
        },
        change: function (e) {
            if (tieneClase(e.currentTarget)) {
                $("#TipoPrueba").data("kendoComboBox").select(0);
            }
            if ($('#TipoPrueba').data("kendoComboBox").dataItem($("#TipoPrueba").data("kendoComboBox").select()) == undefined) {
                $("#TipoPrueba").data("kendoComboBox").value("");
            }
        }
    });


};

function Limpiar() {
    $('#NombreId').val("");
    document.getElementById("chkPreheat").checked = false;
    document.getElementById("chkPwht").checked = false;
    $('#EspesorRelleno').data("kendoNumericTextBox").value("");
    $('#EspesorRaiz').data("kendoNumericTextBox").value("");
    $('#ProcesoSoldaduraRellenoID').data("kendoComboBox").value("");
    $('#ProcesoSoldaduraRaizID').data("kendoComboBox").value("");
    $('#GrupoPMaterialBase1ID').data("kendoComboBox").value("");
    $('#GrupoPMaterialBase2ID').data("kendoComboBox").value("");
    $('#AporteID').val("");
    $('#MezclaID').val("");
    $('#RespaldoID').val("");
    $('#GrupoFID').val("");
    $('#CodigoID').data("kendoComboBox").value("");
    $("#PQRID").val("0");
    opcionHabilitarView(false, "FieldSetView");
    loadingStop();
};

function tieneClase(item) {

    var tieneClass = $(item).hasClass("k-state-border-up") || $(item).hasClass("k-state-border-down");
    return tieneClass;
}