getChartsData();
getCardsData();
function getCardsData() {

    $.ajax({
        url: "/UserDashboard/GetUsersCount",
        type: "POST",
        success: function (result) {
            console.log(result);
            $('.usersCount').text(result + " Total Users!");
        }
    });

    $.ajax({
        url: "/UserDashboard/GetFundsCreatedCount",
        type: "POST",
        success: function (result) {
            console.log(result);
            $('.fundsCreatedCount').text(result + " Total Fundraiser Created!");
        }
    });

    $.ajax({
        url: "/UserDashboard/GetVisitorsCount",
        type: "POST",
        success: function (result) {
            console.log(result);
            $('.visitorCount').text(result + " Total Visitors!");
        }
    });
}

function getChartsData() {
    var visitorChart = document.getElementById("visitorChart").getContext('2d');
    var fundChart = document.getElementById("fundChart").getContext('2d');
    var months = [];

    $.ajax({
        url: "/UserDashboard/GetMonths",
        type: "POST",
        success: function (result) {
            console.log(result);
            months = result;

            $.ajax({
                url: "/UserDashboard/GetVisitorsData",
                type: "POST",
                success: function (data) {
                    var myChart = new Chart(visitorChart, {
                        type: 'line',
                        data: {
                            labels: months,
                            datasets: [{
                                fill: false,
                                backgroundColor: "#00FF00",
                                borderColor: "#00FF00",
                                borderCapStyle: 'butt',
                                data: data,
                            }]
                        },
                        options: {
                            responsive: true,
                            legend: {
                                display: false,
                            },
                            hover: {
                                mode: 'label'
                            },
                            scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true,
                                        stepSize: 10
                                    }
                                }]
                            }
                        }
                    });
                }
            });

            $.ajax({
                url: "/UserDashboard/GetFundsData",
                type: "POST",
                success: function (data) {
                    var myChart = new Chart(fundChart, {
                        type: 'line',
                        data: {
                            labels: months,
                            datasets: [{
                                fill: false,
                                backgroundColor: "#00FF00",
                                borderColor: "#00FF00",
                                borderCapStyle: 'butt',
                                data: data,
                            }]
                        },
                        options: {
                            responsive: true,
                            legend: {
                                display: false,
                            },
                            hover: {
                                mode: 'label'
                            },
                            scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true,
                                        stepSize: 10
                                    }
                                }]
                            }
                        }
                    });
                }
            });

            
        }
    });

    
}