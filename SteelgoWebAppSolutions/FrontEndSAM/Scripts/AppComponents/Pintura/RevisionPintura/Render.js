function renderComentario(container, options) {
    var dataItem;

    $('<input required data-text-field="Rechazo" id=' + options.model.uid + ' data-value-field="TipoRechazoID" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoComboBox({
            autoBind: false,
            dataSource: options.model.ListaMotivosRechazo,
            dataTextField: "Rechazo",
            dataValueField: "TipoRechazoID",
            template: "<i class=\"fa fa-#=data.Rechazo#\"></i> #=data.Rechazo#",
            change: function (e) {
                dataItem = this.dataItem(e.sender.selectedIndex);
                if (dataItem != undefined && dataItem.TipoRechazoID != undefined) {
                    options.model.Comentario = dataItem.Rechazo;
                    options.model.ComentarioID = dataItem.TipoRechazoID;
                }
                $("#grid").data("kendoGrid").refresh();
            }
        });
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

function renderSistemaPintura(container, options) {

    var dataItem;
	
    $('<input required data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="SistemaPinturaID" data-bind="value:' + options.field + '"/>')
			.appendTo(container)
			.kendoComboBox({
				autoBind: false,
				dataSource: options.model.ListadoSistemaPinturaPorProyecto,
				dataTextField: "Nombre",
				dataValueField: "SistemaPinturaID",
				template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
				change: function (e) {

					dataItem = this.dataItem(e.sender.selectedIndex);
					if (dataItem != undefined && dataItem.SistemaPinturaID != undefined) {
						options.model.SistemaPinturaID = dataItem.SistemaPinturaID;
						options.model.SistemaPintura = dataItem.Nombre;
						options.model.NoPintable = dataItem.NoPintable;


						options.model.ListaColorPintura = [];
						options.model.SistemaPinturaColorID = 0;
						options.model.Color = "";


						if (dataItem.SistemaPinturaID != 0 && !dataItem.NoPintable) {
						    AjaxCargarColorPinturaRender(dataItem.SistemaPinturaID, options);
						}
						else
							$("#grid").data("kendoGrid").refresh();
					}

				}
			});
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

function renderColor(container, options) {
    var dataItem;
	
		$('<input required data-text-field="Nombre" id=' + options.model.uid + ' data-value-field="SistemaPinturaColorID" data-bind="value:' + options.field + '"/>')
			.appendTo(container)
			.kendoComboBox({
				autoBind: false,
				dataSource: options.model.ListaColorPintura,
				dataTextField: "Nombre",
				dataValueField: "SistemaPinturaColorID",
				template: "<i class=\"fa fa-#=data.Nombre#\"></i> #=data.Nombre#",
				change: function (e) {

					dataItem = this.dataItem(e.sender.selectedIndex);
					if (dataItem != undefined && dataItem.SistemaPinturaColorID != undefined) {
						options.model.SistemaPinturaColorID = dataItem.SistemaPinturaColorID;
						options.model.Color = dataItem.Nombre;
						$("#grid").data("kendoGrid").refresh();
					}

				}
			});
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