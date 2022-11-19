var arrPortNames = [];
var arrVesselsCount = [];
$(document).ready(() => {
    $("#showCharts").on("click", function () {
        $.ajax({
            url: "/Port/TopTenVisitedPortsHighCharts",
            type: "GET",
            success: function (data) {            
                            
                
                for (var i = 0; i < data.length; i++) {
                    arrPortNames.push(data[i].portName);
                    arrVesselsCount.push(data[i].totalVesselsVisited);
                    console.log(arrPortNames);
                }
                //--------------------HIGHCHARTS
                const chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'column',
                        options3d: {
                            enabled: true,
                            alpha: 26,
                            beta: 2,
                            depth: 45,
                            viewDistance: 25
                        }
                    },
                    xAxis: {
                        categories: arrPortNames
                    },
                    yAxis: {
                        title: {
                            enabled: false
                        }
                    },
                    tooltip: {
                        headerFormat: '<b>{point.key}</b><br>',
                        pointFormat: 'Vessels visited: {point.y}'
                    },
                    title: {
                        text: 'Top 10 Most visited porsts by Vessels'
                    },
                    subtitle: {
                        text: 'Overview:The higher column measures ports conjestion' 
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        column: {
                            depth: 25
                        }
                    },
                    series: [{
                        data: arrVesselsCount,
                        colorByPoint: true
                    }]
                });

                function showValues() {
                    document.getElementById('alpha-value').innerHTML = chart.options.chart.options3d.alpha;
                    document.getElementById('beta-value').innerHTML = chart.options.chart.options3d.beta;
                    document.getElementById('depth-value').innerHTML = chart.options.chart.options3d.depth;
                }
                
                // Activate the sliders
                document.querySelectorAll('#sliders input').forEach(input => input.addEventListener('input', e => {
                    chart.options.chart.options3d[e.target.id] = parseFloat(e.target.value);
                    showValues();
                    chart.redraw(false);
                }));

                showValues();
                //--------------------HIGHCHARTS
                arrPortNames = [];
                arrVesselsCount = [];
            },
            error: function (er) {

            }
        });

    });

});


