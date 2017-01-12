function drawBarGraph(elementIdName, data) {

    var canvas = document.getElementById(elementIdName);
    canvas.width = 800;
    canvas.height = 400;
    var context = canvas.getContext("2d");

    var maxValue = Math.max.apply(Math, data);
    var minValue = Math.min.apply(Math, data);

    var d = new Date();
    var cols = new Date(d.getYear(), d.getMonth(), 0).getDate();
    var step = (maxValue - minValue) / 10;
    var rows = (maxValue - minValue) / step + 1;
    var margin = 10;
    var columnWidth = (canvas.width - 2 * margin) / cols - 1;
    var columnHeight = (canvas.height - 2 * margin) / rows;

    context.strokeStyle = "#a8a8a8";
    context.beginPath();

    // draw vertical lines
    var x;
    for (i = 1; i <= cols + 1; i++) {
        x = i * columnWidth;
        context.moveTo(x, margin);
        context.lineTo(x, canvas.height - 2 * margin);
        if (i == cols + 1)
            continue;
        context.fillText(i, x + 5, canvas.height - 5);
    }

    // draw horizontal lines
    var i, scale, y;
    for (i = 0, scale = maxValue; scale >= minValue; i++, scale -= step) {
        y = margin * 2.5 + i * columnHeight;
        context.moveTo(columnWidth - margin, y);
        context.lineTo(canvas.width - margin, y);
        context.fillText(scale, columnWidth - margin * 2.3, y + 3);
    }

    context.stroke();

    context.strokeStyle = "#312450";
    context.beginPath();

    // draw bars
    var c;
    for (i = 1; i <= cols; i++) {
        if (data[i - 1] == 0)
            continue;
        c = 0;
        for (j = maxValue; j >= minValue; j -= step) {
            if (data[i - 1] < j)
                c++;
            else
                break;
        }
        x = i * columnWidth;
        context.moveTo(x, y);
        context.lineTo(x, margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i - 1] - j));
        context.lineTo((i + 1) * columnWidth,
            margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i - 1] - j));
        context.fillText(data[i - 1],
            x + 2,
            margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i - 1] - j) - 3);
        context.lineTo((i + 1) * columnWidth, y);
    }
    context.stroke();
}