"use strict";
$(document).ready(function() {
    /*Line chart*/
    setTimeout(function () {
        $("#collection-chart").sparkline([60, 65, 60, 50, 30], {
            type: 'line',
            width: '300px',
            height: '100%',
            tooltipClassname: 'chart-sparkline',
            lineColor: '#6422d2',
            fillColor: 'rgba(100, 34, 210,0.8)',
            //spotColor: '#26c6da',
            //lineColor: "#bca0ee",
            //fillColor: "#bca0ee",
            highlightLineColor: "#6422d2",
            highlightSpotColor: "#6422d2",
            //sliceColors: ["#5e6db3", "#00ca95", "#fd7b6c"],
            targetColor: "#6422d2",
            performanceColor: "#6422d2",
            boxFillColor: "#6422d2",
            medianColor: "#6422d2",
            minSpotColor: "#6422d2",
        });

    });

});


google.charts.load('current', {packages: ['corechart', 'bar']});
google.charts.load('current', {'packages':['line']});
google.charts.load('current', {'packages':['corechart']});
google.charts.setOnLoadCallback(drawBasic);
// google.charts.setOnLoadCallback(drawVisualization);

function drawBasic() {



    //line chart
    if ($("#line-chart").length > 0) {
        var data = new google.visualization.DataTable();
        data.addColumn('number');
        data.addColumn('number');
        data.addColumn('number');

        data.addRows([
            [1,  37.8, 80.8],
            [2,  30.9, 10.5],
            [3,  40.4,   57],
            [4,  11.7, 18.8],
            [5,  20, 17.6],
            [6,   8.8, 13.6],
            [7,   7.6, 12.3],
            [8,  12.3, 29.2],
            [9,  16.9, 42.9],
            [10, 12.8, 30.9],
            [11,  5.3,  7.9],
            [12,  6.6,  8.4],

        ]);

        var options = {
            colors: ["#4ED17D", "#8F43FE"],
            height: 350,
            width:'100%',
        };

        var chart = new google.charts.Line(document.getElementById('line-chart'));

        chart.draw(data, google.charts.Line.convertOptions(options));
    }
    
}


setTimeout(function(){
    $("#line-chart-sparkline-dashboard1").sparkline([25, 50, 30, 40, 60, 21, 20, 10, 4, 13,0, 10, 30, 40, 10, 15, 20], {
        type: 'line',
        width: '100%',
        height: '50px',
        tooltipClassname: 'chart-sparkline',
        lineColor: 'rgba(49, 133, 230, 0.8)',
        fillColor: '#3185e6',
        highlightLineColor: "rgba(49, 133, 230, 0.8)",
        highlightSpotColor: "rgba(49, 133, 230, 0.8)",
        targetColor: "rgba(49, 133, 230, 0.8)",
        performanceColor: "rgba(49, 133, 230, 0.8)",
        boxFillColor: "rgba(49, 133, 230, 0.8)",
        medianColor: "rgba(49, 133, 230, 0.8)",
        minSpotColor: "rgba(49, 133, 230, 0.8)",
    });

    $("#line-chart-sparkline-dashboard2").sparkline([5, 10, 20, 14, 17, 21, 20, 10, 4, 13,0, 10, 30, 40, 10, 15, 20], {
        type: 'line',
        width: '100%',
        height: '50px',
        tooltipClassname: 'chart-sparkline',
        lineColor: 'rgba(243, 20, 100, 0.8)',
        fillColor: '#f31464',
        highlightLineColor: "#ff5a75",
        highlightSpotColor: "#ff5a75",
        targetColor: "#ff5a75",
        performanceColor: "#ff5a75",
        boxFillColor: "#ff5a75",
        medianColor: "#ff5a75",
        minSpotColor: "#ff5a75"
    });

    $("#line-chart-sparkline-dashboard3").sparkline([5, 10, 20, 14, 17, 21, 20, 10, 4, 13,0, 10, 30, 40, 10, 15, 20], {
        type: 'line',
        width: '100%',
        height: '50px',
        tooltipClassname: 'chart-sparkline',
        lineColor: 'rgba(78, 209, 125, 0.8)',
        fillColor: '#4ed17d',
        highlightLineColor: "rgba(78, 209, 125, 0.8)",
        highlightSpotColor: "rgba(78, 209, 125, 0.8)",
        targetColor: "rgba(78, 209, 125, 0.8)",
        performanceColor: "rgba(78, 209, 125, 0.8)",
        boxFillColor: "rgba(78, 209, 125, 0.8)",
        medianColor: "rgba(78, 209, 125, 0.8)",
        minSpotColor: "rgba(78, 209, 125, 0.8)"
    });

    $("#line-chart-sparkline-dashboard4").sparkline([5, 10, 20, 14, 17, 21, 20, 10, 4, 13,0, 10, 30, 40, 10, 15, 20], {
        type: 'line',
        width: '100%',
        height: '50px',
        tooltipClassname: 'chart-sparkline',
        lineColor: 'rgba(100, 34, 210, 0.8)',
        fillColor: '#6422d2',
        highlightLineColor: "rgba(100, 34, 210, 0.8)",
        highlightSpotColor: "rgba(100, 34, 210, 0.8)",
        targetColor: "rgba(100, 34, 210, 0.8)",
        performanceColor: "rgba(100, 34, 210, 0.8)",
        boxFillColor: "rgba(100, 34, 210, 0.8)",
        medianColor: "rgba(100, 34, 210, 0.8)",
        minSpotColor: "rgba(100, 34, 210, 0.8)"
    });
    //employee salary chart
    $("#employee-salary-chart").sparkline([25, 30, 14, 17, 21, 25, 35, 20, 30, 22, 26, 23, 21, 18, 20, 25], {
        type: 'line',
        width: '100%',
        height: '100%',
        tooltipClassname: 'chart-sparkline',
        lineColor: '#4ED17D',
        fillColor: '#38dc8d',
        highlightLineColor: "#4ED17D",
        highlightSpotColor: "#fff",
        targetColor: "#fff",
        performanceColor: "#4ED17D",
        boxFillColor: "#4ED17D",
        medianColor: "#4ED17D",
        minSpotColor: "#fff"
    });
});

//employee activity chart
$(function() {
    Morris.Donut({
        element: 'employee-act-chart',
        data: [{
            value: 70,
            label: "foo"
        },
            {
                value: 15,
                label: "bar"
            },
            {
                value: 10,
                label: "baz"
            },
            {
                value: 5,
                label: "A really really long label"
            }],
        backgroundColor: "#ccc",
        labelColor: "#060",
        colors: ["#6228d3", "#f31464" ,"#24d27e" ,"#3185e6" ,"#59a6fe", "#38d3e7" ,"#bca0ee"],
        formatter: function(a) {
            return a + "%"
        }
    });
});

// updating morris chart
$(function() {
    var e = 0,
        f = function(a) {
            for (var b = [], c = 0; c <= 360; c += 10) {
                var d = (a + c) % 360;
                b.push({
                    x: c,
                    y: Math.sin(Math.PI * d / 180).toFixed(4),
                    z: Math.cos(Math.PI * d / 180).toFixed(4)
                })
            }
            return b
        },
        g = Morris.Line({
            element:'updating-data-morris-chart',
            data: f(0),
            xkey: "x",
            ykeys: ["y", "z"],
            labels: ["sin()", "cos()"],
            parseTime: !1,
            ymin: -1,
            ymax: 1,
            hideHover: !0,
            lineColors: ["#4ED17D", "#F31464"],
        }),
        h = function() {
            e++, g.setData(f(5 * e)), $(".reloadStatus").text(e + " reloads")
        };
    setInterval(h, 100)
});

//Morris js for area chart
Morris.Area({
    element: 'east-zone-graph',
    behaveLikeLine: true,
    data: [{
        x: '2011 Q1',
        y: 3
    },
        {
            x: '2011 Q2',
            y: 2
        },
        {
            x: '2011 Q3',
            y: 2

        },
        {
            x: '2011 Q4',
            y: 3

        }
    ],
    xkey: 'x',
    ykeys: ['y'],
    labels: ['Y'],
    lineColors: ["#6422d2"],
    grid: false,
    axes: false
});
$("#west-zone-graph").sparkline([5, 8, 10, 2, 4, 9, 5, 7, 5, 9, 6, 4, 8, 10, 7, 5, 6], {
    type: 'bar',
    barWidth: '15px',
    height: '100%',
    tooltipClassname: 'chart-sparkline',
    barColor: '#4ED17D'
});

setTimeout(function(){
    $("#simple-line-chart-sparkline").sparkline([5, 10, 20, 14, 17, 21, 20, 10, 4, 13,0, 10, 30, 40, 10, 15, 20], {
        type: 'line',
        width: '100%',
        height: '100%',
        tooltipClassname: 'chart-sparkline',
        lineColor: '#F31464',
        fillColor: 'transparent',
        //spotColor: '#26c6da',
        //lineColor: "#bca0ee",
        //fillColor: "#bca0ee",
        highlightLineColor: "#F31464",
        highlightSpotColor: "#F31464",
        //sliceColors: ["#5e6db3", "#00ca95", "#fd7b6c"],
        targetColor: "#F31464",
        performanceColor: "#F31464",
        boxFillColor: "#F31464",
        medianColor: "#F31464",
        minSpotColor: "#F31464",
    });
});

Morris.Line({
    element: "south-zone-chart",
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
    lineColors: ["#3185e6"],
    labels: ["Series A"],
    grid: false,
    axes: false

});

// Responsive tabs js
$(document).ready(function() {

    $("#solitary-scrollpy .nav-pills li a").click(function() {
        var tab_val = $(this).text();
        $('#mob_tab_cls span').text(tab_val);
    });


    $("#mob_tab_cls").click(function(){
        $(this).toggleClass('active');
        if($(this).find('i').text() == '+')
        {
            $(this).find('i').text('-');
        }
        else
        {
            $(this).find('i').text('+');
        }
        $(this).next('ul.nav-pills').slideToggle();
        $('.nav-item-link').toggleClass('active');
    });

});

<!-- responsive sidebar auto hide script End-->