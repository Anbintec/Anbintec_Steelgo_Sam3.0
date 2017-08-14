/* FlatColorPicker messages */

if (kendo.ui.FlatColorPicker) {
    kendo.ui.FlatColorPicker.prototype.options.messages =
    $.extend(true, kendo.ui.FlatColorPicker.prototype.options.messages, {
        "apply": "Aplicar",
        "cancel": "Cancelar"
    });
}

/* ColorPicker messages */

if (kendo.ui.ColorPicker) {
    kendo.ui.ColorPicker.prototype.options.messages =
    $.extend(true, kendo.ui.ColorPicker.prototype.options.messages, {
        "apply": "Aplicar",
        "cancel": "Cancelar"
    });
}

/* ColumnMenu messages */

if (kendo.ui.ColumnMenu) {
    kendo.ui.ColumnMenu.prototype.options.messages =
    $.extend(true, kendo.ui.ColumnMenu.prototype.options.messages, {
        "sortAscending": "Ordenar ascendente",
        "sortDescending": "Ordenar descendiente",
        "filter": "Filtro",
        "columns": "Columnas",
        "done": "Hecho",
        "settings": "Ajustes de columna",
        "lock": "Bloquear",
        "unlock": "Desbloquear"
    });
}

/* Editor messages */

if (kendo.ui.Editor) {
    kendo.ui.Editor.prototype.options.messages =
    $.extend(true, kendo.ui.Editor.prototype.options.messages, {
        "bold": "Negritas",
        "italic": "Cursiva",
        "underline": "Subrayado",
        "strikethrough": "Tachado",
        "superscript": "Superíndice",
        "subscript": "Subíndice",
        "justifyCenter": "Centrar",
        "justifyLeft": "Alinear a la izquierda",
        "justifyRight": "Alinear a la derecha",
        "justifyFull": "Justificar",
        "insertUnorderedList": "Insertar lista desordenada",
        "insertOrderedList": "Insertar lista ordenada",
        "indent": "Sangría",
        "outdent": "Quitar sangría",
        "createLink": "Insertar hipervínculo",
        "unlink": "Quitar hipervínculo",
        "insertImage": "Insertar imagen",
        "insertFile": "Insertar archivo",
        "insertHtml": "Insertar HTML",
        "viewHtml": "Ver HTML",
        "fontName": "Seleccionar fuente",
        "fontNameInherit": "(fuente heredada)",
        "fontSize": "Seleccionar tamaño de fuente",
        "fontSizeInherit": "(tamaño heredado)",
        "formatBlock": "Formato",
        "formatting": "Formato",
        "foreColor": "Color",
        "backColor": "Color de fondo",
        "style": "Estilos",
        "emptyFolder": "Carpeta vacía",
        "uploadFile": "Cargar",
        "orderBy": "Ordenar por:",
        "orderBySize": "Tamaño",
        "orderByName": "Nombre",
        "invalidFileType": "El archivo seleccionado\"{0}\" no es válido. Los archivos soportados son {1}.",
        "deleteFile": '¿Estás seguro de que quieres eliminar "{0}"?',
        "overwriteFile": 'Un archivo con el nombre "{0}" ya existe en la carpeta actual. ¿Quieres sobreescribirlo?',
        "directoryNotFound": "No se encontro un directorio con ese nombre.",
        "imageWebAddress": "Dirección web",
        "imageAltText": "Texto alternativo",
        "imageWidth": "Ancho (px)",
        "imageHeight": "Alto (px)",
        "fileWebAddress": "Dirección web",
        "fileTitle": "Título",
        "linkWebAddress": "Dirección web",
        "linkText": "Texto",
        "linkToolTip": "Consejo",
        "linkOpenInNewWindow": "Abrir en una nueva ventana",
        "dialogUpdate": "Actualizar",
        "dialogInsert": "Insertar",
        "dialogButtonSeparator": "ó",
        "dialogCancel": "Cancela",
        "createTable": "Crear tabla",
        "addColumnLeft": "Añadir columna a la izquierda",
        "addColumnRight": "Añadir columna a la derecha",
        "addRowAbove": "Añadir fila arriba",
        "addRowBelow": "Añadir fila abajo",
        "deleteRow": "Eliminar fila",
        "deleteColumn": "Eliminar columna"
    });
}

/* FileBrowser messages */

if (kendo.ui.FileBrowser) {
    kendo.ui.FileBrowser.prototype.options.messages =
    $.extend(true, kendo.ui.FileBrowser.prototype.options.messages, {
        "uploadFile": "Subir",
        "orderBy": "Ordenar por",
        "orderByName": "Nombre",
        "orderBySize": "Tamaño",
        "directoryNotFound": "No se encontró un directorio con ese nombre.",
        "emptyFolder": "Carpeta vacía",
        "deleteFile": '¿Estás seguro que quieres eliminar "{0}"?',
        "invalidFileType": "El archivo seleccionado\"{0}\" no es válido. Los archivos soportados son {1}.",
        "overwriteFile": 'Un archivo con el nombre "{0}" ya existe en la carpeta actual. ¿Quieres sobreescribirlo?',
        "dropFilesHere": "Arrastra un archivo aqui para subirlo",
        "search": "Buscar"
    });
}

/* FilterMenu messages */

if (kendo.ui.FilterMenu) {
    kendo.ui.FilterMenu.prototype.options.messages =
    $.extend(true, kendo.ui.FilterMenu.prototype.options.messages, {
        "info": "Mostrar elementos con los valores:",
        "isTrue": "Es Verdadero",
        "isFalse": "Es Falso",
        "filter": "Filtro",
        "clear": "Limpiar",
        "and": "Y",
        "or": "O",
        "selectValue": "-Seleccionar Valor-",
        "operator": "Operador",
        "value": "Valor",
        "cancel": "Cancelar"
    });
}

/* FilterMenu operator messages */

if (kendo.ui.FilterMenu) {
    kendo.ui.FilterMenu.prototype.options.operators =
    $.extend(true, kendo.ui.FilterMenu.prototype.options.operators, {
        "string": {
            "eq": "Es igual a",
            "neq": "Es diferente de",
            "startswith": "Empieza con",
            "contains": "Contiene",
            "doesnotcontain": "No Contiene",
            "endswith": "termina Con"
        },
        "number": {
            "eq": "Es igual a",
            "neq": "Es diferente de",
            "gte": "Es mayor o igual que",
            "gt": "Es mayor que",
            "lte": "Es menor o igual que",
            "lt": "Es menor que"
        },
        "date": {
            "eq": "Es igual a",
            "neq": "Es diferente de",
            "gte": "Esta antes o es igual a",
            "gt": "Esta antes de",
            "lte": "Esta despues o es igual a",
            "lt": "Esta despues de"
        },
        "enums": {
            "eq": "Es igual a",
            "neq": "Es diferente de"
        }
    });
}

/* Gantt messages */

if (kendo.ui.Gantt) {
    kendo.ui.Gantt.prototype.options.messages =
    $.extend(true, kendo.ui.Gantt.prototype.options.messages, {
        "views": {
            "day": "Día",
            "week": "Semana",
            "month": "Mes"
        },
        "actions": {
            "append": "Añadir tarea",
            "addChild": "Añadir hijo",
            "insertBefore": "Añadir arriba",
            "insertAfter": "Añadir abajo"
        }
    });
}

/* Grid messages */

if (kendo.ui.Grid) {
    kendo.ui.Grid.prototype.options.messages =
    $.extend(true, kendo.ui.Grid.prototype.options.messages, {
        "commands": {
            "cancel": "Cancelar Cambios",
            "canceledit": "Cancelar",
            "create": "Agregar Nuevo Registro",
            "destroy": "Borrar",
            "edit": "Editar",
            "save": "Salvar Cambios",
            "select": "Seleccionar",
            "update": "Editar"
        },
        "editable": {
            "cancelDelete": "Cancelar",
            "confirmation": "Estas seguro de querer borrar este registro?",
            "confirmDelete": "Borrar"
        }
    });
}

/* Groupable messages */

if (kendo.ui.Groupable) {
    kendo.ui.Groupable.prototype.options.messages =
    $.extend(true, kendo.ui.Groupable.prototype.options.messages, {
        "empty": "Arrastra un encabezado de columna y sueltalo aquí para agrupar por esa columna"
    });
}

/* NumericTextBox messages */

if (kendo.ui.NumericTextBox) {
    kendo.ui.NumericTextBox.prototype.options =
    $.extend(true, kendo.ui.NumericTextBox.prototype.options, {
        "upArrowText": "Aumentar valor",
        "downArrowText": "Disminuir valor"
    });
}

/* Pager messages */

if (kendo.ui.Pager) {
    kendo.ui.Pager.prototype.options.messages =
    $.extend(true, kendo.ui.Pager.prototype.options.messages, {
        "display": "{0} - {1} de {2} elementos",
        "empty": "No hay elementos para mostrar",
        "page": "Página",
        "of": "de {0}",
        "itemsPerPage": "elementos por página",
        "first": "Ir a la primera página",
        "previous": "Ir a la página anterior",
        "next": "Ir a la página siguiente",
        "last": "Ir a la última página",
        "refresh": "Refrescar",
        "morePages": "Más páginas"
    });
}

/* PivotGrid messages */

if (kendo.ui.PivotGrid) {
    kendo.ui.PivotGrid.prototype.options.messages =
    $.extend(true, kendo.ui.PivotGrid.prototype.options.messages, {
        "measureFields": "Suelta campos aquí",
        "columnFields": "Suelta columnas aquí",
        "rowFields": "Suelta columnas aquí"
    });
}

/* PivotFieldMenu operators */

if (kendo.ui.PivotFieldMenu) {
    kendo.ui.PivotGrid.prototype.options.operators =
    $.extend(true, kendo.ui.PivotGrid.prototype.options.operators, {
        "contains": "Contiene",
        "doesnotcontain": "No contiene",
        "startswith": "Empieza con",
        "endswith": "Termina con",
        "eq": "Es igual a",
        "neq": "No es igual a"
    });
}

/* PivotFieldMenu messages */

if (kendo.ui.PivotFieldMenu) {
    kendo.ui.PivotGrid.prototype.options.messages =
    $.extend(true, kendo.ui.PivotGrid.prototype.options.messages, {
        "info": "Mostrar elementos con los valores:",
        "filterFields": "Campos del Filtro",
        "filter": "Filtro",
        "include": "Incluir Campos...",
        "title": "Campos a Incluir",
        "clear": "Limpiar",
        "ok": "Ok",
        "cancel": "Cancelar"
    });
}

/* RecurrenceEditor messages */

if (kendo.ui.RecurrenceEditor) {
    kendo.ui.RecurrenceEditor.prototype.options.messages =
    $.extend(true, kendo.ui.RecurrenceEditor.prototype.options.messages, {
        "frequencies": {
            "never": "Nunca",
            "hourly": "Cada hora",
            "daily": "Diariamente",
            "weekly": "Semanalmente",
            "monthly": "Mensualmente",
            "yearly": "Anualmente"
        },
        "hourly": {
            "repeatEvery": "Repetir cada: ",
            "interval": " hora(s)"
        },
        "daily": {
            "repeatEvery": "Repetir cada: ",
            "interval": " día(s)"
        },
        "weekly": {
            "interval": " semana(s)",
            "repeatEvery": "Repetir cada: ",
            "repeatOn": "Repetir los: "
        },
        "monthly": {
            "repeatEvery": "Repetir cada: ",
            "repeatOn": "Repetir los: ",
            "interval": " mes(es)",
            "day": "día(s) "
        },
        "yearly": {
            "repeatEvery": "Repetir cada: ",
            "repeatOn": "Repetir los : ",
            "interval": " año(s)",
            "of": " de "
        },
        "end": {
            "label": "Fin:",
            "mobileLabel": "Finaliza",
            "never": "Nunca",
            "after": "Después ",
            "occurrence": " ocurrencia(s)",
            "on": "Los "
        },
        "offsetPositions": {
            "first": "Primero",
            "second": "Segundo",
            "third": "Tercero",
            "fourth": "Cuarto",
            "last": "Último"
        },
        "weekdays": {
            "day": "Día",
            "weekday": "Semana",
            "weekend": "Día de fin de semana"
        }
    });
}

/* Scheduler messages */

if (kendo.ui.Scheduler) {
    kendo.ui.Scheduler.prototype.options.messages =
    $.extend(true, kendo.ui.Scheduler.prototype.options.messages, {
        "allDay": "todo el día",
        "date": "Fecha",
        "event": "Evento",
        "time": "Tiempo",
        "showFullDay": "Mostrar todo el día",
        "showWorkDay": "Mostrar horario de trabajo",
        "today": "Hoy",
        "save": "Guardar",
        "cancel": "Cancelar",
        "destroy": "Borrar",
        "deleteWindowTitle": "Eliminar evento",
        "ariaSlotLabel": "Seleccionado de {0:t} a {1:t}",
        "ariaEventLabel": "{0} los {1:D} a las {2:t}",
        "confirmation": "¿Estás seguro de que quieres eliminar este evento?",
        "views": {
            "day": "Día",
            "week": "Semana",
            "workWeek": "Semana laboral",
            "agenda": "Agenda",
            "month": "Mes"
        },
        "recurrenceMessages": {
            "deleteWindowTitle": "Eliminar item recurrente",
            "deleteWindowOccurrence": "Eliminar ocurrencia actual",
            "deleteWindowSeries": "Borrar las series",
            "editWindowTitle": "Editar item recurrente",
            "editWindowOccurrence": "Editar concurrencia actual",
            "editWindowSeries": "Editar las series",
            "deleteRecurring": "¿Quieres borrar sólo esta ocurrencia de evento o la serie completa?",
            "editRecurring": "¿Quieres editar sólo esta ocurrencia de evento o la serie completa??"
        },
        "editor": {
            "title": "Título",
            "start": "Inicio",
            "end": "Fin",
            "allDayEvent": "Evento de todo el día",
            "description": "Descripción",
            "repeat": "Repetir",
            "timezone": "Zona horaria",
            "startTimezone": "Zona horaria inicial",
            "endTimezone": "Zona horaria final",
            "separateTimezones": "Usar zonas horarias inicial y final separadas",
            "timezoneEditorTitle": "Zonas horarias",
            "timezoneEditorButton": "Zona horaria",
            "timezoneTitle": "Zonas horarias",
            "noTimezone": "Sin zona horaria",
            "editorTitle": "Evento"
        }
    });
}

/* Slider messages */

if (kendo.ui.Slider) {
    kendo.ui.Slider.prototype.options =
    $.extend(true, kendo.ui.Slider.prototype.options, {
        "increaseButtonTitle": "Incrementar",
        "decreaseButtonTitle": "Disminuir"
    });
}

/* TreeView messages */

if (kendo.ui.TreeView) {
    kendo.ui.TreeView.prototype.options.messages =
    $.extend(true, kendo.ui.TreeView.prototype.options.messages, {
        "loading": "Cargando...",
        "requestFailed": "Solicitud fallida.",
        "retry": "Reintentar"
    });
}

/* Upload messages */

if (kendo.ui.Upload) {
    kendo.ui.Upload.prototype.options.localization =
    $.extend(true, kendo.ui.Upload.prototype.options.localization, {
        "select": "Seleccionar Archivo...",
        "cancel": "Cancelar",
        "retry": "Reintentar",
        "remove": "Remover",
        "uploadSelectedFiles": "Cargar Archivos",
        "dropFilesHere": "Suelte archivos aquí para cargarlos",
        "statusUploading": "cargando",
        "statusUploaded": "cargado",
        "statusWarning": "peligro",
        "statusFailed": "fallo",
        "headerStatusUploading": "Cargando...",
        "headerStatusUploaded": "Listo"
    });
}

/* Validator messages */

if (kendo.ui.Validator) {
    kendo.ui.Validator.prototype.options.messages =
    $.extend(true, kendo.ui.Validator.prototype.options.messages, {
        "required": "{0} es requerido",
        "pattern": "{0} no es válido",
        "min": "{0} debe ser mayor o igual que {1}",
        "max": "{0} debe ser menor o igual que {1}",
        "step": "{0} no es válido",
        "email": "{0} no es un correo válido",
        "url": "{0} no es una URL válida",
        "date": "{0} no es una fecha válida"
    });
}