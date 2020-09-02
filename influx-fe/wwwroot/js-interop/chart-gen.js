var arr = [];
var ctx = '';
var chart;

var genChart = (values) => {
    chart = new Chart(ctx, {
        // The type of chart we want to create
        type: 'line',
        // The data for our dataset
        data: {
            labels: values.map(x => x.Time),
            datasets: [{
                label: 'My First dataset',
                backgroundColor: 'rgb(255, 99, 132)',
                borderColor: 'rgb(255, 99, 132)',
                data: values.map(x => x.Value)
            }]
        },

        // Configuration options go here
        options: {}
    });
}

var chartInitializer = (data) => {
    ctx = document.getElementById('integer-chart').getContext('2d');
    arr = JSON.parse(data);
    genChart(arr);
}

var editData = (value) => {
    var tmpArray = arr.slice(0, value + 1);
    chart.data.labels = tmpArray.map(x => x.Time);
    chart.data.datasets[0].data = tmpArray.map(x => x.Value);
    chart.update();
} 