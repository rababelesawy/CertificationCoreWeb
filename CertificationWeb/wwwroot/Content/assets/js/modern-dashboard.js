//sales report chart
$(function() {
    Morris.Bar({
        element: 'sales-report-chart',
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
        barColors: ["#88D563", "#F5F5F5" ,"#88D563" ,"#F5F5F5" ,"#88D563", "#F5F5F5" ,"#88D563"],
        stacked: !0,
        horizontal:false,
        axes: false
    });
});


/*Mouse Speed chart*/
var mrefreshinterval = 500; // update display every 500ms
var lastmousex = -1;
var lastmousey = -1;
var lastmousetime;
var mousetravel = 0;
var mpoints = [];
var mpoints_max = 30;
$('body').mousemove(function(e) {
    var mousex = e.pageX;
    var mousey = e.pageY;
    if (lastmousex > -1)
        mousetravel += Math.max(Math.abs(mousex - lastmousex), Math.abs(mousey - lastmousey));
    lastmousex = mousex;
    lastmousey = mousey;
});
var mdraw = function() {
    var md = new Date();
    var timenow = md.getTime();
    if (lastmousetime && lastmousetime != timenow) {
        var pps = Math.round(mousetravel / (timenow - lastmousetime) * 1000);
        mpoints.push(pps);
        if (mpoints.length > mpoints_max)
            mpoints.splice(0, 1);
        mousetravel = 0;

        var mouse_wid = $('#mouse-speed-chart-sparkline').parent('.card-block').parent().width();
        var a = mpoints - mouse_wid;
        $('#mouse-speed-chart-sparkline').sparkline(mpoints, {
            width: '100%',
            height: '100%',
            tooltipClassname: 'chart-sparkline',
            lineColor: '#E4326B',
            fillColor: 'rgba(253,243,246)',
            //spotColor: '#26c6da',
            //lineColor: "#bca0ee",
            //fillColor: "#bca0ee",
            highlightLineColor: "#E4326B",
            highlightSpotColor: "#E4326B",
            //sliceColors: ["#5e6db3", "#00ca95", "#fd7b6c"],
            targetColor: "#E4326B",
            performanceColor: "#E4326B",
            boxFillColor: "#E4326B",
            medianColor: "#E4326B",
            minSpotColor: "#E4326B",
        });
    }
    lastmousetime = timenow;
    mtimer = setTimeout(mdraw, mrefreshinterval);
}
var mtimer = setTimeout(mdraw, mrefreshinterval); // We could use setInterval instead, but I prefer To Do it this way
$.sparkline_display_visible();






//-----Annual Report
var annualreportgraph = [
    {
        value: 300,
        color: "#88D563",
        highlight: "#88D563",
        label: "Selling"
    },
    {
        value: 300,
        color: "#5C37FB",
        highlight: "#5C37FB",
        label: "Marketing"
    },
    {
        value: 200,
        color: "#E4326B",
        highlight: "#E4326B",
        label: "Production"
    },
    {
        value: 300,
        color: "#A052FD",
        highlight: "#A052FD",
        label: "Holiday"
    }
];
var doughnutOptions = {
    //Boolean - Whether we should show a stroke on each segment
    segmentShowStroke: true,

    //String - The colour of each segment stroke
    segmentStrokeColor: "#fff",

    //Number - The width of each segment stroke
    segmentStrokeWidth: 2,

    //Number - The percentage of the chart that we cut out of the middle
    percentageInnerCutout: 50, // This is 0 for Pie charts

    //Number - Amount of animation steps
    animationSteps: 100,

    //String - Animation easing effect
    animationEasing: "easeOutBounce",

    //Boolean - Whether we animate the rotation of the Doughnut
    animateRotate: true,

    //Boolean - Whether we animate scaling the Doughnut from the centre
    animateScale: false,

    //String - A legend template
    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"

};

var doughnutCtx = document.getElementById("annual-report-graph").getContext("2d");
var myDoughnutChart = new Chart(doughnutCtx).Doughnut(annualreportgraph, doughnutOptions);

//-----finance Status 
var data = {
    labels: ['W1', 'W2', 'W3', 'W4', 'W5', 'W6', 'W7', 'W8', 'W9', 'W10'],
    series: [
        [1, 2, 4, 8, 6, -2, -1, -4, -6, -2]
    ]
};

var options = {
    high: 10,
    low: -10,
    axis:false
};

new Chartist.Bar('.finance-stts', data, options);



//production chart
Morris.Area({
    element: 'production',
    behaveLikeLine: true,
    data: [{
        x: '2011 Q1',
        y: 2
    },
        {
            x: '2011 Q2',
            y: 5
        },
        {
            x: '2011 Q3',
            y: 2

        },
        {
            x: '2011 Q4',
            y: 5

        },
        {
            x: '2011 Q5',
            y: 3
        },
        {
            x: '2011 Q6',
            y: 5

        },
        {
            x: '2011 Q7',
            y: 2

        }
    ],
    xkey: 'x',
    ykeys: ['y'],
    labels: ['Y'],
    lineColors: ["#a34ffe"],
    grid: false,
    axes: false
});


Morris.Line({
    element: "tech-chart",
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
    lineColors: ["#E4326B"],
    labels: ["Series A"],
    grid: false,
    axes: false

});


// Product Listing Chart
$(function() {
    Morris.Bar({
        element: 'product-listing-chart',
        data: [{
            x: "2011 Q1",
            y: 1,
            z: 1,
            a: 1
        },
            {
                x: "2011 Q2",
                y: 2,
                z: 2,
                a: 1
            },
            {
                x: "2011 Q3",
                y: 3,
                z: 2,
                a: 1
            },
            {
                x: "2011 Q4",
                y: 4,
                z: 3,
                a: 2
            },
            {
                x: "2011 Q5",
                y: 5,
                z: 3,
                a: 2
            },
            {
                x: "2011 Q6",
                y: 6,
                z: 3,
                a: 2
            },
            {
                x: "2011 Q7",
                y: 7,
                z: 4,
                a: 2
            },
            {
                x: "2011 Q8",
                y: 8,
                z: 5,
                a: 3
            },
            {
                x: "2011 Q9",
                y: 9,
                z: 6,
                a: 3
            },
            {
                x: "2011 Q10",
                y: 10,
                z: 6,
                a: 4
            },
            {
                x: "2011 Q11",
                y: 11,
                z: 7,
                a: 5
            }],
        xkey: "x",
        ykeys: ["y", "z", "a"],
        labels: ["Y", "Z", "A"],
        barColors: ["#5C37FB", "#88D563" ,"#E4326B"],
        stacked: !0,
        grid: false,
        axes: false
    });

});

/*----------------------------------------
 Customer-owlCarousel
 ----------------------------------------*/
$(document).ready(function() {
    $("#testimonial").owlCarousel({
        items :1,
        margin:30,
        loop : true,
        pagination:true,
        navigationText:true,
        nav: true
    });
});