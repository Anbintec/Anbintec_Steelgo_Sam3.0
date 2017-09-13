function AjaxObtenerProyectos() {
    $Proyecto.Proyecto.read({ token: Cookies.get("token") }).done(function (result) {
        ControlErroresObjetosComboBox("inputProyecto", result);
    });
}

function AjaxPlancharCampos(data) {
    var Captura = [];    
    Captura[0] = { Detalle: "" };
    Captura[0].Detalle = JSON.stringify(data);

        var proyecto = $("#inputProyecto").data("kendoComboBox").value();
    if (proyecto != "0" && proyecto != undefined) {
        $PlanchadoMaterial.PlanchadoMaterial.create(Captura[0], { token: Cookies.get("token"), ProyectoID: proyecto }).done(function (data) {
            if (Error(data)) {
                download(data, "ResultadoPlanchadoMateriales.csv", "text/csv");                                
                displayMessage("", "Por favor revisar el resultado del planchado", "0");
                RestablecerCampos();
            }
        });
    }
}

function RestablecerCampos() {    
    $("#inputProyecto").data("kendoComboBox").value("");
    $("input[type=checkbox]").prop("checked", false);
    $("#lblArchivo").html("");
}


function ControlErroresObjetosComboBox(control, result) {
    if (Error(result)) {
        $("#" + control).data("kendoComboBox").dataSource.data(result);
    } else {
        $("#" + control).data("kendoComboBox").dataSource.data([]);
    };
}

function download(strData, strFileName, strMimeType) {
    var D = document,
        a = D.createElement("a");
    strMimeType = strMimeType || "application/octet-stream";


    if (navigator.msSaveBlob) { // IE10
        return navigator.msSaveBlob(new Blob([strData], { type: strMimeType }), strFileName);
    } /* end if(navigator.msSaveBlob) */


    if ('download' in a) { //html5 A[download]
        a.href = "data:" + strMimeType + "," + encodeURIComponent(strData);
        a.setAttribute("download", strFileName);
        a.innerHTML = "downloading...";
        D.body.appendChild(a);
        setTimeout(function () {
            a.click();
            D.body.removeChild(a);
        }, 66);
        return true;
    } /* end if('download' in a) */


    //do iframe dataURL download (old ch+FF):
    var f = D.createElement("iframe");
    D.body.appendChild(f);
    f.src = "data:" + strMimeType + "," + encodeURIComponent(strData);

    setTimeout(function () {
        D.body.removeChild(f);
    }, 333);
    return true;
}