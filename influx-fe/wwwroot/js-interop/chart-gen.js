//http dataset call
var arr = [];
//ref to the chart canvas
var ctx;
//chart object
var chart;

//chartjs initializer
var genChart = (values) => {
    chart = new Chart(ctx, {
        type: 'line',
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

//create new dataset for graph
var newDataset = (newArray) => {
    arr = JSON.parse(newArray);
    chart.data.labels = arr.map(x => x.Time);
    chart.data.datasets[0].data = arr.map(x => x.Value);
    chart.update();
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

//furst initialization of the replay chart
var initReplayChart = (arrStart, arrEnd) => {
    replayCtx = document.getElementById('replay-chart').getContext('2d');
    replayChartarray = arr.slice(arrStart, arrEnd + 1);
    genReplayChart(replayChartarray);
}

//initialization of the chart object
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

//replay chart updater
var resetDataReplay = (arrStart, arrEnd) => {
    replayChartarray = arr.slice(arrStart, arrEnd + 1);
    replayChart.data.labels = replayChartarray.map(x => x.Time);
    replayChart.data.datasets[0].data = replayChartarray.map(x => x.Value);
    replayChart.update();
}

//sets the interval for data reproduction
var initializeSimulation = (ms, zoom) => {
    var counter = 0;
    clearSimulationChart();
    var execution = setInterval(() => {
        addDataToSimulation(counter);
        if (zoom < counter && zoom >= 3) {
            removeLastDataToSimulation();
        }
        replayChart.update()
        counter++;
        if (counter === replayChartarray.length) {
            clearInterval(execution);
            resetAfterSimulation();
        }
    }, ms);
}

//adds the next field for the simulation
var addDataToSimulation = (arrPosition) => {
    replayChart.data.labels.push(replayChartarray[arrPosition].Time);
    replayChart.data.datasets[0].data.push(replayChartarray[arrPosition].Value);
}

//removes the first field for the simulation
var removeLastDataToSimulation = () => {
    replayChart.data.labels.shift();
    replayChart.data.datasets[0].data.shift();
}

//resets the graph after simulation is completed
var resetAfterSimulation = () => {
    replayChart.data.labels = replayChartarray.map(x => x.Time);
    replayChart.data.datasets[0].data = replayChartarray.map(x => x.Value);
    replayChart.update();
}

//empties the chart for initialization
var clearSimulationChart = () => {
    replayChart.data.labels = [];
    replayChart.data.datasets[0].data = [];
    replayChart.update();
}

var rangeSlider;

var initSlider = (maxArrCount) => {
    rangeSlider = document.getElementById('range-slider');

    noUiSlider.create(rangeSlider, {
        range: {
            'min': 0,
            'max': maxArrCount
        },
        step: 100 / maxArrCount,
        start: [0, maxArrCount],
        connect: true,
        orientation: 'horizontal',
        direction: 'ltr',
        tooltips: false,
        behaviour: 'tap-drag',
        tooltips: true,
    });
}

var updateSliderValue = (newMaxValue) => {
    rangeSlider.noUiSlider.updateOptions({
        range = {
            'min': 0,
            'max': newMaxValue
        },
        step = 100 / newMaxValue
    }, true);
    rangeSlider.noUiSlider.set([0, newMaxValue]);
}