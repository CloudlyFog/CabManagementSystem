var _orderTimeLeftMinutes;
var _orderTimeLeftSeconds;
var arrived = false;

function getTimes(orderTimeLeftMinutes, orderTimeLeftSeconds) {
    _orderTimeLeftMinutes = orderTimeLeftMinutes;
    _orderTimeLeftSeconds = orderTimeLeftSeconds;
}

function asyncReloadOrderTime() {
    $.ajax({
        url: "",
        context: document.body,
        success: function () {
            if (_orderTimeLeftSeconds == 1) {
                _orderTimeLeftMinutes--;
                _orderTimeLeftSeconds = 59;
            }
            if (_orderTimeLeftMinutes == -1) {
                arrived = true;
            }
            if (arrived) {
                $('#arrived').text('taxi arrived to the destination.');
                $('#arrivingTime').text();
            } else {
                $('#arrivingTime').text(_orderTimeLeftMinutes + ":" + _orderTimeLeftSeconds);
            }
            _orderTimeLeftSeconds--;
            setTimeout(asyncReloadOrderTime, 1000);
        }
    });
}