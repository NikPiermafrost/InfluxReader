//http dataset call
var arr = [];
//ref to the chart canvas
var ctx;
//chart object
var chart;

//chartjs initializer
var genChart = (values) => {
    chart = new Chart(ctx, {
        // The type of chart we want to create
        type: 'line',
        // The data for our dataset
        data: {
            labels: values.map(x => x.Time),
            datasets: [{
                label: 'Dataset',
                backgroundColor: 'rgb(255, 99, 132)',
                borderColor: 'rgb(255, 99, 132)',
                data: values.map(x => x.Value)
            }]
        },

        // Configuration options go here
        options: {}
    });
}

//json parser from api data
var chartInitializer = (data) => {
    ctx = document.getElementById('integer-chart').getContext('2d');
    arr = JSON.parse(data);
    genChart(arr);
}

//pilot with slider
var editData = (value) => {
    var tmpArray = arr.slice(0, value + 1);
    chart.data.labels = tmpArray.map(x => x.Time);
    chart.data.datasets[0].data = tmpArray.map(x => x.Value);
    chart.update();
}

//replay chart element ref
var replayCtx;
//replay chart object
var replayChart;
//replay chart array
var replayChartarray;

var initReplayChart = (arrStart, arrEnd) => {
    replayCtx = document.getElementById('replay-chart').getContext('2d');
    replayChartarray = arr.splice(arrStart, arrEnd + 1);
    genReplayChart(replayChartarray);
}

var genReplayChart = (values) => {
    replayChart = new Chart(replayCtx, {
        // The type of chart we want to create
        type: 'line',
        // The data for our dataset
        data: {
            labels: values.map(x => x.Time),
            datasets: [{
                label: 'Dataset',
                backgroundColor: 'rgb(99, 255, 132)',
                borderColor: 'rgb(99, 255, 132)',
                data: values.map(x => x.Value)
            }]
        },

        // Configuration options go here
        options: {}
    });
}