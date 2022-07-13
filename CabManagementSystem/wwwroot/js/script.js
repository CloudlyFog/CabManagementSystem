var _orderTimeLeftMinutes;
var _orderTimeLeftSeconds;
var _orderTimeModel;
var arrived = _orderTimeLeftMinutes <= 0 && _orderTimeLeftSeconds <= 0;

function setTimes(orderTimeLeftMinutes, orderTimeLeftSeconds) {
    _orderTimeLeftMinutes = orderTimeLeftMinutes;
    _orderTimeLeftSeconds = orderTimeLeftSeconds;
}


function asyncReloadOrderTime() {
    if (_orderTimeLeftSeconds <= 0) {
        _orderTimeLeftMinutes--;
        _orderTimeLeftSeconds = 60;
    }
    arrived ? $('#arrived').text('taxi arrived to the destination.') + $('#arrivingTime').text() : $('#arrivingTime').text(_orderTimeLeftMinutes + " mins : " + _orderTimeLeftSeconds + " secodns");
    _orderTimeLeftSeconds--;
    setTimes(_orderTimeLeftMinutes, _orderTimeLeftSeconds);
    setTimeout(asyncReloadOrderTime, 1000);
}
