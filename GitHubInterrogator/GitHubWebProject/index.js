var uri = 'api/issues';
var setup = uri + '/setup';

function find() {
    var id = $('#prodId').val();
    $.ajax({
        type: 'GET',
        url: uri + '/url',
        async: true,
        data: {
            repoUrl: id
        },
        success: function (data) {
            var parsedData = parseData(data.Item1);
            drawChart(parsedData, data.Item2);
        },
        fail: function (xhr) {
            $('#product').text('Error: ');
        }
    });
}

function parseData(data) {
    var arr = [];
    for (var i in data) {
        arr.push(
            {
                x: new Date(data[i].Key),
                y: data[i].Value
            }
        );
    }
    return arr;
}

function drawChart(newData, repoName) {
    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Issues over time for " + repoName
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "line",
            dataPoints: newData
        }]
    });
    chart.render();

}