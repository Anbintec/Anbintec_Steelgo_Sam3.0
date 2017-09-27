function RenderComboBoxPreliminar(container, options) {

$('<input required data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="Nombre" data-bind="value:' + options.field + '"/>')
    .appendTo(container)
    .kendoComboBox({
        autoBind: false,
        dataSource: [{ PreliminarID: 0, Nombre: '' }, { PreliminarID: 1, Nombre: 'Aprobado' }, { PreliminarID: 2, Nombre: 'Rechazado' }, { PreliminarID: 3, Nombre: 'Retoma' }],
        dataTextField: "Nombre",
        dataValueField: "PreliminarID",
        template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
        change: function (e) {
            dataItem = this.dataItem(e.sender.selectedIndex);

            if (dataItem != undefined) {
                options.model.PreliminarID = dataItem.PreliminarID;
                options.model.Preliminar = dataItem.Nombre;
            }
            else {
                options.model.PreliminarID = 0;
                options.model.Preliminar = '';
            }

        }
    }
);
$(".k-combobox").parent().on('mouseleave', function (send) {
    var e = $.Event("keydown", { keyCode: 27 });
    var item = $(this).find(".k-combobox")[0];
    if (item != undefined) {
        if (!tieneClase(item)) {
            $(container).trigger(e);
        }
    }
});
}

function tieneClase(item) {
    for (var i = 0; i < item.classList.length; i++) {
        if (item.classList[i] == "k-state-border-up" || item.classList[i] == "k-state-border-down") {
            return true;
        }
    }
    return false
}