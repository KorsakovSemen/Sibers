$(document).ready(function () {
    // тултипы в таблицах
    let allTd = $('[data-toggle="tooltip"]');
    for (var i = 0; i < allTd.length; i++) {
        if (allTd[i].innerText.length > 10)
            // если явно указан title="ваш тултип" то ставим его
            allTd[i].title = allTd[i].title ? allTd[i].title : allTd[i].innerText;
    }
    allTd.tooltip();

    // uncheck radio кнопок
    $("[type='radio']").click(function (e) {
        if (e.ctrlKey) this.checked = false;
    })

    // обрабатываем сортировку таблиц
    let allTh = $('th');
    allTh.each(function (index) {
        // на пустых заголовках не работает
        if (allTh[index].innerText) {
            stateSort.push(-1);
            $(this).on("click", function () {
                sortTable(index, allTh);
            });
        }
    });
});

/* сортировка таблиц */
var stateSort = [];// состояние сортировки для таблицы (-1 не сорт. 0 сорт. 1 инверсия от 0)
function sortTable(index, headers) {
    const table = $('table')[0];
    if (table) {
        let sortedRows = Array.from(table.rows).slice(1);
        if (stateSort[index] === -1) {
            sortedRows.sort((rowA, rowB) => rowA.cells[index].innerHTML > rowB.cells[index].innerHTML ? 1 : -1);
            stateSortChange(index, 0);
            headersChange(index, ' ↑', headers);
        }
        else if (stateSort[index] === 0) {
            sortedRows.reverse();
            stateSortChange(index, 1);
            headersChange(index, ' ↓', headers);
        }
        else if (stateSort[index] === 1) {
            sortedRows.reverse();
            stateSortChange(index, 0);
            headersChange(index, ' ↑', headers);
        }
        table.tBodies[0].append(...sortedRows);
    }
}

// изменение глобального состояния сортировки
function stateSortChange(changeIndex, newState) {
    for (var i in stateSort) {
        if (i == changeIndex) stateSort[i] = newState;
        else stateSort[i] = -1;
    }
}
// визуализация сортировки таблиц
function headersChange(currentIndex, mode, headers) {
    headers.each(function (i) {
        headers[i].innerText = headers[i].innerText.replace(' ↑', '').replace(' ↓', ''); //вырезаем символы
        if (i == currentIndex) headers[i].innerText = headers[i].innerText + mode;
    });
}