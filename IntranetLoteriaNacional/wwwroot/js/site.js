
window.initializeFunctions = function () {
    initializeSelect2();
    addSelect2ChangeListener();
    refreshSelect2();
    addSelect2MultiselectChangeListener();
    observeTableChanges();
    ReverSelectCheck();
}
window.onload = function () {
    initializeFunctions();
}

function ReverSelectCheck (checkboxId, select) {
    var checkbox = document.getElementById(checkboxId);
    if (checkbox) {
        checkbox.checked = select;
    }
};

function initializeSelect2() {
    $('.select2').select2({
        containerCssClass: 'form-control'
    });
}
function paintConsola(texto) {
    console.log(texto);
}

function addSelect2ChangeListener(dotNetReference, identifier) {
    $('#' + identifier).on('change', function (e) { // Utilizar el identificador para seleccionar el elemento por su ID
        var selectedValue = $(this).val();
        dotNetReference.invokeMethodAsync('HandleSelectionChange', selectedValue);
    });
}

window.refreshSelect2 = (identifier) => {
    $('#' + identifier).select2().trigger('change');
};

function addSelect2MultiselectChangeListener(dotnetRef, identifier) {
    $("#" + identifier).on("change", function (e) {
        var selectedValues = $(this).val();
        dotnetRef.invokeMethodAsync("HandleSelectionChange", selectedValues);
    });
}
function observeTableChanges(tableId) {
    var table = document.getElementById(tableId);

    if (!table) {
        //console.error("No se encontró la tabla con el ID: " + tableId);
        return;
    }

    var observer = new MutationObserver(function (mutations) {
        reagroup(tableId);
    });

    var config = { childList: true, subtree: true };

    observer.observe(table, config);
}

function observeTableChangesgroup(tableId) {
    var table = document.getElementById(tableId);

    if (!table) {
        //console.error("No se encontró la tabla con el ID: " + tableId);
        return;
    }

    var observer = new MutationObserver(function (mutations) {
        reagroupWithMultipleTotals(tableId,[2,7]);
        //reagroupWithTotalsBelowColumns(tableId);
        //reagroup(tableId)
    });

    var config = { childList: true, subtree: true };

    observer.observe(table, config);
}

function reagroup(tableId) {
    var table = document.getElementById(tableId);

    if (!table) {
        //console.error("No se encontró la tabla con el ID: " + tableId);
        return;
    }

    var rows = table.getElementsByTagName("tr");

    var hiddenCells = table.querySelectorAll('td[data-hidden="true"]');
    hiddenCells.forEach(function (cell) {
        cell.style.display = "";
    });

    var groupRows = {};

    for (var i = 1; i < rows.length; i++) {
        var row = rows[i];
        var columnValue = row.cells[0].innerText;

        if (groupRows[columnValue]) {
            groupRows[columnValue].rowspan++;
            row.cells[0].style.display = "none";
            row.cells[0].setAttribute("data-hidden", "true");
        } else {
            groupRows[columnValue] = {
                rowspan: 1,
                row: row
            };
        }
    }

    for (var key in groupRows) {
        var groupRow = groupRows[key];
        groupRow.row.cells[0].rowSpan = groupRow.rowspan;
    }

    //updateTotals(tableId);
}
function reagroupWithTotalsBelowColumns(tableId) {
    var table = document.getElementById(tableId);

    if (!table) {
        // Si no se encuentra la tabla con el ID proporcionado, se sale de la función.
        return;
    }

    // Obtener todas las filas de la tabla (excluyendo la primera fila que probablemente contiene encabezados).
    var rows = table.getElementsByTagName("tr");

    // Mostrar las celdas ocultas.
    var hiddenCells = table.querySelectorAll('td[data-hidden="true"]');
    hiddenCells.forEach(function (cell) {
        cell.style.display = "";
    });

    // Objeto para almacenar las filas de grupo y sus totales.
    var groupRows = {};

    // Iterar a través de las filas (excluyendo la primera fila de encabezado).
    for (var i = 1; i < rows.length; i++) {
        var row = rows[i];
        var columnValue = row.cells[0].innerText;
        var numericValue = parseFloat(row.cells[1].innerText); // Cambiar el índice si el valor a sumar está en una columna diferente.

        // Si ya existe un grupo con la misma columna, se aumenta el rowspan y el total.
        if (groupRows[columnValue]) {
            groupRows[columnValue].rowspan++;
            groupRows[columnValue].total += numericValue;
            row.cells[0].style.display = "none";
            row.cells[0].setAttribute("data-hidden", "true");
        } else {
            // Si no existe un grupo con la misma columna, se crea un nuevo grupo.
            groupRows[columnValue] = {
                rowspan: 1,
                total: numericValue,
                row: row
            };
        }
    }

    // Establecer los rowspan de las filas de grupo y mostrar los totales debajo de las columnas.
    for (var key in groupRows) {
        var groupRow = groupRows[key];
        groupRow.row.cells[0].rowSpan = groupRow.rowspan;

        // Mostrar el total debajo de la columna correspondiente.
        var totalRow = table.insertRow(groupRow.row.rowIndex + groupRow.rowspan);
        var totalCell = totalRow.insertCell(groupRow.row.cells.length - 1); // Colocar el total en la última columna.
        totalCell.style.fontWeight = "bold";
        totalCell.innerText = "Total: " + groupRow.total;
    }
}

function reagroupWithTotals(tableId, valueIndex) {
    var table = document.getElementById(tableId);

    if (!table) {
        // Si no se encuentra la tabla con el ID proporcionado, se sale de la función.
        return;
    }

    // Obtener todas las filas de la tabla (excluyendo la primera fila que probablemente contiene encabezados).
    var rows = table.getElementsByTagName("tr");

    // Mostrar las celdas ocultas.
    var hiddenCells = table.querySelectorAll('td[data-hidden="true"]');
    hiddenCells.forEach(function (cell) {
        cell.style.display = "";
    });

    // Objeto para almacenar las filas de grupo y sus totales.
    var groupRows = {};

    // Iterar a través de las filas (excluyendo la primera fila de encabezado).
    for (var i = 1; i < rows.length; i++) {
        var row = rows[i];
        var columnValue = row.cells[0].innerText;

        // Obtener el valor numérico específico para calcular el total del grupo.
        var numericValue = parseFloat(row.cells[valueIndex].innerText);

        // Si ya existe un grupo con la misma columna, se aumenta el rowspan y el total.
        if (groupRows[columnValue]) {
            groupRows[columnValue].rowspan++;
            groupRows[columnValue].total += numericValue;
            row.cells[0].style.display = "none";
            row.cells[0].setAttribute("data-hidden", "true");
        } else {
            // Si no existe un grupo con la misma columna, se crea un nuevo grupo.
            groupRows[columnValue] = {
                rowspan: 1,
                total: numericValue,
                row: row
            };
        }
    }

    // Establecer los rowspan de las filas de grupo y mostrar los totales.
    for (var key in groupRows) {
        var groupRow = groupRows[key];
        groupRow.row.cells[0].rowSpan = groupRow.rowspan;

        // Crear una nueva fila para mostrar el total.
        var totalRow = table.insertRow(groupRow.row.rowIndex + groupRow.rowspan);
        var totalCell = totalRow.insertCell(0);
        totalCell.colSpan = groupRow.row.cells.length - 1;
        totalCell.style.fontWeight = "bold";
        totalCell.innerText = "Total: " + groupRow.total;
    }
}

function reagroupWithMultipleTotals(tableId, valueIndexes) {
    var table = document.getElementById(tableId);

    if (!table) {
        // Si no se encuentra la tabla con el ID proporcionado, se sale de la función.
        return;
    }

    // Obtener todas las filas de la tabla (excluyendo la primera fila que probablemente contiene encabezados).
    var rows = table.getElementsByTagName("tr");

    // Mostrar las celdas ocultas.
    var hiddenCells = table.querySelectorAll('td[data-hidden="true"]');
    hiddenCells.forEach(function (cell) {
        cell.style.display = "";
    });

    // Objeto para almacenar las filas de grupo y sus totales.
    var groupRows = {};

    // Iterar a través de las filas (excluyendo la primera fila de encabezado).
    for (var i = 1; i < rows.length; i++) {
        var row = rows[i];
        var columnValue = row.cells[0].innerText;

        var totalValues = Array.from(valueIndexes).map(function (index) {
            var valor = row.cells[index];
            // iif(totalValues.innerText != "undefined")
            if (valor) {
                return parseFloat(valor.innerText);

            }
            return parseFloat(0);

        });

        // Crear una clave única para el grupo basada en el valor de la primera celda y los valores de las celdas para el total.
        var groupKey = columnValue;

        // Si ya existe un grupo con la misma clave, se aumenta el rowspan y los totales.
        if (groupRows[groupKey]) {
            groupRows[groupKey].rowspan++;
            groupRows[groupKey].totals = groupRows[groupKey].totals.map(function (value, index) {
                return value + totalValues[index];
            });
            row.cells[0].style.display = "none";
            row.cells[0].setAttribute("data-hidden", "true");
        } else {
            // Si no existe un grupo con la misma clave, se crea un nuevo grupo.
            groupRows[groupKey] = {
                rowspan: 1,
                totals: totalValues,
                row: row
            };
        }
    }

    // Establecer los rowspan de las filas de grupo y mostrar los totales.
    for (var key in groupRows) {
        var groupRow = groupRows[key];
        groupRow.row.cells[0].rowSpan = groupRow.rowspan;

        // Crear una nueva fila para mostrar los totales.
        var totalRow = table.insertRow(groupRow.row.rowIndex + groupRow.rowspan);
        var totalCell = totalRow.insertCell(0);
        totalCell.colSpan = groupRow.row.cells.length - 1;
        totalCell.style.fontWeight = "bold";
        totalCell.innerText = "Totales: " + groupRow.totals.join(" | ");
    }
}
function reagroup2(tableId) {
    var table = document.getElementById(tableId);

    if (!table) {
        //console.error("No se encontró la tabla con el ID: " + tableId);
        return;
    }

    var rows = table.getElementsByTagName("tr");

    var hiddenCells = table.querySelectorAll('td[data-hidden="true"]');
    hiddenCells.forEach(function (cell) {
        cell.style.display = "";
    });

    var groupRows = {};

    for (var i = 1; i < rows.length; i++) {
        var row = rows[i];
        var columnValue = row.cells[0].innerText;

        if (groupRows[columnValue]) {
            groupRows[columnValue].rowspan++;
            row.cells[0].style.display = "none";
            row.cells[0].setAttribute("data-hidden", "true");
            groupRows[columnValue].totalColumn2 += parseFloat(row.cells[1].innerText);
            groupRows[columnValue].totalColumn7 += parseFloat(row.cells[6].innerText);
        } else {
            groupRows[columnValue] = {
                rowspan: 1,
                row: row,
                totalColumn2: parseFloat(row.cells[1].innerText),
                totalColumn7: parseFloat(row.cells[6].innerText)
            };
        }
    }

    for (var key in groupRows) {
        var groupRow = groupRows[key];
        groupRow.row.cells[0].rowSpan = groupRow.rowspan;
        groupRow.row.cells[1].innerText = groupRow.totalColumn2.toFixed(2); // Actualizar columna 2 con el total
        groupRow.row.cells[6].innerText = groupRow.totalColumn7.toFixed(2); // Actualizar columna 7 con el total
    }
}

function updateTotals(tableId) {
    var table = document.getElementById(tableId);

    if (!table) {
        //console.error("No se encontró la tabla con el ID: " + tableId);
        return;
    }

    var rows = table.getElementsByTagName("tr");
    var groupRows = {};

    for (var i = 1; i < rows.length; i++) {
        var row = rows[i];
        var columnValue = row.cells[0].innerText;

        if (groupRows[columnValue]) {
            groupRows[columnValue].totalColumn2 += parseFloat(row.cells[3].innerText);
            groupRows[columnValue].totalColumn7 += parseFloat(row.cells[7].innerText);
        } else {
            groupRows[columnValue] = {
                totalColumn2: parseFloat(row.cells[3].innerText),
                totalColumn7: parseFloat(row.cells[7].innerText)
            };
        }
    }

    for (var key in groupRows) {
        var totalRow = table.querySelector('tr[data-group="' + key + '"]');
        totalRow.cells[3].innerText = groupRows[key].totalColumn2.toFixed(2);
        totalRow.cells[7].innerText = groupRows[key].totalColumn7.toFixed(2);
    }
}

window.generateFileUrl = function (fileBytes, mimeType) {
    const blob = new Blob([fileBytes], { type: mimeType });
    return URL.createObjectURL(blob);
};

window.downloadFile = function (url, fileName) {
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

/*exportar excel**/
// Importar la biblioteca xlsx.js
// Asegúrate de haber incluido la biblioteca xlsx.js en tu proyecto antes de usar esta función

function exportarTabla(idTabla, columnas, titleExport) {
    // Obtener la tabla por su ID
    var tabla = document.getElementById(idTabla);

    // Crear un objeto de datos para el archivo Excel
    var datosExcel = { SheetNames: [], Sheets: {} };

    // Crear un array de datos para almacenar los valores de la tabla
    var datosTabla = [];

    // Obtener las cabeceras de la tabla (primera fila)
    var cabeceras = [];
    var primeraFila = tabla.rows[0];

    for (var j = 0; j < primeraFila.cells.length - 1; j++) {
        // Obtener el contenido de la celda de la primera fila
        var contenidoCabecera = primeraFila.cells[j].innerText;

        // Agregar el contenido a las cabeceras
        cabeceras.push(contenidoCabecera);
    }

    // Verificar si se deben exportar todas las columnas
    var exportarTodasLasColumnas = false;
    var columnasEspecificas = [];

    if (columnas.toLowerCase() === "all") {
        exportarTodasLasColumnas = true;
    } else {
        columnasEspecificas = columnas.split(",").map(function (columna) {
            return columna.trim();
        });
    }

    // Recorrer las filas de la tabla, excepto la primera fila (cabeceras)
    for (var i = 1; i < tabla.rows.length; i++) {
        // Crear un objeto de fila para almacenar los valores de la fila
        var fila = {};

        // Recorrer las celdas de cada fila
        for (var j = 0; j < tabla.rows[i].cells.length - 1; j++) {
            // Verificar si se deben exportar todas las columnas o si la columna actual está especificada
            if (exportarTodasLasColumnas || columnasEspecificas.includes(cabeceras[j])) {
                // Obtener el contenido de la celda
                var contenidoCelda = tabla.rows[i].cells[j].innerText;

                // Obtener el nombre de la cabecera correspondiente
                var cabecera = cabeceras[j];

                // Agregar el contenido a la fila con la clave de la columna correspondiente (nombre de la cabecera)
                fila[cabecera] = { v: contenidoCelda };
            }
        }

        // Agregar la fila al array de datos de la tabla
        datosTabla.push(fila);
    }

    // Crear un objeto de hoja de cálculo con los datos de la tabla
    var hojaCalculo = XLSX.utils.json_to_sheet(datosTabla);

    // Agregar la hoja de cálculo al objeto de datos Excel
    datosExcel.SheetNames.push("Tabla");
    datosExcel.Sheets["Tabla"] = hojaCalculo;

    // Generar el archivo Excel en formato binario
    var libroExcel = XLSX.write(datosExcel, { bookType: "xlsx", type: "binary" });

    // Convertir el archivo Excel binario a un objeto Blob
    var blobExcel = new Blob([s2ab(libroExcel)], { type: "application/octet-stream" });

    // Crear un objeto de URL para el archivo Blob
    var urlExcel = URL.createObjectURL(blobExcel);

    // Crear un enlace de descarga para descargar el archivo Excel
    var enlaceDescarga = document.createElement("a");
    enlaceDescarga.href = urlExcel;
    enlaceDescarga.download = titleExport + ".xlsx";
    enlaceDescarga.click();
}



// Función auxiliar para convertir una cadena de texto en un array de bytes
function s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xff;
    return buf;
}




//////////////////////////////////////////////////////////////////////////////
function loadSteps() {
    var form = $("#contact");
    form.validate({
        errorPlacement: function (error, element) {
            element.before(error);
        },
        rules: {
            confirm: {
                equalTo: "#password"
            }
        }
    });
    form.children("div").steps({
        headerTag: "h3",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        onStepChanging: function (event, currentIndex, newIndex) {
            form.validate().settings.ignore = ":disabled,:hidden";
            return form.valid();
        },
        onFinishing: function (event, currentIndex) {
            form.validate().settings.ignore = ":disabled";
            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            alert("Submitted!");
        }
    });
}
function initializeSinFechas() {
    let fechadesde = document.getElementById("fechas");
    if (fechadesde) {
        fechadesde.style.display = "none";
    }
}
//function observeTableChanges() {
//    // Obtener la tabla por su id o cualquier otro selector
//    var table = document.getElementById("table");

//    // Crear una instancia del observador de mutaciones
//    var observer = new MutationObserver(function (mutations) {
//        // Callback para manejar las mutaciones en la tabla
//        // Aquí puedes realizar las operaciones necesarias en la tabla cada vez que ocurra un cambio

//        // Por ejemplo, puedes llamar a la función "reagroup" para agrupar las filas nuevamente
//        reagroup();
//    });

//    // Configurar las opciones del observador
//    var config = { childList: true, subtree: true };

//    // Observar cambios en la tabla
//    observer.observe(table, config);
//}


//function reagroup() {
//    var table = document.getElementById("table");

//    // Obtener las filas de la tabla
//    var rows = table.getElementsByTagName("tr");

//    // Restaurar las celdas ocultas en las filas anteriores
//    var hiddenCells = document.querySelectorAll('td[data-hidden="true"]');
//    hiddenCells.forEach(function (cell) {
//        cell.style.display = "";
//    });

//    // Crear un objeto para realizar el seguimiento de las filas agrupadas
//    var groupRows = {};

//    // Iterar sobre las filas de la tabla (excepto la primera fila de encabezado)
//    for (var i = 1; i < rows.length; i++) {
//        var row = rows[i];

//        // Obtener el valor de la columna 0 en la fila actual
//        var columnValue = row.cells[0].innerText;

//        // Verificar si ya hay una fila agrupada con el mismo valor de la columna 0
//        if (groupRows[columnValue]) {
//            // Incrementar el rowspan de la fila agrupada existente
//            groupRows[columnValue].rowspan++;
//            // Ocultar la celda correspondiente en la fila actual
//            row.cells[0].style.display = "none";
//            row.cells[0].setAttribute("data-hidden", "true");
//        } else {
//            // Crear una nueva entrada en el objeto groupRows
//            groupRows[columnValue] = {
//                rowspan: 1, // Inicializar el rowspan en 1
//                row: row // Guardar la referencia a la primera fila del grupo
//            };
//        }
//    }

//    // Actualizar los rowspan de las filas agrupadas
//    for (var key in groupRows) {
//        var groupRow = groupRows[key];
//        groupRow.row.cells[0].rowSpan = groupRow.rowspan;
//    }
//}

