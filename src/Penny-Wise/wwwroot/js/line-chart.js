function drawLineChart(elementIdName, data) {

    var canvas = document.getElementById(elementIdName);
    canvas.width = 800;
    canvas.height = 400;
    var context = canvas.getContext("2d");

    var months = [" ", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var maxValue = Math.max.apply(Math, data);
    var minValue = Math.min.apply(Math, data);

    var step = (maxValue - minValue) / 10;
    var d = new Date();
    var cols = new Date(d.getYear(), d.getMonth(), 0).getDate();
    var rows = (maxValue - minValue) / step + 1;
    var margin = 10;
    var columnWidth = (canvas.width - 2 * margin) / cols;
    var columnHeight = (canvas.height - 2 * margin) / rows;

    context.strokeStyle = "#a8a8a8";
    context.beginPath();

    // draw vertical lines
    var x, text;
    for (i = 1; i <= cols; i++) {
        x = i * columnWidth;
        context.moveTo(x, margin);
        context.lineTo(x, canvas.height - 2 * margin);
        if (document.getElementById("canvas-line-chart").className == "reports") {
            text = months[i];
        } else {
            text = i;
        }
        context.fillText(text, x - 7, canvas.height - 5);
    }

    // draw horizontal lines
    var i, scale;
    for (i = 0, scale = maxValue; scale >= minValue; i++, scale -= step) {
        var y = margin * 2.5 + i * columnHeight;
        context.moveTo(columnWidth - margin, y);
        context.lineTo(canvas.width - margin, y);
        context.fillText(scale, columnWidth - margin * 2, y + 3);
    }

    context.stroke();

    context.strokeStyle = "#312450";
    context.beginPath();

    // draw data
    var c;
    for (i = 1; i <= cols; i++) {
        c = 0;
        for (j = maxValue; j >= minValue; j -= step) {
            if (data[i - 1] < j)
                c++;
            else
                break;
        }
        context.fillText(data[i - 1],
            i * columnWidth - 5,
            margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i - 1] - j) - 5);
        context.moveTo(i * columnWidth, margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i - 1] - j));
        context.lineTo((i + 1) * columnWidth, margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i] - j));
    }

    context.stroke();
}