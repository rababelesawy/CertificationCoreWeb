//-----site visitor chart
var chart = new Chartist.Line('.site-visitor-chart', {
    labels: ['North', 'South', 'East', 'West'],
    series: [
        [1, 5, 2, 5, 4, 3],
        [2, 3, 4, 8, 1, 2],
        [5, 7, 2, 3, 6, 8],
        [3, 1, 4, 6, 9, 6]
    ]
}, {
    low: 1,
    showArea: true,
    showPoint: true,
    fullWidth: true
});

chart.on('draw', function(data) {
    if(data.type === 'line' || data.type === 'area') {
        data.element.animate({
            d: {
                begin: 2000 * data.index,
                dur: 2000,
                from: data.path.clone().scale(1, 0).translate(0, data.chartRect.height()).stringify(),
                to: data.path.clone().stringify(),
                easing: Chartist.Svg.Easing.easeOutQuint
            }
        });
    }
});

//thin-bar-chart first
$(function() {
    Morris.Bar({
        element: 'vender-chart-first',
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
            },{
                x: "September",
                y: 65,
                z: 5
            },{
                x: "October",
                y: 25,
                z: 5
            },{
                x: "November",
                y: 35,
                z: 5
            },{
                x: "December",
                y: 50,
                z: 5
            }
        ],
        xkey: "x",
        ykeys: ["y", "z"],
        labels: ["Y", "Z"],
        barColors: ["#4099ff", "#f1f1f1"],
        stacked: !0,
        horizontal:false,
        axes: true
    });
});

//thin-bar-chart second
$(function() {
    Morris.Bar({
        element: 'vender-chart-second',
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
            },{
                x: "September",
                y: 65,
                z: 5
            },{
                x: "October",
                y: 25,
                z: 5
            },{
                x: "November",
                y: 35,
                z: 5
            },{
                x: "December",
                y: 50,
                z: 5
            }
        ],
        xkey: "x",
        ykeys: ["y", "z"],
        labels: ["Y", "Z"],
        barColors: ["#26c6da", "#f1f1f1"],
        stacked: !0,
        horizontal:false,
        axes: true
    });
});


//SAles Overview chart

var salesoverview = {
    labels: ["","10", "20", "30", "40", "50", "60", "70", "80"],
    datasets: [
        {
            fillColor: "rgba(220,220,220,0)",
            strokeColor: "#4099ff",
            pointColor: "#fff",
            data: [55, 15, 35, 20, 22, 32, 42, 40, 50]
        },
        {
            fillColor: "rgba(220,220,220,0)",
            strokeColor: "#FF5370",
            pointColor: "#fff",
            data: [25, 35, 50, 35, 25, 30, 35, 45, 55]
        },
        {
            fillColor: "rgba(220,220,220,0)",
            strokeColor: "#ab8ce4",
            pointColor: "#fff",
            data: [15, 25, 20, 30, 40, 20, 30, 35, 60]
        }]
}

Chart.defaults.global.animationSteps = 50;
Chart.defaults.global.tooltipYPadding = 15;
Chart.defaults.global.tooltipCornerRadius = 2;
Chart.defaults.global.tooltipFillColor = "#333333";
Chart.defaults.global.animationEasing = "easeOutBounce";
Chart.defaults.global.responsive = true;
Chart.defaults.global.scaleLineColor = "#eeeeee";
Chart.defaults.global.scaleFontSize = 14;

var ctx = document.getElementById("sales-overview").getContext("2d");
var LineChartDemo = new Chart(ctx).Line(salesoverview, {
    pointDotRadius: 3,
    bezierCurve: false,
    scaleShowVerticalLines: false
});

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