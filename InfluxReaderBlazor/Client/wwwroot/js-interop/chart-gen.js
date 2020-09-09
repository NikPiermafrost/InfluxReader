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
            labels: values[0].Values.map(x => new Date(x.Time).toLocaleString()),
            datasets: values.map((item) => {
                return {
                    label: item.EntityName,
                    fill: false,
                    borderColor: '#' + Math.floor(Math.random() * 16777215).toString(16),
                    data: item.Values.map(x => x.Value)
                };
            })
        },

        // Configuration options go here
        options: {
            responsive: true,
            animation: {
                duration: 0,
            }
        }
    });
}
//create new dataset for graph
var newDataset = (newArray) => {
    arr = JSON.parse(newArray);
    chart.data.labels = arr[0].Values.map(x => new Date(x.Time).toLocaleString());
    chart.data.datasets = arr.map((dataSet) => {
        return {
            label: dataSet.EntityName,
            fill: false,
            borderColor: '#' + Math.floor(Math.random() * 16777215).toString(16),
            data: dataSet.Values.map(x => x.Value)
        };
    });
    chart.update();
}

//json parser from api data
var chartInitializer = (data) => {
    ctx = document.getElementById('integer-chart').getContext('2d');
    arr = JSON.parse(data);
    genChart(arr);
}

//pilot with slider
var editData = (values) => {
    var tmpArray = [];
    arr.forEach((dataSet) => {
        tmpArray.push({ EntityName: dataSet.EntityName, Values: dataSet.Values.slice(values[0], values[1] + 1) });
    });
    chart.data.labels = tmpArray[0].Values.map(x => new Date(x.Time).toLocaleString());
    chart.data.datasets.forEach((x, i) => x.data = tmpArray[i].Values.map(y => y.Value));
    chart.update();
}

//replay chart element ref
var replayCtx;
//replay chart object
var replayChart;
//replay chart array
var replayChartarray = [];

var dataForRabbit = [];

//furst initialization of the replay chart
var initReplayChart = () => {
    replayCtx = document.getElementById('replay-chart').getContext('2d');
    replayChartarray = [];
    arr.forEach((dataSet) => {
        replayChartarray.push({ EntityName: dataSet.EntityName, Values: dataSet.Values.slice(initialArrayPosition, finalArrayPosition + 1) });
    });
    var genGraph = genReplayChart(replayChartarray);
    return genGraph ? replayChartarray : [];
}

//initialization of the chart object
var genReplayChart = (values) => {
    try {
        replayChart = new Chart(replayCtx, {
            type: 'line',
            data: {
                labels: values[0].Values.map(x => new Date(x.Time).toLocaleString()),
                datasets: values.map((item) => {
                    return {
                        label: item.EntityName,
                        fill: false,
                        borderColor: '#' + Math.floor(Math.random() * 16777215).toString(16),
                        data: item.Values.map(x => x.Value)
                    };
                })
            },

            // Configuration options go here
            options: {
                responsive: true,
                animation: {
                    duration: 0,
                }
            }
        });
        return true;
    } catch (e) {
        return false;
    }
}

var counter;
var executionHandler;
var zoomLevel = 5;

var setZoomLevel = (zoom) => {
    zoomLevel = zoom;
    console.log(zoom);
}

//adds the next field for the simulation
var addDataToSimulation = (arrPosition) => {
    replayChart.data.labels.push(new Date(replayChartarray[0].Values[arrPosition].Time).toLocaleString());
    replayChartarray.forEach((dataSet, index) => {
        replayChart.data.datasets[index].data.push(dataSet.Values[arrPosition].Value);
    });
    if (arrPosition >= zoomLevel) {
        removeLastDataToSimulation();
    }
    replayChart.update();
}

//removes the first field for the simulation
var removeLastDataToSimulation = () => {
    replayChart.data.labels.shift();
    replayChart.data.datasets.forEach(x => x.data.shift());
}

//resets the graph after simulation is completed
var resetAfterSimulation = () => {
    replayChart.data.labels = replayChartarray[0].Values.map(x => new Date(x.Time).toLocaleString());
    replayChartarray.forEach((dataSet, index) => {
        replayChart.data.datasets[index].data = dataSet.Values.map(x => x.Value);
    });
    replayChart.update();
}

//empties the chart for initialization
var clearSimulationChart = () => {
    replayChart.data.labels = [];
    replayChart.data.datasets.forEach(x => x.data = []);
    replayChart.update();
    return true;
}

//double slider obj
var rangeSlider;
var initialArrayPosition;
var finalArrayPosition;
//initializes the range slider
var initSlider = (maxArrCount) => {
    rangeSlider = document.getElementById('range-slider');
    noUiSlider.create(rangeSlider, {
        range: {
            'min': 0,
            'max': maxArrCount
        },
        step: 1,
        start: [0, maxArrCount],
        connect: true,
        orientation: 'horizontal',
        direction: 'ltr',
        tooltips: false,
        behaviour: 'tap-drag'
    });
    initialArrayPosition = 0;
    finalArrayPosition = parseInt(maxArrCount);
    catchSliderEvents();
}

//updates the handle values
var updateSliderValue = (newMaxValue) => {
    rangeSlider.noUiSlider.updateOptions({
        range: {
            'min': 0,
            'max': newMaxValue
        }
    }, true);
    rangeSlider.noUiSlider.set([0, newMaxValue]);
    initialArrayPosition = 0;
    finalArrayPosition = parseInt(newMaxValue);
}

//handles the updated values
var catchSliderEvents = () => {
    rangeSlider.noUiSlider.on('update', () => {
        var sliders = rangeSlider.noUiSlider.get();
        initialArrayPosition = parseInt(sliders[0]);
        finalArrayPosition = parseInt(sliders[1]);
        editData([initialArrayPosition, finalArrayPosition]);
    });
}