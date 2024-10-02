//Morris js for first line chart
Morris.Line({
    element: "morris-line-chart-first",
    data: [{
        y: "2011",
        a: 0
    },
        {
            y: "2012",
            a: 75
        },
        {
            y: "2013",
            a: 50
        },
        {
            y: "2014",
            a: 15
        },
        {
            y: "2015",
            a: 50
        },
        {
            y: "2016",
            a: 25

        },
        {
            y: "2017",
            a: 100
        }],
    xkey: "y",
    ykeys: ["a"],
    lineColors: ["#4099ff"],
    labels: ["Series A"],
    grid: false,
    axes: false

});
//Morris js for second line chart
Morris.Line({
    element: "morris-line-chart-second",
    data: [{
        y: "2011",
        a: 100
    },
        {
            y: "2012",
            a: 25
        },
        {
            y: "2013",
            a: 50
        },
        {
            y: "2014",
            a: 75
        },
        {
            y: "2015",
            a: 100
        },
        {
            y: "2016",
            a: 65

        },
        {
            y: "2017",
            a: 0
        }],
    xkey: "y",
    ykeys: ["a"],
    lineColors: ["#FF5370"],
    labels: ["Series A"],
    grid: false,
    axes: false

});
//Morris js for third line chart
Morris.Line({
    element: "morris-line-chart-third",
    data: [{
        y: "2011",
        a: 75
    },
        {
            y: "2012",
            a: 15
        },
        {
            y: "2013",
            a: 95
        },
        {
            y: "2014",
            a: 80
        },
        {
            y: "2015",
            a: 0
        },
        {
            y: "2016",
            a: 25

        },
        {
            y: "2017",
            a: 60
        }],
    xkey: "y",
    ykeys: ["a"],
    lineColors: ["#ab8ce4"],
    labels: ["Series A"],
    grid: false,
    axes: false

});
//Morris js for forth line chart
Morris.Line({element: "morris-line-chart-forth",
    data: [{
        y: "2011",
        a: 0
    },
        {
            y: "2012",
            a: 60
        },
        {
            y: "2013",
            a: 50
        },
        {
            y: "2014",
            a: 15
        },
        {
            y: "2015",
            a: 50
        },
        {
            y: "2016",
            a: 35

        },
        {
            y: "2017",
            a: 50
        }],
    xkey: "y",
    ykeys: ["a"],
    lineColors: ["#26c6da"],
    labels: ["Series A"],
    grid: false,
    axes: false

});



google.charts.load('current', {'packages':['corechart']});
google.charts.setOnLoadCallback(drawBasic);
// google.charts.setOnLoadCallback(drawVisualization);

function drawBasic() {

    if ($("#recent-act").length > 0) {
        var data = google.visualization.arrayToDataTable([
            ['Task', 'Hours per Day'],
            ['Work',     11],
            ['Eat',      2],
            ['Commute',  2],
            ['Watch TV', 2],
            ['Sleep',    7]
        ]);
        var options = {
            pieHole: 0.4,
            width:'100%',
            height: 290,
            colors: ["#4099ff", "#f3d800", "#26c6da", "#fd7b6c", "#00c292"]
        };
        var chart = new google.visualization.PieChart(document.getElementById('recent-act'));
        chart.draw(data, options);
    }


}



/*custom line chart*/
$("#custom-line-chart").sparkline([24, 27, 8, 18, 12, 8, 40, 33, 37, 25, 22, 18, 25, 2, 4, 15, 10, 11, 15, 14, 6, 8, 6, 8, 30, 25], {
    type: 'line',
    width: '100%',
    height: '100%',
    tooltipClassname: 'chart-sparkline',
    chartRangeMax: '50',
    lineColor: 'rgba(35, 215, 223, 0.8)',
    fillColor: 'rgba(35, 215, 223, 0.5)',
    highlightLineColor: "rgba(35, 215, 223, 0.8)",
    highlightSpotColor: "rgba(35, 215, 223, 0.8)"
});

$("#custom-line-chart").sparkline([4, 6, 8, 6, 8, 30, 25, 24, 27, 8, 18, 12, 8, 40, 33, 37, 25, 22, 18, 25, 2, 4, 15, 10, 11, 15, 1], {
    type: 'line',
    width: '100%',
    height: '100%',
    composite: '!0',
    tooltipClassname: 'chart-sparkline',
    chartRangeMax: '40',
    lineColor: 'rgba(64, 153, 255, 0.8)',
    fillColor: 'rgba(64, 153, 255, 0.5)',
    highlightLineColor: "rgba(64, 153, 255, 0.8)",
    highlightSpotColor: "rgba(64, 153, 255, 0.8)"
});

"use strict";
$(document).ready(function() {
//simple-line-chart-sparkline chart
    setTimeout(function(){
        $("#simple-line-chart-cms").sparkline([5, 10, 20, 14, 17, 21, 20, 10, 4, 13,0, 10, 30, 40, 10, 15, 20], {
            type: 'line',
            width: '100%',
            height: '100%',
            tooltipClassname: 'chart-sparkline',
            lineColor: '#ffffff',
            fillColor: 'transparent',
            spotColor: '#fff',
            highlightLineColor: "#fff",
            highlightSpotColor: "#fff",
            sliceColors: ["#fff", "#fff", "#fff"],
            targetColor: "#fff",
            performanceColor: "#fff",
            boxFillColor: "#fff",
            medianColor: "#fff",
            minSpotColor: "#fff"
        });
    });

});

//   speedchart

// set random value
function randomValue() {
    var value = Math.round(Math.random() * 100);
    chart.axes[0].setTopText(value + " %");
    // adjust darker band to new value
    chart.axes[0].bands[1].setEndValue(value);
}
