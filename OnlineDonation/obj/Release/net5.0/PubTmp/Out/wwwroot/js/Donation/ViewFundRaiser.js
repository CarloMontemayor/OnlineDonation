$('#qrCode').hover(function () {
    $(this).css('cursor', 'pointer');
});

$('#qrCode').click(function () {
    window.open('https://help.gcash.com/hc/en-us/articles/360017722773-How-do-I-pay-using-the-QR-');
});

$(document).ready(function () {

    function createHoverState(myobject) {
        myobject.hover(function () {
            $(this).prev().toggleClass('hilite');
        });
        myobject.mousedown(function () {
            $(this).prev().addClass('dragging');
            $("*").mouseup(function () {
                $(myobject).prev().removeClass('dragging');
            });
        });
    }

    $(".slider").slider({
        orientation: "horizontal",
        range: "min",
        max: 100,
        value: 0,
        animate: 1300
    });
    $("#blue").slider("value", 100);
    $('.slider').each(function (index) {
        $(this).slider("value", 75 - index * (50 / ($('.slider').length - 1)));
    });

    createHoverState($(".slider a.ui-slider-handle"));

});

function copyToClipboard() {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(window.location.href).select();
    document.execCommand("copy");
    $temp.remove();
    alert('Copied!');
    var itemId = $("#itemId").val();
    $.ajax({
        url: '/Share/SaveShare',
        type: 'POST',
        data: jQuery.param({ donationId: itemId }),
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (response) {
            console.log(response);
            location.reload();
        },
        error: function () {
            alert("Error while donating. Please contact admin.");
        }
    });
}

$(document).ready(function () {

    $('[data-toggle="tooltip"]').tooltip();

    $('#submit').click(function () {
        var amount = $("#amount").val();
        var reference = $("#reference").val();
        var referenceLength = $("#reference").val().length;
        var userId = $("#userId").val();
        var isCheck = $('#anonymous').is(":checked");
        var itemId = $("#itemId").val();
        if (amount == 0 || amount == null) {
            alert('Please put valid amount!');
            return;
        }
        if (reference == 0 || reference == null) {
            alert('Please put valid reference code!');
            return;
        }
        if (referenceLength != 13) {
            alert('Please put valid reference code!');
            return;
        }

        if (isCheck) {
            userId = "";
        }

        $.ajax({
            url: '/Transaction/SaveTransaction',
            type: 'POST',
            data: jQuery.param({ userId: userId, amount: amount, referenceCode: reference, itemId: itemId }),
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (response) {
                alert('Successfully Donated ' + amount + '!');
                location.reload();

            },
            error: function () {
                alert("Error while donating. Please contact admin.");
            }
        });
    });


    $.ajax({
        url: "/Transaction/GetTransactionsAmount",
        type: "POST",
        data: jQuery.param({ itemId: $("#itemId").val() }),
        success: function (result) {
            console.log(result);
            $('#totalAmount').text("₱" + result);
            $('#amountSlider').val(result);
            var goal = $('#goal').text();
            $('#amountSlider').prop('title', result + ' of ' + goal);
        }
    });

    $.ajax({
        url: "/Transaction/GetTransactionsCount",
        type: "POST",
        data: jQuery.param({ itemId: $("#itemId").val() }),
        success: function (result) {
            console.log(result);
            $('#donors').html("<b>" + result + "</b> Donors");
        }
    });

    $.ajax({
        url: "/Share/GetSharesCount",
        type: "POST",
        data: jQuery.param({ itemId: $("#itemId").val() }),
        success: function (result) {
            console.log(result);
            $('#shares').html("<b>" + result + "</b> Shares");
        }
    });

});


google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {

    $.ajax({
        url: "/UserDashboard/GetChartDonation",
        type: "POST",
        data: jQuery.param({ itemId: $("#itemId").val() }),
        success: function (obj) {
            var result = JSON.stringify(obj);
            var parse = JSON.parse(result);

            var data = google.visualization.arrayToDataTable(parse);
            // Optional; add a title and set the width and height of the chart
            var options = { 'title': 'Donations', 'width': 650, 'height': 500, is3D: true, };

            // Display the chart inside the <div> element with id="piechart"
            var donationChart = new google.visualization.PieChart(document.getElementById('donationChart'));
            donationChart.draw(data, options);
        }
    });

}