//thin-bar-chart first
$(function() {
    Morris.Bar({
        element: 'thin-bar-chart-first',
        data: [{
            x: "January",
            y: 40,
            z: 10
        },
            {
                x: "February",
                y: 30,
                z: 50
            },
            {
                x: "March",
                y: 30,
                z: 2
            },
            {
                x: "April",
                y: 40,
                z: 4
            },
            {
                x: "May",
                y: 50,
                z: 6
            },
            {
                x: "June",
                y: 80,
                z: 20
            },{
                x: "Julay",
                y: 50,
                z: 1
            },{
                x: "August",
                y: 85,
                z: 5
            }
            ],
        xkey: "x",
        ykeys: ["y", "z"],
        labels: ["Y", "Z"],
        barColors: ["#4099ff", "#ddd" ,"#4099ff" ,"#ddd" ,"#4099ff", "#ddd" ,"#4099ff"],
        stacked: !0,
        horizontal:false,
        height: 20,
        axes: false
    });
});
//thin-bar-chart second
$(function() {
    Morris.Bar({
        element: 'thin-bar-chart-second',
        data: [{
            x: "January",
            y: 40,
            z: 10
        },
            {
                x: "February",
                y: 30,
                z: 50
            },
            {
                x: "March",
                y: 30,
                z: 2
            },
            {
                x: "April",
                y: 40,
                z: 4
            },
            {
                x: "May",
                y: 50,
                z: 6
            },
            {
                x: "June",
                y: 80,
                z: 20
            },{
                x: "Julay",
                y: 50,
                z: 1
            },{
                x: "August",
                y: 85,
                z: 5
            }
        ],
        xkey: "x",
        ykeys: ["y", "z"],
        labels: ["Y", "Z"],
        barColors: ["#FF5370", "#ddd" ,"#FF5370" ,"#ddd" ,"#FF5370", "#ddd" ,"#FF5370"],
        stacked: !0,
        horizontal:false,
        axes: false
    });



});
//thin-bar-chart third
$(function() {
    Morris.Bar({
        element: 'thin-bar-chart-third',
        data: [{
            x: "January",
            y: 40,
            z: 10
        },
            {
                x: "February",
                y: 30,
                z: 50
            },
            {
                x: "March",
                y: 30,
                z: 2
            },
            {
                x: "April",
                y: 40,
                z: 4
            },
            {
                x: "May",
                y: 50,
                z: 6
            },
            {
                x: "June",
                y: 80,
                z: 20
            },{
                x: "Julay",
                y: 50,
                z: 1
            },{
                x: "August",
                y: 85,
                z: 5
            }
        ],
        xkey: "x",
        ykeys: ["y", "z"],
        labels: ["Y", "Z"],
        barColors: ["#26c6da", "#ddd" ,"#26c6da" ,"#ddd" ,"#26c6da", "#ddd" ,"#26c6da"],
        stacked: !0,
        horizontal:false,
        axes: false
    });
});


//-----product-sale-chart
new Chartist.Bar('.product-sale-chart', {
    labels: ['2010-11', '2011-12', '2012-13', '2013-13', '2014-15', '2015-16', '2016-17', '2017-18'],
    series: [
        [0.9, 0.4, 1.5, 4.9, 3, 3.8, 3.8, 1.9],
        [6.4, 5.7, 7, 4.95, 3, 3.8, 3.8, 1.9],
        [4.3, 2.3, 3.6, 4.5, 5, 2.8, 3.3, 4.3],
        [3.8, 4.1, 2.8, 1.8, 2.2, 1.9, 6.7, 2.9]

    ]
}, {
    // Default mobile configuration
    stackBars: true,
    axisX: {
        labelInterpolationFnc: function(value) {
            return value.split(/\s+/).map(function(word) {
                return word[0];
            }).join('');
        }
    },
    axisY: {
        offset: 20
    }
}, [
    // Options override for media > 400px
    ['screen and (min-width: 400px)', {
        reverseData: true,
        horizontalBars: true,
        axisX: {
            labelInterpolationFnc: Chartist.noop
        },
        axisY: {
            offset: 60
        }
    }],
    // Options override for media > 800px
    ['screen and (min-width: 800px)', {
        stackBars: false,
        seriesBarDistance: 10
    }],
    // Options override for media > 1000px
    ['screen and (min-width: 1000px)', {
        reverseData: false,
        horizontalBars: false,
        seriesBarDistance: 15
    }]
]);

//yearly graph
$("#yearly-graph").sparkline([35, 37, 40, 27, 30, 28, 35, 40, 30, 32, 33, 28, 32, 33, 30, 25], {
    type: 'line',
    width: '100%',
    height: '101%',
    tooltipClassname: 'chart-sparkline',
    chartRangeMax: '50',
    lineColor: 'rgba(97,161,253)',
    fillColor: 'rgba(97,161,253)',
    highlightLineColor: "rgba(97,161,253)",
});

//monthly graph
$("#monthly-graph").sparkline([30, 32, 35, 25, 18, 22, 24, 35, 18, 24,28, 30, 32, 28, 20, 15, 20], {
    type: 'line',
    width: '100%',
    height: '101%',
    tooltipClassname: 'chart-sparkline',
    chartRangeMax: '50',
    lineColor: '#F45773',
    fillColor: 'rgba(244,124,146)',

});

google.charts.load('current', {packages: ['corechart', 'bar']});
google.charts.load('current', {'packages':['corechart']});
google.charts.setOnLoadCallback(drawBasic);
// google.charts.setOnLoadCallback(drawVisualization);

function drawBasic() {

    // Employee
    if ($("#employee").length > 0) {
        var a = google.visualization.arrayToDataTable([
                ["Element", "Density", {
                    role: "style"
                }],
                ["Class4", 8.94, "#FF5370"],
                ["Class3", 10.49, "#26c6da"],
                ["Class2", 19.3, "#ab8ce4"],
                ["Class1", 21.45, "color: #4099ff"]
            ]),
            d = new google.visualization.DataView(a);
        d.setColumns([0, 1, {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }, 2]);
        var b = {

                width:'100%',
                height: 400,
                bar: {
                    groupWidth: "95%"
                },
                legend: {
                    position: "none"
                }
            },
            c = new google.visualization.BarChart(document.getElementById("employee"));
        c.draw(d, b)

    }

    // my activity
    if ($("#my-activity").length > 0) {
        var data = google.visualization.arrayToDataTable([
            ['Task', 'Hours per Day'],
            ['Work',     11],
            ['Eat',      2],
            ['Commute',  2],
            ['Watch TV', 2],
            ['Sleep',    7]
        ]);

        var options = {
            width:'100%',
            height: 400,
            colors: ["#4099ff", "#f3d800", "#26c6da", "#fd7b6c", "#00c292"]
        };

        var chart = new google.visualization.PieChart(document.getElementById('my-activity'));

        chart.draw(data, options);
    }

}

//growth-graph-first
$("#growth-graph-first").sparkline([15, 25, 60, 40, 55, 30, 45], {
    type: 'line',
    width: '100%',
    height: '101%',
    tooltipClassname: 'chart-sparkline',
    chartRangeMax: '50',
    lineColor: '#AD91E6',
    fillColor: 'rgba(173,145,230)',
    highlightLineColor: "rgba(173,145,230)"
});
//growth-graph-second
$("#growth-graph-second").sparkline([60, 55, 45, 50, 55, 50, 35], {
    type: 'line',
    width: '100%',
    height: '102%',
    tooltipClassname: 'chart-sparkline',
    chartRangeMax: '50',
    lineColor: '#62A3FD',
    fillColor: 'rgba(98,163,253)',
    highlightLineColor: "rgba(98,163,253)"

});