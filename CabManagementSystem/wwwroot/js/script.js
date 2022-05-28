var now = new Date();
var time = [
    now.getHours(),
    ':',
    now.getMinutes(),
    ':',
    now.getSeconds()
].join('');

document.getElementById('arrivingTime').innerHTML = time;