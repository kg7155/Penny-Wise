var canvas = document.getElementById("canvas-line-chart");
canvas.width = 800;
canvas.height = 400;
var context = canvas.getContext("2d");

var data, cols, step;
if (document.getElementById("canvas-line-chart").className == "reports") {
    var months = [" ", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    data = [140, -30, 50, 0, 20, 46, 63, 90, 100, 150, 11];
    cols = 12;
    step = 50;
} else {
    data = [5, 3, 0, -10, 15, 46, 73, 0, 0, 10, -5, -20, -40, 0, 43, 22, 45, 0, 11.2, -100, 80, 0, 0, 0, 0, 15, 22, 41, 0, 0, -11]
    cols = 31;
    step = 20;
}

var maxValue = Math.max.apply(Math, data);
var minValue = Math.min.apply(Math, data);

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
        if (data[i-1] < j)
            c++;
        else 
            break;
    }
    context.fillText(data[i-1], i * columnWidth - 5, margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i-1] - j) - 5);
    context.moveTo(i * columnWidth, margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i-1] - j) );
    context.lineTo( (i + 1) * columnWidth, margin * 2.5 + c * columnHeight - (columnHeight / step) * (data[i] - j) );
}

context.stroke();