

function changeLanguageCall() {

    $(".sumaA").change(function () {
        var suma = 0;
        $(".sumaA").each(function (index, elemento) {
            suma += parseInt($(elemento).val(), 10);
        });
        $("#totalA").text(suma);
    });
    $(".sumaB").change(function () {
        var suma = 0;
        $(".sumaB").each(function (index, elemento) {
            suma += parseInt($(elemento).val(), 10);
        });
        $("#totalB").text(suma);
    });
    $(".sumaC").change(function () {
        var suma = 0;
        $(".sumaC").each(function (index, elemento) {
            suma += parseInt($(elemento).val(), 10);
        });
        $("#totalC").text(suma);
    });

}